using HabboIM;
using HabboIM.Catalogs;
using HabboIM.Core;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Items;
using HabboIM.HabboHotel.Misc;
using HabboIM.HabboHotel.Pets;
using HabboIM.Messages;
using HabboIM.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace HabboIM.HabboHotel.Catalogs
{
    internal sealed class Catalog
    {
        private uint uint_0 = 0;
        private readonly object object_0 = new object();
        public Dictionary<int, CatalogPage> dictionary_0;
        public List<EcotronReward> list_0;
        private VoucherHandler VoucherHandler_0;
        private Marketplace class43_0;
        private ServerMessage[] Message5_0;

        public Catalog()
        {
            this.VoucherHandler_0 = new VoucherHandler();
            this.class43_0 = new Marketplace();
        }

        public void method_0(DatabaseClient class6_0)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Logging.Write("Lädt Katalog..");
            this.dictionary_0 = new Dictionary<int, CatalogPage>();
            this.list_0 = new List<EcotronReward>();
            DataTable dataTable1 = class6_0.ReadDataTable("SELECT * FROM catalog_pages WHERE order_num >= '0' ORDER BY order_num ASC", 30);
            DataTable dataTable2 = class6_0.ReadDataTable("SELECT * FROM ecotron_rewards ORDER BY item_id", 30);
            DataTable dataTable3 = class6_0.ReadDataTable("SELECT * FROM catalog_pages WHERE order_num = '-1' ORDER BY caption ASC", 30);
            try
            {
                this.uint_0 = (uint)class6_0.ReadDataRow("SELECT ID FROM items ORDER BY ID DESC LIMIT 1", 30)[0];
            }
            catch
            {
                this.uint_0 = 0U;
            }
            ++this.uint_0;
            Hashtable hashtable_0 = new Hashtable();
            DataTable dataTable4 = class6_0.ReadDataTable("SELECT * FROM catalog_items", 30);
            if (dataTable4 != null)
            {
                foreach (DataRow dataRow in (InternalDataCollectionBase)dataTable4.Rows)
                {
                    if (!(dataRow["item_ids"].ToString() == "") && (int)dataRow["amount"] > 0)
                    {
                        string BadgeID = dataRow["BadgeID"].ToString();
                        if (string.IsNullOrEmpty(BadgeID) || string.IsNullOrWhiteSpace(BadgeID))
                            BadgeID = string.Empty;
                        hashtable_0.Add((object)(uint)dataRow["Id"], (object)new CatalogItem((uint)dataRow["Id"], (string)dataRow["catalog_name"], (string)dataRow["item_ids"], (int)dataRow["cost_credits"], (int)dataRow["cost_pixels"], (int)dataRow["cost_snow"], (int)dataRow["amount"], (int)dataRow["page_id"], HabboIM.StringToInt(dataRow["vip"].ToString()), (uint)dataRow["achievement"], (int)dataRow["song_id"], BadgeID, (int)dataRow["limited_sold"], (int)dataRow["limited_count"]));
                    }
                }
            }
            if (dataTable1 != null)
            {
                foreach (DataRow dataRow in (InternalDataCollectionBase)dataTable1.Rows)
                {
                    bool bool_3 = false;
                    bool bool_4 = false;
                    if (dataRow["visible"].ToString() == "1")
                        bool_3 = true;
                    if (dataRow["enabled"].ToString() == "1")
                        bool_4 = true;
                    this.dictionary_0.Add((int)dataRow["Id"], new CatalogPage((int)dataRow["Id"], (int)dataRow["parent_id"], (string)dataRow["caption"], bool_3, bool_4, (uint)dataRow["min_rank"], HabboIM.StringToBoolean(dataRow["club_only"].ToString()), (int)dataRow["icon_color"], (int)dataRow["icon_image"], (string)dataRow["page_layout"], (string)dataRow["page_headline"], (string)dataRow["page_teaser"], (string)dataRow["page_special"], (string)dataRow["page_text1"], (string)dataRow["page_text2"], (string)dataRow["page_text_details"], (string)dataRow["page_text_teaser"], (string)dataRow["page_link_description"], (string)dataRow["page_link_pagename"], ref hashtable_0));
                }
            }
            if (dataTable3 != null)
            {
                foreach (DataRow dataRow in (InternalDataCollectionBase)dataTable3.Rows)
                {
                    bool bool_3 = false;
                    bool bool_4 = false;
                    if (dataRow["visible"].ToString() == "1")
                        bool_3 = true;
                    if (dataRow["enabled"].ToString() == "1")
                        bool_4 = true;
                    this.dictionary_0.Add((int)dataRow["Id"], new CatalogPage((int)dataRow["Id"], (int)dataRow["parent_id"], (string)dataRow["caption"], bool_3, bool_4, (uint)dataRow["min_rank"], HabboIM.StringToBoolean(dataRow["club_only"].ToString()), (int)dataRow["icon_color"], (int)dataRow["icon_image"], (string)dataRow["page_layout"], (string)dataRow["page_headline"], (string)dataRow["page_teaser"], (string)dataRow["page_special"], (string)dataRow["page_text1"], (string)dataRow["page_text2"], (string)dataRow["page_text_details"], (string)dataRow["page_text_teaser"], (string)dataRow["page_link_description"], (string)dataRow["page_link_pagename"], ref hashtable_0));
                }
            }
            if (dataTable2 != null)
            {
                foreach (DataRow dataRow in (InternalDataCollectionBase)dataTable2.Rows)
                    this.list_0.Add(new EcotronReward((uint)dataRow["Id"], (uint)dataRow["display_id"], (uint)dataRow["item_id"], (uint)dataRow["reward_level"]));
            }
            Logging.WriteLine("Fertig!", ConsoleColor.Green);
        }

        private string ConvertToTimestamp(DateTime value)
        {
            return (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()).TotalSeconds.ToString();
        }

        public static DateTime UnixTimeStampToDateTime(string unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToDouble(unixTimeStamp)).ToLocalTime();
        }

        internal void method_1()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Logging.Write("Lädt Katalog Cache..");
            int length = HabboIM.GetGame().GetRoleManager().dictionary_2.Count + 1;
            this.Message5_0 = new ServerMessage[length];
            for (int int_0 = 1; int_0 < length; ++int_0)
                this.Message5_0[int_0] = this.method_17(int_0);
            foreach (CatalogPage catalogPage in this.dictionary_0.Values)
                catalogPage.method_0();
            Logging.WriteLine("Fertig!", ConsoleColor.Green);
        }

        public CatalogItem method_2(uint uint_1)
        {
            foreach (CatalogPage catalogPage in this.dictionary_0.Values)
            {
                foreach (CatalogItem catalogItem in catalogPage.list_0)
                {
                    if ((int)catalogItem.uint_0 == (int)uint_1)
                        return catalogItem;
                }
            }
            return (CatalogItem)null;
        }

        public bool method_3(uint uint_1)
        {
            DataRow dataRow = (DataRow)null;
            using (DatabaseClient client = HabboIM.GetDatabase().GetClient())
                dataRow = client.ReadDataRow("SELECT Id FROM catalog_items WHERE item_ids = '" + (object)uint_1 + "' LIMIT 1", 30);
            return dataRow != null;
        }

        public int method_4(int int_0, int int_1)
        {
            int num = 0;
            foreach (CatalogPage catalogPage in this.dictionary_0.Values)
            {
                if ((ulong)catalogPage.uint_0 <= (ulong)int_0 && catalogPage.int_1 == int_1)
                    ++num;
            }
            return num;
        }

        public CatalogPage method_5(int int_0)
        {
            if (!this.dictionary_0.ContainsKey(int_0))
                return (CatalogPage)null;
            return this.dictionary_0[int_0];
        }
        public bool method_6(GameClient Session, int int_0, uint uint_1, string string_0, bool bool_0, string string_1, string string_2, bool bool_1)
        {
            int num1 = 0;
            int num2 = 0;
            CatalogPage catalogPage = this.method_5(int_0);
            if (catalogPage == null || !catalogPage.bool_1 || !catalogPage.bool_0 || catalogPage.uint_0 > Session.GetHabbo().Rank || catalogPage.bool_2 && (!Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club") || !Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_vip")))
                return false;
            CatalogItem catalogItem = catalogPage.method_1(uint_1);
            if (catalogItem == null)
                return false;
            uint id = 0;
            if (bool_0)
            {

                if (Session.GetHabbo().jail == 1)
                {
                    Session.SendNotification("Du bist gebannt und kannst keine Geschenke versenden!");
                    return false;
                }

                if (!catalogItem.method_0().AllowGift)
                    return false;
                if (Session.GetHabbo().method_4() > 0)
                {
                    TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().dateTime_0;
                    if (timeSpan.Seconds > 4)
                        Session.GetHabbo().int_23 = 0;
                    if (timeSpan.Seconds < 4 && Session.GetHabbo().int_23 > 3)
                    {
                        Session.GetHabbo().bool_15 = true;
                        return false;
                    }
                    if (Session.GetHabbo().bool_15 && timeSpan.Seconds < Session.GetHabbo().method_4())
                        return false;
                    Session.GetHabbo().bool_15 = false;
                    Session.GetHabbo().dateTime_0 = DateTime.Now;
                    ++Session.GetHabbo().int_23;
                }
                using (DatabaseClient client = HabboIM.GetDatabase().GetClient())
                {
                    client.AddParamWithValue("gift_user", (object)string_1);
                    try
                    {
                        id = (uint)client.ReadDataRow("SELECT Id FROM users WHERE username = @gift_user LIMIT 1", 30)[0];
                    }
                    catch (Exception ex)
                    {
                    }
                }
                if ((int)id == 0)
                {
                    ServerMessage Message5_0 = new ServerMessage(76U);
                    Message5_0.AppendBoolean(true);
                    Message5_0.AppendStringWithBreak(string_1);
                    Session.SendMessage(Message5_0);
                    return false;
                }
            }
            if (catalogItem.IsLimited)
            {
                if (catalogItem.LimitedSold >= catalogItem.LimitedCount)
                {
                    Session.SendNotification("Dieses Möbelstück war limitiert und ist leider ausverkauft.");
                    return false;
                }
                ++catalogItem.LimitedSold;
                using (DatabaseClient client = HabboIM.GetDatabase().GetClient())
                    client.ExecuteQuery(string.Concat(new object[4]
          {
            (object) "UPDATE catalog_items SET limited_sold = ",
            (object) catalogItem.LimitedSold,
            (object) " WHERE id = ",
            (object) catalogItem.uint_0
          }), 30);
                num2 = catalogItem.LimitedSold;
                num1 = catalogItem.LimitedCount;
                catalogPage.method_0();
                Session.SendMessage(catalogPage.message5_0);
            }
            bool Bool1 = false;
            bool Bool2 = false;
            int num3 = catalogItem.int_2;
            if (Session.GetHabbo().Credits < catalogItem.int_0)
                Bool1 = true;
            if (num3 == 0 && Session.GetHabbo().ActivityPoints < catalogItem.int_1 || num3 > 0 && Session.GetHabbo().VipPoints < catalogItem.int_1)
                Bool2 = true;
            if (Bool1 || Bool2)
            {
                ServerMessage Message5_0 = new ServerMessage(68U);
                Message5_0.AppendBoolean(Bool1);
                Message5_0.AppendBoolean(Bool2);
                Session.SendMessage(Message5_0);
                return false;
            }
            if (bool_0 && (int)catalogItem.method_0().Type == 101)
            {
                Session.SendNotification("Du kannst diese Item nicht als Geschenk kaufen.");
                return false;
            }
            switch (catalogItem.method_0().InteractionType.ToLower())
            {
                case "pet":
                    try
                    {
                        string[] strArray = string_0.Split('\n');
                        string string_0_1 = strArray[0];
                        string s = strArray[1];
                        string str = strArray[2];
                        int.Parse(s);
                        if (!this.method_8(string_0_1) || s.Length > 2)
                            return false;
                        if (str.Length != 6)
                            return false;
                        break;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                case "roomeffect":
                    double num4 = 0.0;
                    try
                    {
                        num4 = double.Parse(string_0);
                    }
                    catch (Exception ex)
                    {
                    }
                    string_0 = num4.ToString().Replace(',', '.');
                    break;
                case "postit":
                    string_0 = "FFFF33";
                    break;
                case "dimmer":
                    string_0 = "1,1,1,#000000,255";
                    break;
                case "trophy":
                    string_0 = Session.GetHabbo().Username + (object)Convert.ToChar(9) + (object)DateTime.Now.Day + "-" + (object)DateTime.Now.Month + "-" + (object)DateTime.Now.Year + (object)Convert.ToChar(9) + ChatCommandHandler.smethod_4(HabboIM.DoFilter(string_0, true, true));
                    break;
                case "musicdisc":
                    string_0 = catalogItem.song_id.ToString();
                    break;
                default:
                    if (catalogItem.string_0.StartsWith("disc_"))
                    {
                        string_0 = catalogItem.string_0.Split('_')[1];
                        break;
                    }
                    string_0 = "";
                    break;
            }
            if (catalogItem.int_0 > 0)
            {
                Session.GetHabbo().Credits -= catalogItem.int_0;
                Session.GetHabbo().UpdateCredits(true);
            }
            if (catalogItem.int_1 > 0 && num3 == 0)
            {
                Session.GetHabbo().ActivityPoints -= catalogItem.int_1;
                Session.GetHabbo().UpdateActivityPoints(true);
            }
            else if (catalogItem.int_1 > 0 && num3 > 0)
            {
                Session.GetHabbo().VipPoints -= catalogItem.int_1;
                Session.GetHabbo().method_16(0);
                Session.GetHabbo().UpdateVipPoints(false, true);
            }
            ServerMessage Message5_0_1 = new ServerMessage(67U);
            Message5_0_1.AppendUInt(catalogItem.method_0().UInt32_0);
            Message5_0_1.AppendStringWithBreak(catalogItem.method_0().Name);
            Message5_0_1.AppendInt32(catalogItem.int_0);
            Message5_0_1.AppendInt32(catalogItem.int_1);
            Message5_0_1.AppendInt32(catalogItem.int_2);
            if (bool_1)
                Message5_0_1.AppendInt32(1);
            else
                Message5_0_1.AppendInt32(0);
            Message5_0_1.AppendStringWithBreak(catalogItem.method_0().Type.ToString());
            Message5_0_1.AppendInt32(catalogItem.method_0().Sprite);
            Message5_0_1.AppendStringWithBreak("");
            Message5_0_1.AppendInt32(1);
            Message5_0_1.AppendInt32(-1);
            Message5_0_1.AppendStringWithBreak("");
            Session.SendMessage(Message5_0_1);
            if (bool_0)
            {
                uint num5 = this.method_14();
                Item obj = this.method_10();
                using (DatabaseClient client = HabboIM.GetDatabase().GetClient())
                {
                    client.AddParamWithValue("gift_message", (object)("!" + ChatCommandHandler.smethod_4(HabboIM.DoFilter(string_2, true, true)) + " - " + Session.GetHabbo().Username));
                    client.AddParamWithValue("extra_data", (object)string_0);
                    client.ExecuteQuery("INSERT INTO items (Id,user_id,base_item,wall_pos) VALUES ('" + (object)num5 + "','" + (object)id + "','" + (object)obj.UInt32_0 + "','')", 30);
                    client.ExecuteQuery(string.Concat(new object[3]
          {
            (object) "INSERT INTO items_extra_data (item_id,extra_data) VALUES ('",
            (object) num5,
            (object) "',@gift_message)"
          }), 30);
                    client.ExecuteQuery("INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES ('" + (object)num5 + "','" + (object)catalogItem.method_0().UInt32_0 + "','" + (object)catalogItem.int_3 + "',@extra_data)", 30);
                }
                GameClient gameClient = HabboIM.GetGame().GetClientManager().method_2(id);
                if (gameClient != null)
                {
                    gameClient.SendNotification("Du hast ein Geschenk bekommen! Überprüfe dein Inventar.");
                    gameClient.GetHabbo().GetInventoryComponent().method_9(true);
                    ++gameClient.GetHabbo().GiftsReceived;
                    gameClient.GetHabbo().CheckGiftReceivedAchievements();
                }
                ++Session.GetHabbo().GiftsGiven;
                Session.GetHabbo().CheckGiftGivenAchievements();
                Session.SendNotification("Geschenk erfolgreich gesendet.");
                return true;
            }
            this.method_9(Session, catalogItem.method_0(), catalogItem.int_3, string_0, true, 0U);
            if (catalogItem.uint_2 > 0U)
                HabboIM.GetGame().GetAchievementManager().addAchievement(Session, catalogItem.uint_2, 1);
            if (!string.IsNullOrEmpty(catalogItem.BadgeID))
                Session.GetHabbo().GetBadgeComponent().SendBadge(Session, catalogItem.BadgeID, true);
            return true;
        }

        public void method_7(string string_0, uint uint_1, uint uint_2, int int_0)
        {
            CatalogItem catalogItem = this.method_5(int_0).method_1(uint_2);
            uint num = this.method_14();
            Item obj = this.method_10();
            using (DatabaseClient client = HabboIM.GetDatabase().GetClient())
            {
                client.AddParamWithValue("gift_message", (object)("!" + ChatCommandHandler.smethod_4(HabboIM.DoFilter(string_0, true, true))));
                client.ExecuteQuery("INSERT INTO items (Id,user_id,base_item,wall_pos) VALUES ('" + (object)num + "','" + (object)uint_1 + "','" + (object)obj.UInt32_0 + "','')", 30);
                client.ExecuteQuery(string.Concat(new object[3]
        {
          (object) "INSERT INTO items_extra_data (item_id,extra_data) VALUES ('",
          (object) num,
          (object) "',@gift_message)"
        }), 30);
                client.ExecuteQuery("INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES ('" + (object)num + "','" + (object)catalogItem.method_0().UInt32_0 + "','" + (object)catalogItem.int_3 + "','')", 30);
            }
            GameClient gameClient = HabboIM.GetGame().GetClientManager().method_2(uint_1);
            if (gameClient == null)
                return;
            gameClient.SendNotification("Du hast ein Geschenk in Inventar erhalten!");
            gameClient.GetHabbo().GetInventoryComponent().method_9(true);
        }

        public bool method_8(string string_0)
        {
            return string_0.Length >= 1 && string_0.Length <= 16 && HabboIM.smethod_9(string_0) && !(string_0 != ChatCommandHandler.smethod_4(string_0));
        }

        public void method_9(GameClient Session, Item Item, int int_0, string string_0, bool bool_0, uint uint_1)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;
            string str = Item.Type.ToString();
            if (str != null)
            {
                if (str == "i" || str == "s")
                {
                    for (int index = 0; index < int_0; ++index)
                    {
                        uint num = bool_0 || uint_1 <= 0U ? this.method_14() : uint_1;
                        switch (Item.InteractionType.ToLower())
                        {
                            case "pet":
                                string[] strArray = string_0.Split('\n');
                                Pet pet = this.method_11(Session.GetHabbo().Id, strArray[0], Convert.ToInt32(Item.Name.Split('t')[1]), strArray[1], strArray[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet);
                                Session.GetHabbo().GetInventoryComponent().method_11(num, 320U, "0", bool_0);
                                break;
                            case "teleport":
                                uint uint_1_1 = this.method_14();
                                using (DatabaseClient client = HabboIM.GetDatabase().GetClient())
                                {
                                    client.ExecuteQuery("INSERT INTO tele_links (tele_one_id,tele_two_id) VALUES ('" + (object)num + "','" + (object)uint_1_1 + "')", 30);
                                    client.ExecuteQuery("INSERT INTO tele_links (tele_one_id,tele_two_id) VALUES ('" + (object)uint_1_1 + "','" + (object)num + "')", 30);
                                }
                                Session.GetHabbo().GetInventoryComponent().method_11(uint_1_1, Item.UInt32_0, "0", bool_0);
                                Session.GetHabbo().GetInventoryComponent().method_11(num, Item.UInt32_0, "0", bool_0);
                                break;
                            case "dimmer":
                                using (DatabaseClient client = HabboIM.GetDatabase().GetClient())
                                    client.ExecuteQuery("INSERT INTO room_items_moodlight (item_id,enabled,current_preset,preset_one,preset_two,preset_three) VALUES ('" + (object)num + "','0','1','#000000,255,0','#000000,255,0','#000000,255,0')", 30);
                                Session.GetHabbo().GetInventoryComponent().method_11(num, Item.UInt32_0, string_0, bool_0);
                                break;
                            default:
                                Session.GetHabbo().GetInventoryComponent().method_11(num, Item.UInt32_0, string_0, bool_0);
                                break;
                        }
                        ServerMessage Message5_0 = new ServerMessage(832U);
                        Message5_0.AppendInt32(1);
                        if (Item.InteractionType.ToLower() == "pet")
                        {
                            Message5_0.AppendInt32(3);
                            ++Session.GetHabbo().NewPetsBuyed;
                            Session.GetHabbo().CheckPetCountAchievements();
                        }
                        else if (Item.Type.ToString() == "i")
                            Message5_0.AppendInt32(2);
                        else
                            Message5_0.AppendInt32(1);
                        Message5_0.AppendInt32(1);
                        Message5_0.AppendUInt(num);
                        Session.SendMessage(Message5_0);
                    }
                    Session.GetHabbo().GetInventoryComponent().method_9(false);
                    return;
                }
                if (str == "e")
                {
                    for (int index = 0; index < int_0; ++index)
                        Session.GetHabbo().GetEffectsInventoryComponent().method_0(Item.Sprite, 3600);
                    return;
                }
                if (str == "h")
                {
                    for (int index = 0; index < int_0; ++index)
                    {
                        Session.GetHabbo().GetSubscriptionManager().method_3("habbo_club", 2678400);
                        Session.GetHabbo().CheckHCAchievements();
                    }
                    ServerMessage Message5_0_1 = new ServerMessage(7U);
                    Message5_0_1.AppendStringWithBreak("habbo_club");
                    if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                    {
                        int num = (int)Math.Ceiling(((double)Session.GetHabbo().GetSubscriptionManager().GetSubscriptionByType("habbo_club").ExpirationTime - HabboIM.GetUnixTimestamp()) / 86400.0);
                        int i = num / 31;
                        if (i >= 1)
                            --i;
                        Message5_0_1.AppendInt32(num - i * 31);
                        Message5_0_1.AppendBoolean(true);
                        Message5_0_1.AppendInt32(i);
                    }
                    else
                    {
                        for (int index = 0; index < 3; ++index)
                            Message5_0_1.AppendInt32(0);
                    }
                    Session.SendMessage(Message5_0_1);
                    ServerMessage Message5_0_2 = new ServerMessage(2U);
                    if (Session.GetHabbo().IsVIP || ServerConfiguration.HabboClubForClothes)
                        Message5_0_2.AppendInt32(2);
                    else if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                        Message5_0_2.AppendInt32(1);
                    else
                        Message5_0_2.AppendInt32(0);
                    if (Session.GetHabbo().HasFuse("acc_anyroomowner"))
                        Message5_0_2.AppendInt32(7);
                    else if (Session.GetHabbo().HasFuse("acc_anyroomrights"))
                        Message5_0_2.AppendInt32(5);
                    else if (Session.GetHabbo().HasFuse("acc_supporttool"))
                        Message5_0_2.AppendInt32(4);
                    else if (Session.GetHabbo().IsVIP || ServerConfiguration.HabboClubForClothes || Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                        Message5_0_2.AppendInt32(2);
                    else
                        Message5_0_2.AppendInt32(0);
                    Session.SendMessage(Message5_0_2);
                    return;
                }
            }
            Session.SendNotification("Something went wrong! The item type could not be processed. Please do not try to buy this item anymore, instead inform support as soon as possible.");
        }

        public Item method_10()
        {
            switch (HabboIM.smethod_5(0, 6))
            {
                case 0:
                    return HabboIM.GetGame().GetItemManager().method_2(164U);
                case 1:
                    return HabboIM.GetGame().GetItemManager().method_2(165U);
                case 2:
                    return HabboIM.GetGame().GetItemManager().method_2(166U);
                case 3:
                    return HabboIM.GetGame().GetItemManager().method_2(167U);
                case 4:
                    return HabboIM.GetGame().GetItemManager().method_2(168U);
                case 5:
                    return HabboIM.GetGame().GetItemManager().method_2(169U);
                case 6:
                    return HabboIM.GetGame().GetItemManager().method_2(170U);
                default:
                    return (Item)null;
            }
        }

        public Pet method_11(uint uint_1, string string_0, int int_0, string string_1, string string_2)
        {
            return new Pet(this.method_14(), uint_1, 0U, string_0, (uint)int_0, string_1, string_2, 0, 100, 100, 0, HabboIM.GetUnixTimestamp(), 0, 0, 0.0)
            {
                DBState = DatabaseUpdateState.NeedsInsert
            };
        }

        public Pet method_12(DataRow dataRow_0)
        {
            if (dataRow_0 == null)
                return (Pet)null;
            return new Pet((uint)dataRow_0["Id"], (uint)dataRow_0["user_id"], (uint)dataRow_0["room_id"], (string)dataRow_0["name"], (uint)dataRow_0["type"], (string)dataRow_0["race"], (string)dataRow_0["color"], (int)dataRow_0["expirience"], (int)dataRow_0["energy"], (int)dataRow_0["nutrition"], (int)dataRow_0["respect"], (double)dataRow_0["createstamp"], (int)dataRow_0["x"], (int)dataRow_0["y"], (double)dataRow_0["z"]);
        }

        internal Pet method_13(DataRow dataRow_0, uint uint_1)
        {
            if (dataRow_0 == null)
                return (Pet)null;
            return new Pet(uint_1, (uint)dataRow_0["user_id"], (uint)dataRow_0["room_id"], (string)dataRow_0["name"], (uint)dataRow_0["type"], (string)dataRow_0["race"], (string)dataRow_0["color"], (int)dataRow_0["expirience"], (int)dataRow_0["energy"], (int)dataRow_0["nutrition"], (int)dataRow_0["respect"], (double)dataRow_0["createstamp"], (int)dataRow_0["x"], (int)dataRow_0["y"], (double)dataRow_0["z"]);
        }

        internal uint method_14()
        {
            string abcde = "dev";
            //HabboIM.GetWebSocketManager().SendMessageToEveryConnection(abcde);
            lock (this.object_0)
                return this.uint_0++;
        }

        public EcotronReward method_15()
        {
            uint uint_1 = 1;
            if (HabboIM.smethod_5(1, 2000) == 2000)
                uint_1 = 5U;
            else if (HabboIM.smethod_5(1, 200) == 200)
                uint_1 = 4U;
            else if (HabboIM.smethod_5(1, 40) == 40)
                uint_1 = 3U;
            else if (HabboIM.smethod_5(1, 4) == 4)
                uint_1 = 2U;
            List<EcotronReward> list = this.method_16(uint_1);
            if (list != null && list.Count >= 1)
                return list[HabboIM.smethod_5(0, list.Count - 1)];
            return new EcotronReward(0U, 0U, 1479U, 0U);
        }

        public List<EcotronReward> method_16(uint uint_1)
        {
            List<EcotronReward> list = new List<EcotronReward>();
            foreach (EcotronReward ecotronReward in this.list_0)
            {
                if ((int)ecotronReward.uint_3 == (int)uint_1)
                    list.Add(ecotronReward);
            }
            return list;
        }

        public ServerMessage method_17(int int_0)
        {
            ServerMessage Message5_1 = new ServerMessage(126U);
            Message5_1.AppendBoolean(true);
            Message5_1.AppendInt32(0);
            Message5_1.AppendInt32(0);
            Message5_1.AppendInt32(-1);
            Message5_1.AppendStringWithBreak("");
            Message5_1.AppendInt32(this.method_4(int_0, -1));
            Message5_1.AppendBoolean(true);
            foreach (CatalogPage catalogPage1 in this.dictionary_0.Values)
            {
                if (catalogPage1.int_1 == -1)
                {
                    catalogPage1.method_2(int_0, Message5_1);
                    foreach (CatalogPage catalogPage2 in this.dictionary_0.Values)
                    {
                        if (catalogPage2.int_1 == catalogPage1.Int32_0)
                            catalogPage2.method_2(int_0, Message5_1);
                    }
                }
            }
            return Message5_1;
        }

        internal ServerMessage method_18(uint uint_1)
        {
            if (uint_1 < 1U)
                uint_1 = 1U;
            if ((ulong)uint_1 > (ulong)HabboIM.GetGame().GetRoleManager().dictionary_2.Count)
                uint_1 = (uint)HabboIM.GetGame().GetRoleManager().dictionary_2.Count;
            return this.Message5_0[(int)(uint)(UIntPtr)uint_1];
        }

        public ServerMessage method_19(CatalogPage class48_0)
        {
            ServerMessage Message5_0 = new ServerMessage((uint)sbyte.MaxValue);
            Message5_0.AppendInt32(class48_0.Int32_0);
            switch (class48_0.string_1)
            {
                case "frontpage":
                    Message5_0.AppendStringWithBreak("frontpage3");
                    Message5_0.AppendInt32(3);
                    Message5_0.AppendStringWithBreak(class48_0.string_2);
                    Message5_0.AppendStringWithBreak(class48_0.string_3);
                    Message5_0.AppendStringWithBreak("");
                    Message5_0.AppendInt32(11);
                    Message5_0.AppendStringWithBreak(class48_0.string_5);
                    Message5_0.AppendStringWithBreak(class48_0.string_9);
                    Message5_0.AppendStringWithBreak(class48_0.string_6);
                    Message5_0.AppendStringWithBreak(class48_0.string_7);
                    Message5_0.AppendStringWithBreak(class48_0.string_10);
                    Message5_0.AppendStringWithBreak("#FAF8CC");
                    Message5_0.AppendStringWithBreak("#FAF8CC");
                    Message5_0.AppendStringWithBreak("Liest mehr >");
                    Message5_0.AppendStringWithBreak("magic.credits");
                    break;
                case "recycler_info":
                    Message5_0.AppendStringWithBreak(class48_0.string_1);
                    Message5_0.AppendInt32(2);
                    Message5_0.AppendStringWithBreak(class48_0.string_2);
                    Message5_0.AppendStringWithBreak(class48_0.string_3);
                    Message5_0.AppendInt32(3);
                    Message5_0.AppendStringWithBreak(class48_0.string_5);
                    Message5_0.AppendStringWithBreak(class48_0.string_6);
                    Message5_0.AppendStringWithBreak(class48_0.string_7);
                    break;
                case "recycler_prizes":
                    Message5_0.AppendStringWithBreak("recycler_prizes");
                    Message5_0.AppendInt32(1);
                    Message5_0.AppendStringWithBreak("catalog_recycler_headline3");
                    Message5_0.AppendInt32(1);
                    Message5_0.AppendStringWithBreak(class48_0.string_5);
                    break;
                case "spaces":
                    Message5_0.AppendStringWithBreak("spaces_new");
                    Message5_0.AppendInt32(1);
                    Message5_0.AppendStringWithBreak(class48_0.string_2);
                    Message5_0.AppendInt32(1);
                    Message5_0.AppendStringWithBreak(class48_0.string_5);
                    break;
                case "recycler":
                    Message5_0.AppendStringWithBreak(class48_0.string_1);
                    Message5_0.AppendInt32(2);
                    Message5_0.AppendStringWithBreak(class48_0.string_2);
                    Message5_0.AppendStringWithBreak(class48_0.string_3);
                    Message5_0.AppendInt32(1);
                    Message5_0.AppendStringWithBreak(class48_0.string_5, (byte)10);
                    Message5_0.AppendStringWithBreak(class48_0.string_6);
                    Message5_0.AppendStringWithBreak(class48_0.string_7);
                    break;
                case "trophies":
                    Message5_0.AppendStringWithBreak("trophies");
                    Message5_0.AppendInt32(1);
                    Message5_0.AppendStringWithBreak(class48_0.string_2);
                    Message5_0.AppendInt32(2);
                    Message5_0.AppendStringWithBreak(class48_0.string_5);
                    Message5_0.AppendStringWithBreak(class48_0.string_7);
                    break;
                case "pets":
                    Message5_0.AppendStringWithBreak("pets");
                    Message5_0.AppendInt32(2);
                    Message5_0.AppendStringWithBreak(class48_0.string_2);
                    Message5_0.AppendStringWithBreak(class48_0.string_3);
                    Message5_0.AppendInt32(4);
                    Message5_0.AppendStringWithBreak(class48_0.string_5);
                    Message5_0.AppendStringWithBreak("");
                    Message5_0.AppendStringWithBreak("Farbe auswählen:");
                    Message5_0.AppendStringWithBreak("Rasse auswählen:");
                    break;
                case "club_buy":
                    Message5_0.AppendStringWithBreak("club_buy");
                    Message5_0.AppendInt32(1);
                    Message5_0.AppendStringWithBreak("habboclub_2");
                    Message5_0.AppendInt32(1);
                    break;
                case "club_gifts":
                    Message5_0.AppendStringWithBreak("club_gifts");
                    Message5_0.AppendInt32(1);
                    Message5_0.AppendStringWithBreak("habboclub_2");
                    Message5_0.AppendInt32(1);
                    Message5_0.AppendStringWithBreak("");
                    Message5_0.AppendInt32(1);
                    break;
                case "soundmachine":
                    Message5_0.AppendStringWithBreak(class48_0.string_1);
                    Message5_0.AppendInt32(2);
                    Message5_0.AppendStringWithBreak(class48_0.string_2);
                    Message5_0.AppendStringWithBreak(class48_0.string_3);
                    Message5_0.AppendInt32(2);
                    Message5_0.AppendStringWithBreak(class48_0.string_5);
                    Message5_0.AppendStringWithBreak(class48_0.string_7);
                    break;
                default:
                    Message5_0.AppendStringWithBreak(class48_0.string_1);
                    Message5_0.AppendInt32(3);
                    Message5_0.AppendStringWithBreak(class48_0.string_2);
                    Message5_0.AppendStringWithBreak(class48_0.string_3);
                    Message5_0.AppendStringWithBreak(class48_0.string_4);
                    Message5_0.AppendInt32(3);
                    Message5_0.AppendStringWithBreak(class48_0.string_5);
                    Message5_0.AppendStringWithBreak(class48_0.string_7);
                    Message5_0.AppendStringWithBreak(class48_0.string_8);
                    break;
            }
            Message5_0.AppendInt32(class48_0.list_0.Count);
            foreach (CatalogItem catalogItem in class48_0.list_0)
                catalogItem.method_1(Message5_0);
            return Message5_0;
        }

        public ServerMessage method_20()
        {
            return new ServerMessage(625U);
        }

        public VoucherHandler method_21()
        {
            return this.VoucherHandler_0;
        }

        public Marketplace method_22()
        {
            return this.class43_0;
        }
    }
}
