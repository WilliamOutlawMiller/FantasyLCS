using System.IO;
using System.Net.Http;
using System.Windows;
using FantasyLCS.App.Classes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace FantasyLCS.App
{
    public partial class App : Application
    {
        private MainWindow _mainWindow;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string tempUsername = "Test";

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Load appsettings.json
                .Build(); // Build the configuration

            var apiService = new ApiService(new HttpClient(), configuration);
            var mainViewModel = new MainViewModel(apiService, tempUsername);

            mainViewModel.InitializeAsync();

            // Optionally show a loading window here if needed

            _mainWindow = new MainWindow(mainViewModel);
            _mainWindow.Show();

            // Close the loading window here if one was opened
        }

    }
}
