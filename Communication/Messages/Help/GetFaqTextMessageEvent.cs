using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Support;
namespace HabboIM.Communication.Messages.Help
{
	internal sealed class GetFaqTextMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			HelpTopic @class = HabboIM.GetGame().GetHelpTool().method_4(uint_);
			if (@class != null)
			{
				Session.SendMessage(HabboIM.GetGame().GetHelpTool().method_9(@class));
			}
		}
	}
}
