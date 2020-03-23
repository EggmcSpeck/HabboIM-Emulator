using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Help
{
	internal sealed class GetModeratorUserInfoMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
				if (HabboIM.GetGame().GetClientManager().GetNameById(uint_) != "Unknown User")
				{
					Session.SendMessage(HabboIM.GetGame().GetModerationTool().method_18(uint_));
				}
				else
				{
					Session.SendNotification("Could not load user info, invalid user.");
				}
			}
		}
	}
}
