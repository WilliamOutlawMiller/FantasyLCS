using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

using FantasyLCS.DataObjects;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Draft_Click(object sender, RoutedEventArgs e)
    {
        // Logic to open draft view
    }

    private void Teams_Click(object sender, RoutedEventArgs e)
    {
        // Logic to open teams view
    }

    private void DraftPlayer_Click(object sender, RoutedEventArgs e)
    {
        var selectedPlayer = lvAvailablePlayers.SelectedItem as Player;
        if (selectedPlayer != null)
        {
            // Call API to add player to team
        }
    }

    private async Task AddPlayerToTeamAsync(int playerId)
    {
        // API call to add player to team
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Player> AvailablePlayers { get; set; }
        public ObservableCollection<Player> TeamPlayers { get; set; }
        // Other properties and methods
    }
}