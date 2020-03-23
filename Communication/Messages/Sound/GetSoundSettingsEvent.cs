using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Sound
{
	internal sealed class GetSoundSettingsEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(308u);
			Message.AppendInt32(Session.GetHabbo().Volume);
			Message.AppendBoolean(false);
			Session.SendMessage(Message);
		}
	}
}
