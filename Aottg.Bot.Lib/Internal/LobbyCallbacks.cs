using System.Collections.Generic;
using System.Linq;

using Photon.Realtime;

namespace AottgBotLib.Internal
{
    internal class LobbyCallbacks : ILobbyCallbacks
    {
        private List<RoomInfo> _rooms = new List<RoomInfo>();

        public IReadOnlyList<RoomInfo> Rooms => _rooms;

        public void OnJoinedLobby()
        {
            _rooms = new List<RoomInfo>();
        }

        public void OnLeftLobby()
        {
            _rooms = null;
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach(RoomInfo room in roomList)
            {
                if(_rooms.FirstOrDefault(inListRoom => inListRoom.Name == room.Name) == null)
                {
                    _rooms.Add(room);
                }
            }
        }
    }
}
