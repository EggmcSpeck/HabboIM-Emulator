using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using HabboIM.Core;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Util;
using HabboIM.Storage;
namespace HabboIM.HabboHotel.Support
{
	internal sealed class ModerationBanManager
	{
        private Hashtable bannedIPs = new Hashtable();
        private Hashtable bannedUsernames = new Hashtable();
		public List<ModerationBan> Bans;

		public ModerationBanManager()
		{
			this.Bans = new List<ModerationBan>();
		}

		public void Initialise(DatabaseClient dbClient)
		{
            Console.BackgroundColor = ConsoleColor.Black;
            Logging.Write("Lädt bans..");

			this.Bans.Clear();
			
            DataTable dataTable = dbClient.ReadDataTable("SELECT bantype,value,reason,expire FROM bans WHERE expire > '" + HabboIM.GetUnixTimestamp() + "'");
			
            if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					ModerationBanType Type = ModerationBanType.IP;
					if ((string)dataRow["bantype"] == "user")
					{
						Type = ModerationBanType.USERNAME;
					}


                    if ((string)dataRow["bantype"] == "static_id")
                    {
                        Type = ModerationBanType.STATICID;
                    }


                    this.Bans.Add(new ModerationBan(Type, (string)dataRow["value"], (string)dataRow["reason"], (double)dataRow["expire"]));
				}

				Logging.WriteLine("Fertig!", ConsoleColor.Green);
			}
		}

		public void method_1(GameClient Session)
		{
			foreach (ModerationBan current in this.Bans)
			{
				if (!current.Expired)
				{
                    if (Session != null && Session.GetHabbo() != null && current.Type == ModerationBanType.IP && Session.GetConnection().String_0 == current.Variable)
					{
						throw new ModerationBanException(current.ReasonMessage);
					}
					if (Session != null && Session.GetHabbo() != null && (current.Type == ModerationBanType.USERNAME && Session.GetHabbo().Username.ToLower() == current.Variable.ToLower()))
					{
						throw new ModerationBanException(current.ReasonMessage);
					}

                    if (Session != null && Session.GetHabbo() != null && current.Type == ModerationBanType.STATICID && Session.GetHabbo().static_id == current.Variable)
                    {
                        throw new ModerationBanException(current.ReasonMessage);
                    }

                }
			}
		}

		public void BanUser(GameClient Session, string string_0, double length, string reason, bool banIp, bool banStatic)
		{
			if (!Session.GetHabbo().IsJuniori)
			{
				ModerationBanType enum4_ = ModerationBanType.USERNAME;
				string text = Session.GetHabbo().Username;
				string object_ = "user";

				double timestamp = HabboIM.GetUnixTimestamp() + length;

				if (banIp)
				{
					enum4_ = ModerationBanType.IP;

					if (!ServerConfiguration.IPLastBan)
                        text = Session.GetConnection().String_0;
					else
					{
						using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
						{
							text = dbClient.ReadString("SELECT ip_last FROM users WHERE Id = " + Session.GetHabbo().Id + " LIMIT 1;");
						}
					}
					object_ = "ip";
				}


                if (banStatic)
                {
                    enum4_ = ModerationBanType.STATICID;
                    text = Session.GetHabbo().static_id;
                    object_ = "static_id";
                }

                this.Bans.Add(new ModerationBan(enum4_, text, reason, timestamp));

				using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
				{
					dbClient.AddParamWithValue("rawvar", object_);
					dbClient.AddParamWithValue("var", text);
					dbClient.AddParamWithValue("reason", reason);
					dbClient.AddParamWithValue("mod", string_0);

					dbClient.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO bans (bantype,value,reason,expire,added_by,added_date,appeal_state) VALUES (@rawvar,@var,@reason,'",
						timestamp,
						"',@mod,'",
						DateTime.Now.ToLongDateString(),
						"', '1')"
					}));
				}

				if (banIp)
				{
					DataTable dataTable = null;

					using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
					{
						dbClient.AddParamWithValue("var", text);
						dataTable = dbClient.ReadDataTable("SELECT Id FROM users WHERE ip_last = @var");
					}

                    if (dataTable != null)
                    {
                        IEnumerator enumerator = dataTable.Rows.GetEnumerator();
                        try
                        {
                            while (enumerator.MoveNext())
                            {
                                DataRow dataRow = (DataRow)enumerator.Current;
                                using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
                                {
                                    @class.ExecuteQuery("UPDATE user_info SET bans = bans + 1 WHERE user_id = '" + (uint)dataRow["Id"] + "' LIMIT 1");
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
                    }
				}


                if (banStatic)
                {
                    DataTable dataTable = null;

                    using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                    {
                        dbClient.AddParamWithValue("var", text);
                        dataTable = dbClient.ReadDataTable("SELECT Id FROM users WHERE static_id_last = @var AND static_id_last != ''");
                    }

                    if (dataTable != null)
                    {
                        IEnumerator enumerator = dataTable.Rows.GetEnumerator();
                        try
                        {
                            while (enumerator.MoveNext())
                            {
                                DataRow dataRow = (DataRow)enumerator.Current;
                                using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
                                {
                                    @class.ExecuteQuery("UPDATE user_info SET bans = bans + 1 WHERE user_id = '" + (uint)dataRow["Id"] + "' LIMIT 1");
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
                    }
                }


                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
				{
					dbClient.ExecuteQuery("UPDATE user_info SET bans = bans + 1 WHERE user_id = '" + Session.GetHabbo().Id + "' LIMIT 1");
				}
				
				Session.NotifyBan("Du bist gebannt: " + reason);
				Session.method_12();
			}
		}
        //Unban
        public void UnBanUser(string UsernameOrIp)
        {


            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
            {


                List<ModerationBan> list = new List<ModerationBan>();
                this.bannedUsernames.Remove(UsernameOrIp);
                this.bannedIPs.Remove(UsernameOrIp);
                using (DatabaseClient dbClientt = HabboIM.GetDatabase().GetClient())
                {

                    dbClientt.AddParamWithValue("userorip", UsernameOrIp);
                    dbClientt.ExecuteQuery("DELETE FROM bans WHERE value = @userorip");
                }

            }


        }
	}
}
