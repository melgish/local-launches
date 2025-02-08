using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Launches.Models;

namespace Launches.Services;

internal static partial class LaunchFilters {
    [ExcludeFromCodeCoverage]
    [GeneratedRegex("(canaveral|kennedy|patrick)", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex SiteRegex();

    /// <summary>
    /// Filter to find launches on the Space Coast.
    /// </summary>
    /// <param name="launches">launch to test</param>
    /// <returns>true if a match, otherwise false</returns>
    public static IEnumerable<Launch> OnTheSpaceCoast(this IEnumerable<Launch> launches) =>
        launches.Where(launch => SiteRegex().IsMatch(launch.Site));
}