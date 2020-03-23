using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Handshake
{
	internal sealed class GetSessionParametersMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(257u);
			Message.AppendInt32(0);
			Session.SendMessage(Message);
		}
	}
}
