using System.Text.RegularExpressions;
using Launches.Models;

public static partial class LaunchFilters {
    [GeneratedRegex("(canaveral|kennedy|patrick)", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex SiteRegex();

    /// <summary>
    /// Test if launch site matches one of the common local names for the
    /// Space Coast.
    /// </summary>
    /// <param name="launch">launch to test</param>
    /// <returns>true if a match, otherwise false</returns>
    public static IEnumerable<Launch> OnTheSpaceCoast(this IEnumerable<Launch> launches) =>
        launches.Where(launch => SiteRegex().IsMatch(launch.Site));
}