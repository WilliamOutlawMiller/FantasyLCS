using PlayerStats;

public class Player
{
    public int ID { get; set; }
    public GeneralStats GeneralStats { get; set; }

    public List<ChampionStats> ChampionStats { get; set; }

    public AggressionStats AggressionStats { get; set; }

    public EarlyGameStats EarlyGameStats { get; set; }

    public VisionStats VisionStats { get; set; }

}