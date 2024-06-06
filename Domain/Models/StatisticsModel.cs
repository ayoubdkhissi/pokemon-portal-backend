namespace Domain.Models;
public class StatisticsModel
{
    public CatchCountCardData CatchCountCardData { get; set; }
}

public class CatchCountCardData
{
    public List<string> Labels { get; set; } = [];
    public List<double> Counts { get; set; } = [];
}
