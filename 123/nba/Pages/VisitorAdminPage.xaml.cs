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

namespace nba.Pages
{
    /// <summary>
    /// Логика взаимодействия для VisitorAdminPage.xaml
    /// </summary>
    public partial class VisitorAdminPage : Page
    {
        public VisitorAdminPage()
        {
            InitializeComponent();
        }

        private void VisitorBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new VisitorPage());
        }
    }
}
