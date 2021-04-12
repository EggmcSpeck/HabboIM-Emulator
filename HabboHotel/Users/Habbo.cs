using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Rooms;
using HabboIM.HabboHotel.Users.Badges;
using HabboIM.HabboHotel.Users.Inventory;
using HabboIM.HabboHotel.Users.Messenger;
using HabboIM.HabboHotel.Users.Subscriptions;
using HabboIM.HabboHotel.Users.UserDataManagement;
using HabboIM.Messages;
using HabboIM.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace HabboIM.HabboHotel.Users
{
    internal sealed class Habbo
    {

        public int mylottozahl = 999;
        public double last_dia = 0.0;
        public double last_ente = 0.0;
        public double last_gearbeitet = 0.0;
        public bool collector = false;
        public bool knastarbeit = false;
        public int bez_level;
        public uint sexanfrage = 0;
        public int lovepoints;
        public double kisstime;
        public double hugtime;
        public int boyfriend;
        public int gchat = 1;
        public int loginha = 1;
        public int Duty;
        public int Shisha = 0;
        public uint Id;
        public int CasinoPlace;
        public int Hochdruecken;
        public int CasinoWin;
        public string Username;
        public string static_id;
        public string RealName;
        public double support_last;

        public double jailtime;
        public int jail;
        public double werber_time = 0;
        public bool IsJuniori;

        public bool IsVisible;

        public bool TradingDisabled;
        public DateTime kicktime;
        public int cmdspam;
        public string SSO;

        public string LastIp;

        public uint Rank;

        public string Motto;

        public string Figure;

        public string Gender;

        public string PetData;

        public bool WerberWarnungOne;

        public int WerberWarnungOneTime;

        public bool WerberWarnungTwo;

        public bool WerberWarnungThree;


        public bool BeleidigungWarnungOne;

        public int BeleidigungWarnungOneTime;

        public bool BeleidigungWarnungTwo;

        public bool BeleidigungWarnungThree;


        public bool whisperlog = true;

        public int int_0;

        public DataTable dataTable_0;

        public List<int> list_0;

        public int int_1;

        public int Credits;

        public int ActivityPoints;

        public double LastActivityPointsUpdate;

        public bool IsMuted;

        public int int_4;

        internal bool bool_4 = false;

        public uint uint_2;

        public bool bool_5;

        public bool bool_6;

        public uint CurrentRoomId;

        public uint HomeRoomId;

        public bool bool_7;

        public uint uint_5;

        public List<uint> list_1;

        public List<uint> list_2;

        public List<string> list_3;

        public Dictionary<uint, int> dictionary_0;

        public List<uint> list_4;

        private SubscriptionManager SubscriptionManager;

        private HabboMessenger Messenger;

        private BadgeComponent BadgeComponent;

        private InventoryComponent InventoryComponent;

        private AvatarEffectsInventoryComponent EffectsInventoryComponent;

        private GameClient Session;

        public List<uint> CompletedQuests;

        public uint CurrentQuestId;

        public int CurrentQuestProgress;

        public int BuilderLevel;

        public int SocialLevel;

        public int IdentityLevel;

        public int ExplorerLevel;

        public uint uint_7;

        public int NewbieStatus;

        public bool bool_8;

        public bool bool_9;

        public bool bool_10;

        public bool BlockNewFriends;

        public bool HideInRom;

        public bool HideOnline;

        public bool IsVIP;

        public int VipPoints;

        public int Volume;

        public int AchievementScore;

        public int RoomVisits;

        public int OnlineTime;

        public int LoginTimestamp;

        public int Respect;

        public int RespectGiven;

        public int GiftsGiven;

        public int GiftsReceived;

        private UserDataFactory UserDataFactory;

        internal List<RoomData> OwnedRooms;

        public int int_23;

        public DateTime dateTime_0;

        public bool bool_15;

        public int int_24;

        private bool bool_16 = false;
        public int hugged;
        public int kissed;
        public double lovedate;
        public int FireworkPixelLoadedCount;

        public int NewPetsBuyed;

        public string DataCadastro;

        public string LastOnline;

        public int RegularVisitor;
        public int gc;
        public int PetBuyed;

        public int RegistrationDuration;

        public int FootballGoalScorer;

        public int FootballGoalHost;

        public bool Online = false;

        public int TilesLocked;

        public int RespectPoints;

        public int PetRespectPoints;

        public int StaffPicks;

        public int RandomRares;

        public double LastVipAlert;

        public double LastVipAlertLink;

        public bool FriendStreamEnabled;

        public bool Raumalert;

        public bool WerberCmd;

        public string working;

        public string ChangeNameTime;

        public bool ChangeName;

        public int QuestsCustom1Progress;

        public bool InRoom
        {
            get
            {
                return this.CurrentRoomId >= 1u;
            }
        }

        public Room CurrentRoom
        {
            get
            {
                Room result;
                if (this.CurrentRoomId <= 0u)
                {
                    result = null;
                }
                else
                {
                    result = HabboIM.GetGame().GetRoomManager().GetRoom(this.CurrentRoomId);
                }
                return result;
            }
        }

        internal string UpdateQuery
        {
            get
            {
                this.bool_16 = true;
                this.Online = false;
                int num = (int)HabboIM.GetUnixTimestamp() - this.LoginTimestamp;
                string text = string.Concat(new object[]
                {
                    "UPDATE users SET last_online = UNIX_TIMESTAMP(), online = '0', activity_points_lastupdate = '",
                    this.LastActivityPointsUpdate.ToString().Replace(",", "."),
                    "', jailtime = '",
                    this.jailtime,
                    "', jail = '",
                    this.jail,
                    "' WHERE Id = '",
                    this.Id,
                    "' LIMIT 1; "
                });
                object obj = text;
                return string.Concat(new object[]
                {
                    obj,
                    "UPDATE user_stats SET RoomVisits = '",
                    this.RoomVisits,
                    "', OnlineTime = OnlineTime + ",
                    num,
                    ", Respect = '",
                    this.Respect,
                    "', RespectGiven = '",
                    this.RespectGiven,
                    "', GiftsGiven = '",
                    this.GiftsGiven,
                    "', GiftsReceived = '",
                    this.GiftsReceived,
                    "', FootballGoalScorer = '",
                    this.FootballGoalScorer,
                    "', FootballGoalHost = '",
                    this.FootballGoalHost,
                    "', TilesLocked = '",
                    this.TilesLocked,
                    "', staff_picks = '",
                    this.RandomRares,
                    "', randomrares_received= '",
                    this.StaffPicks,
                    "' WHERE Id = '",
                    this.Id,
                    "' LIMIT 1; "
                });
            }
        }

        private GameClient GetClient()
        {
            return HabboIM.GetGame().GetClientManager().method_2(this.Id);
        }

        public SubscriptionManager GetSubscriptionManager()
        {
            return this.SubscriptionManager;
        }

        public HabboMessenger GetMessenger()
        {
            return this.Messenger;
        }

        public UserDataFactory GetUserDataFactory()
        {
            return this.UserDataFactory;
        }

        public BadgeComponent GetBadgeComponent()
        {
            return this.BadgeComponent;
        }

        public InventoryComponent GetInventoryComponent()
        {
            return this.InventoryComponent;
        }

        public AvatarEffectsInventoryComponent GetEffectsInventoryComponent()
        {
            return this.EffectsInventoryComponent;
        }

        public Habbo(double support_last,int bez_level, double lovedate, int kissed, int hugged, int lovepoints, double kisstime, double hugtime, int boyfriend, int gc,  string static_id, double jailtime, int jail,  uint UserId, string Username, string Name, string SSO, uint Rank, string Motto, string Look, string Gender, int Credits, int Pixels, double Activity_Points_LastUpdate, string DataCadastro, bool Muted, uint HomeRoom, int NewbieStatus, bool BlockNewFriends, bool HideInRoom, bool HideOnline, bool Vip, int Volume, int Points, bool AcceptTrading, string LastIp, GameClient Session, UserDataFactory userDataFactory, string last_online, int daily_respect_points, int daily_pet_respect_points, double vipha_last, double viphal_last, bool FriendStream, bool Raumalert, bool WerberCmd, string working, string ChangeNameTime, bool ChangeName)
        {
            if (Session != null)
            {
                HabboIM.GetGame().GetClientManager().method_0(UserId, Username, Session);
            }
            this.support_last = support_last;
            this.bez_level = bez_level;
            this.lovedate = lovedate;
            this.hugged = hugged;
            this.kissed = kissed;
            this.lovepoints = lovepoints;
            this.kisstime = kisstime;
            this.boyfriend = boyfriend;
            this.hugtime = hugtime;
            this.static_id = static_id;
            this.jailtime = jailtime;
            this.jail = jail;
            this.Id = UserId;
            this.Username = Username;
            this.RealName = Name;
            this.CasinoPlace = 0;
            this.Respect = 3;
            this.Hochdruecken = 0;
            this.CasinoWin = 0;
            this.IsJuniori = false;
            this.IsVisible = true;
            this.SSO = SSO;
            this.Rank = Rank;
            this.Motto = Motto;
            this.Figure = HabboIM.FilterString(Look.ToLower());
            this.Gender = Gender.ToLower();
            this.Credits = Credits;
            this.VipPoints = Points;
            this.ActivityPoints = Pixels;
            this.LastActivityPointsUpdate = Activity_Points_LastUpdate;
            this.TradingDisabled = AcceptTrading;
            this.IsMuted = Muted;
            this.uint_2 = 0u;
            this.bool_5 = false;
            this.bool_6 = false;
            this.CurrentRoomId = 0u;
            this.HomeRoomId = HomeRoom;
            this.list_1 = new List<uint>();
            this.list_2 = new List<uint>();
            this.list_3 = new List<string>();
            this.dictionary_0 = new Dictionary<uint, int>();
            this.list_4 = new List<uint>();
            this.NewbieStatus = NewbieStatus;
            this.bool_10 = false;
            this.BlockNewFriends = BlockNewFriends;
            this.HideInRom = HideInRoom;
            this.HideOnline = HideOnline;
            this.IsVIP = Vip;
            this.Volume = Volume;
            this.int_1 = 0;
            this.int_24 = 1;
            this.LastIp = LastIp;
            this.bool_7 = false;
            this.uint_5 = 0u;
            this.Session = Session;
            this.UserDataFactory = userDataFactory;
            this.OwnedRooms = new List<RoomData>();
            this.list_0 = new List<int>();
            this.DataCadastro = DataCadastro;
            this.LastOnline = last_online;
            this.Online = true;
            this.RespectPoints = 5;
            this.PetRespectPoints = 5;
            this.LastVipAlert = vipha_last;
            this.LastVipAlertLink = viphal_last;
            this.FriendStreamEnabled = FriendStream;
            this.Raumalert = Raumalert;
            this.WerberCmd = WerberCmd;
            this.gc = gc;
            this.working = working;
            this.ChangeNameTime = ChangeNameTime;
            this.ChangeName = ChangeName;
            DataRow dataRow = null;
            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
            {
                dbClient.AddParamWithValue("user_id", UserId);
                dataRow = dbClient.ReadDataRow("SELECT * FROM user_stats WHERE Id = @user_id LIMIT 1", 30);
                if (dataRow == null)
                {
                    dbClient.ExecuteQuery("INSERT INTO user_stats (Id) VALUES ('" + UserId + "')", 30);
                    dataRow = dbClient.ReadDataRow("SELECT * FROM user_stats WHERE Id = @user_id LIMIT 1", 30);
                }
                this.dataTable_0 = dbClient.ReadDataTable("SELECT * FROM group_memberships WHERE userid = @user_id", 30);
                IEnumerator enumerator;
                if (this.dataTable_0 != null)
                {
                    enumerator = this.dataTable_0.Rows.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            DataRow dataRow2 = (DataRow)enumerator.Current;
                            GroupsManager class2 = Groups.smethod_2((int)dataRow2["groupid"]);
                            if (class2 == null)
                            {
                                DataTable dataTable = dbClient.ReadDataTable("SELECT * FROM groups WHERE Id = " + (int)dataRow2["groupid"] + " LIMIT 1;", 30);
                                IEnumerator enumerator2 = dataTable.Rows.GetEnumerator();
                                try
                                {
                                    while (enumerator2.MoveNext())
                                    {
                                        DataRow dataRow3 = (DataRow)enumerator2.Current;
                                        if (!Groups.GroupsManager.ContainsKey((int)dataRow3["Id"]))
                                        {
                                            Groups.GroupsManager.Add((int)dataRow3["Id"], new GroupsManager((int)dataRow3["Id"], dataRow3, dbClient));
                                        }
                                    }
                                    continue;
                                }
                                finally
                                {
                                    IDisposable disposable = enumerator2 as IDisposable;
                                    if (disposable != null)
                                    {
                                        disposable.Dispose();
                                    }
                                }
                            }
                            if (!class2.list_0.Contains((int)UserId))
                            {
                                class2.method_0((int)UserId);
                            }
                        }
                    }
                    finally
                    {
                        IDisposable disposable = enumerator as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                    int num = (int)dataRow["groupid"];
                    GroupsManager class3 = Groups.smethod_2(num);
                    if (class3 != null)
                    {
                        this.int_0 = num;
                    }
                    else
                    {
                        this.int_0 = 0;
                    }
                }
                else
                {
                    this.int_0 = 0;
                }
                DataTable dataTable2 = dbClient.ReadDataTable("SELECT groupid FROM group_requests WHERE userid = '" + UserId + "';", 30);
                enumerator = dataTable2.Rows.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        DataRow dataRow2 = (DataRow)enumerator.Current;
                        this.list_0.Add((int)dataRow2["groupid"]);
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
            this.RoomVisits = (int)dataRow["RoomVisits"];
            this.LoginTimestamp = (int)HabboIM.GetUnixTimestamp();
            this.OnlineTime = (int)dataRow["OnlineTime"];
            this.Respect = (int)dataRow["Respect"];
            this.RespectGiven = (int)dataRow["RespectGiven"];
            this.GiftsGiven = (int)dataRow["GiftsGiven"];
            this.FireworkPixelLoadedCount = (int)dataRow["fireworks"];
            this.GiftsReceived = (int)dataRow["GiftsReceived"];
            this.RandomRares = (int)dataRow["randomrares_received"];
            this.RespectPoints = (int)dataRow["DailyRespectPoints"];
            this.PetRespectPoints = (int)dataRow["DailyPetRespectPoints"];
            this.AchievementScore = (int)dataRow["AchievementScore"];
            this.CompletedQuests = new List<uint>();
            this.uint_7 = 0u;
            this.CurrentQuestId = (uint)dataRow["quest_id"];
            this.CurrentQuestProgress = (int)dataRow["quest_progress"];
            this.BuilderLevel = (int)dataRow["lev_builder"];
            this.IdentityLevel = (int)dataRow["lev_identity"];
            this.SocialLevel = (int)dataRow["lev_social"];
            this.ExplorerLevel = (int)dataRow["lev_explore"];
            this.RegularVisitor = (int)dataRow["RegularVisitor"];
            this.FootballGoalScorer = (int)dataRow["FootballGoalScorer"];
            this.FootballGoalHost = (int)dataRow["FootballGoalHost"];
            this.TilesLocked = (int)dataRow["TilesLocked"];
            this.StaffPicks = (int)dataRow["staff_picks"];
            if (Session != null)
            {
                this.SubscriptionManager = new SubscriptionManager(UserId, userDataFactory);
                this.BadgeComponent = new BadgeComponent(UserId, userDataFactory);
                this.InventoryComponent = new InventoryComponent(UserId, Session, userDataFactory);
                this.EffectsInventoryComponent = new AvatarEffectsInventoryComponent(UserId, Session, userDataFactory);
                this.bool_8 = false;
                this.bool_9 = false;
                foreach (DataRow dataRow3 in userDataFactory.GetRooms().Rows)
                {
                    this.OwnedRooms.Add(HabboIM.GetGame().GetRoomManager().method_17((uint)dataRow3["Id"], dataRow3));
                }
            }
        }

        public void method_0(DatabaseClient class6_0)
        {

            if(this.boyfriend == 0 || this.lovepoints < 100)
            {
                this.int_0 = 0;

            } else
            {


if(this.lovepoints > 100)
                {
                    int code = this.lovepoints % 100;
                    if (this.lovepoints < 50)
                    {
                        this.int_0 = this.lovepoints - code;
                    }
                    else {
                        this.int_0 = this.lovepoints + (100 - code);


                    }
                }
            }
        }

        internal void method_1(DatabaseClient class6_0)
        {
            this.OwnedRooms.Clear();
            class6_0.AddParamWithValue("name", this.Username);
            DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM rooms WHERE owner = @name ORDER BY Id ASC", 30);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.OwnedRooms.Add(HabboIM.GetGame().GetRoomManager().method_17((uint)dataRow["Id"], dataRow));
            }
        }

        public void method_2(UserDataFactory class12_1)
        {
            this.LoadAchievements(class12_1);
            this.LoadFavorites(class12_1);
            this.LoadIgnores(class12_1);
            this.LoadTags(class12_1);
            this.LoadQuests();
        }

        public bool HasFuse(string string_7)
        {
            bool result;
            if (HabboIM.GetGame().GetRoleManager().method_3(this.Id))
            {
                result = HabboIM.GetGame().GetRoleManager().method_4(this.Id, string_7);
            }
            else
            {
                result = HabboIM.GetGame().GetRoleManager().method_1(this.Rank, string_7);
            }
            return result;
        }

        public int method_4()
        {
            return HabboIM.GetGame().GetRoleManager().method_2(this.Rank);
        }

        public void LoadFavorites(UserDataFactory class12_1)
        {
            this.list_1.Clear();
            DataTable dataTable_ = class12_1.GetFavorites();
            foreach (DataRow dataRow in dataTable_.Rows)
            {
                this.list_1.Add((uint)dataRow["room_id"]);
            }
        }

        public void LoadIgnores(UserDataFactory userdata)
        {
            DataTable dataTable_ = userdata.GetIgnores();
            foreach (DataRow dataRow in dataTable_.Rows)
            {
                this.list_2.Add((uint)dataRow["ignore_id"]);
            }
        }

        public void LoadTags(UserDataFactory userdata)
        {
            this.list_3.Clear();
            DataTable dataTable_ = userdata.GetTags();
            foreach (DataRow dataRow in dataTable_.Rows)
            {
                this.list_3.Add((string)dataRow["tag"]);
            }
            if (this.list_3.Count >= 5 && this.GetClient() != null)
            {
                this.TagAchievementsCompleted();
            }
        }

        public void LoadAchievements(UserDataFactory userdata)
        {
            DataTable dataTable = userdata.GetAchievements();
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (!this.dictionary_0.ContainsKey((uint)dataRow["achievement_id"]))
                    {
                        this.dictionary_0.Add((uint)dataRow["achievement_id"], (int)dataRow["achievement_level"]);
                    }
                }
            }
        }

        public void Dispose()
        {
            if (!this.bool_9)
            {
                this.bool_9 = true;
                HabboIM.GetGame().GetClientManager().method_1(this.Id, this.Username);
                if (!this.bool_16)
                {
                    this.bool_16 = true;
                    this.Online = false;
                    using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                    {
                        dbClient.ExecuteQuery(string.Concat(new object[]
                        {
                            "UPDATE users SET last_online = UNIX_TIMESTAMP(), users.online = '0', activity_points = '",
                            this.ActivityPoints,
                            "', activity_points_lastupdate = '",
                            this.LastActivityPointsUpdate.ToString().Replace(",", "."),
                            "', credits = '",
                            this.Credits,
                            "' WHERE Id = '",
                            this.Id,
                            "' LIMIT 1;"
                        }), 30);
                        int num = (int)HabboIM.GetUnixTimestamp() - this.LoginTimestamp;
                        dbClient.ExecuteQuery(string.Concat(new object[]
                        {
                            "UPDATE user_stats SET RoomVisits = '",
                            this.RoomVisits,
                            "', OnlineTime = OnlineTime + ",
                            num,
                            ", Respect = '",
                            this.Respect,
                            "', RespectGiven = '",
                            this.RespectGiven,
                            "', GiftsGiven = '",
                            this.GiftsGiven,
                            "', randomrares_received = '",
                            this.RandomRares,
                            "', GiftsReceived = '",
                            this.GiftsReceived,
                            "', FootballGoalScorer = '",
                            this.FootballGoalScorer,
                            "', FootballGoalHost = '",
                            this.FootballGoalHost,
                            "', TilesLocked = '",
                            this.TilesLocked,
                            "', staff_picks = '",
                            this.StaffPicks,
                            "' WHERE Id = '",
                            this.Id,
                            "' LIMIT 1; "
                        }), 30);
                    }
                }
                if (this.InRoom && this.CurrentRoom != null)
                {
                    this.CurrentRoom.method_47(this.Session, false, false);
                }
                if (this.Messenger != null)
                {
                    this.Messenger.bool_0 = true;
                    this.Messenger.method_5(true);
                    this.Messenger = null;
                }
                if (this.SubscriptionManager != null)
                {
                    this.SubscriptionManager.GetSubscriptions().Clear();
                    this.SubscriptionManager = null;
                }
                this.InventoryComponent.SavePets();
            }
        }

        internal void SendToRoom(uint roomId)
        {
            if (ServerConfiguration.EnableRoomLog)
            {
                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery(string.Concat(new object[]
                    {
                        "INSERT INTO user_roomvisits (user_id,room_id,entry_timestamp,exit_timestamp,hour,minute) VALUES ('",
                        this.Id,
                        "','",
                        roomId,
                        "',UNIX_TIMESTAMP(),'0','",
                        DateTime.Now.Hour,
                        "','",
                        DateTime.Now.Minute,
                        "')"
                    }), 30);
                }
            }
            this.CurrentRoomId = roomId;
            if (this.CurrentQuestId > 0u && this.CurrentRoom.Owner != this.Username && HabboIM.GetGame().GetQuestManager().GetQuestAction(this.CurrentQuestId) == "ENTEROTHERSROOM")
            {
                HabboIM.GetGame().GetQuestManager().ProgressUserQuest(this.CurrentQuestId, this.GetClient());
            }
            this.Messenger.method_5(false);
        }

        public void RemoveFromRoom()
        {
            try
            {
                if (ServerConfiguration.EnableRoomLog)
                {
                    using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
                    {
                        @class.ExecuteQuery(string.Concat(new object[]
                        {
                            "UPDATE user_roomvisits SET exit_timestamp = UNIX_TIMESTAMP() WHERE room_id = '",
                            this.CurrentRoomId,
                            "' AND user_id = '",
                            this.Id,
                            "' ORDER BY entry_timestamp DESC LIMIT 1"
                        }), 30);
                    }
                }
            }
            catch
            {
            }
            this.CurrentRoomId = 0u;
            if (this.Messenger != null)
            {
                this.Messenger.method_5(false);
            }
        }

        public void method_12()
        {
            if (this.GetMessenger() == null)
            {
                this.Messenger = new HabboMessenger(this.Id);
                this.Messenger.method_0(this.UserDataFactory);
                this.Messenger.method_1(this.UserDataFactory);
                GameClient client = this.GetClient();
                if (client != null)
                {
                    client.SendMessage(this.Messenger.method_21());
                    client.SendMessage(this.Messenger.method_23());
                    this.Messenger.method_5(true);
                }
            }
        }
        public void UpdateJail(bool updateDatabase)
        {


            if (updateDatabase)
            {
                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery(string.Concat(new object[]
                    {
                        "UPDATE users SET jail = '",
                        this.jail,
                        "' WHERE Id = '",
                        this.Id,
                        "' LIMIT 1;"
                    }));
                }
                uint num2 = Convert.ToUInt32(1732);
                Room class3 = HabboIM.GetGame().GetRoomManager().method_15(num2);
                ServerMessage Message2 = new ServerMessage(286u);
                Message2.AppendBoolean(class3.IsPublic);
                Message2.AppendUInt(1732);
                Session.SendMessage(Message2);
            }
        }

        public void UpdateTime()
        {
            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
            {
                dbClient.ExecuteQuery(string.Concat(new object[]
                {
                        "UPDATE users SET lastkiss_time = '",
                        this.kisstime,
                        "', lasthug_time = '",
                    this.hugtime,
                    "', lovepoints = '",
                    this.lovepoints,
                    "', kisses_bf = '",
                    this.kissed,
                    "', hugs_bf = '",
                    this.hugged,
                    "' WHERE Id = '",
                        this.Id,
                        "' OR Id = '",
                this.boyfriend,
                "' LIMIT 2;"
                }));
            }

        }


        public void UpdateJailTime(bool updateDatabase)
        {


            if (updateDatabase)
            {
                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery(string.Concat(new object[]
                    {
                        "UPDATE users SET jailtime = '",
                        this.jailtime,
                        "' WHERE Id = '",
                        this.Id,
                        "' LIMIT 1;"
                    }));
                }

            }
        }

        public void UpdateGC()
        {


            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
            {
                dbClient.ExecuteQuery(string.Concat(new object[]
                {
                        "UPDATE users SET gc = '",
                        this.gc,
                        "' WHERE Id = '",
                        this.Id,
                        "' LIMIT 1;"
                }));
            }
        }

        public void UpdateSupport()
        {
            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
            {
                dbClient.ExecuteQuery(string.Concat(new object[]
                {
                        "UPDATE users SET support_last = '",
                        this.support_last,
                        "' WHERE Id = '",
                        this.Id,
                        "' LIMIT 1;"
                }));
            }
        }

        public void UpdateCredits(bool updateDatabase)
        {
            ServerMessage Message = new ServerMessage(6u);
            Message.AppendStringWithBreak(this.Credits + ".0");
            this.Session.SendMessage(Message);
            if (updateDatabase)
            {
                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery(string.Concat(new object[]
                    {
                        "UPDATE users SET credits = '",
                        this.Credits,
                        "' WHERE Id = '",
                        this.Id,
                        "' LIMIT 1;"
                    }), 30);
                }
            }
        }

        public void UpdateBezLevel(bool updateDatabase)
        {
            if (updateDatabase)
            {
                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery(string.Concat(new object[]
                    {
                        "UPDATE users SET bz_level = '",
                        this.bez_level,
                        "' WHERE Id = '",
                        this.Id,
                        "' LIMIT 1;"
                    }), 30);
                }
            }
        }

        public void UpdateVipPoints(bool getPoints, bool updateDatabase)
        {
            if (getPoints)
            {
                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    this.VipPoints = dbClient.ReadInt32("SELECT vip_points FROM users WHERE Id = '" + this.Id + "' LIMIT 1;", 30);
                }
            }
            if (updateDatabase)
            {
                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery(string.Concat(new object[]
                    {
                        "UPDATE users SET vip_points = '",
                        this.VipPoints,
                        "' WHERE Id = '",
                        this.Id,
                        "' LIMIT 1;"
                    }), 30);
                }
            }
            this.method_16(0);
        }
 
        public void UpdateActivityPoints(bool updateDatabase)
        {
            this.method_16(0);
            if (updateDatabase)
            {
                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery(string.Concat(new object[]
                    {
                        "UPDATE users SET activity_points = '",
                        this.ActivityPoints,
                        "' WHERE Id = '",
                        this.Id,
                        "' LIMIT 1;"
                    }), 30);
                }
            }
        }

        public void method_16(int int_25)
        {
            ServerMessage Message = new ServerMessage(438u);
            Message.AppendInt32(this.ActivityPoints);
            Message.AppendInt32(int_25);
            Message.AppendInt32(0);
            ServerMessage Message2 = new ServerMessage(438u);
            Message2.AppendInt32(this.VipPoints);
            Message2.AppendInt32(0);
            Message2.AppendInt32(1);
            ServerMessage Message3 = new ServerMessage(438u);
            Message3.AppendInt32(this.VipPoints);
            Message3.AppendInt32(0);
            Message3.AppendInt32(2);
            ServerMessage Message4 = new ServerMessage(438u);
            Message4.AppendInt32(this.VipPoints);
            Message4.AppendInt32(0);
            Message4.AppendInt32(3);
            ServerMessage Message5 = new ServerMessage(438u);
            Message5.AppendInt32(this.VipPoints);
            Message5.AppendInt32(0);
            Message5.AppendInt32(4);
            this.Session.SendMessage(Message);
            this.Session.SendMessage(Message2);
            this.Session.SendMessage(Message3);
            this.Session.SendMessage(Message4);
            this.Session.SendMessage(Message5);
        }

        public void Mute()
        {
            if (!this.IsMuted)
            {
                this.GetClient().SendNotification(HabboIMEnvironment.GetExternalText("error_muted_text"));
                this.IsMuted = true;

                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    dbClient.ExecuteQuery("UPDATE users SET is_muted = '1' WHERE id = " + this.Id);
                }
            }
        }

        public void UnMute()
        {
            this.GetClient().SendNotification(HabboIMEnvironment.GetExternalText("success_unmute_text"));
            this.IsMuted = false;

            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
            {
                dbClient.ExecuteQuery("UPDATE users SET is_muted = '0' WHERE id = " + this.Id);
            }
        }

        public void LoadQuests()
        {
            this.CompletedQuests.Clear();
            DataTable dataTable = null;
            using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
            {
                dataTable = @class.ReadDataTable("SELECT quest_id FROM user_quests WHERE user_id = '" + this.Id + "'", 30);
            }
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    this.CompletedQuests.Add((uint)dataRow["quest_Id"]);
                }
            }
        }

        public void UpdateLook(bool getFromDatabase, GameClient client)
        {
            if (this.PetData == null)
            {
                ServerMessage Message = new ServerMessage(266u);
                Message.AppendInt32(-1);
                Message.AppendStringWithBreak(client.GetHabbo().Figure);
                Message.AppendStringWithBreak(client.GetHabbo().Gender.ToLower());
                Message.AppendStringWithBreak(client.GetHabbo().Motto);
                Message.AppendInt32(client.GetHabbo().AchievementScore);
                Message.AppendStringWithBreak("");
                client.SendMessage(Message);
                if (client.GetHabbo().InRoom)
                {
                    Room room = client.GetHabbo().CurrentRoom;
                    if (room != null)
                    {
                        RoomUser roomUser = room.GetRoomUserByHabbo(client.GetHabbo().Id);
                        if (roomUser != null)
                        {
                            if (getFromDatabase)
                            {
                                DataRow dataRow = null;
                                using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                                {
                                    class2.AddParamWithValue("userid", client.GetHabbo().Id);
                                    dataRow = class2.ReadDataRow("SELECT * FROM users WHERE Id = @userid LIMIT 1", 30);
                                }
                                client.GetHabbo().Motto = HabboIM.FilterString((string)dataRow["motto"]);
                                client.GetHabbo().Figure = HabboIM.FilterString((string)dataRow["look"]);
                            }
                            ServerMessage Message2 = new ServerMessage(266u);
                            Message2.AppendInt32(roomUser.VirtualId);
                            Message2.AppendStringWithBreak(client.GetHabbo().Figure);
                            Message2.AppendStringWithBreak(client.GetHabbo().Gender.ToLower());
                            Message2.AppendStringWithBreak(client.GetHabbo().Motto);
                            Message2.AppendInt32(client.GetHabbo().AchievementScore);
                            Message2.AppendStringWithBreak("");
                            room.SendMessage(Message2, null);
                        }
                    }
                }
            }
        }

        public void UpdateRights()
        {
            DataRow dataRow;
            using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
            {
                dataRow = @class.ReadDataRow("SELECT vip FROM users WHERE Id = '" + this.Id + "' LIMIT 1;", 30);
            }
            this.IsVIP = HabboIM.StringToBoolean(dataRow["vip"].ToString());
            ServerMessage Message = new ServerMessage(2u);
            if (this.IsVIP || ServerConfiguration.HabboClubForClothes)
            {
                Message.AppendInt32(2);
            }
            else if (this.GetSubscriptionManager().HasSubscription("habbo_club"))
            {
                Message.AppendInt32(1);
            }
            else
            {
                Message.AppendInt32(0);
            }
            if (this.HasFuse("acc_anyroomowner"))
            {
                Message.AppendInt32(7);
            }
            else if (this.HasFuse("acc_anyroomrights"))
            {
                Message.AppendInt32(5);
            }
            else if (this.HasFuse("acc_supporttool"))
            {
                Message.AppendInt32(4);
            }
            else if (this.IsVIP || ServerConfiguration.HabboClubForClothes || this.GetSubscriptionManager().HasSubscription("habbo_club"))
            {
                Message.AppendInt32(2);
            }
            else
            {
                Message.AppendInt32(0);
            }
            this.GetClient().SendMessage(Message);
        }

        public void Whisper(string str)
        {
            Room room = HabboIM.GetGame().GetRoomManager().GetRoom(this.CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUser = room.GetRoomUserByHabbo(this.Id);
                ServerMessage Message = new ServerMessage(25u);
                Message.AppendInt32(roomUser.VirtualId);
                Message.AppendStringWithBreak(str);
                Message.AppendBoolean(false);
                this.GetClient().SendMessage(Message);
            }
        }

        public void CheckFireworkAchievements()
        {
            int Count = this.FireworkPixelLoadedCount;
            if (Count > 0)
            {
                if (Count >= 20)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 13u, 1);
                }
                if (Count >= 100)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 13u, 2);
                }
                if (Count >= 420)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 13u, 3);
                }
                if (Count >= 600)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 13u, 4);
                }
                if (Count >= 1920)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 13u, 5);
                }
                if (Count >= 3120)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 13u, 6);
                }
                if (Count >= 4620)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 13u, 7);
                }
                if (Count >= 6420)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 13u, 8);
                }
                if (Count >= 8520)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 13u, 9);
                }
                if (Count >= 10920)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 13u, 10);
                }
            }
        }

        public void MottoAchievementsCompleted()
        {
            HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 2u, 1);
        }

        public void TagAchievementsCompleted()
        {
            HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 7u, 1);
        }

        public void AvatarLookAchievementsCompleted()
        {
            HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 1u, 1);
        }

        public void CallGuideBotAchievementsCompleted()
        {
            HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 3u, 1);
        }

        public void ChangeNamaAchievementsCompleted()
        {
            HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 5u, 1);
        }

        public void CheckRandomRares()
        {
            int Count = this.RandomRares;
            if (Count >= 0)
            {
                if (Count >= 1)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 9307620u, 1);
                }
                if (Count >= 5)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 9307620u, 2);
                }
                if (Count >= 15)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 9307620u, 3);
                }
                if (Count >= 35)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 9307620u, 4);
                }
                if (Count >= 50)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 9307620u, 5);
                }
            }
        }

        public void CheckRoomEntryAchievements()
        {
            int Count = this.RoomVisits;
            if (Count > 0)
            {
                if (Count >= 5)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 1);
                }
                if (Count >= 20)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 2);
                }
                if (Count >= 50)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 3);
                }
                if (Count >= 100)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 4);
                }
                if (Count >= 160)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 5);
                }
                if (Count >= 240)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 6);
                }
                if (Count >= 360)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 7);
                }
                if (Count >= 500)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 8);
                }
                if (Count >= 660)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 9);
                }
                if (Count >= 860)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 10);
                }
                if (Count >= 1080)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 11);
                }
                if (Count >= 1320)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 12);
                }
                if (Count >= 1580)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 13);
                }
                if (Count >= 1860)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 14);
                }
                if (Count >= 2160)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 15);
                }
                if (Count >= 2480)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 16);
                }
                if (Count >= 2820)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 17);
                }
                if (Count >= 3180)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 18);
                }
                if (Count >= 3560)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 19);
                }
                if (Count >= 3960)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 8u, 20);
                }
            }
        }

        public void CheckRespectGivedAchievements()
        {
            int Count = this.RespectGiven;
            if (Count > 0)
            {
                if (Count >= 2)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 1);
                }
                if (Count >= 5)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 2);
                }
                if (Count >= 10)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 3);
                }
                if (Count >= 20)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 4);
                }
                if (Count >= 40)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 5);
                }
                if (Count >= 70)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 6);
                }
                if (Count >= 110)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 7);
                }
                if (Count >= 170)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 8);
                }
                if (Count >= 250)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 9);
                }
                if (Count >= 350)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 10);
                }
                if (Count >= 470)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 11);
                }
                if (Count >= 610)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 12);
                }
                if (Count >= 770)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 13);
                }
                if (Count >= 950)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 14);
                }
                if (Count >= 1150)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 15);
                }
                if (Count >= 1370)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 16);
                }
                if (Count >= 1610)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 17);
                }
                if (Count >= 1870)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 18);
                }
                if (Count >= 2150)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 19);
                }
                if (Count >= 2450)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 4u, 20);
                }
            }
        }

        public void CheckRespectReceivedAchievements()
        {
            int Count = this.Respect;
            if (Count > 0)
            {
                if (Count >= 1)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 6u, 1);
                }
                if (Count >= 6)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 6u, 2);
                }
                if (Count >= 16)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 6u, 3);
                }
                if (Count >= 66)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 6u, 4);
                }
                if (Count >= 166)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 6u, 5);
                }
                if (Count >= 366)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 6u, 6);
                }
                if (Count >= 566)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 6u, 7);
                }
                if (Count >= 766)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 6u, 8);
                }
                if (Count >= 966)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 6u, 9);
                }
                if (Count >= 1166)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 6u, 10);
                }
            }
        }

        public void CheckGiftReceivedAchievements()
        {
            int Count = this.GiftsReceived;
            if (Count > 0)
            {
                if (Count >= 1)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 11u, 1);
                }
                if (Count >= 6)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 11u, 2);
                }
                if (Count >= 14)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 11u, 3);
                }
                if (Count >= 26)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 11u, 4);
                }
                if (Count >= 46)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 11u, 5);
                }
                if (Count >= 86)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 11u, 6);
                }
                if (Count >= 146)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 11u, 7);
                }
                if (Count >= 236)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 11u, 8);
                }
                if (Count >= 366)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 11u, 9);
                }
                if (Count >= 566)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 11u, 10);
                }
            }
        }

        public void CheckGiftGivenAchievements()
        {
            int Count = this.GiftsGiven;
            if (Count > 0)
            {
                if (Count >= 1)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 1);
                }
                if (Count >= 6)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 2);
                }
                if (Count >= 14)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 3);
                }
                if (Count >= 26)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 4);
                }
                if (Count >= 46)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 5);
                }
                if (Count >= 86)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 6);
                }
                if (Count >= 146)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 7);
                }
                if (Count >= 236)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 8);
                }
                if (Count >= 366)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 9);
                }
                if (Count >= 566)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 10);
                }
                if (Count >= 816)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 11);
                }
                if (Count >= 1066)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 12);
                }
                if (Count >= 1316)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 13);
                }
                if (Count >= 1566)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 14);
                }
                if (Count >= 1816)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 10u, 15);
                }
            }
        }

        public void CheckTotalTimeOnlineAchievements()
        {
            int Count = this.OnlineTime;
            if (Count > 0)
            {
                if (Count >= 1800)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 1);
                }
                if (Count >= 3600)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 2);
                }
                if (Count >= 7200)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 3);
                }
                if (Count >= 10800)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 4);
                }
                if (Count >= 21600)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 5);
                }
                if (Count >= 43200)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 6);
                }
                if (Count >= 86400)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 7);
                }
                if (Count >= 129600)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 8);
                }
                if (Count >= 172800)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 9);
                }
                if (Count >= 259200)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 10);
                }
                if (Count >= 432000)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 11);
                }
                if (Count >= 604800)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 12);
                }
                if (Count >= 1209600)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 13);
                }
                if (Count >= 1814400)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 14);
                }
                if (Count >= 2419200)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 15);
                }
                if (Count >= 3024000)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 16);
                }
                if (Count >= 3628800)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 17);
                }
                if (Count >= 4838400)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 18);
                }
                if (Count >= 6048000)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 19);
                }
                if (Count >= 8294400)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 15u, 20);
                }
            }
        }

        public void CheckPetCountAchievements()
        {
            int Count = 0;
            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
            {
                dbClient.AddParamWithValue("sessionid", this.Session.GetHabbo().Id);
                DataTable dataTable = dbClient.ReadDataTable("SELECT user_id FROM  `user_pets` WHERE user_id = @sessionid;", 30);
                if (dataTable == null)
                {
                    Count = 0;
                }
                else
                {
                    Count = dataTable.Rows.Count;
                }
            }
            Count = (this.PetBuyed = Count + this.NewPetsBuyed);
            if (Count > 0)
            {
                if (Count >= 1)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 14u, 1);
                }
                if (Count >= 5)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 14u, 2);
                }
                if (Count >= 10)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 14u, 3);
                }
                if (Count >= 15)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 14u, 4);
                }
                if (Count >= 20)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 14u, 5);
                }
                if (Count >= 25)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 14u, 6);
                }
                if (Count >= 30)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 14u, 7);
                }
                if (Count >= 40)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 14u, 8);
                }
                if (Count >= 50)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 14u, 9);
                }
                if (Count >= 75)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 14u, 10);
                }
            }
        }

        public void CheckHappyHourAchievements()
        {
            string s = DateTime.Now.ToString("HH:mm:ss");
            TimeSpan time = DateTime.ParseExact(s, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault).TimeOfDay;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday || DateTime.Now.DayOfWeek == DayOfWeek.Wednesday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday || DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                if (time >= new TimeSpan(15, 0, 0) && time <= new TimeSpan(17, 0, 0))
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 9u, 1);
                }
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                if (time >= new TimeSpan(13, 0, 0) && time <= new TimeSpan(14, 0, 0))
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 9u, 1);
                }
            }
        }

        public void CheckTrueHabboAchievements()
        {
            double dAccountCreated;
            DateTime AccountCreated;
            if (double.TryParse(this.Session.GetHabbo().DataCadastro, out dAccountCreated))
            {
                AccountCreated = HabboIM.TimestampToDate(dAccountCreated);
            }
            else if (!DateTime.TryParseExact(this.Session.GetHabbo().DataCadastro, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out AccountCreated))
            {
                return;
            }
            string hoje = DateTime.Now.ToString("dd-MM-yyyy");
            string[] Hoje = hoje.Split(new char[]
            {
                '-'
            });
            DateTime dataCadastro = Convert.ToDateTime(AccountCreated);
            DateTime data_hoje = new DateTime(int.Parse(Hoje[2]), int.Parse(Hoje[1]), int.Parse(Hoje[0]));
            int Dias = data_hoje.Subtract(dataCadastro).Days;
            this.Session.GetHabbo().RegistrationDuration = Dias;
            if (Dias >= 1)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 1);
            }
            if (Dias >= 3)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 2);
            }
            if (Dias >= 10)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 3);
            }
            if (Dias >= 20)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 4);
            }
            if (Dias >= 30)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 5);
            }
            if (Dias >= 56)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 6);
            }
            if (Dias >= 84)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 7);
            }
            if (Dias >= 126)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 8);
            }
            if (Dias >= 168)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 9);
            }
            if (Dias >= 224)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 10);
            }
            if (Dias >= 280)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 11);
            }
            if (Dias >= 365)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 12);
            }
            if (Dias >= 548)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 13);
            }
            if (Dias >= 730)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 14);
            }
            if (Dias >= 913)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 15);
            }
            if (Dias >= 1095)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 16);
            }
            if (Dias >= 1278)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 17);
            }
            if (Dias >= 1460)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 18);
            }
            if (Dias >= 1643)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 19);
            }
            if (Dias >= 1825)
            {
                HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 17u, 20);
            }
        }

        public void CheckRegularVisitorAchievements()
        {
            DateTime LastLoggedIn = HabboIM.TimestampToDate(double.Parse(this.Session.GetHabbo().LastOnline));
            DateTime yesterday = DateTime.Now.AddDays(-1.0);
            if (LastLoggedIn.ToString("dd-MM-yyyy") == yesterday.ToString("dd-MM-yyyy"))
            {
                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    this.RegularVisitor++;
                    dbClient.AddParamWithValue("sessionid", this.Session.GetHabbo().Id);
                    dbClient.ExecuteQuery("UPDATE user_stats SET RegularVisitor = RegularVisitor + 1 WHERE id = @sessionid LIMIT 1", 30);
                    dbClient.AddParamWithValue("daily_respect_points", this.Session.GetHabbo().RespectPoints);
                    dbClient.AddParamWithValue("daily_pet_respect_points", this.Session.GetHabbo().PetRespectPoints);
                    dbClient.ExecuteQuery("UPDATE user_stats SET DailyRespectPoints = @daily_respect_points, DailyPetRespectPoints = @daily_pet_respect_points WHERE id = @sessionid", 30);
                }
            }
            else if (!(LastLoggedIn.ToString("dd-MM-yyyy") == DateTime.Now.ToString("dd-MM-yyyy")))
            {
                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                {
                    this.RegularVisitor = 1;
                    dbClient.AddParamWithValue("sessionid", this.Session.GetHabbo().Id);
                    dbClient.ExecuteQuery("UPDATE user_stats SET RegularVisitor = 1 WHERE id = @sessionid LIMIT 1", 30);
                    dbClient.AddParamWithValue("daily_respect_points", this.Session.GetHabbo().RespectPoints);
                    dbClient.AddParamWithValue("daily_pet_respect_points", this.Session.GetHabbo().PetRespectPoints);
                    dbClient.ExecuteQuery("UPDATE user_stats SET DailyRespectPoints = @daily_respect_points, DailyPetRespectPoints = @daily_pet_respect_points WHERE id = @sessionid", 30);
                }
            }
            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
            {
                dbClient.AddParamWithValue("sessionid", this.Session.GetHabbo().Id);
                dbClient.ExecuteQuery("UPDATE users SET last_loggedin = UNIX_TIMESTAMP() WHERE id = @sessionid", 30);
            }
            int Count = this.RegularVisitor;
            if (Count > 0)
            {
                if (Count >= 5)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 1);
                }
                if (Count >= 8)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 2);
                }
                if (Count >= 15)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 3);
                }
                if (Count >= 28)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 4);
                }
                if (Count >= 35)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 5);
                }
                if (Count >= 50)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 6);
                }
                if (Count >= 60)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 7);
                }
                if (Count >= 70)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 8);
                }
                if (Count >= 80)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 9);
                }
                if (Count >= 100)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 10);
                }
                if (Count >= 120)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 11);
                }
                if (Count >= 140)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 12);
                }
                if (Count >= 160)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 13);
                }
                if (Count >= 180)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 14);
                }
                if (Count >= 200)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 15);
                }
                if (Count >= 220)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 16);
                }
                if (Count >= 240)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 17);
                }
                if (Count >= 260)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 18);
                }
                if (Count >= 280)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 19);
                }
                if (Count >= 300)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 18u, 20);
                }
            }
        }

        public void CheckHCAchievements()
        {
            int Count = this.Session.GetHabbo().GetSubscriptionManager().CalculateHCSubscription(this.Session.GetHabbo());
            if (Count >= 0)
            {
                if (Count >= 0)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 16u, 1);
                }
                if (Count >= 12)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 16u, 2);
                }
                if (Count >= 24)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 16u, 3);
                }
                if (Count >= 36)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 16u, 4);
                }
                if (Count >= 48)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 16u, 5);
                }
            }
        }

        public void CheckFootballGoalScorerScoreAchievements()
        {
            int Count = this.Session.GetHabbo().FootballGoalScorer;
            if (Count > 0)
            {
                if (Count >= 1)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 19u, 1);
                }
                if (Count >= 10)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 19u, 2);
                }
                if (Count >= 100)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 19u, 3);
                }
                if (Count >= 1000)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 19u, 4);
                }
                if (Count >= 10000)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 19u, 5);
                }
            }
        }

        public void CheckFootballGoalHostScoreAchievements()
        {
            int Count = this.Session.GetHabbo().FootballGoalHost;
            if (Count > 0)
            {
                if (Count >= 1)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 20u, 1);
                }
                if (Count >= 20)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 20u, 2);
                }
                if (Count >= 400)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 20u, 3);
                }
                if (Count >= 8000)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 20u, 4);
                }
                if (Count >= 160000)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 20u, 5);
                }
            }
        }

        public void CheckBattleBanzaiTilesLockedAchievements()
        {
            int Count = this.TilesLocked;
            if (Count > 0)
            {
                if (Count >= 25)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 1);
                }
                if (Count >= 65)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 2);
                }
                if (Count >= 125)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 3);
                }
                if (Count >= 205)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 4);
                }
                if (Count >= 335)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 5);
                }
                if (Count >= 525)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 6);
                }
                if (Count >= 805)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 7);
                }
                if (Count >= 1235)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 8);
                }
                if (Count >= 1875)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 9);
                }
                if (Count >= 2875)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 10);
                }
                if (Count >= 4375)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 11);
                }
                if (Count >= 6875)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 12);
                }
                if (Count >= 10775)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 13);
                }
                if (Count >= 17075)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 14);
                }
                if (Count >= 27175)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 15);
                }
                if (Count >= 43275)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 16);
                }
                if (Count >= 69075)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 17);
                }
                if (Count >= 110375)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 18);
                }
                if (Count >= 176375)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 19);
                }
                if (Count >= 282075)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 21u, 20);
                }
            }
        }
        internal void StoreActivity(string type, uint roomid, double timestamp, string Params)
        {
            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
            {
                dbClient.AddParamWithValue("param1", roomid);
                dbClient.AddParamWithValue("param2", Params);
                dbClient.ExecuteQuery("INSERT INTO hp_aktivitaetenstream (`user_id`,`type`,`extra_data`,`extra_data2`,`timestamp`) VALUES ('" + this.Id + "','" + type + "',@param1,@param2,'" + Convert.ToInt32(timestamp) + "');");
            }
        }
        public void CheckStaffPicksAchievement()
        {
            int Count = this.Session.GetHabbo().StaffPicks;
            if (Count > 0)
            {
                if (Count >= 1)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 22u, 1);
                }
                if (Count >= 2)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 22u, 2);
                }
                if (Count >= 3)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 22u, 3);
                }
                if (Count >= 4)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 22u, 4);
                }
                if (Count >= 5)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 22u, 5);
                }
                if (Count >= 6)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 22u, 6);
                }
                if (Count >= 7)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 22u, 7);
                }
                if (Count >= 8)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 22u, 8);
                }
                if (Count >= 9)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 22u, 9);
                }
                if (Count >= 10)
                {
                    HabboIM.GetGame().GetAchievementManager().addAchievement(this.Session, 22u, 10);
                }
            }
        }
    }
}
