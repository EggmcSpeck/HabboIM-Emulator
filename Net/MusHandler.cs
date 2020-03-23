using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Misc;
using HabboIM.HabboHotel.Rooms;
using HabboIM.HabboHotel.Users;
using HabboIM.Messages;
using HabboIM.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HabboIM.Net
{
    internal sealed class MusHandler
    {
        private Socket ClientSocket;

        private byte[] Buffer = new byte[1024];

        public MusHandler(Socket serverSocket)
        {
            this.ClientSocket = serverSocket;
            try
            {
                this.ClientSocket.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceiveCallback), this.ClientSocket);
            }
            catch
            {
                this.Dispose();
            }
        }

        public void Dispose()
        {
            try
            {
                this.ClientSocket.Shutdown(SocketShutdown.Both);
                this.ClientSocket.Close();
                this.ClientSocket.Dispose();
            }
            catch
            {
            }
        }

        public void OnReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int count = 0;
                try
                {
                    count = this.ClientSocket.EndReceive(ar);
                }
                catch
                {
                    this.Dispose();
                    return;
                }
                string data = Encoding.Default.GetString(this.Buffer, 0, count);
                if (data.Length > 0)
                {
                    this.ParsePacket(data);
                }
            }
            catch
            {
            }
            this.Dispose();
        }

        public void ParsePacket(string data)
        {
            string text = data.Split(new char[]
            {
                Convert.ToChar(1)
            })[0];
            string text2 = data.Split(new char[]
            {
                Convert.ToChar(1)
            })[1];
            GameClient client = null;
            DataRow dataRow = null;
            string text3 = text.ToLower();
            if (text3 != null)
            {
                if (MusCommands.dictionary_0 == null)
                {
                    MusCommands.dictionary_0 = new Dictionary<string, int>(29)
                    {
                        {
                            "update_items",
                            0
                        },
                        {
                            "update_catalogue",
                            1
                        },
                        {
                            "update_catalog",
                            2
                        },
                        {
                            "updateusersrooms",
                            3
                        },
                        {
                            "senduser",
                            4
                        },
                        {
                            "updatevip",
                            5
                        },
                        {
                            "giftitem",
                            6
                        },
                        {
                            "giveitem",
                            7
                        },
                        {
                            "unloadroom",
                            8
                        },
                        {
                            "roomalert",
                            9
                        },
                        {
                            "updategroup",
                            10
                        },
                        {
                            "updateusersgroups",
                            11
                        },
                        {
                            "shutdown",
                            12
                        },
                        {
                            "update_filter",
                            13
                        },
                        {
                            "refresh_filter",
                            14
                        },
                        {
                            "updatecredits",
                            15
                        },
                        {
                            "updatesettings",
                            16
                        },
                        {
                            "updatepixels",
                            17
                        },
                        {
                            "updatepoints",
                            18
                        },
                        {
                            "reloadbans",
                            19
                        },
                        {
                            "update_bots",
                            20
                        },
                        {
                            "signout",
                            21
                        },
                        {
                            "exe",
                            22
                        },
                        {
                            "alert",
                            23
                        },
                        {
                            "sa",
                            24
                        },
                        {
                            "ha",
                            25
                        },
                        {
                            "hal",
                            26
                        },
                        {
                            "updatemotto",
                            27
                        },
                        {
                            "updatelook",
                            28
                        },
                        {
                            "infobuspoll",
                            29
                        },
                        {
                            "givebadge",
                            30
                        },
                        {
                            "addroom",
                            31
                        },
                        {
                            "updatescmd",
                            32
                        },
                        {
                            "whisperall",
                            33

                        }
                    };
                }
                int num;
                if (MusCommands.dictionary_0.TryGetValue(text3, out num))
                {
                    uint uint_2;
                    string text5;
                    switch (num)
                    {
                        case 0:
                            using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                            {
                                HabboIM.GetGame().GetItemManager().method_0(class2);
                                goto IL_1379;
                            }
                            break;
                        case 1:
                        case 2:
                            break;
                        case 3:
                            {
                                Habbo class3 = HabboIM.GetGame().GetClientManager().method_2(Convert.ToUInt32(text2)).GetHabbo();
                                if (class3 != null)
                                {
                                    using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                                    {
                                        class3.method_1(class2);
                                    }
                                }
                                goto IL_1379;
                            }
                        case 4:
                            {
                                uint num2 = uint.Parse(text2.Split(new char[]
                        {
                            ' '
                        })[0]);
                                uint num3 = uint.Parse(text2.Split(new char[]
                        {
                            ' '
                        })[1]);
                                GameClient class4 = HabboIM.GetGame().GetClientManager().method_2(num2);
                                Room class5 = HabboIM.GetGame().GetRoomManager().GetRoom(num3);
                                if (class4 != null)
                                {
                                    ServerMessage Message7 = new ServerMessage(286u);
                                    Message7.AppendBoolean(class5.IsPublic);
                                    Message7.AppendUInt(num3);
                                    class4.SendMessage(Message7);
                                    goto IL_1379;
                                }
                                goto IL_1379;
                            }
                        case 5:
                            {
                                Habbo class3 = HabboIM.GetGame().GetClientManager().method_2(Convert.ToUInt32(text2)).GetHabbo();
                                if (class3 != null)
                                {
                                    class3.UpdateRights();
                                    goto IL_1379;
                                }
                                goto IL_1379;
                            }
                        case 6:
                        case 7:
                            {
                                uint num2 = uint.Parse(text2.Split(new char[]
                        {
                            ' '
                        })[0]);
                                uint uint_ = uint.Parse(text2.Split(new char[]
                        {
                            ' '
                        })[1]);
                                int int_ = int.Parse(text2.Split(new char[]
                        {
                            ' '
                        })[2]);
                                string string_ = text2.Substring(num2.ToString().Length + uint_.ToString().Length + int_.ToString().Length + 3);
                                HabboIM.GetGame().GetCatalog().method_7(string_, num2, uint_, int_);
                                goto IL_1379;
                            }
                        case 8:
                            {
                                uint_2 = uint.Parse(text2);
                                Room class5 = HabboIM.GetGame().GetRoomManager().GetRoom(uint_2);
                                HabboIM.GetGame().GetRoomManager().method_16(class5);
                                goto IL_1379;
                            }
                        case 9:
                            {
                                uint num3 = uint.Parse(text2.Split(new char[]
                        {
                            ' '
                        })[0]);
                                Room class5 = HabboIM.GetGame().GetRoomManager().GetRoom(num3);
                                if (class5 != null)
                                {
                                    string string_2 = text2.Substring(num3.ToString().Length + 1);
                                    for (int i = 0; i < class5.RoomUsers.Length; i++)
                                    {
                                        RoomUser class6 = class5.RoomUsers[i];
                                        if (class6 != null)
                                        {
                                            class6.GetClient().SendNotification(string_2);
                                        }
                                    }
                                    goto IL_1379;
                                }
                                goto IL_1379;
                            }
                        case 10:
                            {
                                int int_2 = int.Parse(text2.Split(new char[]
                        {
                            ' '
                        })[0]);
                                using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                                {
                                    Groups.smethod_3(class2, int_2);
                                    goto IL_1379;
                                }
                                goto IL_5BF;
                            }
                        case 11:
                            goto IL_5BF;
                        case 12:
                            goto IL_119A;
                        case 13:
                        case 14:
                            using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                            {
                                ChatCommandHandler.InitWords(class2);
                                goto IL_1379;
                            }
                            goto IL_5F7;
                        case 15:
                            goto IL_5F7;
                        case 16:
                            using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                            {
                                HabboIM.GetGame().LoadServerSettings(class2);
                                goto IL_1379;
                            }
                            goto IL_62F;
                        case 17:
                            goto IL_62F;
                        case 18:
                            client = HabboIM.GetGame().GetClientManager().method_2(uint.Parse(text2));
                            if (client != null)
                            {
                                client.GetHabbo().UpdateVipPoints(true, false);
                                goto IL_1379;
                            }
                            goto IL_1379;
                        case 19:
                            using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                            {
                                HabboIM.GetGame().GetBanManager().Initialise(class2);
                            }
                            HabboIM.GetGame().GetClientManager().method_28();
                            goto IL_1379;
                        case 20:
                            using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                            {
                                HabboIM.GetGame().GetBotManager().method_0(class2);
                                goto IL_1379;
                            }
                            goto IL_6F1;
                        case 21:
                            goto IL_6F1;
                        case 22:
                            using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                            {
                                class2.ExecuteQuery(text2, 30);
                                goto IL_1379;
                            }
                            goto IL_727;
                        case 23:
                            goto IL_727;
                        case 24:
                            {
                                ServerMessage Message8 = new ServerMessage(134u);
                                Message8.AppendUInt(0u);
                                Message8.AppendString("System: " + text2);
                                HabboIM.GetGame().GetClientManager().method_16(Message8, Message8);
                                goto IL_1379;
                            }
                        case 25:
                            {
                                ServerMessage Message9 = new ServerMessage(808u);
                                Message9.AppendStringWithBreak(HabboIMEnvironment.GetExternalText("mus_ha_title"));
                                Message9.AppendStringWithBreak(text2);
                                ServerMessage Message10 = new ServerMessage(161u);
                                Message10.AppendStringWithBreak(text2);
                                HabboIM.GetGame().GetClientManager().method_15(Message9, Message10);
                                goto IL_1379;
                            }
                        case 26:
                            {
                                string text4 = text2.Split(new char[]
                        {
                            ' '
                        })[0];
                                text5 = text2.Substring(text4.Length + 1);
                                ServerMessage Message11 = new ServerMessage(161u);
                                Message11.AppendStringWithBreak(string.Concat(new string[]
                        {
                            HabboIMEnvironment.GetExternalText("mus_hal_title"),
                            "\r\n",
                            text5,
                            "\r\n-",
                            HabboIMEnvironment.GetExternalText("mus_hal_tail")
                        }));
                                Message11.AppendStringWithBreak(text4);
                                HabboIM.GetGame().GetClientManager().BroadcastMessage(Message11);
                                goto IL_1379;
                            }
                        case 27:
                        case 28:
                            {
                                uint_2 = uint.Parse(text2);
                                client = HabboIM.GetGame().GetClientManager().method_2(uint_2);
                                using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                                {
                                    dataRow = class2.ReadDataRow("SELECT look,gender,motto,mutant_penalty,block_newfriends FROM users WHERE id = '" + client.GetHabbo().Id + "' LIMIT 1", 30);
                                }
                                client.GetHabbo().Figure = (string)dataRow["look"];
                                client.GetHabbo().Gender = dataRow["gender"].ToString().ToLower();
                                client.GetHabbo().Motto = HabboIM.FilterString((string)dataRow["motto"]);
                                client.GetHabbo().BlockNewFriends = HabboIM.StringToBoolean(dataRow["block_newfriends"].ToString());
                                ServerMessage Message12 = new ServerMessage(266u);
                                Message12.AppendInt32(-1);
                                Message12.AppendStringWithBreak(client.GetHabbo().Figure);
                                Message12.AppendStringWithBreak(client.GetHabbo().Gender.ToLower());
                                Message12.AppendStringWithBreak(client.GetHabbo().Motto);
                                client.SendMessage(Message12);
                                if (client.GetHabbo().InRoom)
                                {
                                    Room class5 = HabboIM.GetGame().GetRoomManager().GetRoom(client.GetHabbo().CurrentRoomId);
                                    RoomUser class7 = class5.GetRoomUserByHabbo(client.GetHabbo().Id);
                                    ServerMessage Message13 = new ServerMessage(266u);
                                    Message13.AppendInt32(class7.VirtualId);
                                    Message13.AppendStringWithBreak(client.GetHabbo().Figure);
                                    Message13.AppendStringWithBreak(client.GetHabbo().Gender.ToLower());
                                    Message13.AppendStringWithBreak(client.GetHabbo().Motto);
                                    Message13.AppendInt32(client.GetHabbo().AchievementScore);
                                    Message13.AppendStringWithBreak("");
                                    class5.SendMessage(Message13, null);
                                }
                                text3 = text.ToLower();
                                if (text3 == null)
                                {
                                    goto IL_1379;
                                }
                                if (text3 == "updatemotto")
                                {
                                    client.GetHabbo().MottoAchievementsCompleted();
                                    goto IL_1379;
                                }
                                if (text3 == "updatelook")
                                {
                                    client.GetHabbo().AvatarLookAchievementsCompleted();
                                    goto IL_1379;
                                }
                                goto IL_1379;
                            }

                        case 33:
                            {

                                HabboIM.GetGame().GetClientManager().method_WHISPER(text2);



                                goto IL_1379;
                            }
                        case 29:
                            {
                                int QuestionID = int.Parse(text2);
                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {
                                    DataRow QuestionRow = dbClient.ReadDataRow("SELECT * FROM infobus_questions WHERE id='" + QuestionID + "' LIMIT 1", 30);
                                    string Question = dbClient.ReadString("SELECT question FROM infobus_questions WHERE id = '" + QuestionID + "' LIMIT 1", 30);
                                    DataTable AnswersTable = dbClient.ReadDataTable("SELECT * FROM infobus_answers WHERE question_id='" + QuestionID + "'", 30);
                                    Room PollRoom = HabboIM.GetGame().GetClientManager().GetClientByHabbo(QuestionRow["owner"].ToString()).GetHabbo().CurrentRoom;
                                    PollRoom.CurrentPollId = QuestionID;
                                    GameClient TargetUser = HabboIM.GetGame().GetClientManager().GetClientByHabbo(QuestionRow["owner"].ToString());
                                    if (PollRoom.Owner == QuestionRow["owner"].ToString())
                                    {
                                        ServerMessage InfobusQuestion = new ServerMessage(79u);
                                        InfobusQuestion.AppendStringWithBreak(Question);
                                        InfobusQuestion.AppendInt32(AnswersTable.Rows.Count);
                                        if (AnswersTable != null)
                                        {
                                            foreach (DataRow Row in AnswersTable.Rows)
                                            {
                                                InfobusQuestion.AppendInt32((int)Row["id"]);
                                                InfobusQuestion.AppendStringWithBreak((string)Row["answer_text"]);
                                            }
                                        }
                                        PollRoom.SendMessage(InfobusQuestion, null);
                                        Thread Infobus = new Thread(delegate ()
                                        {
                                            Room.ShowResults(PollRoom, QuestionID, TargetUser);
                                        });
                                        Infobus.Start();
                                    }
                                }
                                goto IL_1379;
                            }
                        case 30:
                            {
                                uint UserId = uint.Parse(text2.Split(new char[]
                        {
                            ' '
                        })[0]);
                                string BadgeCode = text2.Split(new char[]
                        {
                            ' '
                        })[1];
                                GameClient Session = HabboIM.GetGame().GetClientManager().GetClientByHabbo(HabboIM.GetGame().GetClientManager().GetNameById(UserId));
                                Session.GetHabbo().GetBadgeComponent().SendBadge(Session, BadgeCode, true);
                                goto IL_1379;
                            }
                        case 31:
                            {
                                uint UserId = uint.Parse(text2.Split(new char[]
                        {
                            ' '
                        })[0]);
                                string RoomModel = text2.Split(new char[]
                        {
                            ' '
                        })[1];
                                string Caption = text2.Split(new char[]
                        {
                            ' '
                        })[2];
                                GameClient Session = HabboIM.GetGame().GetClientManager().GetClientByHabbo(HabboIM.GetGame().GetClientManager().GetNameById(UserId));
                                uint uint_ = 0u;
                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {
                                    dbClient.AddParamWithValue("caption", Caption);
                                    dbClient.AddParamWithValue("model", RoomModel);
                                    dbClient.AddParamWithValue("username", Session.GetHabbo().Username);
                                    dbClient.ExecuteQuery("INSERT INTO rooms (roomtype,caption,owner,model_name) VALUES ('private',@caption,@username,@model)", 30);
                                    Session.GetHabbo().GetUserDataFactory().SetRooms(dbClient.ReadDataTable("SELECT * FROM rooms WHERE owner = @username ORDER BY Id ASC", 30));
                                    uint_ = (uint)dbClient.ReadDataRow("SELECT Id FROM rooms WHERE owner = @username AND caption = @caption ORDER BY Id DESC", 30)[0];
                                    Session.GetHabbo().method_1(dbClient);
                                }
                                RoomData result = HabboIM.GetGame().GetRoomManager().method_12(uint_);
                                if (result != null)
                                {
                                    ServerMessage Message8 = new ServerMessage(59u);
                                    Message8.AppendUInt(result.Id);
                                    Message8.AppendStringWithBreak(result.Name);
                                    Session.SendMessage(Message8);
                                }
                                goto IL_1379;
                            }
                        case 32:
                            uint_2 = uint.Parse(text2);
                            client = HabboIM.GetGame().GetClientManager().method_2(uint_2);
                            using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                            {
                                dataRow = class2.ReadDataRow("SELECT gc,block_newfriends,accept_trading,raumalert,werbercmd,changename FROM users WHERE id = '" + client.GetHabbo().Id + "' LIMIT 1", 30);
                            }
                            client.GetHabbo().Raumalert = HabboIM.StringToBoolean(dataRow["raumalert"].ToString());
                            client.GetHabbo().WerberCmd = HabboIM.StringToBoolean(dataRow["werbercmd"].ToString());
                            client.GetHabbo().BlockNewFriends = HabboIM.StringToBoolean(dataRow["block_newfriends"].ToString());
                            client.GetHabbo().TradingDisabled = HabboIM.StringToBoolean(dataRow["accept_trading"].ToString());
                            client.GetHabbo().ChangeName = HabboIM.StringToBoolean(dataRow["changename"].ToString());
                            client.GetHabbo().gc = (int)dataRow["gc"];
                            goto IL_1379;

                        default:
                            goto IL_1379;
                    }
                    using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                    {
                        HabboIM.GetGame().GetCatalog().method_0(class2);
                    }
                    HabboIM.GetGame().GetCatalog().method_1();
                    HabboIM.GetGame().GetClientManager().BroadcastMessage(new ServerMessage(441u));
                    goto IL_1379;
                IL_5BF:
                    uint_2 = uint.Parse(text2);
                    using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                    {
                        HabboIM.GetGame().GetClientManager().method_2(uint_2).GetHabbo().method_0(class2);
                        goto IL_1379;
                    }
                    goto IL_119A;
                IL_5F7:
                    client = HabboIM.GetGame().GetClientManager().method_2(uint.Parse(text2));
                    if (client != null)
                    {
                        int int_3 = 0;
                        using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                        {
                            int_3 = (int)class2.ReadDataRow("SELECT credits FROM users WHERE id = '" + client.GetHabbo().Id + "' LIMIT 1", 30)[0];
                        }
                        client.GetHabbo().Credits = int_3;
                        client.GetHabbo().UpdateCredits(false);
                        goto IL_1379;
                    }
                    goto IL_1379;
                IL_62F:
                    client = HabboIM.GetGame().GetClientManager().method_2(uint.Parse(text2));
                    if (client != null)
                    {
                        int int_4 = 0;
                        using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                        {
                            int_4 = (int)class2.ReadDataRow("SELECT activity_points FROM users WHERE id = '" + client.GetHabbo().Id + "' LIMIT 1", 30)[0];
                        }
                        client.GetHabbo().ActivityPoints = int_4;
                        client.GetHabbo().UpdateActivityPoints(false);
                        goto IL_1379;
                    }
                    goto IL_1379;
                IL_6F1:
                    HabboIM.GetGame().GetClientManager().method_2(uint.Parse(text2)).method_12();
                    goto IL_1379;
                IL_727:
                    string text6 = text2.Split(new char[]
                    {
                        ' '
                    })[0];
                    text5 = text2.Substring(text6.Length + 1);
                    ServerMessage Message14 = new ServerMessage(808u);
                    Message14.AppendStringWithBreak(HabboIMEnvironment.GetExternalText("mus_alert_title"));
                    Message14.AppendStringWithBreak(text5);
                    HabboIM.GetGame().GetClientManager().method_2(uint.Parse(text6)).SendMessage(Message14);
                    goto IL_1378;
                IL_119A:
                    HabboIM.Close();
                }
            IL_1378:;
            }
        IL_1379:
            ServerMessage Message15 = new ServerMessage(1u);
            Message15.AppendString("Hallo Housekeeping :)");
            this.ClientSocket.Send(Message15.GetBytes());
        }
    }
}
