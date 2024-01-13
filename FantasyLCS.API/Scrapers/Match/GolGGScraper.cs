using HtmlAgilityPack;
using System.Data;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Reflection;
using System.Security.Cryptography;
using FantasyLCS.DataObjects;
using FantasyLCS.DataObjects.PlayerStats;
using System.Text;

public class GolGGScraper : StatsScraper
{
    public GolGGScraper() : base()
    {

    }

    public GolGGScraper(string url) : base(url)
    {

    }

    public override List<int> GetMatchIDs()
    {
        List<int> ids = new List<int>();

        try
        {
            return GetGolGGMatchIDs();
        }
        catch
        {
            throw new Exception("Unable to get List of Match IDs.");
        }
    }

    public override List<int> GetTeamIDs()
    {
        List<int> ids = new List<int>();

        try
        {
            return GetGolGGTeamIDs();
        }
        catch
        {
            throw new Exception("Unable to get List of Team IDs.");
        }
    }
    
    public override List<int> GetPlayerIDs(int teamID)
    {
        try
        {
            return GetGolGGPlayerIDs();
        }
        catch
        {
            throw new Exception("Unable to get Team object.");
        }
    }

    public override List<FullStats> GetMatchFullStats(int matchID)
    {
        List<FullStats> fullStats = new List<FullStats>();
        string xPath = GolGGConstants.FULLSTATS;

        try
        {
            HtmlNode tableNode = LocateHTMLNode(xPath);
            List<Dictionary<string, string>> scrapedTable = ParseGolGGFullStatsHTML(tableNode);

            // This implementation must be different due to the fact that the fullstats table has headers on the left.
            JsonArray returnJson = ConvertGolGGFullStatsToJson(scrapedTable);
            foreach (JsonObject playerStatsJson in returnJson)
            {
                fullStats.Add(JsonSerializer.Deserialize<FullStats>(playerStatsJson));
            }

            foreach (var fullStat in fullStats)
                fullStat.MatchID = matchID;
                
            return fullStats;
        }
        catch
        {
            throw new Exception("Unable to get FullStats.");
        }
    }

    public override Player GetPlayer(int playerID)
    {
        Player player = new Player();
        player.ID = playerID;
        player.Name = LocateHTMLNode(GolGGConstants.PlayerStats["PlayerName"]).InnerText.Replace("&nbsp;", "");
        Type objectType;
        PropertyInfo property;

        // Champion stats is a different type of table than the rest, and must be parsed differently.
        var championStatsXPath = GolGGConstants.PlayerStats["ChampionStats"];
        var dictionariesToScrape = new Dictionary<string, string>(GolGGConstants.PlayerStats);
        dictionariesToScrape.Remove("ChampionStats");
        dictionariesToScrape.Remove("PlayerName");

        try
        {
            foreach (var dataTypeAndXPath in dictionariesToScrape)
            {
                string dataType = dataTypeAndXPath.Key;
                string xPath = dataTypeAndXPath.Value;

                List<Dictionary<string, string>> scrapedDictionary = ScrapeDictionary(xPath);
                if (scrapedDictionary == null)
                    continue;

                objectType = Type.GetType("FantasyLCS.DataObjects.PlayerStats." + dataType + ", FantasyLCS.DataObjects");
                property = typeof(Player).GetProperty(dataType);
                var deserializedObject = Deserialize(scrapedDictionary, objectType);

                property.SetValue(player, deserializedObject);
            }

            JsonArray scrapedTable = ScrapeTable(championStatsXPath);
            if (scrapedTable == null)
                return player;

            string cleanedJsonString = JsonSerializer.Serialize(scrapedTable).Replace("\\u0026nbsp;", "");
            player.ChampionStats = JsonSerializer.Deserialize<List<ChampionStats>>(cleanedJsonString);

            player.AggressionStats.PlayerID = playerID;
            player.EarlyGameStats.PlayerID = playerID;
            player.GeneralStats.PlayerID = playerID;
            player.VisionStats.PlayerID = playerID;
            foreach (var champStat in player.ChampionStats)
            {
                champStat.PlayerID = playerID;
                champStat.ChampionID = GenerateChampIDFromName(champStat.Champion);
            }

            return player;
        }
        catch
        {
            throw new Exception("Unable to get Player object.");
        }
    }

