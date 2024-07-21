using HtmlAgilityPack;
using Launches.Services;

public class MockHtmlLoader : IHtmlLoader
{
    // Cover multiple conditions like missing nodes, missing attributes, etc.
    public string Html { get; set; } = """
        <html lang='en'>
            <body>
                <div class='datename'>
                    <div class='launchdate'>  1999-03-19   </div>
                    <div class='mission'>FarScape I</div>
                </div>
                <div class="missiondata">
                    <span>Launch time:</span>
                    8:00 EST
                    <br>
                    <span>Launch site:</span>
                    Kennedy Space Center
                </div>
                <div class='missdescrip'>Measure solar activity</div>
                <div class='datename'>
                    <div class='launchdate'>1997-10-16</div>
                    <div class='mission'>Jupiter 2</div>
                </div>
            </body>
        </html>
    """;

    public HtmlDocument Load(string url)
    {
        var document = new HtmlDocument();
        document.LoadHtml(Html);
        return document;
    }
}