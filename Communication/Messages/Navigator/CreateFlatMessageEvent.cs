using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Rooms;
using HabboIM.Util;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Navigator
{
	internal sealed class CreateFlatMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().OwnedRooms.Count <= ServerConfiguration.RoomUserLimit)
			{
				string string_ = HabboIM.FilterString(Event.PopFixedString());
				string string_2 = Event.PopFixedString();
				Event.PopFixedString();
                RoomData @class = HabboIM.GetGame().GetRoomManager().method_20(Session, string_, string_2);
				if (@class != null)
				{
					ServerMessage Message = new ServerMessage(59u);
					Message.AppendUInt(@class.Id);
					Message.AppendStringWithBreak(@class.Name);
					Session.SendMessage(Message);
				}
			}
		}
	}
}
