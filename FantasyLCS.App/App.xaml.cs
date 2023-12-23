using System.Net.Http;
using System.Windows;

namespace FantasyLCS.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var apiService = new ApiService(new HttpClient(), "your_api_base_url");
            var mainViewModel = new MainViewModel(apiService);

            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            mainWindow.Show();
        }
    }
}