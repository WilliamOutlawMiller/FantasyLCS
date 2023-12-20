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

public abstract class StatsController
{
    public abstract List<FullStats> GetMatchFullStats(string url);
    public abstract Team GetTeam(string url);
    public abstract List<Player> GetPlayers(string url);
    protected static async Task<HtmlNode> LocateHTMLNode(string url, string xPath)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);

        var tbodyElement = doc.DocumentNode.SelectSingleNode(xPath);

        try
        {
            if (tbodyElement != null)
            {
                return tbodyElement;
            }
            else
            {
                throw new Exception("Unable to find the <tbody> element.");
            }
        }
        catch (Exception ex)
        {
            return await Task.FromException<HtmlNode>(ex);
        }
    }

    protected static object Deserialize(List<Dictionary<string, string>> data, Type objectType)
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

    /// <summary>
    /// Attempts to scrape a standard HTML table by accessing the tbody and looping through tr and td elements.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="tableXPath"></param>
    /// <returns></returns>
    protected static List<Dictionary<string, string>> ScrapeTable(string url, string tableXPath)
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
}