using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.HighLevel.Client.Abstract;

/// <summary>
/// Provides cached HighLevel HTTP clients with optional per-API-key authentication.
/// </summary>
public interface IHighLevelHttpClient: IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the shared unauthenticated HighLevel transport.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a client configured for a specific HighLevel API key.
    /// </summary>
    /// <param name="apiKey">The HighLevel API key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the configured client.</returns>
    ValueTask<HttpClient> Get(string apiKey, CancellationToken cancellationToken = default);
}
