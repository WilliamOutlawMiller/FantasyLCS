using System.Net.Http;
using System.Windows;

namespace FantasyLCS.App
{
    public partial class App : Application
    {
        private MainWindow _mainWindow;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string tempUsername = "Test";

            var apiService = new ApiService(new HttpClient(), "http://localhost:5031");
            var mainViewModel = new MainViewModel(apiService, tempUsername);

            // Optionally show a loading window here if needed

            // Initialize the ViewModel asynchronously
            await mainViewModel.InitializeAsync();

            // Now that the data is loaded, create and show the MainWindow
            _mainWindow = new MainWindow(mainViewModel);
            _mainWindow.Show();

            // Close the loading window here if one was opened
        }

    }
}
