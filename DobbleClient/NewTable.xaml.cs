using System;
using System.Collections.Generic;
using System.IO;
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

namespace DobbleClient {
    /// <summary>
    /// Logika interakcji dla klasy NewTable.xaml
    /// </summary>
    public partial class NewTable : Page {
        private Server _server;

        private Dictionary<int, BitmapImage> Symbols;

        private readonly string CARDS_PATH = "C:\\cards";

        public NewTable(MainWindow mainWindow) {
            InitializeComponent();
            this._server = Server.GetInstance();
            this.Symbols = new Dictionary<int, BitmapImage>();
            if (!Directory.Exists(CARDS_PATH))
            {
                
                Directory.CreateDirectory(CARDS_PATH);
                Console.WriteLine("Utworzono katalog dla kart.");
            }

            LoadSymbolsFromFiles();
        }

        private static BitmapImage LoadImage(byte[] imageData) {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData)) {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private bool IsImage(string name)
        {
            return name.EndsWith(".png") || name.EndsWith(".jpg") || name.EndsWith(".bmp");
        }
        private void LoadSymbolsFromFiles()
        {
            var filesToLoad = Directory
                .GetFiles(CARDS_PATH)
                .Where(IsImage)
                .ToArray();
        
            int i = 0;
            foreach (var file in filesToLoad)
            {
                
                byte[] imageBytes = File.ReadAllBytes(file);
                this.Symbols.Add(++i, LoadImage(imageBytes));
            }
        }
        private void Frame_Navigated(object sender, NavigationEventArgs e) {

        }

        private void Button_Click(object sender, RoutedEventArgs e) {

        }

        private void Exit(object sender, RoutedEventArgs e) {

            DobbleServerCallback.GetInstance().VisitNewTable(this);
            if (_server == null) {
                MessageBox.Show("BŁĄD! SERWER NULL :o");
            }

            _server.BeginDisconnect(_server.Token, ExitCallback, null);
            Page1 p1 = new Page1();
            this.NavigationService.Navigate(p1);
        }
        public void ExitCallback(IAsyncResult ar) {
            MessageBox.Show("Opuszczono serwer");
        }
        private void Button_Click_1(object sender, RoutedEventArgs e) {
            DobbleServerCallback.GetInstance().VisitNewTable(this);
            _server.Ready = !_server.Ready;
            _server.BeginDeclareReadiness(_server.Token, _server.Ready, ReadinessCallback, null);
        }

        public void UpdatePlayerList(List<PlayerDto> players) {
            this.Dispatcher.Invoke(() => {
                this.PlayerList.Items.Clear();
                players.ForEach(player => {
                    this.PlayerList.Items.Add(string.Format("{0}, {1} punktów, [{2}]", player.Name, player.Points, player.IsReady ? "GOTOWY" : "NIEGOTOWY"));
                });

            });
        }

        public void UpdateLogLabel(string text) {
            this.Dispatcher.Invoke(() => {
                this.LogLabel.Content = text;
            });
        }

        public void ReceiveRoundData(CardRoundDto dto) {
            this.Dispatcher.Invoke(() =>
            {
                ImageBrush brush = new ImageBrush();
                
                sbut1.Background = new ImageBrush(Symbols[dto.CurrentCard.Symbols[0]]);
                
                sbut2.Background = new ImageBrush(Symbols[dto.CurrentCard.Symbols[1]]);
                sbut3.Background = new ImageBrush(Symbols[dto.CurrentCard.Symbols[2]]);
                sbut4.Background = new ImageBrush(Symbols[dto.CurrentCard.Symbols[3]]);
                sbut5.Background = new ImageBrush(Symbols[dto.CurrentCard.Symbols[4]]);
                sbut6.Background = new ImageBrush(Symbols[dto.CurrentCard.Symbols[5]]);
            });
        }

        public void ReadinessCallback(IAsyncResult ar) {
            //((DobbleServerClient)ar.AsyncState).EndDeclareReadiness(ar);

            this.Dispatcher.Invoke(() => {
                this.ReadinessLabel.Content = _server.Ready ? "Gotowy" : "Nie gotowy";
            });
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void bt1(object sender, RoutedEventArgs e) {

        }


        private void bt3(object sender, RoutedEventArgs e) {

        }

        private void bt4(object sender, RoutedEventArgs e) {

        }

        private void bt5(object sender, RoutedEventArgs e) {

        }

        private void bt6(object sender, RoutedEventArgs e) {

        }

        private void bt2(object sender, RoutedEventArgs e) {

        }
    }
}
