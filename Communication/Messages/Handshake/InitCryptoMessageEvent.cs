using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Handshake
{
	internal sealed class InitCryptoMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Interface @interface;
			if (HabboIM.GetPacketManager().Handle(1817u, out @interface))
			{
				@interface.Handle(Session, null);
			}
		}
	}
}
