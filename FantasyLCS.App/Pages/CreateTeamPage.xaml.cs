using System.Windows;
using System.Windows.Controls;
using System.IO;
using FantasyLCS.App.Classes;

namespace FantasyLCS.App
{
    public partial class CreateTeamPage : Page
    {
        NavigationService _navigationService;
        MainViewModel _mainViewModel;
        public CreateTeamPage(NavigationService navigationService, MainViewModel mainViewModel)
        {
            InitializeComponent();

            _mainViewModel = mainViewModel;
            DataContext = _mainViewModel;
            _navigationService = navigationService;

            _mainViewModel.InitializeAsync();
        }

        private async void CreateTeam_Click(object sender, RoutedEventArgs e)
        {
            string teamName = TeamNameTextBox.Text;
            string logoUrl = TeamLogoTextBox.Text;

            if (!IsTeamNameValid(teamName))
            {
                // Display a message box informing about invalid characters
                MessageBox.Show("The team name contains invalid characters. Please avoid using these characters: \\ / : * ? \" < > |");
                return;
            }

            bool success = await _mainViewModel.CreateTeam(teamName, logoUrl);
            if (success)
            {
                MessageBox.Show("Team created successfully!");
                HomePage homePage = new HomePage(_mainViewModel, _navigationService);
                _navigationService.NavigateToPage(homePage);
            }
            else
            {
                MessageBox.Show("Failed to create team.");
            }
        }

        private bool IsTeamNameValid(string teamName)
        {
            // Get a list of invalid filename characters
            char[] invalidChars = Path.GetInvalidFileNameChars();

            if (invalidChars.Any(teamName.Contains) || teamName.Length == 0) 
                return false;

            // Check if the team name contains any invalid characters
            return true;
        }
    }
}
