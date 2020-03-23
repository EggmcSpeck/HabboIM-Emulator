using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Quest
{
	internal sealed class RejectQuestMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            Session.SendNotification("Test: Hallo RejectQuestMessageEvent");
            HabboIM.GetGame().GetQuestManager().method_7(0u, Session);
		}
	}
}
