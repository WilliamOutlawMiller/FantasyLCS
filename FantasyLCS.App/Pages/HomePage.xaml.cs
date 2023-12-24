using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FantasyLCS.App.Classes;

namespace FantasyLCS.App
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        NavigationService _navigationService;
        MainViewModel _mainViewModel;
        public HomePage(MainViewModel viewModel, NavigationService navigationService)
        {
            InitializeComponent();

            _mainViewModel = viewModel;
            DataContext = _mainViewModel;
            _navigationService = navigationService;
        }

        private void FullPlayerStats_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to full player stats view
        }

        private void FullTeamStats_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to full team stats view
        }

        private void StartDraft_Click(object sender, RoutedEventArgs e)
        {
            // Start draft logic
        }

        private void ViewTeamStats_Click(object sender, RoutedEventArgs e)
        {
            // Implement the logic that should occur when the button is clicked
        }

        private void CreateTeamButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateToPage(new CreateTeamPage(_navigationService, _mainViewModel));
        }
    }
}
