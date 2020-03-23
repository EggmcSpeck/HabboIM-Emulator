using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Quest
{
	internal sealed class OpenQuestTrackerMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            	HabboIM.GetGame().GetQuestManager().method_4(Session);
            Session.SendNotification("Test: Hallo OpenQuestTrackerMessageEvent");
        }
	}
}
