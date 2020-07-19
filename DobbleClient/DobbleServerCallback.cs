using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DobbleClient.DobbleServiceReference1;

namespace DobbleClient {
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

    public class DobbleServerCallback: IDobbleServerCallback, IVisitor
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

        public static DobbleServerCallback GetInstance()
        {
            if (instance == null)
            {
                instance = new DobbleServerCallback();
            }

            return instance;
        }

        public void LockClient()
        {
            throw new NotImplementedException();
        }

        public void UnlockClient()
        {
            throw new NotImplementedException();
        }

        public void SendLog(string message)
        {
            throw new NotImplementedException();
        }

        public void SendPlayerData(PlayerDto player)
        {
            throw new NotImplementedException();
        }

        public void SendGameInfo(GameInfo info)
        {
            throw new NotImplementedException();
        }

        public void SendRoundData(CardRoundDto roundDto)
        {
            throw new NotImplementedException();
        }

        public void SendLeaderBoard(LeaderboardRow[] leaderboard)
        {
            throw new NotImplementedException();
        }

        public void EndGame()
        {
            throw new NotImplementedException();
        }

        public int Ping()
        {
            throw new NotImplementedException();
        }

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
