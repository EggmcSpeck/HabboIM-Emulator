using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Help
{
	internal sealed class CallForHelpMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			bool flag = false;
			if (HabboIM.GetGame().GetModerationTool().method_9(Session.GetHabbo().Id))
			{
				flag = true;
			}
			if (!flag)
			{
				string string_ = HabboIM.FilterString(Event.PopFixedString());
				Event.PopWiredInt32();
				int int_ = Event.PopWiredInt32();
				uint uint_ = Event.PopWiredUInt();
				HabboIM.GetGame().GetModerationTool().method_3(Session, int_, uint_, string_);
			}
			ServerMessage Message = new ServerMessage(321u);
			Message.AppendBoolean(flag);
			Session.SendMessage(Message);
		}
	}
}
