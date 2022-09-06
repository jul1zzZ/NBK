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
        int pageNum = 1;
        public List<Player> Players { get; set; }
        public PlayersPage()
        {
            InitializeComponent();
            Players = NBAShortEntities.GetContext().Players.ToList();
            DataContext = this;
            List<Season> seasons = NBAShortEntities.GetContext().Seasons.ToList();

            seasons.Insert(0, new Season
            {
                Name = "Сезоны"
            });
            SeasonSortCb.ItemsSource = seasons;
            SeasonSortCb.DisplayMemberPath = "Name";
            SeasonSortCb.SelectedIndex = 0;

            CountryCodeCb.SelectedIndex = 0;
        }


        private void Update()
        {
            List<Player> players1 = NBAShortEntities.GetContext().Players.OrderBy(p => p.Name).ToList();
            List<Season> seasons1 = NBAShortEntities.GetContext().Seasons.OrderBy(p => p.Name).ToList();

            if (SearchTb.Text.Trim() != "")
            {
                players1 = players1
                    .Where(p => p.Name.Trim().ToLower().Contains(SearchTb.Text.Trim().ToLower()) ||
                    p.CountryCode.Trim().Contains(SearchTb.Text.Trim().ToLower())).ToList();
            }
            if (SeasonSortCb.SelectedIndex > 0)
            {
                seasons1 = seasons1.Where(p => p.SeasonId == (SeasonSortCb.SelectedItem as Season).SeasonId).ToList();
            }

            if (CountryCodeCb.SelectedIndex > 0)
            {
                switch (CountryCodeCb.SelectedIndex)
                {
                    case 1:
                        players1 = players1.OrderBy(p => p.Weight).ToList();
                        break;
                    case 2:
                        players1 = players1.OrderByDescending(p => p.Weight).ToList();
                        break;
                }
            }
        }


        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void PlayersSortCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void CountryCodeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            List<Season> seasons = NBAShortEntities.GetContext().Seasons.ToList();
            seasons.Insert(0, new Season
            {
                Name = "Сезоны"
            });
            SeasonSortCb.ItemsSource = seasons;
            SeasonSortCb.DisplayMemberPath = "Name";
            SeasonSortCb.SelectedIndex = 0;

            CountryCodeCb.SelectedIndex = 0;
        }

        //private void PageCount_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        //private void prevPages_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void firstBtn_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void secondBtn_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void fourthBtn_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void fivethBtn_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void nextPages_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}

