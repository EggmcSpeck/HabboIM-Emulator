using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Items;
namespace HabboIM.HabboHotel.Items.Interactors
{
    internal abstract class FurniInteractor
    {
        public abstract void OnPlace(GameClient Session, RoomItem RoomItem_0);
        public abstract void OnRemove(GameClient Session, RoomItem RoomItem_0);
        public abstract void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0);
    }
}
