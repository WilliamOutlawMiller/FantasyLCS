using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;

namespace FantasyLCS.App.Classes
{
    public static class LogoHelper
    {
        public static async Task DownloadImageAsync(string imageUrl, string teamName)
        {
            // Define the base path for the Images folder
            string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string imagesFolder = Path.Combine(basePath, "Images");

            // Ensure the Images directory exists
            Directory.CreateDirectory(imagesFolder);

            // Construct the local file path
            string localPath = Path.Combine(imagesFolder, $"{teamName}");

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
