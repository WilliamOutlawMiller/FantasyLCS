namespace FantasyLCS.DataObjects
{
    public class Team
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<int> PlayerIDs { get; set; }
        public List<int> SubIDs { get; set; }
    }
}