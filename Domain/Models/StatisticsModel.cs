using Domain.Entities;

namespace Domain.Models;
public class StatisticsModel
{
    public CatchCountCardData CatchCountCardData { get; set; }
    public List<AveragesByPower> AveragesByPower { get; set; }
    public List<CatchCountByPowerData> CatchCountByPower { get; set; }

}

public class CatchCountCardData
{
    public List<string> Labels { get; set; } = [];
    public List<int> Counts { get; set; } = [];
}


public class AveragesByPower
{
    public Power Power { get; set; }
    public int PokemonCount { get; set; }
    public double AvgAttack { get; set; }
    public double AvgDefense { get; set; }
}

public class CatchCountByPowerData
{
    public Power Power { get; set; }
    public int Count { get; set; }
}