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

namespace DobbleClient
{
    /// <summary>
    /// Logika interakcji dla klasy NewTable.xaml
    /// </summary>
    public partial class NewTable : Page
    {
        public NewTable()
        {
            InitializeComponent();

        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Page1 p1 = new Page1();
            this.NavigationService.Navigate(p1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }



        //buttony gracza

        private void bt1(object sender, RoutedEventArgs e)
        {
            if (true) { } 
            else { }
        }

        private void bt2(object sender, RoutedEventArgs e)
        {

        }
        private void bt3(object sender, RoutedEventArgs e)
        {

        }
        private void bt4(object sender, RoutedEventArgs e)
        {

        }
        private void bt5(object sender, RoutedEventArgs e)
        {

        }
        private void bt6(object sender, RoutedEventArgs e)
        {

        }
    }
}
