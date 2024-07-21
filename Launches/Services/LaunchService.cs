namespace Launches.Services;

internal interface ILaunchRepository
{
    /// <summary>
    /// Read-only list of Launch data
    /// </summary>
    public IEnumerable<Launch> Items { get; }

    public TimeSpan GetNextUpdate();
}

internal sealed partial class LaunchService(
    ILogger<LaunchService> logger
) : BackgroundService, ILaunchRepository
{
    // Last time the launch data was updated.
    private DateTime _lastUpdate = DateTime.Now;
    // Update interval
    private readonly TimeSpan _updateInterval = TimeSpan.FromHours(4);

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
                Items = SpaceFlightNow.GetSpaceCoastLaunches();
            }
            catch (Exception e)
            {
                // Trap error, and keep previous launches.
                logger.LogError(e, "Failed to load launches");
            }
            await timer.WaitForNextTickAsync(stoppingToken);
        }
    }
}