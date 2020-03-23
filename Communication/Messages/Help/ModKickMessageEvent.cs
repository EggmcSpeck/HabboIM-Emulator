using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Help
{
	internal sealed class ModKickMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				uint num = Event.PopWiredUInt();
				string text = Event.PopFixedString();
				string string_ = string.Concat(new object[]
				{
					"User: ",
					num,
					", Message: ",
					text
				});
				HabboIM.GetGame().GetClientManager().method_31(Session, "ModTool - Kick User", string_);
				HabboIM.GetGame().GetModerationTool().method_15(Session, num, text, false);
			}
		}
	}
}
