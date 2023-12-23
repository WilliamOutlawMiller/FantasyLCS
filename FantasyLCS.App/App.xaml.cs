using System.Net.Http;
using System.Windows;

namespace FantasyLCS.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string tempUsername = "Test";

            var apiService = new ApiService(new HttpClient(), "https://localhost:7273");
            var mainViewModel = new MainViewModel(apiService, tempUsername);

            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            mainWindow.Show();
        }
    }
}