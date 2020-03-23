using System;
using System.Data;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Items;
using HabboIM.Storage;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Rooms.Furniture
{
	internal sealed class PresentOpenMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                if (@class != null && @class.CheckRights(Session, true))
				{
					RoomItem class2 = @class.method_28(Event.PopWiredUInt());
					if (class2 != null)
					{
						DataRow dataRow = null;
						using (DatabaseClient class3 = HabboIM.GetDatabase().GetClient())
						{
							dataRow = class3.ReadDataRow("SELECT base_id,amount,extra_data FROM user_presents WHERE item_id = '" + class2.uint_0 + "' LIMIT 1");
						}
						if (dataRow != null)
						{
							Item class4 = HabboIM.GetGame().GetItemManager().method_2((uint)dataRow["base_id"]);
							if (class4 != null)
							{
								@class.method_29(Session, class2.uint_0, true, true);
								ServerMessage Message = new ServerMessage(219u);
								Message.AppendUInt(class2.uint_0);
								Session.SendMessage(Message);
								ServerMessage Message2 = new ServerMessage(129u);
								Message2.AppendStringWithBreak(class4.Type.ToString());
								Message2.AppendInt32(class4.Sprite);
								Message2.AppendStringWithBreak(class4.Name);
								Session.SendMessage(Message2);
								using (DatabaseClient class3 = HabboIM.GetDatabase().GetClient())
								{
									class3.ExecuteQuery("DELETE FROM user_presents WHERE item_id = '" + class2.uint_0 + "' LIMIT 1");
								}
								HabboIM.GetGame().GetCatalog().method_9(Session, class4, (int)dataRow["amount"], (string)dataRow["extra_data"], true, 0u);
                                Console.Write("TEST");
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
