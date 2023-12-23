using System.Windows;
using System.Windows.Navigation;

namespace FantasyLCS.App
{
    public partial class MainWindow : Window
    {
        private readonly NavigationService _navigationService;
        private readonly MainViewModel _mainViewModel;
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();

            _navigationService = new NavigationService(MainFrame);
            _mainViewModel = viewModel;
            DataContext = _mainViewModel;

            HomePage homePage = new HomePage(viewModel, _navigationService);
            _navigationService.NavigateToPage(homePage);
        }
    }
}
