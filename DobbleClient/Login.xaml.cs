using DobbleClient;
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

namespace GuiDoobleGame
{
    /// <summary>
    /// Logika interakcji dla klasy Login.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void NewTable(object sender, RoutedEventArgs e)
        {
            NewTable nt = new NewTable();
            this.NavigationService.Navigate(nt);
        }



        private void singleplayertable(object sender, RoutedEventArgs e)
        {
            singleplayertable sp = new singleplayertable();
            this.NavigationService.Navigate(sp);
        }

        private void Help(object sender, RoutedEventArgs e)
        {

        }

        private void hulp(object sender, RoutedEventArgs e)
        {
          /* hulp h = new hulp();
           this.NavigationService.Navigate(h);*/
        }
    }
}
