using System;
using System.Data;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.Storage;
namespace HabboIM.Communication.Messages.Avatar
{
    internal sealed class GetWardrobeMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            ServerMessage Message = new ServerMessage(267u);
            Message.AppendBoolean(Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"));
            if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
            {
                using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
                {
                    @class.AddParamWithValue("userid", Session.GetHabbo().Id);
                    DataTable dataTable = @class.ReadDataTable("SELECT slot_id, look, gender FROM user_wardrobe WHERE user_id = @userid;");
                    if (dataTable == null)
                    {
                        Message.AppendInt32(0);
                    }
                    else
                    {
                        Message.AppendInt32(dataTable.Rows.Count);
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            Message.AppendUInt((uint)dataRow["slot_id"]);
                            Message.AppendStringWithBreak((string)dataRow["look"]);
                            Message.AppendStringWithBreak((string)dataRow["gender"]);
                        }
                    }
                }
                Session.SendMessage(Message);
            }
            else
            {
                using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
                {
                    @class.AddParamWithValue("userid", Session.GetHabbo().Id);
                    DataTable dataTable = @class.ReadDataTable("SELECT slot_id, look, gender FROM user_wardrobe WHERE user_id = @userid;");
                    if (dataTable == null)
                    {
                        Message.AppendInt32(0);
                    }
                    else
                    {
                        Message.AppendInt32(dataTable.Rows.Count);
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            Message.AppendUInt((uint)dataRow["slot_id"]);
                            Message.AppendStringWithBreak((string)dataRow["look"]);
                            Message.AppendStringWithBreak((string)dataRow["gender"]);
                        }
                    }
                }
                Session.SendMessage(Message);
            }
        }
    }
}