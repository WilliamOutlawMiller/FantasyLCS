using static StorageManager;

public class GetData
{
    public static Player GetPlayer(int id)
    {
        List<Player> players = ReadData<Player>();
        return players.Where(player => player.ID == id).Single();
    }

    public static Match GetMatch(int id)
    {
        List<Match> matches = ReadData<Match>();
        return matches.Where(match => match.ID == id).Single();
    }
}
