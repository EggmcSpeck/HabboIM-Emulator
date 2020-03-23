using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Util;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Navigator
{
	internal sealed class RoomTextSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			string text = Event.PopFixedString();
			if (Session != null && Session.GetHabbo() != null && text != HabboIM.smethod_0(Session.GetHabbo().Username))
			{
				Session.SendMessage(HabboIM.GetGame().GetNavigator().method_10(text));
			}
			else
			{
			}
		}
	}
}
