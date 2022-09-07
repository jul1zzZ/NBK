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
            List<Player> players = NBAShortEntities.GetContext().Players.OrderBy(p => p.Name).ToList();
            //List<Season> seasons1 = NBAShortEntities.GetContext().Seasons.OrderBy(p => p.Name).ToList();

            if (SearchTb.Text.Trim() != "")
            {
                players = players
                    .Where(p => p.Name.Trim().ToLower().Contains(SearchTb.Text.Trim().ToLower()) ||
                    p.CountryCode.Trim().Contains(SearchTb.Text.Trim().ToLower())).ToList();
            }
            //if (SeasonSortCb.SelectedIndex > 0)
            //{
            //    seasons1 = seasons1.Where(p => p.SeasonId == (SeasonSortCb.SelectedItem as Season).SeasonId).ToList();
            //}

            if (CountryCodeCb.SelectedIndex > 0)
            {
                switch (CountryCodeCb.SelectedIndex)
                {
                    case 1:
                        players = players.OrderBy(p => p.Weight).ToList();
                        break;
                    case 2:
                        players = players.OrderByDescending(p => p.Weight).ToList();
                        break;
                }
            }
            try
            {
                bool canParse = int.TryParse(PageCount.Text, out int currentPage);
                List<Player> pagePlayers = new List<Player>();
                currentPage = currentPage <= 0 || currentPage > players.Count || !canParse ? 1 : currentPage;
                int itemPerPage = 6;
                int offset = ((currentPage - 1) * itemPerPage + 1) - 1;
                for ( int i =offset; i<itemPerPage +offset;i++)
                {
                    if ( i < players.Count)
                    {
                        pagePlayers.Add(players[i]);
                    }
                }
                DataPlayers.ItemsSource = pagePlayers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void PageCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void prevPages_Click(object sender, RoutedEventArgs e)
        {
            List<Player> players = NBAShortEntities.GetContext().Players.OrderBy(p => p.Name).ToList();
            if (pageNum > 4)
            {
                pageNum -= 4;
                firstBtn.Content = pageNum;
                secondBtn.Content = pageNum + 1;
                fourthBtn.Content = pageNum + 2;
                fivethBtn.Content = pageNum + 3;
            }
        }

        private void firstBtn_Click(object sender, RoutedEventArgs e)
        {
            PageCount.Text = pageNum.ToString();
            Update();
        }

        private void secondBtn_Click(object sender, RoutedEventArgs e)
        {
            PageCount.Text = (pageNum + 1).ToString();
            Update();
        }

        private void fourthBtn_Click(object sender, RoutedEventArgs e)
        {
            PageCount.Text = (pageNum + 2).ToString();
            Update();
        }

        private void fivethBtn_Click(object sender, RoutedEventArgs e)
        {
            PageCount.Text = (pageNum + 3).ToString();
            Update();
        }

        private void nextPages_Click(object sender, RoutedEventArgs e)
        {
            List<Player> players = NBAShortEntities.GetContext().Players.OrderBy(p => p.Name).ToList();
            if (pageNum < players.Count / 6)
            {
                pageNum += 4;
                firstBtn.Content = pageNum;
                secondBtn.Content = pageNum + 1;
                fourthBtn.Content = pageNum + 2;
                fivethBtn.Content = pageNum + 3;
            }

        }
    }
}

