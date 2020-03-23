using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Navigator
{
	internal sealed class RoomsWithHighestScoreSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.GetConnection().SendData(HabboIM.GetGame().GetNavigator().SerializeNavigator(Session, -2));
		}
	}
}
