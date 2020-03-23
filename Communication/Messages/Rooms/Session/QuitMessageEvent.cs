using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Rooms.Session
{
	internal sealed class QuitMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			try
			{
                if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().InRoom)
				{
					HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId).method_47(Session, true, false);
				}
			}
			catch
			{
			}
		}
	}
}
