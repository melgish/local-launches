using Launches.Services;

namespace Launches.Tests.Services;
public class SpaceFlightNowTests
{
    private readonly MockHtmlLoader loader;
    private readonly ISpaceFlightNow parser;


    public SpaceFlightNowTests()
    {
        loader = new MockHtmlLoader();
        parser = new SpaceFlightNow(loader);
    }

    [Fact]
    public void GetAllLaunches_ReturnsAllLaunches()
    {
        var result = parser.GetLaunches().ToList();

        Assert.Equal(2, result.Count());

        var first = result.First();
        Assert.Equal("1999-03-19", first.Date);
        Assert.Equal("FarScape I", first.Mission);
        Assert.Equal("Measure solar activity", first.Description);
        Assert.Equal("Kennedy Space Center", first.Site);
        Assert.Equal("8:00 EST", first.Time);

        var last = result.Last();
        Assert.Equal("1967-10-16", last.Date);
        Assert.Equal("Jupiter 2", last.Mission);
        Assert.Empty(last.Description);
        Assert.Empty(last.Site);
        Assert.Empty(last.Time);
    }
}
