using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Marketplace
{
	internal sealed class GetOwnOffersMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.SendMessage(HabboIM.GetGame().GetCatalog().method_22().method_9(Session.GetHabbo().Id));
		}
	}
}
