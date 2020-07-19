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
                DobbleServerCallback.GetInstance().VisitNewTable(this);
                _server.Ready = !_server.Ready;
                _server.BeginDeclareReadiness(_server.Token, _server.Ready, ReadinessCallback, null);
        }

        public void UpdatePlayerList(List<PlayerDto> players)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.PlayerList.Items.Clear();
                players.ForEach(player =>
                {
                    this.PlayerList.Items.Add(string.Format("{0}, {1} punktów, [{2}]", player.Name, player.Points, player.IsReady ? "GOTOWY" : "NIEGOTOWY"));
                });

            });
        }

        public void UpdateLogLabel(string text)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.LogLabel.Content = text;
            });
        }

        public void ReadinessCallback(IAsyncResult ar)
        {
            //((DobbleServerClient)ar.AsyncState).EndDeclareReadiness(ar);

            this.Dispatcher.Invoke(() =>
            {
                this.ReadinessLabel.Content = _server.Ready ? "Gotowy" : "Nie gotowy";
            });
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }
    }
}
