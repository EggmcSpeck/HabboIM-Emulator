using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
using HabboIM.HabboHotel.Pathfinding;
namespace HabboIM.Communication.Messages.Rooms.Avatar
{
	internal sealed class LookToMessageEvent : Interface
	{
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                if (@class != null)
                {
                    RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
                    if (class2 != null)
                    {
                        class2.Unidle();
                        int num = Event.PopWiredInt32();
                        int num2 = Event.PopWiredInt32();
                        if (num != class2.X || num2 != class2.Y)
                        {
                            int int_ = Class107.smethod_0(class2.X, class2.Y, num, num2);
                            class2.method_9(int_);
                            if (class2.class34_1 != null && class2.RoomUser_0 != null)
                            {
                                class2.RoomUser_0.method_9(int_);
                            }
                        }
                    }
                }
            }
        }
	}
}
