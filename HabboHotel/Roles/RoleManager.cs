using HabboIM.Core;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Storage;
using System;
using System.Collections.Generic;
using System.Data;

namespace HabboIM.HabboHotel.Roles
{
    internal sealed class RoleManager
    {
        private Dictionary<uint, List<string>> dictionary_0;

        private Dictionary<uint, List<string>> dictionary_1;

        public Dictionary<uint, string> dictionary_2;

        private Dictionary<uint, int> dictionary_3;

        public Dictionary<string, int> dictionary_4;

        public Dictionary<string, int> dictionary_5;

        public RoleManager()
        {
            this.dictionary_0 = new Dictionary<uint, List<string>>();
            this.dictionary_1 = new Dictionary<uint, List<string>>();
            this.dictionary_2 = new Dictionary<uint, string>();
            this.dictionary_3 = new Dictionary<uint, int>();
            this.dictionary_4 = new Dictionary<string, int>();
            this.dictionary_5 = new Dictionary<string, int>();
        }

        public void method_0(DatabaseClient class6_0)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Logging.Write(HabboIMEnvironment.GetExternalText("emu_loadroles"));
            this.method_10();
            DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM ranks ORDER BY Id ASC;", 30);
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    this.dictionary_2.Add((uint)dataRow["Id"], dataRow["badgeid"].ToString());
                }
            }
            dataTable = class6_0.ReadDataTable("SELECT * FROM permissions_users ORDER BY userid ASC;", 30);
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    List<string> list = new List<string>();
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_settings"].ToString()))
                    {
                        list.Add("cmd_update_settings");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_bans"].ToString()))
                    {
                        list.Add("cmd_update_bans");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_bots"].ToString()))
                    {
                        list.Add("cmd_update_bots");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_catalogue"].ToString()))
                    {
                        list.Add("cmd_update_catalogue");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_navigator"].ToString()))
                    {
                        list.Add("cmd_update_navigator");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_items"].ToString()))
                    {
                        list.Add("cmd_update_items");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_award"].ToString()))
                    {
                        list.Add("cmd_award");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_coords"].ToString()))
                    {
                        list.Add("cmd_coords");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_override"].ToString()))
                    {
                        list.Add("cmd_override");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_coins"].ToString()))
                    {
                        list.Add("cmd_coins");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_pixels"].ToString()))
                    {
                        list.Add("cmd_pixels");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_ha"].ToString()))
                    {
                        list.Add("cmd_ha");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_hal"].ToString()))
                    {
                        list.Add("cmd_hal");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_freeze"].ToString()))
                    {
                        list.Add("cmd_freeze");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_enable"].ToString()))
                    {
                        list.Add("cmd_enable");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roommute"].ToString()))
                    {
                        list.Add("cmd_roommute");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_setspeed"].ToString()))
                    {
                        list.Add("cmd_setspeed");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_masscredits"].ToString()))
                    {
                        list.Add("cmd_masscredits");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_globalcredits"].ToString()))
                    {
                        list.Add("cmd_globalcredits");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_masspixels"].ToString()))
                    {
                        list.Add("cmd_masspixels");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_globalpixels"].ToString()))
                    {
                        list.Add("cmd_globalpixels");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roombadge"].ToString()))
                    {
                        list.Add("cmd_roombadge");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_massbadge"].ToString()))
                    {
                        list.Add("cmd_massbadge");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_userinfo"].ToString()))
                    {
                        list.Add("cmd_userinfo");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_userinfo_viewip"].ToString()))
                    {
                        list.Add("cmd_userinfo_viewip");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_shutdown"].ToString()))
                    {
                        list.Add("cmd_shutdown");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_givebadge"].ToString()))
                    {
                        list.Add("cmd_givebadge");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_removebadge"].ToString()))
                    {
                        list.Add("cmd_removebadge");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_summon"].ToString()))
                    {
                        list.Add("cmd_summon");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_invisible"].ToString()))
                    {
                        list.Add("cmd_invisible");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_ban"].ToString()))
                    {
                        list.Add("cmd_ban");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_superban"].ToString()))
                    {
                        list.Add("cmd_superban");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roomkick"].ToString()))
                    {
                        list.Add("cmd_roomkick");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roomalert"].ToString()))
                    {
                        list.Add("cmd_roomalert");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_mute"].ToString()))
                    {
                        list.Add("cmd_mute");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_unmute"].ToString()))
                    {
                        list.Add("cmd_unmute");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_alert"].ToString()))
                    {
                        list.Add("cmd_alert");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_motd"].ToString()))
                    {
                        list.Add("cmd_motd");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_kick"].ToString()))
                    {
                        list.Add("cmd_kick");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_filter"].ToString()))
                    {
                        list.Add("cmd_update_filter");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_permissions"].ToString()))
                    {
                        list.Add("cmd_update_permissions");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_sa"].ToString()))
                    {
                        list.Add("cmd_sa");
                    }
                    if (HabboIM.StringToBoolean(dataRow["receive_sa"].ToString()))
                    {
                        list.Add("receive_sa");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_ipban"].ToString()))
                    {
                        list.Add("cmd_ipban");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_spull"].ToString()))
                    {
                        list.Add("cmd_spull");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_disconnect"].ToString()))
                    {
                        list.Add("cmd_disconnect");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_achievements"].ToString()))
                    {
                        list.Add("cmd_update_achievements");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_texts"].ToString()))
                    {
                        list.Add("cmd_update_texts");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_teleport"].ToString()))
                    {
                        list.Add("cmd_teleport");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_points"].ToString()))
                    {
                        list.Add("cmd_points");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_masspoints"].ToString()))
                    {
                        list.Add("cmd_masspoints");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_globalpoints"].ToString()))
                    {
                        list.Add("cmd_globalpoints");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_empty"].ToString()))
                    {
                        list.Add("cmd_empty");
                    }
                    if (HabboIM.StringToBoolean(dataRow["ignore_roommute"].ToString()))
                    {
                        list.Add("ignore_roommute");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_anyroomrights"].ToString()))
                    {
                        list.Add("acc_anyroomrights");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_anyroomowner"].ToString()))
                    {
                        list.Add("acc_anyroomowner");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_supporttool"].ToString()))
                    {
                        list.Add("acc_supporttool");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_chatlogs"].ToString()))
                    {
                        list.Add("acc_chatlogs");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_enter_fullrooms"].ToString()))
                    {
                        list.Add("acc_enter_fullrooms");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_enter_anyroom"].ToString()))
                    {
                        list.Add("acc_enter_anyroom");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_restrictedrooms"].ToString()))
                    {
                        list.Add("acc_restrictedrooms");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_unkickable"].ToString()))
                    {
                        list.Add("acc_unkickable");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_unbannable"].ToString()))
                    {
                        list.Add("acc_unbannable");
                    }
                    if (HabboIM.StringToBoolean(dataRow["ignore_friendsettings"].ToString()))
                    {
                        list.Add("ignore_friendsettings");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_sql"].ToString()))
                    {
                        list.Add("wired_give_sql");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_badge"].ToString()))
                    {
                        list.Add("wired_give_badge");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_effect"].ToString()))
                    {
                        list.Add("wired_give_effect");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_award"].ToString()))
                    {
                        list.Add("wired_give_award");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_dance"].ToString()))
                    {
                        list.Add("wired_give_dance");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_send"].ToString()))
                    {
                        list.Add("wired_give_send");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_credits"].ToString()))
                    {
                        list.Add("wired_give_credits");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_pixels"].ToString()))
                    {
                        list.Add("wired_give_pixels");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_points"].ToString()))
                    {
                        list.Add("wired_give_points");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_rank"].ToString()))
                    {
                        list.Add("wired_give_rank");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_respect"].ToString()))
                    {
                        list.Add("wired_give_respect");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_handitem"].ToString()))
                    {
                        list.Add("wired_give_handitem");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_moebel"].ToString()))
                    {
                        list.Add("wired_give_moebel");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_alert"].ToString()))
                    {
                        list.Add("wired_give_alert");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_roomusers"].ToString()))
                    {
                        list.Add("wired_cnd_roomusers");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_userhasachievement"].ToString()))
                    {
                        list.Add("wired_cnd_userhasachievement");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_userhasbadge"].ToString()))
                    {
                        list.Add("wired_cnd_userhasbadge");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_userhasvip"].ToString()))
                    {
                        list.Add("wired_cnd_userhasvip");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_userhaseffect"].ToString()))
                    {
                        list.Add("wired_cnd_userhaseffect");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_userrank"].ToString()))
                    {
                        list.Add("wired_cnd_userrank");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_usercredits"].ToString()))
                    {
                        list.Add("wired_cnd_usercredits");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_userpixels"].ToString()))
                    {
                        list.Add("wired_cnd_userpixels");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_userpoints"].ToString()))
                    {
                        list.Add("wired_cnd_userpoints");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_usergroups"].ToString()))
                    {
                        list.Add("wired_cnd_usergroups");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_wearing"].ToString()))
                    {
                        list.Add("wired_cnd_wearing");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_carrying"].ToString()))
                    {
                        list.Add("wired_cnd_carrying");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_wiredactived"].ToString()))
                    {
                        list.Add("wired_give_wiredactived");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_wiredactived"].ToString()))
                    {
                        list.Add("wired_cnd_wiredactived");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_unlimitedselects"].ToString()))
                    {
                        list.Add("wired_unlimitedselects");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_dance"].ToString()))
                    {
                        list.Add("cmd_dance");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_rave"].ToString()))
                    {
                        list.Add("cmd_rave");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roll"].ToString()))
                    {
                        list.Add("cmd_roll");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_control"].ToString()))
                    {
                        list.Add("cmd_control");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_makesay"].ToString()))
                    {
                        list.Add("cmd_makesay");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_sitdown"].ToString()))
                    {
                        list.Add("cmd_sitdown");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_lay"].ToString()))
                    {
                        list.Add("cmd_lay");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_push"].ToString()))
                    {
                        list.Add("cmd_push");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_pull"].ToString()))
                    {
                        list.Add("cmd_pull");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_flagme"].ToString()))
                    {
                        list.Add("cmd_flagme");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_mimic"].ToString()))
                    {
                        list.Add("cmd_mimic");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_moonwalk"].ToString()))
                    {
                        list.Add("cmd_moonwalk");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_follow"].ToString()))
                    {
                        list.Add("cmd_follow");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_handitem"].ToString()))
                    {
                        list.Add("cmd_handitem");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_startquestion"].ToString()))
                    {
                        list.Add("cmd_startquestion");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_vipha"].ToString()))
                    {
                        list.Add("cmd_vipha");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_spush"].ToString()))
                    {
                        list.Add("cmd_spush");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roomeffect"].ToString()))
                    {
                        list.Add("cmd_roomeffect");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_viphal"].ToString()))
                    {
                        list.Add("cmd_viphal");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_moveotheruserstodoor"].ToString()))
                    {
                        list.Add("acc_moveotheruserstodoor");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_pet"].ToString()))
                    {
                        list.Add("cmd_pet");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roomfreeze"].ToString()))
                    {
                        list.Add("cmd_roomfreeze");
                    }
                    this.dictionary_0.Add((uint)dataRow["userid"], list);
                }
            }
            dataTable = class6_0.ReadDataTable("SELECT * FROM permissions_ranks ORDER BY rank ASC;", 30);
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    this.dictionary_3.Add((uint)dataRow["rank"], (int)dataRow["floodtime"]);
                }
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    List<string> list = new List<string>();
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_settings"].ToString()))
                    {
                        list.Add("cmd_update_settings");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_bans"].ToString()))
                    {
                        list.Add("cmd_update_bans");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_bots"].ToString()))
                    {
                        list.Add("cmd_update_bots");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_catalogue"].ToString()))
                    {
                        list.Add("cmd_update_catalogue");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_navigator"].ToString()))
                    {
                        list.Add("cmd_update_navigator");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_items"].ToString()))
                    {
                        list.Add("cmd_update_items");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_award"].ToString()))
                    {
                        list.Add("cmd_award");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_coords"].ToString()))
                    {
                        list.Add("cmd_coords");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_override"].ToString()))
                    {
                        list.Add("cmd_override");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_coins"].ToString()))
                    {
                        list.Add("cmd_coins");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_pixels"].ToString()))
                    {
                        list.Add("cmd_pixels");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_ha"].ToString()))
                    {
                        list.Add("cmd_ha");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_hal"].ToString()))
                    {
                        list.Add("cmd_hal");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_freeze"].ToString()))
                    {
                        list.Add("cmd_freeze");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_enable"].ToString()))
                    {
                        list.Add("cmd_enable");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roommute"].ToString()))
                    {
                        list.Add("cmd_roommute");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_setspeed"].ToString()))
                    {
                        list.Add("cmd_setspeed");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_masscredits"].ToString()))
                    {
                        list.Add("cmd_masscredits");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_globalcredits"].ToString()))
                    {
                        list.Add("cmd_globalcredits");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_masspixels"].ToString()))
                    {
                        list.Add("cmd_masspixels");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_globalpixels"].ToString()))
                    {
                        list.Add("cmd_globalpixels");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roombadge"].ToString()))
                    {
                        list.Add("cmd_roombadge");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_massbadge"].ToString()))
                    {
                        list.Add("cmd_massbadge");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_userinfo"].ToString()))
                    {
                        list.Add("cmd_userinfo");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_userinfo_viewip"].ToString()))
                    {
                        list.Add("cmd_userinfo_viewip");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_shutdown"].ToString()))
                    {
                        list.Add("cmd_shutdown");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_givebadge"].ToString()))
                    {
                        list.Add("cmd_givebadge");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_removebadge"].ToString()))
                    {
                        list.Add("cmd_removebadge");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_summon"].ToString()))
                    {
                        list.Add("cmd_summon");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_invisible"].ToString()))
                    {
                        list.Add("cmd_invisible");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_ban"].ToString()))
                    {
                        list.Add("cmd_ban");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_superban"].ToString()))
                    {
                        list.Add("cmd_superban");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roomkick"].ToString()))
                    {
                        list.Add("cmd_roomkick");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roomalert"].ToString()))
                    {
                        list.Add("cmd_roomalert");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_mute"].ToString()))
                    {
                        list.Add("cmd_mute");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_unmute"].ToString()))
                    {
                        list.Add("cmd_unmute");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_alert"].ToString()))
                    {
                        list.Add("cmd_alert");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_motd"].ToString()))
                    {
                        list.Add("cmd_motd");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_kick"].ToString()))
                    {
                        list.Add("cmd_kick");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_filter"].ToString()))
                    {
                        list.Add("cmd_update_filter");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_permissions"].ToString()))
                    {
                        list.Add("cmd_update_permissions");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_sa"].ToString()))
                    {
                        list.Add("cmd_sa");
                    }
                    if (HabboIM.StringToBoolean(dataRow["receive_sa"].ToString()))
                    {
                        list.Add("receive_sa");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_ipban"].ToString()))
                    {
                        list.Add("cmd_ipban");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_spull"].ToString()))
                    {
                        list.Add("cmd_spull");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_disconnect"].ToString()))
                    {
                        list.Add("cmd_disconnect");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_achievements"].ToString()))
                    {
                        list.Add("cmd_update_achievements");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_update_texts"].ToString()))
                    {
                        list.Add("cmd_update_texts");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_teleport"].ToString()))
                    {
                        list.Add("cmd_teleport");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_points"].ToString()))
                    {
                        list.Add("cmd_points");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_masspoints"].ToString()))
                    {
                        list.Add("cmd_masspoints");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_globalpoints"].ToString()))
                    {
                        list.Add("cmd_globalpoints");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_empty"].ToString()))
                    {
                        list.Add("cmd_empty");
                    }
                    if (HabboIM.StringToBoolean(dataRow["ignore_roommute"].ToString()))
                    {
                        list.Add("ignore_roommute");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_anyroomrights"].ToString()))
                    {
                        list.Add("acc_anyroomrights");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_anyroomowner"].ToString()))
                    {
                        list.Add("acc_anyroomowner");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_supporttool"].ToString()))
                    {
                        list.Add("acc_supporttool");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_chatlogs"].ToString()))
                    {
                        list.Add("acc_chatlogs");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_enter_fullrooms"].ToString()))
                    {
                        list.Add("acc_enter_fullrooms");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_enter_anyroom"].ToString()))
                    {
                        list.Add("acc_enter_anyroom");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_restrictedrooms"].ToString()))
                    {
                        list.Add("acc_restrictedrooms");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_unkickable"].ToString()))
                    {
                        list.Add("acc_unkickable");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_unbannable"].ToString()))
                    {
                        list.Add("acc_unbannable");
                    }
                    if (HabboIM.StringToBoolean(dataRow["ignore_friendsettings"].ToString()))
                    {
                        list.Add("ignore_friendsettings");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_sql"].ToString()))
                    {
                        list.Add("wired_give_sql");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_badge"].ToString()))
                    {
                        list.Add("wired_give_badge");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_effect"].ToString()))
                    {
                        list.Add("wired_give_effect");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_award"].ToString()))
                    {
                        list.Add("wired_give_award");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_dance"].ToString()))
                    {
                        list.Add("wired_give_dance");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_send"].ToString()))
                    {
                        list.Add("wired_give_send");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_credits"].ToString()))
                    {
                        list.Add("wired_give_credits");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_pixels"].ToString()))
                    {
                        list.Add("wired_give_pixels");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_points"].ToString()))
                    {
                        list.Add("wired_give_points");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_rank"].ToString()))
                    {
                        list.Add("wired_give_rank");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_respect"].ToString()))
                    {
                        list.Add("wired_give_respect");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_handitem"].ToString()))
                    {
                        list.Add("wired_give_handitem");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_moebel"].ToString()))
                    {
                        list.Add("wired_give_moebel");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_alert"].ToString()))
                    {
                        list.Add("wired_give_alert");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_roomusers"].ToString()))
                    {
                        list.Add("wired_cnd_roomusers");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_userhasachievement"].ToString()))
                    {
                        list.Add("wired_cnd_userhasachievement");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_userhasbadge"].ToString()))
                    {
                        list.Add("wired_cnd_userhasbadge");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_userhasvip"].ToString()))
                    {
                        list.Add("wired_cnd_userhasvip");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_userhaseffect"].ToString()))
                    {
                        list.Add("wired_cnd_userhaseffect");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_userrank"].ToString()))
                    {
                        list.Add("wired_cnd_userrank");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_usercredits"].ToString()))
                    {
                        list.Add("wired_cnd_usercredits");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_userpixels"].ToString()))
                    {
                        list.Add("wired_cnd_userpixels");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_userpoints"].ToString()))
                    {
                        list.Add("wired_cnd_userpoints");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_usergroups"].ToString()))
                    {
                        list.Add("wired_cnd_usergroups");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_wearing"].ToString()))
                    {
                        list.Add("wired_cnd_wearing");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_carrying"].ToString()))
                    {
                        list.Add("wired_cnd_carrying");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_wiredactived"].ToString()))
                    {
                        list.Add("wired_give_wiredactived");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_wiredactived"].ToString()))
                    {
                        list.Add("wired_cnd_wiredactived");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_unlimitedselects"].ToString()))
                    {
                        list.Add("wired_unlimitedselects");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_dance"].ToString()))
                    {
                        list.Add("cmd_dance");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_rave"].ToString()))
                    {
                        list.Add("cmd_rave");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roll"].ToString()))
                    {
                        list.Add("cmd_roll");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_control"].ToString()))
                    {
                        list.Add("cmd_control");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_makesay"].ToString()))
                    {
                        list.Add("cmd_makesay");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_sitdown"].ToString()))
                    {
                        list.Add("cmd_sitdown");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_lay"].ToString()))
                    {
                        list.Add("cmd_lay");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_push"].ToString()))
                    {
                        list.Add("cmd_push");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_pull"].ToString()))
                    {
                        list.Add("cmd_pull");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_flagme"].ToString()))
                    {
                        list.Add("cmd_flagme");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_mimic"].ToString()))
                    {
                        list.Add("cmd_mimic");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_moonwalk"].ToString()))
                    {
                        list.Add("cmd_moonwalk");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_follow"].ToString()))
                    {
                        list.Add("cmd_follow");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_handitem"].ToString()))
                    {
                        list.Add("cmd_handitem");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_startquestion"].ToString()))
                    {
                        list.Add("cmd_startquestion");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_vipha"].ToString()))
                    {
                        list.Add("cmd_vipha");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_spush"].ToString()))
                    {
                        list.Add("cmd_spush");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roomeffect"].ToString()))
                    {
                        list.Add("cmd_roomeffect");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_viphal"].ToString()))
                    {
                        list.Add("cmd_viphal");
                    }
                    if (HabboIM.StringToBoolean(dataRow["acc_moveotheruserstodoor"].ToString()))
                    {
                        list.Add("acc_moveotheruserstodoor");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_pet"].ToString()))
                    {
                        list.Add("cmd_pet");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roomfreeze"].ToString()))
                    {
                        list.Add("cmd_roomfreeze");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roomcredits"].ToString()))
                    {
                        list.Add("cmd_roomcredits");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roompixels"].ToString()))
                    {
                        list.Add("cmd_roompixels");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roompoints"].ToString()))
                    {
                        list.Add("cmd_roompoints");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roomdc"].ToString()))
                    {
                        list.Add("cmd_roomdc");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_raumkick"].ToString()))
                    {
                        list.Add("cmd_raumkick");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_raumalert"].ToString()))
                    {
                        list.Add("cmd_raumalert");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_verwarnung"].ToString()))
                    {
                        list.Add("cmd_verwarnung");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_myteleport"].ToString()))
                    {
                        list.Add("cmd_myteleport");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_makesayall"].ToString()))
                    {
                        list.Add("cmd_makesayall");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roomaward"].ToString()))
                    {
                        list.Add("cmd_roomaward");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_massaward"].ToString()))
                    {
                        list.Add("cmd_massaward");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_summonall"].ToString()))
                    {
                        list.Add("cmd_summonall");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_cnd_useronline"].ToString()))
                    {
                        list.Add("wired_cnd_useronline");
                    }
                    if (HabboIM.StringToBoolean(dataRow["wired_give_item"].ToString()))
                    {
                        list.Add("wired_give_item");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_summonstaff"].ToString()))
                    {
                        list.Add("cmd_summonstaff");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_antiwerber"].ToString()))
                    {
                        list.Add("cmd_antiwerber");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_antiwerberreset"].ToString()))
                    {
                        list.Add("cmd_antiwerberreset");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_roomitem"].ToString()))
                    {
                        list.Add("cmd_roomitem");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_timermute"].ToString()))
                    {
                        list.Add("cmd_timermute");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_timermuteall"].ToString()))
                    {
                        list.Add("cmd_timermuteroom");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_timermuteall"].ToString()))
                    {
                        list.Add("cmd_timermuteall");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_staffpicks"].ToString()))
                    {
                        list.Add("cmd_staffpicks");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_vouchergame"].ToString()))
                    {
                        list.Add("cmd_vouchergame");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_eventuhr"].ToString()))
                    {
                        list.Add("cmd_eventuhr");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_spielerakte"].ToString()))
                    {
                        list.Add("cmd_spielerakte");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_eventhaws"].ToString()))
                    {
                        list.Add("cmd_eventhaws");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_reloadws"].ToString()))
                    {
                        list.Add("cmd_reloadws");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_sitealert"].ToString()))
                    {
                        list.Add("cmd_sitealert");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_sellroom"].ToString()))
                    {
                        list.Add("cmd_sellroom");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_buyroom"].ToString()))
                    {
                        list.Add("cmd_buyroom");
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmd_buy"].ToString()))
                    {
                        list.Add("cmd_buy");
                    }
                if (HabboIM.StringToBoolean(dataRow["cmd_spam"].ToString()))
                {
                    list.Add("cmd_spam");
                    }
                    this.dictionary_1.Add((uint)dataRow["rank"], list);
                }
        
            }


            dataTable = class6_0.ReadDataTable("SELECT * FROM permissions_vip;", 30);
            if (dataTable != null)
            {
                ServerConfiguration.UnknownBoolean1 = false;
                ServerConfiguration.UnknownBoolean2 = false;
                ServerConfiguration.UnknownBoolean3 = false;
                ServerConfiguration.UnknownBoolean7 = false;
                ServerConfiguration.UnknownBoolean8 = false;
                ServerConfiguration.UnknownBoolean9 = false;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (HabboIM.StringToBoolean(dataRow["cmdPush"].ToString()))
                    {
                        ServerConfiguration.UnknownBoolean1 = true;
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmdPull"].ToString()))
                    {
                        ServerConfiguration.UnknownBoolean2 = true;
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmdFlagme"].ToString()))
                    {
                        ServerConfiguration.UnknownBoolean3 = true;
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmdMimic"].ToString()))
                    {
                        ServerConfiguration.UnknownBoolean7 = true;
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmdMoonwalk"].ToString()))
                    {
                        ServerConfiguration.UnknownBoolean8 = true;
                    }
                    if (HabboIM.StringToBoolean(dataRow["cmdFollow"].ToString()))
                    {
                        ServerConfiguration.UnknownBoolean9 = true;
                    }
                }
            }
            this.dictionary_5.Clear();
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_free"), 0);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_sit"), 1);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_down"), 2);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_here"), 3);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_beg"), 4);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_play_dead"), 5);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_stay"), 6);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_follow"), 7);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_stand"), 8);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_jump"), 9);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_speak"), 10);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_play"), 11);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_silent"), 12);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_nest"), 13);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_drink"), 14);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_follow_left"), 15);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_follow_right"), 16);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_play_football"), 17);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_move_forwar"), 24);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_turn_left"), 25);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_turn_right"), 26);
            this.dictionary_5.Add(HabboIMEnvironment.GetExternalText("pet_cmd_eat"), 43);
            this.dictionary_4.Clear();
            //this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_about_name"), 1);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_alert_name"), 2);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_award_name"), 3);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_ban_name"), 4);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_buy_name"), 5);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_coins_name"), 6);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_coords_name"), 7);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_disablediagonal_name"), 8);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_emptyitems_name"), 9);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_empty_name"), 10);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_enable_name"), 11);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_flagme_name"), 12);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_follow_name"), 13);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_freeze_name"), 14);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_givebadge_name"), 15);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_globalcredits_name"), 16);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_globalpixels_name"), 17);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_globalpoints_name"), 18);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_hal_name"), 19);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_ha_name"), 20);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_invisible_name"), 21);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_ipban_name"), 22);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_kick_name"), 23);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_massbadge_name"), 24);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_masscredits_name"), 25);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_masspixels_name"), 26);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_masspoints_name"), 27);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_mimic_name"), 28);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_moonwalk_name"), 29);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_motd_name"), 30);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_mute_name"), 31);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_override_name"), 32);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_pickall_name"), 33);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_pixels_name"), 34);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_points_name"), 35);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_pull_name"), 36);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_push_name"), 37);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_redeemcreds_name"), 38);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_removebadge_name"), 39);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_ride_name"), 40);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_roomalert_name"), 41);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_roombadge_name"), 42);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_roomkick_name"), 43);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_roommute_name"), 44);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_sa_name"), 45);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_setmax_name"), 46);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_setspeed_name"), 47);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_shutdown_name"), 48);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_spull_name"), 49);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_summon_name"), 50);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_superban_name"), 51);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_teleport_name"), 52);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_unload_name"), 53);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_unmute_name"), 54);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_update_achievements_name"), 55);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_update_bans_name"), 56);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_update_bots_name"), 57);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_update_catalogue_name"), 58);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_update_filter_name"), 59);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_update_items_name"), 60);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_update_navigator_name"), 61);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_update_permissions_name"), 62);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_update_settings_name"), 63);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_userinfo_name"), 64);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_update_texts_name"), 65);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_disconnect_name"), 66);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_commands_name"), 67);
            this.dictionary_4.Add("about", 168);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_roominfo_name"), 69);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_dance_name"), 71);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_rave_name"), 72);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_roll_name"), 73);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_control_name"), 74);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_makesay_name"), 75);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_sitdown_name"), 76);
            this.dictionary_4.Add("geben", 79);
            this.dictionary_4.Add("sit", 80);
            this.dictionary_4.Add("dismount", 81);
            this.dictionary_4.Add("getoff", 82);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_emptypets_name"), 83);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_startquestion_name"), 94);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_lay_name"), 86);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_handitem_name"), 85);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_vipha_name"), 87);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_spush_name"), 88);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_roomeffect_name"), 91);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_redeempixel_name"), 95);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_redeemshell_name"), 96);
            this.dictionary_4.Add(HabboIMEnvironment.GetExternalText("cmd_viphal_name"), 97);
            //Custom Commands
            //this.dictionary_4.Add("tweet", 122);
            this.dictionary_4.Add("pet", 123);
            this.dictionary_4.Add("habbo", 124);
            this.dictionary_4.Add("mark", 125);
            this.dictionary_4.Add("mini", 127);
            this.dictionary_4.Add("afk", 128);
            this.dictionary_4.Add("brb", 128);
            this.dictionary_4.Add("zahl", 129);
            this.dictionary_4.Add("kiss", 130);
            this.dictionary_4.Add("staff", 131);
            this.dictionary_4.Add("raumalert", 140);
            this.dictionary_4.Add("roomfreeze", 141);
            this.dictionary_4.Add("sellroom", 142);
            this.dictionary_4.Add("buyroom", 143);
            this.dictionary_4.Add("roomcredits", 144);
            this.dictionary_4.Add("roompixels", 145);
            this.dictionary_4.Add("roompoints", 146);
            this.dictionary_4.Add("roomdc", 147);
            this.dictionary_4.Add("raumkick", 148);
            this.dictionary_4.Add("verwarnung", 149);
            this.dictionary_4.Add("randomuser", 150);
            this.dictionary_4.Add("randomkick", 151);
            this.dictionary_4.Add("werber", 152);
            this.dictionary_4.Add("myteleport", 153);
            this.dictionary_4.Add("makesayall", 155);
            this.dictionary_4.Add("roomaward", 156);
            this.dictionary_4.Add("massaward", 157);
            this.dictionary_4.Add("summonall", 158);
            this.dictionary_4.Add("room", 159);
            this.dictionary_4.Add("summonstaff", 160);
            this.dictionary_4.Add("antiwerber", 161);
            this.dictionary_4.Add("delaws", 162);
            this.dictionary_4.Add("delbws", 16299);
            this.dictionary_4.Add("roomitem", 163);
            this.dictionary_4.Add("timermute", 164);
            this.dictionary_4.Add("timermuteroom", 165);
            this.dictionary_4.Add("timermuteall", 166);
            this.dictionary_4.Add("staffpicks", 167);
            this.dictionary_4.Add("developer", 168);
            this.dictionary_4.Add("mutewc", 55356);
            this.dictionary_4.Add("cheat", 169);
            this.dictionary_4.Add("getws", 170);
            this.dictionary_4.Add("reloadws", 171);
            this.dictionary_4.Add("eventha", 172);
            this.dictionary_4.Add("sitealert", 173);
            this.dictionary_4.Add("wc", 174);
            this.dictionary_4.Add("ac", 175);
            this.dictionary_4.Add("unban", 176);
            this.dictionary_4.Add("duty", 177);
            this.dictionary_4.Add("sneeze", 179);
            this.dictionary_4.Add("ehe", 180);
            this.dictionary_4.Add("wil", 181);
            this.dictionary_4.Add("kill", 182);
            this.dictionary_4.Add("love", 183);
            this.dictionary_4.Add("like", 184);
            this.dictionary_4.Add("regen", 185);
            this.dictionary_4.Add("box", 186);
            this.dictionary_4.Add("umarm", 187);
            this.dictionary_4.Add("casino", 188);
            this.dictionary_4.Add("werberunde", 191);
            this.dictionary_4.Add("awhisper", 192);
            this.dictionary_4.Add("sms", 193);
            this.dictionary_4.Add("dislike", 194);
            this.dictionary_4.Add("hot", 195);
            this.dictionary_4.Add("jump", 196);
            this.dictionary_4.Add("habnam", 197);
            this.dictionary_4.Add("sonne", 198);
            this.dictionary_4.Add("updates", 199);
            this.dictionary_4.Add("rauchen", 200);
            this.dictionary_4.Add("adminafk", 201);
            this.dictionary_4.Add("adminwd", 202);
            this.dictionary_4.Add("rw", 203);
            this.dictionary_4.Add("ytplayer", 204);
            this.dictionary_4.Add("extras", 205);
            this.dictionary_4.Add("vouchergame", 206);
            this.dictionary_4.Add("eventuhr", 207);
            this.dictionary_4.Add("regoff", 208);
            this.dictionary_4.Add("regon", 209);
            this.dictionary_4.Add("jail", 2070);
            this.dictionary_4.Add("unjail", 2080);
            this.dictionary_4.Add("wd", 2090);
            this.dictionary_4.Add("spam", 3000);
            this.dictionary_4.Add("knuddel", 3010);
            this.dictionary_4.Add("spielerakte", 3020);
            this.dictionary_4.Add("info", 2081);
            // this.dictionary_4.Add("staticban", 4999);
            this.dictionary_4.Add("fastwalk", 888833);
            this.dictionary_4.Add("fasterwalk", 888834);
            this.dictionary_4.Add("fastwon", 888838);
            this.dictionary_4.Add("fastwoff", 888836);
            this.dictionary_4.Add("offban", 2048);
            this.dictionary_4.Add("switchuser", 2058);
            this.dictionary_4.Add("teleportuser", 2068);
            this.dictionary_4.Add("gc", 77585);
            this.dictionary_4.Add("gcon", 4734867);
            this.dictionary_4.Add("gcoff", 4734868);
            this.dictionary_4.Add("gcadd", 83789);
            this.dictionary_4.Add("gcdel", 83784);
            this.dictionary_4.Add("offbadge", 88998874);
            this.dictionary_4.Add("suchen", 998877345);
            this.dictionary_4.Add("sammeln", 998877345);
            this.dictionary_4.Add("mutegc", 878683);
            this.dictionary_4.Add("unmutegc", 88179938);
            this.dictionary_4.Add("beziehung", 66666);
            this.dictionary_4.Add("support", 666668);
            this.dictionary_4.Add("lotto", 900000);
            this.dictionary_4.Add("minitanz", 999983);
            this.dictionary_4.Add("iohawk", 999984);
            this.dictionary_4.Add("springen", 999985);
            this.dictionary_4.Add("laufen", 999986);
            this.dictionary_4.Add("feel", 999987);
            this.dictionary_4.Add("waffe", 999988);
            this.dictionary_4.Add("fly", 999989);
            this.dictionary_4.Add("user", 999992);
            this.dictionary_4.Add("aduty", 999993);
            this.dictionary_4.Add("tweet", 999994);
            this.dictionary_4.Add("rotate", 999995);
            this.dictionary_4.Add("befehle", 67);

            Logging.WriteLine("Fertig!", ConsoleColor.Green);
        }

        public bool method_1(uint uint_0, string string_0)
        {
            bool result;
            if (!this.method_7(uint_0))
            {
                result = false;
            }
            else
            {
                List<string> list = this.dictionary_1[uint_0];
                result = list.Contains(string_0);
            }
            return result;
        }

        public int method_2(uint uint_0)
        {
            return this.dictionary_3[uint_0];
        }

        public bool method_3(uint uint_0)
        {
            return this.method_6(uint_0);
        }

        public bool method_4(uint uint_0, string string_0)
        {
            bool result;
            if (!this.method_6(uint_0))
            {
                result = false;
            }
            else
            {
                List<string> list = this.dictionary_0[uint_0];
                result = list.Contains(string_0);
            }
            return result;
        }

        public List<string> method_5(uint uint_0, uint uint_1)
        {
            List<string> result = new List<string>();
            if (this.method_6(uint_0))
            {
                result = this.dictionary_0[uint_0];
            }
            else
            {
                result = this.dictionary_1[uint_1];
            }
            return result;
        }

        public bool method_6(uint uint_0)
        {
            return this.dictionary_0.ContainsKey(uint_0);
        }

        public bool method_7(uint uint_0)
        {
            return this.dictionary_1.ContainsKey(uint_0);
        }

        public string method_8(uint uint_0)
        {
            string result;
            if (this.dictionary_2.ContainsKey(uint_0))
            {
                result = this.dictionary_2[uint_0];
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nCan't find rank: " + uint_0);
                Console.ForegroundColor = ConsoleColor.Gray;
                result = "error";
            }
            return result;
        }

        public int method_9()
        {
            return this.dictionary_2.Count;
        }

        public void method_10()
        {
            this.dictionary_2.Clear();
            this.dictionary_0.Clear();
            this.dictionary_1.Clear();
            this.dictionary_3.Clear();
        }

        public bool method_11(string string_0, GameClient Session)
        {
            bool result;
            bool result2;
            switch (string_0)
            {
                case "roomuserseq":
                case "roomuserslt":
                case "roomusersmt":
                case "roomusersmte":
                case "roomuserslte":
                    if (Session.GetHabbo().HasFuse("wired_cnd_roomusers"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "userhasachievement":
                case "userhasntachievement":
                    if (Session.GetHabbo().HasFuse("wired_cnd_userhasachievement"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "userhasbadge":
                case "userhasntbadge":
                    if (Session.GetHabbo().HasFuse("wired_cnd_userhasbadge"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "userhasvip":
                case "userhasntvip":
                    if (Session.GetHabbo().HasFuse("wired_cnd_userhasvip"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "userhaseffect":
                case "userhasnteffect":
                    if (Session.GetHabbo().HasFuse("wired_cnd_userhaseffect"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "userrankeq":
                case "userrankmt":
                case "userrankmte":
                case "userranklt":
                case "userranklte":
                    if (Session.GetHabbo().HasFuse("wired_cnd_userrank"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "usercreditseq":
                case "usercreditsmt":
                case "usercreditsmte":
                case "usercreditslt":
                case "usercreditslte":
                    if (Session.GetHabbo().HasFuse("wired_cnd_usercredits"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "userpixelseq":
                case "userpixelsmt":
                case "userpixelsmte":
                case "userpixelslt":
                case "userpixelslte":
                    if (Session.GetHabbo().HasFuse("wired_cnd_userpixels"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "userpointseq":
                case "userpointsmt":
                case "userpointsmte":
                case "userpointslt":
                case "userpointslte":
                    if (Session.GetHabbo().HasFuse("wired_cnd_userpoints"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "usergroupeq":
                case "userisingroup":
                    if (Session.GetHabbo().HasFuse("wired_cnd_usergroups"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "wearing":
                case "notwearing":
                    if (Session.GetHabbo().HasFuse("wired_cnd_wearing"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "carrying":
                case "notcarrying":
                    if (Session.GetHabbo().HasFuse("wired_cnd_carrying"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "wiredactived":
                case "notwiredactived":
                    if (Session.GetHabbo().HasFuse("wired_cnd_wiredactived"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "useronlineeq":
                case "useronlinelt":
                case "useronlinemt":
                case "useronlinemte":
                case "useronlinelte":
                    if (Session.GetHabbo().HasFuse("wired_cnd_useronline"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
            }
            result = false;
            result2 = result;
            return result2;
        }

        public bool method_12(string string_0, GameClient Session)
        {
            bool result;
            bool result2;
            switch (string_0)
            {
                case "sql":
                    if (Session.GetHabbo().HasFuse("wired_give_sql"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "badge":
                    if (Session.GetHabbo().HasFuse("wired_give_badge"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "effect":
                    if (Session.GetHabbo().HasFuse("wired_give_effect"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "award":
                    if (Session.GetHabbo().HasFuse("wired_give_award"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "dance":
                    if (Session.GetHabbo().HasFuse("wired_give_dance"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "send":
                    if (Session.GetHabbo().HasFuse("wired_give_send"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "credits":
                    if (Session.GetHabbo().HasFuse("wired_give_credits"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "pixels":
                    if (Session.GetHabbo().HasFuse("wired_give_pixels"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "points":
                    if (Session.GetHabbo().HasFuse("wired_give_points"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "rank":
                    if (Session.GetHabbo().HasFuse("wired_give_rank"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "respect":
                    if (Session.GetHabbo().HasFuse("wired_give_respect"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "handitem":
                    if (Session.GetHabbo().HasFuse("wired_give_handitem"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "moebel":
                    if (Session.GetHabbo().HasFuse("wired_give_moebel"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "alert":
                    if (Session.GetHabbo().HasFuse("wired_give_alert"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "wiredactived":
                    if (Session.GetHabbo().HasFuse("wired_give_wiredactived"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
                case "item":
                    if (Session.GetHabbo().HasFuse("wired_give_item"))
                    {
                        result = true;
                        result2 = result;
                        return result2;
                    }
                    break;
            }
            result = false;
            result2 = result;
            return result2;
        }
    }
}
