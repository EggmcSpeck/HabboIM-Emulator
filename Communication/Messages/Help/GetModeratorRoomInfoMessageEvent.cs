using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Rooms;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Help
{
	internal sealed class GetModeratorRoomInfoMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
                RoomData class27_ = HabboIM.GetGame().GetRoomManager().method_11(uint_);
				Session.SendMessage(HabboIM.GetGame().GetModerationTool().method_14(class27_));
			}
		}
	}
}
