using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Users.UserDataManagement;
using System;
using System.Data;

namespace HabboIM.HabboHotel.Users.Authenticator
{
    internal sealed class Authenticator
    {
        internal static Habbo CreateHabbo(string ssoTicket, GameClient Session, UserDataFactory userData, UserDataFactory otherData)
        {
            return Authenticator.CreateHabbo(userData.GetUserData(), ssoTicket, Session, otherData);
        }

        private static Habbo CreateHabbo( DataRow habboData, string ssoTicket, GameClient session, UserDataFactory otherData)
        {
           
            uint Id = (uint)habboData["Id"];
            string Username = (string)habboData["username"];
            string Name = (string)habboData["real_name"];
            uint Rank = (uint)habboData["rank"];
            string Motto = (string)habboData["motto"];
            string ip_last = (string)habboData["ip_last"];
            string look = (string)habboData["look"];
            string gender = (string)habboData["gender"];
            int credits = (int)habboData["credits"];
            int pixels = (int)habboData["activity_points"];
            string account_created = (string)habboData["account_created"];
            double activity_points_lastupdate = (double)habboData["activity_points_lastupdate"];
            string last_loggedin = (string)habboData["last_loggedin"];
            int daily_respect_points = (int)habboData["daily_respect_points"];
            int daily_pet_respect_points = (int)habboData["daily_pet_respect_points"];
            double vipha_last = (double)habboData["vipha_last"];
            double viphal_last = (double)habboData["viphal_last"];
            //string static_id = (string)habboData["static_id_last"];
            string static_id = "NOT EXIST IN THIS MOMENT";
            int jail = (int)habboData["jail"];
            int lovepoints = (int)habboData["lovepoints"];
            int gc = (int)habboData["gc"];
            double jailtime = (double)habboData["jailtime"];
            double kisstime = (double)habboData["lastkiss_time"];
            double hugtime = (double)habboData["lasthug_time"];
            int boyfriend = (int)habboData["boyfriend"];
            int kissed = (int)habboData["kisses_bf"];
            int hugged = (int)habboData["hugs_bf"];
            double lovedate = (double)habboData["love_date"];
            int bez_level = (int)habboData["bz_level"];
            double support_last = (double)habboData["support_last"];

            return new Habbo(support_last, bez_level, lovedate, kissed, hugged, lovepoints, kisstime, hugtime, boyfriend, gc, static_id, jailtime, jail,  Id, Username, Name, ssoTicket, Rank, Motto, look, gender, credits, pixels, activity_points_lastupdate, account_created, HabboIM.StringToBoolean(habboData["is_muted"].ToString()), (uint)habboData["home_room"], (int)habboData["newbie_status"], HabboIM.StringToBoolean(habboData["block_newfriends"].ToString()), HabboIM.StringToBoolean(habboData["hide_inroom"].ToString()), HabboIM.StringToBoolean(habboData["hide_online"].ToString()), HabboIM.StringToBoolean(habboData["vip"].ToString()), (int)habboData["volume"], (int)habboData["vip_points"], HabboIM.StringToBoolean(habboData["accept_trading"].ToString()), ip_last, session, otherData, last_loggedin, daily_respect_points, daily_pet_respect_points, vipha_last, viphal_last, HabboIM.StringToBoolean(habboData["friend_stream_enabled"].ToString()), HabboIM.StringToBoolean(habboData["raumalert"].ToString()), HabboIM.StringToBoolean(habboData["werbercmd"].ToString()), (string)habboData["working"], (string)habboData["changenametime"], HabboIM.StringToBoolean(habboData["changename"].ToString()));
        }

        internal static Habbo CreateHabbo(string username)
        {
            UserDataFactory userdata = new UserDataFactory(username, false);
            return Authenticator.CreateHabbo( userdata.GetUserData(), "", null, userdata);
        }
    }
}
