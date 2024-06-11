using Domain.Entities;

namespace Domain.Models;
public class StatisticsModel
{
    public CatchCountCardData CatchCountCardData { get; set; }
    public List<AveragesByPower> AveragesByPower { get; set; }
    public List<PokemonsByPowerData> PokemonsByPower { get; set; }

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

public class PokemonsByPowerData
{
    public Power Power { get; set; }
    public int CatchCount { get; set; }
    public int PokemonCount { get; set; }

}