using Microsoft.Extensions.Options;
using Launches.Models;

namespace Launches.Services;

public interface ILaunchRepository
{
    /// <summary>
    /// Read-only list of Launch data
    /// </summary>
    public IEnumerable<Launch> Items { get; }

    /// <summary>
    /// Get the time remaining until the next update.
    /// </summary>
    /// <returns>Time between updates</returns>
    public TimeSpan GetNextUpdate();
}

internal sealed class LaunchService(
    IOptions<LaunchOptions> options,
    ISpaceFlightNow parser,
    ILogger<LaunchService> logger
) : BackgroundService, ILaunchRepository
{
    // Last time the launch data was updated.
    private DateTime _lastUpdate = DateTime.Now;

    // Update interval
    private readonly TimeSpan _updateInterval = options.Value.UpdateInterval;

    /// <summary>
    /// List of launch items
    /// </summary>
    public IEnumerable<Launch> Items { get; private set; } = [];

    /// <summary>
    /// Get the time until the next update.
    /// </summary>
    public TimeSpan GetNextUpdate() =>
        _lastUpdate.Add(_updateInterval) - DateTime.Now;

    /// <summary>
    /// Every 4 hours, test to see if the launch schedule has changed.
    /// </summary>
    /// <param name="stoppingToken"></param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_updateInterval);
        while (!stoppingToken.IsCancellationRequested)
        {
            _lastUpdate = DateTime.Now;
            try
            {
                logger.LogInformation("Loading launches");
                Items = parser
                    .GetLaunches()
                    .OnTheSpaceCoast()
                    .ToList();
            }
            catch (Exception e)
            {
                // Trap error, and keep previous launches.
                logger.LogError(e, "Failed to load launches");
            }

            if (!stoppingToken.IsCancellationRequested)
            {
                await timer.WaitForNextTickAsync(stoppingToken);
            }
        }
    }
}