
using HabboIM.Core;
using HabboIM.HabboHotel.Achievements;
using HabboIM.HabboHotel.Support;
using HabboIM.Messages;
using HabboIM.Net;
using HabboIM.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HabboIM.HabboHotel.Rooms;

namespace HabboIM.HabboHotel.GameClients
{
    internal sealed class GameClientManager
    {
        private Task task_0;

        public bool gc = true;

        public bool wartung = true;


        private GameClient[] Clients;

        private Hashtable hashtable_0;

        private Hashtable hashtable_1;

        private Timer DisposeTimer;

        private List<SocketConnection> DisposeQueue;

        public int ClientCount
        {
            get
            {
                int result;
                if (this.Clients == null)
                {
                    result = 0;
                }
                else
                {
                    int num = 0;
                    for (int i = 0; i < this.Clients.Length; i++)
                    {
                        if (this.Clients[i] != null && this.Clients[i].GetHabbo() != null && !string.IsNullOrEmpty(this.Clients[i].GetHabbo().Username))
                        {
                            num++;
                        }
                    }
                    result = num;
                }
                return result;
            }
        }

        public GameClientManager(int clientCapacity)
        {
            this.hashtable_0 = new Hashtable();
            this.hashtable_1 = new Hashtable();
            this.Clients = new GameClient[clientCapacity];
            this.DisposeQueue = new List<SocketConnection>();
            this.DisposeTimer = new Timer(new TimerCallback(this.DisposeTimerCallback), null, 500, 500);
        }

        public void method_0(uint uint_0, string string_0, GameClient class16_1)
        {
            this.hashtable_0[uint_0] = class16_1;
            this.hashtable_1[string_0.ToLower()] = class16_1;
        }

        public void method_1(uint uint_0, string string_0)
        {
            this.hashtable_0[uint_0] = null;
            this.hashtable_1[string_0.ToLower()] = null;
        }

        public GameClient method_2(uint id)
        {
            GameClient result;
            if (this.Clients == null || this.hashtable_0 == null)
            {
                result = null;
            }
            else if (this.hashtable_0.ContainsKey(id))
            {
                result = (GameClient)this.hashtable_0[id];
            }
            else
            {
                result = null;
            }
            return result;
        }

        public GameClient GetClientByHabbo(string string_0)
        {
            GameClient result;
            if (this.Clients == null || this.hashtable_1 == null || string.IsNullOrEmpty(string_0))
            {
                result = null;
            }
            else if (this.hashtable_1.ContainsKey(string_0.ToLower()))
            {
                result = (GameClient)this.hashtable_1[string_0.ToLower()];
            }
            else
            {
                result = null;
            }
            return result;
        }

