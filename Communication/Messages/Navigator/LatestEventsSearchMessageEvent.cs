using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Navigator
{
	internal sealed class LatestEventsSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int int_ = int.Parse(Event.PopFixedString());
			Session.SendMessage(HabboIM.GetGame().GetNavigator().method_8(Session, int_));
		}
	}
}
