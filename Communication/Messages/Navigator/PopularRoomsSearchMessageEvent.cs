using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Navigator
{
	internal sealed class PopularRoomsSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            if (Session != null && Session.GetConnection() != null)
            {
                Session.GetConnection().SendData(HabboIM.GetGame().GetNavigator().SerializeNavigator(Session, Event.PopFixedInt32()));
            }
		}
	}
}
