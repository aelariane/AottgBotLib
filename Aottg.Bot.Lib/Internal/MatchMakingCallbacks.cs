using System.Collections.Generic;

using Photon.Realtime;

namespace AottgBotLib.Internal
{
    internal class MatchMakingCallbacks : IMatchmakingCallbacks
    {
        private BotClient _client;

        internal MatchMakingCallbacks(BotClient client)
        {
            _client = client;
        }

        public void OnCreatedRoom()
        {
            _client.SpawnLogic();
            Log.LogInfo("Created room " + _client.CurrentRoom.Name);
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            Log.LogError($"Create random room failed. ReturnCode {returnCode}, message: {message}");
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnJoinedRoom()
        {
            Log.LogInfo("Joined room " + _client.CurrentRoom.Name);
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            Log.LogError($"Join random room failed. ReturnCode {returnCode}, message: {message}");
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            Log.LogError($"Join room failed. ReturnCode {returnCode}, message: {message}");
        }

        public void OnLeftRoom()
        {
            Log.LogInfo("Left room");
            //System.Environment.Exit(-98765);
        }
    }
}
