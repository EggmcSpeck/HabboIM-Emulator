using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Inventory.Furni
{
	internal sealed class RequestFurniInventoryEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session != null && Session.GetHabbo() != null)
			{
                if (Session.GetHabbo().Rank < 6 && HabboIM.GetGame().GetClientManager().wartung == true)
                {
                    Session.SendNotification("Das Hotel befindet sich momentan im Wartungszustand!\r\rDu kannst dein Inventar nicht öffnen.");

                    return;
                }
                Session.SendMessage(Session.GetHabbo().GetInventoryComponent().method_13());
			}
		}
	}
}
