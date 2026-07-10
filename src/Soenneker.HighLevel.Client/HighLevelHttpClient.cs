using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Hashing.XxHash;
using Soenneker.HighLevel.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.HighLevel.Client;

///<inheritdoc cref="IHighLevelHttpClient"/>
public sealed class HighLevelHttpClient : IHighLevelHttpClient
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly string _version;
    private readonly ConcurrentDictionary<string, byte> _clientIds = new();

    private static readonly Uri _prodBaseUrl = new("https://services.leadconnectorhq.com/", UriKind.Absolute);

    public HighLevelHttpClient(IHttpClientCache httpClientCache, IConfiguration config)
    {
        _httpClientCache = httpClientCache;
        _version = config.GetValue<string>("HighLevel:Version") ?? "2021-07-28";
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        // No closure: state passed explicitly + static lambda
        return _httpClientCache.Get(nameof(HighLevelHttpClient), (prodBaseUrl: _prodBaseUrl, version: _version), static state => new HttpClientOptions
        {
            BaseAddress = state.prodBaseUrl,
            DefaultRequestHeaders = new Dictionary<string, string>
            {
                {"Version", state.version}
            }
        }, cancellationToken);
    }

    public ValueTask<HttpClient> Get(string apiKey, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        string clientId = $"{nameof(HighLevelHttpClient)}:{XxHash3Util.Hash(apiKey)}";
        _clientIds.TryAdd(clientId, 0);

        return _httpClientCache.Get(clientId, (apiKey, version: _version), static state => new HttpClientOptions
        {
            BaseAddress = _prodBaseUrl,
            DefaultRequestHeaders = new Dictionary<string, string>
            {
                {"Authorization", $"Bearer {state.apiKey}"},
                {"Version", state.version}
            }
        }, cancellationToken);
    }

    /// <summary>
    /// Releases resources used by the current instance.
    /// </summary>
    public void Dispose()
    {
        _httpClientCache.RemoveSync(nameof(HighLevelHttpClient));

        foreach (string clientId in _clientIds.Keys)
        {
            _httpClientCache.RemoveSync(clientId);
        }
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask DisposeAsync()
    {
        await _httpClientCache.Remove(nameof(HighLevelHttpClient));

        foreach (string clientId in _clientIds.Keys)
        {
            await _httpClientCache.Remove(clientId);
        }
    }
}
