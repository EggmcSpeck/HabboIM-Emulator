using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.Storage;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Navigator
{
	internal sealed class RateFlatMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && !Session.GetHabbo().list_4.Contains(@class.Id) && !@class.CheckRights(Session, true))
			{
				switch (Event.PopWiredInt32())
				{
				case -1:
					@class.Score--;
					break;
				case 0:
					return;
				case 1:
					@class.Score++;
                    if (Session.GetHabbo().FriendStreamEnabled)
                    {
                        using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                        {
                            string look = HabboIM.FilterString(Session.GetHabbo().Figure);
                            class2.AddParamWithValue("look", look);
                            class2.ExecuteQuery("INSERT INTO `friend_stream` (`id`, `type`, `userid`, `gender`, `look`, `time`, `data`) VALUES (NULL, '1', '" + Session.GetHabbo().Id + "', '" + Session.GetHabbo().Gender + "', @look, UNIX_TIMESTAMP(), '" + Session.GetHabbo().CurrentRoomId + "');");
                        }
                    }
					break;
				default:
					return;
				}
				using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
				{
					class2.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE rooms SET score = '",
						@class.Score,
						"' WHERE Id = '",
						@class.Id,
						"' LIMIT 1"
					}));
				}
				Session.GetHabbo().list_4.Add(@class.Id);
				ServerMessage Message = new ServerMessage(345u);
				Message.AppendInt32(@class.Score);
				Session.SendMessage(Message);
			}
		}
	}
}
