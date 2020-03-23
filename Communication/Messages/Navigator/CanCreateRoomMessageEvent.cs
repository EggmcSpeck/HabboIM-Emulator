using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Util;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Navigator
{
	internal sealed class CanCreateRoomMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(512u);
			if (Session.GetHabbo().OwnedRooms.Count > ServerConfiguration.RoomUserLimit)
			{
				Message.AppendBoolean(true);
				Message.AppendInt32(ServerConfiguration.RoomUserLimit);
			}
			else
			{
				Message.AppendBoolean(false);
			}
			Session.SendMessage(Message);
		}
	}
}
