namespace FantasyLCS.DataObjects;

public class Team
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string OwnerName { get; set; }
    public string LogoUrl { get; set; }
    public string LogoPath { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public List<int> PlayerIDs { get; set; }
    public List<int> SubIDs { get; set; }
}