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

namespace nba.Pages
{
    /// <summary>
    /// Логика взаимодействия для VisitorPage.xaml
    /// </summary>
    public partial class VisitorPage : Page
    {
        public VisitorPage()
        {
            InitializeComponent();
        }

        private void TeamsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MatchupsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.MatchupPage());
        }

        private void PlayersBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.PlayersPage());
        }

        private void PhotosBtn_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
