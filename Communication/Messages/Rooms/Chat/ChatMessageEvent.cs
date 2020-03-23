using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Rooms.Chat
{
	internal sealed class ChatMessageEvent : Interface
	{
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                Room room = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

                if (room != null)
                {
                    RoomUser user = room.GetRoomUserByHabbo(Session.GetHabbo().Id);

                    if (user != null)
                        user.HandleSpeech(Session, HabboIM.FilterString(Event.PopFixedString()), false);
                }
            }
        }
	}
}
