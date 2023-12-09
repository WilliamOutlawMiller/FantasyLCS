using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;
using System.Data;
using Microsoft.AspNetCore.Mvc;

public class MatchController : Controller
{
    public IActionResult GetMatchPicksAndBans(string url)
    {
        string picksAndBansXPath = "//table[@class='wikitable plainlinks hoverable-rows column-show-hide-1' and @id='pbh-table']/tbody";
        try
        {
            HtmlNode tableNode = LocateHTMLNode(url, picksAndBansXPath).Result;
            DataTable dataTable = ParseHtmlToDataTable(tableNode);

            // todo: serialize the data from the datatable into an object that contains pick and ban data for a specific match
            // we could also get all pick ban data available and cache it on the server. 
            return Json(new { key = "value" }); 
        }
        catch
        {
            return BadRequest("Unable to locate table from XPath (most likely, no promises though)");
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

    private static DataTable ParseHtmlToDataTable(HtmlNode tableNode)
    {
        var dataTable = new DataTable();
        
        // Skip the title row
        var tableRows = tableNode.Descendants("tr").Skip(1);

        // Extract column headers
        var headerRow = tableRows.FirstOrDefault();
        var headers = headerRow.Descendants("th");
        foreach (var header in headers)
        {
            dataTable.Columns.Add(header.InnerText.Trim());
        }

        // Skip the header row
        var dataRows = tableRows.Skip(1); 

        // Extract rows and data
        foreach (var row in dataRows) // Skip the header row
        {
            var dataCells = row.Descendants("td").ToArray();
            var rowData = new List<string>();

            foreach (var cell in dataCells)
            {
                var titleAttribute = cell.Attributes["title"];
                var championAttribute = cell.Attributes["data-c1"];
                if (championAttribute != null)
                {
                    // If a title attribute is present, use its value
                    rowData.Add(championAttribute.Value.Trim());
                }
                else if (titleAttribute != null)
                {
                    // If a title attribute is present, use its value
                    rowData.Add(titleAttribute.Value.Trim());
                }
                else
                {
                    // Otherwise, use the inner text
                    rowData.Add(cell.InnerText.Trim());
                }
            }

            dataTable.Rows.Add(rowData.ToArray());
        }

        return dataTable;
    }

}