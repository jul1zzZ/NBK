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
using nba.Entities;
using nba.Properties;

namespace nba.Pages
{
    /// <summary>
    /// Логика взаимодействия для MatchupPage.xaml
    /// </summary>
    public partial class MatchupPage : Page
    {
        public MatchupPage()
        {
            InitializeComponent();
            NBAShortEntities context = new NBAShortEntities();
            ListMatchups.ItemsSource = context.Matchups.ToList();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Matchup selectedMatchup = (sender as Button).DataContext as Matchup;
            GridSelectedMatchup.DataContext = selectedMatchup;
        }

    }
}

