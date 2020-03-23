using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Inventory.AvatarFX
{
	internal sealed class AvatarEffectActivatedEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.GetHabbo().GetEffectsInventoryComponent().method_3(Event.PopWiredInt32());
		}
	}
}
