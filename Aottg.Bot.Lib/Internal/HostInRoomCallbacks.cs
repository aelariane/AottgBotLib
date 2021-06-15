using System.Collections;
using System.Threading.Tasks;

using Photon.Realtime;

namespace AottgBotLib.Internal
{
    /// <summary>
    /// Room callbacks specifically for <seealso cref="HostBotClient"/>
    /// </summary>
    internal class HostInRoomCallbacks : IInRoomCallbacks
    {
        private HostBotClient _client;

        internal HostInRoomCallbacks(HostBotClient client)
        {
            _client = client;
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            Task.Run(async () => 
            {
                await Task.Delay(150);
                if(_client.GreetingMessage.Length > 0)
                {
                    _client.SendRPC(2, "Chat", new object[] { _client.GreetingMessage, _client.PlayerName }, newPlayer.ActorNumber);
                }
            });
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
        }
    }
}
