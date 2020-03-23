using System;
using System.Linq;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.SoundMachine;
using HabboIM.HabboHotel.Rooms;

namespace HabboIM.Communication.Messages.SoundMachine
{
	internal sealed class GetJukeboxPlayListMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            if (Session != null && Session.GetHabbo() != null)
            {
                /*ServerMessage Message = new ServerMessage(334u);
                Message.AppendInt32(20);
                Message.AppendInt32(16);
                for (int i = 1; i <= 16; i++)
                {
                    Message.AppendInt32(i);
                    Message.AppendInt32(i);
                }
                Session.SendMessage(Message);*/

                Room currentRoom = Session.GetHabbo().CurrentRoom;
                RoomMusicController roomMusicController = currentRoom.GetRoomMusicController();
                Session.SendMessage(JukeboxDiscksComposer.Compose(roomMusicController.PlaylistCapacity, roomMusicController.Playlist.Values.ToList<SongInstance>()));
            }
		}
	}
}
