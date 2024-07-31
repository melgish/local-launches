using Launches.Models;

static class TestData
{
    public static readonly List<Launch> Launches = [
        new Launch(
            "1999-03-19",
            "Measure solar activity",
            "FarScape I",
            "Kennedy Space Center",
            "8:00 EST"
        ),
        new Launch(
            "1967-10-16",
            "Jupiter 2",
            string.Empty,
            string.Empty,
            string.Empty
        )
    ];

    public const string Html = """
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
                <div class='launchdate'>1967-10-16</div>
                <div class='mission'>Jupiter 2</div>
            </div>
        </body>
    </html>
    """;
}

