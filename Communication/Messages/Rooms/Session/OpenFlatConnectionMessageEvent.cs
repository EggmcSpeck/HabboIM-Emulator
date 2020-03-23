using System;
using HabboIM.Core;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Rooms.Session
{
	internal sealed class OpenFlatConnectionMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			string string_ = Event.PopFixedString();
			Event.PopWiredInt32();
			if (HabboIM.GetConfig().data["emu.messages.roommgr"] == "1")
			{
				Logging.WriteLine("[RoomMgr] Requesting Private Room [ID: " + num + "]");
			}
			Session.GetClientMessageHandler().method_5(num, string_);
		}
	}
}
