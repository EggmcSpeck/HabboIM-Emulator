using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
using HabboIM.Storage;
namespace HabboIM.Communication.Messages.Rooms.Action
{
	internal sealed class RemoveAllRightsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true))
			{
				foreach (uint current in @class.UsersWithRights)
				{
					RoomUser class2 = @class.GetRoomUserByHabbo(current);
					if (class2 != null && !class2.IsBot)
					{
						class2.GetClient().SendMessage(new ServerMessage(43u));
					}
					ServerMessage Message = new ServerMessage(511u);
					Message.AppendUInt(@class.Id);
					Message.AppendUInt(current);
					Session.SendMessage(Message);
				}
				using (DatabaseClient class3 = HabboIM.GetDatabase().GetClient())
				{
					class3.ExecuteQuery("DELETE FROM room_rights WHERE room_id = '" + @class.Id + "'");
				}
				@class.UsersWithRights.Clear();
			}
		}
	}
}
