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
    /// Logika interakcji dla klasy SelectTable.xaml
    /// </summary>
    public partial class SelectTable : Page
    {
        public SelectTable()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void EXIT(object sender, RoutedEventArgs e)
        {
            Page1 p1 = new Page1();
            this.NavigationService.Navigate(p1);
        }
    }
}
