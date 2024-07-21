namespace Launches.Models;

/// <summary>
/// Holds information about a launch
/// </summary>
public sealed record Launch(
    string Date,
    string Description,
    string Mission,
    string Site,
    string Time
);
