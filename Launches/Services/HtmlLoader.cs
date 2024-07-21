using HtmlAgilityPack;

namespace Launches.Services;

public interface IHtmlLoader {
    /// <summary>
    /// Load an HTML document from the supplied URL.
    /// </summary>
    /// <param name="url"></param>
    /// <returns>HtmlDocument instance</returns>
    HtmlDocument Load(string url);
}

/// <summary>
/// Implements the HTML Loader class
/// </summary>
public class HtmlLoader : IHtmlLoader
{
    /// <summary>
    /// Load an HTML document from the supplied URL.
    /// </summary>
    /// <param name="url"></param>
    /// <returns>HtmlDocument instance</returns>
    public HtmlDocument Load(string url) => new HtmlWeb().Load(url);
}
