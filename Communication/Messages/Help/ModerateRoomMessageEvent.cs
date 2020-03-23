using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Help
{
	internal sealed class ModerateRoomMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
				bool flag = Event.PopWiredBoolean();
				bool flag2 = Event.PopWiredBoolean();
				bool flag3 = Event.PopWiredBoolean();
				string text = "";
				if (flag)
				{
					text += "Klingeln aktivieren";
				}
				if (flag2)
				{
					text += "Raumnamen aktualisieren";
				}
				if (flag3)
				{
					text += "Habbos aus Raum kicken";
				}
				HabboIM.GetGame().GetClientManager().method_31(Session, "HabboIM -Raum Aktion", text);
				HabboIM.GetGame().GetModerationTool().method_12(Session, uint_, flag3, flag, flag2);
			}
		}
	}
}
