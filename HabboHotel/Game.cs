using HabboIM.Core;
using HabboIM;
using HabboIM.HabboHotel.Achievements;
using HabboIM.HabboHotel.Advertisements;
using HabboIM.HabboHotel.Catalogs;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Items;
using HabboIM.HabboHotel.Misc;
using HabboIM.HabboHotel.Navigators;
using HabboIM.HabboHotel.Quests;
using HabboIM.HabboHotel.Roles;
using HabboIM.HabboHotel.RoomBots;
using HabboIM.HabboHotel.Rooms;
using HabboIM.HabboHotel.Support;
using HabboIM.Storage;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace HabboIM.HabboHotel
{
    internal sealed class Game
    {
        private const int GameLoopSleepTime = 25;

        public bool AntiWerberStatus = true;

        private GameClientManager ClientManager;

        private ModerationBanManager BanManager;

        private RoleManager RoleManager;

        private HelpTool HelpTool;

        private Catalog Catalog;

        private Navigator Navigator;

        private ItemManager ItemManager;

        private RoomManager RoomManager;

        private AdvertisementManager AdvertisementManager;

        private PixelManager PixelManager;

        private AchievementManager AchievementManager;

        private ModerationTool ModerationTool;

        private BotManager BotManager;

        private Task task_0;

        private NavigatorCache NavigatorCache;

        private Marketplace Marketplace;

        private QuestManager QuestManager;

        private HabboIMEnvironment HabboIMEnvironment;

        private Groups Groups;

        private Task GameLoop;

        private bool GameLoopActive;

        private bool GameLoopEnded = true;

        public Game(int conns)
        {
            this.ClientManager = new GameClientManager(conns);
            if (HabboIM.GetConfig().data["client.ping.enabled"] == "1")
            {
                this.ClientManager.method_10();
            }
            DateTime now = DateTime.Now;
            Logging.Write("Verbinde mit Datenbank.. ");
            try
            {
                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Logging.WriteLine("Fertig!", ConsoleColor.Green);
                    HabboIM.Game = this;
                    this.LoadServerSettings(dbClient);
                    this.BanManager = new ModerationBanManager();
                    this.RoleManager = new RoleManager();
                    this.HelpTool = new HelpTool();
                    this.Catalog = new Catalog();
                    this.Navigator = new Navigator();
                    this.ItemManager = new ItemManager();
                    this.RoomManager = new RoomManager();
                    this.AdvertisementManager = new AdvertisementManager();
                    this.PixelManager = new PixelManager();
                    this.AchievementManager = new AchievementManager();
                    this.ModerationTool = new ModerationTool();
                    this.BotManager = new BotManager();
                    this.Marketplace = new Marketplace();
                    this.QuestManager = new QuestManager();
                    this.HabboIMEnvironment = new HabboIMEnvironment();
                    this.Groups = new Groups();
                    HabboIMEnvironment.LoadExternalTexts(dbClient);
                    this.BanManager.Initialise(dbClient);
                    this.RoleManager.method_0(dbClient);
                    this.HelpTool.method_0(dbClient);
                    this.HelpTool.method_3(dbClient);
                    this.ModerationTool.method_1(dbClient);
                    this.ModerationTool.method_2(dbClient);
                    this.ItemManager.method_0(dbClient);
                    this.Catalog.method_0(dbClient);
                    this.Catalog.method_1();
                    this.Navigator.method_0(dbClient);
                    this.RoomManager.method_8(dbClient);
                    this.RoomManager.method_0();
                    this.NavigatorCache = new NavigatorCache();
                    this.AdvertisementManager.method_0(dbClient);
                    this.BotManager.method_0(dbClient);
                    AchievementManager.smethod_0(dbClient);
                    this.PixelManager.method_0();
                    ChatCommandHandler.smethod_0(dbClient);
                    this.QuestManager.method_0();
                    Groups.smethod_0(dbClient);
                    this.RestoreStatistics(dbClient, 1);
                }
            }
            catch (MySqlException e)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Logging.WriteLine("Fehler!", ConsoleColor.Red);
                Logging.WriteLine(e.Message + " Check the given configuration details in config.conf\r\n", ConsoleColor.Blue);
                HabboIM.Destroy("", true, true);
                return;
            }
            this.task_0 = new Task(new Action(LowPriorityWorker.Initialise));
            this.task_0.Start();
            this.StartGameLoop();
        }

        public void RestoreStatistics(DatabaseClient dbClient, int status)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Logging.Write(HabboIMEnvironment.GetExternalText("emu_cleandb"));
            bool flag = true;
            try
            {
                if (int.Parse(HabboIM.GetConfig().data["debug"]) == 1)
                {
                    flag = false;
                }
            }
            catch
            {
            }
            if (flag)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                dbClient.ExecuteQuery("UPDATE users SET online = '0' WHERE online != '0'", 30);
                dbClient.ExecuteQuery("UPDATE rooms SET users_now = '0' WHERE users_now != '0'", 30);
                dbClient.ExecuteQuery("UPDATE user_roomvisits SET exit_timestamp = UNIX_TIMESTAMP() WHERE exit_timestamp <= 0", 30);
                dbClient.ExecuteQuery(string.Concat(new object[]
				{
					"UPDATE server_status SET status = '",
					status,
					"', users_online = '0', rooms_loaded = '0', server_ver = '",
					HabboIM.Version,
					"', stamp = UNIX_TIMESTAMP() LIMIT 1;"
				}), 30);
            }
            Logging.WriteLine("Fertig!", ConsoleColor.Green);
        }

        public void ContinueLoading()
        {
            if (this.task_0 != null)
            {
                this.task_0 = null;
            }
            try
            {
                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    this.RestoreStatistics(dbClient, 0);
                }
            }
            catch (MySqlException)
            {
            }
            if (this.GetClientManager() != null)
            {
                this.GetClientManager().method_6();
                this.GetClientManager().method_11();
            }
            if (this.GetPixelManager() != null)
            {
                this.PixelManager.KeepAlive = false;
            }
            this.ClientManager = null;
            this.BanManager = null;
            this.RoleManager = null;
            this.HelpTool = null;
            this.Catalog = null;
            this.Navigator = null;
            this.ItemManager = null;
            this.RoomManager = null;
            this.AdvertisementManager = null;
            this.PixelManager = null;
        }

        public GameClientManager GetClientManager()
        {
            return this.ClientManager;
        }

        public ModerationBanManager GetBanManager()
        {
            return this.BanManager;
        }

        public RoleManager GetRoleManager()
        {
            return this.RoleManager;
        }

        public HelpTool GetHelpTool()
        {
            return this.HelpTool;
        }

        public Catalog GetCatalog()
        {
            return this.Catalog;
        }

        public Navigator GetNavigator()
        {
            return this.Navigator;
        }

        public ItemManager GetItemManager()
        {
            return this.ItemManager;
        }

        public RoomManager GetRoomManager()
        {
            return this.RoomManager;
        }

        public AdvertisementManager GetAdvertisementManager()
        {
            return this.AdvertisementManager;
        }

        public PixelManager GetPixelManager()
        {
            return this.PixelManager;
        }

        public AchievementManager GetAchievementManager()
        {
            return this.AchievementManager;
        }

        public ModerationTool GetModerationTool()
        {
            return this.ModerationTool;
        }

        public BotManager GetBotManager()
        {
            return this.BotManager;
        }

        internal NavigatorCache GetNavigatorCache()
        {
            return this.NavigatorCache;
        }

        public QuestManager GetQuestManager()
        {
            return this.QuestManager;
        }

        public void LoadServerSettings(DatabaseClient class6_0)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Logging.Write("Lädt Einstellungen..");
            DataRow dataRow = class6_0.ReadDataRow("SELECT * FROM server_settings LIMIT 1", 30);
            ServerConfiguration.RoomUserLimit = (int)dataRow["MaxRoomsPerUser"];
            ServerConfiguration.MOTD = (string)dataRow["motd"];
            ServerConfiguration.CreditingInterval = (int)dataRow["timer"];
            ServerConfiguration.CreditingAmount = (int)dataRow["credits"];
            ServerConfiguration.PointingAmount = (int)dataRow["pixels"];
            ServerConfiguration.PixelingAmount = (int)dataRow["points"];
            ServerConfiguration.PixelLimit = (int)dataRow["pixels_max"];
            ServerConfiguration.CreditLimit = (int)dataRow["credits_max"];
            ServerConfiguration.PointLimit = (int)dataRow["points_max"];
            ServerConfiguration.PetsPerRoomLimit = (int)dataRow["MaxPetsPerRoom"];
            ServerConfiguration.MarketplacePriceLimit = (int)dataRow["MaxMarketPlacePrice"];
            ServerConfiguration.MarketplaceTax = (int)dataRow["MarketPlaceTax"];
            ServerConfiguration.DDoSProtectionEnabled = HabboIM.StringToBoolean(dataRow["enable_antiddos"].ToString());
            ServerConfiguration.HabboClubForClothes = HabboIM.StringToBoolean(dataRow["vipclothesforhcusers"].ToString());
            ServerConfiguration.EnableChatlog = HabboIM.StringToBoolean(dataRow["enable_chatlogs"].ToString());
            ServerConfiguration.EnableCommandLog = HabboIM.StringToBoolean(dataRow["enable_cmdlogs"].ToString());
            ServerConfiguration.EnableRoomLog = HabboIM.StringToBoolean(dataRow["enable_roomlogs"].ToString());
            ServerConfiguration.EnableExternalLinks = (string)dataRow["enable_externalchatlinks"];
            ServerConfiguration.EnableSSO = HabboIM.StringToBoolean(dataRow["enable_securesessions"].ToString());
            ServerConfiguration.AllowFurniDrops = HabboIM.StringToBoolean(dataRow["allow_friendfurnidrops"].ToString());
            ServerConfiguration.EnableRedeemCredits = HabboIM.StringToBoolean(dataRow["enable_cmd_redeemcredits"].ToString());
            ServerConfiguration.EnableRedeemPixels = HabboIM.StringToBoolean(dataRow["enable_cmd_redeempixels"].ToString());
            ServerConfiguration.EnableRedeemShells = HabboIM.StringToBoolean(dataRow["enable_cmd_redeemshells"].ToString());
            ServerConfiguration.UnloadCrashedRooms = HabboIM.StringToBoolean(dataRow["unload_crashedrooms"].ToString());
            ServerConfiguration.ShowUsersAndRoomsInAbout = HabboIM.StringToBoolean(dataRow["ShowUsersAndRoomsInAbout"].ToString());
            ServerConfiguration.SleepTimer = (int)dataRow["idlesleep"];
            ServerConfiguration.KickTimer = (int)dataRow["idlekick"];
            ServerConfiguration.IPLastBan = HabboIM.StringToBoolean(dataRow["ip_lastforbans"].ToString());
            ServerConfiguration.StaffPicksID = (int)dataRow["StaffPicksCategoryID"];
            ServerConfiguration.VIPHotelAlertInterval = (double)dataRow["vipha_interval"];
            ServerConfiguration.VIPHotelAlertLinkInterval = (double)dataRow["viphal_interval"];
            ServerConfiguration.PreventDoorPush = HabboIM.StringToBoolean(dataRow["DisableOtherUsersToMovingOtherUsersToDoor"].ToString());
            Logging.WriteLine("Fertig!", ConsoleColor.Green);
        }

        internal void StartGameLoop()
        {
            this.GameLoopEnded = false;
            this.GameLoopActive = true;
            this.GameLoop = new Task(new Action(this.MainGameLoop));
            this.GameLoop.Start();
        }

        internal void StopGameLoop()
        {
            this.GameLoopActive = false;
            while (!this.GameLoopEnded)
            {
                Thread.Sleep(25);
            }
        }

        private void MainGameLoop()
        {
            while (this.GameLoopActive)
            {
                try
                {
                    this.RoomManager.OnCycle();
                }
                catch
                {
                }
                Thread.Sleep(25);
            }
            this.GameLoopEnded = true;
        }
    }
}
