using System.ComponentModel.DataAnnotations;

/// <summary>
/// Options for the LaunchService
/// </summary>
internal record LaunchOptions
{
    /// <summary>
    /// Time to wait between scraping the launch schedule.
    /// </summary>
    [Required]
    public TimeSpan UpdateInterval { get; init; } = TimeSpan.FromHours(4);
}