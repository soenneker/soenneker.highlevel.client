using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.HighLevel.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.HighLevel.Client.Registrars;

/// <summary>
/// A .NET thread-safe singleton HttpClient for GitHub
/// </summary>
public static class HighLevelHttpClientRegistrar
{
    /// <summary>
    /// Adds <see cref="HighLevelHttpClient"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddHighLevelHttpClientAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
            .TryAddSingleton<IHighLevelHttpClient, HighLevelHttpClient>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="HighLevelHttpClient"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddHighLevelHttpClientAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
            .TryAddScoped<IHighLevelHttpClient, HighLevelHttpClient>();

        return services;
    }
}