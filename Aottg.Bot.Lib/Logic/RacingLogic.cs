using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using System.Threading;
using System.Threading.Tasks;

using Photon.Realtime;
using AottgBotLib.Handlers;
using System.Collections;

namespace AottgBotLib.Logic
{
    /// <summary>
    /// Logic for Akina racing map
    /// </summary>
    public  class RacingLogic : BaseLogic
    {
        private List<KeyValuePair<string, float>> finishers = new List<KeyValuePair<string, float>>();

        public RacingLogic(BotClient client) : base(client)
        {
            client.RPCHandler.AddCallback("netRefreshRacingResult", (args) => { });
            client.RPCHandler.AddCallback("getRacingResult", OnGetRacingResult);
            client.RPCHandler.AddCallback("Chat", (args) =>
            {
                object[] par = args.Arguments;
                string message = par[0] as string;
                string sender = par[1] as string;

                string toAdd = string.Empty;

                if(sender.Length > 0)
                {
                    toAdd = sender + ": " + message;
                }
                else
                {
                    toAdd = message;
                }

                toAdd = $"[{args.CallInfo.Sender.ActorNumber}] {toAdd}".RemoveAll();
                Console.WriteLine(toAdd);
            });

            RestartTimer = 20f;
        }

        protected override void OnGameWinRpc(int score)
        {
            if (Round.GameEnd)
            {
                return;
            }
            Notify($"Round ended. Restart in {RestartTimer} seconds...", true);
            RestartOnTimer();
        }

        public override void OnRestart()
        {
            finishers.Clear();
        }

        private void OnGetRacingResult(RPCArguments args)
        {
            if (!Round.GameEnd)
            {
                GameWin();
                Notify($"Round ended. Restart in {RestartTimer} seconds...", true);
            }

            var pair = new KeyValuePair<string, float>(args.Arguments[0] as string, (float)args.Arguments[1]);
            finishers.Add(pair);
            finishers = finishers.OrderBy(x => x.Value).ToList();

            string str = "Result";
            int i = 1;
            foreach(var idk in finishers)
            {
                str += $"\nRank {i++}: {idk.Key}[FFFFFF] - {idk.Value.ToString("F2")} s";
            }

            Client.SendRPC(2, "netRefreshRacingResult", new object[] { str }, PhotonTargets.Others);
        }
    }
}
