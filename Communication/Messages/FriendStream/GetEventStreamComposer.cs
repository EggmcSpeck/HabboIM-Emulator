using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Rooms;
using HabboIM.Messages;
using HabboIM.Storage;
using System;
using System.Data;

namespace HabboIM.Communication.Messages.FriendStream
{
    internal sealed class GetEventStreamComposer : Interface
    {
        public void Handle(GameClient session, ClientMessage message)
        {
            try
            {
                ServerMessage response = new ServerMessage(950u);
                session.GetHabbo().GetUserDataFactory().UpdateFriendStream();
                int streamCount = session.GetHabbo().GetUserDataFactory().GetFriendStream().Rows.Count;
                response.AppendInt32(streamCount);
                DataTable dataTable_ = session.GetHabbo().GetUserDataFactory().GetFriendStream();
                foreach (DataRow row in dataTable_.Rows)
                {
                    int type = (int)row["type"];
                   if (type == 5 )
                   {
                        uint id = (uint)row["id"];
                        int likes = 0;
                        bool canlike = false;
                        using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
                        {
                            likes = @class.ReadInt32("SELECT COUNT(friend_stream_id) FROM friend_stream_likes WHERE friend_stream_id = '" + id + "' LIMIT 1", 30);
                            DataRow datarow = @class.ReadDataRow(string.Concat(new object[]
                            {
                            "SELECT id FROM friend_stream_likes WHERE friend_stream_id = '",
                            id,
                            "' AND userid = '",
                            session.GetHabbo().Id,
                            "' LIMIT 1"
                            }), 30);
                            canlike = (datarow == null);
                        }
                        uint userid = (uint)row["userid"];
                        string username = HabboIM.GetGame().GetClientManager().GetNameById(userid);
                        string gender = row["gender"].ToString().ToLower();
                        string look = (string)row["look"];
                        int time = (int)((HabboIM.GetUnixTimestamp() - (double)row["time"]) / 60.0);
                        string data = (string)row["data"];
                        response.AppendUInt(id);
                        response.AppendInt32(1); // type 1
                        response.AppendStringWithBreak(userid.ToString());
                        response.AppendStringWithBreak(username);
                        response.AppendStringWithBreak(gender);
                        response.AppendStringWithBreak("http://live-hotel.biz/web-gallery/friendstream/index.gif?figure=" + look + ".gif");
                        response.AppendInt32(time);
                        response.AppendInt32(2);
                        response.AppendInt32(likes);
                       response.AppendBoolean(canlike);
                       response.AppendStringWithBreak("1");
                        response.AppendStringWithBreak(data);
                    }
                }
                session.SendMessage(response);
            }
            catch (Exception x)
            {
                session.SendNotification("Es trat folgender Fehler auf:\r\n" + x);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(x);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
