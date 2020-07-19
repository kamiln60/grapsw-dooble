using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DobbleClient.DobbleServiceReference1;

namespace DobbleClient
{
    public interface IVisitor
    {
        void VisitMainWindow(MainWindow window);
        void VisitLogin(Page1 login);
        void VisitHulp(SelectTable table);
        void VisitSinglePlayerTable(singleplayertable table);
        void VisitNewTable(NewTable table);
        void VisitApp(App app);
    }

    /*
     * Taka luźna propozycja.
     * Żeby skorzystać, w konstruktorach można wywołać: DobbleServerCallback.GetInstance().Visit[NAZWA KLASY](this);
     * Ponieważ zrobiłem z callbacka singleton, będzie łatwo można się do tego dostać.
     * Następnie zaimplementujecie metody callbacka tak, by wywołały publiczne metody danych klas.
     * Przykładowo po otrzymaniu danych kart dobrze byłoby je pokazać na stole.
     */

    public class DobbleServerCallback : IDobbleServerCallback, IVisitor
    {
        private MainWindow mainWindow;

        private Page1 login;

        private SelectTable selectTable;

        private singleplayertable sptable;

        private NewTable newTable;

        private App app;

        private static DobbleServerCallback instance;
        private DobbleServerCallback()
        {
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static DobbleServerCallback GetInstance()
        {
            if (instance == null)
            {
                instance = new DobbleServerCallback();
                // MessageBox.Show("Instancja callbacka zrobiona");
            }


            return instance;
        }

        public void LockClient()
        {
            MessageBox.Show("Dostałeś bana, noobie");
        }



        public void UnlockClient()
        {
            MessageBox.Show("Możesz grać dalej... chyba. REEEE");
        }



        public void SendLog(string message)
        {
            newTable.UpdateLogLabel(message);
        }



        public void SendPlayerData(PlayerDto player)
        {
            Console.WriteLine("Odebrano dane!");
            mainWindow.AcceptPlayerData(player);
        }



        public void SendGameInfo(GameInfo info)
        {

        }



        public void SendRoundData(CardRoundDto roundDto)
        {
            this.newTable.ReceiveRoundData(roundDto);
        }



        public void SendLeaderBoard(LeaderboardRow[] leaderboard)
        {

        }



        public void EndGame()
        {

        }

        public int Ping()
        {
            return Server.GetInstance().Token;
        }

        public void SendPlayerList(PlayerDto[] players)
        {
            // int attempts = 0;
            // while (newTable == null && attempts < 5)
            // {
            //     Thread.Sleep(500);
            //     attempts++;
            // }
            this.newTable.UpdatePlayerList(players.ToList());
        }


        #region AsyncCallbacks
        public IAsyncResult BeginLockClient(AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndLockClient(IAsyncResult result)
        {

        }
        public IAsyncResult BeginUnlockClient(AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndUnlockClient(IAsyncResult result)
        {

        }
        public IAsyncResult BeginSendRoundData(CardRoundDto roundDto, AsyncCallback callback, object asyncState)
        {
            return null;
        }
        public IAsyncResult BeginSendPlayerData(PlayerDto player, AsyncCallback callback, object asyncState)
        {
            return null;
        }
        public IAsyncResult BeginSendLog(string message, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndSendLog(IAsyncResult result)
        {

        }
        public void EndSendPlayerData(IAsyncResult result)
        {

        }
        public void EndSendRoundData(IAsyncResult result)
        {

        }
        public IAsyncResult BeginSendGameInfo(GameInfo info, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndSendGameInfo(IAsyncResult result)
        {

        }
        public IAsyncResult BeginEndGame(AsyncCallback callback, object asyncState)
        {
            return null;
        }
        public IAsyncResult BeginSendLeaderBoard(LeaderboardRow[] leaderboard, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndSendLeaderBoard(IAsyncResult result)
        {

        }
        public void EndEndGame(IAsyncResult result)
        {

        }
        public IAsyncResult BeginPing(AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public int EndPing(IAsyncResult result)
        {
            return Server.GetInstance().Token;
        }
        public IAsyncResult BeginSendPlayerList(PlayerDto[] players, AsyncCallback callback, object asyncState)
        {
            return null;
        }
        public void NotifyRoundEnd(int round)
        {

        }
        public void EndSendPlayerList(IAsyncResult result)
        {

        }

        public void NotifyRoundStart(int round)
        {

        }
        public IAsyncResult BeginNotifyRoundStart(int round, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndNotifyRoundStart(IAsyncResult result)
        {

        }
        public IAsyncResult BeginNotifyRoundEnd(int round, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndNotifyRoundEnd(IAsyncResult result)
        {

        }
        #endregion
        public void VisitMainWindow(MainWindow window)
        {
            this.mainWindow = window;
        }

        public void VisitLogin(Page1 login)
        {
            this.login = login;
        }

        public void VisitHulp(SelectTable table)
        {
            this.selectTable = table;
        }

        public void VisitSinglePlayerTable(singleplayertable table)
        {
            this.sptable = table;
        }

        public void VisitNewTable(NewTable table)
        {
            this.newTable = table;
        }

        public void VisitApp(App app)
        {
            this.app = app;
        }
    }
}