        public void method_checkstats(GameClient Session)
        {
            int new_level = 0;

            if (Session.GetHabbo().lovepoints > 100)
            {
                int code = Session.GetHabbo().lovepoints % 100;
                if (Session.GetHabbo().lovepoints < 50)
                {
                    new_level = Session.GetHabbo().lovepoints - code;
                }
                else {
                    new_level = Session.GetHabbo().lovepoints + (100 - code);


                }
            }



            if (Session.GetHabbo().lovepoints >= 100 && Session.GetHabbo().bez_level != new_level)
            {



                if(Session.GetHabbo().bez_level == 0 && Session.GetHabbo().lovepoints >= 100)
                {
                    //beschissenescheissbelohnung

                    HabboIM.webSocketManager.getWebSocketByName(Session.GetHabbo().Username).Send("beschissenescheissbelohnung|1");

                    Session.GetHabbo().bez_level = 1;
                    Session.GetHabbo().UpdateBezLevel(true);
                    Session.GetHabbo().GetBadgeComponent().SendBadge(Session, "BZLVL1", true);

                    Session.GetHabbo().ActivityPoints += 5;
                    Session.GetHabbo().UpdateActivityPoints(true);
                    // Beziehungslevel ist nun 1, wie geil ist das denn?

                }

                if(Session.GetHabbo().bez_level == 1 && Session.GetHabbo().lovepoints >= 200)
                {
                    HabboIM.webSocketManager.getWebSocketByName(Session.GetHabbo().Username).Send("beschissenescheissbelohnung|2");
                    Session.GetHabbo().bez_level = 2;
                    Session.GetHabbo().UpdateBezLevel(true);
                    Session.GetHabbo().GetBadgeComponent().RemoveBadge("BZLVL1");
                    Session.GetHabbo().GetBadgeComponent().SendBadge(Session, "BZLVL2", true);
                    Session.GetHabbo().ActivityPoints += 10;
                    Session.GetHabbo().UpdateActivityPoints(true);
                    Session.GetHabbo().Credits += 2000;
                    Session.GetHabbo().UpdateCredits(true);

                }
                if(Session.GetHabbo().bez_level == 2 && Session.GetHabbo().lovepoints >= 300)
                {

                    HabboIM.webSocketManager.getWebSocketByName(Session.GetHabbo().Username).Send("beschissenescheissbelohnung|3");
                    // 300 Beziehungspunkte > Badge + 5 Diamanten + 10 Aktivitätspunkte
                    Session.GetHabbo().bez_level = 3;
                    Session.GetHabbo().UpdateBezLevel(true);
                    Session.GetHabbo().GetBadgeComponent().RemoveBadge("BZLVL2");
                    Session.GetHabbo().GetBadgeComponent().SendBadge(Session, "BZLVL3", true);
                    Session.GetHabbo().ActivityPoints += 10;    //10 AKP
                    Session.GetHabbo().UpdateActivityPoints(true);  //UPDATE AKP
                    Session.GetHabbo().VipPoints += 2;
                    Session.GetHabbo().UpdateVipPoints(false, true);




                }
                if(Session.GetHabbo().bez_level == 3 && Session.GetHabbo().lovepoints >= 400)
                {
                    HabboIM.webSocketManager.getWebSocketByName(Session.GetHabbo().Username).Send("beschissenescheissbelohnung|4");
                    // 400 Beziehungspunkte > Badge + 10 Diamanten + 5 Aktivitätspunkte + 10000 Taler
                    Session.GetHabbo().bez_level = 4;
                    Session.GetHabbo().UpdateBezLevel(true);
                    Session.GetHabbo().GetBadgeComponent().RemoveBadge("BZLVL3");
                    Session.GetHabbo().GetBadgeComponent().SendBadge(Session, "BZLVL4", true);
                    Session.GetHabbo().Credits += 10000;    //10.000 Taler
                    Session.GetHabbo().UpdateCredits(true); //UPDATE TALER
                    Session.GetHabbo().ActivityPoints += 5;    //5 AKP
                    Session.GetHabbo().UpdateActivityPoints(true);  //UPDATE AKP
                    Session.GetHabbo().VipPoints += 3;
                    Session.GetHabbo().UpdateVipPoints(false, true);


                }

                if(Session.GetHabbo().bez_level == 4 && Session.GetHabbo().lovepoints >= 500)
                {
                    HabboIM.webSocketManager.getWebSocketByName(Session.GetHabbo().Username).Send("beschissenescheissbelohnung|5");
                    // 500 Beziehungspunkte > Badge + 15 Diamanten + 30000 Taler
                    Session.GetHabbo().bez_level = 5;
                    Session.GetHabbo().UpdateBezLevel(true);
                    Session.GetHabbo().GetBadgeComponent().RemoveBadge("BZLVL4");
                    Session.GetHabbo().GetBadgeComponent().SendBadge(Session, "BZLVL5", true);
                    Session.GetHabbo().VipPoints += 4;
                    Session.GetHabbo().UpdateVipPoints(false, true);
                    Session.GetHabbo().Credits += 30000;    //30.000 Taler
                    Session.GetHabbo().UpdateCredits(true); //UPDATE TALER



                }
                if(Session.GetHabbo().bez_level == 5 && Session.GetHabbo().lovepoints >= 600)
                {
                    HabboIM.webSocketManager.getWebSocketByName(Session.GetHabbo().Username).Send("beschissenescheissbelohnung|6");
                    // 600 Beziehungspunkte > Badge + 75000 Taler
                    Session.GetHabbo().bez_level = 6;
                    Session.GetHabbo().UpdateBezLevel(true);
                    Session.GetHabbo().GetBadgeComponent().RemoveBadge("BZLVL5");
                    Session.GetHabbo().GetBadgeComponent().SendBadge(Session, "BZLVL6", true);
                    Session.GetHabbo().Credits += 75000;    //75.000 Taler
                    Session.GetHabbo().UpdateCredits(true); //UPDATE TALER


                }

                if (Session.GetHabbo().bez_level == 6 && Session.GetHabbo().lovepoints >= 700)
                {
                    HabboIM.webSocketManager.getWebSocketByName(Session.GetHabbo().Username).Send("beschissenescheissbelohnung|7");
                    // 700 Beziehungspunkte > Badge + 50 Diamanten + 10 Aktivitätspunkte
                    Session.GetHabbo().bez_level = 7;
                    Session.GetHabbo().UpdateBezLevel(true);
                    Session.GetHabbo().GetBadgeComponent().RemoveBadge("BZLVL6");
                    Session.GetHabbo().GetBadgeComponent().SendBadge(Session, "BZLVL7", true);
                    Session.GetHabbo().VipPoints += 5;
                    Session.GetHabbo().UpdateVipPoints(false, true);
                    Session.GetHabbo().ActivityPoints += 10;    //10 AKP
                    Session.GetHabbo().UpdateActivityPoints(true);  //UPDATE AKP

                }
                if (Session.GetHabbo().bez_level == 7 && Session.GetHabbo().lovepoints >= 800)
                {
                    HabboIM.webSocketManager.getWebSocketByName(Session.GetHabbo().Username).Send("beschissenescheissbelohnung|8");
                    // 800 Beziehungspunkte > Badge + 35000 Taler
                    Session.GetHabbo().bez_level = 8;
                    Session.GetHabbo().UpdateBezLevel(true);
                    Session.GetHabbo().GetBadgeComponent().RemoveBadge("BZLVL7");
                    Session.GetHabbo().GetBadgeComponent().SendBadge(Session, "BZLVL8", true);
                    Session.GetHabbo().Credits += 35000;    //35.000 Taler
                    Session.GetHabbo().UpdateCredits(true); //UPDATE TALER

                }

                if (Session.GetHabbo().bez_level == 8 && Session.GetHabbo().lovepoints >= 900)
                {
                    HabboIM.webSocketManager.getWebSocketByName(Session.GetHabbo().Username).Send("beschissenescheissbelohnung|9");
                    // 900 Beziehungspunkte > Badge + 40 Diamanten
                    Session.GetHabbo().bez_level = 9;
                    Session.GetHabbo().UpdateBezLevel(true);
                    Session.GetHabbo().GetBadgeComponent().RemoveBadge("BZLVL8");
                    Session.GetHabbo().GetBadgeComponent().SendBadge(Session, "BZLVL9", true);
                    Session.GetHabbo().VipPoints += 6;
                    Session.GetHabbo().UpdateVipPoints(false, true);

                }


                if (Session.GetHabbo().bez_level == 9 && Session.GetHabbo().lovepoints >= 1000)
                {
                    HabboIM.webSocketManager.getWebSocketByName(Session.GetHabbo().Username).Send("beschissenescheissbelohnung|10");
                    // 1000 Beziehungspunkte > Badge + 20 Aktivitätspunkte + 75 Diamanten
                    Session.GetHabbo().bez_level = 10;
                    Session.GetHabbo().UpdateBezLevel(true);
                    Session.GetHabbo().GetBadgeComponent().RemoveBadge("BZLVL9");
                    Session.GetHabbo().GetBadgeComponent().SendBadge(Session, "BZLVL10", true);
                    Session.GetHabbo().VipPoints += 7;
                    Session.GetHabbo().UpdateVipPoints(false, true);


                }

            }



        }


