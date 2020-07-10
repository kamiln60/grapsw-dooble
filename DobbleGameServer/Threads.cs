using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DobbleGameServer.data;
using DobbleGameServer.dto;

namespace DobbleGameServer {
    public partial class DobbleServer {
        public Thread PlayerPickThread { get; set; }
        public Thread ConnectionManagementThread { get; set; }

        public SpinLock PlayerPickThreadSpinLock { get; set; }

        public void InitializeThreads()
        {
            this.PlayerPickThread = new Thread(PlayerPickThreadMethod);
            this.ConnectionManagementThread = new Thread(ConnectionManagementThreadMethod);

            this.ConnectionManagementThread.Start();
            
        }

        public void PlayerPickThreadMethod() {
            PlayerPick pick;
            while (true) {

                Monitor.Wait(picksQueue);
                pick = picksQueue.Dequeue();
                Thread.BeginCriticalRegion();
                if (this.state.CurrentCard.Symbols.Contains(pick.Pick)) {
                    this.state.State = State.WAIT_FOR_CARD;
                }

                Thread.EndCriticalRegion();
            }
        }

        public void ConnectionManagementThreadMethod() {
            ConnectionRequest request;
            while (true) {
                request = connectionQueue.Dequeue();

                Thread.BeginCriticalRegion();
                if (this.state.State != State.LOBBY) {
                    request.Callback.SendLog("Gra jest w toku. Spróbuj ponownie później.");
                }
                else {

                }
                Thread.EndCriticalRegion();
            }
        }
    }
}
