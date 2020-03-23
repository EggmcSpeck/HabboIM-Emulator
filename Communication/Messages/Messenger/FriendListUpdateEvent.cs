using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Messenger
{
	internal sealed class FriendsListUpdateEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().GetMessenger() != null)
			{
				Session.SendMessage(Session.GetHabbo().GetMessenger().SerializeUpdates());
			}
		}
	}
}
