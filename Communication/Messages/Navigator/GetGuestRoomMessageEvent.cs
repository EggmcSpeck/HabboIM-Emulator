using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Rooms;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Navigator
{
	internal sealed class GetGuestRoomMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			bool bool_ = Event.PopWiredBoolean();
			bool flag = Event.PopWiredBoolean();
            RoomData @class = HabboIM.GetGame().GetRoomManager().method_12(uint_);
			if (@class != null)
			{
				ServerMessage Message = new ServerMessage(454u);
				Message.AppendBoolean(bool_);
				@class.method_3(Message, false, flag);
				Message.AppendBoolean(flag);
				Message.AppendBoolean(bool_);
				Session.SendMessage(Message);
			}
		}
	}
}
