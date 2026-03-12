using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.HighLevel.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.HighLevel.Client;

///<inheritdoc cref="IHighLevelHttpClient"/>
public sealed class HighLevelHttpClient : IHighLevelHttpClient
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly string _version;

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

    public void Dispose()
    {
        _httpClientCache.RemoveSync(nameof(HighLevelHttpClient));
    }

    public ValueTask DisposeAsync()
    {
        return _httpClientCache.Remove(nameof(HighLevelHttpClient));
    }
}
