using HabboIM;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.Storage;
using System;
using System.Data;

namespace HabboIM.Communication.Messages.Catalog
{
    internal sealed class PurchaseFromCatalogEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int int_ = Event.PopWiredInt32();
            uint uint_ = Event.PopWiredUInt();
            string string_ = Event.PopFixedString();
            if (Session != null && Session.GetHabbo() != null)
            {

                if (Session.GetHabbo().Rank < 6 && HabboIM.GetGame().GetClientManager().wartung == true)
                {
                    Session.SendNotification("Das Hotel befindet sich momentan im Wartungszustand!\r\rDu kannst keine Einkäufe tätigen.");
                    return;

                }
                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    DataRow ltd = dbClient.ReadDataRow("SELECT limited_count FROM catalog_items WHERE id = " + uint_ + " LIMIT 1", 30);
                    if (ltd["limited_count"] != null && (int)ltd["limited_count"] > 0)
                    {
                        HabboIM.GetGame().GetCatalog().method_6(Session, int_, uint_, string_, false, "", "", true);
                    }
                    else if (Session.GetHabbo().int_24 > 1)
                    {
                        int num = 0;
                        while (num < Session.GetHabbo().int_24 && HabboIM.GetGame().GetCatalog().method_6(Session, int_, uint_, string_, false, "", "", num == 0))
                        {
                            num++;
                        }
                        Session.GetHabbo().int_24 = 1;
                    }
                    else
                    {
                        HabboIM.GetGame().GetCatalog().method_6(Session, int_, uint_, string_, false, "", "", true);
                    }
                }
            }
        }
    }
}
