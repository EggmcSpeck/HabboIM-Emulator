using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Achievements;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Inventory.Achievements
{
	internal sealed class GetAchievementsEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.SendMessage(AchievementManager.smethod_1(Session));
		}
	}
}
