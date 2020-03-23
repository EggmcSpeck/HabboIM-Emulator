using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Rooms.Action
{
	internal sealed class LetUserInMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.method_26(Session))
			{
				string string_ = Event.PopFixedString();
				byte[] array = Event.ReadBytes(1);
				GameClient class2 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(string_);
				if (class2 != null && class2.GetHabbo() != null && class2.GetHabbo().bool_6 && class2.GetHabbo().uint_2 == Session.GetHabbo().CurrentRoomId)
				{
					if (array[0] == Convert.ToByte(65))
					{
						class2.GetHabbo().bool_5 = true;
						class2.SendMessage(new ServerMessage(41u));
					}
					else
					{
						class2.SendMessage(new ServerMessage(131u));
					}
				}
			}
		}
	}
}
