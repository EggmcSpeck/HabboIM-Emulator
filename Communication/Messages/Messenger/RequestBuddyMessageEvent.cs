using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Messenger
{
	internal sealed class RequestBuddyMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetMessenger() != null)
			{
                if (Session.GetHabbo().CurrentQuestId > 0 && HabboIM.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "REQUEST_FRIEND")
				{
                    HabboIM.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
				}
				Session.GetHabbo().GetMessenger().method_16(Event.PopFixedString());
			}
		}
	}
}
