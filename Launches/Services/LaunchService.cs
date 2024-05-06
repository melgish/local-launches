using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Launches.Services;

internal sealed class LaunchService : BackgroundService, ILaunchRepository
{
    private readonly ILogger<LaunchService> _logger;
    private readonly PeriodicTimer _timer;
    public IEnumerable<Launch> Items { get; private set; } = Enumerable.Empty<Launch>();

    public LaunchService(ILogger<LaunchService> logger)
    {
        _logger = logger;
        _timer = new PeriodicTimer(TimeSpan.FromHours(4));
    }

    public override void Dispose()
    {
        _timer.Dispose();
    }

    /// <summary>
    /// Test if launch-site
    /// </summary>
    /// <param name="launch"></param>
    /// <returns></returns>
    private bool SiteIsLocal(Launch launch) {
        return Regex.IsMatch(launch.Site, "(canaveral|kennedy|patrick)", RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Scrape spaceflightnow.com/launch-schedule/ page
    /// </summary>
    private void LoadLaunches() {
        try
        {
            // <div class="datename">
            //    <div class="launchdate">{Date}</div>
            //    <div class="mission">{Mission}</div>
            // </div>
            // <div class="missiondata">
            //    <span>Launch time:</span>
            //    {Time}
            //    <br>
            //    <span>Launch site:</span>
            //    {Site}
            // </div>
            // <div class="missdescrip">{Description}</div>
            var web = new HtmlWeb();
            var doc = web.Load("https://spaceflightnow.com/launch-schedule/");

            var items = doc.DocumentNode
              .SelectNodes("//div[contains(@class, 'datename')]")
              .Select(node =>
              {
                  var date = node.SelectSingleNode("*[contains(@class, 'launchdate')]");
                  var mission = node.SelectSingleNode("*[contains(@class, 'mission')]");
                  var desc = node.SelectSingleNode("following-sibling::div[contains(@class,'missdescrip')]");
                  var data = node.SelectSingleNode("following-sibling::div[contains(@class, 'missiondata')]");
                  var time = data?.SelectSingleNode("span[contains(text(), 'Launch time:')]/following-sibling::text()[1]");
                  var site = data?.SelectSingleNode("span[contains(text(), 'Launch site:')]/following-sibling::text()[1]");

                  return new Launch
                  {
                      Date = date?.InnerText.Trim() ?? "",
                      Description = desc?.InnerText.Trim() ?? "",
                      Mission = mission?.InnerText.Trim() ?? "",
                      Time = time?.InnerText.Trim() ?? "",
                      Site = site?.InnerText.Trim() ?? "",
                  };
              })
              // Only interested in local launches.
              .Where(SiteIsLocal)
              .ToList();

            Items = items;
        }
        catch (Exception e) {
            _logger.LogError(e, "Failed to load launches");
        }
    }

    /// <summary>
    /// Execute the background work.
    /// </summary>
    /// <param name="stoppingToken"></param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            LoadLaunches();
            await _timer.WaitForNextTickAsync(stoppingToken);
        }
    }
}