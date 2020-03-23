using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Navigator
{
	internal sealed class RoomTagSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Event.PopWiredInt32();
			Session.SendMessage(HabboIM.GetGame().GetNavigator().method_10(Event.PopFixedString()));
		}
	}
}
