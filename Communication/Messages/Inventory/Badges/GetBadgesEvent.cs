using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Inventory.Badges
{
	internal sealed class GetBadgesEvent : Interface
	{
        public void Handle(GameClient Session, ClientMessage Event)
        {

            Session.SendMessage(Session.GetHabbo().GetBadgeComponent().ComposeBadgeListMessage());
        }
	}
}
