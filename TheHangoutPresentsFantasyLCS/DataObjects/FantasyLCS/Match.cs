public class Match
{
    public int ID { get; set; }
    public Team TeamOne { get; set; }
    public Team TeamTwo { get; set; }
    
    // todo: commented out until we have winner and loser data from page other than fullstats
    /* 
    public Team Winner { get; set; }
    public Team Loser { get; set; }
    */

    public List<FullStats> FullStats { get; set; }
}