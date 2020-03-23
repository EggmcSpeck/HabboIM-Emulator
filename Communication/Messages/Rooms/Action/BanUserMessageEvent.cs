using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Rooms.Action
{
	internal sealed class BanUserMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true))
			{
				uint uint_ = Event.PopWiredUInt();
				RoomUser class2 = @class.GetRoomUserByHabbo(uint_);
				if (class2 != null && !class2.IsBot && !class2.GetClient().GetHabbo().HasFuse("acc_unbannable"))
				{
					@class.method_70(uint_);
					@class.method_47(class2.GetClient(), true, true);
				}
			}
		}
	}
}
