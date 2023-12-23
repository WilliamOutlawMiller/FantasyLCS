using System.Windows;

namespace FantasyLCS.App
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadTeamInformation();
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

        private void LoadTeamInformation()
        {
            // Replace with actual data retrieval logic
            string teamName = "My Team";
            string userName = "John Doe";
            string winLossRecord = "5W - 3L";

            // Set the text of TextBlocks
            TeamNameTextBlock.Text = "Team Name: " + teamName;
            UserNameTextBlock.Text = "User's Name: " + userName;
            WinLossTextBlock.Text = "Win/Loss: " + winLossRecord;
        }
    }
}
