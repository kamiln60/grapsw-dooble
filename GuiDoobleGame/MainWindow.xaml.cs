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
using DobbleGameServer;
using GuiDoobleGame.DobbleServerServiceReference;

namespace GuiDoobleGame
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public class CallbackHandler : IGameClientCallback
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
        private InstanceContext Context { get; set; }

        private DobbleServerClient Server { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            this.Context = new InstanceContext(new CallbackHandler());
            this.Server = new DobbleServerClient(this.Context);
        }
    }
}
