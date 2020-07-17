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
    /// Logika interakcji dla klasy singleplayertable.xaml
    /// </summary>
    public partial class singleplayertable : Page
    {
        public singleplayertable()
        {
            InitializeComponent();
        }


        private void Exit(object sender, RoutedEventArgs e)
        {
            Page1 p1 = new Page1();
            this.NavigationService.Navigate(p1);
        }


    }
}
