using System.Text.RegularExpressions;
using HtmlAgilityPack;

internal sealed record Launch(
    string Date,
    string Description,
    string Mission,
    string Site,
    string Time
);

// Site HTML looks like this:
//
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
//

internal static partial class SpaceFlightNow
{
    [GeneratedRegex("(canaveral|kennedy|patrick)", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex SiteRegex();

    private const string SiteURL = "https://spaceflightnow.com/launch-schedule/";
    private const string LaunchXPath = "//div[contains(@class, 'datename')]";
    private const string DataXPath = "following-sibling::div[contains(@class, 'missiondata')]";
    private const string DateXPath = "*[contains(@class, 'launchdate')]";
    private const string DescriptionXPath = "following-sibling::div[contains(@class,'missdescrip')]";
    private const string MissionXPath = "*[contains(@class, 'mission')]";
    private const string SiteXPath = "span[contains(text(), 'Launch site:')]/following-sibling::text()[1]";
    private const string TimeXPath = "span[contains(text(), 'Launch time:')]/following-sibling::text()[1]";

    /// <summary>
    /// Select a child node of the supplied node, and extract trimmed inner text.
    /// </summary>
    private static string SelectAndTrim(this HtmlNode? node, string xpath) =>
        node?.SelectSingleNode(xpath)?.InnerText.Trim() ?? string.Empty;

    /// <summary>
    /// Create a Launch object from an HTML node.
    /// </summary>
    private static Launch CreateFromHtmlNode(HtmlNode node)
    {
        var data = node.SelectSingleNode(DataXPath);
        return new Launch(
          node.SelectAndTrim(DateXPath),
          node.SelectAndTrim(DescriptionXPath),
          node.SelectAndTrim(MissionXPath),
          data.SelectAndTrim(SiteXPath),
          data.SelectAndTrim(TimeXPath)
        );
    }

    /// <summary>
    /// Test if launch site matches one of the common local names for the
    /// Space Coast.
    /// </summary>
    /// <param name="launch"></param>
    /// <returns></returns>
    private static bool SiteIsSpaceCoast(Launch launch) =>
        SiteRegex().IsMatch(launch.Site);

    /// <summary>
    /// Load and parse HTML Page
    /// </summary>
    public static List<Launch> GetSpaceCoastLaunches() =>
        new HtmlWeb()
            .Load(SiteURL)
            .DocumentNode
            .SelectNodes(LaunchXPath)
            .Select(CreateFromHtmlNode)
            .Where(SiteIsSpaceCoast)
            .ToList();
}