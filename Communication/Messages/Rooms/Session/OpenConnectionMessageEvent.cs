using System;
using HabboIM.Core;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Rooms;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Rooms.Session
{
	internal sealed class OpenConnectionMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Event.PopWiredInt32();
			uint num = Event.PopWiredUInt();
			Event.PopWiredInt32();
			if (HabboIM.GetConfig().data["emu.messages.roommgr"] == "1")
			{
				Logging.WriteLine("[RoomMgr] Requesting Public Room [ID: " + num + "]");
			}
            RoomData @class = HabboIM.GetGame().GetRoomManager().method_12(num);
			if (@class != null && !(@class.Type != "public"))
			{
				Session.GetClientMessageHandler().method_5(num, "");
			}
		}
	}
}
