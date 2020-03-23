using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Help
{
	internal sealed class GetRoomChatlogMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_chatlogs"))
			{
				Event.PopWiredInt32();
				uint uint_ = Event.PopWiredUInt();
				if (HabboIM.GetGame().GetRoomManager().GetRoom(uint_) != null)
				{
					Session.SendMessage(HabboIM.GetGame().GetModerationTool().method_22(uint_));
				}
			}
		}
	}
}
