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
    public abstract List<PlayerStats> GetMatchFullStats(string url);
    public abstract TeamStats GetTeamStats(string url);
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
}