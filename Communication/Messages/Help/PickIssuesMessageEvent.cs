using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Help
{
	internal sealed class PickIssuesMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				Event.PopWiredInt32();
				uint uint_ = Event.PopWiredUInt();
				HabboIM.GetGame().GetModerationTool().method_6(Session, uint_);
			}
		}
	}
}
