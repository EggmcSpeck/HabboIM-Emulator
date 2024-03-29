using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Items;
using HabboIM.Messages;
using HabboIM.Storage;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Rooms.Furniture
{
	internal sealed class PlacePostItMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
				if (@class != null)
				{
					if (@class.method_72("stickiepole") > 0 || @class.method_26(Session))
					{
						uint uint_ = Event.PopWiredUInt();
						string text = Event.PopFixedString();
						string[] array = text.Split(new char[]
						{
							' '
						});
						if (array[0].Contains("-"))
						{
							array[0] = array[0].Replace("-", "");
						}
						UserItem class2 = Session.GetHabbo().GetInventoryComponent().GetItemById(uint_);
						if (class2 != null)
						{
							if (array[0].StartsWith(":"))
							{
								string text2 = @class.method_98(text);
								if (text2 == null)
								{
									ServerMessage Message = new ServerMessage(516u);
									Message.AppendInt32(11);
									Session.SendMessage(Message);
									return;
								}
								RoomItem RoomItem_ = new RoomItem(class2.uint_0, @class.Id, class2.uint_1, class2.string_0, 0, 0, 0.0, 0, text2, @class);
								if (@class.method_82(Session, RoomItem_, true, null))
								{
                                    Session.GetHabbo().GetInventoryComponent().method_12(uint_, 1u, false);
								}
							}
							using (DatabaseClient class3 = HabboIM.GetDatabase().GetClient())
							{
								class3.ExecuteQuery(string.Concat(new object[]
								{
									"UPDATE items SET room_id = '",
									@class.Id,
									"' WHERE Id = '",
									class2.uint_0,
									"' LIMIT 1"
								}));
							}
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
