using TeamStats;

public class Team
{
    public TeamSummary TeamSummary { get; set; }
    public BannedBy BannedBy { get; set; }
    public BannedAgainst BannedAgainst { get; set; }
    public Economy Economy { get; set; }
    public Aggression Aggression { get; set; }
    public Objectives Objectives { get; set; }
    public Vision Vision { get; set; }
    public Roster Roster { get; set; }
}