using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Catalogs;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Catalog
{
	internal sealed class GetIsOfferGiftableEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			CatalogItem @class = HabboIM.GetGame().GetCatalog().method_2(uint_);
			if (@class != null)
			{
				ServerMessage Message = new ServerMessage(622u);
				Message.AppendUInt(@class.uint_0);
				Message.AppendBoolean(@class.method_0().AllowGift);
				Session.SendMessage(Message);
			}
		}
	}
}
