using System;
using System.Data;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Items;
using HabboIM.Storage;
namespace HabboIM.Communication.Messages.Marketplace
{
	internal sealed class BuyOfferMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			DataRow dataRow = null;
			using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
			{
				dataRow = @class.ReadDataRow("SELECT state, timestamp, total_price, extra_data, item_id, furni_id FROM catalog_marketplace_offers WHERE offer_id = '" + num + "' LIMIT 1");
			}
			if (dataRow == null || (string)dataRow["state"] != "1" || (double)dataRow["timestamp"] <= HabboIM.GetGame().GetCatalog().method_22().method_3())
			{
				Session.SendNotification(HabboIMEnvironment.GetExternalText("marketplace_error_expired"));
			}
			else
			{
				Item class2 = HabboIM.GetGame().GetItemManager().method_2((uint)dataRow["item_id"]);
				if (class2 != null)
				{
					if ((int)dataRow["total_price"] >= 1)
					{
						if (Session.GetHabbo().Credits < (int)dataRow["total_price"])
						{
							Session.SendNotification(HabboIMEnvironment.GetExternalText("marketplace_error_credits"));
							return;
						}
						Session.GetHabbo().Credits -= (int)dataRow["total_price"];
						Session.GetHabbo().UpdateCredits(true);
					}
					HabboIM.GetGame().GetCatalog().method_9(Session, class2, 1, (string)dataRow["extra_data"], false, (uint)dataRow["furni_id"]);
					using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
					{
						@class.ExecuteQuery("UPDATE catalog_marketplace_offers SET state = '2' WHERE offer_id = '" + num + "' LIMIT 1");
						int num2 = 0;
						try
						{
							num2 = @class.ReadInt32("SELECT Id FROM catalog_marketplace_data WHERE daysago = 0 AND sprite = " + class2.Sprite + " LIMIT 1;");
						}
						catch
						{
						}
						if (num2 > 0)
						{
							@class.ExecuteQuery(string.Concat(new object[]
							{
								"UPDATE catalog_marketplace_data SET sold = sold + 1, avgprice = (avgprice + ",
								(int)dataRow["total_price"],
								") WHERE Id = ",
								num2,
								" LIMIT 1;"
							}));
						}
						else
						{
							@class.ExecuteQuery(string.Concat(new object[]
							{
								"INSERT INTO catalog_marketplace_data (sprite, sold, avgprice, daysago) VALUES ('",
								class2.Sprite,
								"', 1, ",
								(int)dataRow["total_price"],
								", 0)"
							}));
						}
						if (HabboIM.GetGame().GetCatalog().method_22().dictionary_0.ContainsKey(class2.Sprite) && HabboIM.GetGame().GetCatalog().method_22().dictionary_1.ContainsKey(class2.Sprite))
						{
							int num3 = HabboIM.GetGame().GetCatalog().method_22().dictionary_1[class2.Sprite];
							int num4 = HabboIM.GetGame().GetCatalog().method_22().dictionary_0[class2.Sprite];
							num4 += (int)dataRow["total_price"];
							HabboIM.GetGame().GetCatalog().method_22().dictionary_0.Remove(class2.Sprite);
							HabboIM.GetGame().GetCatalog().method_22().dictionary_0.Add(class2.Sprite, num4);
							HabboIM.GetGame().GetCatalog().method_22().dictionary_1.Remove(class2.Sprite);
							HabboIM.GetGame().GetCatalog().method_22().dictionary_1.Add(class2.Sprite, num3 + 1);
						}
						else
						{
							if (!HabboIM.GetGame().GetCatalog().method_22().dictionary_0.ContainsKey(class2.Sprite))
							{
								HabboIM.GetGame().GetCatalog().method_22().dictionary_0.Add(class2.Sprite, (int)dataRow["total_price"]);
							}
							if (!HabboIM.GetGame().GetCatalog().method_22().dictionary_1.ContainsKey(class2.Sprite))
							{
								HabboIM.GetGame().GetCatalog().method_22().dictionary_1.Add(class2.Sprite, 1);
							}
						}
					}
					ServerMessage Message = new ServerMessage(67u);
					Message.AppendUInt(class2.UInt32_0);
					Message.AppendStringWithBreak(class2.Name);
					Message.AppendInt32((int)dataRow["total_price"]);
					Message.AppendInt32(0);
					Message.AppendInt32(0);
					Message.AppendInt32(1);
					Message.AppendStringWithBreak(class2.Type.ToString());
					Message.AppendInt32(class2.Sprite);
					Message.AppendStringWithBreak("");
					Message.AppendInt32(1);
					Message.AppendInt32(-1);
					Message.AppendStringWithBreak("");
					Session.SendMessage(Message);
					Session.SendMessage(HabboIM.GetGame().GetCatalog().method_22().method_5(-1, -1, "", 1));
				}
			}
		}
	}
}
