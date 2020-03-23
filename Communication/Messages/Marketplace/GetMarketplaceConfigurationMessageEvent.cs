using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Util;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Marketplace
{
	internal sealed class GetMarketplaceConfigurationMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			DateTime now = DateTime.Now;
			TimeSpan timeSpan = now - HabboIM.ServerStarted;
			ServerMessage Message = new ServerMessage(612u);
			Message.AppendBoolean(true);
			Message.AppendInt32(ServerConfiguration.MarketplaceTax);
			Message.AppendInt32(1);
			Message.AppendInt32(5);
			Message.AppendInt32(1);
			Message.AppendInt32(ServerConfiguration.MarketplacePriceLimit);
			Message.AppendInt32(48);
			Message.AppendInt32(timeSpan.Days);
			Session.SendMessage(Message);
		}
	}
}
