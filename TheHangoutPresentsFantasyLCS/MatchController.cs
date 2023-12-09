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
    public IActionResult Index()
    {
        string url = "https://en.wikipedia.org/wiki/List_of_programmers";
        var response = RequestHandler.CallUrl(url).Result;
        return View();
    }
}