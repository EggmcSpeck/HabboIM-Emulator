using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Inventory.Purse
{
	internal sealed class GetCreditsInfoEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.GetHabbo().UpdateCredits(false);
			Session.GetHabbo().UpdateActivityPoints(false);
		}
	}
}
