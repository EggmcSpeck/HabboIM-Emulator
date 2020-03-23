using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Navigator
{
	internal sealed class CancelEventMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true) && @class.Event != null)
			{
				@class.Event = null;
				ServerMessage Message = new ServerMessage(370u);
				Message.AppendStringWithBreak("-1");
				@class.SendMessage(Message, null);
			}
		}
	}
}
