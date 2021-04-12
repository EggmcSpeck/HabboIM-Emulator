using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using HabboIM.Core;
using HabboIM.HabboHotel;
using HabboIM.Net;
using HabboIM.Storage;
using HabboIM.Communication;
using HabboIM.Messages;
using System.Threading.Tasks;
using System.Net;
using System.Reflection;
using System.Data;
using HabboIM.WebSocket;

namespace HabboIM
{
    internal sealed class HabboIM
    {
        public static readonly int build = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build;
        public const string string_0 = "localhost";

        private static PacketManager PacketManager;

        private static ConfigurationData Configuration;

        private static DatabaseManager DatabaseManager;
        private static SocketsManager SocketsManager;
        //private static ConnectionHandeling ConnectionManage;
        private static MusListener MusListener;

        private static Game Internal_Game;

        internal static DateTime ServerStarted;

        public string string_2 = HabboIM.smethod_1(14986.ToString());

        public static bool bool_0 = false;
        public static int int_1 = 0;
        public static int int_2 = 0;
        public static string string_5 = null;

        private static bool bool_1 = false;
        public static WebSocketServerManager webSocketManager;

        public static List<string> UserAdMessage;
        public static int UserAdType;
        public static string UserAdLink;

        // Lotterie

        public static int hour_lastlotto = DateTime.Now.Hour;
        public static double lotto_end = HabboIM.GetUnixTimestamp();
        public static bool lotto = false;
        public static int lottozahl = 999;
        public static uint lottowinner = 0;

        // Zufallsrare

        public static double hour_lastrr = 0.0;
        public static bool rr = false;
        public static uint rrwinner = 0;
        public static bool zufallsrare = false;
        public static uint nxt_rr = 30;

        //LICENCE
        public static string licence = "";
        public static string discord = "https://discord.gg/Snt8yZy";


        public static int lotto_einsatz;

        public static int Build
        {
            get
            {
                return Build;
            }
        }


        internal static Game Game
        {
            get
            {
                return HabboIM.Internal_Game;
            }
            set
            {
                HabboIM.Internal_Game = value;
            }
        }

        public static string FilterText(string Input)
        {
            string lower = Input.ToLower();
            if (lower.Contains("select") && lower.Contains("from") || lower.Contains("insert") && lower.Contains("into") || lower.Contains("update") && lower.Contains("users") && lower.Contains("rank") || (lower.Contains("drop table") || lower.Contains("truncate") || lower.Contains("delete")) || lower.Contains("char("))
                return "";
            return Input;
        }

        public static PacketManager GetPacketManager()
        {
            return HabboIM.PacketManager;
        }

        public static ConfigurationData GetConfig()
        {
            return Configuration;
        }

        public static DatabaseManager GetDatabase()
        {
            return DatabaseManager;
        }

        public static Encoding GetDefaultEncoding()
        {
            return Encoding.Default;
        }

