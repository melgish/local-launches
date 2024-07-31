// spell-checker: ignore Mahia, Vandenberg
using Launches.Models;

namespace Launches.Tests.Services;

public sealed class LaunchFiltersTests
{
    private Launch Create(string site) => TestData.Launches[0] with { Site = site };

    [Fact]
    public void OnTheSpaceCoast_ShouldFilterNonMatchingValues()
    {
        var all = new[] {
            Create("SLC-40, Cape Canaveral Space Force Station, Florida"),
            Create("LC-39A Kennedy Space Center"),
            Create("Patrick Air Force Base"),
            Create("Patrick Space Force Base"),
            Create(""),
            Create("Mahia Peninsula, New Zealand"),
            Create("Vandenberg Space Force Base"),
        };

        var filtered = all.OnTheSpaceCoast().ToArray();

        Assert.Equal(4, filtered.Count());
        Assert.Equal(all[0..4], filtered);
    }
}