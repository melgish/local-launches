using HtmlAgilityPack;
using Launches.Models;

namespace Launches.Services;

public interface ISpaceFlightNow
{
    /// <summary>
    /// Fetch and filter launches from SpaceFlightNow.
    /// </summary>
    /// <returns></returns>
    IEnumerable<Launch> GetLaunches();
}

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

public partial class SpaceFlightNow(IHtmlLoader htmlLoader) : ISpaceFlightNow
{
    internal const string SiteURL = "https://spaceflightnow.com/launch-schedule/";
    internal const string LaunchXPath = "//div[contains(@class, 'datename')]";
    internal const string DataXPath = "following-sibling::div[contains(@class, 'missiondata')]";
    internal const string DateXPath = "*[contains(@class, 'launchdate')]";
    internal const string DescriptionXPath = "following-sibling::div[contains(@class,'missdescrip')]";
    internal const string MissionXPath = "*[contains(@class, 'mission')]";
    internal const string SiteXPath = "span[contains(text(), 'Launch site:')]/following-sibling::text()[1]";
    internal const string TimeXPath = "span[contains(text(), 'Launch time:')]/following-sibling::text()[1]";

    /// <summary>
    /// Select a child node of the supplied node, and extract trimmed inner text.
    /// </summary>
    internal static string SelectAndTrim(HtmlNode? node, string xpath) =>
        node?.SelectSingleNode(xpath)?.InnerText.Trim() ?? string.Empty;

    /// <summary>
    /// Create a Launch object from an HTML node.
    /// </summary>
    internal static Launch CreateFromNode(HtmlNode node)
    {
        var data = node.SelectSingleNode(DataXPath);
        return new Launch(
          SelectAndTrim(node, DateXPath),
          SelectAndTrim(node, DescriptionXPath),
          SelectAndTrim(node, MissionXPath),
          SelectAndTrim(data, SiteXPath),
          SelectAndTrim(data, TimeXPath)
        );
    }

    /// <summary>
    /// Fetch and filter launches from SpaceFlightNow.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Launch>GetLaunches() =>
        htmlLoader.Load(SiteURL)
            .DocumentNode
            .SelectNodes(LaunchXPath)
            .Select(CreateFromNode);
}
