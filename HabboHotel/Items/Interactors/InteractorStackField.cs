using HabboIM.HabboHotel.GameClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabboIM.HabboHotel.Items.Interactors
{
    class InteractorStackField : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
        {
        }
        public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
        {
        }
        public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            if (Session.GetHabbo().CurrentRoom.CheckRights(Session, false))
                HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("9|" + RoomItem_0.uint_0 + "|" + RoomItem_0.Double_0);
        }
    }
}
