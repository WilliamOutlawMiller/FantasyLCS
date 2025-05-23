using HtmlAgilityPack;
using System.Text.Json.Nodes;
using System.Text.Json;

using FantasyLCS.DataObjects;

public abstract class StatsScraper
{   
    private string _url;
    public string URL 
    { 
        get
        {
            if (_url != null)
                return _url;
            
            throw new Exception("URL is null, please populate this property.");
        }
        set
        {
            // Any time we change the webpage we are pointed at, we must reload the HtmlDocument
            _url = value;
            CurrentWebpage = LoadHtmlDocumentAsync(_url).Result;
        }
    }

    private HtmlDocument _currentWebpage;
    protected HtmlDocument CurrentWebpage 
    { 
        get 
        {
            if (_currentWebpage != null)
                return _currentWebpage;

            _currentWebpage = LoadHtmlDocumentAsync(URL).Result;
            return _currentWebpage;
        }
        set
        {
            _currentWebpage = value;
        }
    }
    
    public StatsScraper()
    {
        // This constructor assumes that you will be setting the URL later.
    }

    public StatsScraper(string url)
    {
        URL = url;
    }

    public abstract List<int> GetMatchIDs();
    public abstract List<int> GetTeamIDs();
    public abstract List<int> GetPlayerIDs(int teamID);
    public abstract List<FullStat> GetMatchFullStats(int matchID);
    public abstract Player GetPlayer(int playerID);

    public static async Task<HtmlDocument> LoadHtmlDocumentAsync(string url)
    {
        var web = new HtmlWeb();
        var htmlDoc = await web.LoadFromWebAsync(url);
        return htmlDoc;
    }

    protected HtmlNode LocateHTMLNode(string xPath)
    {
        var element = CurrentWebpage.DocumentNode.SelectSingleNode(xPath);
        
        if (element != null)
        {
            return element;
        }
        else
        {
            throw new Exception("Unable to find the node from the given xPath: " + xPath);
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
    /// Attempts to scrape a page that contains Key/Value pairs i.e. CS per Minute: 8.5
    /// Will not work on tables with headers.
    /// </summary>
    /// <param name="dictionaryXPath"></param>
    /// <returns></returns>
    protected List<Dictionary<string, string>> ScrapeDictionary(string dictionaryXPath)
    {
        var tableNode = CurrentWebpage.DocumentNode.SelectSingleNode(dictionaryXPath);

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

            var rowData = new Dictionary<string, string>
            {
                // God the formatting of this website is so irregular that we have to handle for so many specific edge cases
                { cells[0].InnerText.Trim(' ').Trim(':').Trim(' '), cells[1].InnerText.Trim() }
            };

            data.Add(rowData);
        }

        return data;
    }

    /// <summary>
    /// Attempts to scrape a table by accessing the tbody and looping through tr and td elements.
    /// </summary>
    /// <param name="tableXPath"></param>
    /// <returns></returns>
    protected JsonArray ScrapeTable(string tableXPath)
    {
        JsonArray data = new JsonArray();

        var tableNode = CurrentWebpage.DocumentNode.SelectSingleNode(tableXPath);

        if (tableNode == null)
            return data;

        var headers = tableNode.SelectNodes("thead/tr/th");
        var rows = tableNode.SelectNodes("tbody/tr");

        if (rows == null)
            return data;

        foreach (var row in rows)
        {
            var dataObject = new JsonObject();
            var cells = row.SelectNodes("td");
            
            for (int i = 0; i < cells.Count; i++)
            {
                dataObject.Add(headers[i].InnerText.Trim(), cells[i].InnerText.Trim());
            }

            data.Add(dataObject);
        }

        return data;
    }
}