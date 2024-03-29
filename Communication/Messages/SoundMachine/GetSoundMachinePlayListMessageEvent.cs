using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.SoundMachine
{
	internal sealed class GetSoundMachinePlayListMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(323u);
			Message.AppendUInt(Session.GetHabbo().CurrentRoomId);
			Message.AppendInt32(1);
			Message.AppendInt32(1);
			Message.AppendInt32(1);
			Message.AppendStringWithBreak("Watercolour");
			Message.AppendStringWithBreak("Pendulum");
			Message.AppendInt32(1);
			Session.SendMessage(Message);
		}
	}
}
