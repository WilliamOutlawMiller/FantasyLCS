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

    static object Deserialize(List<Dictionary<string, string>> data, Type objectType)
    {
        var jsonObject = new JsonObject();
        string modifiedKey = string.Empty;
        string modifiedValue = string.Empty;

        foreach (var dict in data)
        {
            foreach (var kvp in dict)
            {
                // Check for empty values, cannot have empty's in a dict
                if (kvp.Key.Contains("&nbsp;") || kvp.Value.Contains("&nbsp;"))
                {
                    modifiedKey = kvp.Key.Replace("&nbsp;", "");
                    modifiedValue = kvp.Value.Replace("&nbsp;", "");

                    if (modifiedKey.Length == 0 || modifiedValue.Length == 0)
                        continue;
                    else
                        jsonObject.Add(modifiedKey, modifiedValue);
                }
                else
                    jsonObject.Add(kvp.Key, kvp.Value);
            }
        }     

        var result = JsonSerializer.Deserialize(jsonObject, objectType);
        return result;
    }

    static List<Dictionary<string, string>> ScrapeTable(string url, string tableXPath)
    {
        var web = new HtmlWeb();
        var doc = web.Load(url);

        var tableNode = doc.DocumentNode.SelectSingleNode(tableXPath);

        if (tableNode == null)
        {
            Console.WriteLine("Table not found.");
            return null;
        }

        var rows = tableNode.SelectNodes("tbody/tr");

        if (rows == null || rows.Count == 0)
        {
            Console.WriteLine("No rows found in the tbody.");
            return null;
        }

        var data = new List<Dictionary<string, string>>();

        foreach (var row in rows)
        {
            var cells = row.SelectNodes("td");

            if (cells != null && cells.Count == 2)
            {
                var rowData = new Dictionary<string, string>
                {
                    // God the formatting of this website is so irregular that we have to handle for so many specific edge cases
                    { cells[0].InnerText.Trim(' ').Trim(':').Trim(' '), cells[1].InnerText.Trim() }
                };

                data.Add(rowData);
            }
        }

        return data;
    }

    public override List<FullStats> GetMatchFullStats(string url)
    {
        List<FullStats> fullStats = new List<FullStats>();

        string xPath = GolGGConstants.FULLSTATS;
        try
        {
            HtmlNode tableNode = LocateHTMLNode(url, xPath).Result;
            List<Dictionary<string, string>> pickBanDict = ParseGolGGFullStatsHTML(tableNode);
            JsonArray returnJson = ConvertGolGGFullStatsToJson(pickBanDict);
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