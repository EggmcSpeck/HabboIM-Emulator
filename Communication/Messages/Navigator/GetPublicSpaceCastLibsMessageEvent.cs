using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Rooms;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Navigator
{
	internal sealed class GetPublicSpaceCastLibsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			Event.PopFixedString();
			Event.PopWiredInt32();
            RoomData @class = HabboIM.GetGame().GetRoomManager().method_12(num);
			if (@class != null)
			{
				if (@class.Type == "private")
				{
					ServerMessage Message = new ServerMessage(286u);
					Message.AppendBoolean(@class.IsPublicRoom);
					Message.AppendUInt(num);
					Session.SendMessage(Message);
				}
				else
				{
					ServerMessage Message2 = new ServerMessage(453u);
					Message2.AppendUInt(@class.Id);
					Message2.AppendStringWithBreak(@class.CCTs);
					Message2.AppendUInt(@class.Id);
					Session.SendMessage(Message2);
				}
			}
		}
	}
}
