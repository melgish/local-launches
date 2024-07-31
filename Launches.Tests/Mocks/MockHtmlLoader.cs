using HtmlAgilityPack;
using Launches.Services;

public class MockHtmlLoader : IHtmlLoader
{
    // Cover multiple conditions like missing nodes, missing attributes, etc.
    public string Html { get; set; } = TestData.Html;

    public HtmlDocument Load(string url)
    {
        var document = new HtmlDocument();
        document.LoadHtml(Html);
        return document;
    }
}