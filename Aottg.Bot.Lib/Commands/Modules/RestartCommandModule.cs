using System.Collections.Generic;
using System.Linq;

namespace AottgBotLib.Commands.Modules
{
    /// <summary>
    /// Provides restart command support
    /// </summary>
    public sealed class RestartCommandModule : CommandModule
    {
        private int _lastKnownRound = -1;
        private List<int> _votedIds = new List<int>();

        /// <summary>
        /// Handles restart request
        /// </summary>
        /// <param name="context">Command execution context</param>
        [Command("restart")]
        [Description("Starts voting to restart. Needed half of player voted to restart")]
        [MasterClientCommand]
        public void RestartCommand(CommandContext context)
        {
            int round = (int) context.Client.CurrentRoom.CustomProperties[CustomRoomProperty.CurrentRound];

            if(_lastKnownRound != round)
            {
                _lastKnownRound = round;
                _votedIds.Clear();
            }
            else if (_votedIds.Contains(context.Sender.ActorNumber))
            {
                context.Client.SendChatMessage("Your vote was already counted, be patient! QwQ", context.Sender);
                return;
            }

            //Doing this to check for players who left room, to not count them in voting
            _votedIds = _votedIds
                .Where(id => context.Client.CurrentRoom.Players.ContainsKey(id))
                .ToList();

            _votedIds.Add(context.Sender.ActorNumber);

            int needs = context.Client.CurrentRoom.PlayerCount / 2;
            context.Client.SendChatMessage($"{_votedIds.Count}/{needs} votes to restart");


            if(_votedIds.Count >= needs)
            {
                context.Client.logic.RestartGame();
            }
        }
    }
}

