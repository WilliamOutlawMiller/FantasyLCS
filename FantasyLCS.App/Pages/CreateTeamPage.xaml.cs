using System.Windows;
using System.Windows.Controls;
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

        }

        private async void CreateTeam_Click(object sender, RoutedEventArgs e)
        {
            string teamName = TeamNameTextBox.Text;
            string logoUrl = TeamLogoTextBox.Text;

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
    }
}
