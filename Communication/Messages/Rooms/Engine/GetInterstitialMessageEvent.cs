using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Rooms.Engine
{
	internal sealed class GetInterstitialMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.GetClientMessageHandler().method_4();
		}
	}
}