    private int GenerateChampIDFromName(string name)
    {
        using (var sha256 = SHA256.Create())
        {
            // Compute the hash of the name
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(name));

            // Use only the first few bytes of the hash to create a number
            // and ensure the ID is a manageable size
            int id = BitConverter.ToInt32(hashBytes, 0);

            // Ensure the ID is positive
            return Math.Abs(id);
        }
    }

    /// <summary>
    /// This method is hardcoded to read from a match fullstats page i.e. https://gol.gg/game/stats/47993/page-fullstats/
    /// Not extensible for other pages, but some code could be moved to methods for reuse (possibly).
    /// </summary>
    /// <param name="tableNode"></param>
    /// <returns></returns>
    private static List<Dictionary<string, string>> ParseGolGGFullStatsHTML(HtmlNode tableNode)
    {
        var resultList = new List<Dictionary<string, string>>();

        var tableRows = tableNode.Descendants("tr").ToArray();

        // Extract the top-level headers text value from the image alt text
        var topLevelHeaders = tableRows[0].Descendants("th");
        var headerImgNodes = topLevelHeaders.Skip(1).Select(th => th.SelectSingleNode(".//img")).ToArray();
        List<string> topLevelImageTexts = new List<string>();

        foreach (var imageNode in headerImgNodes)
        {
            topLevelImageTexts.Add(imageNode.Attributes["alt"].Value);
        }
        
        tableRows = tableRows.AsEnumerable().Skip(1).ToArray();

        foreach (var row in tableRows)
        {
            Dictionary<string, string> rowDict = new Dictionary<string, string>();
            var dataCells = row.Descendants("td").ToArray();
            string subHeader = dataCells[0].InnerText.Trim();

            // The first cell in each row represents the sub-header
            dataCells = dataCells.Skip(1).ToArray();

            // Combine sub-header with top-level headers to form unique keys
            var keys = new List<string>();

            foreach (var topLevelText in topLevelImageTexts)
                keys.Add($"{topLevelText}_{subHeader}");

            // Assign values to the keys
            for (int i = 0; i < keys.Count() && i < dataCells.Length; i++)
            {
                rowDict.Add(keys.ElementAt(i), dataCells[i].InnerText.Trim());
            }

            resultList.Add(rowDict);
        }

        return resultList;
    }

    private static JsonArray ConvertGolGGFullStatsToJson(List<Dictionary<string, string>> data)
    {
        var groupedData = new Dictionary<string, JsonObject>();
        var jsonArray = new JsonArray();

        foreach (var entry in data)
        {
            foreach (var kvp in entry)
            {
                // Assuming the format is "Champion_Stat"
                string[] parts = kvp.Key.Split('_');

                if (parts.Length == 2)
                {
                    string champion = parts[0];
                    string stat = parts[1];

                    if (!groupedData.ContainsKey(champion))
                    {
                        groupedData[champion] = new JsonObject { { "Champion", champion } };
                    }

                    groupedData[champion][stat] = kvp.Value;
                }
            }
        }

        foreach (var value in groupedData.Values)
            jsonArray.Add(value);

        return jsonArray;
    }

    private List<int> GetGolGGPlayerIDs()
    {
        List<int> playerIDs = new List<int>();

        var tableNode = CurrentWebpage.DocumentNode.SelectSingleNode(GolGGConstants.TeamStats["Roster"]);
        
        var rows = tableNode.SelectNodes("tbody/tr");

        foreach (var row in rows)
        {
            // There are two rows that do not contain data, skip them.
            if (row.Descendants("td").Count() == 1) continue;

            // Getting unique player id from the link. I know.
            HtmlNode playerCell = row.SelectNodes("td")[1].SelectSingleNode("a");
            string url = playerCell.Attributes["href"].Value;
            string[] parts = url.Split('/');
            int uniquePlayerID = Convert.ToInt32(parts[3]);
            playerIDs.Add(uniquePlayerID);
        }

        return playerIDs;
    }

    private List<int> GetGolGGMatchIDs()
    {
        List<int> matchIDs = new List<int>();

        var tableNode = CurrentWebpage.DocumentNode.SelectSingleNode(GolGGConstants.MATCHLIST);
        
        var rows = tableNode.SelectNodes("tbody/tr");

        foreach (var row in rows)
        {
            HtmlNode hyperlinkCell = row.SelectNodes("td")[0].SelectSingleNode("a");
            string url = hyperlinkCell.Attributes["href"].Value;
            string[] parts = url.Split('/');
            int uniqueMatchID = Convert.ToInt32(parts[3]);
            matchIDs.Add(uniqueMatchID);
        }

        return matchIDs;
    }
    
    private List<int> GetGolGGTeamIDs()
    {
        List<int> teamIDs = new List<int>();

        var tableNode = CurrentWebpage.DocumentNode.SelectSingleNode(GolGGConstants.TEAMLIST);
        
        var rows = tableNode.SelectNodes("tr");

        foreach (var row in rows)
        {
            HtmlNode hyperlinkCell = row.SelectNodes("td")[0].SelectSingleNode("a");
            string url = hyperlinkCell.Attributes["href"].Value;
            string[] parts = url.Split('/');
            int uniqueTeamID = Convert.ToInt32(parts[2]);
            teamIDs.Add(uniqueTeamID);
        }

        return teamIDs;
    }
}