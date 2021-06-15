using AottgBotLib.Handlers;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace AottgBotLib.Logic
{
    /// <summary>
    /// Base set of game logic
    /// </summary>
    public abstract class BaseLogic
    {
        private DateTime _lastTime;

        /// <summary>
        /// Instance of <seealso cref="BotClient"/> who owns this logic
        /// </summary>
        protected internal BotClient Client { get; private set; }

        /// <summary>
        /// Base time of server.
        /// </summary>
        public float BaseServerTime { get; private set; }

        /// <summary>
        /// Score of human side
        /// </summary>
        public int HumanScore { get; set; }

        /// <summary>
        /// Cancellation source for <seealso cref="CancellationToken"/>. Can be used to cancel tasks on restart
        /// </summary>
        public CancellationTokenSource RestartCancellationSource { get; private set; } = null;

        /// <summary>
        /// Timer to restart game after win or lose
        /// </summary>
        public float RestartTimer { get; protected set; } = 10f;

        /// <summary>
        /// Instance of <seealso cref="Logic.Round"/> that represents round state
        /// </summary>
        public Round Round { get; private set; }

        /// <summary>
        /// How many rounds were played
        /// </summary>
        public int RoundsCount { get; private set; } = 1;

        /// <summary>
        /// Remaining alive time of server
        /// </summary>
        public float ServerTime { get; private set; }

        /// <summary>
        /// Score ot titan side
        /// </summary>
        public int TitanScore { get; set; }

        /// <summary>
        /// Constructir that should be implemented in child classes
        /// </summary>
        /// <param name="client"></param>
        public BaseLogic(BotClient client)
        {
            _lastTime = DateTime.Now;
            Client = client;
            Round = new Round(client.CancellationToken);
            Task.Run(UpdateLoop);

            BaseServerTime = Convert.ToInt32(client.CurrentRoom.Name.Split('`')[3]);
            ServerTime = BaseServerTime;

            client.RPCHandler.AddCallback(SupportedRPC.LoadLevelRPC, (args) => { if (args.CallInfo.Sender.IsMasterClient) { Restart(); } });
            client.RPCHandler.AddCallback(SupportedRPC.NetGameLose, (args) => { OnGameLoseRpc((int)args.Arguments[0]); });
            client.RPCHandler.AddCallback(SupportedRPC.NetGameWin, (args) => { OnGameWinRpc((int)args.Arguments[0]); });
            client.RPCHandler.AddCallback(SupportedRPC.RequireStatus, (args) => { OnRequireStatus(); });
            client.RPCHandler.AddCallback(SupportedRPC.SomeOneIsDead, (args) => { OnSomeOneIsDead((int)args.Arguments[0]); });
            client.RPCHandler.AddCallback(SupportedRPC.OneTitanDown, (args) => { OnTitanDown((string)args.Arguments[0], (bool)args.Arguments[1]); });
            client.RPCHandler.AddCallback(SupportedRPC.RefreshStatus, (rcvArgs) =>
            {
                object[] args = rcvArgs.Arguments;
                OnRefreshStatus(
                    (int)args[0],
                    (int)args[1],
                    (int)args[2],
                    (int)args[3],
                    (float)args[4],
                    (float)args[5],
                    (bool)args[6],
                    (bool)args[7]
                    );
            });

            Task.Run(async () =>
            {
                await Task.Delay(50);
                if (Client.LocalPlayer.IsMasterClient)
                {
                    Client.CurrentRoom.SetCustomProperties(new Hashtable() { { CustomRoomProperty.CurrentRound, RoundsCount } });
                }
            });
        }

        /// <summary>
        /// Restarts game
        /// </summary>
        private void Restart()
        {
            RoundsCount++;
            if (Client.LocalPlayer.IsMasterClient)
            {
                Client.CurrentRoom.SetCustomProperties(new Hashtable() { { CustomRoomProperty.CurrentRound, RoundsCount } });
            }

            if (RestartCancellationSource != null)
            {
                RestartCancellationSource.Cancel();
                RestartCancellationSource = null;
            }

            Round.OnRestart();
            OnRestart();
        }

        private async Task RestartTask(CancellationToken token)
        {
            await Task.Delay((int)(RestartTimer * 1000f));

            if (token.IsCancellationRequested)
            {
                return;
            }

            RestartGame();

            await Task.Delay(50);

            OnRequireStatus();
        }

        private async Task UpdateLoop()
        {
            while (true)
            {
                await Task.Delay(100);
                DateTime now = DateTime.Now;
                float delta = ((now - _lastTime).Milliseconds / 1000f);
                OnUpdate(delta);
                _lastTime = now;
                Client.CancellationToken.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Sends message to all players
        /// </summary>
        /// <param name="message"></param>
        protected void Notify(string message, bool systemMessage)
        {
            Client.SendRPC(2, "Chat", new object[] { message, systemMessage ? string.Empty : Client.PlayerName }, PhotonTargets.All);
        }

        /// <summary>
        /// Calls on <seealso cref="GameLose"/>
        /// </summary>
        protected virtual void OnGameLose()
        {
        }

        /// <summary>
        /// Calls when "netGameLose" RPC received
        /// </summary>
        /// <param name="score"></param>
        protected virtual void OnGameLoseRpc(int score)
        {
        }

        /// <summary>
        /// Calls on <seealso cref="GameWin"/>
        /// </summary>
        protected virtual void OnGameWin()
        {
        }

        /// <summary>
        /// Calls when "netGameWin" RPC received
        /// </summary>
        /// <param name="score"></param>
        protected virtual void OnGameWinRpc(int score)
        {
        }

        /// <summary>
        /// Calls when refreshStatus RPC received
        /// </summary>
        /// <param name="hScore">Human side score</param>
        /// <param name="tScore">Titan side score</param>
        /// <param name="wave">Current wave</param>
        /// <param name="highestWave">Highest acheived wave</param>
        /// <param name="roundTime">Current round time</param>
        /// <param name="serverTime">Remained server time</param>
        /// <param name="startRace">If race was started (UNUSED)</param>
        /// <param name="endRace">If race was finished (UNUSED)</param>
        protected virtual void OnRefreshStatus(int hScore, int tScore, int wave, int highestWave,
                                               float roundTime, float serverTime, bool startRace, bool endRace)
        {
            HumanScore = hScore;
            TitanScore = tScore;
            Round.RoundTime = roundTime;
            ServerTime = BaseServerTime - serverTime;
        }

        /// <summary>
        /// Sends refreshStatus RPC
        /// </summary>
        protected virtual void OnRequireStatus()
        {
            object[] arguments = new object[]
            {
                HumanScore, TitanScore,
                0, 0,
                Round.RoundTime, (BaseServerTime - ServerTime),
                false, false
            };

            Client.SendRPC(2, "refreshStatus", arguments, PhotonTargets.Others);
        }

        /// <summary>
        /// Calls when player dies
        /// </summary>
        /// <param name="id"></param>
        protected virtual void OnSomeOneIsDead(int id)
        {
        }

        /// <summary>
        /// Calls when titan dies
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isLeaving"></param>
        protected virtual void OnTitanDown(string name, bool isLeaving)
        {
        }

        /// <summary>
        /// Ends round with titan victory
        /// </summary>
        public void GameLose()
        {
            if (Round.GameEnd)
            {
                return;
            }
            Round.GameLose = true;
            Log.LogInfo("Round lost");
            OnGameLose();

            TitanScore++;

            Client.SendRPC(2, "netGameLose", new object[] { TitanScore }, PhotonTargets.Others);
            if (Client.LocalPlayer.IsMasterClient)
            {
                RestartOnTimer();
            }
        }

        /// <summary>
        /// Ends round with humanity victory
        /// </summary>
        public void GameWin()
        {
            if (Round.GameEnd)
            {
                return;
            }
            Log.LogInfo("Round won");
            Round.GameWon = true;
            OnGameWin();

            HumanScore++;

            Client.SendRPC(2, "netGameWin", new object[] { HumanScore }, PhotonTargets.Others);
            if (Client.LocalPlayer.IsMasterClient)
            {
                RestartOnTimer();
            }
        }

        /// <summary>
        /// Calls on Restart
        /// </summary>
        public virtual void OnRestart()
        {
        }

        /// <summary>
        /// Calls on Update
        /// </summary>
        /// <param name="deltaTime">Time between last and current OnUpdate calls</param>
        public virtual void OnUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// Restarts game. Appliable only if player is MasterClient
        /// </summary>
        public void RestartGame()
        {
            if (!Client.LocalPlayer.IsMasterClient)
            {
                return;
            }

            Client.SendRPC(2, "RPCLoadLevel", new object[0], PhotonTargets.Others);
            Restart();
            OnRequireStatus();
        }

        /// <summary>
        /// Restarts game after <seealso cref="RestartTimer"/> seconds
        /// </summary>
        public void RestartOnTimer()
        {
            if (RestartCancellationSource == null)
            {
                RestartCancellationSource = new CancellationTokenSource();
            }
            CancellationToken token = RestartCancellationSource.Token;

            Task.Run(() => RestartTask(token));
        }
    }
}