        private void DisposeTimerCallback(object sender)
        {
            try
            {
                List<SocketConnection> list = this.DisposeQueue;
                this.DisposeQueue = new List<SocketConnection>();
                if (list != null)
                {
                    foreach (SocketConnection current in list)
                    {
                        if (current != null)
                        {
                            current.method_1();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogThreadException(ex.ToString(), "Disconnector task");
            }
        }

        internal void DisposeConnection(SocketConnection connection)
        {
            if (!this.DisposeQueue.Contains(connection))
            {
                this.DisposeQueue.Add(connection);
            }
        }
        internal void SendToStaffs(ServerMessage Message5_0, ServerMessage Message5_1)
        {
            byte[] byte_ = Message5_0.GetBytes();
            byte[] byte_2 = Message5_1.GetBytes();
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null)
                {
                    try
                    {
                        if (@class.GetHabbo().HasFuse("receive_sa"))
                        {
                            if (@class.GetHabbo().InRoom)
                            {
                                @class.GetConnection().SendData(byte_);
                            }
                            else
                            {
                                @class.GetConnection().SendData(byte_2);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void SendToStaffs(GameClient class16_1, ServerMessage Message5_0, bool IsAntiAd = true)
        {
            byte[] byte_ = Message5_0.GetBytes();
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && (@class != class16_1 || IsAntiAd))
                {
                    try
                    {
                        if (@class.GetHabbo().HasFuse("receive_sa"))
                        {
                            @class.GetConnection().SendData(byte_);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
        public void method_6()
        {
        }

        public GameClient GetClientById(uint id)
        {
            GameClient result;
            try
            {
                result = this.Clients[(int)((uint)((UIntPtr)id))];
            }
            catch
            {
                result = null;
            }
            return result;
        }

        internal void method_8(uint uint_0, ref SocketConnection Message1_0)
        {
            this.Clients[(int)((uint)((UIntPtr)uint_0))] = new GameClient(uint_0, ref Message1_0);
            this.Clients[(int)((uint)((UIntPtr)uint_0))].GetSocketConnection();
        }

        public void method_9(uint uint_0)
        {
            GameClient @class = this.GetClientById(uint_0);
            if (@class != null)
            {
                @class.method_11();
                this.Clients[(int)((uint)((UIntPtr)uint_0))] = null;
            }
        }

        public void method_10()
        {
            if (this.task_0 == null)
            {
                this.task_0 = new Task(new Action(this.method_12));
                this.task_0.Start();
            }
        }

        public void method_11()
        {
            if (this.task_0 != null)
            {
                this.task_0 = null;
            }
        }

        private void method_12()
        {
            int num = int.Parse(HabboIM.GetConfig().data["client.ping.interval"]);
            if (num <= 100)
            {
                throw new ArgumentException("Invalid configuration value for ping interval! Must be above 100 miliseconds.");
            }
            while (true)
            {
                try
                {
                    ServerMessage Message = new ServerMessage(50u);
                    List<GameClient> list = new List<GameClient>();
                    List<GameClient> list2 = new List<GameClient>();
                    for (int i = 0; i < this.Clients.Length; i++)
                    {
                        GameClient @class = this.Clients[i];
                        if (@class != null)
                        {
                            if (@class.bool_0)
                            {
                                @class.bool_0 = false;
                                list2.Add(@class);
                            }
                            else
                            {
                                list.Add(@class);
                            }
                        }
                    }
                    foreach (GameClient @class in list)
                    {
                        try
                        {
                           
                            @class.method_12();
                        }
                        catch
                        {
                        }
                    }
                    byte[] byte_ = Message.GetBytes();
                    foreach (GameClient @class in list2)
                    {
                        try
                        {
                            
                            @class.GetConnection().SendData(byte_);
                        }
                        catch
                        {
                            
                            @class.method_12();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogThreadException(ex.ToString(), "Connection checker task");
                }
                Thread.Sleep(num);
            }
        }

        internal void method_13()
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null)
                {
                    try
                    {
                        @class.SendMessage(AchievementManager.smethod_1(@class));
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void BroadcastMessage(ServerMessage message)
        {
            byte[] bytes = message.GetBytes();
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient client = this.Clients[i];
                if (client != null)
                {
                    try
                    {
                        client.GetConnection().SendData(bytes);
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void method_15(ServerMessage Message5_0, ServerMessage Message5_1)
        {
            byte[] byte_ = Message5_0.GetBytes();
            byte[] byte_2 = Message5_1.GetBytes();
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null)
                {
                    try
                    {
                        if (@class.GetHabbo().InRoom)
                        {
                            @class.GetConnection().SendData(byte_);
                        }
                        else
                        {
                            @class.GetConnection().SendData(byte_2);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void method_16(ServerMessage Message5_0, ServerMessage Message5_1)
        {
            byte[] byte_ = Message5_0.GetBytes();
            byte[] byte_2 = Message5_1.GetBytes();
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null)
                {
                    try
                    {
                        if (@class.GetHabbo().HasFuse("receive_sa"))
                        {
                            if (@class.GetHabbo().InRoom)
                            {
                                @class.GetConnection().SendData(byte_);
                            }
                            else
                            {
                                @class.GetConnection().SendData(byte_2);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void method_17(GameClient class16_1, ServerMessage Message5_0)
        {
            byte[] byte_ = Message5_0.GetBytes();
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class != class16_1)
                {
                    try
                    {
                        if (@class.GetHabbo().HasFuse("receive_sa"))
                        {
                            @class.GetConnection().SendData(byte_);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void method_18(int int_0)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    try
                    {
                        long NoBug = 0L;
                        NoBug += (long)@class.GetHabbo().Credits;
                        NoBug += (long)int_0;
                        if (NoBug <= 2147483647L || -2147483648L >= NoBug)
                        {
                            @class.GetHabbo().Credits += int_0;
                            @class.GetHabbo().UpdateCredits(true);
                            @class.SendNotification("Du hast " + int_0 + " Taler vom Hotelmanagement bekommen!");
                        }
                        else if (int_0 > 0)
                        {
                            @class.GetHabbo().Credits = 2147483647;
                            @class.GetHabbo().UpdateCredits(true);
                            @class.SendNotification("You just received max credits from staff!");
                        }
                        else if (int_0 < 0)
                        {
                            @class.GetHabbo().Credits = -2147483648;
                            @class.GetHabbo().UpdateCredits(true);
                            @class.SendNotification("You just received max negative credits from staff!");
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
        internal void method_18NEW(string username,string str)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                
                    try
                    {

                        @class.GetHabbo().Whisper("[ADMIN] - " + username + " sagt: " + str + "");
                       
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void method_18ONLINE(string username)
        {

            
            for (int index = 0; index < this.Clients.Length; ++index)
            {
                GameClient gameClient = this.Clients[index];
                if (gameClient != null && gameClient.GetHabbo() != null)
                {
                    try
                    {
                        //gameClient.GetHabbo().Whisper("Teammitglied " + username + " hat sich gerade eingeloggt!");
                    }
                    catch
                    {
                    }
                }
            }
        }


        internal void method_18GC(string username, string str)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null && @class.GetHabbo().gchat == 1)
                {
                    try
                    {

                        @class.GetHabbo().Whisper("[Globalchat] - " + username + " sagt: " + str + "");

                    }
                    catch
                    {
                    }
                }
            }
        }
        internal void method_WHISPER(string str)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    try
                    {

                        @class.GetHabbo().Whisper(str);

                    }
                    catch
                    {
                    }
                }
            }
        }
        internal void resetlotto( )
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    try
                    {

                        @class.GetHabbo().mylottozahl = 999;

                    }
                    catch
                    {
                    }
                }
            }
        }

        

        internal void method_18SA(string username, string str)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    if (@class.GetHabbo().Rank >= 6)
                    {
                        try
                        {

                            @class.GetHabbo().Whisper("[ADMIN-CHAT] - " + username + ": " + str + "");

                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        internal void method_StaffWhisper(string str)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    if (@class.GetHabbo().Rank >= 6)
                    {
                        try
                        {

                            @class.GetHabbo().Whisper(str);

                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
        internal void method_18EH(string username, string str)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    try
                    {

                        @class.GetHabbo().Whisper("Offizieller Eventstart! Benutze :follow " + username + ", um zum Event zu gelangen.");

                    }
                    catch
                    {
                    }
                }
            }
        }
        internal void method_18AFK(string username, string str)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    try
                    {

                        @class.GetHabbo().Whisper("Admin " + username + " meldet sich AFK.");

                    }
                    catch
                    {
                    }
                }
            }
        }
        internal void method_18WD(string username, string str)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    try
                    {

                        @class.GetHabbo().Whisper("Admin " + username + " ist nun wieder anwesend.");

                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void method_18AD(string username)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    try
                    {

                        @class.GetHabbo().Whisper("Admin " + username + " ist nun im Admin-Dienst!");

                    }
                    catch
                    {
                    }
                }
            }
        }

        /*internal void method_18BAN(string bannedName,string mod,string grund)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    if (@class.GetHabbo().Rank >= 6)
                    {
                        try
                        {

                            @class.GetHabbo().Whisper("BanManager:" + mod + " hat den Spieler " + bannedName + " wegen " + grund +" gebannt!");

                        }
                        catch
                        {
                        }
                    }
                }
            }
        }*/
        internal void method_19(int int_0, bool bool_0)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    try
                    {
                        long NoBug = 0L;
                        NoBug += (long)@class.GetHabbo().ActivityPoints;
                        NoBug += (long)int_0;
                        if (NoBug <= 2147483647L || -2147483648L >= NoBug)
                        {
                            @class.GetHabbo().ActivityPoints += int_0;
                            @class.GetHabbo().UpdateActivityPoints(bool_0);
                            @class.SendNotification("Quak Quak, die Enten sind los!\n\nJedem User wurden " + int_0 + " Enten gutgeschrieben.");
                        }
                        else if (int_0 > 0)
                        {
                            @class.GetHabbo().Credits = 2147483647;
                            @class.GetHabbo().UpdateCredits(true);
                            @class.SendNotification("Du hast Enten bekommen!");
                        }
                        else if (int_0 < 0)
                        {
                            @class.GetHabbo().Credits = -2147483648;
                            @class.GetHabbo().UpdateCredits(true);
                            @class.SendNotification("You just received max negative pixels from staff!");
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void method_20(int int_0, bool bool_0)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    try
                    {
                        long NoBug = 0L;
                        NoBug += (long)@class.GetHabbo().VipPoints;
                        NoBug += (long)int_0;
                        if (NoBug <= 2147483647L || -2147483648L >= NoBug)
                        {
                            @class.GetHabbo().VipPoints += int_0;
                            @class.GetHabbo().UpdateVipPoints(false, bool_0);
                            @class.SendNotification("Du hast eine Belohnung erhalten!\r\rDeinem Konto wurden " + int_0 + " Sterne gutgeschrieben.");
                        }
                        else if (int_0 > 0)
                        {
                            @class.GetHabbo().Credits = 2147483647;
                            @class.GetHabbo().UpdateCredits(true);
                            @class.SendNotification("You just received max stars from staff!");
                        }
                        else if (int_0 < 0)
                        {
                            @class.GetHabbo().Credits = -2147483648;
                            @class.GetHabbo().UpdateCredits(true);
                            @class.SendNotification("You just received max negative stars from staff!");
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void method_21(string string_0)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    try
                    {
                        @class.GetHabbo().GetBadgeComponent().SendBadge(@class, string_0, true);
                        @class.SendNotification("Du hast ein Badge vom Hotelmanagement bekommen!");
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void method_22(ServerMessage Message5_0, string string_0)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null)
                {
                    try
                    {
                        if (string_0.Length <= 0 || (@class.GetHabbo() != null && @class.GetHabbo().HasFuse(string_0)))
                        {
                            @class.SendMessage(Message5_0);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void method_23()
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null && @class.GetHabbo().GetEffectsInventoryComponent() != null)
                {
                    @class.GetHabbo().GetEffectsInventoryComponent().method_7();
                }
            }
        }

        internal void method_New1(string string_0)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    try
                    {
                        HabboIM.GetGame().GetAchievementManager().addAchievement(@class, Convert.ToUInt32(string_0));
                        @class.SendNotification("Du hast ein Bonusbadge vom Hotelmanagement bekommen!");
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void method_New2(string string_0)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null && @class.GetHabbo().CurrentRoom != HabboIM.GetGame().GetClientManager().GetClientByHabbo(string_0).GetHabbo().CurrentRoom)
                {
                    try
                    {
                        ServerMessage Message5 = new ServerMessage(286u);
                        Message5.AppendBoolean(HabboIM.GetGame().GetClientManager().GetClientByHabbo(string_0).GetHabbo().CurrentRoom.IsPublic);
                        Message5.AppendUInt(HabboIM.GetGame().GetClientManager().GetClientByHabbo(string_0).GetHabbo().CurrentRoomId);
                        @class.SendMessage(Message5);
                    }
                    catch
                    {
                    }
                }
            }
        }


        internal void method_New99()
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                uint num2 = Convert.ToUInt32(901);
                Room class3 = HabboIM.GetGame().GetRoomManager().method_15(num2);
                if (@class != null && @class.GetHabbo() != null && @class.GetHabbo().CurrentRoom != class3 && @class.GetHabbo().jail == 0)
                {
                    try
                    {


                       
                        ServerMessage Message5 = new ServerMessage(286u);
                        Message5.AppendBoolean(class3.IsPublic);
                        Message5.AppendUInt(901);
                        @class.SendMessage(Message5);

                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void method_New3(string string_0)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo().Rank > 5u && @class.GetHabbo() != null && @class.GetHabbo().CurrentRoom != HabboIM.GetGame().GetClientManager().GetClientByHabbo(string_0).GetHabbo().CurrentRoom)
                {
                    try
                    {
                        ServerMessage Message5 = new ServerMessage(286u);
                        Message5.AppendBoolean(HabboIM.GetGame().GetClientManager().GetClientByHabbo(string_0).GetHabbo().CurrentRoom.IsPublic);
                        Message5.AppendUInt(HabboIM.GetGame().GetClientManager().GetClientByHabbo(string_0).GetHabbo().CurrentRoomId);
                        @class.SendMessage(Message5);
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void method_New4(int string_0)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    try
                    {
                        ServerMessage Message = new ServerMessage(27u);
                        Message.AppendInt32(string_0);
                        @class.SendMessage(Message);
                        @class.GetHabbo().IsMuted = true; 
                        @class.GetHabbo().int_4 = string_0;
                        @class.SendNotification("Alle im Client wurden für " + string_0 + " Sekunden gemutet!");
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void CloseAll()
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;
            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
            {
                for (int i = 0; i < this.Clients.Length; i++)
                {
                    GameClient client = this.Clients[i];
                    if (client != null && client.GetHabbo() != null)
                    {
                        try
                        {
                            client.GetHabbo().GetInventoryComponent().SavePets(dbClient, true);
                            stringBuilder.Append(client.GetHabbo().UpdateQuery);
                            flag = true;
                        }
                        catch
                        {
                        }
                    }
                }
                if (flag)
                {
                    try
                    {
                        dbClient.ExecuteQuery(stringBuilder.ToString(), 30);
                    }
                    catch (Exception ex)
                    {
                        Logging.HandleException(ex.ToString());
                    }
                }
            }
            Console.WriteLine("Done saving users inventory!");
            Console.WriteLine("Closing server connections...");
            try
            {
                for (int i = 0; i < this.Clients.Length; i++)
                {
                    GameClient class2 = this.Clients[i];
                    if (class2 != null && class2.GetConnection() != null)
                    {
                        try
                        {
                            class2.GetConnection().Close();
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.HandleException(ex.ToString());
            }
            Array.Clear(this.Clients, 0, this.Clients.Length);
            Console.WriteLine("Connections closed!");
        }

        public void method_25(uint uint_0)
        {
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null && @class.GetHabbo().Id == uint_0)
                {
                    @class.method_12();
                }
            }
        }

        public string GetNameById(uint uint_0)
        {
            GameClient @class = this.method_2(uint_0);
            string result;
            if (@class != null)
            {
                result = @class.GetHabbo().Username;
            }
            else
            {
                DataRow dataRow = null;
                using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                {
                    dataRow = class2.ReadDataRow("SELECT username FROM users WHERE Id = '" + uint_0 + "' LIMIT 1", 30);
                }
                if (dataRow == null)
                {
                    result = "Unknown User";
                }
                else
                {
                    result = (string)dataRow[0];
                }
            }
            return result;
        }

        public string GetDataById(uint uint_0, string data)
        {
            string result = "";
            string result2;
            if (data != "gender" || data != "look")
            {
                result2 = result;
            }
            else
            {
                GameClient @class = this.method_2(uint_0);
                if (@class != null)
                {
                    if (data == "gender")
                    {
                        result = @class.GetHabbo().Gender;
                    }
                    else if (data == "look")
                    {
                        result = @class.GetHabbo().Figure;
                    }
                }
                else
                {
                    DataRow dataRow = null;
                    using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                    {
                        dataRow = class2.ReadDataRow(string.Concat(new object[]
						{
							"SELECT ",
							data,
							" FROM users WHERE Id = '",
							uint_0,
							"' LIMIT 1"
						}), 30);
                    }
                    if (dataRow == null)
                    {
                        result = "Unknown data";
                    }
                    else
                    {
                        result = (string)dataRow[0];
                    }
                }
                result2 = result;
            }
            return result2;
        }

        public uint method_27(string string_0)
        {
            GameClient @class = this.GetClientByHabbo(string_0);
            uint result;
            if (@class != null && @class.GetHabbo() != null)
            {
                result = @class.GetHabbo().Id;
            }
            else
            {
                DataRow dataRow = null;
                using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                {
                    dataRow = class2.ReadDataRow("SELECT Id FROM users WHERE username = '" + string_0 + "' LIMIT 1", 30);
                }
                if (dataRow == null)
                {
                    result = 0u;
                }
                else
                {
                    result = (uint)dataRow[0];
                }
            }
            return result;
        }

        public void method_28()
        {
            Dictionary<GameClient, ModerationBanException> dictionary = new Dictionary<GameClient, ModerationBanException>();
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null)
                {
                    try
                    {
                        HabboIM.GetGame().GetBanManager().method_1(@class);
                    }
                    catch (ModerationBanException value)
                    {
                        dictionary.Add(@class, value);
                    }
                }
            }
            foreach (KeyValuePair<GameClient, ModerationBanException> current in dictionary)
            {
                current.Key.NotifyBan(current.Value.Message);
                current.Key.method_12();
            }
        }

        public void method_29()
        {
            try
            {
                if (this.Clients != null)
                {
                    for (int i = 0; i < this.Clients.Length; i++)
                    {
                        GameClient @class = this.Clients[i];
                        if (@class != null && @class.GetHabbo() != null && HabboIM.GetGame().GetPixelManager().method_2(@class))
                        {
                            HabboIM.GetGame().GetPixelManager().method_3(@class);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogThreadException(ex.ToString(), "GCMExt.CheckPixelUpdates task");
            }
        }

        internal List<ServerMessage> method_30()
        {
            List<ServerMessage> list = new List<ServerMessage>();
            int num = 0;
            ServerMessage Message = new ServerMessage();
            Message.Init(161u);
            Message.AppendStringWithBreak("Users online:\r");
            for (int i = 0; i < this.Clients.Length; i++)
            {
                GameClient @class = this.Clients[i];
                if (@class != null && @class.GetHabbo() != null)
                {
                    if (num > 20)
                    {
                        list.Add(Message);
                        num = 0;
                        Message = new ServerMessage();
                        Message.Init(161u);
                    }
                    num++;
                    Message.AppendStringWithBreak(string.Concat(new object[]
					{
						@class.GetHabbo().Username,
						" {",
						@class.GetHabbo().Rank,
						"}\r"
					}));
                }
            }
            list.Add(Message);
            return list;
        }

        internal void method_31(GameClient class16_1, string string_0, string string_1)
        {
            if (ServerConfiguration.EnableCommandLog)
            {
                using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
                {
                    @class.AddParamWithValue("extra_data", string_1);
                    @class.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO cmdlogs (user_id,user_name,command,extra_data,timestamp) VALUES ('",
						class16_1.GetHabbo().Id,
						"','",
						class16_1.GetHabbo().Username,
						"','",
						string_0,
						"', @extra_data, UNIX_TIMESTAMP())"
					}), 30);
                }
            }
        }
    }
}
