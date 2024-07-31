using Launches.Services;
using Launches.Models;
using Microsoft.Extensions.Logging.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace Launches.Tests.Services;

// Mock the parser to return a known set of data.
class MockParser : CancellationTokenSource, ISpaceFlightNow
{
    public TimeSpan CancelDelay { get; set; } = TimeSpan.Zero;

    // Mock data to return.
    public List<Launch> Launches { get; set; } = TestData.Launches;

    public IEnumerable<Launch> GetLaunches()
    {
        // Make sure the loop terminates.
        if (CancelDelay == TimeSpan.Zero)
        {
            Cancel();
        }
        else
        {
            CancelAfter(CancelDelay);
        }

        return Launches;
    }
}


public class LaunchServiceTests
{
    private readonly FakeLogger<LaunchService> logger = new();
    private readonly MockParser parser = new ();
    private readonly IOptions<LaunchOptions> options =
        Options.Create(new LaunchOptions { UpdateInterval = TimeSpan.FromHours(1) });

    // Invoke the protected method on the service.
    private async Task ExecuteAsync(BackgroundService service)
    {
        try
        {
            var method = service
                        .GetType()
                        .GetMethod("ExecuteAsync", BindingFlags.NonPublic | BindingFlags.Instance)!;

            await (Task)method.Invoke(service, [parser.Token])!;
        }
        catch(OperationCanceledException)
        {
            // These are expected when the token is cancelled.
        }
    }


    [Fact]
    public async Task ExecuteAsync_WhenRunning_ShouldRequestLaunchesAsync()
    {
        // Arrange
        var service = new LaunchService(options, parser, logger);

        // Act
        await ExecuteAsync(service);

        // Assert
        Assert.Single(service.Items);
        Assert.Equal(parser.Launches.First(), service.Items.First());
    }


    [Fact]
    public async Task ExecuteAsync_WhenShutDown_ShouldNotRequestLaunchesAsync()
    {
        // Arrange
        var service = new LaunchService(options, parser, logger);
        // Force an NPE
        parser.Launches = null!;

        // Act
        await ExecuteAsync(service);

        // Assert
        Assert.Empty(service.Items);
    }

    [Fact]
    public async Task ExecuteAsync_WhenShuttingDown_ShouldNotRequestLaunchesAsync()
    {
        parser.CancelDelay = TimeSpan.Zero;

        // Arrange
        var service = new LaunchService(options, parser, logger);
        parser.Launches = [];
        parser.CancelDelay = TimeSpan.FromMilliseconds(100);

        // Act
        await ExecuteAsync(service);

        // Assert
        Assert.Empty(service.Items);
    }

    [Fact]
    public async Task ExecuteAsync_OnError_ShouldContinueAsync()
    {
        // Arrange
        var service = new LaunchService(options, parser, logger);
        // Force an NPE
        parser.Launches = null!;

        // Act
        await ExecuteAsync(service);

        // Assert
        Assert.Matches("Failed to load", logger.LatestRecord.Message);
    }

    [Fact]
    public void GetNextUpdate_ShouldReturnTimeUntilUpdate()
    {
        // Arrange
        var service = new LaunchService(options, parser, logger);

        // Act
        var result = service.GetNextUpdate();

        // Assert
        // Test that nearly all of the time is left.
        Assert.InRange(result, TimeSpan.FromMinutes(59), TimeSpan.FromHours(1));
    }

}
