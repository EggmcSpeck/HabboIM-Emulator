using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Rooms.Chat
{
	internal sealed class ShoutMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null)
			{
				RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (class2 != null)
				{
					class2.HandleSpeech(Session, HabboIM.FilterString(Event.PopFixedString()), true);
				}
			}
		}
	}
}
