using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Launches.Models;

internal static partial class LaunchFilters {
    [ExcludeFromCodeCoverage]
    [GeneratedRegex("(canaveral|kennedy|patrick)", RegexOptions.IgnoreCase, "en-US")]
    public static partial Regex SiteRegex();

    /// <summary>
    /// Test if launch site matches one of the common local names for the
    /// Space Coast.
    /// </summary>
    /// <param name="launch">launch to test</param>
    /// <returns>true if a match, otherwise false</returns>
    public static IEnumerable<Launch> OnTheSpaceCoast(this IEnumerable<Launch> launches) =>
        launches.Where(launch => SiteRegex().IsMatch(launch.Site));
}