        public static string Version
        {
            get
            {
                return "HabboIM Emulator v" + getNewestVersion(1);
            }
        }
        public static string Developer
        {
            get
            {
                return "Made with <3 by Baumstamm & Jimmy.";
            }
        }
        public static void Check()
        {
            try
            {
                uint gettext = uint.Parse(getLicence(1));
                uint gettext2 = 1;
                    if (gettext != gettext2)
                    {
                        //Nicht signiert
                        Console.WriteLine();
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("                               @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ");
                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*************************************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");
                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*************************************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");
                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*************************************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");

                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*************************************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");
                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*************************************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");
                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*************************************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");

                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
                        Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
                        Console.WriteLine("                               @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("                             ");
                        Console.WriteLine("                                       " + Version);
                        Console.WriteLine("                                   " + Developer);
                        Console.WriteLine("                                         Lizenz: " + licence);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("____________________________________________________________________________________________________");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(licence);
                        Console.WriteLine("Ihre Lizenz ist ausgelaufen. Erhalte eine neue Lizenz via Discord! " + discord + getLicence(1));
                        Console.ReadKey(true);
                        Close();

                    }
                    else
                    {

                        return;
                    }
                
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unable to check for updates now...");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

        }
        public static Version getNewestVersion(int choose)
        {
            if (choose == 1)
            {
                try
                {
                    var match = DownloadServerVersion();

                    var gitVersion = new Version(match);

                    return gitVersion;

                }
                catch (Exception)
                {
                    return Assembly.GetExecutingAssembly().GetName().Version;
                }
            }
            else
            {
                var match = "0.0.0.0";

                var gitVersion = new Version(match);

                return gitVersion;
            }
        }

        public static string DownloadServerVersion()
        {
            using (var wC = new WebClient())
                return
                    wC.DownloadString(
                        "https://raw.githubusercontent.com/EggmcSpeck/Licence/webadress/version");
        }



        public static string getLicence(int choose)
        {
            if (choose == 1)
            {
                try
                {
                    var match2 = DownloadLicence();

                    var gitVersion2 = match2;

                    return gitVersion2;

                }
                catch
                {
                    var match2 = DownloadLicence();

                    var gitVersion2 = match2;

                    return gitVersion2;
                }
                /*catch (Exception)
                {
                    return Assembly.GetExecutingAssembly().GetName().Version;
                }*/
            }
            else
            {
                var match2 = DownloadLicence();

                var gitVersion2 = match2;

                return gitVersion2;
            }
        }

        public static string DownloadLicence()
        {
            using (var wC = new WebClient())
                return
                    wC.DownloadString(
                        "https://raw.githubusercontent.com/EggmcSpeck/Licence/webadress/" + licence);
        }



        public static SocketsManager GetSocketsManager()
        {
            return HabboIM.SocketsManager;
        }

        //public static ConnectionHandeling smethod_14()
        //{
        //    return HabboIM.ConnectionManage;
        //}

        internal static Game GetGame()
        {
            return Internal_Game;
        }

        public static string smethod_0(string string_8)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] array = Encoding.UTF8.GetBytes(string_8);
            array = mD5CryptoServiceProvider.ComputeHash(array);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder.Append(b.ToString("x2").ToLower());
            }
            string text = stringBuilder.ToString();
            return text.ToUpper();
        }

