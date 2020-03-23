using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Help
{
	internal sealed class DeletePendingCallsForHelpMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (HabboIM.GetGame().GetModerationTool().method_9(Session.GetHabbo().Id))
			{
				HabboIM.GetGame().GetModerationTool().method_10(Session.GetHabbo().Id);
				ServerMessage Message5_ = new ServerMessage(320u);
				Session.SendMessage(Message5_);
			}
		}
	}
}
