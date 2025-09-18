using Soenneker.HighLevel.Client.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.HighLevel.Client.Tests;

[Collection("Collection")]
public sealed class HighLevelHttpClientTests : FixturedUnitTest
{
    private readonly IHighLevelHttpClient _httpclient;

    public HighLevelHttpClientTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _httpclient = Resolve<IHighLevelHttpClient>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
