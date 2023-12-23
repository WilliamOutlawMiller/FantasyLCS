using System.Windows.Controls;

namespace FantasyLCS.App
{
    public class NavigationService
    {
        private Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateToPage(Page page)
        {
            _frame.Navigate(page);
        }
    }
}
