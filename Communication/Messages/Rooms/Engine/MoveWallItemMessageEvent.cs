using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Items;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Rooms.Engine
{
	internal sealed class MoveWallItemMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.method_26(Session))
			{
				RoomItem class2 = @class.method_28(Event.PopWiredUInt());
				if (class2 != null)
				{
					string string_ = Event.PopFixedString();
					@class.method_82(Session, class2, false, string_);
				}
			}
		}
	}
}
