using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Inventory.Furni
{
	internal sealed class GetPetInventoryEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetInventoryComponent() != null)
			{
				Session.SendMessage(Session.GetHabbo().GetInventoryComponent().ComposePetInventoryListMessage());
			}
		}
	}
}
