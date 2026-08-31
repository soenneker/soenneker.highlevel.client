[![](https://img.shields.io/nuget/v/soenneker.highlevel.client.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.highlevel.client/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.highlevel.client/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.highlevel.client/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.highlevel.client.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.highlevel.client/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.highlevel.client/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.highlevel.client/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.HighLevel.Client

Reuse HighLevel HTTP transports with the required API version header and optional per-account bearer authentication.

## Installation

```bash
dotnet add package Soenneker.HighLevel.Client
```

## Configuration

The client targets `https://services.leadconnectorhq.com/` and sends HighLevel API version `2021-07-28` by default. Override only the version when needed:

```json
{
  "HighLevel": {
    "Version": "2021-07-28"
  }
}
```

API keys are passed to `Get`; they are not read from configuration.

## Registration

```csharp
services.AddHighLevelHttpClientAsSingleton();
```

Use `AddHighLevelHttpClientAsScoped()` only when each scope should own its own client set. Each provider instance uses isolated cache keys, and disposing it removes only the clients that provider created.

## Authenticated clients

```csharp
HttpClient tenantClient = await highLevelHttpClient.Get(
    tenantApiKey,
    cancellationToken);

HttpResponseMessage response = await tenantClient.GetAsync(
    "contacts/",
    cancellationToken);
response.EnsureSuccessStatusCode();
```

Each distinct API key receives a separately cached client with `Authorization: Bearer <api-key>`. Repeated calls with the same key on the same provider reuse that client.

## Unauthenticated transport

```csharp
HttpClient transport = await highLevelHttpClient.Get(cancellationToken);
```

The parameterless overload returns a cached client with the base address and `Version` header but no authorization header. It is intended for higher-level components that add authentication themselves.

The provider owns every returned client. Let the service container dispose the provider; do not dispose individual cached clients.
