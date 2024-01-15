using FantasyLCS.DataObjects;

public class League
{
    public int ID { get; set; }

    public string Name { get; set; }

    public string Owner { get; set; }

    public List<User> Users { get; set; }
}
