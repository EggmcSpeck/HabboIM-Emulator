using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Items;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Rooms.Engine
{
    internal sealed class PickupObjectMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                Event.PopWiredInt32();
                Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                if (@class != null && @class.CheckRights(Session, true))
                {
                    RoomItem class2 = @class.method_28(Event.PopWiredUInt());
                    if (class2 != null)
                    {
                        string text = class2.GetBaseItem().InteractionType.ToLower();
                        if (text == null || !(text == "postit"))
                        {
                            @class.method_29(Session, class2.uint_0, false, true);
                            Session.GetHabbo().GetInventoryComponent().method_11(class2.uint_0, class2.uint_2, class2.ExtraData, false);
                            Session.GetHabbo().GetInventoryComponent().method_9(true);
                            if (Session.GetHabbo().CurrentQuestId > 0 && HabboIM.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "PICKUPITEM")
                            {
                                HabboIM.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
                            }
                        }
                    }
                }
            }
        }
    }
}
