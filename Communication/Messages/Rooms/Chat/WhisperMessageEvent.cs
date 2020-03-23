using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Misc;
using HabboIM.HabboHotel.Rooms;
using HabboIM.Messages;
using HabboIM.Storage;
using System;

namespace HabboIM.Communication.Messages.Rooms.Chat
{
    internal sealed class WhisperMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && Session != null)
            {
                if (Session.GetHabbo().IsMuted)
                {
                    Session.SendNotification(HabboIMEnvironment.GetExternalText("error_muted"));
                }
                else if (Session.GetHabbo().HasFuse("ignore_roommute") || !@class.bool_4)
                {
                    string text = HabboIM.FilterString(Event.PopFixedString());
                    string text2 = text.Split(new char[]
					{
						' '
					})[0];
                    string text3 = text.Substring(text2.Length + 1);
                    string antiweberstring = text3;
                    text3 = ChatCommandHandler.smethod_4(text3);
                    RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
                    RoomUser class3 = @class.method_56(text2);
                    if (Session.GetHabbo().method_4() > 0)
                    {
                        TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().dateTime_0;
                        if (timeSpan.Seconds > 4)
                        {
                            Session.GetHabbo().int_23 = 0;
                        }
                        if (timeSpan.Seconds < 4 && Session.GetHabbo().int_23 > 5 && !class2.IsBot)
                        {
                            ServerMessage Message = new ServerMessage(27u);
                            Message.AppendInt32(Session.GetHabbo().method_4());
                            Session.SendMessage(Message);
                            Session.GetHabbo().IsMuted = true;
                            Session.GetHabbo().int_4 = Session.GetHabbo().method_4();
                            return;
                        }
                        Session.GetHabbo().dateTime_0 = DateTime.Now;
                        Session.GetHabbo().int_23++;
                    }
                    ServerMessage Message2 = new ServerMessage(25u);
                    Message2.AppendInt32(class2.VirtualId);
                    Message2.AppendStringWithBreak(text3);
                    Message2.AppendBoolean(false);
                    if (class2 != null && !class2.IsBot)
                    {
                        class2.GetClient().SendMessage(Message2);
                        try {
                            var class266 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

                            for (int i = 0; i < class266.RoomUsers.Length; i++)
                            {

                                try {
                                    RoomUser class5 = class266.RoomUsers[i];
                                    if (class5 != null && class3 != null)
                                    {
                                        if (class5.GetClient().GetHabbo().whisperlog == true && class5.GetClient().GetHabbo().Rank >= 6 && class5.GetClient().GetHabbo().Id != Session.GetHabbo().Id && class5.GetClient().GetHabbo().Id != class3.GetClient().GetHabbo().Id)
                                            class5.GetClient().GetHabbo().Whisper(" < " + Session.GetHabbo().Username + " fl�stert zu " + class3.GetClient().GetHabbo().Username + " > " + text.Substring(text2.Length + 1));

                                    }

                                } catch { }
                            }

                        }  catch { }
                        //HabboIM.GetGame().GetClientManager().method_StaffWhisper("<" + Session.GetHabbo().Username + " fl�stert zu " + class3.GetClient().GetHabbo().Username + "> " + text3);
                    }
                    class2.Unidle();
                    if (class3 != null && !class3.IsBot && (class3.GetClient().GetHabbo().list_2.Count <= 0 || !class3.GetClient().GetHabbo().list_2.Contains(Session.GetHabbo().Id)))
                    {
                        class3.GetClient().SendMessage(Message2);
                        if (ServerConfiguration.EnableChatlog)
                        {
                            using (DatabaseClient class4 = HabboIM.GetDatabase().GetClient())
                            {
                                class4.AddParamWithValue("message", "<Whisper to " + class3.GetClient().GetHabbo().Username + ">: " + text3);
                                class4.ExecuteQuery(string.Concat(new object[]
								{
									"INSERT INTO chatlogs (user_id,room_id,hour,minute,timestamp,message,user_name,full_date) VALUES ('",
									Session.GetHabbo().Id,
									"','",
									@class.Id,
									"','",
									DateTime.Now.Hour,
									"','",
									DateTime.Now.Minute,
									"',UNIX_TIMESTAMP(),@message,'",
									Session.GetHabbo().Username,
									"','",
									DateTime.Now.ToLongDateString(),
									"')"
								}), 30);
                            }
                        }
                        if (HabboIM.GetGame().AntiWerberStatus)
                        {
                            string textaw = ChatCommandHandler.smethod_4b(Session, antiweberstring, "Raum");
                        }
                    }
                }
            }
        }
    }
}
