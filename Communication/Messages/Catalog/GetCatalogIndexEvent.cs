using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Catalog
{
	internal sealed class GetCatalogIndexEvent : Interface
	{
        public void Handle(GameClient Session, ClientMessage Event)
        {

            if (Session != null && Session.GetHabbo() != null)
            {
                if (Session.GetHabbo().Rank < 6 && HabboIM.GetGame().GetClientManager().wartung == true)
                {
                    Session.SendNotification("Das Hotel befindet sich momentan im Wartungszustand!\r\rDu kannst keine Einkäufe tätigen.");


                }
                else

            if (Session != null && Session.GetHabbo() != null)
                {
                    Session.SendMessage(HabboIM.GetGame().GetCatalog().method_18(Session.GetHabbo().Rank));
                }
            }
        }
	}
}
