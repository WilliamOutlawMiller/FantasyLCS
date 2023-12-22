public class Team
{
    public int ID { get; set; }
    public string Name { get; set; }
    public List<Player> Players { get; set; }

    public List<Player> Subs { get; set; }

    public List<int> PlayerIDs { get; set; }
}