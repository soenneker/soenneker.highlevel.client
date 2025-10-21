using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.HighLevel.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.HighLevel.Client;

///<inheritdoc cref="IHighLevelHttpClient"/>
public sealed class HighLevelHttpClient : IHighLevelHttpClient
{
    private readonly IHttpClientCache _httpClientCache;

    private const string _prodBaseUrl = "https://services.leadconnectorhq.com/";

    public HighLevelHttpClient(IHttpClientCache httpClientCache)
    {
        _httpClientCache = httpClientCache;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        return _httpClientCache.Get(nameof(HighLevelHttpClient), () => {
            var options = new HttpClientOptions
            {
                BaseAddress = _prodBaseUrl
            };
            return options;
        }, cancellationToken: cancellationToken);
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
