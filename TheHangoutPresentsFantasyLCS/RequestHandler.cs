using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;

public class RequestHandler
{
	public static async Task<string> CallUrl(string fullUrl)
	{
		HttpClient client = new HttpClient();
		var response = await client.GetStringAsync(fullUrl);
		return response;
	}
}