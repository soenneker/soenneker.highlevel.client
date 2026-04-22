using Soenneker.HighLevel.Client.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.HighLevel.Client.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class HighLevelHttpClientTests : HostedUnitTest
{
    private readonly IHighLevelHttpClient _httpclient;

    public HighLevelHttpClientTests(Host host) : base(host)
    {
        _httpclient = Resolve<IHighLevelHttpClient>(true);
    }

    [Test]
    public void Default()
    {

    }
}
