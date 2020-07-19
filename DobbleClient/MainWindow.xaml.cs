using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public class CallbackHandler
    {
        public void LockClient()
        {
            throw new NotImplementedException();
        }

        public void UnlockClient()
        {
            throw new NotImplementedException();
        }
    }

    public partial class MainWindow : Window
    {
        private Server _server;

        private NewTable newTable;
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this._server = Server.GetInstance();
            this.newTable = new NewTable(this);
            DobbleServerCallback.GetInstance().VisitMainWindow(this);
            DobbleServerCallback.GetInstance().VisitNewTable(newTable);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _server.BeginConnect(NicknameInput.Text, ConnectCallback,null);
            
        }

        public void ConnectCallback(IAsyncResult ar)
        {
           
        }

        public void AcceptPlayerData(PlayerDto player)
        {
            _server.Token = player.Id;
            myframe.Content = newTable;
        }

        private void Main_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void ReturnToMain()
        {
            this.myframe.Content = null;
        }
    }
}
