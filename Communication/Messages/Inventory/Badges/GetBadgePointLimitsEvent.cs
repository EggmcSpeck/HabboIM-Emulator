using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Inventory.Badges
{
	internal sealed class GetBadgePointLimitsEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(443u);
			Message.AppendInt32(Session.GetHabbo().AchievementScore);
			Message.AppendStringWithBreak("");
			Session.SendMessage(Message);
		}
	}
}
