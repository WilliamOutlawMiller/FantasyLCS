using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Text.Json.Nodes;
using System.Text.Json;
using Constants;

public class GolGGController : StatsController
{
    public override List<FullStats> GetMatchFullStats(string url)
    {
        List<FullStats> fullStats = new List<FullStats>();

        string xPath = GolGGConstants.FULLSTATS;
        try
        {
            HtmlNode tableNode = LocateHTMLNode(url, xPath).Result;
            List<Dictionary<string, string>> scrapedTable = ParseGolGGFullStatsHTML(tableNode);

            // This implementation must be different due to the fact that the fullstats table has headers on the left.
            JsonArray returnJson = ConvertGolGGFullStatsToJson(scrapedTable);
            foreach (JsonObject playerStatsJson in returnJson)
            {
                fullStats.Add(JsonSerializer.Deserialize<FullStats>(playerStatsJson));
            }

            return fullStats;
        }
        catch
        {
            return new List<FullStats>();
        }
    }

    public override Team GetTeam(string url)
    {
        Team team = new Team();
        try
        {
            foreach (var dataTypeAndXPath in GolGGConstants.TeamStats)
            {
                string dataType = dataTypeAndXPath.Key;
                string xPath = dataTypeAndXPath.Value;

                List<Dictionary<string, string>> scrapedTable = ScrapeTable(url, xPath);
                var objectType = Type.GetType("TeamStats." + dataType);
                var property = typeof(Team).GetProperty(dataType);
                var deserializedObject = Deserialize(scrapedTable, objectType);

                property.SetValue(team, deserializedObject);
            }

            return team;
        }
        catch
        {
            return new Team();
        }
    }

    public override Player GetPlayer(string url)
    {
        Player player = new Player();

        try
        {
            foreach (var dataTypeAndXPath in GolGGConstants.PlayerStats)
            {
                string dataType = dataTypeAndXPath.Key;
                string xPath = dataTypeAndXPath.Value;

                List<Dictionary<string, string>> scrapedTable = ScrapeTable(url, xPath);
                var objectType = Type.GetType("PlayerStats." + dataType);
                var property = typeof(Player).GetProperty(dataType);
                var deserializedObject = Deserialize(scrapedTable, objectType);

                property.SetValue(player, deserializedObject);
            }

            return player;
        }
        catch
        {
            return new Player();
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
}