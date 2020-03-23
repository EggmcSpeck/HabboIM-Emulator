using System;
using System.Diagnostics;
using System.Threading;
using System.Data;
using HabboIM.Core;
using HabboIM.Storage;
using System.Globalization;
using HabboIM.Messages;
using HabboIM.HabboHotel.GameClients;

using System.Text;


namespace HabboIM.HabboHotel.Misc
{
    public sealed class LowPriorityWorker
    {
        public static short taktmod = 3000;
        public static void Initialise()
        {
            double lastDatabaseUpdate = HabboIM.GetUnixTimestamp();
            double lastDatabaseUpdate2 = HabboIM.GetUnixTimestamp();

            while (true)
            {
                try
                {
                    DateTime now = DateTime.Now;
                    TimeSpan timeSpan = now - HabboIM.ServerStarted;
                    new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    int Status = 1;

                    int UsersOnline = HabboIM.GetGame().GetClientManager().ClientCount;
                    int RoomsLoaded = HabboIM.GetGame().GetRoomManager().LoadedRoomsCount;

                    try
                    {
                        if (HabboIM.GetConfig().data["shutdown-server"] != null)
                        {
                            DateTime shutdown_server_time = Convert.ToDateTime(HabboIM.GetConfig().data["shutdown-server"]);
                            var time = shutdown_server_time.TimeOfDay.TotalSeconds;
                            string s = DateTime.Now.ToString("HH:mm:ss");
                            DateTime dt2 = DateTime.ParseExact(s, "HH:mm:ss", CultureInfo.InvariantCulture);
                            var time2 = dt2.TimeOfDay.TotalSeconds;
                            try
                            {
                                if (HabboIM.GetConfig().data["shutdown-warning-alert"] != null)
                                {
                                    if (time - time2 <= 60 && time - time2 >= 50)
                                    {
                                        try
                                        {
                                            if (int.Parse(HabboIM.GetConfig().data["shutdown-server-player-limit"]) < UsersOnline || int.Parse(HabboIM.GetConfig().data["shutdown-server-player-limit"]) <= 0)
                                            {
                                                string str = HabboIM.GetConfig().data["shutdown-warning-alert"];
                                                ServerMessage Message2 = new ServerMessage(808u);
                                                Message2.AppendStringWithBreak(HabboIMEnvironment.GetExternalText("cmd_ha_title"));
                                                Message2.AppendStringWithBreak(str + "\r\n- " + "Hotel");
                                                ServerMessage Message3 = new ServerMessage(161u);
                                                Message3.AppendStringWithBreak(str + "\r\n- " + "Hotel");
                                                HabboIM.GetGame().GetClientManager().method_15(Message2, Message3);
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }
                            if (time - time2 <= 11 && time - time2 >= 0)
                            {
                                try
                                {
                                    if (int.Parse(HabboIM.GetConfig().data["shutdown-server-player-limit"]) < UsersOnline || int.Parse(HabboIM.GetConfig().data["shutdown-server-player-limit"]) <= 0)
                                    {
                                        HabboIM.Destroy("SERVER SHUTDOWN! YOU HAVE SETUP TO CONFIG.CONF FILE SHUTDOWN TIME!", true);
                                    }
                                }
                                catch
                                {
                                    HabboIM.Destroy("SERVER SHUTDOWN! YOU HAVE SETUP TO CONFIG.CONF FILE SHUTDOWN TIME!", true);
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    double timestamp = HabboIM.GetUnixTimestamp() - lastDatabaseUpdate;

                    if (timestamp >= 5)
                    {
                        using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                        {
                            dbClient.ExecuteQuery(string.Concat(new object[]
						    {
							    "UPDATE server_status SET stamp = UNIX_TIMESTAMP(), status = '", Status, "', users_online = '",	UsersOnline, "', rooms_loaded = '",	RoomsLoaded, "', server_ver = '", HabboIM.Version,	"' LIMIT 1" 	}));
                                uint num3 = (uint)dbClient.ReadInt32("SELECT users FROM system_stats ORDER BY ID DESC LIMIT 1");
                                if ((long)UsersOnline > (long)((ulong)num3))
                                {
                                    dbClient.ExecuteQuery(string.Concat(new object[]
							    {
								    "UPDATE system_stats SET users = '",
								    UsersOnline,
								    "', rooms = '",
								    RoomsLoaded,
								    "' ORDER BY ID DESC LIMIT 1"
							    }));
                            }
                        }

                        lastDatabaseUpdate = HabboIM.GetUnixTimestamp();
                    }
                    double timestamp2 = HabboIM.GetUnixTimestamp() - lastDatabaseUpdate2;
                    if (timestamp2 >= 30.0)
                    {
                        using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                        {
                            dbClient.ExecuteQuery(string.Concat(new object[]
							{
								"INSERT INTO online_statistik (`useronline`,`rooms_loaded`,`timestamp`) VALUES ('",
								UsersOnline,
								"','",
								RoomsLoaded,
								"', '",
								HabboIM.GetUnixTimestamp(),
								"');"
							}), 30);
                        }
                        lastDatabaseUpdate2 = HabboIM.GetUnixTimestamp();
                    }
                    HabboIM.GetGame().GetClientManager().method_23();

                    Console.Title = string.Concat(new object[]
					{
						"HabboIM Emulator | Spieler: ",
						UsersOnline,
						" | Räume: ",
						RoomsLoaded,
						" | Online: ",
						timeSpan.Days,
						" Tage, ",
						timeSpan.Hours,
						" Stunden und ",
						timeSpan.Minutes,
						" Minuten"
					});

                    if(HabboIM.hour_lastlotto != now.Hour)
                    {
                        HabboIM.hour_lastlotto = now.Hour;

                        //ServerMessage Message2 = new ServerMessage(808u);
                        //Message2.AppendStringWithBreak("MyHuBBa Lotterie");
                        //Message2.AppendStringWithBreak("Aufgepasst, nun könnt ihr an unserer Lotterie teilnehmen. Verwende hierzu den Befehl :lotto ZAHL und schon nimmst du teil. Die Zahl darf zwischen 1 & 50 liegen. Kosten für Teilnahme beträgt 3.500 Taler!\r\n- System");
                        ServerMessage Message3 = new ServerMessage(161u);
                        //Message3.AppendStringWithBreak("Aufgepasst, nun könnt ihr an unserer Lotterie teilnehmen. Verwende hierzu den Befehl :lotto ZAHL und schon nimmst du teil. Die Zahl darf zwischen 1 & 50 liegen. Kosten für Teilnahme beträgt 3.500 Taler!\r\n- System");
                        //HabboIM.GetGame().GetClientManager().method_15(Message2, Message3);
                       
                        HabboIM.GetGame().GetClientManager().method_WHISPER("Lotto: Sende jetzt eine Zahl von 0-50 mit :lotto Zahl ein. Kosten: 3.500 Taler.");
                        HabboIM.lotto = true;
                     

                        Random rand = new Random();
                        int lottozahl = rand.Next(1, 50);  // Lottozahlen definieren
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("Lotto: Ziehung gestartet! Die Zahl lautet " + lottozahl);

                     HabboIM.lottozahl = lottozahl;
                        HabboIM.lottowinner = 0;
                        HabboIM.lotto_end = HabboIM.GetUnixTimestamp() + 5 * 60;
                        //Info für den dummen Hazed:
                        // 5 * 60 steht für 5 Minuten, da 5 Minuten 5 mal 60 Sekunden sind.
                        HabboIM.lotto_einsatz = 3500;



                    }
                    if(HabboIM.GetUnixTimestamp() >= HabboIM.lotto_end &&  HabboIM.lotto == true)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("Lotto: Ziehung gestoppt!");
                        HabboIM.lotto = false;
                        HabboIM.GetGame().GetClientManager().resetlotto();

                        GameClients.GameClient client;
                        try
                        {
                            client = HabboIM.GetGame().GetClientManager().method_2(HabboIM.lottowinner);


                            client.GetHabbo().Credits += HabboIM.lotto_einsatz;
                            client.GetHabbo().UpdateCredits(true);
                       
                            HabboIM.GetGame().GetClientManager().method_WHISPER(""+client.GetHabbo().Username+" hat den Jackpot in Höhe von "+HabboIM.lotto_einsatz.ToString()+ " Talern geknackt! (Zahl: " + HabboIM.lottozahl + ")");
                            HabboIM.lotto_einsatz = 3500;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Lotto: "+client.GetHabbo().Username+ " hat den Jackpot geknackt.");
                        }
                        catch
                        {
                            HabboIM.GetGame().GetClientManager().method_WHISPER("Lotto: Heute hat leider keiner den Jackpot geknackt!");
                            HabboIM.lotto_einsatz = 3500;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Lotto: Keiner konnte den Jackpot knacken.");
                        }
                    }





                    var minimum_users = int.Parse(HabboIM.GetConfig().data["habboim.zufallsrare_minimum_spieler"]);

                    if (HabboIM.hour_lastrr + HabboIM.nxt_rr * 60 < HabboIM.GetUnixTimestamp())
                    {
                        
                        HabboIM.hour_lastrr = HabboIM.GetUnixTimestamp();
                        if (int.Parse(HabboIM.GetConfig().data["habboim.zufallsrare"]) == 1)
                        {
                            //Wenn genug Habbos Online
                            if (UsersOnline >= minimum_users)
                            {
                                //Ausgabe in Konsole
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("____________________________________________________________________________________________________");
                                Console.WriteLine("                                                                                                    ");
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.Write("[");
                                string time1 = DateTime.Now.ToString();
                                Console.Write(time1 + "] ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("Zufallsrare: Verteilt!");
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.Write("[");
                                string time2 = DateTime.Now.ToString();
                                Console.Write(time2 + "] ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("Nächste Zufallsrare wird in " + HabboIM.nxt_rr + " Minuten verteilt!");
                                Console.WriteLine();
                            } else
                            {
                                if (UsersOnline < minimum_users)
                                {
                                    string time3 = DateTime.Now.ToString();
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("____________________________________________________________________________________________________");
                                    Console.WriteLine("                                                                                                    ");
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.Write("[");
                                    Console.Write(time3 + "] ");
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write("Zufallsrare nicht verteilt. Grund: Nicht genug Online! Minimum: " + minimum_users + " Habbos.");
                                    Console.WriteLine();
                                    HabboIM.zufallsrare = false;
                                }
                            }

                            if (UsersOnline >= minimum_users)
                            {
                                
                                //Ermittle über Websocket den Gewinner des Zufallsrare
                                HabboIM.zufallsrare = false;
                                string random_rare = "9307620";
                                HabboIM.GetWebSocketManager().SendMessageToEveryConnection(random_rare);
                            }
                        }

                        //string random_rare = "5|ZUFALLSRARE|Baumstamm|15180";
                        //HabboIM.GetWebSocketManager().SendMessageToEveryConnection(random_rare);
                        
                       /* using (DatabaseClient stafflist = HabboIM.GetDatabase().GetClient())
                        {
                            //DataTable start = stafflist.ReadDataTable("SELECT id FROM users ORDER BY desc LIMIT 1")
                            //DataTable Staffs = stafflist.ReadDataTable("SELECT * FROM users WHERE id = '" + testst + "' LIMIT 1");
                            StringBuilder StringStaff = new StringBuilder();
                            DataTable Staffs = stafflist.ReadDataTable("SELECT * FROM users ORDER BY id desc");
                            foreach(DataRow testRow in Staffs.Rows)
                            {
                                int dreiund = rand.Next(Staffs.Rows.Count, Staffs.Rows.Count);
                                Console.WriteLine("ZUFALL (MIT FOREACH) " + dreiund);
                            }
                            int testst = rand.Next(Staffs.Rows.Count, Staffs.Rows.Count);
                            Console.WriteLine("STAFF ROW " + Staffs.Rows.Count);
                            
                                    Console.WriteLine("ZUFALL (OHNE FOREACH) " + testst);
                            

                            //Console.WriteLine("User gefunden ID:" + testst);
                             foreach (DataRow baumstammRow in Staffs.Rows)
                             {
                                 //Console.WriteLine("User gefunden ID:" + testst);
                                 // Console.WriteLine((string)baumstammRow["username"]);
                                 Console.WriteLine(testst);
                             }
                            //Console.WriteLine((string)baumstammRow["username"]);

                        }*/

                    }
            
                }
                catch (Exception ex)
                {
                    Program.DeleteMenu(Program.GetSystemMenu(Program.GetConsoleWindow(), true), Program.SC_CLOSE, Program.MF_BYCOMMAND);
                    Logging.LogThreadException(ex.ToString(), "Server status update task");
                }
                Thread.Sleep(5000);
            }
        }
    }
}
