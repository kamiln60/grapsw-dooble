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
using DobbleClient.DobbleServiceReference1;

namespace DobbleClient
{
    /// <summary>
    /// Logika interakcji dla klasy NewTable.xaml
    /// </summary>
    public partial class NewTable : Page
    {
        private Server _server;

        public NewTable(MainWindow mainWindow)
        {
            InitializeComponent();
            this._server = Server.GetInstance();
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            // Page1 p1 = new Page1();
            // this.NavigationService.Navigate(p1);
            DobbleServerCallback.GetInstance().VisitNewTable(this);
            if (_server == null)
            {
                MessageBox.Show("BŁĄD! SERWER NULL :o");
            }

            _server.BeginDisconnect(_server.Token, ExitCallback, null);

        }
        public void ExitCallback(IAsyncResult ar)
        {
            MessageBox.Show("Opuszczono serwer");
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                _server.Ready = !_server.Ready;
                MessageBox.Show("Tu jest okej1");
                _server.BeginDeclareReadiness(_server.Token, _server.Ready, ReadinessCallback, null);
                MessageBox.Show("Tu jest okej2");
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }

        }

        public void ReadinessCallback(IAsyncResult ar)
        {
            //((DobbleServerClient)ar.AsyncState).EndDeclareReadiness(ar);
            
            this.ReadinessLabel.Content = _server.Ready? "Gotowy":"Nie gotowy";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }
    }
}
