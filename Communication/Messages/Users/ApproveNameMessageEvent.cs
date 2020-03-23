using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Users
{
	internal sealed class ApproveNameMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(36u);
			Message.AppendInt32(HabboIM.GetGame().GetCatalog().method_8(Event.PopFixedString()) ? 0 : 2);
			Session.SendMessage(Message);
		}
	}
}
