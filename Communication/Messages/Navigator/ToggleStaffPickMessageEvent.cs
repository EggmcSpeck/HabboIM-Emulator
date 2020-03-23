using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
using HabboIM.Storage;
using HabboIM.Util;
namespace HabboIM.Communication.Messages.Navigator
{
    internal sealed class ToggleStaffPickMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session.GetHabbo().HasFuse("acc_staffpicks"))
            {
                Room Room = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

                int AlreadyStaffPicks;
                AlreadyStaffPicks = 0;

                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    if (dbClient.ReadDataRow("SELECT * FROM navigator_publics WHERE room_id = '" + Room.Id + "'") != null)
                    {
                        AlreadyStaffPicks = 1;
                    }
                }


                if (AlreadyStaffPicks == 0)
                {
                    string Owner;
                    int OwnerID;
                    using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                    {
                        Owner = dbClient.ReadString("SELECT owner FROM rooms WHERE id = '" + Room.Id + "'");
                        dbClient.AddParamWithValue("roomname", Room.Name);
                        dbClient.ExecuteQuery("INSERT INTO `navigator_publics` (`bannertype`, `caption`, `room_id`, `category_parent_id`, `image`, `image_type`) VALUES ('1', @roomname, '" + Room.Id + "', '" + ServerConfiguration.StaffPicksID + "', 'officialrooms_hq/staffpickfolder.gif', 'external')");
                    }

                    GameClient RoomOwner = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Owner);
                    if (RoomOwner != null)
                    {
                        RoomOwner.GetHabbo().StaffPicks++;
                        RoomOwner.GetHabbo().CheckStaffPicksAchievement();
                    }
                    else
                    {
                        using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                        {
                            try
                            {
                                OwnerID = dbClient.ReadInt32("SELECT id FROM users WHERE username = '" + Owner + "'");
                                dbClient.ExecuteQuery("UPDATE user_stats SET staff_picks = staff_picks + 1 WHERE id = '" + OwnerID + "' LIMIT 1");
                            }
                            catch (Exception)
                            {
                                Session.SendNotification("Room owner is not in database!");
                            }
                        }
                    }

                    using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                    {
                        HabboIM.GetGame().GetNavigator().method_0(dbClient);
                    }

                    Session.SendNotification("Room added to Staff Picks successfully.");

                }
                else
                {
                    using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                    {

                        dbClient.ExecuteQuery("DELETE FROM `navigator_publics` WHERE (`room_id`='" + Room.Id + "')");
                    }

                    using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                    {
                        HabboIM.GetGame().GetNavigator().method_0(dbClient);
                    }

                    Session.SendNotification("Room removed from Staff Picks successfully.");
                }
            }
        }
    }
}
