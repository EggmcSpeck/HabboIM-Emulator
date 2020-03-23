using HabboIM;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Items;
using System;

namespace HabboIM.HabboHotel.Items.Interactors
{

    internal class InteractorSlitherIO : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("362");
        }
    }
}
