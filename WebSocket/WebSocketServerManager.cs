using System;
using System.Collections.Generic;
using Fleck;
using System.Data;
using HabboIM.Storage;
using HabboIM.HabboHotel.Items;
using HabboIM.HabboHotel.Rooms;
using HabboIM.HabboHotel.Catalogs;
using HabboIM.HabboHotel.Users;
using HabboIM.HabboHotel.Users.Authenticator;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using System.Text;

namespace HabboIM.WebSocket
{
    class WebSocketServerManager
    {
        private List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();
        public static Dictionary<string, IWebSocketConnection> socketbyName = new Dictionary<string, IWebSocketConnection>();
        public static Dictionary<IWebSocketConnection, string> namebySocket = new Dictionary<IWebSocketConnection, string>();
        private WebSocketServer server;
        public WebSocketServerManager()
        {
            this.allSockets = new List<IWebSocketConnection>();
            socketbyName = new Dictionary<string, IWebSocketConnection>();
            namebySocket = new Dictionary<IWebSocketConnection, string>();
        }
        public List<IWebSocketConnection> getAllSockets()
        {
            return this.allSockets;
        }
        public IWebSocketConnection getWebSocketByName(string name)
        {
            if (socketbyName.ContainsKey(name))
                return socketbyName[name];

            return null;
        }
        public string GetNameByWebSocket(IWebSocketConnection socket)
        {
            if (namebySocket.ContainsKey(socket))
                return namebySocket[socket];
            return "";
        }
        public void SendMessageToEveryConnection(string Message)
        {
            foreach (IWebSocketConnection iwsc in allSockets)
            {
                try
                {
                    iwsc.Send(Message);
                }
                catch { }
            }
        }
        public void LogWebsocketException(LogLevel ll, string s, Exception ex)
        {
            switch (ll)
            {
                case LogLevel.Error:
                    {
                        //Logging.LogException(s + ex.ToString());
                        break;
                    }
            }
        }



        public int getWSlistener()
        {
            int anzahl = 0;
            foreach (IWebSocketConnection iwsc in allSockets)
            {
                anzahl = anzahl + 1;
            }

            return anzahl;
        }

