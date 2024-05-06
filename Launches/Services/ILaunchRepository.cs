namespace Launches.Services;

internal sealed class Launch
{
  required public string Date { get; init; }
  required public string Mission { get; init; }
  required public string Time { get; init; }
  required public string Site { get; init; }
  required public string Description { get; init; }
}

internal interface ILaunchRepository {
  public IEnumerable<Launch> Items { get; }
}
