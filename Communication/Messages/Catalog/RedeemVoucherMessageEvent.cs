using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Catalog
{
	internal sealed class RedeemVoucherMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			HabboIM.GetGame().GetCatalog().method_21().method_2(Session, Event.PopFixedString());
		}
	}
}
