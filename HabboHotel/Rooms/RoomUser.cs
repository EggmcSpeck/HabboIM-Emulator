using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Items;
using HabboIM.HabboHotel.Misc;
using HabboIM.HabboHotel.Pathfinding;
using HabboIM.HabboHotel.Pets;
using HabboIM.HabboHotel.RoomBots;
using HabboIM.HabboHotel.Rooms.Games;
using HabboIM.HabboHotel.Users;
using HabboIM.Messages;
using System.Threading;
using System.Threading.Tasks;
using HabboIM.Core;
using HabboIM.Storage;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace HabboIM.HabboHotel.Rooms
{
    internal sealed class RoomUser
    {
        public uint UId;

        public string knasttimer;

        public bool fastwalk;
        public bool fasterwalk;
        public int VirtualId;

        public uint RoomId;

        public int int_1;

        internal int int_2;

        public int X;

        public int Y;

        public double double_0;

        internal byte byte_0;

        public int CarryItemID;

        public int int_6;

        public int int_7;

        public int BodyRotation;

        public bool bool_0;

        public bool bool_1;

        public bool TeleportMode;

        public int int_9;

        public int int_10;

        public int int_11;

        public bool bool_3;

        public bool bool_4;

        public int int_12;

        public int int_13;

        public double double_1;

        public RoomBot RoomBot;

        public BotAI BotAI;

        public Pet PetData;

        internal byte byte_1;

        internal bool bool_5;

        public RoomUser RoomUser_0;

        public RoomItem RoomItem_0;

        public RoomBot class34_1;

        public bool bool_6;

        public bool UpdateNeeded;

        public bool bool_8;

        public int int_14;

        public Dictionary<string, string> Statusses;

        internal Team team;

        internal HabboHotel.Rooms.Games.Game game;

        public int DanceId;

        public int int_16;

        public bool bool_10;

        public int int_17;

        public int int_18;

        public int int_19;

        internal bool bool_11;

        internal bool bool_12;

        internal string string_0;

        internal int int_20;

        internal FreezePowerUp freezePowerUp;

        internal bool Freezed;

        internal int FreezeLives;

        internal bool shieldActive;

        internal int shieldCounter;

        internal int FreezeRange;

        internal int FreezeCounter;

        internal int FreezeBalls;
        internal bool isFlying;
        internal int flyk;

        public ThreeDCoord Position
        {
            get
            {
                return new ThreeDCoord(this.X, this.Y);
            }
        }

        public bool IsPet
        {
            get
            {
                return this.IsBot && this.RoomBot.Boolean_0;
            }
        }

        internal bool IsDancing
        {
            get
            {
                return this.DanceId >= 1;
            }
        }

        internal bool Boolean_2
        {
            get
            {
                return !this.IsBot && this.int_1 >= ServerConfiguration.KickTimer;
            }
        }

        internal bool Boolean_3
        {
            get
            {
                return !this.IsBot && this.Statusses.ContainsKey("trd");
            }
        }

        internal bool IsBot
        {
            get
            {
                return this.RoomBot != null;
            }
        }

        public RoomUser(uint UserId, uint RoomId, int VirtualId, bool Invisible)
        {
            this.isFlying = false;
            this.flyk = 0;
            this.bool_5 = false;
            this.UId = UserId;
            this.RoomId = RoomId;
            this.VirtualId = VirtualId;
            this.int_1 = 0;
            this.X = 0;
            this.Y = 0;
            this.double_0 = 0.0;
            this.int_7 = 0;
            this.BodyRotation = 0;
            this.UpdateNeeded = true;
            this.Statusses = new Dictionary<string, string>();
            this.int_16 = 0;
            this.int_19 = -1;
            this.RoomUser_0 = null;
            this.bool_1 = false;
            this.bool_0 = true;
            this.bool_11 = false;
            this.byte_0 = 3;
            this.int_2 = 0;
            this.int_20 = 0;
            this.byte_1 = 0;
            this.bool_12 = Invisible;
            this.string_0 = "";
        }

        public void Unidle()
        {
            this.int_1 = 0;
            if (this.bool_8)
            {
                this.bool_8 = false;
                ServerMessage Message = new ServerMessage(486u);
                Message.AppendInt32(this.VirtualId);
                Message.AppendBoolean(false);
                this.GetRoom().SendMessage(Message, null);
                this.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                this.GetClient().GetHabbo().Whisper("Willkommen zurück!");
            }
        }

        internal void HandleSpeech(GameClient Session, string str, bool bool_13)
        {



            if (!this.IsBot)
            {
                if (Session.GetHabbo().jail == 1 && Session.GetHabbo().jailtime > 10 * 60)
                {

                    Session.GetHabbo().Whisper("Du bist mehr als 10 Minuten gebannt und kannst deswegen nicht chatten!");

                    return;




                }




            }

            string object_ = str;
            string linkRegex = "((http|https):\\/\\/|www.)?[a-zA-Z0-9\\-\\.]+\\b(com|co\\.uk|org|net|eu|cf|info|ml|nl|ca|es|fi)\\b";
            if (Session == null || Session.GetHabbo().HasFuse("ignore_roommute") || !this.GetRoom().bool_4)
            {
                this.Unidle();
                if (!this.IsBot && this.GetClient().GetHabbo().IsMuted)
                {
                    this.GetClient().GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("error_muted"));
                }
                else if (!str.StartsWith(":") || Session == null || !ChatCommandHandler.smethod_5(Session, str.Substring(1)))
                {
                    uint num = 24u;
                    if (bool_13)
                    {
                        num = 26u;
                    }
                    if (!this.IsBot && Session.GetHabbo().method_4() > 0)
                    {
                        TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().dateTime_0;
                        if (timeSpan.Seconds > 4)
                        {
                            Session.GetHabbo().int_23 = 0;
                        }
                        if (timeSpan.Seconds < 4 && Session.GetHabbo().int_23 > 5 && !this.IsBot)
                        {
                            ServerMessage Message = new ServerMessage(27u);
                            Message.AppendInt32(Session.GetHabbo().method_4());
                            this.GetClient().SendMessage(Message);
                            this.GetClient().GetHabbo().IsMuted = true;
                            this.GetClient().GetHabbo().int_4 = Session.GetHabbo().method_4();
                            this.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(620, true);
                            this.GetClient().GetHabbo().Whisper("Du wurdest stumm geschaltet, da du zu schnell getippt hast!");

                            return;
                        }
                        Session.GetHabbo().dateTime_0 = DateTime.Now;
                        Session.GetHabbo().int_23++;
                        
                    }
                    bool aws = false;
                    if (!this.IsBot && HabboIM.GetGame().AntiWerberStatus)
                    {

                        str = ChatCommandHandler.smethod_4b(Session, object_, "Raum");
                        str = ChatCommandHandler.amina_zikki(Session, str, "Raum");
                        if (str != object_) aws = true;
                    }
                    if (!this.IsBot)
                    {
                        str = ChatCommandHandler.smethod_4(str);
                    }
                    if (!this.GetRoom().method_9(this, str))
                    {
                        ServerMessage Message2 = new ServerMessage(num);
                        Message2.AppendInt32(this.VirtualId);
                        if (!this.IsBot && !this.IsPet)
                        {
                            try
                            {
                                if (HabboIM.GetConfig().data.ContainsKey("anti.ads.enable") && HabboIM.GetConfig().data["anti.ads.enable"] == "1")
                                {
                                    if (Session.GetHabbo().Rank <= uint.Parse(HabboIM.GetConfig().data["anti.ads.rank"]))
                                    {
                                        if (Regex.IsMatch(str, linkRegex, RegexOptions.IgnoreCase))
                                        {
                                            Session.SendNotification(HabboIM.GetConfig().data["anti.ads.msg"]);
                                            return;
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                        if (str.Contains("http://") || str.Contains("www.") || str.Contains("https://"))
                        {
                            string[] array = str.Split(new char[]
                            {
                                ' '
                            });
                            int num2 = 0;
                            string text = "";
                            string text2 = "";
                            string[] array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                string text3 = array2[i];
                                if (ChatCommandHandler.InitLinks(text3))
                                {
                                    if (num2 > 0)
                                    {
                                        text += ",";
                                    }
                                    text += text3;
                                    object obj = text2;
                                    text2 = string.Concat(new object[]
                                    {
                                        obj,
                                        " {",
                                        num2,
                                        "}"
                                    });
                                    num2++;
                                }
                                else
                                {
                                    text2 = text2 + " " + text3;
                                }
                            }
                            text = text2;
                            string[] array3 = text.Split(new char[]
                            {
                                ','
                            });
                            Message2.AppendStringWithBreak(text);
                            if (array3.Length > 0)
                            {
                                Message2.AppendBoolean(false);
                                Message2.AppendInt32(num2);
                                array2 = array3;
                                for (int i = 0; i < array2.Length; i++)
                                {
                                    string text4 = array2[i];
                                    string text5 = ChatCommandHandler.smethod_3(text4.Replace("http://", "").Replace("https://", ""));
                                    Message2.AppendStringWithBreak(text5.Replace("http://", "").Replace("https://", ""));
                                    Message2.AppendStringWithBreak(text4);
                                    Message2.AppendBoolean(false);
                                }
                            }
                        }
                        else
                        {
                            Message2.AppendStringWithBreak(str);
                        }
                        Message2.AppendInt32(this.ParseEmoticon(str));

                        try
                        {
                            if (this.ParseSmileys(str) != 0 && Session.GetHabbo().GetEffectsInventoryComponent().int_0 == 0)
                            {


                                Session.GetHabbo().GetEffectsInventoryComponent().method_2(this.ParseSmileys(str), true);


                                Thread thread = new Thread(delegate ()
                                {
                                    try
                                    {
                                        Thread.Sleep(3000);
                                        Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);

                                    }
                                    catch
                                    {
                                    }
                                });

                                thread.Start();


                            }
                        } catch
                        {

                            Console.WriteLine("Error in Smiley to Enable Function.");
                        }


                        Message2.AppendBoolean(false);
                        if (!this.IsBot)
                        {

                            if (aws == false)
                                this.GetRoom().method_58(Message2, Session.GetHabbo().list_2, Session.GetHabbo().Id);
                        }
                        else
                        {
                            this.GetRoom().SendMessage(Message2, null);
                        }
                    }
                    else if (!this.IsBot)
                    {
                        Session.GetHabbo().Whisper(str);
                    }
                    if (!this.IsBot)
                    {
                        this.GetRoom().method_7(this, str, bool_13);
                        if (Session.GetHabbo().CurrentQuestId > 0u && HabboIM.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "CHAT_WITH_SOMEONE")
                        {
                            HabboIM.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
                        }
                    }
                    if (ServerConfiguration.EnableChatlog && !this.IsBot)
                    {
                        using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
                        {
                            @class.AddParamWithValue("message", object_);
                            @class.ExecuteQuery(string.Concat(new object[]
                            {
                                "INSERT INTO chatlogs (user_id,room_id,hour,minute,timestamp,message,user_name,full_date) VALUES ('",
                                Session.GetHabbo().Id,
                                "','",
                                this.GetRoom().Id,
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

                }
            }
        }


        internal int ParseSmileys(string string_1)
        {
            string_1 = string_1.ToLower();
            int result;
            if (string_1.Contains(":d") ||  string_1.Contains("=d"))
            {
                result = 1;
            }
            else if (string_1.Contains(":)") || string_1.Contains("=]") || string_1.Contains(":>"))
            {
                result = 2;
            }
            else if (string_1.Contains(">:(") || string_1.Contains(":@"))
            {
                result = 3;
            }
            else if (string_1.Contains(":o") || string_1.Contains(";o") || string_1.Contains("omg"))
            {
                result = 4;
            }
            else if (string_1.Contains(":(") || string_1.Contains(";<") || string_1.Contains("=[") || string_1.Contains(":'(") || string_1.Contains("='[") || string_1.Contains(":<"))
            {
                result = 5;
            }
            else if (string_1.Contains(";)"))
            {
                result = 6;
            }
            else if (string_1.Contains(":p") || string_1.Contains(";p"))
            {
                result = 7;
            }
            else if (string_1.Contains("8)") || string_1.Contains("cool"))
            {
                result = 8;
            }
            else if (string_1.Contains(":$"))
            {
                result = 9;
            }
            else if (string_1.Contains("|") || string_1.Contains("<3"))
            {
                result = 10;
            }
            else if (string_1.Contains("haha") || string_1.Contains("xd"))
            {
                result = 12;
            }
            else if (string_1.Contains("scheisse") || string_1.Contains("shit"))
            {
                result = 92;
            }
            else
            {
                result = 0;
            }
            return result;


        }



        internal int ParseEmoticon(string string_1)
        {
            string_1 = string_1.ToLower();
            int result;
            if (string_1.Contains(":)") || string_1.Contains(":d") || string_1.Contains("=]") || string_1.Contains("=d") || string_1.Contains(":>"))
            {
                result = 1;
            }
            
            else if (string_1.Contains(">:(") || string_1.Contains(":@"))
            {
                result = 2;
            }
            else if (string_1.Contains(":o") || string_1.Contains(";o"))
            {
                result = 3;
            }
            else if (string_1.Contains(":(") || string_1.Contains(";<") || string_1.Contains("=[") || string_1.Contains(":'(") || string_1.Contains("='["))
            {
                result = 4;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        internal void method_3(bool bool_13)
        {
            this.bool_6 = false;
            this.bool_10 = false;
            this.Statusses.Remove("mv");
            this.int_10 = 0;
            this.int_11 = 0;
            this.bool_4 = false;
            this.int_12 = 0;
            this.int_13 = 0;
            this.double_1 = 0.0;
            if (bool_13)
            {
                this.UpdateNeeded = true;
            }
        }

        internal void MoveTo(ThreeDCoord position)
        {
            this.MoveTo(position.x, position.y);
        }

        internal void MoveTo(int x, int y)
        {
            if (this.GetRoom().method_92(x, y) && !this.GetRoom().method_96(x, y))
            {
                this.Unidle();
                this.bool_6 = true;
                this.bool_10 = true;
                this.int_17 = x;
                this.int_18 = y;
                if (x >= this.GetRoom().RoomModel.int_4 || y >= this.GetRoom().RoomModel.int_5)
                {
                    this.int_10 = x;
                    this.int_11 = y;
                }
                else
                {
                    this.int_10 = this.GetRoom().gstruct1_0[x, y].x;
                    this.int_11 = this.GetRoom().gstruct1_0[x, y].y;
                }
            }
        }

        internal void method_6()
        {
            this.bool_1 = false;
            this.bool_0 = true;
        }

        internal void method_7(int int_21, int int_22, double double_2)
        {
            this.X = int_21;
            this.Y = int_22;
            this.double_0 = double_2;
           /* if (this.isFlying)
            {
                this.double_0 += 1.0 + (0.5 * Math.Sin(0.7 * this.flyk));
            }*/
        }

        public void CarryItem(int int_21)
        {
            this.CarryItemID = int_21;
            if (int_21 > 1000)
            {
                this.int_6 = 5000;
            }
            else if (int_21 > 0)
            {
                this.int_6 = 240;
            }
            else
            {
                this.int_6 = 0;
            }
            ServerMessage Message = new ServerMessage(482u);
            Message.AppendInt32(this.VirtualId);
            Message.AppendInt32(int_21);
            this.GetRoom().SendMessage(Message, null);
        }

        public void method_9(int int_21)
        {
            this.method_10(int_21, false);
        }

        public void method_10(int int_21, bool bool_13)
        {
            if (!this.Statusses.ContainsKey("lay") && !this.bool_6)
            {
                int num = this.BodyRotation - int_21;
                this.int_7 = this.BodyRotation;
                if (this.Statusses.ContainsKey("sit") || bool_13)
                {
                    if (this.BodyRotation == 2 || this.BodyRotation == 4)
                    {
                        if (num > 0)
                        {
                            this.int_7 = this.BodyRotation - 1;
                        }
                        else if (num < 0)
                        {
                            this.int_7 = this.BodyRotation + 1;
                        }
                    }
                    else if (this.BodyRotation == 0 || this.BodyRotation == 6)
                    {
                        if (num > 0)
                        {
                            this.int_7 = this.BodyRotation - 1;
                        }
                        else if (num < 0)
                        {
                            this.int_7 = this.BodyRotation + 1;
                        }
                    }
                }
                else if (num <= -2 || num >= 2)
                {
                    this.int_7 = int_21;
                    this.BodyRotation = int_21;
                }
                else
                {
                    this.int_7 = int_21;
                }
                this.UpdateNeeded = true;
            }
        }

        public void AddStatus(string string_1, string string_2)
        {
            this.Statusses[string_1] = string_2;
        }

        public void RemoveStatus(string string_1)
        {
            if (this.Statusses.ContainsKey(string_1))
            {
                this.Statusses.Remove(string_1);
            }
        }

        public void ClearStatuses()
        {
            this.Statusses = new Dictionary<string, string>();
        }

        public void method_14(ServerMessage Message5_0)
        {
            if (Message5_0 != null && !this.bool_11)
            {
                if (!this.IsBot)
                {
                    if (this.GetClient() != null && this.GetClient().GetHabbo() != null)
                    {
                        Habbo @class = this.GetClient().GetHabbo();
                        Message5_0.AppendUInt(@class.Id);
                        Message5_0.AppendStringWithBreak(@class.Username);
                        Message5_0.AppendStringWithBreak(@class.Motto);
                        Message5_0.AppendStringWithBreak((@class.PetData != null) ? @class.PetData : @class.Figure);
                        Message5_0.AppendInt32(this.VirtualId);
                        Message5_0.AppendInt32(this.X);
                        Message5_0.AppendInt32(this.Y);
                       /* if (this.isFlying)
                        {
                            this.double_0 += 1.0 + (0.5 * Math.Sin(0.7 * this.flyk));
                            Message5_0.AppendStringWithBreak(this.double_0.ToString().Replace(',', '.'));
                        }
                        else
                        {
                            Message5_0.AppendStringWithBreak(this.double_0.ToString().Replace(',', '.'));
                        }*/
                        Message5_0.AppendStringWithBreak(this.double_0.ToString().Replace(',', '.'));
                        if (@class.PetData != null)
                        {
                            Message5_0.AppendInt32(4);
                            Message5_0.AppendInt32(2);
                            Message5_0.AppendInt32(0);
                        }
                        else
                        {
                            Message5_0.AppendInt32(2);
                            Message5_0.AppendInt32(1);
                            Message5_0.AppendStringWithBreak(@class.Gender.ToLower());
                            Message5_0.AppendInt32(-1);
                            if (@class.int_0 > 0)
                            {
                                Message5_0.AppendInt32(@class.int_0);
                            }
                            else
                            {
                                Message5_0.AppendInt32(-1);
                            }
                            Message5_0.AppendInt32(-1);
                            Message5_0.AppendStringWithBreak("");
                            Message5_0.AppendInt32(@class.AchievementScore);
                        }
                    }
                }
                else
                {
                    Message5_0.AppendInt32(this.BotAI.int_0);
                    Message5_0.AppendStringWithBreak(this.RoomBot.Name);
                    Message5_0.AppendStringWithBreak(this.RoomBot.Motto);
                    Message5_0.AppendStringWithBreak(this.RoomBot.Look);
                    Message5_0.AppendInt32(this.VirtualId);
                    Message5_0.AppendInt32(this.X);
                    Message5_0.AppendInt32(this.Y);
                    Message5_0.AppendStringWithBreak(this.double_0.ToString().Replace(',', '.'));
                    Message5_0.AppendInt32(4);
                    Message5_0.AppendInt32((this.RoomBot.AiType == AIType.const_0) ? 2 : 3);
                    if (this.RoomBot.AiType == AIType.const_0)
                    {
                        Message5_0.AppendInt32(0);
                    }
                }
            }
        }

        public void method_15(ServerMessage Message5_0)
        {
            if (!this.bool_11)
            {
                Message5_0.AppendInt32(this.VirtualId);
                Message5_0.AppendInt32(this.X);
                Message5_0.AppendInt32(this.Y);
                Message5_0.AppendStringWithBreak(this.double_0.ToString().Replace(',', '.'));
                Message5_0.AppendInt32(this.int_7);
                Message5_0.AppendInt32(this.BodyRotation);
                Message5_0.AppendString("/");
                try
                {
                    foreach (KeyValuePair<string, string> current in this.Statusses)
                    {
                        Message5_0.AppendString(current.Key);
                        Message5_0.AppendString(" ");
                        Message5_0.AppendString(current.Value);
                        Message5_0.AppendString("/");
                    }
                }
                catch
                {
                }
                Message5_0.AppendStringWithBreak("/");
            }
        }

        public GameClient GetClient()
        {
            GameClient result;
            if (this.IsBot)
            {
                result = null;
            }
            else
            {
                result = null;
                if (this.UId > 0u)
                {
                    result = HabboIM.GetGame().GetClientManager().method_2(this.UId);
                }
            }
            return result;
        }

        internal Room GetRoom()
        {
            return HabboIM.GetGame().GetRoomManager().GetRoom(this.RoomId);
        }
    }
}