        public static string smethod_1(string string_8)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(string_8);
            byte[] array = new SHA1Managed().ComputeHash(bytes);
            string text = string.Empty;
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                text += b.ToString("X2");
            }
            return text;
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public void Initialize()
        {
            
            HabboIM.ServerStarted = DateTime.Now;
            Console.Title = "HabboIM Emulator wird gestartet..";
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(100, 20);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.CursorVisible = false;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
             try
             {

            Console.WriteLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("                               @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*************************************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*************************************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*************************************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");

            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*************************************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*************************************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*************************************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");

            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*****************"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
            Console.WriteLine("                               @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("                             ");
            Console.WriteLine("                                       " + Version);
            Console.WriteLine("                                   " + Developer);
            Console.WriteLine("                                         Lizenz: " + licence);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("____________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine();
       

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Lotto gestartet! Nächste Ziehung um " + (hour_lastlotto+1) + ":00 Uhr");
            Console.ResetColor();

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            try
            {
                DateTime now = DateTime.Now;
                try
                {
                    HabboIM.Configuration = new ConfigurationData("config.conf");

                    Check();

                    DatabaseServer dbServer = new DatabaseServer(HabboIM.GetConfig().data["db.hostname"], uint.Parse(HabboIM.GetConfig().data["db.port"]), HabboIM.GetConfig().data["db.username"], HabboIM.GetConfig().data["db.password"]);
                    Database database = new Database(HabboIM.GetConfig().data["db.name"], uint.Parse(HabboIM.GetConfig().data["db.pool.minsize"]), uint.Parse(HabboIM.GetConfig().data["db.pool.maxsize"]));
                    HabboIM.DatabaseManager = new DatabaseManager(dbServer, database);
                }
                catch
                {
                    Logging.WriteLine("Der Emulator wurde falsch konfiguriert!");
                    Logging.WriteLine("Press any key to shut down ...");
                    Console.ReadKey(true);
                    HabboIM.Destroy();

                }
                try
                {
                    using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                    {
                        dbClient.ExecuteQuery("SET @@global.sql_mode= '';");
                        dbClient.ExecuteQuery("UPDATE users SET online = '0' WHERE online = '1' ");
                        dbClient.ExecuteQuery("UPDATE rooms SET users_now = '0' WHERE users_now > 0");

                        DataRow DataRow;
                        DataRow = dbClient.ReadDataRow("SHOW COLUMNS FROM `items` WHERE field = 'fw_count'");

                        DataRow DataRow2;
                        DataRow2 = dbClient.ReadDataRow("SHOW COLUMNS FROM `items` WHERE field = 'extra_data'");

                        if (DataRow != null || DataRow2 != null)
                        {
                            if (DoYouWantContinue("Remember get backups before continue! Do you want continue? [Y/N]"))
                            {
                                if (DataRow != null)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("UPDATING ITEMS POSSIBLY TAKE A LONG TIME! DONT SHUTDOWN EMULATOR! PLEASE WAIT!");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.Write("Items werden geupdatet (Feuerwerke) ...");

                                    dbClient.ExecuteQuery("DROP TABLE IF EXISTS items_firework", int.MaxValue);
                                    dbClient.ExecuteQuery("CREATE TABLE IF NOT EXISTS `items_firework` (`item_id` int(10) unsigned NOT NULL, `fw_count` int(10) NOT NULL, PRIMARY KEY (`item_id`)) ENGINE=MyISAM DEFAULT CHARSET=latin1;", int.MaxValue);
                                    dbClient.ExecuteQuery("INSERT INTO items_firework SELECT Id, fw_count FROM items WHERE fw_count > 0;", int.MaxValue);
                                    dbClient.ExecuteQuery("ALTER TABLE items DROP fw_count", int.MaxValue);

                                    Console.WriteLine("Erfolgreich!");
                                }

                                if (DataRow2 != null)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("UPDATING ITEMS POSSIBLY TAKE A LONG TIME! DONT SHUTDOWN EMULATOR! PLEASE WAIT!");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.Write("Items werden geupdatet (Extra data) ...");

                                    dbClient.ExecuteQuery("DROP TABLE IF EXISTS items_extra_data", int.MaxValue);
                                    dbClient.ExecuteQuery("CREATE TABLE IF NOT EXISTS `items_extra_data` (`item_id` int(10) unsigned NOT NULL, `extra_data` text NOT NULL, PRIMARY KEY (`item_id`)) ENGINE=MyISAM DEFAULT CHARSET=latin1;", int.MaxValue);
                                    dbClient.ExecuteQuery("INSERT INTO items_extra_data SELECT Id, extra_data FROM items WHERE extra_data != '';", int.MaxValue);
                                    dbClient.ExecuteQuery("ALTER TABLE items DROP extra_data", int.MaxValue);

                                    Console.WriteLine("Erfolgreich!");
                                }

                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Logging.WriteLine("Press any key to shut down ...");
                                Console.ReadKey(true);
                                HabboIM.Destroy();
                                Logging.WriteLine("Press any key to close window ...");
                                Console.ReadKey(true);
                                Environment.Exit(0);
                                return;
                            }
                        }
                    }
                    //HabboIM.ConnectionManage.method_7();
                    HabboIM.Internal_Game.ContinueLoading();
                }
                catch { }
                Console.BackgroundColor = ConsoleColor.Black;

                try
                {
                    HabboIM.Internal_Game = new Game(int.Parse(HabboIM.GetConfig().data["game.tcp.conlimit"]));
                }
                catch
                {

                    Console.BackgroundColor = ConsoleColor.Black;
                    Logging.WriteLine("Fehler bei den Permissions");
                    Logging.WriteLine("Press any key to shut down ...");
                    Console.ReadKey(true);
                    HabboIM.Destroy();

                }

                try
                {
                    HabboIM.PacketManager = new PacketManager();

                    HabboIM.PacketManager.Handshake();

                    HabboIM.PacketManager.Messenger();

                    HabboIM.PacketManager.Navigator();

                    HabboIM.PacketManager.RoomsAction();
                    HabboIM.PacketManager.RoomsAvatar();
                    HabboIM.PacketManager.RoomsChat();
                    HabboIM.PacketManager.RoomsEngine();
                    HabboIM.PacketManager.RoomsFurniture();
                    HabboIM.PacketManager.RoomsPets();
                    HabboIM.PacketManager.RoomsPools();
                    HabboIM.PacketManager.RoomsSession();
                    HabboIM.PacketManager.RoomsSettings();

                    HabboIM.PacketManager.Catalog();
                    HabboIM.PacketManager.Marketplace();
                    HabboIM.PacketManager.Recycler();

                    HabboIM.PacketManager.Quest();

                    HabboIM.PacketManager.InventoryAchievements();
                    HabboIM.PacketManager.InventoryAvatarFX();
                    HabboIM.PacketManager.InventoryBadges();
                    HabboIM.PacketManager.InventoryFurni();
                    HabboIM.PacketManager.InventoryPurse();
                    HabboIM.PacketManager.InventoryTrading();

                    HabboIM.PacketManager.Avatar();
                    HabboIM.PacketManager.Users();
                    HabboIM.PacketManager.Register();

                    HabboIM.PacketManager.Help();

                    HabboIM.PacketManager.Sound();

                    HabboIM.PacketManager.Wired();

                    HabboIM.PacketManager.Jukebox();

                    HabboIM.PacketManager.FriendStream();

                }
                catch
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Logging.WriteLine("Fehler bei wat weiß ich.");
                    Logging.WriteLine("Press any key to shut down ...");
                    Console.ReadKey(true);
                    HabboIM.Destroy();
                }
                try
                {
                    HabboIM.webSocketManager = new WebSocketServerManager(HabboIM.GetConfig().data["websockets.url"]);
                    HabboIM.MusListener = new MusListener(HabboIM.GetConfig().data["mus.tcp.bindip"], int.Parse(HabboIM.GetConfig().data["mus.tcp.port"]), HabboIM.GetConfig().data["mus.tcp.allowedaddr"].Split(new char[] { ';' }), 20);
                    HabboIM.SocketsManager = new SocketsManager(HabboIM.GetConfig().data["game.tcp.bindip"], int.Parse(HabboIM.GetConfig().data["game.tcp.port"]), int.Parse(HabboIM.GetConfig().data["game.tcp.conlimit"]));
                    HabboIM.SocketsManager.method_3().method_0();
                }
                catch
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Logging.WriteLine("MUS Verbindung fehlgeschlagen!");
                    Logging.WriteLine("Press any key to shut down ...");
                    Console.ReadKey(true);
                    HabboIM.Destroy();

                }
                TimeSpan timeSpan = DateTime.Now - now;
                Console.BackgroundColor = ConsoleColor.Black;
                Logging.WriteLine(string.Concat(new object[]
                    {
                        "HabboIM -> BEREIT! (",
                        timeSpan.Seconds,
                        " s, ",
                        timeSpan.Milliseconds,
                        " ms)"
                    }));
                Console.Beep();
            }
            catch (KeyNotFoundException KeyNotFoundException)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Logging.WriteLine("Failed to boot, key not found: " + KeyNotFoundException);
                Logging.WriteLine("Press any key to shut down ...");
                Console.ReadKey(true);
                HabboIM.Destroy();
            }

            catch (InvalidOperationException ex)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Logging.WriteLine("Failed to initialize Unicorn: " + ex.Message);
                Logging.WriteLine("Press any key to shut down ...");
                Console.ReadKey(true);
                HabboIM.Destroy();
            }

             }
            catch
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Logging.WriteLine("Configuration not found ");
                Logging.WriteLine("Press any key to shut down ...");
                Console.ReadKey(true);
                HabboIM.Destroy();
            }
        }

        public static int StringToInt(string str)
        {
            return Convert.ToInt32(str);
        }

        public static bool StringToBoolean(string str)
        {
            return (str == "1" || str == "true");
        }

        public static string BooleanToString(bool b)
        {
            if (b)
                return "1";
            else
                return "0";
        }

        public static int smethod_5(int int_3, int int_4)
        {
            RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] array = new byte[4];
            rNGCryptoServiceProvider.GetBytes(array);
            int seed = BitConverter.ToInt32(array, 0);
            return new Random(seed).Next(int_3, int_4 + 1);
        }

        public static double GetUnixTimestamp()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        public static string FilterString(string str)
        {
            return DoFilter(str, false, false);
        }

        public static string DoFilter(string Input, bool bool_2, bool bool_3)
        {
            Input = Input.Replace(Convert.ToChar(1), ' ');
            Input = Input.Replace(Convert.ToChar(2), ' ');
            Input = Input.Replace(Convert.ToChar(9), ' ');
            if (!bool_2)
            {
                Input = Input.Replace(Convert.ToChar(13), ' ');
            }
            if (bool_3)
            {
                Input = Input.Replace('\'', ' ');
            }
            return Input;
        }

        public static bool smethod_9(string string_8)
        {
            if (string.IsNullOrEmpty(string_8))
            {
                return false;
            }
            else
            {
                for (int i = 0; i < string_8.Length; i++)
                {
                    if (!char.IsLetter(string_8[i]) && !char.IsNumber(string_8[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        public static void Destroy()
        {
            Program.DeleteMenu(Program.GetSystemMenu(Program.GetConsoleWindow(), true), Program.SC_CLOSE, Program.MF_BYCOMMAND);
            Console.BackgroundColor = ConsoleColor.Black;
            Logging.WriteLine("Destroying Unicorn environment...");

            if (HabboIM.GetGame() != null)
            {
                HabboIM.GetGame().ContinueLoading();
                HabboIM.Internal_Game = null;
            }

            if (HabboIM.GetSocketsManager() != null)
            {
                Logging.WriteLine("Destroying connection manager.");
                HabboIM.GetSocketsManager().method_3().method_2();
                //HabboIM.smethod_14().Destroy();
                HabboIM.GetSocketsManager().method_0();
                HabboIM.SocketsManager = null;
            }

            if (HabboIM.GetDatabase() != null)
            {
                try
                {
                    Logging.WriteLine("Destroying database manager.");
                    MySqlConnection.ClearAllPools();
                    HabboIM.DatabaseManager = null;
                }
                catch
                {
                }
            }

            Logging.WriteLine("Uninitialized successfully. Closing.");
        }

        internal static void smethod_17(string string_8)
        {
            try
            {
                ServerMessage Message = new ServerMessage(139u);
                Message.AppendStringWithBreak(string_8);
                HabboIM.GetGame().GetClientManager().BroadcastMessage(Message);
            }
            catch
            {
            }
        }

        internal static void Close()
        {
            HabboIM.Destroy("", true);
        }

        internal static void Destroy(string string_8, bool ExitWhenDone, bool waitExit = false)
        {
            Program.DeleteMenu(Program.GetSystemMenu(Program.GetConsoleWindow(), true), Program.SC_CLOSE, Program.MF_BYCOMMAND);

            try
            {
                Internal_Game.StopGameLoop();
            }
            catch { }

            try
            {
                if (HabboIM.GetPacketManager() != null)
                {
                    HabboIM.GetPacketManager().Clear();
                }
            }
            catch { }

            if (string_8 != "")
            {
                if (HabboIM.bool_1)
                {
                    return;
                }
                Console.WriteLine(string_8);
                Logging.Disable();
                HabboIM.smethod_17("ATTENTION:\r\nThe server is shutting down. All furniture placed in rooms/traded/bought after this message is on your own responsibillity.");
                HabboIM.bool_1 = true;
                Console.WriteLine("Server shutting down...");
                try
                {
                    HabboIM.Internal_Game.GetRoomManager().method_4();
                }
                catch
                {
                }
                try
                {
                    HabboIM.GetSocketsManager().method_3().method_1();
                    //HabboIM.smethod_14().Destroy();
                    HabboIM.GetGame().GetClientManager().CloseAll();
                }
                catch
                {
                }
                try
                {
                    Console.WriteLine("Destroying database manager.");
                    MySqlConnection.ClearAllPools();
                    HabboIM.DatabaseManager = null;
                }
                catch
                {
                }
                Console.WriteLine("System disposed, goodbye!");
            }
            else
            {
                Logging.Disable();
                HabboIM.bool_1 = true;
                try
                {
                    if (HabboIM.Internal_Game != null && HabboIM.Internal_Game.GetRoomManager() != null)
                    {
                        HabboIM.Internal_Game.GetRoomManager().UnloadAllRooms();
                        HabboIM.Internal_Game.GetRoomManager().method_4();
                    }
                }
                catch
                {
                }
                try
                {
                    if (HabboIM.GetSocketsManager() != null)
                    {
                        HabboIM.GetSocketsManager().method_3().method_1();
                        //HabboIM.smethod_14().Destroy();
                        HabboIM.GetGame().GetClientManager().CloseAll();
                    }
                }
                catch
                {
                }
                if (SocketsManager != null)
                {
                    //HabboIM.ConnectionManage.method_7();
                }
                if (HabboIM.Internal_Game != null)
                {
                    HabboIM.Internal_Game.ContinueLoading();
                }
                Console.WriteLine(string_8);
            }
            if (ExitWhenDone)
            {
                if (waitExit)
                {
                    Console.WriteLine("Press any key to exit..");
                    Console.ReadKey();
                }

                Environment.Exit(0);
            }
        }

        public static bool CanBeDividedBy(int i, int j)
        {
            return i % j == 0;
        }

        public static DateTime TimestampToDate(double timestamp)
        {
            DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return result.AddSeconds(timestamp).ToLocalTime();
        }


        public static int[] IntToArray(string numbers)
        {
            string[] ColorList = numbers.Split(new char[]
            {
                '|'
            });

            var digits = new List<int>();

            if (ColorList.Count() > 1)
            {
                for (int i = 0; i < ColorList.Count(); i++)
                {
                    digits.Add(int.Parse(ColorList[i]));
                }
            }
            else
            {
                digits.Add(int.Parse(ColorList[0]));
            }

            var arr = digits.ToArray();
            Array.Reverse(arr);
            return arr;
        }

        public static void RainbowText(string text, int[] colors, int color, int interval, int count, int maxcount, bool randomcolors, int lastcolor)
        {
            if (count > maxcount)
            {
                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write("\r{0}   ", text);
                Console.WriteLine();
                return;
            }

            count++;

            if (randomcolors)
            {
                Random random = new Random();
                int randomcolor = random.Next(1, 15);

                while (lastcolor == randomcolor || randomcolor == 0)
                {
                    randomcolor = random.Next(1, 15);
                }

                color = randomcolor;
            }
            else
            {
                if (colors.Count() > 1)
                {
                    if (!(color >= 0 && color <= 15))
                    {
                        color = 0;
                    }

                    while (!colors.Contains(color) || lastcolor == color || (!(color >= 0 && color <= 15)))
                    {
                        color++;

                        if (!(color >= 0 && color <= 15))
                        {
                            color = 0;
                        }
                    }
                }
                else
                {
                    color = colors[1];
                }
            }

            if (color > 0 && color <= 15)
            {
                Console.ForegroundColor = (ConsoleColor)color;
                Console.Write("\r{0}   ", text);
                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.White;
                lastcolor = color;
            }

            System.Threading.Thread.Sleep(interval);

            RainbowText(text, colors, color, interval, count, maxcount, randomcolors, lastcolor);
        }

        public static bool DoYouWantContinue(string message)
        {
            Console.WriteLine(message);
            ConsoleKeyInfo ConsoleKeyInfo = Console.ReadKey();
            if (ConsoleKeyInfo.Key == ConsoleKey.Y)
            {
                return true;
            }
            else if (ConsoleKeyInfo.Key == ConsoleKey.N)
            {
                return false;
            }
            else
            {
                DoYouWantContinue(message);
            }

            return false;
        }
        public static WebSocketServerManager GetWebSocketManager()
        {
            return HabboIM.webSocketManager;
        }


    }
}
