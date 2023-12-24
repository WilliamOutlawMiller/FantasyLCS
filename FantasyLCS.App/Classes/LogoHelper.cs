using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

namespace FantasyLCS.App.Classes
{
    public static class LogoHelper
    {
        private static async Task DownloadImageAsync(string imageUrl, string localPath)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(imageUrl))
            using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
            {
                using (Stream streamToWriteTo = File.Open(localPath, FileMode.Create))
                {
                    await streamToReadFrom.CopyToAsync(streamToWriteTo);
                }
            }
        }
    }
}
