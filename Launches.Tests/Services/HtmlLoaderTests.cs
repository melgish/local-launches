using Launches.Services;

namespace Launches.Tests.Services;

// HtmlAgilityPack is a third-party library that helps parse HTML documents.
// Unfortunately it's not easy to mock. Rather than setting up a test that
// requires an HTML endpoint, or worse hitting some public website,
// this test passes an invalid URL and checks for failure.

public class HtmlLoaderTests
{
    [Fact]
    public void HtmlLoader_WhenURLIsNotValid_ShouldThrow()
    {
        // Arrange
        var loader = new HtmlLoader();

        // Assert
        Assert.Throws<ArgumentNullException>(() => loader.Load(null!));
    }
}


