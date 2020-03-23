using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Support;
namespace HabboIM.Communication.Messages.Help
{
	internal sealed class GetFaqCategoryMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			HelpCategory @class = HabboIM.GetGame().GetHelpTool().method_1(uint_);
			if (@class != null)
			{
				Session.SendMessage(HabboIM.GetGame().GetHelpTool().method_11(@class));
			}
		}
	}
}
