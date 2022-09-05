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
using nba.Pages;

namespace nba.Pages
{
    /// <summary>
    /// Логика взаимодействия для PlayersPage.xaml
    /// </summary>
    public partial class PlayersPage : Page
    {
        public List<Player> Players { get; set; }
        public PlayersPage()
        {
            InitializeComponent();
            Players = NBAShortEntities.GetContext().Players.ToList();
            DataContext = this;


        }
    }
}
