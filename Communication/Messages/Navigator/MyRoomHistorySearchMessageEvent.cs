using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Navigator
{
	internal sealed class MyRoomHistorySearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.SendMessage(HabboIM.GetGame().GetNavigator().method_7(Session));
		}
	}
}
