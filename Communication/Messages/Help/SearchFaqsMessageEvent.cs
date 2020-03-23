using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Help
{
	internal sealed class SearchFaqsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			string text = HabboIM.FilterString(Event.PopFixedString());
			if (text.Length >= 1)
			{
				Session.SendMessage(HabboIM.GetGame().GetHelpTool().method_10(text));
			}
		}
	}
}
