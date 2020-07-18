using DobbleGameServer.data;
using DobbleGameServer.dto;
using System.Linq;
using System.Threading;

namespace DobbleGameServer {
    public partial class DobbleServer {
        public void InitGame() {
            GameLobbyLock.EnterWriteLock();
            lock (state) {
                if (IsRequiredNumberOfPlayers() && IsEveryoneReady() && this.state.State == State.Lobby) {
                    StartGame();
                } else if (IsRequiredNumberOfPlayers() && IsEveryoneReady() && this.state.State == State.WaitForReadiness) {
                    this.state.State = State.WaitForCard;
                    BroadcastMessage(string.Format("Rozpoczynanie rundy za {0} sekund...\n", Config.RoundIntervalMS / 1000));
                    NotifyEveryoneAboutRoundStart(state.RoundNumber+1);
                    this.state.NextRoundTimer = new Timer(InitializeRound, null, Config.RoundIntervalMS, 0);
                }
            }
            GameLobbyLock.ExitWriteLock();
        }

        public void StartGame() {
            lock (state) {
                this.state.State = State.WaitForCard;
                this.GenerateCardSchema(Config.SymbolsPerCard, out _, out _);
                this.state.GenerateCardsFromSchema(CardSchema, rng);

                BroadcastMessage(string.Format("Rozpoczynanie rundy za {0} sekund...\n", Config.RoundIntervalMS / 1000));
                NotifyEveryoneAboutRoundStart(state.RoundNumber+1);
                this.state.NextRoundTimer = new Timer(InitializeRound, null, Config.RoundIntervalMS, 0);

            }
        }

        public void InitializeRound(object arg) {
            pingQueue.Enqueue(1);

            lock (this.state) {
                if (this.state.NextRoundTimer != null) {
                    this.state.NextRoundTimer.Dispose();
                    this.state.NextRoundTimer = null;
                }

                if (this.state.State != State.WaitForCard) {

                    return;
                }
                this.state.CardsInUse.Clear();
                this.state.CurrentCard = GetRandomUniqueCard();
                this.state.RoundNumber++;
                if (state.RoundNumber > Config.MaxRoundNumber) {
                    StopGame();
                    return;
                }
                foreach (var playersValue in this.state.Players.Values) {
                    playersValue.CardId = GetRandomUniqueCard().Id;
                    playersValue.Callback.SendRoundData(new CardRoundDto(
                        this.state.RoundNumber,
                        this.state.CurrentCard,
                        this.state.Cards[playersValue.CardId])
                    );
                }
                this.picksQueue.Clear();
                this.state.State = State.PickCard;
            }

        }

        public void StopGame() {
            lock (this.state) {
                this.state.State = State.End;
                BroadcastMessage("Gra zakończona.");
                SendLeaderboard();
                this.state.GameTimer = new Timer(CleanupAfterGame, null, 10000, 0);


                if (state.NextRoundTimer == null) return;
                state.NextRoundTimer.Dispose();
                state.NextRoundTimer = null;
            }
        }

        public void CleanupAfterGame(object arg) {
            lock (this.state) {
                BroadcastMessage("Zostałeś rozłączony (koniec gry)");
                this.state.Players.Clear();
                state.Players.Values.ToList()
                    .ForEach(player => player.Callback.EndGame());
                this.state = new GameState(CardSchema, rng);
                if (this.state.GameTimer != null) {
                    this.state.GameTimer.Dispose();
                    this.state.GameTimer = null;
                }
            }
        }
    }
}