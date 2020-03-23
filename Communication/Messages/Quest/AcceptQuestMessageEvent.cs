using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Quest
{
	internal sealed class AcceptQuestMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            Session.SendNotification("Test: Hallo AcceptQuestMessageEvent");
            uint uint_ = Event.PopWiredUInt();
            	HabboIM.GetGame().GetQuestManager().method_7(uint_, Session);
        }
	}
}