        public void Dispose()
        {
            foreach (IWebSocketConnection iwsc in allSockets)
            {
                try { iwsc.Close(); }
                catch { }
            }
            server.Dispose();
        }
        public WebSocketServerManager(string SocketURL)
        {
            FleckLog.Level = LogLevel.Error;
            allSockets = new List<IWebSocketConnection>();
            socketbyName = new Dictionary<string, IWebSocketConnection>();
            namebySocket = new Dictionary<IWebSocketConnection, string>();
            server = new WebSocketServer(SocketURL);
            /*if (SocketURL.StartsWith("wss://"))
                server.Certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2("client.habbo.tl.cert.cer"); */
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    if (allSockets.Contains(socket))
                        allSockets.Remove(socket);
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    string name = "";
                    if (namebySocket.ContainsKey(socket))
                    {
                        name = namebySocket[socket].ToString();
                        namebySocket.Remove(socket);
                    }
                    if (socketbyName.ContainsKey(name) && name != "")
                        socketbyName.Remove(name);
                    if (allSockets.Contains(socket))
                        allSockets.Remove(socket);
                    if (name != "")
                    {
                        using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                        {
                            dbClient.AddParamWithValue("name", name);
                            dbClient.ExecuteQuery("UPDATE users SET websocket='0' WHERE username=@name");
                        }
                    }
                };
                socket.OnMessage = message =>
                {
                    var msg = message;
                    int pId = 0;
                    if (!int.TryParse(msg.Split('|')[0], out pId))
                        return;
                    if (msg.Length > 1024)
                        return;
                    if (msg.StartsWith("1|"))
                    {
                        using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                        {
                            dbClient.AddParamWithValue("auth", msg.Substring(2));
                            DataRow drow = null;
                            drow = dbClient.ReadDataRow("SELECT username FROM users WHERE auth_ticket= @auth LIMIT 1");
                            if (drow == null)
                            {
                                socket.Close();

                            }
                            else
                            {
                                if (socketbyName.ContainsKey((string)drow["username"]))
                                {/* socketbyName[(string)drow["username"]].Close();*/ socketbyName.Remove((string)drow["username"]); }
                                socketbyName.Add(drow["username"].ToString(), socket);
                                if (namebySocket.ContainsKey(socket))
                                    namebySocket.Remove(socket);
                                namebySocket.Add(socket, drow["username"].ToString());
                                dbClient.AddParamWithValue("name", drow["username"].ToString());

                            }
                        }
                    }
                    else
                    {
                        GameClient Session = HabboIM.GetGame().GetClientManager().GetClientByHabbo(GetNameByWebSocket(socket));
                        Room room = Session.GetHabbo().CurrentRoom;
                        string[] args = msg.Split('|');
                        switch (int.Parse(args[0]))
                        {


                            case 88:
                                {

                                    GameClient class4 = HabboIM.GetGame().GetClientManager().method_2(Convert.ToUInt32(args[1]));



                                    if(class4 == null)
                                    {

                                        Session.GetHabbo().Whisper("Diese Person ist bereits offline. Du kannst die Anfrage deshalb nicht mehr annehmen.");
                                        break;
                                    }

                                    if(class4.GetHabbo().sexanfrage != Session.GetHabbo().Id)
                                    {

                                        Session.GetHabbo().Whisper("Ooops, da lief wohl etwas schief.");
                                        break;
                                    }
                                    if(class4.GetHabbo().boyfriend != 0 || Session.GetHabbo().boyfriend != 0)
                                    {
                                        Session.GetHabbo().Whisper("Du bist noch mit wem zusammen.");
                                        break;
                                    }
                                    if (args[2] == "1")
                                    {
                                        class4.GetHabbo().lovepoints = 0;
                                        class4.GetHabbo().kissed = 0;
                                        class4.GetHabbo().lovedate = 0;
                                        class4.GetHabbo().hugged = 0;
                                        class4.GetHabbo().hugtime = 0;
                                        class4.GetHabbo().kisstime = 0;
                                        class4.GetHabbo().boyfriend = 0;
                                        Session.GetHabbo().lovepoints = 0;
                                        Session.GetHabbo().kissed = 0;
                                        Session.GetHabbo().lovedate = 0;
                                        Session.GetHabbo().hugged = 0;
                                        Session.GetHabbo().hugtime = 0;
                                        Session.GetHabbo().kisstime = 0;


                                        Session.GetHabbo().lovedate = HabboIM.GetUnixTimestamp();
                                        class4.GetHabbo().lovedate = HabboIM.GetUnixTimestamp();
                                        class4.GetHabbo().boyfriend = Convert.ToInt32(Session.GetHabbo().Id);
                                        Session.GetHabbo().boyfriend = Convert.ToInt32(class4.GetHabbo().Id);

                                        try
                                        {

                                            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                            {
                                                dbClient.ExecuteQuery(string.Concat(new object[]
                                                {
                        "UPDATE users SET boyfriend = '",
                        Session.GetHabbo().Id,
                        "', love_date = '",
                        class4.GetHabbo().lovedate,
                        "' WHERE Id = '",
                        class4.GetHabbo().Id,
                        "'  LIMIT 1;"
                                                }));

                                                dbClient.ExecuteQuery(string.Concat(new object[]
                                                {
                        "UPDATE users SET boyfriend = '",
                        class4.GetHabbo().Id,
                        "', love_date = '",
                        Session.GetHabbo().lovedate,
                        "' WHERE Id = '",
                        Session.GetHabbo().Id,
                        "'  LIMIT 1;"
                                                }));
                                            }


                                            HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("textosteron|Beziehung|Du bist jetzt in einer Beziehung mit "+ class4.GetHabbo().Username+".");
                                            HabboIM.GetWebSocketManager().getWebSocketByName(class4.GetHabbo().Username).Send("textosteron|Beziehung|Du bist jetzt in einer Beziehung mit " + Session.GetHabbo().Username + ".");

                                        }
                                        catch
                                        {


                                        }

                                    }

                                    if(args[2] == "2") {



                                     
                                        HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("textosteron|Beziehungsanfrage|Die Beziehungsanfrage wurde abgelehnt.");
                                        HabboIM.GetWebSocketManager().getWebSocketByName(class4.GetHabbo().Username).Send("textosteron|Beziehungsanfrage|Die Beziehungsanfrage wurde abgelehnt.");





                                    }
                                    class4.GetHabbo().sexanfrage = 0;
                                    Session.GetHabbo().sexanfrage = 0;

                                    break;
                                }


                            case 74:
                                {

                                    try {
                                        using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                        {
                                            dbClient.ExecuteQuery(string.Concat(new object[]
                                            {
                        "UPDATE users SET lastkiss_time = '0', lasthug_time = '0', lovepoints = '0', kisses_bf = '0', hugs_bf = '0', boyfriend = '0' WHERE Id = '",
                        Session.GetHabbo().Id,
                        "' OR Id = '",
                Session.GetHabbo().boyfriend,
                "' LIMIT 2;"
                                            }));
                                        }


                                        Session.GetHabbo().lovepoints = 0;
                                        Session.GetHabbo().kissed = 0;
                                        Session.GetHabbo().lovedate = 0;
                                        Session.GetHabbo().hugged = 0;
                                        Session.GetHabbo().hugtime = 0;
                                        Session.GetHabbo().kisstime = 0;

                                        GameClient class4 = HabboIM.GetGame().GetClientManager().method_2(Convert.ToUInt32(Session.GetHabbo().boyfriend));

                                        if (class4 != null)
                                        {
                                            class4.GetHabbo().lovepoints = 0;
                                            class4.GetHabbo().kissed = 0;
                                            class4.GetHabbo().lovedate = 0;
                                            class4.GetHabbo().hugged = 0;
                                            class4.GetHabbo().hugtime = 0;
                                            class4.GetHabbo().kisstime = 0;
                                            class4.GetHabbo().boyfriend = 0;
                                            HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("textosteron|Trennung|Du hast mit "+class4.GetHabbo().Username +" Schluss gemacht.");
                                            HabboIM.GetWebSocketManager().getWebSocketByName(class4.GetHabbo().Username).Send("textosteron|Trennung|" + Session.GetHabbo().Username + " hat mit dir Schluss gemacht.");


                                        }
                                        Session.GetHabbo().boyfriend = 0;
                                        if (class4 == null)
                                        {
                                            HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("textosteron|Trennung|Du hast Schluss gemacht.");

                                      //      Session.SendNotification("Du hast erfolgreich Schluss gemacht!");
                                        }
                                    } catch(Exception x)
                                    {
                                  //      Session.SendNotification(x.ToString());


                                    }




                                    break;
                                }

                              



                                


                            case 6:
                                {
                                    try
                                    {
                                        room = HabboIM.GetGame().GetRoomManager().GetRoom(uint.Parse(args[1]));
                                        if (Session != null && room != null)
                                        {
                                            ServerMessage Message = new ServerMessage(286u);
                                            Message.AppendBoolean(room.IsPublic);
                                            Message.AppendUInt(room.Id);
                                            Session.SendMessage(Message);
                                        }

                                    }
                                    catch { }
                                    break;
                                }
                            case 9:
                                {

                                    try
                                    {
                                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, false))
                                        {
                                            int ItemId = int.Parse(args[1]);
                                            double newZ = double.Parse(args[2]);
                                            RoomItem ri = Session.GetHabbo().CurrentRoom.method_28((uint)ItemId);
                                            if (ri != null && ri.GetBaseItem().InteractionType == "stackfield")
                                            {
                                                ri.setHeight(newZ);
                                                ri.method_8().method_80(ri);
                                                ServerMessage Message = new ServerMessage(94u);
                                                Message.AppendRawUInt(ri.uint_0);
                                                Message.AppendStringWithBreak("");
                                                Message.AppendBoolean(false);
                                                Session.GetHabbo().CurrentRoom.SendMessage(Message, null);
                                                ServerMessage Message5_ = new ServerMessage(95u);
                                                ri.SerializeStackField(Message5_);
                                                Session.GetHabbo().CurrentRoom.SendMessage(Message5_, null);
                                                ServerMessage Message1_ = new ServerMessage(93u);
                                                ri.SerializeStackField(Message1_);
                                                Session.GetHabbo().CurrentRoom.SendMessage(Message1_, null);
                                            }
                                        }
                                    }
                                    catch { }
                                    break;
                                }
                            case 10:
                                {
                                    try
                                    {
                                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, false))
                                        {
                                            uint itemid = uint.Parse(args[1]);
                                            int handitemId = int.Parse(args[2]);
                                            RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(itemid);
                                            if (ri != null && ri.GetBaseItem().InteractionType == "wf_cnd_has_handitem")
                                            {
                                                ri.string_2 = handitemId.ToString();
                                                ri.UpdateState(true, false);
                                            }
                                        }
                                    }
                                    catch { }
                                    break;
                                }
                            case 12:
                                {
                                    try
                                    {
                                        if (Session.GetHabbo().CurrentRoom.CheckRights(Session, false))
                                        {
                                            uint itemid = uint.Parse(args[1]);
                                            string team = args[2];
                                            RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(itemid);
                                            if (ri != null && (ri.GetBaseItem().InteractionType == "wf_cnd_actor_in_team" || ri.GetBaseItem().InteractionType == "wf_cnd_not_in_team") && Session.GetHabbo().CurrentRoom.IsValidTeam(team))
                                            {
                                                ri.string_2 = team;
                                                ri.UpdateState(true, false);
                                            }
                                        }
                                    }
                                    catch { }
                                    break;
                                }
                            case 14:
                                {
                                    try
                                    {
                                        Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username).CarryItem(int.Parse(args[1]));
                                    }
                                    catch { }
                                    break;
                                }
                            case 32:
                                {
                                    if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                                    {
                                        try
                                        {
                                            RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(uint.Parse(args[1]));
                                            if (ri != null && ri.GetBaseItem().InteractionType == "wf_act_yt")
                                            {
                                                string ytlink = args[2].Split('=')[1];
                                                ri.string_2 = ytlink;
                                                ri.UpdateState(true, false);
                                            }
                                        }
                                        catch { }
                                    }
                                    break;
                                }
                            case 35:
                                {
                                    if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                                    {
                                        try
                                        {
                                            RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(uint.Parse(args[1]));
                                            if (ri != null && (ri.GetBaseItem().InteractionType == "wf_cnd_has_purse" || ri.GetBaseItem().InteractionType == "wf_cnd_hasnot_purse"))
                                            {
                                                string currency = Session.GetHabbo().CurrentRoom.IsValidCurrency(args[2]) ? args[2] : "credits";
                                                int number = 1337;
                                                int.TryParse(args[3], out number);
                                                ri.string_2 = currency + ";" + number;
                                                ri.UpdateState(true, false);
                                            }
                                        }
                                        catch { }
                                    }
                                    break;
                                }
                            case 36:
                                {
                                    if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                                    {
                                        try
                                        {
                                            RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(uint.Parse(args[1]));
                                            if (ri != null && ri.GetBaseItem().InteractionType == "wf_act_img" && IsValidFile(args[2]))
                                            {
                                                ri.string_2 = args[2];
                                                ri.UpdateState(true, false);
                                            }
                                        }
                                        catch { }
                                    }
                                    break;
                                }
                            case 93076201:
                                {
                                    if (HabboIM.zufallsrare == false)
                                    {
                                        HabboIM.zufallsrare = true;
                                        uint item = uint.Parse(args[1]);
                                        uint user = uint.Parse(args[2]);
                                        uint id = uint.Parse(args[3]);
                                        uint geschenk = 169;
                                        var geschenkmsg = "!HabboIM Zufallsrare - Was hier wohl drin sein mag?";
                                        uint amount = 1;

                                        using (DatabaseClient dbclient = HabboIM.GetDatabase().GetClient())
                                        {
                                            dbclient.AddParamWithValue("spieler", uint.Parse(args[2]));
                                            dbclient.AddParamWithValue("raum", '0');
                                            dbclient.AddParamWithValue("moebel", uint.Parse(args[1]));
                                            dbclient.AddParamWithValue("geschenk", geschenk);
                                            dbclient.AddParamWithValue("geschenkmsg", geschenkmsg);
                                            dbclient.AddParamWithValue("amount", amount);
                                            StringBuilder StringGetiditem = new StringBuilder();
                                            DataTable Getiditem = dbclient.ReadDataTable("SELECT * FROM items ORDER BY RAND() LIMIT 1");
                                            foreach (DataRow StaffRow in Getiditem.Rows) {
                                                uint num5 = HabboIM.GetGame().GetCatalog().method_14();
                                                dbclient.AddParamWithValue("id", num5);
                                                Console.WriteLine(StaffRow["id"]);
                                                dbclient.ExecuteQuery(string.Concat(new object[]
                        {
                            "INSERT INTO items (Id,user_id,base_item,wall_pos) VALUES ('",
                            (object)num5,
                            "','",
                            uint.Parse(args[2]),
                            "','169','')"
                        }));
                                                dbclient.ExecuteQuery(string.Concat(new object[]
                        {
                            "INSERT INTO items_extra_data (item_id,extra_data) VALUES ('",
                            (object)num5,
                            "','",
                            "!HabboIM Zufallsrare - Was hier wohl drin sein mag?",
                            "')"
                        }));
                                                dbclient.ExecuteQuery(string.Concat(new object[]
                        {"INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES ('",
                            (object)num5,
                            "','",
                            uint.Parse(args[1]),
                            "','1','')"
                        }));
                                                /*
                                                dbclient.ExecuteQuery("INSERT INTO items (id, user_id, room_id, base_item) VALUES ('" + (object)num5 + "', @spieler, @raum, @geschenk)");
                                                dbclient.ExecuteQuery("INSERT INTO items_extra_data (item_id, extra_data) VALUES ('" + (object)num5 + "', @geschenkmsg)");
                                                dbclient.ExecuteQuery("INSERT INTO user_presents (item_id, base_id, amount) VALUES ('" + (object)num5 + "', @moebel, @amount)");*/
                                            }
                                            
                                        }
                                        //Staffchat Ausgabe
                                        ServerMessage Logging = new ServerMessage(134u);
                                        Logging.AppendUInt(0u);
                                        Logging.AppendString("INFO: Der Spieler " + args[4] + " hat eine Zufallsrarität erhalten!");
                                        HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                        
                                        if(int.Parse(HabboIM.GetConfig().data["habboim.customdesign"]) == 1)
                                        {
                                            string rmessage = "rndrmsg";
                                            HabboIM.GetWebSocketManager().SendMessageToEveryConnection(rmessage);
                                            HabboIM.GetGame().GetClientManager().method_WHISPER("Nächste Zufallsrare wird in " + HabboIM.nxt_rr + " Minuten verteilt!");
                                        } else
                                        {
                                            HabboIM.GetGame().GetClientManager().method_WHISPER("Die Zufallsrare wurde soeben verteilt!");
                                            HabboIM.GetGame().GetClientManager().method_WHISPER("Nächste Zufallsrare wird in " + HabboIM.nxt_rr + " Minuten verteilt!");
                                        }

                                        GameClient gameClient = HabboIM.GetGame().GetClientManager().method_2(uint.Parse(args[2]));
                                        if (gameClient != null)
                                        {
                                            gameClient.SendNotification("Überprüfe dein Inventar, denn du hast eine Zufallsrarität erhalten!");
                                            gameClient.GetHabbo().GetInventoryComponent().method_9(true);

                                            ++gameClient.GetHabbo().RandomRares;
                                            gameClient.GetHabbo().CheckRandomRares();
                                        }
                                        return;
                                    }
                                    break;
                                }
                            case 51:
                                {
                                    if (Session.GetHabbo().CurrentRoom.CheckRights(Session, true))
                                    {
                                        try
                                        {
                                            RoomItem ri = Session.GetHabbo().CurrentRoom.method_28(uint.Parse(args[1]));
                                            if (ri != null && (ri.GetBaseItem().InteractionType == "wf_cnd_user_count_in" || ri.GetBaseItem().InteractionType == "wfc_cnd_not_user_count") && IsValidFile(args[2]))
                                            {
                                                int min = 1;
                                                int max = 50;
                                                int.TryParse(args[2], out min);
                                                int.TryParse(args[3], out max);
                                                ri.string_3 = min + ";" + max;
                                                ri.UpdateState(true, false);
                                            }
                                        }
                                        catch { }
                                    }
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                };
            });
        }
        private uint uint_0 = 0;
        private readonly object object_0 = new object();
        internal uint method_14()
        {
            string abcde = "dev";
            //HabboIM.GetWebSocketManager().SendMessageToEveryConnection(abcde);
            lock (this.object_0)
                return this.uint_0++;
        }
        public bool IsValidFile(string url)
        {
            return url.StartsWith("http://") && (url.EndsWith(".png") || url.EndsWith(".jpg") || url.EndsWith(".gif"));
        }
    }
}