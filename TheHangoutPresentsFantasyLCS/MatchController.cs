using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc;

public class MatchController : Controller
{
    public IActionResult GetMatchPicksAndBans(string url)
    {
        string picksAndBansXPath = "//table[@class='wikitable plainlinks hoverable-rows column-show-hide-1' and @id='pbh-table']/tbody";
        try
        {
            string htmlTable = ParseHtml(url, picksAndBansXPath).Result;
            // todo: parse html into readable datatable or other data structure
            return View();
        }
        catch
        {
            return BadRequest("Unable to locate table from XPath (most likely, no promises though)");
        }
    }

    private static async Task<string> ParseHtml(string url, string xpath)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);

        var tbodyElement = doc.DocumentNode.SelectSingleNode(xpath);

        try
        {
            if (tbodyElement != null)
            {
                return tbodyElement.OuterHtml;
            }
            else
            {
                throw new Exception("Unable to find the <tbody> element.");
            }
        }
        catch (Exception ex)
        {
            return await Task.FromException<string>(ex);
        }
    }
}