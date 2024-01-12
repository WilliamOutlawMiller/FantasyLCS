using FantasyLCS.DataObjects.PlayerStats;

namespace FantasyLCS.DataObjects;

public class Player
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int TeamID { get; set; }
    public GeneralStats GeneralStats { get; set; }

    public AggressionStats AggressionStats { get; set; }

    public EarlyGameStats EarlyGameStats { get; set; }

    public VisionStats VisionStats { get; set; }

    public List<ChampionStats> ChampionStats { get; set; }

}