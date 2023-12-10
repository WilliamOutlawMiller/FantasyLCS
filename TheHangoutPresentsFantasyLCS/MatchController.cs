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

public class MatchController
{

    public List<PickBans.Match> Matches { get; set; } = new List<PickBans.Match>();

    public JsonArray GetMatchPicksAndBans(string url)
    {
        string picksAndBansXPath = "//table[@class='table_list footable toggle-square-filled footable-loaded phone breakpoint']";
        try
        {
            HtmlNode tableNode = LocateHTMLNode(url, picksAndBansXPath).Result;
            JsonArray pickBanJson = ParseHtmlToJsonArray(tableNode);
            
            /*
            (foreach (var item in pickBanJson)
            {
                Matches.Add(JsonSerializer.Deserialize<PickBans.Match>(item));
            }
            */

            return pickBanJson;
        }
        catch
        {
            return new JsonArray();
        }
    }

    private static async Task<HtmlNode> LocateHTMLNode(string url, string xpath)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);

        var tbodyElement = doc.DocumentNode.SelectSingleNode(xpath);

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

    private static JsonArray ParseHtmlToJsonArray(HtmlNode tableNode)
    {
        var resultArray = new JsonArray();
        
        // Skip the title row
        var tableRows = tableNode.Descendants("tr").Skip(1);

        // Extract column headers
        var headerRow = tableRows.FirstOrDefault();
        var headers = headerRow.Descendants("th").Select(header => header.InnerText.Trim());

        // Skip the header row
        var dataRows = tableRows.Skip(1); 

        // Extract rows and data
        foreach (var row in dataRows) // Skip the header row
        {
            var dataCells = row.Descendants("td").ToArray();
            var rowData = new JsonObject();

            var headerEnumerator = headers.GetEnumerator();
            
            foreach (var cell in dataCells)
            {
                var titleAttribute = cell.Attributes["title"];
                var championAttribute = cell.Attributes["data-c1"];

                string columnName = headerEnumerator.MoveNext() ? headerEnumerator.Current : null;

                if (championAttribute != null)
                {
                    // If a title attribute is present, use its value
                    rowData[columnName] = championAttribute.Value.Trim();
                }
                else if (titleAttribute != null)
                {
                    // If a title attribute is present, use its value
                    rowData[columnName] = titleAttribute.Value.Trim();
                }
                else
                {
                    // Otherwise, use the inner text
                    rowData[columnName] = cell.InnerText.Trim();
                }
            }

            resultArray.Add(rowData);
        }

        return resultArray;
    }
}