using HabboIM.Core;
using HabboIM.HabboHotel.Achievements;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Items;
using HabboIM.HabboHotel.Rooms;
using HabboIM.HabboHotel.Users;
using HabboIM.HabboHotel.Users.Authenticator;
using HabboIM.Messages;
using HabboIM.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HabboIM.WebSocket;

namespace HabboIM.HabboHotel.Misc
{
    internal sealed class ChatCommandHandler
    {
        private static List<string> list_0;
        private static Dictionary<int, bool> isBrb = new Dictionary<int, bool>();
        private static List<string> list_1;
        private static List<int> list_2;
        private static List<string> list_3;
        public static void smethod_0(DatabaseClient class6_0)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Logging.Write("Lädt Chat Filter..");
            ChatCommandHandler.list_0 = new List<string>();
            ChatCommandHandler.list_1 = new List<string>();
            ChatCommandHandler.list_2 = new List<int>();
            ChatCommandHandler.list_3 = new List<string>();
            ChatCommandHandler.InitWords(class6_0);
            Logging.WriteLine("Erfolgreich!", ConsoleColor.Green);
        }

        public static void InitWords(DatabaseClient dbClient)
        {
            ChatCommandHandler.list_0.Clear();
            ChatCommandHandler.list_1.Clear();
            ChatCommandHandler.list_2.Clear();
            ChatCommandHandler.list_3.Clear();
            DataTable dataTable = dbClient.ReadDataTable("SELECT * FROM wordfilter ORDER BY word ASC;", 30);
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ChatCommandHandler.list_0.Add(dataRow["word"].ToString());
                    ChatCommandHandler.list_1.Add(dataRow["replacement"].ToString());
                    ChatCommandHandler.list_2.Add(HabboIM.StringToInt(dataRow["strict"].ToString()));
                }
            }
            DataTable dataTable2 = dbClient.ReadDataTable("SELECT * FROM linkfilter;", 30);
            if (dataTable2 != null)
            {
                foreach (DataRow dataRow in dataTable2.Rows)
                {
                    ChatCommandHandler.list_3.Add(dataRow["externalsite"].ToString());
                }
            }
        }

        public static bool InitLinks(string URLs)
        {
            bool result;
            if (ServerConfiguration.EnableExternalLinks == "disabled")
            {
                result = false;
            }
            else
            {
                if ((URLs.StartsWith("http://") || URLs.StartsWith("www.") || URLs.StartsWith("https://")) && ChatCommandHandler.list_3 != null && ChatCommandHandler.list_3.Count > 0)
                {
                    foreach (string current in ChatCommandHandler.list_3)
                    {
                        if (URLs.Contains(current))
                        {
                            if (ServerConfiguration.EnableExternalLinks == "whitelist")
                            {
                                result = true;
                                return result;
                            }
                            if (!(ServerConfiguration.EnableExternalLinks == "blacklist"))
                            {
                            }
                        }
                    }
                }
                bool arg_12D_0;
                if (!URLs.StartsWith("http://") && !URLs.StartsWith("www.") && (!URLs.StartsWith("https://") || !(ServerConfiguration.EnableExternalLinks == "blacklist")))
                {
                    arg_12D_0 = false;
                }
                else
                {
                    arg_12D_0 = true;
                }
                result = arg_12D_0;
            }
            return result;
        }

        public static string smethod_3(string string_0)
        {
            try
            {
            }
            catch
            {
            }
            return string_0;
        }

        public static string smethod_4(string string_0)
        {
            string result;
            if (ChatCommandHandler.list_0 != null && ChatCommandHandler.list_0.Count > 0)
            {
                int num = -1;
                foreach (string current in ChatCommandHandler.list_0)
                {
                    num++;
                    if (string_0.ToLower().Contains(current.ToLower()) && ChatCommandHandler.list_2[num] == 1)
                    {
                        string_0 = Regex.Replace(string_0, current, ChatCommandHandler.list_1[num], RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        if (ChatCommandHandler.list_2[num] == 2)
                        {
                            string cheaters = "\\s*";
                            Regex re = new Regex("\\b(" + string.Join("|", from word in ChatCommandHandler.list_0
                                                                           select string.Join<char>(cheaters, word.ToCharArray())) + ")\\b", RegexOptions.IgnoreCase);
                            result = re.Replace(string_0, (Match match) => ChatCommandHandler.list_1[num]);
                            return result;
                        }
                        if (string_0.ToLower().Contains(" " + current.ToLower() + " "))
                        {
                            string_0 = Regex.Replace(string_0, current, ChatCommandHandler.list_1[num], RegexOptions.IgnoreCase);
                        }
                    }
                }
            }
            result = string_0;
            return result;
        }

        public static string RemoveDiacritics(string s)
        {
            string normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string fnRemoveSplChars(string strMyString)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strMyString.Length; i++)
            {
                char c = strMyString[i];
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string smethod_4b(GameClient Session, string string_0, string werberort)
        {
            if (Session.GetHabbo().Rank <= 3u)
            {
                using (DatabaseClient dbWord = HabboIM.GetDatabase().GetClient())
                {
                    byte[] bytes = Encoding.Default.GetBytes(string_0);
                    string satz2 = Encoding.UTF8.GetString(bytes);
                    satz2 = ChatCommandHandler.RemoveDiacritics(satz2);
                    satz2 = Regex.Replace(satz2, "(\\s+)", "");
                    satz2 = ChatCommandHandler.fnRemoveSplChars(satz2);
                    satz2 = ChatCommandHandler.fnRemoveSplChars(satz2);
                    satz2 = ChatCommandHandler.fnRemoveSplChars(satz2);
                    satz2 = ChatCommandHandler.fnRemoveSplChars(satz2);
                    satz2 = ChatCommandHandler.fnRemoveSplChars(satz2);
                    string satz3 = satz2.ToLower();
                    DataTable dataTable = dbWord.ReadDataTable("SELECT * FROM wordfilter WHERE antiwerber = '1' ORDER BY word ASC;", 30);
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        if (satz3.Contains((string)dataRow["word"]))
                        {
                            string wort = (string)dataRow["word"];
                            if (Session.GetHabbo().WerberWarnungOne)
                            {
                                int WarnungOneTime;
                                int TimeNow;
                                if (Session.GetHabbo().WerberWarnungTwo || Session.GetHabbo().WerberWarnungThree)
                                {
                                    WarnungOneTime = 1;
                                    TimeNow = 0;
                                }
                                else
                                {
                                    WarnungOneTime = Session.GetHabbo().WerberWarnungOneTime + 900;
                                    TimeNow = (int)HabboIM.GetUnixTimestamp();
                                }
                                if (WarnungOneTime > TimeNow)
                                {
                                    if (Session.GetHabbo().WerberWarnungTwo)
                                    {
                                        if (Session.GetHabbo().WerberWarnungThree)
                                        {
                                            ServerMessage Logging = new ServerMessage(134u);
                                            Logging.AppendUInt(0u);

                                            if (werberort == "Chat")
                                            {

                                                Logging.AppendString(string.Concat(new string[]
                                                {
                                                    "BanManager: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") vom System gebannt!\rChatlog: "+ string_0
                                                }));


                                            }
                                            else if (werberort == "GC")
                                            {


                                                Logging.AppendString(string.Concat(new string[]
                                              {
                                        "System: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") im Global Chat vom System verwarnt. - 1/3 Verwarnung(en)!\rChatlog: "+ string_0
                                              }));




                                            }
                                            else
                                            {
                                                Logging.AppendString(string.Concat(new string[]
                                                {
                                                    "BanManager: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") im Raum \"",
                                                    Session.GetHabbo().CurrentRoom.Name,
                                                    "\" vom System gebannt!\rChatlog: "+ string_0
                                                }));
                                            }
                                            HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                            Session.GetHabbo().WerberWarnungThree = false;
                                            Session.GetHabbo().WerberWarnungTwo = false;
                                            Session.GetHabbo().WerberWarnungOne = false;
                                            int banLength = 630720000;
                                            //   HabboIM.GetGame().GetBanManager().BanUser(Session, "System", (double)banLength, "Dauerban", false, false);




                                            Session.GetHabbo().jail = 1;
                                            Session.GetHabbo().jailtime = banLength;
                                            Session.GetHabbo().UpdateJailTime(true);
                                            Session.GetHabbo().UpdateJail(true);

                                            ServerMessage Message = new ServerMessage(35u);
                                            Message.AppendStringWithBreak("Du wurdest dauerhaft aus Habbo gebannt!\r\rBanngrund: Sicherheitsban\rGebannt von: Server-System", 13);
                                            Session.SendMessage(Message);


                                            return "";

                                        }
                                        else
                                        {
                                            Session.GetHabbo().WerberWarnungThree = true;
                                            Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("aws_error_text"));
                                            ServerMessage Logging = new ServerMessage(134u);
                                            Logging.AppendUInt(0u);
                                            if (werberort == "Chat")
                                            {
                                                Logging.AppendString(string.Concat(new string[]
                                                {
                                                    "AWS: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") im Chat vom System verwarnt. - 3/3 Verwarnung(en)!\rChatlog: "+ string_0
                                                }));
                                            }
                                            else if (werberort == "GC")
                                            {


                                                Logging.AppendString(string.Concat(new string[]
                                              {
                                        "AWS: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") im Global Chat vom System verwarnt. - 1/3 Verwarnung(en)!\rChatlog: "+ string_0
                                              }));




                                            }
                                            else
                                            {
                                                Logging.AppendString(string.Concat(new string[]
                                                {
                                                    "AWS: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") im Raum \"",
                                                    Session.GetHabbo().CurrentRoom.Name,
                                                    "\" vom System verwarnt. - 3/3 Verwarnung(en)!\rChatlog: "+ string_0
                                                }));
                                            }
                                            HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);

                                            return "Hobba ist einfach toll!";

                                        }
                                    }
                                    else
                                    {
                                        Session.GetHabbo().WerberWarnungTwo = true;
                                        Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("aws_error_text"));
                                        ServerMessage Logging = new ServerMessage(134u);
                                        Logging.AppendUInt(0u);
                                        if (werberort == "Chat")
                                        {
                                            Logging.AppendString(string.Concat(new string[]
                                            {

                                                "AWS: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") im Chat vom System verwarnt. - 2/3 Verwarnung(en)!\rChatlog: "+ string_0
                                            }));
                                        }
                                        else if (werberort == "GC")
                                        {


                                            Logging.AppendString(string.Concat(new string[]
                                          {
                                        "AWS: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") im Global Chat vom System verwarnt. - 1/3 Verwarnung(en)!\rChatlog: "+ string_0
                                          }));




                                        }
                                        else
                                        {
                                            Logging.AppendString(string.Concat(new string[]
                                            {
                                                "AWS: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") im Raum \"",
                                                    Session.GetHabbo().CurrentRoom.Name,
                                                    "\" vom System verwarnt. - 2/3 Verwarnung(en)!\rChatlog: "+ string_0
                                            }));
                                        }
                                        HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);

                                    }
                                    return "Hobba ist einfach toll!";

                                }
                                else
                                {
                                    Session.GetHabbo().WerberWarnungOne = true;
                                    Session.GetHabbo().WerberWarnungOneTime = (int)HabboIM.GetUnixTimestamp();
                                    Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("aws_error_text"));
                                    ServerMessage Logging = new ServerMessage(134u);
                                    Logging.AppendUInt(0u);
                                    if (werberort == "Chat")
                                    {
                                        Logging.AppendString(string.Concat(new string[]
                                        {
                                            "AWS: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") im Chat vom System verwarnt. - 1/3 Verwarnung(en)!\rChatlog: "+ string_0
                                        }));
                                    }
                                    else if (werberort == "GC")
                                    {


                                        Logging.AppendString(string.Concat(new string[]
                                      {
                                        "AWS: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") im Global Chat vom System verwarnt. - 1/3 Verwarnung(en)!\rChatlog: "+ string_0
                                      }));




                                    }
                                    else
                                    {
                                        Logging.AppendString(string.Concat(new string[]
                                        {
                                            "AWS: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") im Raum \"",
                                                    Session.GetHabbo().CurrentRoom.Name,
                                                    "\" vom System verwarnt. - 1/3 Verwarnung(en)!\rChatlog: "+ string_0
                                        }));
                                    }
                                    HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                }
                                return "Hobba ist einfach toll!";

                            }
                            else
                            {
                                Session.GetHabbo().WerberWarnungOne = true;
                                Session.GetHabbo().WerberWarnungOneTime = (int)HabboIM.GetUnixTimestamp();
                                Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("aws_error_text"));
                                ServerMessage Logging = new ServerMessage(134u);
                                Logging.AppendUInt(0u);
                                if (werberort == "Chat")
                                {
                                    Logging.AppendString(string.Concat(new string[]
                                    {
                                        "AWS: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") im Chat vom System verwarnt. - 1/3 Verwarnung(en)!\rChatlog: "+ string_0
                                    }));
                                }
                                else if (werberort == "GC")
                                {


                                    Logging.AppendString(string.Concat(new string[]
                                  {
                                        "AWS: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") im Global Chat vom System verwarnt. - 1/3 Verwarnung(en)!\rChatlog: "+ string_0
                                  }));




                                }
                                else
                                {
                                    Logging.AppendString(string.Concat(new string[]
                                    {
                                        "AWS: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Verdacht auf Fremdwerbung (",
                                                    wort,
                                                    ") im Raum \"",
                                                    Session.GetHabbo().CurrentRoom.Name,
                                                    "\" vom System verwarnt. - 1/3 Verwarnung(en)!\rChatlog: "+ string_0
                                    }));
                                }
                                HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                return "Hobba ist einfach toll!";

                            }
                        }
                    }
                }
            }
            return string_0;
        }






        public static string amina_zikki(GameClient Session, string string_0, string werberort)
        {
            if (Session.GetHabbo().Rank <= 3u)
            {
                using (DatabaseClient dbWord = HabboIM.GetDatabase().GetClient())
                {
                    byte[] bytes = Encoding.Default.GetBytes(string_0);
                    string satz2 = Encoding.UTF8.GetString(bytes);
                    satz2 = ChatCommandHandler.RemoveDiacritics(satz2);
                    satz2 = Regex.Replace(satz2, "(\\s+)", "");
                    satz2 = ChatCommandHandler.fnRemoveSplChars(satz2);
                    satz2 = ChatCommandHandler.fnRemoveSplChars(satz2);
                    satz2 = ChatCommandHandler.fnRemoveSplChars(satz2);
                    satz2 = ChatCommandHandler.fnRemoveSplChars(satz2);
                    satz2 = ChatCommandHandler.fnRemoveSplChars(satz2);
                    string satz3 = satz2.ToLower();
                    DataTable dataTable = dbWord.ReadDataTable("SELECT * FROM wordfilter WHERE beleidigung = '1' ORDER BY word ASC;", 30);
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        if (satz3.Contains((string)dataRow["word"]))
                        {
                            string wort = (string)dataRow["word"];
                            if (Session.GetHabbo().BeleidigungWarnungOne)
                            {
                                int BeleidigungOneTime;
                                int TimeNow;
                                if (Session.GetHabbo().BeleidigungWarnungTwo || Session.GetHabbo().BeleidigungWarnungThree)
                                {
                                    BeleidigungOneTime = 1;
                                    TimeNow = 0;
                                }
                                else
                                {
                                    BeleidigungOneTime = Session.GetHabbo().BeleidigungWarnungOneTime + 900;
                                    TimeNow = (int)HabboIM.GetUnixTimestamp();
                                }
                                if (BeleidigungOneTime > TimeNow)
                                {
                                    if (Session.GetHabbo().BeleidigungWarnungTwo)
                                    {
                                        if (Session.GetHabbo().BeleidigungWarnungThree)
                                        {
                                            ServerMessage Logging = new ServerMessage(134u);
                                            Logging.AppendUInt(0u);


                                            Logging.AppendString(string.Concat(new string[]
                                            {
                                                    "BanManager: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen mehrfacher Beleidigung (",
                                                    wort,
                                                    ") im Raum \"",
                                                    Session.GetHabbo().CurrentRoom.Name,
                                                    "\" vom System für 15 Minuten gebannt!\rChatlog: "+ string_0
                                            }));

                                            HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                            Session.GetHabbo().BeleidigungWarnungThree = false;
                                            Session.GetHabbo().BeleidigungWarnungTwo = false;
                                            Session.GetHabbo().BeleidigungWarnungOne = false;
                                            int banLength = 15 * 60;
                                            //   HabboIM.GetGame().GetBanManager().BanUser(Session, "System", (double)banLength, "Dauerban", false, false);




                                            Session.GetHabbo().jail = 1;
                                            Session.GetHabbo().jailtime = banLength;
                                            Session.GetHabbo().UpdateJailTime(true);
                                            Session.GetHabbo().UpdateJail(true);

                                            ServerMessage Message = new ServerMessage(35u);
                                            Message.AppendStringWithBreak("Aufgrund deiner Ausdrucksweise wurdest du vom Server-System zu einer 15 Minütigen Auszeit verdonnert!", 13);
                                            Session.SendMessage(Message);


                                            return "";

                                        }
                                        else
                                        {
                                            Session.GetHabbo().BeleidigungWarnungThree = true;
                                            Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("beleidigung_error_text"));
                                            ServerMessage Logging = new ServerMessage(134u);
                                            Logging.AppendUInt(0u);

                                            Logging.AppendString(string.Concat(new string[]
                                            {
                                                    "System: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Beleidigung (",
                                                    wort,
                                                    ") im Raum \"",
                                                    Session.GetHabbo().CurrentRoom.Name,
                                                    "\" vom System verwarnt. - 3/3 Verwarnung(en)!\rChatlog: "+ string_0
                                            }));

                                            HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);

                                            return "Hobba ist einfach toll!";

                                        }
                                    }
                                    else
                                    {
                                        Session.GetHabbo().BeleidigungWarnungTwo = true;
                                        Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("beleidigung_error_text"));

                                        ServerMessage Logging = new ServerMessage(134u);
                                        Logging.AppendUInt(0u);



                                        Logging.AppendString(string.Concat(new string[]
                                        {
                                                "System: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Beleidigung (",
                                                    wort,
                                                    ") im Raum \"",
                                                    Session.GetHabbo().CurrentRoom.Name,
                                                    "\" vom System verwarnt. - 2/3 Verwarnung(en)!\rChatlog: "+ string_0
                                        }));

                                        HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);

                                    }
                                    return "Hobba ist einfach toll!";

                                }
                                else
                                {
                                    Session.GetHabbo().BeleidigungWarnungOne = true;
                                    Session.GetHabbo().BeleidigungWarnungOneTime = (int)HabboIM.GetUnixTimestamp();
                                    Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("beleidigung_error_text"));
                                    ServerMessage Logging = new ServerMessage(134u);
                                    Logging.AppendUInt(0u);

                                    Logging.AppendString(string.Concat(new string[]
                                    {
                                            "System: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Beleidigung (",
                                                    wort,
                                                    ") im Raum \"",
                                                    Session.GetHabbo().CurrentRoom.Name,
                                                    "\" vom System verwarnt. - 1/3 Verwarnung(en)!\rChatlog: "+ string_0
                                    }));

                                    HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                }
                                return "Hobba ist einfach toll!";

                            }
                            else
                            {
                                Session.GetHabbo().BeleidigungWarnungOne = true;
                                Session.GetHabbo().BeleidigungWarnungOneTime = (int)HabboIM.GetUnixTimestamp();
                                Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("beleidigung_error_text"));
                                ServerMessage Logging = new ServerMessage(134u);
                                Logging.AppendUInt(0u);

                                Logging.AppendString(string.Concat(new string[]
                                {
                                        "System: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" wurde wegen Beleidigung (",
                                                    wort,
                                                    ") im Raum \"",
                                                    Session.GetHabbo().CurrentRoom.Name,
                                                    "\" vom System verwarnt. - 1/3 Verwarnung(en)!\rChatlog: "+ string_0
                                }));

                                HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                return "Hobba ist einfach toll!";

                            }
                        }
                    }
                }
            }
            return string_0;
        }


        public static bool smethod_5(GameClient Session, string Input)
        {
            string[] Params = Input.Split(new char[]
            {
                ' '
            });



            TimeSpan timeSpanxx = DateTime.Now - Session.GetHabbo().kicktime;
            if (timeSpanxx.Seconds > 4)
            {
                Session.GetHabbo().cmdspam = 0;
            }
            if (timeSpanxx.Seconds < 4 && Session.GetHabbo().cmdspam > 5)
            {

                Session.GetHabbo().CurrentRoom.method_47(Session, true, false);
                Session.SendNotification("Du hast innerhalb von 5 Sekunden zuviele Befehle ausgeführt!\r\rBitte benutze unsere Commands etwas langsamer.");
                return true;
            }
            Session.GetHabbo().kicktime = DateTime.Now;
            Session.GetHabbo().cmdspam++;






            if (Session.GetHabbo().jail == 1 && Session.GetHabbo().jailtime < 1)
            {





                Session.GetHabbo().jail = 0;
                Session.GetHabbo().jailtime = 0.0;
                Session.GetHabbo().UpdateJail(true);
                Session.GetHabbo().UpdateJailTime(true);
                Room @classx = HabboIM.GetGame().GetRoomManager().GetRoom(1732);
                @classx.method_47(Session, true, false);
                Session.SendNotification("Dein Bann wurde automatisch aufgehoben.");

            }

            if (Session.GetHabbo().jail == 1)
            {
                Session.GetHabbo().Whisper("Du bist gebannt und kannst keine Commands ausführen!");
                return true;
            }

            GameClient TargetClient = null;
            Room class2 = Session.GetHabbo().CurrentRoom;
            bool result;
            if (!HabboIM.GetGame().GetRoleManager().dictionary_4.ContainsKey(Params[0]))
            {
                result = false;
            }
            else
            {
                try
                {
                    int num;
                    if (class2 != null && class2.CheckRights(Session, true))
                    {
                        num = HabboIM.GetGame().GetRoleManager().dictionary_4[Params[0]];
                        if (num <= 33)
                        {
                            if (num == 8)
                            {
                                class2 = Session.GetHabbo().CurrentRoom;
                                if (class2.bool_5)
                                {
                                    class2.bool_5 = false;
                                }
                                else
                                {
                                    class2.bool_5 = true;
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            if (num == 33)
                            {
                                class2 = Session.GetHabbo().CurrentRoom;
                                if (class2 != null && class2.CheckRights(Session, true))
                                {
                                    List<RoomItem> list = class2.method_24(Session);
                                    Session.GetHabbo().GetInventoryComponent().method_17(list);
                                    Session.GetHabbo().GetInventoryComponent().method_9(true);
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input + " " + Session.GetHabbo().CurrentRoomId);
                                    result = true;
                                    return result;
                                }
                                result = false;
                                return result;
                            }
                        }
                        else
                        {
                            if (num == 46)
                            {
                                class2 = Session.GetHabbo().CurrentRoom;
                                try
                                {
                                    int num2 = int.Parse(Params[1]);
                                    if (Session.GetHabbo().Rank >= 6u)
                                    {
                                        class2.UsersMax = num2;
                                    }
                                    else if (num2 > 100 || num2 < 5)
                                    {
                                        Session.SendNotification("ERROR: Use a number between 5 and 100");
                                    }
                                    else
                                    {
                                        class2.UsersMax = num2;
                                    }
                                }
                                catch
                                {
                                    result = false;
                                    return result;
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            if (num == 53)
                            {
                                class2 = Session.GetHabbo().CurrentRoom;
                                HabboIM.GetGame().GetRoomManager().method_16(class2);
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        }
                    }
                    switch (HabboIM.GetGame().GetRoleManager().dictionary_4[Params[0]])
                    {
                        case 2:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_alert"))
                                {
                                    result = false;
                                    return result;
                                }
                                string TargetUser = Params[1];
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(TargetUser);
                                if (TargetClient == null)
                                {
                                    Session.GetHabbo().Whisper("Could not find user: " + TargetUser);
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                TargetClient.SendNotification(ChatCommandHandler.MergeParams(Params, 2), 0);
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        case 3:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_award"))
                                {
                                    result = false;
                                    return result;
                                }
                                string text = Params[1];
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null)
                                {
                                    Session.GetHabbo().Whisper("Could not find user: " + text);
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                HabboIM.GetGame().GetAchievementManager().addAchievement(TargetClient, Convert.ToUInt32(ChatCommandHandler.MergeParams(Params, 2)));
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }




                        case 900000:

                            if (Session.GetHabbo().AchievementScore < 100)
                            {
                                Session.GetHabbo().Whisper("Du benötigst 100 Erfahrungspunkte um am Lotto teilnehmen zu können!");
                                return true;
                            }
                            else
                            {


                                if(HabboIM.lotto == false)
                                {

                                    Session.GetHabbo().Whisper("Die Lotto Ziehung findet zu jeder vollen Stunde statt. Wir benachrichtigen dich, sobald es los geht.");
                                    return true;
                                }

                                if(Session.GetHabbo().Credits < 2500)
                                {
                                    Session.GetHabbo().Whisper("Du hast nicht genügend Taler, um deine Zahlen einsenden zu können!");
                                    return true;
                                
                                }

                                string lottozahl = Params[1];
                                if (Convert.ToInt32(lottozahl) < 0 || Convert.ToInt32(lottozahl) > 50) // Lottozahlen definieren
                                {
                                    Session.GetHabbo().Whisper("Der Command lautet :lotto [Zahl]. Die Zahl muss zwischen 1 und 50 sein.");
                                    return true;


                                }
                                if(Session.GetHabbo().mylottozahl != 999)
                                {
                                    Session.GetHabbo().Whisper("Du hast bereits die Zahl "+Session.GetHabbo().mylottozahl+" gezogen.");
                                    return true;

                                }
                                Session.GetHabbo().Whisper("Zahl " + lottozahl + " erfolgreich gezogen! In wenigen Minuten wird gezogen.");
                                Session.GetHabbo().mylottozahl = Convert.ToInt32(lottozahl);
                                Session.GetHabbo().Credits -= 2500; // Lotto Preis
                                Session.GetHabbo().UpdateCredits(true); //Datenbank updaten
                                HabboIM.lotto_einsatz += 7000;  // Einsatz steigt

                                if(Convert.ToInt32(lottozahl) == HabboIM.lottozahl && HabboIM.lottowinner == 0 )
                                {
                                    HabboIM.lottowinner = Session.GetHabbo().Id;
                                    
                                }
                                return true;
                            }

                        case 2070:
                            {
                                if (Session.GetHabbo().Rank < 6)
                                {

                                    Session.GetHabbo().Whisper("Du scheinst nicht die nötigen Rechte zu besitzen...");
                                    return true;
                                }

                                {

                                    TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                    if (TargetClient == null)
                                    {

                                        Session.GetHabbo().Whisper("" + Params[1] + " muss online sein!");
                                        return true;
                                    }
                                    if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
                                    {
                                        Session.GetHabbo().Whisper("Der Spieler " + Params[1] + " hat einen höheren Rang als du, weshalb das Jailen dieses Spielers nicht möglich ist!");
                                        return true;

                                    }
                                    if (Params[2] == null)
                                    {

                                        Session.GetHabbo().Whisper("Bitte gebe die Haftzeit in Minuten an (:jail Username Zeit Grund).");
                                        return true;
                                    }
                                    if (Convert.ToInt32(Params[2]) < 1)
                                    {

                                        Session.GetHabbo().Whisper("Die Haftzeit muss mindestens eine Minute betragen.");
                                        return true;
                                    }


                                    double newtime = Convert.ToDouble(Params[2]);

                                    TargetClient.GetHabbo().jail = 1;
                                    TargetClient.GetHabbo().jailtime = (newtime * 60);
                                    TargetClient.GetHabbo().UpdateJailTime(true);
                                    TargetClient.GetHabbo().UpdateJail(true);

                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    Session.GetHabbo().Whisper("Der Spieler " + TargetClient.GetHabbo().Username + " wurde inhaftiert!");
                                    if (ChatCommandHandler.MergeParams(Params, 3) != null)
                                    {

                                        TargetClient.SendNotification("Du wurdest aufgrund eines Regelverstoßes inhaftiert!\r\rDeine Haftstrafe endet in " + newtime + " Minuten.\r\rSolltest du dich ausloggen, wird die Zeit gestoppt und erst dann wieder fortgesetzt, wenn du dich einloggst.");

                                        ServerMessage Logging = new ServerMessage(134u);
                                        Logging.AppendUInt(0u);


                                        Logging.AppendString(string.Concat(new string[]
                                        {
                                                  "Frank: "+ Session.GetHabbo().Username +" hat "+ TargetClient.GetHabbo().Username+" für "+ newtime+" Minuten inhaftiert!"

                                        }));

                                        HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);





                                    }
                                    else
                                    {


                                        ServerMessage Logging = new ServerMessage(134u);
                                        Logging.AppendUInt(0u);


                                        Logging.AppendString(string.Concat(new string[]
                                        {
                                                  "System: "+ Session.GetHabbo().Username +" hat "+ TargetClient.GetHabbo().Username+ " für "+ newtime+" Minuten inhaftiert."

                                        }));

                                        HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);

                                        TargetClient.SendNotification("Du wurdest aus dem Hotel gebannt!\r\rDein Bann endet in " + newtime + " Minuten und du wirst automatisch aus dem Adminprison entbannt.\r\rZu unrecht bestraft?\rKontaktiere uns via Hilferuf!");
                                    }
                                    return true;
                                }
                            }


                        case 2080:
                            {

                                if (Session.GetHabbo().Rank < 6)
                                {

                                    Session.GetHabbo().Whisper("Du scheinst nicht die nötigen Rechte zu besitzen...");
                                    return true;
                                }

                                {

                                    TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                    if (TargetClient == null)
                                    {

                                        Session.GetHabbo().Whisper("" + Params[1] + " muss online sein!");
                                        return true;
                                    }
                                    if (TargetClient.GetHabbo().jail == 0 || TargetClient.GetHabbo().jailtime < 1)
                                    {

                                        Session.GetHabbo().Whisper("" + Params[1] + " sitzt derzeit nicht im Gefängnis!");
                                        return true;
                                    }

                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    TargetClient.GetHabbo().jail = 0;
                                    TargetClient.GetHabbo().jailtime = 0.0;
                                    TargetClient.GetHabbo().UpdateJailTime(true);
                                    TargetClient.GetHabbo().UpdateJail(true);

                                    Session.GetHabbo().Whisper("Der User " + TargetClient.GetHabbo().Username + " wurde erfolgreich aus der MyHuBBa Haftanstalt entlassen!");

                                    TargetClient.SendNotification("Frank, der MyHabbo Gefängniswärter hat dich frühzeitig aus der Haftanstalt entlassen. Grund: Gute Führung!");

                                    return true;
                                }
                            }




                        case 4:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_ban"))
                                {
                                    result = false;
                                    return result;
                                }
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.GetHabbo().Whisper("Der User existiert nicht oder ist nicht mehr online.");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }

                                if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
                                {
                                    Session.GetHabbo().Whisper("Du besitzt nicht die nötigen Rechte.");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                string banLengthWithoutParams = Params[2];
                                banLengthWithoutParams = banLengthWithoutParams.Replace("m", "");
                                banLengthWithoutParams = banLengthWithoutParams.Replace("s", "");
                                banLengthWithoutParams = banLengthWithoutParams.Replace("t", "");
                                int banLength = 0;
                                try
                                {
                                    banLength = int.Parse(banLengthWithoutParams);
                                }
                                catch (FormatException ex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("OOPS! Something went wrong when trying format ban length! Report this and your date format!" + ex.ToString());
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                }
                                if (Params[2].Contains("m"))
                                {
                                    banLength *= 60;
                                }
                                if (Params[2].Contains("s"))
                                {
                                    banLength *= 3600;
                                }
                                if (Params[2].Contains("t"))
                                {
                                    banLength *= 86400;
                                }
                                int laskettupaivia = 0;
                                int laskettutunteja = 0;
                                int laskettuminuutteja = 0;
                                int laskettavaa = banLength;
                                int i = 0;
                                while (laskettavaa >= 86400)
                                {
                                    laskettupaivia++;
                                    laskettavaa -= 86400;
                                    i++;
                                }
                                i = 0;
                                while (laskettavaa >= 3600)
                                {
                                    laskettutunteja++;
                                    laskettavaa -= 3600;
                                    i++;
                                }
                                i = 0;
                                while (laskettavaa >= 60)
                                {
                                    laskettuminuutteja++;
                                    laskettavaa -= 60;
                                    i++;
                                }
                                if (banLength < 600)
                                {
                                    Session.SendNotification("Wenn du einen Spieler temporär sperren möchtest, musst du die Zeit in Sekunden angeben. Das Minimum hierbei beträgt 600 (10 Minuten).");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                Session.SendNotification(string.Concat(new object[]
                        {
                            "Du hast folgenden Spieler gebannt: ",
                            TargetClient.GetHabbo().Username,
                            " Bandauer: ",
                            laskettupaivia,
                            " Tag(e) ",
                            laskettutunteja,
                            "  Stunde(n) ",
                            laskettuminuutteja,
                            " Minute(n) ",
                            laskettavaa,
                            " second"
                        }));
                                HabboIM.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, (double)banLength, ChatCommandHandler.MergeParams(Params, 3), false, false);
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }






                        case 2058:
                            {

                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    return false;

                                }
                                RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                if (class3 == null)
                                {
                                    return false;
                                }
                                Room room = Session.GetHabbo().CurrentRoom; // Current Room
                                if (room.Owner != Session.GetHabbo().Username && !Session.GetHabbo().HasFuse("acc_anyroomowner"))
                                {

                                    Session.GetHabbo().Whisper("Dieser Befehl kann nur vom Raumbesitzer ausgeführt werden!");
                                    return true;
                                }
                                var TargetClient1 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[2]);
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null || TargetClient1 == null)
                                {
                                    Session.GetHabbo().Whisper("Die User müssen online sein!");
                                    return true;
                                }

                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient.GetHabbo().CurrentRoomId != Session.GetHabbo().CurrentRoomId || TargetClient1.GetHabbo().CurrentRoomId != Session.GetHabbo().CurrentRoomId)
                                {
                                    Session.GetHabbo().Whisper("Die User müssen in diesem Raum sein!");
                                    return true;
                                }
                                var class23 = HabboIM.GetGame().GetRoomManager().GetRoom(TargetClient.GetHabbo().CurrentRoomId);
                                RoomUser class38 = class23.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                var class233 = HabboIM.GetGame().GetRoomManager().GetRoom(TargetClient1.GetHabbo().CurrentRoomId);
                                RoomUser class383 = class23.GetRoomUserByHabbo(TargetClient1.GetHabbo().Id);

                                int oldy = class38.Y;
                                int oldx = class38.X;
                                double olddouble = class38.double_0;
                                int oldrot = class38.BodyRotation;

                                class38.Y = class383.Y;

                                class38.X = class383.X;

                                class38.double_0 = class383.double_0;

                                class38.BodyRotation = class383.BodyRotation;


                                class383.Y = oldy;

                                class383.X = oldx;

                                class383.double_0 = olddouble;

                                class383.BodyRotation = oldrot;

                                class383.UpdateNeeded = true;

                                class38.UpdateNeeded = true;





                                //class2.method_22();
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }

                        case 2068:
                            {

                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    return false;

                                }
                                RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                if (class3 == null)
                                {
                                    return false;
                                }
                                Room room = Session.GetHabbo().CurrentRoom; // Current Room
                                if (room.Owner != Session.GetHabbo().Username && !Session.GetHabbo().HasFuse("acc_anyroomowner"))
                                {
                                    Session.GetHabbo().Whisper("Dieser Befehl kann nur vom Raumbesitzer ausgeführt werden!");
                                    return true;
                                }

                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.GetHabbo().Whisper("Der User muss online sein!");
                                    return true;
                                }

                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient.GetHabbo().CurrentRoomId != Session.GetHabbo().CurrentRoomId)
                                {
                                    Session.GetHabbo().Whisper("Der User muss in diesem Raum sein!");
                                    return true;
                                }
                                var class23 = HabboIM.GetGame().GetRoomManager().GetRoom(TargetClient.GetHabbo().CurrentRoomId);
                                RoomUser class38 = class23.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);

                                class38.Y = class3.Y;

                                class38.X = class3.X;

                                class38.double_0 = class3.double_0;

                                class38.BodyRotation = class3.BodyRotation;

                                class38.UpdateNeeded = true;




                                //class2.method_22();
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }

                        case 2048:
                            {

                                if (!Session.GetHabbo().HasFuse("cmd_superban"))
                                {

                                    result = false;
                                    return result;
                                }


                                string text5 = Params[1];
                                bool flag2 = true;
                                if (string.IsNullOrEmpty(text5))
                                {
                                    Session.GetHabbo().Whisper("Bitte ein Username eingeben.");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                GameClient class7 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text5);
                                Habbo class8;
                                if (class7 == null)
                                {

                                    flag2 = false;
                                    class8 = Authenticator.CreateHabbo(text5);


                                }
                                else
                                {

                                    class8 = class7.GetHabbo();
                                }

                                if (class8.Rank >= Session.GetHabbo().Rank)
                                {
                                    Session.SendNotification("Du besitzt nicht die nötigen Rechte.");

                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }



                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                if (flag2 == true)
                                {

                                    if (ChatCommandHandler.MergeParams(Params, 2) != "")
                                    {
                                        
                                        TargetClient.GetHabbo().jail = 1;
                                        TargetClient.GetHabbo().jailtime = 360000000.0;
                                        TargetClient.GetHabbo().UpdateJailTime(true);
                                        TargetClient.GetHabbo().UpdateJail(true);

                                        ServerMessage Message = new ServerMessage(35u);
                                        Message.AppendStringWithBreak("Du wurdest aufgrund eines Regelverstoßes dauerhaft aus dem Hotel gebannt!\r\r", 13);
                                        Message.AppendStringWithBreak("Grund: Dauerhafte Accountsperrung");
                                        TargetClient.SendMessage(Message);
                                        ServerMessage Logging = new ServerMessage(134u);
                                        Logging.AppendUInt(0u);


                                        Logging.AppendString(string.Concat(new string[]
                                        {
                                                  "BanManager: "+ Session.GetHabbo().Username +" hat "+ TargetClient.GetHabbo().Username+ " dauerhaft aus dem Hotel gebannt! (Grund: "+ ChatCommandHandler.MergeParams(Params, 2) +")"

                                        }));

                                        HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);


                                        //HabboIM.GetGame().GetBanManager().BanUser(class7, Session.GetHabbo().Username, 360000000.0, ChatCommandHandler.MergeParams(Params, 2), false, false);

                                    }
                                    else
                                    {
                                        ServerMessage Logging = new ServerMessage(134u);
                                        Logging.AppendUInt(0u);


                                        Logging.AppendString(string.Concat(new string[]
                                        {
                                                  "BanManager: "+ Session.GetHabbo().Username +" hat "+ class7.GetHabbo().Username+ " dauerhaft aus dem Hotel gebannt!"

                                        }));

                                        HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);


                                        TargetClient.GetHabbo().jail = 1;
                                        TargetClient.GetHabbo().jailtime = 360000000.0;
                                        TargetClient.GetHabbo().UpdateJailTime(true);
                                        TargetClient.GetHabbo().UpdateJail(true);

                                        ServerMessage Message = new ServerMessage(35u);
                                        Message.AppendStringWithBreak("Du wurdest dauerhaft aus dem Hotel gebannt!\r\rBegründung: Dauerban", 13);

                                        Session.SendMessage(Message);


                                        //HabboIM.GetGame().GetBanManager().BanUser(class7, Session.GetHabbo().Username, 360000000.0, "Dauerban", false, false);

                                    }



                                    Session.SendNotification("Du hast " + class7.GetHabbo().Username + " erfolreich gebannt.");



                                }
                                else
                                {



                                    var reason = "";

                                    if (ChatCommandHandler.MergeParams(Params, 2) != "")
                                    {

                                        reason = ChatCommandHandler.MergeParams(Params, 2);
                                    }
                                    else
                                    {

                                        reason = "Dauerban";
                                    }
                                    double timestamp = HabboIM.GetUnixTimestamp() + 360000000.0;




                                    try
                                    {


                                        /*  


                                     using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                    {
                                    dbClient.AddParamWithValue("rawvar", "user");
                                             dbClient.AddParamWithValue("var", class8.Username);
                                             dbClient.AddParamWithValue("reason", reason);
                                             dbClient.AddParamWithValue("mod", Session.GetHabbo().Username);

                                             dbClient.ExecuteQuery(string.Concat(new object[]
                                             {
                         "INSERT INTO bans (bantype,value,reason,expire,added_by,added_date,appeal_state) VALUES (@rawvar,@var,@reason,'",
                         timestamp,
                         "',@mod,'",
                         DateTime.Now.ToLongDateString(),
                         "', '1')"
                                             }));


                                         }


                                           }
                                catch
                                {

                                    Session.SendNotification("Error in SQL-Abfrage.");
                                }

 */


                                        using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                        {

                                            dbClient.ExecuteQuery(string.Concat(new object[]
                           {
                        "UPDATE users SET jailtime = '360000000' , jail = '1' WHERE Id = '",
                       class8.Id,
                        "' LIMIT 1;"
                           }));


                                        }

                                    }
                                    catch
                                    {

                                        Session.SendNotification("Error in SQL-Abfrage.");
                                    }






                                    using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
                                    {
                                        @class.ExecuteQuery("UPDATE user_info SET bans = bans + 1 WHERE user_id = '" + class8.Id + "' LIMIT 1");
                                    }

                                    Session.SendNotification("Du hast " + class8.Username + " erfolreich gebannt.");



                                }


                                try
                                {
                                    using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                    {
                                        HabboIM.GetGame().GetBanManager().Initialise(dbClient);
                                    }
                                    HabboIM.GetGame().GetClientManager().method_28();
                                }
                                catch
                                {
                                    Session.SendNotification("Da lief etwas schief.");

                                }



                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);


                                result = true;
                                return result;

                            }



                        case 4999:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_ban"))
                                {
                                    result = false;
                                    return result;
                                }
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.GetHabbo().Whisper("Der User existiert nicht oder ist nicht mehr online.");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
                                {
                                    Session.GetHabbo().Whisper("Du besitzt nicht die nötigen Rechte.");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                string banLengthWithoutParams = Params[2];
                                banLengthWithoutParams = banLengthWithoutParams.Replace("m", "");
                                banLengthWithoutParams = banLengthWithoutParams.Replace("s", "");
                                banLengthWithoutParams = banLengthWithoutParams.Replace("t", "");
                                int banLength = 0;
                                try
                                {
                                    banLength = int.Parse(banLengthWithoutParams);
                                }
                                catch (FormatException ex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("OOPS! Something went wrong when trying format ban length! Report this and your date format!" + ex.ToString());
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                }
                                if (Params[2].Contains("m"))
                                {
                                    banLength *= 60;
                                }
                                if (Params[2].Contains("s"))
                                {
                                    banLength *= 3600;
                                }
                                if (Params[2].Contains("t"))
                                {
                                    banLength *= 86400;
                                }
                                int laskettupaivia = 0;
                                int laskettutunteja = 0;
                                int laskettuminuutteja = 0;
                                int laskettavaa = banLength;
                                int i = 0;
                                while (laskettavaa >= 86400)
                                {
                                    laskettupaivia++;
                                    laskettavaa -= 86400;
                                    i++;
                                }
                                i = 0;
                                while (laskettavaa >= 3600)
                                {
                                    laskettutunteja++;
                                    laskettavaa -= 3600;
                                    i++;
                                }
                                i = 0;
                                while (laskettavaa >= 60)
                                {
                                    laskettuminuutteja++;
                                    laskettavaa -= 60;
                                    i++;
                                }
                                if (banLength < 600)
                                {
                                    Session.SendNotification("Wenn du einen Spieler temporär sperren möchtest, musst du die Zeit in Sekunden angeben. Das Minimum hierbei beträgt 600 (10 Minuten).");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                Session.SendNotification(string.Concat(new object[]
                        {
                            "Du hast folgenden Spieler gebannt: ",
                            TargetClient.GetHabbo().Username,
                            " Bandauer: ",
                            laskettupaivia,
                            " Tag(e) ",
                            laskettutunteja,
                            "  Stunde(n) ",
                            laskettuminuutteja,
                            " Minute(n) ",
                            laskettavaa,
                            " second"
                        }));
                                HabboIM.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, (double)banLength, ChatCommandHandler.MergeParams(Params, 3), false, true);
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        case 6:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_coins"))
                                {
                                    result = false;
                                    return result;
                                }
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Der Spieler konnte nicht gefunden werden.");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                int num3;
                                if (int.TryParse(Params[2], out num3))
                                {
                                    Console.WriteLine(string.Concat(new object[]
                            {
                                "Staff vergibt Taler: ",
                                num3,
                                " (ID: ",
                                Session.GetHabbo().Id,
                                ")"
                            }));
                                    if (num3 <= 1000000 && num3 >= -1000000)
                                    {
                                        TargetClient.GetHabbo().Credits = TargetClient.GetHabbo().Credits + num3;
                                        TargetClient.GetHabbo().UpdateCredits(true);
                                        TargetClient.SendNotification(Session.GetHabbo().Username + " hat dir " + num3.ToString() + " Taler gegeben!");
                                        Session.SendNotification("Taler erfolgreich geupdatet.");
                                        HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    }
                                    else
                                    {
                                        Session.SendNotification("Taler dürfen nur maximal 1.000.00 gegeben bzw. abgezogen werden!");
                                    }
                                    result = true;
                                    return result;
                                }
                                Session.SendNotification("Benutze bitte eine gültige Anzahl an Talern.");
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        case 7:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_coords"))
                                {
                                    result = false;
                                    return result;
                                }
                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    result = false;
                                    return result;
                                }
                                RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                if (class3 == null)
                                {
                                    result = false;
                                    return result;
                                }
                                Session.SendNotification(string.Concat(new object[]
                        {
                            "X: ",
                            class3.X,
                            " - Y: ",
                            class3.Y,
                            " - Z: ",
                            class3.double_0,
                            " - Rot: ",
                            class3.BodyRotation,
                            ", sqState: ",
                            class2.Byte_0[class3.X, class3.Y].ToString()
                        }));
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        case 11:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_enable"))
                                {
                                    result = false;
                                    return result;
                                }
                                int effectid = (int)Convert.ToInt16(Params[1]);
                                if (effectid == 10200000/* || effectid == 178 || effectid == 650 || effectid == 653 || effectid == 997 || effectid == 998 || effectid == 999 || effectid == 620 || (effectid >= 500 && effectid <= 530)*/)
                                {
                                    Session.GetHabbo().Whisper("Dieser Effekt wurde für normale User gesperrt!");
                                    result = true;
                                    return result;
                                }
                                int int_ = int.Parse(Params[1]);
                                Session.GetHabbo().GetEffectsInventoryComponent().method_2(int_, true);
                                result = true;
                                return result;
                            }
                        case 14:
                            if (Session.GetHabbo().Rank > 6 || Session.GetHabbo().CurrentRoom.Owner == Session.GetHabbo().Username)
                            {
                                RoomUser class4 = Session.GetHabbo().CurrentRoom.method_56(Params[1]);
                                if (class4 != null)
                                {
                                    class4.bool_5 = !class4.bool_5;
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 15:
                            if (Session.GetHabbo().HasFuse("cmd_givebadge"))
                            {
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient != null)
                                {
                                    TargetClient.GetHabbo().GetBadgeComponent().SendBadge(TargetClient, HabboIM.FilterString(Params[2]), true);
                                }
                                else
                                {
                                    Session.SendNotification("Der Spieler: " + Params[1] + " konnte nicht in der Datenbank gefunden werden.\rBitte versuche es erneut.");
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 16:
                            if (Session.GetHabbo().HasFuse("cmd_globalcredits"))
                            {
                                try
                                {
                                    int num4 = int.Parse(Params[1]);
                                    HabboIM.GetGame().GetClientManager().method_18(num4);
                                    using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                    {
                                        dbClient.ExecuteQuery("UPDATE users SET credits = credits + " + num4, 30);
                                    }
                                }
                                catch
                                {
                                    Session.SendNotification("Du musst eine Zahl eingeben!");
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 17:
                            if (Session.GetHabbo().HasFuse("cmd_globalpixels"))
                            {
                                try
                                {
                                    int num4 = int.Parse(Params[1]);
                                    HabboIM.GetGame().GetClientManager().method_19(num4, false);
                                    using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                    {
                                        dbClient.ExecuteQuery("UPDATE users SET activity_points = activity_points + " + num4, 30);
                                    }
                                }
                                catch
                                {
                                    Session.SendNotification("Du musst eine Zahl eingeben!");
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 18:
                            if (Session.GetHabbo().HasFuse("cmd_globalpoints"))
                            {
                                try
                                {
                                    int num4 = int.Parse(Params[1]);
                                    HabboIM.GetGame().GetClientManager().method_20(num4, false);
                                    using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                    {
                                        dbClient.ExecuteQuery("UPDATE users SET vip_points = vip_points + " + num4, 30);
                                    }
                                }
                                catch
                                {
                                    Session.SendNotification("Du musst eine Zahl eingeben!");
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 19:
                            if (Session.GetHabbo().HasFuse("cmd_hal"))
                            {
                                string text2 = Params[1];
                                Input = Input.Substring(4).Replace(text2, "");
                                string text3 = Input.Substring(1);
                                ServerMessage Message = new ServerMessage(161u);
                                Message.AppendStringWithBreak(string.Concat(new string[]
                            {
                                HabboIMEnvironment.GetExternalText("cmd_hal_title"),
                                "\r\n",
                                text3,
                                "\r\n-",
                                Session.GetHabbo().Username
                            }));
                                Message.AppendStringWithBreak(text2);
                                HabboIM.GetGame().GetClientManager().BroadcastMessage(Message);
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 20:
                            if (Session.GetHabbo().HasFuse("cmd_ha"))
                            {
                                string str = Input.Substring(3);
                                ServerMessage Message2 = new ServerMessage(808u);
                                Message2.AppendStringWithBreak(HabboIMEnvironment.GetExternalText("cmd_ha_title"));
                                Message2.AppendStringWithBreak(str + "\r\n- " + Session.GetHabbo().Username);
                                ServerMessage Message3 = new ServerMessage(161u);
                                Message3.AppendStringWithBreak(str + "\r\n- " + Session.GetHabbo().Username);
                                HabboIM.GetGame().GetClientManager().method_15(Message2, Message3);
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 21:
                            if (Session.GetHabbo().HasFuse("cmd_invisible"))
                            {
                                Session.GetHabbo().IsVisible = !Session.GetHabbo().IsVisible;
                                Session.SendNotification("Du bist nun " + (Session.GetHabbo().IsVisible ? "sichtbar" : "unsichtbar") + "\nLade den Raum neu um deine Änderung wirksam zu machen ;D");
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 22:
                            if (!Session.GetHabbo().HasFuse("cmd_ipban"))
                            {
                                result = false;
                                return result;
                            }
                            TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                Session.GetHabbo().Whisper("Der Spieler konnte nicht gefunden werden.");
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
                            {
                                Session.GetHabbo().Whisper("Du besitzt nicht die nötigen Rechte.");
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            HabboIM.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, 360000000.0, ChatCommandHandler.MergeParams(Params, 2), true, false);
                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                            result = true;
                            return result;
                        case 23:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_kick"))
                                {
                                    result = false;
                                    return result;
                                }
                                string text = Params[1];
                                string reason = Params[2];
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null)
                                {
                                    Session.GetHabbo().Whisper("User konnte nicht gefunden werden: " + text);
                                    result = true;
                                    return result;
                                }
                                if (Session.GetHabbo().Rank < 6u && string.IsNullOrEmpty(reason))
                                {
                                    Session.GetHabbo().Whisper("Du musst einen Grund angeben!");
                                    result = true;
                                    return result;
                                }
                                if (Session.GetHabbo().Rank <= TargetClient.GetHabbo().Rank)
                                {
                                    Session.SendNotification("Du besitzt nicht die nötigen Rechte.");
                                    result = true;
                                    return result;
                                }
                                if (TargetClient.GetHabbo().CurrentRoomId < 1u)
                                {
                                    Session.GetHabbo().Whisper("Der User ist nicht im Raum und kann deshalb nicht gekickt werden!");
                                    result = true;
                                    return result;
                                }
                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(TargetClient.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                class2.method_47(TargetClient, true, false);
                                if (Params.Length > 2)
                                {
                                    TargetClient.SendNotification("Du wurdest mit dem Grund: \"" + ChatCommandHandler.MergeParams(Params, 2) + "\"  aus dem Raum gekickt!");
                                }
                                else
                                {
                                    TargetClient.SendNotification("Du wurdest aus dem Raum gekickt!");
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        case 24:
                            if (Session.GetHabbo().HasFuse("cmd_massbadge"))
                            {
                                HabboIM.GetGame().GetClientManager().method_21(Params[1]);
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 25:
                            if (Session.GetHabbo().HasFuse("cmd_masscredits"))
                            {
                                try
                                {
                                    int num4 = int.Parse(Params[1]);
                                    HabboIM.GetGame().GetClientManager().method_18(num4);
                                }
                                catch
                                {
                                    Session.GetHabbo().Whisper("Du musst eine Zahl angeben!");
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 26:
                            if (Session.GetHabbo().HasFuse("cmd_masspixels"))
                            {
                                try
                                {
                                    int num4 = int.Parse(Params[1]);
                                    HabboIM.GetGame().GetClientManager().method_19(num4, true);
                                }
                                catch
                                {
                                    Session.GetHabbo().Whisper("Du musst eine Zahl angeben!");
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 27:
                            if (Session.GetHabbo().HasFuse("cmd_masspoints"))
                            {
                                try
                                {
                                    int num4 = int.Parse(Params[1]);
                                    HabboIM.GetGame().GetClientManager().method_20(num4, true);
                                }
                                catch
                                {
                                    Session.GetHabbo().Whisper("Du musst eine Zahl angeben");
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 30:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_motd"))
                                {
                                    result = false;
                                    return result;
                                }
                                string text = Params[1];
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null)
                                {
                                    Session.GetHabbo().Whisper("User konnte nicht gefunden werden: " + text);
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                TargetClient.SendNotification(ChatCommandHandler.MergeParams(Params, 2), 2);
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        case 31:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_mute"))
                                {
                                    result = false;
                                    return result;
                                }
                                string text = Params[1];
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null || TargetClient.GetHabbo() == null)
                                {
                                    Session.GetHabbo().Whisper("User konnte nicht gefunden werden: " + text);
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
                                {
                                    Session.GetHabbo().Whisper("Du besitzt nicht die nötigen Rechte.");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                TargetClient.GetHabbo().Mute();
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        case 32:
                            {
                                if (Session.GetHabbo().Rank < 6 && Session.GetHabbo().CurrentRoom.Owner != Session.GetHabbo().Username)
                                {
                                    return false;
                                }
                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    result = false;
                                    return result;
                                }
                                RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                if (class3 == null)
                                {
                                    result = false;
                                    return result;
                                }
                                if (class3.bool_1)
                                {
                                    class3.bool_1 = false;
                                    Session.GetHabbo().Whisper("Du hast Override deaktiviert.");
                                }
                                else
                                {
                                    class3.bool_1 = true;
                                    Session.GetHabbo().Whisper("Du hast Override aktiviert.");
                                }
                                class2.method_22();
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        case 34:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_pixels"))
                                {
                                    result = false;
                                    return result;
                                }
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.GetHabbo().Whisper("User wurde nicht gefunden!");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                int num3;
                                if (int.TryParse(Params[2], out num3))
                                {
                                    Console.WriteLine(string.Concat(new object[]
                            {
                                "Staff vergibt Enten: ",
                                num3,
                                " (ID: ",
                                Session.GetHabbo().Id,
                                ")"
                            }));
                                    if (num3 <= 50 && num3 >= -50)
                                    {
                                        TargetClient.GetHabbo().ActivityPoints = TargetClient.GetHabbo().ActivityPoints + num3;
                                        TargetClient.GetHabbo().UpdateActivityPoints(true);
                                        TargetClient.SendNotification(Session.GetHabbo().Username + " hat dir " + num3.ToString() + " Enten gutgeschrieben.");
                                        Session.GetHabbo().Whisper("Enten wurden erfolgreich versendet!");
                                        HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    }
                                    else
                                    {
                                        Session.SendNotification("Du kannst maximal 50 Enten versenden/abziehen!");
                                    }
                                    result = true;
                                    return result;
                                }
                                Session.SendNotification("Gebe eine gültige Anzahl an Enten ein.");
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        case 35:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_points"))
                                {
                                    result = false;
                                    return result;
                                }
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("User could not be found.");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                int num3;
                                if (int.TryParse(Params[2], out num3))
                                {
                                    Console.WriteLine(string.Concat(new object[]
                            {
                                "Staff vergibt Sterne: ",
                                num3,
                                " (ID: ",
                                Session.GetHabbo().Id,
                                ")"
                            }));
                                    if (num3 <= 10 && num3 >= -10)
                                    {
                                        TargetClient.GetHabbo().VipPoints = TargetClient.GetHabbo().VipPoints + num3;
                                        TargetClient.GetHabbo().UpdateVipPoints(false, true);
                                        TargetClient.SendNotification(Session.GetHabbo().Username + " hat dir " + num3.ToString() + " Sterne gegeben!");
                                        Session.GetHabbo().Whisper("Sterne wurden erfolgreich versendet!");
                                        HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    }
                                    else
                                    {
                                        Session.SendNotification("Du darfst max. 10 Sterne vergeben/abziehen!");
                                    }
                                    result = true;
                                    return result;
                                }
                                Session.SendNotification("Bitte ein Anzahl von Sternen eingeben.");
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        case 39:
                            if (Session.GetHabbo().HasFuse("cmd_removebadge"))
                            {
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient != null)
                                {
                                    TargetClient.GetHabbo().GetBadgeComponent().RemoveBadge(HabboIM.FilterString(Params[2]));
                                }
                                else
                                {
                                    Session.SendNotification("User: " + Params[1] + " konnte nicht gefunden werden.\rBitte versuchen Sie erneut.");
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;



                        case 41:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_roomalert"))
                                {
                                    result = false;
                                    return result;
                                }
                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    result = false;
                                    return result;
                                }
                                string string_ = ChatCommandHandler.MergeParams(Params, 1);
                                for (int i = 0; i < class2.RoomUsers.Length; i++)
                                {
                                    RoomUser class5 = class2.RoomUsers[i];
                                    if (class5 != null)
                                    {
                                        class5.GetClient().SendNotification(string_);
                                    }
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }


                        case 42:
                            if (!Session.GetHabbo().HasFuse("cmd_roombadge"))
                            {
                                result = false;
                                return result;
                            }
                            if (Session.GetHabbo().CurrentRoom == null)
                            {
                                result = false;
                                return result;
                            }
                            for (int i = 0; i < Session.GetHabbo().CurrentRoom.RoomUsers.Length; i++)
                            {
                                try
                                {
                                    RoomUser class5 = Session.GetHabbo().CurrentRoom.RoomUsers[i];
                                    if (class5 != null)
                                    {
                                        if (!class5.IsBot)
                                        {
                                            if (class5.GetClient() != null)
                                            {
                                                if (class5.GetClient().GetHabbo() != null)
                                                {
                                                    class5.GetClient().GetHabbo().GetBadgeComponent().SendBadge(class5.GetClient(), Params[1], true);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex2)
                                {
                                    Session.SendNotification("Error: " + ex2.ToString());
                                }
                            }
                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                            result = true;
                            return result;
                        case 43:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_roomkick"))
                                {
                                    result = false;
                                    return result;
                                }
                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    result = false;
                                    return result;
                                }
                                bool flag = true;
                                string text4 = ChatCommandHandler.MergeParams(Params, 1);
                                if (text4.Length > 0)
                                {
                                    flag = false;
                                }
                                for (int i = 0; i < class2.RoomUsers.Length; i++)
                                {
                                    RoomUser class6 = class2.RoomUsers[i];
                                    if (class6 != null && class6.GetClient().GetHabbo().Rank < Session.GetHabbo().Rank)
                                    {
                                        if (!flag)
                                        {
                                            class6.GetClient().SendNotification("Du wurdest von ein Moderator gekickt: " + text4);
                                        }
                                        class2.method_47(class6.GetClient(), true, flag);
                                    }
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        case 44:
                            if (Session.GetHabbo().HasFuse("cmd_roommute"))
                            {
                                if (Session.GetHabbo().CurrentRoom.bool_4)
                                {
                                    Session.GetHabbo().CurrentRoom.bool_4 = false;
                                }
                                else
                                {
                                    Session.GetHabbo().CurrentRoom.bool_4 = true;
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 45:
                            if (Session.GetHabbo().HasFuse("cmd_sa"))
                            {
                                ServerMessage Logging = new ServerMessage(134u);
                                Logging.AppendUInt(0u);
                                Logging.AppendString(Session.GetHabbo().Username + ": " + Input.Substring(3));
                                HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 47:
                            if (Session.GetHabbo().Rank > 7 || Session.GetHabbo().CurrentRoom.Owner == Session.GetHabbo().Username)
                            {
                                int.Parse(Params[1]);
                                Session.GetHabbo().CurrentRoom.method_102(int.Parse(Params[1]));
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 48:
                            if (Session.GetHabbo().HasFuse("cmd_shutdown"))
                            {
                                Logging.LogCriticalException("Der Spieler " + Session.GetHabbo().Username + " hat den Emulator runtergefahren. " + DateTime.Now.ToString());
                                Task task = new Task(new Action(HabboIM.Close));
                                task.Start();
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 49:
                            if (Session.GetHabbo().Rank > 6 || Session.GetHabbo().CurrentRoom.Owner == Session.GetHabbo().Username)
                            {
                                try
                                {
                                    string a = "down";
                                    string text = Params[1];
                                    TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text);
                                    class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    if (Session == null || TargetClient == null)
                                    {
                                        result = false;
                                        return result;
                                    }
                                    RoomUser class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                    if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                    {
                                        Session.GetHabbo().Whisper("You cannot pull yourself");
                                        result = true;
                                        return result;
                                    }
                                    class5.HandleSpeech(Session, "*pulls " + TargetClient.GetHabbo().Username + " to them*", false);
                                    if (class5.BodyRotation == 0)
                                    {
                                        a = "up";
                                    }
                                    if (class5.BodyRotation == 2)
                                    {
                                        a = "right";
                                    }
                                    if (class5.BodyRotation == 4)
                                    {
                                        a = "down";
                                    }
                                    if (class5.BodyRotation == 6)
                                    {
                                        a = "left";
                                    }
                                    if (a == "up")
                                    {
                                        if (ServerConfiguration.PreventDoorPush)
                                        {
                                            if (class5.X != class2.RoomModel.int_0 || class5.Y - 1 != class2.RoomModel.int_1 || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                            {
                                                class4.MoveTo(class5.X, class5.Y - 1);
                                            }
                                            else
                                            {
                                                class4.MoveTo(class5.X, class5.Y + 1);
                                            }
                                        }
                                        else
                                        {
                                            class4.MoveTo(class5.X, class5.Y - 1);
                                        }
                                    }
                                    if (a == "right")
                                    {
                                        if (ServerConfiguration.PreventDoorPush)
                                        {
                                            if (class5.X + 1 != class2.RoomModel.int_0 || class5.Y != class2.RoomModel.int_1 || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                            {
                                                class4.MoveTo(class5.X + 1, class5.Y);
                                            }
                                            else
                                            {
                                                class4.MoveTo(class5.X - 1, class5.Y);
                                            }
                                        }
                                        else
                                        {
                                            class4.MoveTo(class5.X + 1, class5.Y);
                                        }
                                    }
                                    if (a == "down")
                                    {
                                        if (ServerConfiguration.PreventDoorPush)
                                        {
                                            if (class5.X != class2.RoomModel.int_0 || class5.Y + 1 != class2.RoomModel.int_1 || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                            {
                                                class4.MoveTo(class5.X, class5.Y + 1);
                                            }
                                            else
                                            {
                                                class4.MoveTo(class5.X, class5.Y - 1);
                                            }
                                        }
                                        else
                                        {
                                            class4.MoveTo(class5.X, class5.Y + 1);
                                        }
                                    }
                                    if (a == "left")
                                    {
                                        if (ServerConfiguration.PreventDoorPush)
                                        {
                                            if (class5.X - 1 != class2.RoomModel.int_0 || class5.Y != class2.RoomModel.int_1 || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                            {
                                                class4.MoveTo(class5.X - 1, class5.Y);
                                            }
                                            else
                                            {
                                                class4.MoveTo(class5.X + 1, class5.Y);
                                            }
                                        }
                                        else
                                        {
                                            class4.MoveTo(class5.X - 1, class5.Y);
                                        }
                                    }
                                    result = true;
                                    return result;
                                }
                                catch
                                {
                                    result = false;
                                    return result;
                                }
                            }
                            result = false;
                            return result;




                        case 878683:    //MUTEGC
                            {


                                if (Session.GetHabbo().gchat == 0)
                                {


                                    Session.GetHabbo().Whisper("Du hast den globalen Chat bereits gemutet.");
                                    return true;

                                }


                                Session.GetHabbo().gchat = 0;


                                Session.GetHabbo().Whisper("Du hast globale Benachrichtigungen stumm geschaltet.");





                                return true;
                            }

                        

                        case 88179938: //UNMUTE GC
                            {

                                if (Session.GetHabbo().gchat == 1)
                                {


                                    Session.GetHabbo().Whisper("Du erhältst bereits globale Benachrichtigungen.");
                                    return true;

                                }


                                Session.GetHabbo().gchat = 1;


                                Session.GetHabbo().Whisper("Du erhältst nun wieder globale Benachrichtigungen.");






                                return true;



                            }


                        case 4734867: //GC anschalten
                            {


                                if (Session.GetHabbo().Rank < 10)
                                {

                                    return false;
                                }



                                if (HabboIM.GetGame().GetClientManager().gc == true)
                                {
                                    Session.SendNotification("Der Globale Chat ist bereits aktiv.");
                                    return true;
                                }

                                HabboIM.GetGame().GetClientManager().gc = true;


                                HabboIM.GetGame().GetClientManager().method_WHISPER("" + Session.GetHabbo().Username + " hat den globalen Chat aktiviert. (Benutze :gc <nachricht> oder :mutegc)");
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);


                                return true;
                            }


                        case 4734868: //GC ausschalten
                            {


                                if (Session.GetHabbo().Rank < 10)
                                {

                                    return false;
                                }



                                if (HabboIM.GetGame().GetClientManager().gc == false)
                                {
                                    Session.SendNotification("Der Globale Chat ist bereits inaktiv.");
                                    return true;
                                }

                                HabboIM.GetGame().GetClientManager().gc = false;
                                HabboIM.GetGame().GetClientManager().method_WHISPER("Der globale Chat wurde von " + Session.GetHabbo().Username + " deaktiviert.");
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);

                                return true;
                            }



                        case 83784: //DELGC
                            {


                                if (Session.GetHabbo().Rank < 10)
                                {

                                    return false;
                                }



                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);


                                if (TargetClient.GetHabbo() == null)
                                {

                                    Session.GetHabbo().Whisper("Der User wurde nicht gefunden.");
                                    return true;
                                }
                                if (TargetClient.GetHabbo().gc == 0)
                                {

                                    Session.GetHabbo().Whisper("Dem User wurden diese Rechte bereits entzogen.");
                                    return true;
                                }


                                TargetClient.GetHabbo().gc = 0;
                                Session.GetHabbo().Whisper("Dem User " + TargetClient.GetHabbo().Username + " wurden die Rechte für den Globalen Chat entfernt.");

                                TargetClient.GetHabbo().Whisper("Dir wurden folgende Rechte entzogen: Globalchat");
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }


                        case 88998874: //OFFBADGE DU NUUB
                            {


                                if (Session.GetHabbo().Rank < 8) return false;





                                string text5 = Params[1];
                                bool flag2 = true;
                                if (string.IsNullOrEmpty(text5))
                                {
                                    Session.GetHabbo().Whisper("Bitte ein Username eingeben.");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                GameClient class7 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text5);
                                Habbo class8;
                                if (class7 == null)
                                {
                                    flag2 = false;
                                    class8 = Authenticator.CreateHabbo(text5);
                                }
                                else
                                {
                                    class8 = class7.GetHabbo();
                                }
                                if (class8 == null)
                                {
                                    Session.GetHabbo().Whisper("User konnte nicht gefunden werden: " + Params[1]);
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }



                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {

                                    dbClient.AddParamWithValue("badge", Params[2]);

                                    dbClient.ExecuteQuery(string.Concat(new object[]
                                    {
                        "INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES ('",
                        class8.Id,
                        "',@badge,'0')"
                                    }));
                                }





                                Session.SendNotification("Du hast das Badge erfolgreich versendet.");




                                return true;
                            }




                        case 83789: //ADDGC
                            {


                                if (Session.GetHabbo().Rank < 10)
                                {

                                    return false;
                                }



                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);


                                if (TargetClient.GetHabbo() == null)
                                {

                                    Session.GetHabbo().Whisper("Der User wurde nicht gefunden.");
                                    return true;
                                }

                                if (TargetClient.GetHabbo().gc == 1)
                                {

                                    Session.GetHabbo().Whisper("Der User hat bereits die Global Chat Rechte.");
                                    return true;
                                }


                                TargetClient.GetHabbo().gc = 1;
                                Session.GetHabbo().Whisper("Dem User " + TargetClient.GetHabbo().Username + " wurden die Rechte für den Globalen Chat gegeben");

                                TargetClient.GetHabbo().Whisper("Dir wurden folgende Rechte zugeteilt: Globalchat");
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }







                        case 77585:
                            {

                                string created = Session.GetHabbo().DataCadastro;
                                int createdint = Convert.ToInt32(created);
                                int createdcheck = createdint + (7 * 60 * 60 * 24);
                                int timenow = (int)HabboIM.GetUnixTimestamp();
                                if (Session.GetHabbo().AchievementScore < 1200 || timenow <= createdcheck)
                                {
                                    Session.SendNotification("Du benötigst 1200 Erfahrungspunkte und musst 7 Tage im Habbo registriert sein um den Globalchat benutzen zu können!");

                                    return true;
                                }

                                if (Session.GetHabbo().gc == 0)
                                {
                                    Session.SendNotification("Du wurdest aus dem Globalchat ausgeschlossen!");
                                    return true;

                                }

                                if (HabboIM.GetGame().GetClientManager().gc == false)

                                {
                                    Session.GetHabbo().Whisper("Der Globale Chat ist derzeit deaktiviert.");
                                    return true;

                                }
                                if (Session.GetHabbo().gchat == 0)
                                {

                                    Session.GetHabbo().Whisper("Du hast den globalen Chat gemutet und kannst ihn deshalb nicht mehr nutzen, bis du :unmutegc sagst."); return true;
                                }
                                if (smethod_4b(Session, Input.Substring(3), "GC") != Input.Substring(3))
                                {

                                    Session.GetHabbo().Whisper("Deine Nachricht wurde blockiert: Verdacht auf Fremdwerbung!");
                                }



                                if (smethod_4(Input.Substring(3)) != Input.Substring(3))
                                {

                                    Session.GetHabbo().Whisper("Beleidigungen sind auch im Globalchat zu unterlassen!");
                                    return true;
                                }


                                HabboIM.GetGame().GetClientManager().method_18GC(Session.GetHabbo().Username, Input.Substring(3));
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);

                                /*
                                ServerMessage Message = new ServerMessage(440u);
                                Message.AppendUInt(Session.GetHabbo().Id);
                                Message.AppendInt32(Session.GetHabbo().Respect);
                                Session.GetHabbo().CurrentRoom.SendMessage(Message, null);
                                */




                                return true;
                            }


                        case 50:
                            if (Session.GetHabbo().HasFuse("cmd_summon"))
                            {
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient != null && TargetClient.GetHabbo().CurrentRoom != Session.GetHabbo().CurrentRoom)
                                {
                                    ServerMessage Message4 = new ServerMessage(286u);
                                    Message4.AppendBoolean(Session.GetHabbo().CurrentRoom.IsPublic);
                                    Message4.AppendUInt(Session.GetHabbo().CurrentRoomId);
                                    TargetClient.SendMessage(Message4);
                                    TargetClient.SendNotification(Session.GetHabbo().Username + " hat dich zu sich teleportiert.");
                                }
                                else
                                {
                                    Session.GetHabbo().Whisper("Der Spieler " + Params[1] + " ist aktuell nicht online oder bereits im Raum!");
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 51:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_superban"))
                                {
                                    result = false;
                                    return result;
                                }
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.GetHabbo().Whisper("User konnte nicht gefunden werden.");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
                                {
                                    Session.GetHabbo().Whisper("Du besitzt nicht die nötigen Rechte.");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }


                                TargetClient.GetHabbo().jail = 1;
                                TargetClient.GetHabbo().jailtime = 360000000.0;
                                TargetClient.GetHabbo().UpdateJailTime(true);
                                TargetClient.GetHabbo().UpdateJail(true);

                                ServerMessage Message = new ServerMessage(35u);
                                Message.AppendStringWithBreak("Aufgrund eines Regelverstoßes wurdest du dauerhaft aus Habbo gebannt!\r\r", 13);
                                Message.AppendStringWithBreak("Banngrund: " + ChatCommandHandler.MergeParams(Params, 2));
                                Message.AppendStringWithBreak("Gebannt von: "+ Session.GetHabbo().Username +"");
                                TargetClient.SendMessage(Message);




                                ServerMessage Logging = new ServerMessage(134u);
                                Logging.AppendUInt(0u);


                                Logging.AppendString(string.Concat(new string[]
                                {
                                                  "BanManager: "+ Session.GetHabbo().Username +" hat "+ TargetClient.GetHabbo().Username+ " dauerhaft aus dem Hotel gebannt! (Grund: "+ ChatCommandHandler.MergeParams(Params, 2) +")"

                                }));

                                HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);






                                //HabboIM.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, 360000000.0, ChatCommandHandler.MergeParams(Params, 2), false,false);
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }


                        case 998877345:

                            if (Session.GetHabbo().Rank < 1)
                            {
                                Session.SendNotification("Du scheinst ein Geist zu sein... Du besitzt nichteinmal Rang 1 was eigentlich User entspricht!");

                                return true;


                            }

                            {
                                uint num2 = Convert.ToUInt32(5310);
                                Room class3 = HabboIM.GetGame().GetRoomManager().method_15(num2);

                                if (Session.GetHabbo().CurrentRoom != class3)
                                {


                                    ServerMessage Message4 = new ServerMessage(286u);
                                    Message4.AppendBoolean(class3.IsPublic);
                                    Message4.AppendUInt(class3.Id);
                                    Session.SendMessage(Message4);
                                    Session.SendNotification("Um nach Enten oder Kristallen zu suchen musst du in der AFK Arena sein. Wir haben dich mal hinteleportiert...");

                                    return true;

                                }


                                if (Session.GetHabbo().CurrentRoom == class3)
                                {
                                    if (Session.GetHabbo().collector == false)
                                    {
                                        Session.GetHabbo().last_ente = HabboIM.GetUnixTimestamp();
                                        Session.GetHabbo().last_dia = HabboIM.GetUnixTimestamp();
                                        Session.GetHabbo().collector = true;
                                        Session.SendNotification("Du hast begonnen, nach Enten und Kristallen zu suchen!");

                                        var class5 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);

                                        class5.MoveTo(13, 21);




                                        var t = Task.Run(async delegate
                                        {
                                            await Task.Delay(3150);

                                        X:
                                            await Task.Delay(2025);

                                            if (Session.GetHabbo().collector == true && Session.GetHabbo().CurrentRoom == class3)
                                            {

                                                var random_diamonds = new Random();//Das sorgt dafür das variable random_diamons einen Rand ablegt. Also Random
                                                var random_enten = new Random(); // Sorgt wie oben nur halt enten...
                                                RoomUser class9553104 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);

                                                if (Session.GetHabbo().last_dia + 11 * 60 < HabboIM.GetUnixTimestamp())
                                                {
                                                    //hier sagen wir er soll 1-5 nutzen. hierbei greift er auf random_diaonds zu. dahier sagt er also maximal 1-5 als RANDOM denn sonst könnte auch 8998 raus kommen und dann würde es nicht funktionierern...
                                                    switch (random_diamonds.Next(1, 5))
                                                    {

                                                        case 1:
                                                            GameClient search_event_k1 = class9553104.GetClient();
                                                            class9553104.HandleSpeech(search_event_k1, "*Gräbt einen kleinen Kristall aus*", false);

                                                            Session.GetHabbo().last_dia = HabboIM.GetUnixTimestamp();
                                                            Session.GetHabbo().Whisper("Du hast 1 Kristall gefunden!");
                                                            Session.GetHabbo().VipPoints += 1;
                                                            Session.GetHabbo().UpdateVipPoints(false, true);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(44, true);

                                                            await Task.Delay(5000);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(500, true);
                                                        break;

                                                        case 2:
                                                            GameClient search_event_k5 = class9553104.GetClient();
                                                            class9553104.HandleSpeech(search_event_k5, "*Gräbt 5 Kristalle aus*", false);

                                                            Session.GetHabbo().last_dia = HabboIM.GetUnixTimestamp();
                                                            Session.GetHabbo().Whisper("Du hast 5 Kristalle gefunden!");
                                                            Session.GetHabbo().VipPoints += 5;
                                                            Session.GetHabbo().UpdateVipPoints(false, true);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(44, true);

                                                            await Task.Delay(5000);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(500, true);

                                                            break;

                                                        case 3:
                                                            GameClient search_event_k4 = class9553104.GetClient();
                                                            class9553104.HandleSpeech(search_event_k4, "*Gräbt 4 Kristalle aus*", false);

                                                            Session.GetHabbo().last_dia = HabboIM.GetUnixTimestamp();
                                                            Session.GetHabbo().Whisper("Du hast 4 Kristalle gefunden!");
                                                            Session.GetHabbo().VipPoints += 4;
                                                            Session.GetHabbo().UpdateVipPoints(false, true);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(44, true);

                                                            await Task.Delay(5000);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(500, true);

                                                            break;

                                                        case 4:
                                                            GameClient search_event_k3 = class9553104.GetClient();
                                                            class9553104.HandleSpeech(search_event_k3, "*Gräbt 3 Kristalle aus*", false);

                                                            Session.GetHabbo().last_dia = HabboIM.GetUnixTimestamp();
                                                            Session.GetHabbo().Whisper("Du hast 3 Kristalle gefunden!");
                                                            Session.GetHabbo().VipPoints += 3;
                                                            Session.GetHabbo().UpdateVipPoints(false, true);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(44, true);

                                                            await Task.Delay(5000);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(500, true);

                                                            break;

                                                        case 5:
                                                            GameClient search_event_k2 = class9553104.GetClient();
                                                            class9553104.HandleSpeech(search_event_k2, "*Gräbt 2 Kristalle aus*", false);

                                                            Session.GetHabbo().last_dia = HabboIM.GetUnixTimestamp();
                                                            Session.GetHabbo().Whisper("Du hast 2 Kristalle gefunden!");
                                                            Session.GetHabbo().VipPoints += 2;
                                                            Session.GetHabbo().UpdateVipPoints(false, true);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(44, true);

                                                            await Task.Delay(5000);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(500, true);

                                                            break;

                                                    }
                                                }
                                                if (Session.GetHabbo().last_ente + 24 * 60 < HabboIM.GetUnixTimestamp())
                                                {
                                                    switch (random_enten.Next(1, 5))
                                                    {

                                                        case 1:
                                                            GameClient search_event_e5 = class9553104.GetClient();
                                                            class9553104.HandleSpeech(search_event_e5, "*Fängt 5 Enten ein*", false);

                                                            Session.GetHabbo().last_ente = HabboIM.GetUnixTimestamp();
                                                            Session.GetHabbo().Whisper("Du hast gerade 5 Enten gefunden.");
                                                            Session.GetHabbo().ActivityPoints += 5;
                                                            Session.GetHabbo().UpdateActivityPoints(true);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(170, true);
                                                            

                                                            await Task.Delay(6500);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(500, true);
                                                            break;

                                                        case 2:
                                                            GameClient search_event_e3 = class9553104.GetClient();
                                                            class9553104.HandleSpeech(search_event_e3, "*Fängt 3 Enten ein*", false);

                                                            Session.GetHabbo().last_ente = HabboIM.GetUnixTimestamp();
                                                            Session.GetHabbo().Whisper("Du hast gerade 3 Enten gefunden.");
                                                            Session.GetHabbo().ActivityPoints += 3;
                                                            Session.GetHabbo().UpdateActivityPoints(true);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(170, true);

                                                            await Task.Delay(6500);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(500, true);
                                                            break;

                                                        case 3:
                                                            GameClient search_event_e2 = class9553104.GetClient();
                                                            class9553104.HandleSpeech(search_event_e2, "*Fängt 2 Enten ein*", false);

                                                            Session.GetHabbo().last_ente = HabboIM.GetUnixTimestamp();
                                                            Session.GetHabbo().Whisper("Du hast gerade 2 Enten gefunden.");
                                                            Session.GetHabbo().ActivityPoints += 2;
                                                            Session.GetHabbo().UpdateActivityPoints(true);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(170, true);

                                                            await Task.Delay(6500);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(500, true);
                                                            break;

                                                        case 4:
                                                            GameClient search_event_e4 = class9553104.GetClient();
                                                            class9553104.HandleSpeech(search_event_e4, "*Fängt 4 Enten ein*", false);

                                                            Session.GetHabbo().last_ente = HabboIM.GetUnixTimestamp();
                                                            Session.GetHabbo().Whisper("Du hast gerade 4 Enten gefunden.");
                                                            Session.GetHabbo().ActivityPoints += 4;
                                                            Session.GetHabbo().UpdateActivityPoints(true);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(170, true);

                                                            await Task.Delay(6500);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(500, true);
                                                            break;

                                                        case 5:
                                                            GameClient search_event_e1 = class9553104.GetClient();
                                                            class9553104.HandleSpeech(search_event_e1, "*Fängt eine kleine süße Ente ein*", false);

                                                            Session.GetHabbo().last_ente = HabboIM.GetUnixTimestamp();
                                                            Session.GetHabbo().Whisper("Du hast gerade 1 Ente gefunden.");
                                                            Session.GetHabbo().ActivityPoints += 1;
                                                            Session.GetHabbo().UpdateActivityPoints(true);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(170, true);

                                                            await Task.Delay(6500);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(500, true);

                                                            break;
                                                    }
                                                }


                                                int x3 = class5.X;
                                                int y3 = class5.Y;

                                                Random rand = new Random();
                                                int x = rand.Next(2, 12);
                                                int y = rand.Next(2, 20);
                                                int x2 = rand.Next(2, 20);
                                                int y2 = rand.Next(3, 13);


                                                if (y < 5)
                                                {
                                                    y = y2;
                                                    x = x2;



                                                }
                                                if (x > 15)
                                                {
                                                    y = y2;


                                                }
                                                class5.MoveTo(x, y);

                                                goto X;


                                            }
                                            Session.GetHabbo().collector = false;
                                            return true;

                                        });




                                        return true;
                                    }

                                    if (Session.GetHabbo().collector == true)
                                    {

                                        Session.GetHabbo().collector = false;
                                        ServerMessage Message4 = new ServerMessage(286u);
                                        Message4.AppendBoolean(class3.IsPublic);
                                        Message4.AppendUInt(class3.Id);
                                        Session.SendMessage(Message4);
                                        Session.SendNotification("Du hast aufgehört, nach Enten und Kristallen zu suchen!");
                                        return true;
                                    }



                                }








                                return false;


                            }

                        case 52:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_teleport"))
                                {
                                    result = false;
                                    return result;
                                }
                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    result = false;
                                    return result;
                                }
                                RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                if (class3 == null)
                                {
                                    result = false;
                                    return result;
                                }
                                if (class3.TeleportMode)
                                {
                                    class3.TeleportMode = false;
                                    Session.GetHabbo().Whisper("Du hast Teleport deaktiviert.");
                                }
                                else
                                {
                                    class3.TeleportMode = true;
                                    Session.GetHabbo().Whisper("Du hast Teleport aktiviert.");
                                }
                                class2.method_22();
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        case 54:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_unmute"))
                                {
                                    result = false;
                                    return result;
                                }
                                string text = Params[1];
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null || TargetClient.GetHabbo() == null)
                                {
                                    Session.GetHabbo().Whisper("User konnte nicht gefunden werden: " + text);
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                TargetClient.GetHabbo().UnMute();
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        case 55:
                            if (Session.GetHabbo().HasFuse("cmd_update_achievements"))
                            {
                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {
                                    AchievementManager.smethod_0(dbClient);
                                }
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 56:
                            if (Session.GetHabbo().HasFuse("cmd_update_bans"))
                            {
                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {
                                    HabboIM.GetGame().GetBanManager().Initialise(dbClient);
                                }
                                HabboIM.GetGame().GetClientManager().method_28();
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 57:
                            if (Session.GetHabbo().HasFuse("cmd_update_bots"))
                            {
                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {
                                    HabboIM.GetGame().GetBotManager().method_0(dbClient);
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 58:
                            if (Session.GetHabbo().HasFuse("cmd_update_catalogue"))
                            {
                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {
                                    HabboIM.GetGame().GetCatalog().method_0(dbClient);
                                }
                                HabboIM.GetGame().GetCatalog().method_1();
                                HabboIM.GetGame().GetClientManager().BroadcastMessage(new ServerMessage(441u));
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 59:
                            if (Session.GetHabbo().HasFuse("cmd_update_filter"))
                            {
                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {
                                    ChatCommandHandler.InitWords(dbClient);
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 60:
                            if (Session.GetHabbo().HasFuse("cmd_update_items"))
                            {
                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {
                                    HabboIM.GetGame().GetItemManager().method_0(dbClient);
                                }
                                Session.SendNotification("Item defenitions reloaded successfully.");
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 61:
                            if (Session.GetHabbo().HasFuse("cmd_update_navigator"))
                            {
                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {
                                    HabboIM.GetGame().GetNavigator().method_0(dbClient);
                                    HabboIM.GetGame().GetRoomManager().method_8(dbClient);
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;

                        case 3589389:

                            {

                                if (Session.GetHabbo().Rank < 1)
                                {


                                    return false;
                                }
                                if (HabboIM.GetGame().GetClientManager().wartung == false)
                                {
                                    HabboIM.GetGame().GetClientManager().wartung = true;
                                    HabboIM.GetGame().GetClientManager().method_New99();
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    ServerMessage Message2 = new ServerMessage(808u);


                                    Message2.AppendStringWithBreak("Hotel geschlossen!");
                                    Message2.AppendStringWithBreak("Das Hotel wird geschlossen. Kommt alle ins www.Habbo.ai");
                                    ServerMessage Message3 = new ServerMessage(161u);
                                    Message3.AppendStringWithBreak("Das Hotel wird geschlossen. Kommt alle ins www.Habbo.ai");
                                    HabboIM.GetGame().GetClientManager().method_15(Message3, Message3);

                                    return true;
                                }

                                if (HabboIM.GetGame().GetClientManager().wartung == true)
                                {
                                    HabboIM.GetGame().GetClientManager().wartung = false;

                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    ServerMessage Message2 = new ServerMessage(808u);


                                    Message2.AppendStringWithBreak("Das Hotel wird geschlossen. Kommt alle ins www.Habbo.ai");
                                    Message2.AppendStringWithBreak("Das Hotel wird geschlossen. Kommt alle ins www.Habbo.ai");
                                    ServerMessage Message3 = new ServerMessage(161u);
                                    Message3.AppendStringWithBreak("Das Hotel wird geschlossen. Kommt alle ins www.Habbo.ai");
                                    HabboIM.GetGame().GetClientManager().method_15(Message2, Message3);


                                }


                                return true;
                            }


                        case 62:
                            if (Session.GetHabbo().HasFuse("cmd_update_permissions"))
                            {
                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {
                                    HabboIM.GetGame().GetRoleManager().method_0(dbClient);
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 63:
                            if (Session.GetHabbo().HasFuse("cmd_update_settings"))
                            {
                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {
                                    HabboIM.GetGame().LoadServerSettings(dbClient);
                                }
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 64:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_userinfo"))
                                {
                                    result = false;
                                    return result;
                                }
                                string text5 = Params[1];
                                bool flag2 = true;
                                if (string.IsNullOrEmpty(text5))
                                {
                                    Session.GetHabbo().Whisper("Bitte ein Username eingeben.");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                GameClient class7 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text5);
                                Habbo class8;
                                if (class7 == null)
                                {
                                    flag2 = false;
                                    class8 = Authenticator.CreateHabbo(text5);
                                }
                                else
                                {
                                    class8 = class7.GetHabbo();
                                }
                                if (class8 == null)
                                {
                                    Session.GetHabbo().Whisper("User konnte nicht gefunden werden: " + Params[1]);
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                StringBuilder stringBuilder = new StringBuilder();
                                if (class8.CurrentRoom != null)
                                {
                                    stringBuilder.Append(" - Rauminformationen von Raum ID: " + class8.CurrentRoom.Id + " - \r");
                                    stringBuilder.Append("Besitzer: " + class8.CurrentRoom.Owner + "\r");
                                    stringBuilder.Append("Raumname: " + class8.CurrentRoom.Name + "\r");
                                    stringBuilder.Append(string.Concat(new object[]
                            {
                                "Users in Raum: ",
                                class8.CurrentRoom.UserCount,
                                " von ",
                                class8.CurrentRoom.UsersMax
                            }));
                                }
                                uint num5 = class8.Rank;
                                string text6 = "";
                                if (Session.GetHabbo().HasFuse("cmd_userinfo_viewip"))
                                {
                                    text6 = "User IP: " + class8.LastIp + " \r";
                                }
                                Session.SendNotification(string.Concat(new object[]
                        {
                            "User Informationen: ",
                            text5,
                            ":\rRank: ",
                            num5,
                            " \rUser Online: ",
                            flag2.ToString(),
                            " \rUser ID: ",
                            class8.Id,
                            " \r",
                            text6,
                            "Besuchte Raum: ",
                            class8.CurrentRoomId,
                            " \rUser Mission: ",
                            class8.Motto,
                            " \rUser Taler: ",
                            class8.Credits,
                            " \rUser Enten: ",
                            class8.ActivityPoints,
                            " \rUser Sterne: ",
                            class8.VipPoints,
                            " \rUser gemutet: ",
                            class8.IsMuted.ToString(),
                            "\r\r\r",
                            stringBuilder.ToString()
                        }));
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                        case 65:
                            if (Session.GetHabbo().HasFuse("cmd_update_texts"))
                            {
                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {
                                    HabboIMEnvironment.LoadExternalTexts(dbClient);
                                }
                                result = true;
                                return result;
                            }
                            result = false;
                            return result;
                        case 66:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_disconnect"))
                                {
                                    result = false;
                                    return result;
                                }
                                string text = Params[1];
                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null)
                                {
                                    Session.GetHabbo().Whisper("Could not find user: " + text);
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                if (Session.GetHabbo().Rank <= TargetClient.GetHabbo().Rank)
                                {
                                    Session.GetHabbo().Whisper("Du besitzt nicht die nötigen Rechte.");
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                }
                                TargetClient.method_12();
                                result = true;
                                return result;
                            }
                        case 87:
                            if (!Session.GetHabbo().HasFuse("cmd_vipha"))
                            {
                                result = false;
                                return result;
                            }
                            if (HabboIM.GetUnixTimestamp() - Session.GetHabbo().LastVipAlert >= ServerConfiguration.VIPHotelAlertInterval)
                            {
                                Session.GetHabbo().LastVipAlert = HabboIM.GetUnixTimestamp();
                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {
                                    dbClient.AddParamWithValue("sessionid", Session.GetHabbo().Id);
                                    dbClient.ExecuteQuery("UPDATE users SET vipha_last = UNIX_TIMESTAMP() WHERE id = @sessionid", 30);
                                }
                                string str = Input.Substring(6);
                                ServerMessage Message2 = new ServerMessage(808u);
                                Message2.AppendStringWithBreak(HabboIMEnvironment.GetExternalText("cmd_vipha_title"));
                                Message2.AppendStringWithBreak(str + "\r\n- " + Session.GetHabbo().Username);
                                ServerMessage Message3 = new ServerMessage(161u);
                                Message3.AppendStringWithBreak(str + "\r\n- " + Session.GetHabbo().Username);
                                HabboIM.GetGame().GetClientManager().method_15(Message2, Message3);
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            Session.SendNotification("You need wait: " + (int)((Session.GetHabbo().LastVipAlert - HabboIM.GetUnixTimestamp() + ServerConfiguration.VIPHotelAlertInterval) / 60.0) + " minute!");
                            result = true;
                            return result;
                        case 91:
                            if (Session.GetHabbo().Rank < 6 && Session.GetHabbo().CurrentRoom.Owner != Session.GetHabbo().Username)
                            {
                                return false;
                            }
                            if (Session.GetHabbo().CurrentRoom == null)
                            {
                                result = false;
                                return result;
                            }
                            for (int i = 0; i < Session.GetHabbo().CurrentRoom.RoomUsers.Length; i++)
                            {
                                try
                                {
                                    RoomUser class5 = Session.GetHabbo().CurrentRoom.RoomUsers[i];
                                    if (class5 != null)
                                    {
                                        if (!class5.IsBot)
                                        {
                                            if (class5.GetClient() != null)
                                            {
                                                if (class5.GetClient().GetHabbo() != null)
                                                {
                                                    int int_ = int.Parse(Params[1]);
                                                    class5.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(int_, true);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex2)
                                {
                                    Session.SendNotification("Error: " + ex2.ToString());
                                }
                            }
                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                            result = true;
                            return result;
                        case 97:
                            if (!Session.GetHabbo().HasFuse("cmd_viphal"))
                            {
                                result = false;
                                return result;
                            }
                            if (HabboIM.GetUnixTimestamp() - Session.GetHabbo().LastVipAlertLink >= ServerConfiguration.VIPHotelAlertLinkInterval)
                            {
                                Session.GetHabbo().LastVipAlertLink = HabboIM.GetUnixTimestamp();
                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                {
                                    dbClient.AddParamWithValue("sessionid", Session.GetHabbo().Id);
                                    dbClient.ExecuteQuery("UPDATE users SET viphal_last = UNIX_TIMESTAMP() WHERE id = @sessionid", 30);
                                }
                                string text2 = Params[1];
                                Input = Input.Substring(4).Replace(text2, "");
                                string text3 = Input.Substring(1);
                                ServerMessage Message = new ServerMessage(161u);
                                Message.AppendStringWithBreak(string.Concat(new string[]
                            {
                                HabboIMEnvironment.GetExternalText("cmd_viphal_title"),
                                "\r\n",
                                text3,
                                "\r\n-",
                                Session.GetHabbo().Username
                            }));
                                Message.AppendStringWithBreak(text2);
                                HabboIM.GetGame().GetClientManager().BroadcastMessage(Message);
                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                result = true;
                                return result;
                            }
                            Session.SendNotification("You need wait: " + (int)((Session.GetHabbo().LastVipAlertLink - HabboIM.GetUnixTimestamp() + ServerConfiguration.VIPHotelAlertLinkInterval) / 60.0) + " minute!");
                            result = true;
                            return result;
                    }
                    num = HabboIM.GetGame().GetRoleManager().dictionary_4[Params[0]];
                    if (num <= 13)
                    {
                        if (num != 1)
                        {
                            switch (num)
                            {
                                case 5:
                                    {
                                        if (!Session.GetHabbo().HasFuse("cmd_buy"))
                                        {
                                            result = false;
                                            return result;
                                        }
                                        int num6 = (int)Convert.ToInt16(Params[1]);
                                        if (num6 > 0 && num6 < 101)
                                        {
                                            Session.GetHabbo().int_24 = (int)Convert.ToInt16(Params[1]);
                                        }
                                        else
                                        {
                                            Session.GetHabbo().Whisper("Bitte die Zahl zwischen 1 und 100 nehmen!");
                                        }
                                        result = true;
                                        return result;
                                    }
                                case 6:
                                case 7:
                                case 8:
                                case 11:
                                    goto IL_B3BE;
                                case 9:
                                    Session.GetHabbo().GetInventoryComponent().ClearInventory();
                                    Session.SendNotification(HabboIMEnvironment.GetExternalText("cmd_emptyitems_success"));
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                case 10:
                                    if (Session.GetHabbo().HasFuse("cmd_empty") && Params[1] != null)
                                    {
                                        GameClient class9 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                        if (class9 != null && class9.GetHabbo() != null)
                                        {
                                            class9.GetHabbo().GetInventoryComponent().ClearInventory();
                                            Session.SendNotification("Inventory cleared! (Database and cache)");
                                        }
                                        else
                                        {
                                            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                            {
                                                dbClient.AddParamWithValue("usrname", Params[1]);
                                                int num7 = int.Parse(dbClient.ReadString("SELECT Id FROM users WHERE username = @usrname LIMIT 1;", 30));
                                                dbClient.ExecuteQuery("DELETE FROM items WHERE user_id = '" + num7 + "' AND room_id = 0;", 30);
                                                Session.SendNotification("Inventory cleared! (Database)");
                                            }
                                        }
                                        HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                        result = true;
                                        return result;
                                    }
                                    result = false;
                                    return result;
                                case 12:
                                    {
                                        if (!Session.GetHabbo().GetBadgeComponent().HasBadge("NAMECHANGE"))
                                        {
                                            Session.GetHabbo().Whisper("Du darfst deinen Usernamen nicht ändern!");
                                            result = true;
                                            return result;
                                        }
                                        if (Session.GetHabbo().VipPoints < 10)
                                        {
                                            Session.GetHabbo().Whisper("Um deinen Usernamen ändern zu können, benötigst du 10 Sterne.");
                                            result = true;
                                            return result;
                                        }
                                        string changetime = Session.GetHabbo().ChangeNameTime;
                                        int changetimeint = Convert.ToInt32(changetime);
                                        if (changetimeint != 0)
                                        {
                                            int changetimecheck = changetimeint + 604800;
                                            int timenow = (int)HabboIM.GetUnixTimestamp();
                                            if (timenow < changetimecheck)
                                            {
                                                Session.GetHabbo().Whisper("Du kannst deinen Namen nur alle 7 Tage ändern!");
                                                result = true;
                                                return result;
                                            }
                                        }
                                        ServerMessage Message5_ = new ServerMessage(573u);
                                        Session.SendMessage(Message5_);
                                        result = true;
                                        return result;
                                    }
                                case 13:
                                    if (!ServerConfiguration.UnknownBoolean9 && !Session.GetHabbo().HasFuse("cmd_follow"))
                                    {
                                        Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_error_disabled"));
                                        result = true;
                                        return result;
                                    }
                                    if (!Session.GetHabbo().IsVIP && !Session.GetHabbo().HasFuse("cmd_follow"))
                                    {
                                        Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_error_permission_vip"));
                                        result = true;
                                        return result;
                                    }
                                    TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                    if (TargetClient != null && TargetClient.GetHabbo().InRoom && Session.GetHabbo().CurrentRoom != TargetClient.GetHabbo().CurrentRoom && !TargetClient.GetHabbo().HideInRom)
                                    {
                                        ServerMessage Message4 = new ServerMessage(286u);
                                        Message4.AppendBoolean(TargetClient.GetHabbo().CurrentRoom.IsPublic);
                                        Message4.AppendUInt(TargetClient.GetHabbo().CurrentRoomId);
                                        Session.SendMessage(Message4);
                                    }
                                    else
                                    {
                                        Session.GetHabbo().Whisper("User: " + Params[1] + " could not be found - Maybe they're not online or not in a room anymore (or maybe they're a ninja)");
                                    }
                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    result = true;
                                    return result;
                                default:
                                    goto IL_B3BE;
                            }
                        }
                    }
                    else
                    {
                        switch (num)
                        {
                            case 28:
                                {
                                    if (!ServerConfiguration.UnknownBoolean7 && !Session.GetHabbo().HasFuse("cmd_mimic"))
                                    {
                                        Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_error_disabled"));
                                        result = true;
                                        return result;
                                    }
                                    if (!Session.GetHabbo().IsVIP && !Session.GetHabbo().HasFuse("cmd_mimic"))
                                    {
                                        Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_error_permission_vip"));
                                        result = true;
                                        return result;
                                    }
                                    string text = Params[1];
                                    TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text);
                                    if (TargetClient == null)
                                    {
                                        Session.GetHabbo().Whisper("Could not find user: " + text);
                                        result = true;
                                        return result;
                                    }
                                    Session.GetHabbo().Figure = TargetClient.GetHabbo().Figure;
                                    Session.GetHabbo().UpdateLook(false, Session);
                                    result = true;
                                    return result;
                                }
                            case 29:
                                {
                                    if (!ServerConfiguration.UnknownBoolean8 && !Session.GetHabbo().HasFuse("cmd_moonwalk"))
                                    {
                                        Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_error_disabled"));
                                        result = true;
                                        return result;
                                    }
                                    if (!Session.GetHabbo().IsVIP && !Session.GetHabbo().HasFuse("cmd_moonwalk"))
                                    {
                                        Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_error_permission_vip"));
                                        result = true;
                                        return result;
                                    }
                                    class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    if (class2 == null)
                                    {
                                        result = false;
                                        return result;
                                    }
                                    RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    if (class3 == null)
                                    {
                                        result = false;
                                        return result;
                                    }
                                    if (class3.bool_3)
                                    {
                                        class3.bool_3 = false;
                                        Session.GetHabbo().Whisper("Your moonwalk has been disabled.");
                                        result = true;
                                        return result;
                                    }
                                    class3.bool_3 = true;
                                    Session.GetHabbo().Whisper("Your moonwalk has been enabled.");
                                    result = true;
                                    return result;
                                }
                            default:
                                {
                                    int num9 = num;
                                    RoomUser class5;
                                    switch (num9)
                                    {
                                        case 36:
                                            try
                                            {
                                                if (!ServerConfiguration.UnknownBoolean2 && !Session.GetHabbo().HasFuse("cmd_pull"))
                                                {
                                                    Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_error_disabled"));
                                                    result = true;
                                                    return result;
                                                }
                                                if (!Session.GetHabbo().IsVIP && !Session.GetHabbo().HasFuse("cmd_pull"))
                                                {
                                                    Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_error_permission_vip"));
                                                    result = true;
                                                    return result;
                                                }
                                                string a = "down";
                                                string text = Params[1];
                                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text);
                                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                if (Session == null || TargetClient == null)
                                                {
                                                    result = false;
                                                    return result;
                                                }
                                                class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                                {
                                                    Session.GetHabbo().Whisper("You cannot pull yourself");
                                                    result = true;
                                                    return result;
                                                }
                                                if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId && Math.Abs(class5.X - class4.X) < 3 && Math.Abs(class5.Y - class4.Y) < 3)
                                                {
                                                    class5.HandleSpeech(Session, "*pulls " + TargetClient.GetHabbo().Username + " to them*", false);
                                                    if (class5.BodyRotation == 0)
                                                    {
                                                        a = "up";
                                                    }
                                                    if (class5.BodyRotation == 2)
                                                    {
                                                        a = "right";
                                                    }
                                                    if (class5.BodyRotation == 4)
                                                    {
                                                        a = "down";
                                                    }
                                                    if (class5.BodyRotation == 6)
                                                    {
                                                        a = "left";
                                                    }
                                                    if (a == "up")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (class5.X != class2.RoomModel.int_0 || class5.Y - 1 != class2.RoomModel.int_1 || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                            {
                                                                class4.MoveTo(class5.X, class5.Y - 1);
                                                            }
                                                            else
                                                            {
                                                                class4.MoveTo(class5.X, class5.Y + 1);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class5.X, class5.Y - 1);
                                                        }
                                                    }
                                                    if (a == "right")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (class5.X + 1 != class2.RoomModel.int_0 || class5.Y != class2.RoomModel.int_1 || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                            {
                                                                class4.MoveTo(class5.X + 1, class5.Y);
                                                            }
                                                            else
                                                            {
                                                                class4.MoveTo(class5.X - 1, class5.Y);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class5.X + 1, class5.Y);
                                                        }
                                                    }
                                                    if (a == "down")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (class5.X != class2.RoomModel.int_0 || class5.Y + 1 != class2.RoomModel.int_1 || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                            {
                                                                class4.MoveTo(class5.X, class5.Y + 1);
                                                            }
                                                            else
                                                            {
                                                                class4.MoveTo(class5.X, class5.Y - 1);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class5.X, class5.Y + 1);
                                                        }
                                                    }
                                                    if (a == "left")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (class5.X - 1 != class2.RoomModel.int_0 || class5.Y != class2.RoomModel.int_1 || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                            {
                                                                class4.MoveTo(class5.X - 1, class5.Y);
                                                            }
                                                            else
                                                            {
                                                                class4.MoveTo(class5.X + 1, class5.Y);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class5.X - 1, class5.Y);
                                                        }
                                                    }
                                                    result = true;
                                                    return result;
                                                }
                                                Session.GetHabbo().Whisper("That user is not close enough to you to be pulled, try getting closer");
                                                result = true;
                                                return result;
                                            }
                                            catch
                                            {
                                                result = false;
                                                return result;
                                            }
                                        case 37:
                                            break;
                                        case 38:
                                            goto IL_B1EA;
                                        case 39:
                                            goto IL_B3BE;
                                        case 40:
                                            {
                                                string text = Params[1];
                                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                RoomUser class4 = class2.method_57(text);
                                                if (class5.class34_1 != null)
                                                {
                                                    Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_ride_err_riding"));
                                                    result = true;
                                                    return result;
                                                }
                                                if (!class4.IsBot || class4.PetData.Type != 13u)
                                                {
                                                    Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_ride_err_nothorse"));
                                                    result = true;
                                                    return result;
                                                }
                                                bool arg_40EB_0;
                                                if ((class5.X + 1 != class4.X || class5.Y != class4.Y) && (class5.X - 1 != class4.X || class5.Y != class4.Y) && (class5.Y + 1 != class4.Y || class5.X != class4.X))
                                                {
                                                    if (class5.Y - 1 == class4.Y)
                                                    {
                                                        if (class5.X == class4.X)
                                                        {
                                                            goto IL_4D52;
                                                        }
                                                    }
                                                    arg_40EB_0 = (class5.X != class4.X || class5.Y != class4.Y);
                                                    goto IL_4D55;
                                                }
                                            IL_4D52:
                                                arg_40EB_0 = false;
                                            IL_4D55:
                                                if (arg_40EB_0)
                                                {
                                                    Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_ride_err_toofar"));
                                                    result = true;
                                                    return result;
                                                }
                                                if (class4.RoomBot.RoomUser_0 == null)
                                                {
                                                    class4.RoomBot.RoomUser_0 = class5;
                                                    class5.class34_1 = class4.RoomBot;
                                                    class5.X = class4.X;
                                                    class5.Y = class4.Y;
                                                    class5.double_0 = class4.double_0 + 1.0;
                                                    class5.BodyRotation = class4.BodyRotation;
                                                    class5.int_7 = class4.int_7;
                                                    class5.UpdateNeeded = true;
                                                    class2.method_87(class5, false, false);
                                                    class5.RoomUser_0 = class4;
                                                    class5.Statusses.Clear();
                                                    class4.Statusses.Clear();
                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(77, true);
                                                    Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_ride_instr_getoff"));
                                                    class2.method_22();
                                                    result = true;
                                                    return result;
                                                }
                                                Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_ride_err_tooslow"));
                                                result = true;
                                                return result;
                                            }
                                        default:
                                            {
                                                if (num9 == 88)
                                                {
                                                    try
                                                    {
                                                        if (!Session.GetHabbo().HasFuse("cmd_spush"))
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        string a = "down";
                                                        string text = Params[1];
                                                        TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text);
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        if (Session == null || TargetClient == null)
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                        if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                                        {
                                                            Session.GetHabbo().Whisper("It can't be that bad mate, no need to push yourself!");
                                                            result = true;
                                                            return result;
                                                        }
                                                        bool arg_3DD2_0;
                                                        if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                        {
                                                            if ((class5.X + 1 != class4.X || class5.Y != class4.Y) && (class5.X - 1 != class4.X || class5.Y != class4.Y) && (class5.Y + 1 != class4.Y || class5.X != class4.X))
                                                            {
                                                                if (class5.Y - 1 == class4.Y)
                                                                {
                                                                    if (class5.X == class4.X)
                                                                    {
                                                                        goto IL_50AE;
                                                                    }
                                                                }
                                                                arg_3DD2_0 = (class5.X != class4.X || class5.Y != class4.Y);
                                                                goto IL_50B9;
                                                            }
                                                        IL_50AE:
                                                            arg_3DD2_0 = false;
                                                        }
                                                        else
                                                        {
                                                            arg_3DD2_0 = true;
                                                        }
                                                    IL_50B9:
                                                        if (!arg_3DD2_0)
                                                        {
                                                            class5.HandleSpeech(Session, "*pushes " + TargetClient.GetHabbo().Username + "*", false);
                                                            if (class5.BodyRotation == 0)
                                                            {
                                                                a = "up";
                                                            }
                                                            if (class5.BodyRotation == 2)
                                                            {
                                                                a = "right";
                                                            }
                                                            if (class5.BodyRotation == 4)
                                                            {
                                                                a = "down";
                                                            }
                                                            if (class5.BodyRotation == 6)
                                                            {
                                                                a = "left";
                                                            }
                                                            if (a == "up")
                                                            {
                                                                if (ServerConfiguration.PreventDoorPush)
                                                                {
                                                                    if (Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                    {
                                                                        class4.MoveTo(class4.X, class4.Y - 1);
                                                                        class4.MoveTo(class4.X, class4.Y - 2);
                                                                        class4.MoveTo(class4.X, class4.Y - 3);
                                                                        class4.MoveTo(class4.X, class4.Y - 4);
                                                                        class4.MoveTo(class4.X, class4.Y - 5);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (class4.X != class2.RoomModel.int_0 || class4.Y - 1 != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X, class4.Y - 1);
                                                                        }
                                                                        if (class4.X != class2.RoomModel.int_0 || class4.Y - 2 != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X, class4.Y - 2);
                                                                        }
                                                                        if (class4.X != class2.RoomModel.int_0 || class4.Y - 3 != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X, class4.Y - 3);
                                                                        }
                                                                        if (class4.X != class2.RoomModel.int_0 || class4.Y - 4 != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X, class4.Y - 4);
                                                                        }
                                                                        if (class4.X != class2.RoomModel.int_0 || class4.Y - 5 != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X, class4.Y - 5);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    class4.MoveTo(class4.X, class4.Y - 1);
                                                                    class4.MoveTo(class4.X, class4.Y - 2);
                                                                    class4.MoveTo(class4.X, class4.Y - 3);
                                                                    class4.MoveTo(class4.X, class4.Y - 4);
                                                                    class4.MoveTo(class4.X, class4.Y - 5);
                                                                }
                                                            }
                                                            if (a == "right")
                                                            {
                                                                if (ServerConfiguration.PreventDoorPush)
                                                                {
                                                                    if (Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                    {
                                                                        class4.MoveTo(class4.X + 1, class4.Y);
                                                                        class4.MoveTo(class4.X + 2, class4.Y);
                                                                        class4.MoveTo(class4.X + 3, class4.Y);
                                                                        class4.MoveTo(class4.X + 4, class4.Y);
                                                                        class4.MoveTo(class4.X + 5, class4.Y);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (class4.X + 1 != class2.RoomModel.int_0 || class4.Y != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X + 1, class4.Y);
                                                                        }
                                                                        if (class4.X + 2 != class2.RoomModel.int_0 || class4.Y != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X + 2, class4.Y);
                                                                        }
                                                                        if (class4.X + 3 != class2.RoomModel.int_0 || class4.Y != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X + 3, class4.Y);
                                                                        }
                                                                        if (class4.X + 4 != class2.RoomModel.int_0 || class4.Y != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X + 4, class4.Y);
                                                                        }
                                                                        if (class4.X + 5 != class2.RoomModel.int_0 || class4.Y != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X + 5, class4.Y);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    class4.MoveTo(class4.X + 1, class4.Y);
                                                                    class4.MoveTo(class4.X + 2, class4.Y);
                                                                    class4.MoveTo(class4.X + 3, class4.Y);
                                                                    class4.MoveTo(class4.X + 4, class4.Y);
                                                                    class4.MoveTo(class4.X + 5, class4.Y);
                                                                }
                                                            }
                                                            if (a == "down")
                                                            {
                                                                if (ServerConfiguration.PreventDoorPush)
                                                                {
                                                                    if (Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                    {
                                                                        class4.MoveTo(class4.X, class4.Y + 1);
                                                                        class4.MoveTo(class4.X, class4.Y + 2);
                                                                        class4.MoveTo(class4.X, class4.Y + 3);
                                                                        class4.MoveTo(class4.X, class4.Y + 4);
                                                                        class4.MoveTo(class4.X, class4.Y + 5);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (class4.X != class2.RoomModel.int_0 || class4.Y + 1 != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X, class4.Y + 1);
                                                                        }
                                                                        if (class4.X != class2.RoomModel.int_0 || class4.Y + 2 != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X, class4.Y + 2);
                                                                        }
                                                                        if (class4.X != class2.RoomModel.int_0 || class4.Y + 3 != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X, class4.Y + 3);
                                                                        }
                                                                        if (class4.X != class2.RoomModel.int_0 || class4.Y + 4 != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X, class4.Y + 4);
                                                                        }
                                                                        if (class4.X != class2.RoomModel.int_0 || class4.Y + 5 != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X, class4.Y + 5);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    class4.MoveTo(class4.X, class4.Y + 1);
                                                                    class4.MoveTo(class4.X, class4.Y + 2);
                                                                    class4.MoveTo(class4.X, class4.Y + 3);
                                                                    class4.MoveTo(class4.X, class4.Y + 4);
                                                                    class4.MoveTo(class4.X, class4.Y + 5);
                                                                }
                                                            }
                                                            if (a == "left")
                                                            {
                                                                if (ServerConfiguration.PreventDoorPush)
                                                                {
                                                                    if (Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                    {
                                                                        class4.MoveTo(class4.X - 1, class4.Y);
                                                                        class4.MoveTo(class4.X - 2, class4.Y);
                                                                        class4.MoveTo(class4.X - 3, class4.Y);
                                                                        class4.MoveTo(class4.X - 4, class4.Y);
                                                                        class4.MoveTo(class4.X - 5, class4.Y);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (class4.X - 1 != class2.RoomModel.int_0 || class4.Y != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X - 1, class4.Y);
                                                                        }
                                                                        if (class4.X - 2 != class2.RoomModel.int_0 || class4.Y != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X - 2, class4.Y);
                                                                        }
                                                                        if (class4.X - 3 != class2.RoomModel.int_0 || class4.Y != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X - 3, class4.Y);
                                                                        }
                                                                        if (class4.X - 4 != class2.RoomModel.int_0 || class4.Y != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X - 4, class4.Y);
                                                                        }
                                                                        if (class4.X - 5 != class2.RoomModel.int_0 || class4.Y != class2.RoomModel.int_1)
                                                                        {
                                                                            class4.MoveTo(class4.X - 5, class4.Y);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    class4.MoveTo(class4.X - 1, class4.Y);
                                                                    class4.MoveTo(class4.X - 2, class4.Y);
                                                                    class4.MoveTo(class4.X - 3, class4.Y);
                                                                    class4.MoveTo(class4.X - 4, class4.Y);
                                                                    class4.MoveTo(class4.X - 5, class4.Y);
                                                                }
                                                            }
                                                        }
                                                        result = true;
                                                        return result;
                                                    }
                                                    catch
                                                    {
                                                        result = false;
                                                        return result;
                                                    }
                                                }
                                                string string_;
                                                switch (num)
                                                {
                                                    case 67:
                                                        {
                                                            HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("befehle");
                                                            result = true;
                                                            return result;
                                                            /* string text7 = "Deine Commands:\r\r";
                                                             if (Session.GetHabbo().HasFuse("cmd_update_settings"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_update_settings_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_update_bans"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_update_bans_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_update_permissions"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_update_permissions_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_update_filter"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_update_filter_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_update_bots"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_update_bots_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_update_catalogue"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_update_catalogue_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_update_items"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_update_items_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_update_navigator"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_update_navigator_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_update_achievements"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_update_achievements_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_award"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_award_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_override"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_override_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_teleport"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_teleport_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_coins"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_coins_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_pixels"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_pixels_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_points"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_points_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_alert"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_alert_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_motd"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_motd_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_roomalert"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_roomalert_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_ha"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_ha_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_hal"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_hal_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_freeze"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_freeze_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_roommute"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_roommute_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_setspeed"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_setspeed_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_globalcredits"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_globalcredits_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_globalpixels"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_globalpixels_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_globalpoints"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_globalpoints_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_masscredits"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_masscredits_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_masspixels"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_masspixels_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_masspoints"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_masspoints_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_givebadge"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_givebadge_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_removebadge"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_removebadge_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_summon"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_summon_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_roombadge"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_roombadge_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_massbadge"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_massbadge_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_userinfo"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_userinfo_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_shutdown"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_shutdown_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_invisible"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_invisible_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_ban"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_ban_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_superban"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_superban_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_ipban"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_ipban_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_kick"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_kick_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_roomkick"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_roomkick_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_mute"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_mute_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_unmute"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_unmute_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_sa"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_sa_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_spull"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_spull_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_empty"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_empty_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_update_texts"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_update_texts_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_dance"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_dance_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_rave"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_rave_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_roll"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_roll_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_control"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_control_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_makesay"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_makesay_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_sitdown"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_sitdown_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_startquestion"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_startquestion_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_vipha"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_vipha_desc") + "\r\r";
                                                             }
                                                             if (Session.GetHabbo().HasFuse("cmd_roomeffect"))
                                                             {
                                                                 text7 = text7 + HabboIMEnvironment.GetExternalText("cmd_roomeffect_desc") + "\r\r";
                                                             }
                                                             text7 += "- - - - - - - - - - - - - - - - - - - - - -\r\r";
                                                             if (Session.GetHabbo().Rank > 6u)
                                                             {
                                                                 text7 += "Admin Commands:\r~~~~~~~~~~~~~~\r:jail [USERNAME] [MINUTEN] - Sperrt den Spieler für x Minuten in den Knast!\r:verwarnung [USERNAME] [GRUND] - Erstellt einen Verwarneintrag.\r:spielerakte [USERNAME] [EINTRAG] - Erstellt einen Akteneintrag.\r:delaws [USERNAME] - Leert AntiWerber Verwarnungen vom User.\r:delbws [USERNAME] - Entfernt Beleidigungsverwarnungen von einem User.\r:eventha [EVENTNAME] - Lädt alle User zu einem Event ein.\r:reloadws - Startet die WebSocket Verbindung neu.\r:rw [USERNAME] - Weist den Spieler auf seine negative Spielweise hin.\r:offban [USERNAME] - Bannt den User offline.\r:wc <nachricht> - Versendet eine globale Flüsternachricht.\r:ac <nachricht> - Versendet eine Nachricht an alle Admins.\r:aduty - Wechselt in den Admi-Dienst.\r\r";
                                                             }
                                                             if (Session.GetHabbo().Rank > 1u)
                                                             {
                                                                 text7 += "Coole Commands\r~~~~~~~~~~~~~~\r:mimic [USERNAME] - kopiert die Habbo Style\r:follow [USERNAME] - verfolgt ein User\r:push [USERNAME] - schubst einen User von dir weg\r:pull [USERNAME] - zieht einen User zu dir\r:spush [USERNAME] - schubst einen User von dir weg\r:moonwalk - rückwärts gehen\r:pet [ID] - verwandelt sich als Haustier (Haustierliste: \":pet help\")\r:raumalert [TEXT] - informiert die User per Raumalert\r:myteleport - teleportiert in deine eigene Räume\r:roomfreeze - Friert alle in deinem Raum ein.\r:setspeed <1-50> - Stellt die Geschwindigkeit deiner Roller um.\r:spull <username> - Zieht einen User von weitem zu dir.\r:roomeffect <enable id> - Setzt jedem Spieler in deinem Raum ein Effekt.\r:freeze <username> - Friert einen Spieler ein.\r:override - Lässt dich über Möbel laufen.\r:freeze <username> - Friert einen Spieler ein.\r:fastwoff - Deaktiviert Fastwalk in deinem Raum.\r:fastwon - Aktiviert Fastwalk in deinem Raum.\r\r";
                                                             }

                                                             string text8 = text7;
                                                             text7 = string.Concat(new string[]
                                     {
                                         text8,
                                         "Standard Commands:\r~~~~~~~~~~~~~~\r:coords - zeigt die Koordinaten\r:enable [ID] - aktiviert ein Effekt\r:lay - sich hinlegen\r:sit - sich hinsetzen\r:handitem [ID] - aktiviert ein Handitem (Kaffee, Popcorn, etc.)\r:pickall - nimmt alle Möbel im Raum auf\r:unload - lädt den Raum neu\r:disablediagonal - verbietet das Laufen des Diagonals\r:setmax [ZAHL] - ändert die maximale Anzahl an Usern in einem Raum\r:convertcredits - löst alle Taler im Inventar als Währung ein\r:redeempixel - löst alle Pixel im Inventar als Währung ein\r:ride [NAME] - reitet ein Pferd\r:buy [ZAHL] - mehrere Möbel im Katalog gleichzeitig kaufen\r:emptypets - löscht alle deine Haustiere im Inventar\r:emptyitems - löscht alle deine Möbel im Inventar\r:raumkick  - kickt alle User im Raum\r:afk & :brb - markiert dich als AFK\r:zahl [ZAHL] - zeigt ein Schild mit der Zahl von 0 bis 10\r:kiss [USERNAME] - küsst ein User\r:sellroom [TALER] - verkauft ein Raum für bestimmten Betrag an Taler\r:buyroom - kauft ein Raum ein\r:werber [USERNAME] - meldet ein Werber direkt an die Staffs\r:staff - zeigt alle Mitarbeiter, die gerade online sind\r:sneeze - Niesen\r:ehe [USERNAME] - Einem User einen Heiratsantrag machen\r:kill [USERNAME] - Einen Spieler töten\r:love [USERNAME] - Jemanden lieben\r:knuddel [USERNAME] - Knuddel jemanden!\r:like [USERNAME] - Jemanden liken\r:umarm [USERNAME] - Jemanden umarmen\r:box [USERNAME] - Jemanden ins Gesicht boxen\r:wil [USERNAME] - Einen User Willkommen heißen\r:developer - Informationen über die Entwicklung des Emulators\r:cheat [USERNAME] - Zeigt dir eine kleine Userinfo eines Spielers\r:shisha - Rauche eine Shisha\r:faceless - Lasse dein Gesicht verschwinden\r:regen - Lass es in deinem Raum regnen\r:sonne - Lass in deinem Raum die Sonne scheinen!\r:dislike - Dislike jemanden!\r:hot - Finde jemanden heiß!\r:habnam - Tanze den Habnamstyle!\r:jump - Springe hoch in die Luft!\r:rauchen - Zünde dir eine Zigarette an.\r:info <username> - Zeigt dir Informationen über einen User an.\r:beziehung <username> - Versendet eine Beziehungsanfrage.\r:minitanz <1-3> - Lässt deinen Habbo im Minilook tanzen!\r:fastwalk - Lässt deinen Habbo schneller laufen.\r:fasterwalk - Lässt deinen Habbo noch schneller laufen.\r:gc <nachricht> - Versendet eine globale Nachricht an alle Habbos.\r:mutegc - Schaltet globale Benachrichtigungen stumm.\r\r"
                                     });

                                                             if (Session.GetHabbo().Rank > 1u)
                                                             {
                                                                 text7 += "FX Commands\r~~~~~~~~~~~~~~\r:iohawk <farbe> - Aktiviert ein IO Hawk.\r:springen <1-3> - Lässt deinen Avatar springen.\r:laufen <1-2> - Lässt deinen Avatar auf der Stelle laufen.\r:feel <gefühl> - Drückt Emotionen aus.\r:waffe <1-4> - Eine Waffe für deinen Habbo!\r:mini <1-4> - verwandelt dich in einen kleinen Habbo.\r:fly - Lässt deinen Habbo auf einem Besen fliegen!\r:user <username> - Für User die ihre eigenen Enables gewonnen haben.\r\rStand: 01.01.2020\rZuletzt aktualisiert von: BusinessMan";
                                                             }

                                                             Session.SendNotification(text7, 2);
                                                             result = true;
                                                             return result;*/
                                                        }



                                                    case 68:
                                                        goto IL_B2A3;
                                                    case 69:
                                                        {
                                                            StringBuilder stringBuilder2 = new StringBuilder();
                                                            for (int i = 0; i < Session.GetHabbo().CurrentRoom.RoomUsers.Length; i++)
                                                            {
                                                                class5 = Session.GetHabbo().CurrentRoom.RoomUsers[i];
                                                                if (class5 != null)
                                                                {
                                                                    stringBuilder2.Append(string.Concat(new object[]
                                            {
                                                "UserID: ",
                                                class5.UId,
                                                " RoomUID: ",
                                                class5.int_20,
                                                " VirtualID: ",
                                                class5.VirtualId,
                                                " IsBot:",
                                                class5.IsBot.ToString(),
                                                " X: ",
                                                class5.X,
                                                " Y: ",
                                                class5.Y,
                                                " Z: ",
                                                class5.double_0,
                                                " \r\r"
                                            }));
                                                                }
                                                            }
                                                            Session.SendNotification(stringBuilder2.ToString());
                                                            Session.SendNotification("RoomID: " + Session.GetHabbo().CurrentRoomId);
                                                            result = true;
                                                            return result;
                                                        }
                                                    case 70:
                                                        result = false;
                                                        return result;
                                                    case 71:
                                                        if (Session.GetHabbo().HasFuse("cmd_dance"))
                                                        {
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            GameClient class9 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                                            RoomUser class3 = class2.GetRoomUserByHabbo(class9.GetHabbo().Id);
                                                            class3.DanceId = 1;
                                                            ServerMessage Message5 = new ServerMessage(480u);
                                                            Message5.AppendInt32(class3.VirtualId);
                                                            Message5.AppendInt32(1);
                                                            class2.SendMessage(Message5, null);
                                                            result = true;
                                                            return result;
                                                        }
                                                        result = false;
                                                        return result;
                                                    case 72:
                                                        if (Session.GetHabbo().HasFuse("cmd_rave"))
                                                        {
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            class2.Rave();
                                                            result = true;
                                                            return result;
                                                        }
                                                        result = false;
                                                        return result;
                                                    case 73:
                                                        if (Session.GetHabbo().HasFuse("cmd_roll"))
                                                        {
                                                            GameClient class9 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                                            class9.GetHabbo().int_1 = (int)Convert.ToInt16(Params[2]);
                                                            result = true;
                                                            return result;
                                                        }
                                                        result = false;
                                                        return result;
                                                    case 74:
                                                        if (Session.GetHabbo().HasFuse("cmd_control"))
                                                        {
                                                            string text = Params[1];
                                                            try
                                                            {
                                                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text);
                                                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                if (Session == null || TargetClient == null)
                                                                {
                                                                    result = false;
                                                                    return result;
                                                                }
                                                                RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                                class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                                class5.RoomUser_0 = class4;
                                                            }
                                                            catch
                                                            {
                                                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                if (Session == null || TargetClient == null)
                                                                {
                                                                    result = false;
                                                                    return result;
                                                                }
                                                                class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                                class5.RoomUser_0 = null;
                                                            }
                                                            result = true;
                                                            return result;
                                                        }
                                                        result = false;
                                                        return result;
                                                    case 75:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_makesay"))
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            string text2 = Params[1];
                                                            TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text2);
                                                            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
                                                            {
                                                                Session.SendNotification("Du kannst nur unter deine Position \":makesay\" benutzen.");
                                                                result = true;
                                                                return result;
                                                            }
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            if (Session == null || TargetClient == null)
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            RoomUser roomUser = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                            roomUser.HandleSpeech(TargetClient, Input.Substring(9 + text2.Length), false);
                                                            result = true;
                                                            return result;
                                                        }
                                                    case 76:
                                                        if (Session.GetHabbo().HasFuse("cmd_sitdown"))
                                                        {
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            class2.method_55();
                                                            result = true;
                                                            return result;
                                                        }
                                                        result = false;
                                                        return result;
                                                    case 77:
                                                        result = false;
                                                        return result;
                                                    case 78:
                                                        goto IL_B3BE;
                                                    case 79:
                                                        {
                                                            if (!Session.GetHabbo().InRoom)
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            int int_2 = class2.method_56(Session.GetHabbo().Username).CarryItemID;
                                                            if (int_2 <= 0)
                                                            {
                                                                Session.GetHabbo().Whisper("You're not holding anything, pick something up first!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            string text = Params[1];
                                                            TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text);
                                                            class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                            if (Session == null || TargetClient == null)
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                                            {
                                                                result = true;
                                                                return result;
                                                            }
                                                            if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId && Math.Abs(class5.X - class4.X) < 3 && Math.Abs(class5.Y - class4.Y) < 3)
                                                            {
                                                                try
                                                                {
                                                                    class2.method_56(Params[1]).CarryItem(int_2);
                                                                    class2.method_56(Session.GetHabbo().Username).CarryItem(0);
                                                                }
                                                                catch
                                                                {
                                                                }
                                                                result = true;
                                                                return result;
                                                            }
                                                            Session.GetHabbo().Whisper("You are too far away from " + Params[1] + ", try getting closer");
                                                            result = true;
                                                            return result;
                                                        }
                                                    case 80:
                                                        if (!Session.GetHabbo().InRoom)
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        class5 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);
                                                        if (class5.Statusses.ContainsKey("sit") || class5.Statusses.ContainsKey("lay") || class5.BodyRotation == 1 || class5.BodyRotation == 3 || class5.BodyRotation == 5 || class5.BodyRotation == 7)
                                                        {
                                                            result = true;
                                                            return result;
                                                        }
                                                        if (class5.byte_1 > 0 || class5.class34_1 != null)
                                                        {
                                                            result = true;
                                                            return result;
                                                        }
                                                        class5.AddStatus("sit", ((class5.double_0 + 1.0) / 2.0 - class5.double_0 * 0.5).ToString().Replace(",", "."));
                                                        class5.UpdateNeeded = true;
                                                        result = true;
                                                        return result;
                                                    case 81:
                                                    case 82:
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        if (class5.class34_1 != null)
                                                        {
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(-1, true);
                                                            class5.class34_1.RoomUser_0 = null;
                                                            class5.class34_1 = null;
                                                            class5.double_0 -= 1.0;
                                                            class5.Statusses.Clear();
                                                            class5.UpdateNeeded = true;
                                                            int int_3 = HabboIM.smethod_5(0, class2.RoomModel.int_4);
                                                            int int_4 = HabboIM.smethod_5(0, class2.RoomModel.int_5);
                                                            class5.RoomUser_0.MoveTo(int_3, int_4);
                                                            class5.RoomUser_0 = null;
                                                            class2.method_87(class5, false, false);
                                                        }
                                                        result = true;
                                                        return result;
                                                    case 83:
                                                        Session.GetHabbo().GetInventoryComponent().RemovePetsFromInventory();
                                                        Session.SendNotification(HabboIMEnvironment.GetExternalText("cmd_emptypets_success"));
                                                        HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                        result = true;
                                                        return result;
                                                    case 84:
                                                    case 87:
                                                    case 88:
                                                    case 89:
                                                    case 90:
                                                    case 91:
                                                    case 92:
                                                    case 93:
                                                    case 97:
                                                    case 98:
                                                    case 99:
                                                    case 100:
                                                    case 101:
                                                    case 102:
                                                    case 103:
                                                    case 104:
                                                    case 105:
                                                    case 106:
                                                    case 107:
                                                    case 108:
                                                    case 109:
                                                    case 110:
                                                    case 111:
                                                    case 112:
                                                    case 113:
                                                    case 114:
                                                    case 115:
                                                    case 116:
                                                    case 117:
                                                    case 118:
                                                    case 119:
                                                    case 120:
                                                    case 121:
                                                    case 126:
                                                    case 132:
                                                    case 133:
                                                    case 134:
                                                    case 135:
                                                    case 136:
                                                    case 137:
                                                    case 138:
                                                    case 139:
                                                        goto IL_ABF2;
                                                    case 85:
                                                        if (!Session.GetHabbo().HasFuse("cmd_handitem"))
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        if (class2 == null)
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        class2.method_56(Session.GetHabbo().Username).CarryItem(int.Parse(Params[1]));
                                                        result = true;
                                                        return result;
                                                    case 86:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_lay"))
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            Room currentRoom = Session.GetHabbo().CurrentRoom;
                                                            if (currentRoom == null)
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            RoomUser roomUserByHabbo2 = currentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            if (roomUserByHabbo2 == null)
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            if (!roomUserByHabbo2.Statusses.ContainsKey("lay"))
                                                            {
                                                                if (roomUserByHabbo2.BodyRotation % 2 == 0)
                                                                {
                                                                    roomUserByHabbo2.Statusses.Add("lay", Convert.ToString((double)Session.GetHabbo().CurrentRoom.Byte_0[roomUserByHabbo2.X, roomUserByHabbo2.Y] + 0.55).ToString().Replace(",", "."));
                                                                    roomUserByHabbo2.UpdateNeeded = true;
                                                                }
                                                                else
                                                                {
                                                                    Session.GetHabbo().Whisper("Du kannst nicht liegen, wenn du diagonal stehst.");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                roomUserByHabbo2.Statusses.Remove("lay");
                                                                roomUserByHabbo2.UpdateNeeded = true;
                                                            }
                                                            result = true;
                                                            return result;
                                                        }
                                                    case 94:
                                                        if (Session.GetHabbo().HasFuse("cmd_startquestion"))
                                                        {
                                                            if (Params[1] != null)
                                                            {
                                                                Room Room = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                DataTable Data = null;
                                                                int QuestionId = int.Parse(Params[1]);
                                                                Room.CurrentPollId = QuestionId;
                                                                string Question;
                                                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                                                {
                                                                    Question = dbClient.ReadString("SELECT question FROM infobus_questions WHERE id = '" + QuestionId + "' LIMIT 1", 30);
                                                                    Data = dbClient.ReadDataTable("SELECT * FROM infobus_answers WHERE question_id = '" + QuestionId + "'", 30);
                                                                }
                                                                ServerMessage InfobusQuestion = new ServerMessage(79u);
                                                                InfobusQuestion.AppendStringWithBreak(Question);
                                                                InfobusQuestion.AppendInt32(Data.Rows.Count);
                                                                if (Data != null)
                                                                {
                                                                    foreach (DataRow Row in Data.Rows)
                                                                    {
                                                                        InfobusQuestion.AppendInt32((int)Row["id"]);
                                                                        InfobusQuestion.AppendStringWithBreak((string)Row["answer_text"]);
                                                                    }
                                                                }
                                                                Room.SendMessage(InfobusQuestion, null);
                                                                Thread Infobus = new Thread(delegate ()
                                                                {
                                                                    Room.ShowResults(Room, QuestionId, Session);
                                                                });
                                                                Infobus.Start();
                                                                result = true;
                                                                return result;
                                                            }
                                                        }
                                                        result = false;
                                                        return result;
                                                    case 95:
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        if (class5.Boolean_3)
                                                        {
                                                            Session.GetHabbo().Whisper("Command unavailable while trading!");
                                                            result = true;
                                                            return result;
                                                        }
                                                        if (ServerConfiguration.EnableRedeemPixels)
                                                        {
                                                            Session.GetHabbo().GetInventoryComponent().RedeemPixel(Session);
                                                        }
                                                        else
                                                        {
                                                            Session.GetHabbo().Whisper(HabboIM.smethod_1("cmd_error_disabled"));
                                                        }
                                                        result = true;
                                                        return result;
                                                    case 96:
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        if (class5.Boolean_3)
                                                        {
                                                            Session.GetHabbo().Whisper("Command unavailable while trading!");
                                                            result = true;
                                                            return result;
                                                        }
                                                        if (ServerConfiguration.EnableRedeemShells)
                                                        {
                                                            Session.GetHabbo().GetInventoryComponent().RedeemShell(Session);
                                                        }
                                                        else
                                                        {
                                                            Session.GetHabbo().Whisper(HabboIM.smethod_1("cmd_error_disabled"));
                                                        }
                                                        result = true;
                                                        return result;
                                                    //Custom Commands
                                                    case 122:
                                                        result = false;
                                                        return result;
                                                    case 123:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_pet"))
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            string created = Session.GetHabbo().DataCadastro;
                                                            int createdint = Convert.ToInt32(created);
                                                            int createdcheck = createdint + 259200;
                                                            int timenow = (int)HabboIM.GetUnixTimestamp();
                                                            if (timenow <= createdcheck || Session.GetHabbo().AchievementScore < 600)
                                                            {
                                                                Session.SendNotification("Du musst mindestens 3 Tage im Habbo registriert sein und 600 Aktivitätspunkte haben um diesen Befehl ausführen zu können!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            string stringpet = ChatCommandHandler.MergeParams(Params, 1);
                                                            if (!(stringpet != ""))
                                                            {
                                                                Session.SendNotification("Bitte eine Haustier ID auswählen. Haustier ID findest du über den Command \":pet help\"");
                                                                result = true;
                                                                return result;
                                                            }
                                                            if (!(Params[1] != "help"))
                                                            {
                                                                Session.SendNotification("Haustiere ID:\n\n0 -> Hund\n1 -> Katze\n2 -> Krokodil\n3 -> Terrier\n4 -> Eisbär\n5 -> Schwein\n6 -> Löwe\n7 -> Nashorn\n8 -> Spinne\n9 -> Schildkröte\n10 -> Küken\n11 -> Frosch\n12 -> Drache\n13 -> Pferd\n14 -> Affe\n15 -> -\n16 -> Monsterpflanze\n17 -> Weißer Hase\n18 -> Böser Hase\n19 -> gelangweilter Hase\n20 -> verliebter Hase\n21 -> weiße Taube\n22 -> schwarze Taube\n23 -> Unbekannt\n24 -> Zwerg\n25 -> Baby Bär\n26 -> Baby Terrier\n27 -> Pikachu\n28 -> Mario\n29 -> Pinguin\n30 -> Elefant\n31 -> Haloompa\n32 -> Stein\n33 -> Flugsaurer\n34 -> Velociraptor\n35 -> Baby Katze\n36 -> Baby Hund\n\nUm zurück zu verwandeln: \":habbo\" Command benutzen.\n\nViel Spaß damit!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            int numpet = (int)Convert.ToInt16(Params[1]);
                                                            if ((numpet >= 0 && numpet <= 36) || (Session.GetHabbo().Rank >= 10u && numpet >= 0 && numpet <= 50))
                                                            {
                                                                int UserPetId = int.Parse(Params[1]);
                                                                Session.GetHabbo().PetData = UserPetId + " 0 #fff";
                                                                Session.SendNotification("Du bist jetzt ein Tier! Bitte lade den Raum neu, damit die Änderungen wirksam werden.\n\nFalls du kein Tier mehr sein möchtest musst du :habbo eingeben und den Raum neuladen.");
                                                                result = true;
                                                                return result;
                                                            }
                                                            Session.GetHabbo().Whisper("Haustier ID darf nur 0 bis 36 haben!");
                                                            result = true;
                                                            return result;
                                                        }
                                                    case 124:
                                                        if (Session.GetHabbo().PetData != null)
                                                        {
                                                            Session.GetHabbo().PetData = null;
                                                            Session.SendNotification("Du bist jetzt ein normaler Habbo. Bitte Raum neu laden!");
                                                            result = true;
                                                            return result;
                                                        }
                                                        Session.GetHabbo().Whisper("Du bist schon ein normaler Habbo.");
                                                        result = true;
                                                        return result;
                                                    case 125:
                                                        using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
                                                        {
                                                            if (Session.GetHabbo().Rank == 6u)
                                                            {
                                                                if (Session.GetHabbo().GetEffectsInventoryComponent().int_0 != 650)
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(650, true);
                                                                    Session.GetHabbo().Whisper("Du wurdest als MOD markiert. Wenn die Markierung aufgehoben werden soll, dann sollst du nochmal :mark eingeben.");
                                                                }


                                                                else
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                }
                                                                result = true;
                                                                return result;
                                                            }
                                                            if (Session.GetHabbo().Rank >= 7u)
                                                            {
                                                                if (Session.GetHabbo().GetEffectsInventoryComponent().int_0 != 650)
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(650, true);
                                                                    Session.GetHabbo().Whisper("Du wurdest als STAFF markiert. Wenn die Markierung aufgehoben werden soll, dann sollst du nochmal :mark eingeben.");
                                                                }
                                                                else
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                }
                                                                result = true;
                                                                return result;
                                                            }

                                                            result = false;
                                                            return result;
                                                        }





                                                    case 55356:
                                                        {

                                                            if (Session.GetHabbo().Rank < 6)
                                                            {


                                                                return false;
                                                            }




                                                            if (Session.GetHabbo().whisperlog == true)
                                                            {
                                                                Session.GetHabbo().Whisper("Der Flüsterchatlog ist nun für dich ausgeschaltet.");
                                                                Session.GetHabbo().whisperlog = false;


                                                                return true;
                                                            }



                                                            if (Session.GetHabbo().whisperlog == false)
                                                            {
                                                                Session.GetHabbo().Whisper("Der Flüsterchatlog ist nun für dich eingeschaltet.");
                                                                Session.GetHabbo().whisperlog = true;


                                                                return true;
                                                            }







                                                            return true;

                                                        }


                                                    case 127:
                                                        {
                                                            if (Session.GetHabbo().Rank > 1u)
                                                            {
                                                                string Message = Input.Substring(5).ToLower();

                                                                if (Message == "1")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(613, true);
                                                                }
                                                                else if (Message == "2")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(614, true);
                                                                }
                                                                else if (Message == "3")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(526, true);
                                                                }
                                                                else if (Message == "4")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(606, true);
                                                                }
                                                                else if (Message == "0")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                }
                                                                else
                                                                {
                                                                    Session.GetHabbo().Whisper("Wähle eine ID zwischen 1-4 aus!");
                                                                }

                                                                return true;
                                                            }

                                                            return false;
                                                        }


                                                    case 128:
                                                        TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Session.GetHabbo().Username);
                                                        if (!isBrb.ContainsKey((int)Session.GetHabbo().Id))
                                                        {
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                            RoomUser talk = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);

                                                            isBrb.Add((int)Session.GetHabbo().Id, true);

                                                            if (Params.Length >= 2)
                                                            {
                                                                talk.HandleSpeech(TargetClient, "Bin gleich zurück! (Grund: " + MergeParams(Params, 1) + ")", false);
                                                                class4.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(500, true);
                                                            }
                                                            else
                                                            {
                                                                talk.HandleSpeech(TargetClient, "Bin gleich wieder da!", false);
                                                                class4.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(500, true);
                                                            }

                                                            System.Timers.Timer st = new System.Timers.Timer();
                                                            st.Interval = TimeSpan.FromSeconds(120).TotalMilliseconds;
                                                            st.Elapsed += (ss, ee) =>
                                                            {
                                                                if (isBrb.ContainsKey((int)Session.GetHabbo().Id) && class2.RoomUsers.Contains(class2.GetRoomUserByHabbo(Session.GetHabbo().Id)))
                                                                {
                                                                    if (Params.Length >= 2)
                                                                    {
                                                                        talk.HandleSpeech(TargetClient, "Bin gleich wieder da! (Grund: " + MergeParams(Params, 1) + ")", false);
                                                                    }
                                                                    else
                                                                    {
                                                                        talk.HandleSpeech(TargetClient, "Bin gleich wieder da!", false);

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    st.Stop();
                                                                    if (Session.GetHabbo() != null)
                                                                        isBrb.Remove((int)Session.GetHabbo().Id);
                                                                }
                                                            };
                                                            st.Enabled = true;
                                                            st.AutoReset = true;
                                                            st.Start();
                                                            return true;
                                                        }
                                                        else
                                                        {
                                                            Session.SendNotification("Du bist bereits weg, komm doch erstmal wieder zurück! (:wd).");
                                                            return true;
                                                        }

                                                    case 2090:
                                                        if (isBrb.ContainsKey((int)Session.GetHabbo().Id))
                                                        {
                                                            TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Session.GetHabbo().Username);
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                            RoomUser talk2 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);

                                                            talk2.HandleSpeech(TargetClient, "Wieder da!", false);
                                                            class4.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                            isBrb.Remove((int)Session.GetHabbo().Id);
                                                            return true;
                                                        }
                                                        else
                                                        {
                                                            Session.SendNotification("Du bist gerade nicht afk, du kannst andere Hobbas wissen lassen das du afk bist mit: :afk GRUND (Grund ist nicht notwendig)");
                                                            return true;
                                                        }


                                                    case 129:
                                                        {
                                                            int numzahl = (int)Convert.ToInt16(Params[1]);
                                                            if (numzahl < 0 || numzahl > 10)
                                                            {
                                                                Session.GetHabbo().Whisper("Es gibt nur von 0 bis 10 Zahlen!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            if (numzahl >= 0 && numzahl <= 9)
                                                            {
                                                                int zahlmark = 510 + numzahl;
                                                                if (Session.GetHabbo().GetEffectsInventoryComponent().int_0 != zahlmark)
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(zahlmark, true);
                                                                    Thread threadzahl = new Thread(delegate ()
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
                                                                    threadzahl.Start();
                                                                    result = true;
                                                                    return result;
                                                                }
                                                                result = true;
                                                                return result;
                                                            }
                                                            else
                                                            {
                                                                if (numzahl != 10)
                                                                {
                                                                    result = true;
                                                                    return result;
                                                                }
                                                                if (Session.GetHabbo().GetEffectsInventoryComponent().int_0 != 520)
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(520, true);
                                                                    Thread threadzahl = new Thread(delegate ()
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
                                                                    threadzahl.Start();
                                                                    result = true;
                                                                    return result;
                                                                }
                                                                result = true;
                                                                return result;
                                                            }
                                                            
                                                        }
                                                    case 130:
                                                        try
                                                        {
                                                            GameClient User2 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                            RoomUser roomUser2 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);
                                                            RoomUser roomUser = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);
                                                            if (User2.GetHabbo().CurrentRoomId != Session.GetHabbo().CurrentRoomId)
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            if (User2.GetHabbo().Id == Session.GetHabbo().Id)
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            bool flag3 = false;
                                                            using (DatabaseClient client2 = HabboIM.GetDatabase().GetClient())
                                                            {
                                                                DataTable dataTable = client2.ReadDataTable("SELECT * FROM messenger_friendships", 30);
                                                                foreach (DataRow dataRow in dataTable.Rows)
                                                                {
                                                                    if (((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)User2.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)Session.GetHabbo().Id)) || ((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)Session.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)User2.GetHabbo().Id)))
                                                                    {
                                                                        flag3 = true;
                                                                    }
                                                                }
                                                            }
                                                            if (!flag3)
                                                            {
                                                                Session.GetHabbo().Whisper("Wenn ihr euch küssen wollt, müsst ihr Freunde sein.");
                                                                result = true;
                                                                return result;
                                                            }
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            RoomUser roomUserByHabbo3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            RoomUser roomUserByHabbo4 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);
                                                            if (Math.Abs(roomUserByHabbo3.X - roomUserByHabbo4.X) < 3 && Math.Abs(roomUserByHabbo3.Y - roomUserByHabbo4.Y) < 3)
                                                            {


                                                                TargetClient = User2;

                                                                roomUserByHabbo3.HandleSpeech(Session, "*küsst " + User2.GetHabbo().Username + "*", false);
                                                                try
                                                                {


                                                                    if (TargetClient.GetHabbo().boyfriend == Session.GetHabbo().Id && TargetClient.GetHabbo().Id == Session.GetHabbo().boyfriend && TargetClient.GetHabbo().kisstime < HabboIM.GetUnixTimestamp() - 60 * 10 && Session.GetHabbo().kisstime < HabboIM.GetUnixTimestamp() - 60 * 10)
                                                                    {

                                                                        try
                                                                        {
                                                                            TargetClient.GetHabbo().kisstime = HabboIM.GetUnixTimestamp();
                                                                            Session.GetHabbo().kisstime = HabboIM.GetUnixTimestamp();
                                                                        }
                                                                        catch (Exception x)
                                                                        {
                                                                            //      Session.SendNotification(x.ToString());
                                                                        }
                                                                        try
                                                                        {
                                                                            HabboIM.GetWebSocketManager().getWebSocketByName(TargetClient.GetHabbo().Username).Send("kisshug|" + Session.GetHabbo().Username + " hat dich geküsst!|2|Beziehungspunkte");
                                                                            HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("kisshug|Du hast " + TargetClient.GetHabbo().Username + " geküsst!|2|Beziehungspunkte");

                                                                            Session.GetHabbo().lovepoints += 2;
                                                                            TargetClient.GetHabbo().lovepoints += 2;
                                                                            Session.GetHabbo().kissed += 1;
                                                                            TargetClient.GetHabbo().kissed += 1;
                                                                            Session.GetHabbo().UpdateTime();

                                                                        }
                                                                        catch (Exception x)
                                                                        {
                                                                            ///        Session.SendNotification(x.ToString());
                                                                        }
                                                                    }
                                                                    if (TargetClient.GetHabbo().boyfriend == Session.GetHabbo().Id && TargetClient.GetHabbo().Id == Session.GetHabbo().boyfriend)
                                                                    {
                                                                        HabboIM.GetGame().GetClientManager().method_checkstats(Session);
                                                                        HabboIM.GetGame().GetClientManager().method_checkstats(TargetClient);
                                                                    }


                                                                }
                                                                catch (Exception x)
                                                                {
                                                                    //     Session.SendNotification(x.ToString());
                                                                }


                                                                Session.GetHabbo().GetEffectsInventoryComponent().method_2(168, true);
                                                                User2.GetHabbo().GetEffectsInventoryComponent().method_2(168, true);
                                                                Thread thread = new Thread(delegate ()
                                                                {
                                                                    try
                                                                    {
                                                                        Thread.Sleep(3000);
                                                                        Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                        User2.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                    }
                                                                    catch
                                                                    {
                                                                    }
                                                                });
                                                                thread.Start();
                                                                result = true;
                                                                return result;
                                                            }
                                                            Session.GetHabbo().Whisper("Ihr seid zu weit weg voneinander.");
                                                            result = true;
                                                            return result;
                                                        }
                                                        catch
                                                        {
                                                            result = false;
                                                            return result;
                                                        }




                                                    case 66666:

                                                        {


                                                            TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());



                                                            if (TargetClient == null)
                                                            {

                                                                Session.GetHabbo().Whisper("Der User existiert nicht.");
                                                                return true;
                                                            }
                                                            if (Session.GetHabbo().boyfriend != 0 || TargetClient.GetHabbo().boyfriend != 0)
                                                            {
                                                                Session.GetHabbo().Whisper("Du oder " + TargetClient.GetHabbo().Username + " seid bereits in einer Beziehung.");
                                                                return true;

                                                            }

                                                            if (Session == TargetClient)
                                                            {
                                                                Session.GetHabbo().Whisper("Du kannst nicht mit dir selber zusammen sein.");
                                                                return true;

                                                            }
                                                            Session.GetHabbo().sexanfrage = TargetClient.GetHabbo().Id;



                                                            HabboIM.GetWebSocketManager().getWebSocketByName(TargetClient.GetHabbo().Username).Send("sexanfrage|" + Session.GetHabbo().Id + "");
                                                            return true;
                                                        }

                                                    case 131:
                                                        break;
                                                    case 140:
                                                        goto IL_8648;
                                                    case 141:
                                                        if (Session.GetHabbo().Rank < 6 && Session.GetHabbo().CurrentRoom.Owner != Session.GetHabbo().Username)
                                                        {
                                                            return false;
                                                        }
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        if (class2 == null)
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        for (int i = 0; i < class2.RoomUsers.Length; i++)
                                                        {
                                                            RoomUser class10 = class2.RoomUsers[i];
                                                            if (class10 != null)
                                                            {
                                                                class10.bool_5 = !class10.bool_5;
                                                                if (class10.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0 != 12)
                                                                {
                                                                    class10.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(12, true);
                                                                }
                                                                else
                                                                {
                                                                    class10.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                }
                                                            }
                                                        }
                                                        HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                        result = true;
                                                        return result;
                                                    case 142:
                                                        if (!Session.GetHabbo().HasFuse("cmd_sellroom"))
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        if (class2 != null && class2.CheckRights(Session, true))
                                                        {
                                                            try
                                                            {
                                                                int CostCheckMinus = int.Parse(Params[1]);
                                                                if (CostCheckMinus >= 0)
                                                                {
                                                                    if (class2.CanBuy)
                                                                    {
                                                                        class2.CanBuy = false;
                                                                        class2.CanBuyCheck = false;
                                                                        Session.SendNotification("Der Raumverkauf wurde beendet.");
                                                                    }
                                                                    else if (class2.CanBuyCheck)
                                                                    {
                                                                        int CanBuyCheckTime = class2.CanBuyCheckTime + 300;
                                                                        int TimeNow = (int)HabboIM.GetUnixTimestamp();
                                                                        if (CanBuyCheckTime > TimeNow)
                                                                        {
                                                                            int CostCheck = int.Parse(Params[1]);
                                                                            if (class2.RoomCost == CostCheck)
                                                                            {
                                                                                class2.CanBuy = true;
                                                                                Session.SendNotification("Der Raum kann nun gekauft werden! Wenn der Raumverkauf beendet soll, dann Tippe :sellroom ein.");
                                                                                for (int j = 0; j < class2.RoomUsers.Length; j++)
                                                                                {
                                                                                    RoomUser roomUser3 = class2.RoomUsers[j];
                                                                                    if (roomUser3 != null)
                                                                                    {
                                                                                        roomUser3.GetClient().SendNotification("Dieser Raum wird für " + class2.RoomCost + " Taler verkauft!\n\nWenn du den Raum kaufen willst, dann Tippe :buyroom ein.");
                                                                                    }
                                                                                }
                                                                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), string.Concat(new object[]
                                                            {
                                                                "Raum (ID ",
                                                                class2.Id,
                                                                ") für ",
                                                                class2.RoomCost,
                                                                " verkaufen"
                                                            }));
                                                                            }
                                                                            else
                                                                            {
                                                                                class2.CanBuyCheck = false;
                                                                                Session.SendNotification("Die Preis stimmen nicht mit vorherige Preis überein. Geben Sie erneut ein.");
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            class2.CanBuyCheck = false;
                                                                            Session.SendNotification("Die Zeit ist abgelaufen. Bitte geben Sie nochmal ein.");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        class2.RoomCost = int.Parse(Params[1]);
                                                                        class2.CanBuyCheck = true;
                                                                        class2.CanBuyCheckTime = (int)HabboIM.GetUnixTimestamp();
                                                                        Session.SendNotification(string.Concat(new object[]
                                                    {
                                                        "Bist du dir sicher, dieser Raum für ",
                                                        class2.RoomCost,
                                                        " Taler zu verkaufen? Wenn ja, geben sie nochmal \":sellroom ",
                                                        class2.RoomCost,
                                                        " \" ein.\n\nWichtig: Du kannst verkaufte Räume nicht wieder zurück bekommen!"
                                                    }));
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Session.SendNotification("Die Zahl darf kein Minus haben!");
                                                                }
                                                            }
                                                            catch
                                                            {
                                                                Session.SendNotification("Es ist ein Fehler aufgetreten. Bitte geben Sie :sellroom [Preis] ein.");
                                                            }
                                                        }
                                                        result = true;
                                                        return result;
                                                    case 143:
                                                        if (!Session.GetHabbo().HasFuse("cmd_buyroom"))
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        if (class2.CanBuy)
                                                        {
                                                            if (Session.GetHabbo().Credits >= class2.RoomCost)
                                                            {
                                                                class2.CanBuy = false;
                                                                class2.CanBuyCheck = false;
                                                                int credits = Session.GetHabbo().Credits - class2.RoomCost;
                                                                Session.GetHabbo().Credits = credits;
                                                                Session.GetHabbo().UpdateCredits(true);
                                                                using (DatabaseClient client3 = HabboIM.GetDatabase().GetClient())
                                                                {
                                                                    GameClient clientByHabbo3 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(class2.Owner);
                                                                    int credits2 = clientByHabbo3.GetHabbo().Credits + class2.RoomCost;
                                                                    clientByHabbo3.GetHabbo().Credits = credits2;
                                                                    clientByHabbo3.GetHabbo().UpdateCredits(true);
                                                                    client3.ExecuteQuery(string.Concat(new object[]
                                                {
                                                    "UPDATE rooms SET owner = '",
                                                    Session.GetHabbo().Username,
                                                    "' WHERE id = ",
                                                    class2.Id,
                                                    " LIMIT 1"
                                                }), 30);
                                                                }
                                                                for (int j = 0; j < class2.RoomUsers.Length; j++)
                                                                {
                                                                    RoomUser roomUser3 = class2.RoomUsers[j];
                                                                    if (roomUser3 != null)
                                                                    {
                                                                        roomUser3.GetClient().SendNotification(string.Concat(new string[]
                                                    {
                                                        "Dieser Raum wurde gerade an ",
                                                        Session.GetHabbo().Username,
                                                        " verkauft! Alle wurden aus dem Raum gekickt, damit der Raumbesitzer aktualisiert wird "

                                                    }));
                                                                    }
                                                                }
                                                                HabboIM.GetGame().GetRoomManager().method_16(class2);
                                                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), string.Concat(new object[]
                                            {
                                                "Raum (ID ",
                                                class2.Id,
                                                ") für ",
                                                class2.RoomCost,
                                                " Taler gekauft"
                                            }));
                                                            }
                                                            else
                                                            {
                                                                Session.SendNotification("Du hast zu wenig Taler!");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Session.SendNotification("Dieser Raum wird nicht verkauft!");
                                                        }
                                                        result = true;
                                                        return result;
                                                    case 144:
                                                        if (!Session.GetHabbo().HasFuse("cmd_roomcredits"))
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        if (class2 == null)
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        for (int i = 0; i < class2.RoomUsers.Length; i++)
                                                        {
                                                            RoomUser class6 = class2.RoomUsers[i];
                                                            int num8 = int.Parse(Params[1]);
                                                            if (class6 != null)
                                                            {
                                                                class6.GetClient().GetHabbo().Credits = class6.GetClient().GetHabbo().Credits + num8;
                                                                class6.GetClient().GetHabbo().UpdateCredits(true);
                                                                class6.GetClient().SendNotification(Session.GetHabbo().Username + " hat dir " + num8.ToString() + " Taler gegeben!");
                                                            }
                                                        }
                                                        Session.SendNotification("Taler erfolgreich verteilt.");
                                                        HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                        result = true;
                                                        return result;
                                                    case 145:
                                                        if (!Session.GetHabbo().HasFuse("cmd_roompixels"))
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        if (class2 == null)
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        for (int i = 0; i < class2.RoomUsers.Length; i++)
                                                        {
                                                            RoomUser class6 = class2.RoomUsers[i];
                                                            int num8 = int.Parse(Params[1]);
                                                            if (class6 != null)
                                                            {
                                                                class6.GetClient().GetHabbo().ActivityPoints = class6.GetClient().GetHabbo().ActivityPoints + num8;
                                                                class6.GetClient().GetHabbo().UpdateActivityPoints(true);
                                                                class6.GetClient().SendNotification(Session.GetHabbo().Username + " hat dir " + num8.ToString() + " Enten gutgeschrieben!");
                                                            }
                                                        }
                                                        Session.SendNotification("Enten verteilt!");
                                                        HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                        result = true;
                                                        return result;
                                                    case 146:
                                                        if (!Session.GetHabbo().HasFuse("cmd_roompoints"))
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        if (class2 == null)
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        for (int i = 0; i < class2.RoomUsers.Length; i++)
                                                        {
                                                            RoomUser class6 = class2.RoomUsers[i];
                                                            int num8 = int.Parse(Params[1]);
                                                            if (class6 != null)
                                                            {
                                                                class6.GetClient().GetHabbo().VipPoints = class6.GetClient().GetHabbo().VipPoints + num8;
                                                                class6.GetClient().GetHabbo().UpdateVipPoints(false, true);
                                                                class6.GetClient().SendNotification(Session.GetHabbo().Username + " hat dir " + num8.ToString() + " Sterne gegeben!");
                                                            }
                                                        }
                                                        Session.SendNotification("Sterne erfolgreich verteilt.");
                                                        HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                        result = true;
                                                        return result;
                                                    case 147:
                                                        if (!Session.GetHabbo().HasFuse("cmd_roomdc"))
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        if (class2 == null)
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        for (int i = 0; i < class2.RoomUsers.Length; i++)
                                                        {
                                                            RoomUser class6 = class2.RoomUsers[i];
                                                            if (class6 != null && class6.GetClient().GetHabbo().Rank < Session.GetHabbo().Rank)
                                                            {
                                                                string text = class6.GetClient().GetHabbo().Username;
                                                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text);
                                                                TargetClient.method_12();
                                                            }
                                                        }
                                                        Session.SendNotification("Alle User im Raum erfolgreich disconnected.");
                                                        HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                        result = true;
                                                        return result;
                                                    case 148:
                                                        if (!Session.GetHabbo().HasFuse("cmd_raumkick"))
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        if (!(Session.GetHabbo().CurrentRoom.Owner == Session.GetHabbo().Username))
                                                        {
                                                            Session.SendNotification("Du kannst das nur in deinen eigenen Räumen machen!");
                                                            result = true;
                                                            return result;
                                                        }
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        if (class2 == null)
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        string_ = ChatCommandHandler.MergeParams(Params, 1);
                                                        if (string_ != "")
                                                        {
                                                            for (int i = 0; i < class2.RoomUsers.Length; i++)
                                                            {
                                                                RoomUser roomUser4 = class2.RoomUsers[i];
                                                                if (roomUser4 != null && roomUser4.GetClient().GetHabbo().Rank < 4u && roomUser4.GetClient().GetHabbo().Username != Session.GetHabbo().CurrentRoom.Owner)
                                                                {
                                                                    string roomowner = Session.GetHabbo().CurrentRoom.Owner;
                                                                    string message = string.Concat(new object[]
                                                {
                                                    "Du wurdest vom Raumbesitzer ",
                                                    roomowner,
                                                    " mit folgender Begründung gekickt:\n\n ",
                                                    string_
                                                });
                                                                    roomUser4.GetClient().SendNotification(message);
                                                                    class2.method_47(roomUser4.GetClient(), true, false);
                                                                }
                                                            }
                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            result = true;
                                                            return result;
                                                        }
                                                        Session.SendNotification("Du musst dazu eine Nachricht schreiben bzw. die Begründung.");
                                                        result = true;
                                                        return result;

                                                    case 149:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_verwarnung"))
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            string werber = Params[1];
                                                            string stringwerber = ChatCommandHandler.MergeParams(Params, 1);
                                                            if (stringwerber == "")
                                                            {
                                                                Session.SendNotification("Bitte einen Username angeben!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            string username = Session.GetHabbo().Username;
                                                            if (werber == username)
                                                            {
                                                                Session.SendNotification("Du kannst dich nicht selber verwarnen!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            GameClient class7 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(werber);
                                                            Habbo class8;
                                                            if (class7 == null)
                                                            {
                                                                class8 = Authenticator.CreateHabbo(werber);
                                                            }
                                                            else
                                                            {
                                                                class8 = class7.GetHabbo();
                                                            }
                                                            if (class8 == null)
                                                            {
                                                                Session.SendNotification("User konnte nicht gefunden werden: " + Params[1]);
                                                                result = true;
                                                                return result;
                                                            }
                                                            if (Session.GetHabbo().Rank < class8.Rank)
                                                            {
                                                                Session.SendNotification("Zugriff verweigert!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            string stringreason = ChatCommandHandler.MergeParams(Params, 2);
                                                            if (stringreason == "")
                                                            {
                                                                Session.SendNotification("Du musst einen Grund angeben!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            string reason = ChatCommandHandler.MergeParams(Params, 2);
                                                            ServerMessage Logging = new ServerMessage(134u);
                                                            Logging.AppendUInt(0u);
                                                            Logging.AppendString(string.Concat(new string[]
                                    {
                                        "VERWARNUNG: \"",
                                        Session.GetHabbo().Username,
                                        "\" hat den Spieler \"",
                                        class8.Username,
                                        "\" mit der Begründung \"",
                                        reason,
                                        "\" verwarnt."
                                    }));
                                                            HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                                            TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(class8.Username);
                                                            TargetClient.SendNotification(string.Concat(new string[]
                                    {
                                        "Du wurdest von einem Staff verwarnt!\r\rVerwarnt von: ",
                                        Session.GetHabbo().Username,
                                        "\rGrund: ",
                                        reason,
                                        "\r\rBei 3/3 Adminverwarnungen wirst du dauerhaft aus dem Hotel gebannt!"
                                    }));
                                                            double unixTimestamp = HabboIM.GetUnixTimestamp();
                                                            using (DatabaseClient clientverwarnung = HabboIM.GetDatabase().GetClient())
                                                            {
                                                                clientverwarnung.ExecuteQuery(string.Concat(new object[]
                                        {
                                            "INSERT INTO user_warning (`user_id`,`reason`,`timestamp`) VALUES ('",
                                            class8.Id,
                                            "','",
                                            reason,
                                            "','",
                                            unixTimestamp,
                                            "');"
                                        }), 30);
                                                            }
                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            result = true;
                                                            return result;
                                                        }
                                                    case 150:
                                                        result = false;
                                                        return result;
                                                    case 151:
                                                        result = false;
                                                        return result;
                                                    case 152:
                                                        {
                                                            if (!Session.GetHabbo().WerberCmd)
                                                            {
                                                                Session.SendNotification("Aufgrund eines Regelverstoßes wurde dir dieser Befehl entzogen!");
                                                                result = true;
                                                                return result;
                                                            }

                                                            if (HabboIM.GetUnixTimestamp() < Session.GetHabbo().werber_time)
                                                            {
                                                                Session.GetHabbo().Whisper("Du hast diesen Command eben erst ausgeführt. Gedulde dich jetzt etwas.");
                                                                return true;
                                                            }

                                                            string werber = Params[1];
                                                            string stringwerber = ChatCommandHandler.MergeParams(Params, 1);
                                                            if (stringwerber == "")
                                                            {
                                                                Session.SendNotification("Bitte einen Username eingeben!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            string username = Session.GetHabbo().Username;
                                                            if (werber == username)
                                                            {
                                                                Session.SendNotification("Du kannst dich nicht selber als Werber alamieren!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            GameClient class7 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(werber);
                                                            Habbo class8;
                                                            if (class7 == null)
                                                            {
                                                                class8 = Authenticator.CreateHabbo(werber);
                                                            }
                                                            else
                                                            {
                                                                class8 = class7.GetHabbo();
                                                            }
                                                            if (class8 == null)
                                                            {
                                                                Session.SendNotification("User konnte nicht gefunden werden: " + Params[1]);
                                                                result = true;
                                                                return result;
                                                            }
                                                            ServerMessage Logging = new ServerMessage(134u);
                                                            Logging.AppendUInt(0u);
                                                            Logging.AppendString(string.Concat(new string[]
                                    {
                                        "ALARM: Der Spieler \"",
                                        Session.GetHabbo().Username,
                                        "\" (Raum: ",
                                        Session.GetHabbo().CurrentRoom.Name,
                                        ") hat den User \"",
                                        class8.Username,
                                        "\" (Raum: ",
                                        class8.CurrentRoom.Name,
                                        ") als Werber alamiert. Bitte prüfe alle Chatlogs!"
                                    }));

                                                            Session.GetHabbo().werber_time = HabboIM.GetUnixTimestamp() + 60;
                                                            HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                                            result = true;
                                                            return result;
                                                        }
                                                    case 153:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_myteleport"))
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            if (!(Session.GetHabbo().CurrentRoom.Owner == Session.GetHabbo().Username))
                                                            {
                                                                Session.SendNotification("Du kannst dich nur in deinen eigenen Räumen teleportieren!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            if (class2 == null)
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            if (class3 == null)
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            if (class3.TeleportMode)
                                                            {
                                                                class3.TeleportMode = false;
                                                                Session.SendNotification("Teleportation deaktiviert!");
                                                            }
                                                            else
                                                            {
                                                                class3.TeleportMode = true;
                                                                Session.SendNotification("Teleportation aktiviert!");
                                                            }
                                                            class2.method_22();
                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            result = true;
                                                            return result;
                                                        }
                                                    case 154:
                                                        Session.SendNotification("Lotto\n~~~~~~\n\nComing soon ... !", 2);
                                                        result = true;
                                                        return result;
                                                    case 155:
                                                        if (!Session.GetHabbo().HasFuse("cmd_makesayall"))
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        if (class2 == null)
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        string_ = ChatCommandHandler.MergeParams(Params, 1);
                                                        if (string_ != "")
                                                        {
                                                            for (int i = 0; i < class2.RoomUsers.Length; i++)
                                                            {
                                                                RoomUser roomUser4 = class2.RoomUsers[i];
                                                                if (roomUser4 != null)
                                                                {
                                                                    TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(roomUser4.GetClient().GetHabbo().Username);
                                                                    roomUser4.HandleSpeech(TargetClient, string_, false);
                                                                }
                                                            }
                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            result = true;
                                                            return result;
                                                        }
                                                        Session.SendNotification("Der Text darf nicht leer sein!");
                                                        result = true;
                                                        return result;
                                                    case 156:
                                                        if (!Session.GetHabbo().HasFuse("cmd_roomaward"))
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        if (class2 == null)
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        for (int i = 0; i < class2.RoomUsers.Length; i++)
                                                        {
                                                            RoomUser class6 = class2.RoomUsers[i];
                                                            int num8 = int.Parse(Params[1]);
                                                            if (class6 != null)
                                                            {
                                                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(class6.GetClient().GetHabbo().Username);
                                                                HabboIM.GetGame().GetAchievementManager().addAchievement(TargetClient, Convert.ToUInt32(ChatCommandHandler.MergeParams(Params, 1)));
                                                            }
                                                        }
                                                        Session.SendNotification("Bonusbadge im Raum erfolgreich verteilt.");
                                                        HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                        result = true;
                                                        return result;
                                                    case 157:
                                                        if (Session.GetHabbo().HasFuse("cmd_massaward"))
                                                        {
                                                            HabboIM.GetGame().GetClientManager().method_New1(Params[1]);
                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            result = true;
                                                            return result;
                                                        }
                                                        result = false;
                                                        return result;
                                                    case 158:
                                                        if (Session.GetHabbo().HasFuse("cmd_summonall"))
                                                        {
                                                            HabboIM.GetGame().GetClientManager().method_New2(Session.GetHabbo().Username);
                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            result = true;
                                                            return result;
                                                        }
                                                        result = false;
                                                        return result;
                                                    case 159:
                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        if (class2.CanBuy)
                                                        {
                                                            Session.SendNotification("Dieser Raum wird für " + class2.RoomCost + " Taler verkauft!\n\nWenn du den Raum kaufen willst, dann tipp :buyroom ein.");
                                                        }
                                                        else
                                                        {
                                                            Session.SendNotification("Dieser Raum wird nicht verkauft!");
                                                        }
                                                        result = true;
                                                        return result;
                                                    case 160:
                                                        if (Session.GetHabbo().HasFuse("cmd_summonstaff"))
                                                        {
                                                            HabboIM.GetGame().GetClientManager().method_New3(Session.GetHabbo().Username);
                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            result = true;
                                                            return result;
                                                        }
                                                        result = false;
                                                        return result;
                                                    case 161:
                                                        if (!Session.GetHabbo().HasFuse("cmd_antiwerber"))
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        if (HabboIM.GetGame().AntiWerberStatus)
                                                        {
                                                            HabboIM.GetGame().AntiWerberStatus = false;
                                                            Session.SendNotification("AntiWerber System ist deaktiviert!");
                                                            ServerMessage Logging = new ServerMessage(134u);
                                                            Logging.AppendUInt(0u);
                                                            Logging.AppendString("Achtung: AntiWerber System ist deaktiviert!");
                                                            HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                                        }
                                                        else
                                                        {
                                                            HabboIM.GetGame().AntiWerberStatus = true;
                                                            Session.SendNotification("AntiWerber System ist aktiviert!");
                                                            ServerMessage Logging = new ServerMessage(134u);
                                                            Logging.AppendUInt(0u);
                                                            Logging.AppendString("Achtung: AntiWerber System ist aktiviert!");
                                                            HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                                        }
                                                        result = true;
                                                        return result;
                                                    case 162:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_antiwerberreset"))
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            string werber = Params[1];
                                                            string stringwerber = ChatCommandHandler.MergeParams(Params, 1);
                                                            if (stringwerber == "")
                                                            {
                                                                Session.SendNotification("Bitte einen Username eingeben!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            GameClient class7 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(werber);
                                                            Habbo class8;
                                                            if (class7 == null)
                                                            {
                                                                class8 = Authenticator.CreateHabbo(werber);
                                                            }
                                                            else
                                                            {
                                                                class8 = class7.GetHabbo();
                                                            }
                                                            if (class8 == null)
                                                            {
                                                                Session.SendNotification("User konnte nicht gefunden werden: " + Params[1]);
                                                                result = true;
                                                                return result;
                                                            }
                                                            TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(class8.Username);
                                                            TargetClient.GetHabbo().WerberWarnungOne = false;
                                                            TargetClient.GetHabbo().WerberWarnungTwo = false;
                                                            TargetClient.GetHabbo().WerberWarnungThree = false;
                                                            TargetClient.SendNotification("Deine AWS-Verwarnungen wurden von einem Staff entfernt.");
                                                            ServerMessage Logging = new ServerMessage(134u);
                                                            Logging.AppendUInt(0u);
                                                            Logging.AppendString(string.Concat(new string[]
                                    {
                                        "Info: ",
                                        Session.GetHabbo().Username,
                                        " hat die Verwarnungen von \"",
                                        TargetClient.GetHabbo().Username,
                                        "\" geleert!"
                                    }));
                                                            HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                                            result = true;
                                                            return result;
                                                        }





                                                    case 16299:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_antiwerberreset"))
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            string werber = Params[1];
                                                            string stringwerber = ChatCommandHandler.MergeParams(Params, 1);
                                                            if (stringwerber == "")
                                                            {
                                                                Session.SendNotification("Bitte einen Username eingeben!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            GameClient class7 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(werber);
                                                            Habbo class8;
                                                            if (class7 == null)
                                                            {
                                                                class8 = Authenticator.CreateHabbo(werber);
                                                            }
                                                            else
                                                            {
                                                                class8 = class7.GetHabbo();
                                                            }
                                                            if (class8 == null)
                                                            {
                                                                Session.SendNotification("User konnte nicht gefunden werden: " + Params[1]);
                                                                result = true;
                                                                return result;
                                                            }
                                                            TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(class8.Username);
                                                            TargetClient.GetHabbo().BeleidigungWarnungOne = false;
                                                            TargetClient.GetHabbo().BeleidigungWarnungTwo = false;
                                                            TargetClient.GetHabbo().BeleidigungWarnungThree = false;
                                                            TargetClient.SendNotification("Deine Verwarnungen wurden entfernt!");
                                                            ServerMessage Logging = new ServerMessage(134u);
                                                            Logging.AppendUInt(0u);
                                                            Logging.AppendString(string.Concat(new string[]
                                    {
                                        "Info: ",
                                        Session.GetHabbo().Username,
                                        " hat die Beleidigungsverwarnungen von \"",
                                        TargetClient.GetHabbo().Username,
                                        "\" geleert!"
                                    }));
                                                            HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                                            result = true;
                                                            return result;
                                                        }


                                                    case 163:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_roomitem"))
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            string itemid = Params[1];
                                                            string item_id = ChatCommandHandler.MergeParams(Params, 1);
                                                            if (item_id == "")
                                                            {
                                                                Session.SendNotification("Bitte ein Item ID von Tabelle \"catalog_items\" Spalte \"id\" eingeben!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            Session.SendNotification(":roomitem [CATALOG_ITEMS ID] - Coming Soon");
                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            result = true;
                                                            return result;
                                                        }
                                                    case 164:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_timermute"))
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            string TargetUser = Params[1];
                                                            int timer = int.Parse(Params[2]);
                                                            string reason = Params[3];
                                                            TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(TargetUser);
                                                            if (TargetClient == null)
                                                            {
                                                                Session.SendNotification("User konnte nicht gefunden werden: " + TargetUser);
                                                                result = true;
                                                                return result;
                                                            }
                                                            if (Session.GetHabbo().Rank < 6u && string.IsNullOrEmpty(reason))
                                                            {
                                                                Session.SendNotification("Ein Grund eingeben!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
                                                            {
                                                                Session.SendNotification("Du besitzt nicht die nötigen Rechte.");
                                                                result = true;
                                                                return result;
                                                            }
                                                            if (timer > 0 && timer <= 600)
                                                            {
                                                                ServerMessage Message = new ServerMessage(27u);
                                                                Message.AppendInt32(timer);
                                                                TargetClient.SendMessage(Message);
                                                                TargetClient.GetHabbo().IsMuted = true;
                                                                TargetClient.GetHabbo().int_4 = timer;
                                                                TargetClient.SendNotification("Du wurdest für " + timer + " Sekunden gemutet!");
                                                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                                result = true;
                                                                return result;
                                                            }
                                                            Session.SendNotification("Man darf nur bis zu 10 Minuten stummen.");
                                                            result = true;
                                                            return result;
                                                        }
                                                    case 165:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_timermuteroom"))
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            if (class2 == null)
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            int timer = int.Parse(Params[1]);
                                                            if (timer > 0 && timer <= 300)
                                                            {
                                                                for (int i = 0; i < class2.RoomUsers.Length; i++)
                                                                {
                                                                    RoomUser class6 = class2.RoomUsers[i];
                                                                    if (class6 != null && class6.GetClient().GetHabbo().Rank < 4u && class6.GetClient().GetHabbo().Username != Session.GetHabbo().CurrentRoom.Owner)
                                                                    {
                                                                        ServerMessage Message = new ServerMessage(27u);
                                                                        Message.AppendInt32(timer);
                                                                        TargetClient.SendMessage(Message);
                                                                        TargetClient.GetHabbo().IsMuted = true;
                                                                        TargetClient.GetHabbo().int_4 = timer;
                                                                        TargetClient.SendNotification("Alle im Raum wurden für " + timer + " Sekunden gemutet!");
                                                                    }
                                                                }
                                                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                                result = true;
                                                                return result;
                                                            }
                                                            Session.SendNotification("Man darf nur bis zu 5 Minuten stummen.");
                                                            result = true;
                                                            return result;
                                                        }
                                                    case 166:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_timermuteall"))
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            int timer = int.Parse(Params[1]);
                                                            if (timer > 0 && timer <= 300)
                                                            {
                                                                HabboIM.GetGame().GetClientManager().method_New4(timer);
                                                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                                result = true;
                                                                return result;
                                                            }
                                                            Session.SendNotification("Man darf nur bis zu 5 Minuten stummen.");
                                                            result = true;
                                                            return result;
                                                        }
                                                    case 167:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_staffpicks"))
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            Room Room2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            int AlreadyStaffPicks = 0;
                                                            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                                            {
                                                                if (dbClient.ReadDataRow(string.Concat(new object[]
                                        {
                                            "SELECT * FROM navigator_publics WHERE room_id = '",
                                            Room2.Id,
                                            "' AND category_parent_id = '",
                                            ServerConfiguration.StaffPicksID,
                                            "'"
                                        }), 30) != null)
                                                                {
                                                                    AlreadyStaffPicks = 1;
                                                                }
                                                            }
                                                            if (AlreadyStaffPicks == 0)
                                                            {
                                                                string Owner;
                                                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                                                {
                                                                    Owner = dbClient.ReadString("SELECT owner FROM rooms WHERE id = '" + Room2.Id + "'", 30);
                                                                    dbClient.AddParamWithValue("roomname", Room2.Name);
                                                                    dbClient.ExecuteQuery(string.Concat(new object[]
                                            {
                                                "INSERT INTO `navigator_publics` (`bannertype`, `caption`, `room_id`, `category_parent_id`, `image`, `image_type`) VALUES ('1', @roomname, '",
                                                Room2.Id,
                                                "', '",
                                                ServerConfiguration.StaffPicksID,
                                                "', 'officialrooms/category_staffpicker.png', 'external')"
                                            }), 30);
                                                                }
                                                                GameClient RoomOwner = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Owner);
                                                                if (RoomOwner != null)
                                                                {
                                                                    RoomOwner.GetHabbo().StaffPicks++;
                                                                    RoomOwner.GetHabbo().CheckStaffPicksAchievement();
                                                                }
                                                                else
                                                                {
                                                                    using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                                                    {
                                                                        try
                                                                        {
                                                                            int OwnerID = dbClient.ReadInt32("SELECT id FROM users WHERE username = '" + Owner + "'", 30);
                                                                            dbClient.ExecuteQuery("UPDATE user_stats SET staff_picks = staff_picks + 1 WHERE id = '" + OwnerID + "' LIMIT 1", 30);
                                                                        }
                                                                        catch (Exception)
                                                                        {
                                                                            Session.SendNotification("Der Raumbesitzer existiert nicht.");
                                                                        }
                                                                    }
                                                                }
                                                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                                                {
                                                                    HabboIM.GetGame().GetNavigator().method_0(dbClient);
                                                                }
                                                                Session.SendNotification("Raum erfolgreich in \"Empfohlen vom Staff\" hinzugefügt");
                                                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), string.Concat(new object[]
                                        {
                                            "Raum ID ",
                                            Room2.Id,
                                            " von ",
                                            Room2.Owner,
                                            " gepickt"
                                        }));
                                                            }
                                                            else
                                                            {
                                                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                                                {
                                                                    dbClient.ExecuteQuery(string.Concat(new object[]
                                            {
                                                "DELETE FROM `navigator_publics` WHERE room_id = '",
                                                Room2.Id,
                                                "' AND category_parent_id = '",
                                                ServerConfiguration.StaffPicksID,
                                                "'"
                                            }), 30);
                                                                }
                                                                using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                                                {
                                                                    HabboIM.GetGame().GetNavigator().method_0(dbClient);
                                                                }
                                                                Session.SendNotification("Raum erfolgreich aus \"Empfohlen vom Staff\" entfernt");
                                                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), string.Concat(new object[]
                                        {
                                            "Raum ID ",
                                            Room2.Id,
                                            " von ",
                                            Room2.Owner,
                                            " ungepickt"
                                        }));
                                                            }
                                                            result = true;
                                                            return result;
                                                        }
                                                    case 168:
                                                        HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("dev");
                                                        return true;
                                                    case 169:

                                                        TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                                        if (TargetClient == null)
                                                        {
                                                            Session.SendNotification("Konnte den User " + Params[1] + " nicht finden.");
                                                        }
                                                        else
                                                        {
                                                            Session.method_10("Kleine Informationen von " + Params[1] + "\nTaler: " + Convert.ToString(TargetClient.GetHabbo().Credits) + "\nEnten: " + Convert.ToString(TargetClient.GetHabbo().ActivityPoints) + "\rSterne: " + Convert.ToString(TargetClient.GetHabbo().VipPoints) + "", "");
                                                        }
                                                        return true;

                                                    case 170:
                                                        {
                                                            int wscount = HabboIM.GetWebSocketManager().getWSlistener();
                                                            Session.GetHabbo().Whisper("Derzeit verbundene User: " + wscount);
                                                            return true;
                                                        }
                                                    case 171:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_reloadws"))
                                                            {
                                                                Session.GetHabbo().Whisper("Du hast keine Berechtigung, um diesen Command zu benutzen!");
                                                                return true;
                                                            }

                                                            HabboIM.GetWebSocketManager().Dispose();
                                                            HabboIM.webSocketManager = new WebSocketServerManager(HabboIM.GetConfig().data["websockets.url"]);
                                                            Session.GetHabbo().Whisper("WebSockets werden nun neugestartet!");
                                                            return true;

                                                        }


                                                    case 172:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_eventhaws"))
                                                            {
                                                                result = false;
                                                                return result;
                                                            }
                                                            string text = Input.Substring(1 + "eventha".Length);
                                                            if (text.Length > 1)
                                                            {

                                                                string toSend = "5|" + text + "|" + Session.GetHabbo().CurrentRoom.Owner + "|" + Session.GetHabbo().CurrentRoomId;
                                                                HabboIM.GetWebSocketManager().SendMessageToEveryConnection(toSend);

                                                                Session.GetHabbo().StoreActivity("makeevent", Session.GetHabbo().CurrentRoomId, HabboIM.GetUnixTimestamp(), text);
                                                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);


                                                            }
                                                            else
                                                            {
                                                                Session.SendNotification("Du musst einen Eventname eingeben!");

                                                            }
                                                            return true;
                                                        }
                                                    case 173:
                                                        {
                                                            if (!Session.GetHabbo().HasFuse("cmd_sitealert"))
                                                            {
                                                                Session.GetHabbo().Whisper("Keine Berechtigung!");
                                                                return true;
                                                            }
                                                            string text = Input.Substring(1 + "sitealert".Length);
                                                            if (text.Length > 1)
                                                            {
                                                                string toSend = "57|" + text;

                                                                HabboIM.GetWebSocketManager().SendMessageToEveryConnection(toSend);
                                                                return true;
                                                            }
                                                            else
                                                            {
                                                                Session.GetHabbo().Whisper("Du musst einen Text eingeben!");
                                                                return true;
                                                            }

                                                        }





                                                    case 888833:

                                                        {

                                                            int users = HabboIM.GetGame().GetClientManager().ClientCount;

                                                            /*
                                                            if (Session.GetHabbo().Rank < 6)

                                                            {

                                                                Session.GetHabbo().Whisper("Dieser Command wurde aufgrund von Instabilitäten deaktiviert. Kommt bald wieder!");

                                                                return true;

                                                            }
                                                            */

                                                            if ( /* Session.GetHabbo().Rank < 2 || */ Session.GetHabbo().AchievementScore < 800)
                                                            {
                                                                Session.GetHabbo().Whisper("Du benötigst 800 Erfahrungspunkte um diesen Befehl ausführen zu können!");

                                                                return true;


                                                            }
                                                            if (users > 50)
                                                            {
                                                                Session.GetHabbo().Whisper("Es sind mehr als 50 User online, du kannst diesen Befehl nicht ausführen!");

                                                                return true;


                                                            }
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            RoomUser class4 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);


                                                            if (class4.fastwalk == true)

                                                            {
                                                                class4.fastwalk = false;
                                                                class4.fasterwalk = false;
                                                                Session.GetHabbo().Whisper("Du läufst nun wieder normal.");

                                                                return true;


                                                            }



                                                            if (class4.fastwalk == false)

                                                            {
                                                                class4.fastwalk = true;
                                                                class4.fasterwalk = false;
                                                                Session.GetHabbo().Whisper("Du läufst nun schnell.");

                                                                return true;


                                                            }




                                                            return true;
                                                        }

                                                    case 888834:

                                                        {

                                                            /*
                                                            if (Session.GetHabbo().Rank < 6)

                                                            {

                                                                Session.GetHabbo().Whisper("Oops, leider kannst du diesen Befehl nicht ausführen!");

                                                                return true;

                                                            }
                                                            */



                                                            int users = HabboIM.GetGame().GetClientManager().ClientCount;

                                                            if (Session.GetHabbo().AchievementScore < 800)
                                                            {
                                                                Session.GetHabbo().Whisper("Du benötigst 800 Erfahrungspunkte um diesen Befehl ausführen zu können!");

                                                                return true;


                                                            }
                                                            if (users > 50)
                                                            {
                                                                Session.GetHabbo().Whisper("Es sind mehr als 50 User online, du kannst diesen Command nicht ausführen!");

                                                                return true;


                                                            }


                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            RoomUser class4 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);


                                                            if (class4.fasterwalk == true)

                                                            {
                                                                class4.fastwalk = false;
                                                                class4.fasterwalk = false;

                                                                Session.GetHabbo().Whisper("Du läufst nun wieder normal.");

                                                                return true;


                                                            }



                                                            if (class4.fasterwalk == false)

                                                            {
                                                                class4.fastwalk = true;
                                                                class4.fasterwalk = true;

                                                                Session.GetHabbo().Whisper("Du läufst nun schnell.");

                                                                return true;


                                                            }




                                                            return true;
                                                        }




                                                    case 888836:

                                                        {

                                                            if (Session.GetHabbo().Rank > 5 || Session.GetHabbo().CurrentRoom.Owner == Session.GetHabbo().Username)
                                                            {

                                                                Session.GetHabbo().CurrentRoom.fastwalk_disabled = true;  // Fastwalk ausgeschaltet


                                                                Session.GetHabbo().Whisper("Fastwalk wurde in diesem Raum deaktiviert.");

                                                                return true;
                                                            }





                                                            return false;
                                                        }


                                                    case 888838:

                                                        {

                                                            if (Session.GetHabbo().Rank > 5 || Session.GetHabbo().CurrentRoom.Owner == Session.GetHabbo().Username)
                                                            {

                                                                Session.GetHabbo().CurrentRoom.fastwalk_disabled = false;  // Fastwalk eingeschaltet


                                                                Session.GetHabbo().Whisper("Fastwalk wurde in diesem Raum aktiviert.");

                                                                return true;
                                                            }





                                                            return false;
                                                        }


                                                    case 174:
                                                        if (Session.GetHabbo().Rank >= 7)
                                                        {
                                                            try
                                                            {
                                                                string str = Input.Substring(3);
                                                                string username = Session.GetHabbo().Username;
                                                                HabboIM.GetGame().GetClientManager().method_18NEW(username, str);
                                                            }
                                                            catch
                                                            {
                                                                Session.SendNotification("Du musst einen Text eingeben");
                                                            }
                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            result = true;
                                                            return result;
                                                        }
                                                        result = false;
                                                        return result;

                                                    case 666668:
                                                        {

                                                            if (Session.GetHabbo().support_last > HabboIM.GetUnixTimestamp() - (60 * 60))
                                                            {
                                                                Session.GetHabbo().Whisper("Diesen Befehl kannst du pro Stunde nur einmal ausführen.");
                                                                return true;
                                                            }
                                                            try
                                                            {
                                                                string str = Input.Substring(8);


                                                                ServerMessage Logging = new ServerMessage(134u);
                                                                Logging.AppendUInt(0u);
                                                                Logging.AppendString(string.Concat(new string[]
                                                   {
                                                    "Support: Der Spieler \"",
                                                    Session.GetHabbo().Username,
                                                    "\" benötigt dringend im Raum \"",
                                                    Session.GetHabbo().CurrentRoom.Name,
                                                    "\" Hilfe!\rNachricht: "+ str
                                                   }));

                                                                HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                                                Session.GetHabbo().support_last = HabboIM.GetUnixTimestamp();
                                                                Session.GetHabbo().UpdateSupport();
                                                                Session.GetHabbo().Whisper("Deine Support-Anfrage wurde gesendet.");
                                                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            }
                                                            catch
                                                            {
                                                                Session.GetHabbo().Whisper("Bitte gebe eine Nachricht für den Support ein.");
                                                                return true;

                                                            }


                                                            return true;
                                                        }
                                                    case 175:
                                                        if (Session.GetHabbo().Rank >= 6)
                                                        {
                                                            try
                                                            {
                                                                string str = Input.Substring(3);
                                                                string username = Session.GetHabbo().Username;
                                                                HabboIM.GetGame().GetClientManager().method_18SA(username, str);
                                                            }
                                                            catch
                                                            {
                                                                Session.SendNotification("Du musst einen Text eingeben");
                                                            }
                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            result = true;
                                                            return result;
                                                        }
                                                        result = false;
                                                        return result;

                                                    case 176:

                                                        if (Session.GetHabbo().Rank >= 6)
                                                        {
                                                            if (Params.Length > 1)
                                                            {
                                                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                                HabboIM.GetGame().GetBanManager().UnBanUser(Params[1]);
                                                                Session.GetHabbo().Whisper("Der User wurde entbannt. Bitte update noch die Bans!");
                                                            }
                                                            else
                                                            {
                                                                Session.GetHabbo().Whisper("Der User ist nicht gebannt oder existiert nicht.");
                                                            }

                                                            return true;
                                                        }
                                                        return false;
                                                    case 177:
                                                        using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
                                                        {
                                                            string string__ = @class.ReadString("SELECT id FROM users WHERE username = '" + HabboIMEnvironment.DownloadName() + "' LIMIT 1;");
                                                            if (Session.GetHabbo().Rank >= 7)
                                                            {
                                                                ServerMessage Logging = new ServerMessage(134u);
                                                                Logging.AppendUInt(0u);
                                                                if (Session.GetHabbo().GetEffectsInventoryComponent().int_0 != 503)
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(503, true);
                                                                    Logging.AppendString(string.Concat(new string[]
                                                    {
                                                    "System: Der Mitarbeiter \"",
                                                    Session.GetHabbo().Username,
                                                    "\" ist absofort im Dienst!"
                                                    }));
                                                                }
                                                                else
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                    Logging.AppendString(string.Concat(new string[]
                                                    {
                                                    "System: Der Mitarbeiter \"",
                                                    Session.GetHabbo().Username,
                                                    "\" ist nicht mehr im Dienst!"
                                                    }));
                                                                }
                                                                HabboIM.GetGame().GetClientManager().method_16(Logging, Logging);
                                                                return true;
                                                            }

                                                            else
                                                                return false;

                                                        }

                                                    case 178:

                                                        if (Session.GetHabbo().Rank >= 1)
                                                        {
                                                            RoomUser class6;
                                                            if (Session.GetHabbo().Shisha == 1)
                                                            {
                                                                Session.GetHabbo().Shisha = 0;
                                                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                                Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                return true;
                                                            }
                                                            if (Session.GetHabbo().CurrentRoomId == 0) { return false; }
                                                            if (!Session.GetHabbo().InRoom)
                                                            {
                                                                return true;
                                                            }
                                                            class6 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);

                                                            if (class6.Statusses.ContainsKey("sit") || class6.Statusses.ContainsKey("lay") || class6.BodyRotation == 1 || class6.BodyRotation == 3 || class6.BodyRotation == 5 || class6.BodyRotation == 7)
                                                            {
                                                                return true;
                                                            }
                                                            if (class6.byte_1 > 0 || class6.class34_1 != null)
                                                            {
                                                                return true;
                                                            }
                                                            Session.GetHabbo().Shisha = 1;
                                                            class6.AddStatus("sit", ((class6.double_0 + 1.0) / 2.0 - class6.double_0 * 0.5).ToString());
                                                            class6.UpdateNeeded = true;

                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(16, true);

                                                            return true;
                                                        }
                                                        return false;
                                                    case 179:
                                                        {
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(169, true);
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            class5.HandleSpeech(Session, "*hat geniest*", false);
                                                            System.Threading.Thread.Sleep(5000);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                            return true;
                                                        }
                                                    case 180:
                                                        {
                                                            try
                                                            {

                                                                RoomUser class6_001;
                                                                GameClient User2 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                                class6_001 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);

                                                                RoomUser class4 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);

                                                                if (User2.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                                {
                                                                    if (User2.GetHabbo().Id == Session.GetHabbo().Id)
                                                                    {
                                                                        return false;
                                                                    }
                                                                    else
                                                                    {



                                                                        bool AreFriends = false;

                                                                        using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
                                                                        {
                                                                            DataTable dt1 = @class.ReadDataTable("SELECT * FROM messenger_friendships");


                                                                            foreach (DataRow dr1 in dt1.Rows)
                                                                            {
                                                                                if (Convert.ToInt32(dr1["user_one_id"].ToString()) == User2.GetHabbo().Id && Convert.ToInt32(dr1["user_two_id"]) == Session.GetHabbo().Id || Convert.ToInt32(dr1["user_one_id"].ToString()) == Session.GetHabbo().Id && Convert.ToInt32(dr1["user_two_id"]) == User2.GetHabbo().Id)
                                                                                {
                                                                                    AreFriends = false;
                                                                                }
                                                                            }
                                                                        }
                                                                        if (AreFriends == false)
                                                                        {
                                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                            RoomUser Aktuelleruser = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                                            RoomUser Usr2 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);
                                                                            if (Math.Abs(Aktuelleruser.X - Usr2.X) < 3 && Math.Abs(Aktuelleruser.Y - Usr2.Y) < 3)
                                                                            {
                                                                                Aktuelleruser.HandleSpeech(Session, "*Macht " + User2.GetHabbo().Username + " ein Heiratsantrag|*", true);
                                                                                Session.GetHabbo().GetEffectsInventoryComponent().method_2(168, true);
                                                                                User2.GetHabbo().GetEffectsInventoryComponent().method_2(168, true);
                                                                                System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        System.Threading.Thread.Sleep(5000);
                                                                                        Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                                        User2.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                                    }
                                                                                    catch
                                                                                    {

                                                                                    }
                                                                                });
                                                                                thrd.Start();
                                                                                return true;
                                                                            }
                                                                            else
                                                                            {
                                                                                Session.GetHabbo().Whisper("Ihr seid zu weit weg voneinander");
                                                                                return true;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            Session.GetHabbo().Whisper("Ihr müsst Freunde sein, um zu Heiraten!");
                                                                            return true;
                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    return false;
                                                                }
                                                            }

                                                            catch
                                                            {
                                                                return false;
                                                            }

                                                        }
                                                    case 181:
                                                        {

                                                            RoomUser class6_001;
                                                            GameClient User2 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                            class6_001 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);

                                                            RoomUser class4 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);

                                                            if (User2.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                            {
                                                                if (User2.GetHabbo().Id == Session.GetHabbo().Id)
                                                                {
                                                                    return false;
                                                                }
                                                                else
                                                                {
                                                                    class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                    RoomUser Aktuelleruser = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                                    RoomUser Usr2 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);
                                                                    if (Math.Abs(Aktuelleruser.X - Usr2.X) < 24 && Math.Abs(Aktuelleruser.Y - Usr2.Y) < 24)
                                                                    {
                                                                        Aktuelleruser.HandleSpeech(Session, "Hallo " + User2.GetHabbo().Username + "! Herzlich Willkommen im MyHubba Hotel. |", true);
                                                                        Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                        User2.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                        System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
                                                                        {
                                                                            try
                                                                            {
                                                                                System.Threading.Thread.Sleep(5000);
                                                                                Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                                User2.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                            }
                                                                            catch
                                                                            {

                                                                            }
                                                                        });
                                                                        thrd.Start();
                                                                        return true;
                                                                    }

                                                                }

                                                            }
                                                            else
                                                            {
                                                                return false;
                                                            }
                                                            return true;
                                                        }
                                                    case 182:
                                                        try
                                                        {


                                                            RoomUser class6_001;
                                                            GameClient User2 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                            class6_001 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);

                                                            RoomUser class4 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);

                                                            if (User2.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                            {
                                                                if (User2.GetHabbo().Id == Session.GetHabbo().Id)
                                                                {
                                                                    return false;
                                                                }
                                                                else
                                                                {



                                                                    bool flag3 = false;
                                                                    using (DatabaseClient client2 = HabboIM.GetDatabase().GetClient())
                                                                    {
                                                                        DataTable dataTable = client2.ReadDataTable("SELECT * FROM messenger_friendships", 30);
                                                                        foreach (DataRow dataRow in dataTable.Rows)
                                                                        {
                                                                            if (((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)User2.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)Session.GetHabbo().Id)) || ((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)Session.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)User2.GetHabbo().Id)))
                                                                            {
                                                                                flag3 = true;
                                                                            }
                                                                        }
                                                                    }

                                                                    if (flag3 == true)
                                                                    {
                                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);//BRAUCHEICH
                                                                        RoomUser Aktuelleruser = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                                        RoomUser Usr2 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);
                                                                        if (Math.Abs(Aktuelleruser.X - Usr2.X) < 3 && Math.Abs(Aktuelleruser.Y - Usr2.Y) < 3)
                                                                        {
                                                                            Aktuelleruser.HandleSpeech(Session, "*Hat " + User2.GetHabbo().Username + " getötet!*", false);
                                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(616, true);
                                                                            User2.GetHabbo().GetEffectsInventoryComponent().method_2(133, true);
                                                                            System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
                                                                            {
                                                                                try
                                                                                {
                                                                                    System.Threading.Thread.Sleep(5000);
                                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(616, true);
                                                                                    User2.GetHabbo().GetEffectsInventoryComponent().method_2(133, true);
                                                                                }
                                                                                catch
                                                                                {

                                                                                }
                                                                            });
                                                                            thrd.Start();
                                                                            return true;
                                                                        }
                                                                        else
                                                                        {
                                                                            Session.GetHabbo().Whisper("Ihr seid zu weit weg voneinander");
                                                                            return true;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Session.GetHabbo().Whisper("Ihr müsst befreundet sein!");
                                                                        return true;
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                return false;
                                                            }
                                                        }

                                                        catch
                                                        {
                                                            return false;
                                                        }

                                                    case 183:
                                                        try
                                                        {

                                                            RoomUser class6_001;
                                                            GameClient User2 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                            class6_001 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);

                                                            RoomUser class4 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);

                                                            if (User2.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                            {
                                                                if (User2.GetHabbo().Id == Session.GetHabbo().Id)
                                                                {
                                                                    return false;
                                                                }
                                                                else
                                                                {



                                                                    bool flag3 = false;
                                                                    using (DatabaseClient client2 = HabboIM.GetDatabase().GetClient())
                                                                    {
                                                                        DataTable dataTable = client2.ReadDataTable("SELECT * FROM messenger_friendships", 30);
                                                                        foreach (DataRow dataRow in dataTable.Rows)
                                                                        {
                                                                            if (((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)User2.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)Session.GetHabbo().Id)) || ((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)Session.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)User2.GetHabbo().Id)))
                                                                            {
                                                                                flag3 = true;
                                                                            }
                                                                        }
                                                                    }

                                                                    if (flag3 == true)
                                                                    {
                                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                        RoomUser Aktuelleruser = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                                        RoomUser Usr2 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);
                                                                        if (Math.Abs(Aktuelleruser.X - Usr2.X) < 3 && Math.Abs(Aktuelleruser.Y - Usr2.Y) < 3)
                                                                        {
                                                                            Aktuelleruser.HandleSpeech(Session, "*Liebt " + User2.GetHabbo().Username + " über alles |*", false);
                                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(9, true);
                                                                            User2.GetHabbo().GetEffectsInventoryComponent().method_2(9, true);
                                                                            System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
                                                                            {
                                                                                try
                                                                                {
                                                                                    System.Threading.Thread.Sleep(5000);
                                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(9, true);
                                                                                    User2.GetHabbo().GetEffectsInventoryComponent().method_2(9, true);
                                                                                }
                                                                                catch
                                                                                {

                                                                                }
                                                                            });
                                                                            thrd.Start();
                                                                            return true;
                                                                        }
                                                                        else
                                                                        {
                                                                            Session.GetHabbo().Whisper("Ihr seid zu weit weg voneinander");
                                                                            return true;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Session.GetHabbo().Whisper("Ihr müsst Freunde sein, um euch zu Lieben");
                                                                        return true;
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                return false;
                                                            }
                                                        }

                                                        catch
                                                        {
                                                            return false;
                                                        }
                                                    case 184:
                                                        try
                                                        {

                                                            RoomUser class6_001;
                                                            GameClient User2 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                            class6_001 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);

                                                            RoomUser class4 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);

                                                            if (User2.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                            {
                                                                if (User2.GetHabbo().Id == Session.GetHabbo().Id)
                                                                {
                                                                    return false;
                                                                }
                                                                else
                                                                {



                                                                    bool flag3 = false;
                                                                    using (DatabaseClient client2 = HabboIM.GetDatabase().GetClient())
                                                                    {
                                                                        DataTable dataTable = client2.ReadDataTable("SELECT * FROM messenger_friendships", 30);
                                                                        foreach (DataRow dataRow in dataTable.Rows)
                                                                        {
                                                                            if (((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)User2.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)Session.GetHabbo().Id)) || ((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)Session.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)User2.GetHabbo().Id)))
                                                                            {
                                                                                flag3 = true;
                                                                            }
                                                                        }
                                                                    }

                                                                    if (flag3 == true)
                                                                    {
                                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                        RoomUser Aktuelleruser = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                                        RoomUser Usr2 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);
                                                                        if (Math.Abs(Aktuelleruser.X - Usr2.X) < 3 && Math.Abs(Aktuelleruser.Y - Usr2.Y) < 3)
                                                                        {
                                                                            Aktuelleruser.HandleSpeech(Session, "*" + User2.GetHabbo().Username + " gefällt mir!*", true);
                                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(165, true);
                                                                            User2.GetHabbo().GetEffectsInventoryComponent().method_2(165, true);
                                                                            System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
                                                                            {
                                                                                try
                                                                                {
                                                                                    System.Threading.Thread.Sleep(5000);
                                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                                    User2.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                                }
                                                                                catch
                                                                                {

                                                                                }
                                                                            });
                                                                            thrd.Start();
                                                                            return true;
                                                                        }
                                                                        else
                                                                        {
                                                                            Session.GetHabbo().Whisper("Ihr seid zu weit weg voneinander");
                                                                            return true;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Session.GetHabbo().Whisper("Du kannst diesen User nicht liken, da ihr nicht befreundet seid!");
                                                                        return true;
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                return false;
                                                            }
                                                        }

                                                        catch
                                                        {
                                                            return false;
                                                        }
                                                    case 185:
                                                        if (Session.GetHabbo().Rank >= 2)
                                                        {
                                                            if (!(Session.GetHabbo().CurrentRoom.Owner == Session.GetHabbo().Username))
                                                            {
                                                                Session.SendNotification("Du kannst es nur in deinen eigenen Räumen Regnen lassen!");
                                                                result = true;
                                                                return result;
                                                            }

                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            if (class2 == null)
                                                            {
                                                                result = false;
                                                                return result;
                                                            }

                                                            for (int i = 0; i < class2.RoomUsers.Length; i++)
                                                            {
                                                                RoomUser roomUser4 = class2.RoomUsers[i];
                                                                if (roomUser4 != null)
                                                                {
                                                                    TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(roomUser4.GetClient().GetHabbo().Username);

                                                                    if (TargetClient.GetHabbo().GetEffectsInventoryComponent().int_0 != 113)
                                                                    {
                                                                        TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(roomUser4.GetClient().GetHabbo().Username);
                                                                        TargetClient.GetHabbo().GetEffectsInventoryComponent().method_2(113, true);
                                                                        TargetClient.GetHabbo().Whisper("In diesem Raum regnet es nun!");
                                                                    }
                                                                    else
                                                                    {

                                                                        TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(roomUser4.GetClient().GetHabbo().Username);
                                                                        TargetClient.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                        TargetClient.GetHabbo().Whisper("In diesem Raum regnet es nun nicht mehr!");

                                                                    }
                                                                }
                                                            }

                                                            result = true;
                                                            return result;
                                                        }
                                                        return false;
                                                    case 186:
                                                        try
                                                        {

                                                            RoomUser class6_001;
                                                            GameClient User2 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                            class6_001 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);

                                                            RoomUser class4 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);

                                                            if (User2.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                            {
                                                                if (User2.GetHabbo().Id == Session.GetHabbo().Id)
                                                                {
                                                                    return false;
                                                                }
                                                                else
                                                                {



                                                                    bool flag3 = false;
                                                                    using (DatabaseClient client2 = HabboIM.GetDatabase().GetClient())
                                                                    {
                                                                        DataTable dataTable = client2.ReadDataTable("SELECT * FROM messenger_friendships", 30);
                                                                        foreach (DataRow dataRow in dataTable.Rows)
                                                                        {
                                                                            if (((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)User2.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)Session.GetHabbo().Id)) || ((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)Session.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)User2.GetHabbo().Id)))
                                                                            {
                                                                                flag3 = true;
                                                                            }
                                                                        }
                                                                    }

                                                                    if (flag3 == true)
                                                                    {
                                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                        RoomUser Aktuelleruser = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                                        RoomUser Usr2 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);
                                                                        if (Math.Abs(Aktuelleruser.X - Usr2.X) < 3 && Math.Abs(Aktuelleruser.Y - Usr2.Y) < 3)
                                                                        {
                                                                            Aktuelleruser.HandleSpeech(Session, "*Haut " + User2.GetHabbo().Username + " voll auf den Kopf!*", false);
                                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(611, true);
                                                                            User2.GetHabbo().GetEffectsInventoryComponent().method_2(157, true);
                                                                            System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
                                                                            {
                                                                                try
                                                                                {
                                                                                    System.Threading.Thread.Sleep(5000);
                                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                                    User2.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                                }
                                                                                catch
                                                                                {

                                                                                }
                                                                            });
                                                                            thrd.Start();
                                                                            return true;
                                                                        }
                                                                        else
                                                                        {
                                                                            Session.GetHabbo().Whisper("Ihr seid zu weit weg voneinander");
                                                                            return true;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Session.GetHabbo().Whisper("Ihr müsst Freunde sein, um euch zu Schlagen");
                                                                        return true;
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                return false;
                                                            }
                                                        }

                                                        catch
                                                        {
                                                            return false;
                                                        }
                                                    case 187:
                                                        try
                                                        {

                                                            RoomUser class6_001;
                                                            GameClient User2 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                            class6_001 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);

                                                            RoomUser class4 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);

                                                            if (User2.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                            {
                                                                if (User2.GetHabbo().Id == Session.GetHabbo().Id)
                                                                {
                                                                    return false;
                                                                }
                                                                else
                                                                {



                                                                    bool flag3 = false;
                                                                    using (DatabaseClient client2 = HabboIM.GetDatabase().GetClient())
                                                                    {
                                                                        DataTable dataTable = client2.ReadDataTable("SELECT * FROM messenger_friendships", 30);
                                                                        foreach (DataRow dataRow in dataTable.Rows)
                                                                        {
                                                                            if (((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)User2.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)Session.GetHabbo().Id)) || ((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)Session.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)User2.GetHabbo().Id)))
                                                                            {
                                                                                flag3 = true;
                                                                            }
                                                                        }
                                                                    }

                                                                    if (flag3 == true)
                                                                    {
                                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                        RoomUser Aktuelleruser = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                                        RoomUser Usr2 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);
                                                                        if (Math.Abs(Aktuelleruser.X - Usr2.X) < 3 && Math.Abs(Aktuelleruser.Y - Usr2.Y) < 3)
                                                                        {
                                                                            Aktuelleruser.HandleSpeech(Session, "*Umarmt " + User2.GetHabbo().Username + " *", true);
                                                                            TargetClient = User2;
                                                                            try
                                                                            {



                                                                                if (User2.GetHabbo().boyfriend == Session.GetHabbo().Id && User2.GetHabbo().Id == Session.GetHabbo().boyfriend && User2.GetHabbo().hugtime < HabboIM.GetUnixTimestamp() - 60 * 10 && Session.GetHabbo().hugtime < HabboIM.GetUnixTimestamp() - 60 * 10)
                                                                                {

                                                                                    try
                                                                                    {
                                                                                        User2.GetHabbo().hugtime = HabboIM.GetUnixTimestamp();
                                                                                        Session.GetHabbo().hugtime = HabboIM.GetUnixTimestamp();
                                                                                    }
                                                                                    catch (Exception x)
                                                                                    {
                                                                                        //      Session.SendNotification(x.ToString());
                                                                                    }
                                                                                    try
                                                                                    {
                                                                                        HabboIM.GetWebSocketManager().getWebSocketByName(User2.GetHabbo().Username).Send("kisshug|" + Session.GetHabbo().Username + " hat dich umarmt!|1|Beziehungspunkte");
                                                                                        HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("kisshug|Du hast " + TargetClient.GetHabbo().Username + " umarmt!|1|Beziehungspunkte");

                                                                                        Session.GetHabbo().lovepoints += 1;
                                                                                        User2.GetHabbo().lovepoints += 1;
                                                                                        Session.GetHabbo().hugged += 1;
                                                                                        User2.GetHabbo().hugged += 1;
                                                                                        Session.GetHabbo().UpdateTime();

                                                                                    }
                                                                                    catch (Exception x)
                                                                                    {
                                                                                        //    Session.SendNotification(x.ToString());
                                                                                    }
                                                                                }

                                                                                if (TargetClient.GetHabbo().boyfriend == Session.GetHabbo().Id && TargetClient.GetHabbo().Id == Session.GetHabbo().boyfriend)
                                                                                {
                                                                                    HabboIM.GetGame().GetClientManager().method_checkstats(Session);
                                                                                    HabboIM.GetGame().GetClientManager().method_checkstats(TargetClient);
                                                                                }
                                                                            }
                                                                            catch (Exception x)
                                                                            {
                                                                                // Session.SendNotification(x.ToString());
                                                                            }




                                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(168, true);
                                                                            User2.GetHabbo().GetEffectsInventoryComponent().method_2(168, true);
                                                                            System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
                                                                            {
                                                                                try
                                                                                {
                                                                                    System.Threading.Thread.Sleep(5000);
                                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                                    User2.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                                }
                                                                                catch
                                                                                {

                                                                                }
                                                                            });
                                                                            thrd.Start();
                                                                            return true;
                                                                        }
                                                                        else
                                                                        {
                                                                            Session.GetHabbo().Whisper("Ihr seid zu weit weg voneinander");
                                                                            return true;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Session.GetHabbo().Whisper("Ihr müsst Freunde sein, um euch zu umarmen");
                                                                        return true;
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                return false;
                                                            }
                                                        }

                                                        catch
                                                        {
                                                            return false;
                                                        }
                                                    case 188:
                                                        {
                                                            try
                                                            {

                                                                string einsatz = Params[1];
                                                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                if (Session == null) { return false; }
                                                                if (Session.GetHabbo().CurrentRoomId == 0) { return false; }

                                                                string hochdruecken = Params[1];
                                                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                if (Session == null) { return false; }
                                                                if (Session.GetHabbo().CurrentRoomId == 0) { return false; }

                                                                if (einsatz == "ja" || einsatz == "nein")
                                                                {
                                                                    if (Session.GetHabbo().Hochdruecken == 0) { return false; }

                                                                    int win = Session.GetHabbo().CasinoWin;
                                                                    if (einsatz == "ja")
                                                                    {
                                                                        Random rnd = new Random();
                                                                        int random = rnd.Next(1, 3);
                                                                        if (random == 1)
                                                                        {
                                                                            win = win + win;
                                                                            Session.GetHabbo().Whisper("Du gewinst " + win + " Taler!");
                                                                            Session.GetHabbo().Credits = Session.GetHabbo().Credits + win;
                                                                            Session.GetHabbo().UpdateCredits(true);
                                                                        }
                                                                        else
                                                                        {
                                                                            win = 0;
                                                                            Session.GetHabbo().Whisper("Schade, du hast verloren!");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Session.GetHabbo().Credits = Session.GetHabbo().Credits + win;
                                                                        Session.GetHabbo().UpdateCredits(true);
                                                                    }
                                                                    Session.GetHabbo().CasinoWin = 0;
                                                                    Session.GetHabbo().Hochdruecken = 0;
                                                                    return true;
                                                                }


                                                                Session.GetHabbo().CasinoPlace = 0;

                                                                double number;
                                                                if (double.TryParse(einsatz, out number))
                                                                {
                                                                    if (int.Parse(einsatz) > 100000000 || int.Parse(einsatz) <= 0) { Session.GetHabbo().Whisper("Dein Einsatz darf nur zwischen 1 und 100.000.000 Taler liegen."); return true; }
                                                                    Session.GetHabbo().CasinoPlace = int.Parse(einsatz);
                                                                    Session.GetHabbo().Whisper("Dein Einsatz ist nun " + einsatz + " Taler.");
                                                                    return true;
                                                                }
                                                                Session.GetHabbo().Whisper("Du musst einen gültigen Betrag angeben.");
                                                                return true;
                                                            }
                                                            catch
                                                            {
                                                                Session.GetHabbo().Whisper("Du musst einen gültigen Betrag angeben.");
                                                                return true;
                                                            }
                                                        }
                                                    case 189:
                                                        if (Session.GetHabbo().Rank > 6u)
                                                        {
                                                            Room currentRoom1 = Session.GetHabbo().CurrentRoom;
                                                            RoomUser roomUserByHabbo = null;
                                                            currentRoom1 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            if (currentRoom1 != null)
                                                            {
                                                                roomUserByHabbo = currentRoom1.GetRoomUserByHabbo(Session.GetHabbo().Id);

                                                                if (roomUserByHabbo != null)
                                                                {
                                                                    roomUserByHabbo.isFlying = true;
                                                                    roomUserByHabbo.bool_1 = true;
                                                                }
                                                            }
                                                            return true;
                                                        }
                                                        return false;

                                                    case 191:
                                                        {
                                                            if (Session.GetHabbo().Rank >= 6)
                                                            {
                                                                if (Advertisements.Werberunde.flag)
                                                                {
                                                                    Advertisements.Werberunde.flag = false;
                                                                    Advertisements.Werberunde.SUsers = 0;
                                                                    Advertisements.Werberunde.WERBERUNDE_Alert("Dieses Werberundenziel wurde abgesetzt!");
                                                                    return true;
                                                                }
                                                                Advertisements.Werberunde.SUsers = short.Parse(Params[1]);
                                                                Advertisements.Werberunde.SET();
                                                                return true;
                                                            }
                                                            return false;
                                                        }
                                                    case 192:
                                                        if (Session.GetHabbo().Rank >= 6)
                                                        {
                                                            try
                                                            {
                                                                string TargetUser = Params[1];
                                                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(TargetUser);
                                                                if (TargetClient == null)
                                                                {
                                                                    Session.SendNotification("Konnte den User nicht finden: " + TargetUser);
                                                                    result = true;
                                                                    return result;
                                                                }
                                                                TargetClient.GetHabbo().Whisper("Admin " + Session.GetHabbo().Username + " flüstert dir: " + ChatCommandHandler.MergeParams(Params, 2));

                                                            }
                                                            catch
                                                            {
                                                                Session.SendNotification("Du musst einen Text eingeben");
                                                            }

                                                            result = true;
                                                            return result;
                                                        }
                                                        else
                                                        {

                                                        }
                                                        result = false;
                                                        return result;

                                                    case 193:
                                                        try
                                                        { 
                                                        if (/* Session.GetHabbo().Rank < 2 && */ Session.GetHabbo().AchievementScore < 1000)
                                                        {
                                                            Session.GetHabbo().Whisper("Du benötigst 1000 Erfahrungspunkte um diesen Befehl ausführen zu können!");
                                                            return true;
                                                        }

                                                        GameClient User2 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                            bool flag3 = false;
                                                            using (DatabaseClient client2 = HabboIM.GetDatabase().GetClient())
                                                            {
                                                                DataTable dataTable = client2.ReadDataTable("SELECT * FROM messenger_friendships", 30);
                                                                foreach (DataRow dataRow in dataTable.Rows)
                                                                {
                                                                    if (((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)User2.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)Session.GetHabbo().Id)) || ((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)Session.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)User2.GetHabbo().Id)))
                                                                    {
                                                                        flag3 = true;
                                                                    }
                                                                }
                                                            }


                                                            if (flag3 == true)
                                                            {
                                                                try
                                                                {

                                                                    string TargetUser = Params[1];
                                                                    TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(TargetUser);
                                                                    if (TargetClient == null)
                                                                    {
                                                                        Session.SendNotification("Konnte den User nicht finden: " + TargetUser);
                                                                        result = true;
                                                                        return result;
                                                                    }
                                                                    string antiweberstring = ChatCommandHandler.MergeParams(Params, 2);
                                                                    string textaw = ChatCommandHandler.smethod_4b(Session, antiweberstring, "Raum");
                                                                    TargetClient.GetHabbo().Whisper("Nachricht von " + Session.GetHabbo().Username + ": " + textaw);
                                                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                                }
                                                                catch
                                                                {
                                                                    Session.SendNotification("Du musst einen Text eingeben");
                                                                }

                                                                result = true;
                                                                return result;
                                                            }
                                                            else
                                                            {
                                                                Session.GetHabbo().Whisper("Du musst mit dem Spieler befreundet sein, um ihm eine SMS zu senden!");
                                                                result = true;
                                                                return result;
                                                            }

                                                        }
                                                        catch
                                                        {
                                                            return false;
                                                        }
                                                    case 194:
                                                        try
                                                        {

                                                            RoomUser class6_001;
                                                            GameClient User2 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                            class6_001 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);

                                                            RoomUser class4 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);

                                                            if (User2.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                            {
                                                                if (User2.GetHabbo().Id == Session.GetHabbo().Id)
                                                                {
                                                                    return false;
                                                                }
                                                                else
                                                                {



                                                                    bool flag3 = false;
                                                                    using (DatabaseClient client2 = HabboIM.GetDatabase().GetClient())
                                                                    {
                                                                        DataTable dataTable = client2.ReadDataTable("SELECT * FROM messenger_friendships", 30);
                                                                        foreach (DataRow dataRow in dataTable.Rows)
                                                                        {
                                                                            if (((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)User2.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)Session.GetHabbo().Id)) || ((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)Session.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)User2.GetHabbo().Id)))
                                                                            {
                                                                                flag3 = true;
                                                                            }
                                                                        }
                                                                    }

                                                                    if (flag3 == true)
                                                                    {
                                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                        RoomUser Aktuelleruser = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                                        RoomUser Usr2 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);
                                                                        if (Math.Abs(Aktuelleruser.X - Usr2.X) < 3 && Math.Abs(Aktuelleruser.Y - Usr2.Y) < 3)
                                                                        {
                                                                            Aktuelleruser.HandleSpeech(Session, "*" + User2.GetHabbo().Username + " gefällt mir nicht!*", true);
                                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(165, true);
                                                                            User2.GetHabbo().GetEffectsInventoryComponent().method_2(165, true);
                                                                            System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
                                                                            {
                                                                                try
                                                                                {
                                                                                    System.Threading.Thread.Sleep(5000);
                                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                                    User2.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                                }
                                                                                catch
                                                                                {

                                                                                }
                                                                            });
                                                                            thrd.Start();
                                                                            return true;
                                                                        }
                                                                        else
                                                                        {
                                                                            Session.GetHabbo().Whisper("Ihr seid zu weit weg voneinander");
                                                                            return true;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Session.GetHabbo().Whisper("Du kannst diesen User nicht disliken, da ihr nicht befreundet seid!");
                                                                        return true;
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                return false;
                                                            }
                                                        }

                                                        catch
                                                        {
                                                            return false;
                                                        }
                                                    case 195:
                                                        try
                                                        {

                                                            RoomUser class6_001;
                                                            GameClient User2 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                            class6_001 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);

                                                            RoomUser class4 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);

                                                            if (User2.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                            {
                                                                if (User2.GetHabbo().Id == Session.GetHabbo().Id)
                                                                {
                                                                    return false;
                                                                }
                                                                else
                                                                {



                                                                    bool flag3 = false;
                                                                    using (DatabaseClient client2 = HabboIM.GetDatabase().GetClient())
                                                                    {
                                                                        DataTable dataTable = client2.ReadDataTable("SELECT * FROM messenger_friendships", 30);
                                                                        foreach (DataRow dataRow in dataTable.Rows)
                                                                        {
                                                                            if (((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)User2.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)Session.GetHabbo().Id)) || ((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)Session.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)User2.GetHabbo().Id)))
                                                                            {
                                                                                flag3 = true;
                                                                            }
                                                                        }
                                                                    }

                                                                    if (flag3 == true)
                                                                    {
                                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                        RoomUser Aktuelleruser = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                                        RoomUser Usr2 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);
                                                                        if (Math.Abs(Aktuelleruser.X - Usr2.X) < 3 && Math.Abs(Aktuelleruser.Y - Usr2.Y) < 3)
                                                                        {
                                                                            Aktuelleruser.HandleSpeech(Session, "*Findet " + User2.GetHabbo().Username + " heiß!*", true);
                                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(25, true);
                                                                            User2.GetHabbo().GetEffectsInventoryComponent().method_2(25, true);
                                                                            System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
                                                                            {
                                                                                try
                                                                                {
                                                                                    System.Threading.Thread.Sleep(5000);
                                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                                    User2.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                                }
                                                                                catch
                                                                                {

                                                                                }
                                                                            });
                                                                            thrd.Start();
                                                                            return true;
                                                                        }
                                                                        else
                                                                        {
                                                                            Session.GetHabbo().Whisper("Ihr seid zu weit weg voneinander");
                                                                            return true;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Session.GetHabbo().Whisper("Du kannst diesen User nicht heiß finden, da ihr nicht befreundet seid!");
                                                                        return true;
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                return false;
                                                            }
                                                        }

                                                        catch
                                                        {
                                                            return false;
                                                        }
                                                    case 196:
                                                        {
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(35, true);
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            class5.HandleSpeech(Session, "*Springt hoch in die Luft!*", false);
                                                            System.Threading.Thread.Sleep(5000);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                            return true;
                                                        }
                                                    case 197:
                                                        {
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(140, true);
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            class5.HandleSpeech(Session, "*Tanzt den Habnam Style!*", false);
                                                            System.Threading.Thread.Sleep(5000);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                            return true;
                                                        }
                                                    case 198:
                                                        if (Session.GetHabbo().Rank >= 2)
                                                        {
                                                            if (!(Session.GetHabbo().CurrentRoom.Owner == Session.GetHabbo().Username))
                                                            {
                                                                Session.SendNotification("Du kannst die Sonne nur in deinen eigenen Räumen aktivieren!");
                                                                result = true;
                                                                return result;
                                                            }

                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            if (class2 == null)
                                                            {
                                                                result = false;
                                                                return result;
                                                            }

                                                            for (int i = 0; i < class2.RoomUsers.Length; i++)
                                                            {
                                                                RoomUser roomUser4 = class2.RoomUsers[i];
                                                                if (roomUser4 != null)
                                                                {
                                                                    TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(roomUser4.GetClient().GetHabbo().Username);

                                                                    if (TargetClient.GetHabbo().GetEffectsInventoryComponent().int_0 != 26)
                                                                    {
                                                                        TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(roomUser4.GetClient().GetHabbo().Username);
                                                                        TargetClient.GetHabbo().GetEffectsInventoryComponent().method_2(26, true);
                                                                        TargetClient.GetHabbo().Whisper("In diesem Raum scheint nun die Sonne!");
                                                                    }
                                                                    else
                                                                    {

                                                                        TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(roomUser4.GetClient().GetHabbo().Username);
                                                                        TargetClient.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                        TargetClient.GetHabbo().Whisper("In diesem Raum scheint die Sonne nun nicht mehr!");

                                                                    }
                                                                }
                                                            }

                                                            result = true;
                                                            return result;
                                                        }
                                                        return false;
                                                    case 199:
                                                        HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("updateLog");
                                                        return true;
                                                    case 200:
                                                        {
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(16, true);
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            class5.HandleSpeech(Session, "*Zündet sich eine Zigarette an*", false);
                                                            System.Threading.Thread.Sleep(5000);
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                            return true;
                                                        }
                                                    case 201:
                                                        if (Session.GetHabbo().Rank >= 7)
                                                        {
                                                            try
                                                            {
                                                                string str = Input.Substring(10);
                                                                string username = Session.GetHabbo().Username;
                                                                HabboIM.GetGame().GetClientManager().method_18AFK(username, str);
                                                            }
                                                            catch
                                                            {
                                                                Session.SendNotification("Bitte gebe einen Grund ein, weswegen du Abwesend gehst.");
                                                            }
                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            result = true;
                                                            return result;
                                                        }
                                                        result = false;
                                                        return result;
                                                    case 202:
                                                        if (Session.GetHabbo().Rank >= 7)
                                                        {
                                                            try
                                                            {
                                                                string str = Input.Substring(10);
                                                                string username = Session.GetHabbo().Username;
                                                                HabboIM.GetGame().GetClientManager().method_18WD(username, str);
                                                            }
                                                            catch
                                                            {
                                                                Session.SendNotification("Bitte gebe einen Grund ein, weswegen du wieder da bist.");
                                                            }
                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            result = true;
                                                            return result;
                                                        }
                                                        result = false;
                                                        return result;
                                                    case 203:
                                                        if (Session.GetHabbo().Rank >= 6)
                                                        {
                                                            try
                                                            {
                                                                string TargetUser = Params[1];
                                                                TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(TargetUser);
                                                                if (TargetClient == null)
                                                                {
                                                                    Session.SendNotification("Konnte den User nicht finden: " + TargetUser);
                                                                    result = true;
                                                                    return result;
                                                                }
                                                                TargetClient.SendNotification("Deine Spielweise entspricht nicht unseren Regeln!\r\rVerhalte dich bitte unseren Regeln entsprechend.\r\rWeitere Verstöße führen zu einem Ausschluss aus der Community.");

                                                            }
                                                            catch
                                                            {
                                                                Session.SendNotification("Du musst einen Text eingeben");
                                                            }

                                                            result = true;
                                                            return result;
                                                        }
                                                        else
                                                        {

                                                        }
                                                        result = false;
                                                        return result;

                                                    case 204:
                                                        {
                                                            HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("8");
                                                            return true;
                                                        }
                                                    case 205:
                                                        HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("features");
                                                        return true;
                                                    case 206:
                                                        if (!Session.GetHabbo().HasFuse("cmd_vouchergame"))
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        string count2 = Params[2];
                                                        int count = HabboIM.StringToInt(count2);
                                                        string waehrung = Params[1];
                                                        Random Rnd = new Random(); // initialisiert die Zufallsklasse
                                                        int voucher = Rnd.Next();
                                                        string wae = "";
                                                        int taler = 0;
                                                        int dia = 0;
                                                        int akti = 0;

                                                        if (waehrung == "taler")
                                                        {
                                                            wae = "Taler";
                                                            taler = count;

                                                        }
                                                        else if (waehrung == "Sterne")
                                                        {
                                                            wae = "Sterne";
                                                            dia = count;
                                                        }
                                                        else if (waehrung == "enten")
                                                        {
                                                            wae = "Enten";
                                                            akti = count;
                                                        }
                                                        else
                                                        {
                                                            wae = "Taler";
                                                        }


                                                        string toSendd = "vouchergame|" + count + "|" + wae + "|" + voucher;
                                                        HabboIM.GetWebSocketManager().SendMessageToEveryConnection(toSendd);
                                                        using (DatabaseClient vouchergame = HabboIM.GetDatabase().GetClient())
                                                        {
                                                            vouchergame.ExecuteQuery(string.Concat(new object[]
                                    {
                                            "INSERT INTO vouchers (`code`,`credits`,`pixels`,`vip_points`,`userid`) VALUES ('",
                                            voucher,
                                            "','",
                                            taler,
                                            "','",
                                            akti,
                                            "','",
                                            dia,
                                            "','",
                                            Session.GetHabbo().Id,
                                            "');"
                                    }), 30);
                                                        }
                                                        HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);



                                                        return true;

                                                    case 207:
                                                        if (Session.GetHabbo().Rank < 6 && Session.GetHabbo().CurrentRoom.Owner != Session.GetHabbo().Username)
                                                        {
                                                            return false;
                                                        }
                                                        RoomUser class16 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        if (class16 != null)
                                                        {
                                                            GameClient gc = class16.GetClient();
                                                            //RoomUser class16 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            class16.HandleSpeech(gc, "Das Spiel startet in:", false);
                                                            System.Threading.Thread.Sleep(1000);
                                                            class16.HandleSpeech(gc, "5", false);
                                                            System.Threading.Thread.Sleep(1000);
                                                            class16.HandleSpeech(gc, "4", false);
                                                            System.Threading.Thread.Sleep(1000);
                                                            class16.HandleSpeech(gc, "3", false);
                                                            System.Threading.Thread.Sleep(1000);
                                                            class16.HandleSpeech(gc, "2", false);
                                                            System.Threading.Thread.Sleep(1000);
                                                            class16.HandleSpeech(gc, "1", false);
                                                            System.Threading.Thread.Sleep(1000);
                                                            class16.HandleSpeech(gc, "GOOOOOOOOOO", false);


                                                        }
                                                        return true;

                                                    case 208:
                                                        if (Session.GetHabbo().Rank > 8u)
                                                        {
                                                            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                                            {

                                                                dbClient.ExecuteQuery(string.Concat(new object[]
                                               {
                                                                    "UPDATE `cms_config` SET `option`='0' WHERE (`config`='register-open')"
                                               }));


                                                            }

                                                            Session.GetHabbo().Whisper("Du hast den Register erfolgreich deaktiviert!");
                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            return true;
                                                        }
                                                        else
                                                        {
                                                            Session.GetHabbo().Whisper("Du besitzt nicht die nötigen Rechte.");
                                                            return true;
                                                        }

                                                    case 209:
                                                        if (Session.GetHabbo().Rank > 8u)
                                                        {
                                                            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                                            {

                                                                dbClient.ExecuteQuery(string.Concat(new object[]
                                               {
                                                                    "UPDATE `cms_config` SET `option`='1' WHERE (`config`='register-open')"
                                               }));
                                                            }

                                                            Session.SendNotification("Du hast den Register erfolgreich aktiviert!");
                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            return true;
                                                        }
                                                        else
                                                        {
                                                            Session.SendNotification("Du besitzt nicht die nötigen Rechte.");
                                                            return true;
                                                        }

                                                    case 3000:
                                                        if (!Session.GetHabbo().HasFuse("cmd_spam"))
                                                        {
                                                            result = false;
                                                            return result;
                                                        }
                                                        RoomUser class17 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        if (class17 != null)
                                                        {
                                                            GameClient gc = class17.GetClient();
                                                            //RoomUser class16 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            class17.HandleSpeech(gc, "Werbeschutz!", false);
                                                            System.Threading.Thread.Sleep(150);
                                                            class17.HandleSpeech(gc, "Chat wird gecleart.", false);
                                                            System.Threading.Thread.Sleep(150);
                                                            class17.HandleSpeech(gc, "Chat wird gecleart.", false);
                                                            System.Threading.Thread.Sleep(150);
                                                            class17.HandleSpeech(gc, "Chat wird gecleart.", false);
                                                            System.Threading.Thread.Sleep(150);
                                                            class17.HandleSpeech(gc, "Chat wird gecleart.", false);
                                                            System.Threading.Thread.Sleep(150);
                                                            class17.HandleSpeech(gc, "Chat wird gecleart.", false);
                                                            System.Threading.Thread.Sleep(150);
                                                            class17.HandleSpeech(gc, "Chat wird gecleart.", false);


                                                        }
                                                        return true;


                                                    case 2081:
                                                        {

                                                            string text5 = Params[1];
                                                            bool flag2 = true;
                                                            if (string.IsNullOrEmpty(text5))
                                                            {
                                                                Session.GetHabbo().Whisper("Bitte ein Username eingeben.");
                                                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                                result = true;
                                                                return result;
                                                            }
                                                            GameClient class7 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text5);
                                                            Habbo class8;
                                                            if (class7 == null)
                                                            {
                                                                flag2 = false;
                                                                class8 = Authenticator.CreateHabbo(text5);
                                                            }
                                                            else
                                                            {
                                                                class8 = class7.GetHabbo();
                                                            }
                                                            if (class8 == null)
                                                            {
                                                                Session.GetHabbo().Whisper("User konnte nicht gefunden werden: " + Params[1]);
                                                                HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                                result = true;
                                                                return result;
                                                            }
                                                            if (class7 != null)
                                                            {
                                                                int numx = (int)HabboIM.GetUnixTimestamp() - class8.LoginTimestamp;

                                                                numx = numx / 60;

                                                                var adminprison = (class8.jail == 0) ? "Nein" : "Ja";
                                                                Session.GetHabbo().Whisper(class8.Username + " ist seit " + numx + " Minuten online. Gebannt: " + adminprison + "");


                                                            }
                                                            else
                                                            {
                                                                DateTime LastLoggedIn = HabboIM.TimestampToDate(double.Parse(class8.LastOnline));
                                                                var tach = LastLoggedIn.ToString("dd.MM.yyyy"); // Format wie 01.01.1900
                                                                var uhhr = LastLoggedIn.ToString("HH:mm"); // Format wie 01:59 oder 22:01


                                                                var adminprison = (class8.jail == 0) ? "Nein" : "Ja";

                                                                var text = "";
                                                                try
                                                                {

                                                                    double timestamp = HabboIM.GetUnixTimestamp();
                                                                    using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
                                                                    {




                                                                        text = dbClient.ReadString("SELECT value FROM bans WHERE value = '" + class8.Username + "' AND expire > " + timestamp + "  LIMIT 1;", 30);


                                                                    }
                                                                    if (text == class8.Username)
                                                                    {

                                                                        text = "Ja";
                                                                    }
                                                                    else
                                                                    {

                                                                        text = "Nein";
                                                                    }



                                                                }
                                                                catch
                                                                {

                                                                    text = "Unbekannt";
                                                                }

                                                                Session.GetHabbo().Whisper(class8.Username + " war zuletzt online am " + tach + " um " + uhhr + " Uhr - Gebannt: " + adminprison + "");


                                                            }

                                                            return true;
                                                        }

                                                    case 3010:
                                                        try
                                                        {

                                                            RoomUser class6_001;
                                                            GameClient User2 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                            class6_001 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);

                                                            RoomUser class4 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);

                                                            if (User2.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                            {
                                                                if (User2.GetHabbo().Id == Session.GetHabbo().Id)
                                                                {
                                                                    return false;
                                                                }
                                                                else
                                                                {



                                                                    bool flag3 = false;
                                                                    using (DatabaseClient client2 = HabboIM.GetDatabase().GetClient())
                                                                    {
                                                                        DataTable dataTable = client2.ReadDataTable("SELECT * FROM messenger_friendships", 30);
                                                                        foreach (DataRow dataRow in dataTable.Rows)
                                                                        {
                                                                            if (((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)User2.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)Session.GetHabbo().Id)) || ((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)Session.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)User2.GetHabbo().Id)))
                                                                            {
                                                                                flag3 = true;
                                                                            }
                                                                        }
                                                                    }

                                                                    if (flag3 == true)
                                                                    {
                                                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                                        RoomUser Aktuelleruser = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                                        RoomUser Usr2 = class2.GetRoomUserByHabbo(User2.GetHabbo().Id);
                                                                        if (Math.Abs(Aktuelleruser.X - Usr2.X) < 3 && Math.Abs(Aktuelleruser.Y - Usr2.Y) < 3)
                                                                        {
                                                                            Aktuelleruser.HandleSpeech(Session, "*Hat " + User2.GetHabbo().Username + " geknuddelt!*", false);
                                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(9, true);
                                                                            User2.GetHabbo().GetEffectsInventoryComponent().method_2(9, true);
                                                                            System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
                                                                            {
                                                                                try
                                                                                {
                                                                                    System.Threading.Thread.Sleep(5000);
                                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(9, true);
                                                                                    User2.GetHabbo().GetEffectsInventoryComponent().method_2(9, true);
                                                                                }
                                                                                catch
                                                                                {

                                                                                }
                                                                            });
                                                                            thrd.Start();
                                                                            return true;
                                                                        }
                                                                        else
                                                                        {
                                                                            Session.GetHabbo().Whisper("Ihr seid zu weit weg voneinander");
                                                                            return true;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Session.GetHabbo().Whisper("Ihr müsst Freunde sein, um euch zu knuddeln!");
                                                                        return true;
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                return false;
                                                            }
                                                        }

                                                        catch
                                                        {
                                                            return false;
                                                        }

                                                    case 999983:
                                                        {
                                                            int numzahl = (int)Convert.ToInt16(Params[1]);
                                                            if (numzahl < 0 || numzahl > 3)
                                                            {
                                                                Session.GetHabbo().Whisper("Wähle eine Tanz-ID zwischen 1-3 aus!");
                                                                result = true;
                                                                return result;
                                                            }
                                                            if (numzahl >= 0 && numzahl <= 3)
                                                            {
                                                                int zahlmark = 530 + numzahl;
                                                                if (Session.GetHabbo().GetEffectsInventoryComponent().int_0 != zahlmark)
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(zahlmark, true);
                                                                    Thread threadzahl = new Thread(delegate ()
                                                                    {
                                                                        try
                                                                        {
                                                                            Thread.Sleep(30000);
                                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                        }
                                                                        catch
                                                                        {
                                                                        }
                                                                    });
                                                                    threadzahl.Start();
                                                                    result = true;
                                                                    return result;
                                                                }
                                                                result = true;
                                                                return result;
                                                            }
                                                            else
                                                            {
                                                                if (numzahl != 3)
                                                                {
                                                                    result = true;
                                                                    return result;
                                                                }
                                                                if (Session.GetHabbo().GetEffectsInventoryComponent().int_0 != 533)
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(533, true);
                                                                    Thread threadzahl = new Thread(delegate ()
                                                                    {
                                                                        try
                                                                        {
                                                                            Thread.Sleep(30000);
                                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                        }
                                                                        catch
                                                                        {
                                                                        }
                                                                    });
                                                                    threadzahl.Start();
                                                                    result = true;
                                                                    return result;
                                                                }
                                                                result = true;
                                                                return result;
                                                            }
                                                        }

                                                    case 999984:
                                                        {
                                                            if (Session.GetHabbo().Rank > 1u)
                                                            {
                                                                string Message = Input.Substring(7).ToLower();

                                                                if (Message == "weiss")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(600, true);
                                                                }
                                                                else if (Message == "pink")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(602, true);
                                                                }
                                                                else if (Message == "lila")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(603, true);
                                                                }
                                                                else if (Message == "gold")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(599, true);
                                                                }
                                                                else if (Message == "schwarz")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(598, true);
                                                                }
                                                                else if (Message == "0")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                }
                                                                else
                                                                {
                                                                    Session.SendNotification("Folgende Farben kannst du für dein IO Hawk nehmen:\r\r- weiss\r- gold\r- lila\r- pink\r- schwarz\r\rFarbe ausgesucht?\rSchreibe :iohawk <farbe>");
                                                                }

                                                                return true;
                                                            }

                                                            return false;
                                                        }


                                                    case 999987:
                                                        {
                                                            if (Session.GetHabbo().Rank > 1u)
                                                            {
                                                                string Message = Input.Substring(5).ToLower();

                                                                if (Message == "gut")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(701, true);
                                                                    Session.GetHabbo().Whisper("Du fühlst dich gut!");
                                                                }
                                                                else if (Message == "schlecht")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(707, true);
                                                                    Session.GetHabbo().Whisper("Du fühlst dich schlecht!");
                                                                }
                                                                else if (Message == "cool")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(706, true);
                                                                    Session.GetHabbo().Whisper("Du fühlst dich cool!");
                                                                }
                                                                else if (Message == "angry")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(708, true);
                                                                    Session.GetHabbo().Whisper("Du bist wütend!");
                                                                }
                                                                else if (Message == "verliebt")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(710, true);
                                                                    Session.GetHabbo().Whisper("Du bist verliebt! Wie süß.");
                                                                }
                                                                else if (Message == "bobba")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(711, true);
                                                                    Session.GetHabbo().Whisper("bobba bobba bobba!");
                                                                }
                                                                else if (Message == "surprised")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(703, true);
                                                                    Session.GetHabbo().Whisper("Du bist überrascht!");
                                                                }
                                                                else if (Message == "happy")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(700, true);
                                                                    Session.GetHabbo().Whisper("Du bist glücklich!");
                                                                }
                                                                else if (Message == "0")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                }
                                                                else
                                                                {
                                                                    Session.SendNotification("Zeige anderen wie du dich fühlst!\r\rWähle aus folgenden Gefühlen aus:\r\r- gut\r- schlecht\r- cool\r- angry\r- verliebt\r- bobba\r- surprised\r- happy\r\rGebe anschließend :feel <gefühl> ein!");
                                                                }

                                                                return true;
                                                            }

                                                            return false;
                                                        }

                                                    case 999985:
                                                        {
                                                            if (Session.GetHabbo().Rank > 1u)
                                                            {
                                                                string Message = Input.Substring(9).ToLower();

                                                                if (Message == "1")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(604, true);
                                                                }
                                                                else if (Message == "2")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(609, true);
                                                                }
                                                                else if (Message == "3")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(610, true);
                                                                }
                                                                else if (Message == "0")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                }
                                                                else
                                                                {
                                                                    Session.GetHabbo().Whisper("Wähle eine Sprung-ID von 1-3 aus!");
                                                                }

                                                                return true;
                                                            }

                                                            return false;
                                                        }


                                                    case 999986:
                                                        {
                                                            if (Session.GetHabbo().Rank > 1u)
                                                            {
                                                                string Message = Input.Substring(7).ToLower();

                                                                if (Message == "1")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(605, true);
                                                                }
                                                                else if (Message == "2")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(601, true);
                                                                }
                                                                else if (Message == "0")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                }
                                                                else
                                                                {
                                                                    Session.GetHabbo().Whisper("Wähle eine Lauf-ID von 1-2 aus!");
                                                                }

                                                                return true;
                                                            }

                                                            return false;
                                                        }


                                                    case 999988:
                                                        {
                                                            if (Session.GetHabbo().Rank > 1u)
                                                            {
                                                                string Message = Input.Substring(6).ToLower();

                                                                if (Message == "1")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(611, true);
                                                                    Session.GetHabbo().Whisper("Du trägst Waffen-ID: 1");
                                                                }
                                                                else if (Message == "2")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(612, true);
                                                                    Session.GetHabbo().Whisper("Du trägst Waffen-ID: 2");
                                                                }
                                                                else if (Message == "3")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(615, true);
                                                                    Session.GetHabbo().Whisper("Du trägst Waffen-ID: 3");
                                                                }
                                                                else if (Message == "4")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(616, true);
                                                                    Session.GetHabbo().Whisper("Du trägst Waffen-ID: 4");
                                                                }
                                                                else if (Message == "0")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                                    Session.GetHabbo().Whisper("Waffe abgelegt!");
                                                                }
                                                                else
                                                                {
                                                                    Session.GetHabbo().Whisper("Wähle eine Waffen-ID von 1-4 aus!");
                                                                }

                                                                return true;
                                                            }

                                                            return false;
                                                        }


                                                    case 999989:
                                                        {
                                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(617, true);
                                                            class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            class5.HandleSpeech(Session, "*Fliegt weg*", false);
                                                            return true;
                                                        }


                                                    case 999990:
                                                        {
                                                            if (Session.GetHabbo().Rank > 1u)
                                                            {
                                                                string Message = Input.Substring(6).ToLower();

                                                                if (Message == "1")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(680, true);
                                                                    Session.GetHabbo().Whisper("du bist jetzt 1 mini fabsi");
                                                                }
                                                                else if (Message == "2")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(654, true);
                                                                    Session.GetHabbo().Whisper("du trägst jetzt 1 mini fabsi auf deinem kopf!!");
                                                                }
                                                                else
                                                                {
                                                                    Session.GetHabbo().Whisper("wähle eine fabsi id zwischen 1-2 aus meeeen!");
                                                                }

                                                                return true;
                                                            }

                                                            return false;
                                                        }


                                                    case 999991:
                                                        {
                                                            if (Session.GetHabbo().Rank > 1u)
                                                            {
                                                                string Message = Input.Substring(6).ToLower();

                                                                if (Message == "1")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(681, true);
                                                                    Session.GetHabbo().Whisper("du bist jetzt 1 mini görksi");
                                                                }
                                                                else if (Message == "2")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(655, true);
                                                                    Session.GetHabbo().Whisper("du trägst jetzt 1 mini görksi auf deinem kopf!!");
                                                                }
                                                                else
                                                                {
                                                                    Session.GetHabbo().Whisper("wähle eine görksi id zwischen 1-2 aus meeeen!");
                                                                }

                                                                return true;
                                                            }

                                                            return false;
                                                        }


                                                    case 999992:
                                                        {
                                                            if (Session.GetHabbo().Rank > 1u)
                                                            {
                                                                string Message = Input.Substring(5).ToLower();

                                                                if (Message == "MaruanKR7")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(900, true);
                                                                    Session.GetHabbo().Whisper("Du bist jetzt ein mini MaruanKR7!");
                                                                }
                                                                else if (Message == "iKlex")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(901, true);
                                                                    Session.GetHabbo().Whisper("Du bist jetzt ein mini iKlex!");
                                                                }
                                                                else if (Message == "Merlin")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(902, true);
                                                                    Session.GetHabbo().Whisper("Du bist jetzt ein mini Merlin!");
                                                                }
                                                                else if (Message == "Fytos")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(903, true);
                                                                    Session.GetHabbo().Whisper("Du bist jetzt ein mini Fytos!");
                                                                }
                                                                else if (Message == "Iwan")
                                                                {
                                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(904, true);
                                                                    Session.GetHabbo().Whisper("Du bist jetzt ein mini Iwan!");
                                                                }
                                                                else
                                                                {
                                                                    Session.GetHabbo().Whisper("Oops, dieser User hat noch kein eigenes Enable! Vielleicht vertippt?");
                                                                }

                                                                return true;
                                                            }

                                                            return false;
                                                        }
                                                    /*case 28012000:
                                                        {
                                                            HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("befehle");
                                                            return true;
                                                        }*/
                                                    case 999993:
                                                        if (Session.GetHabbo().Rank >= 7)
                                                        {
                                                            try
                                                            {
                                                                string username = Session.GetHabbo().Username;
                                                                HabboIM.GetGame().GetClientManager().method_18AD(username);
                                                                Session.GetHabbo().GetEffectsInventoryComponent().method_2(650, true);
                                                                Session.SendNotification("Du bist nun im Admin-Dienst!");
                                                            }
                                                            catch
                                                            {
                                                                Session.GetHabbo().Whisper("Du besitzt nicht die nötigen Rechte.");
                                                            }

                                                            HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                            result = true;
                                                            return result;
                                                        }
                                                        result = false;
                                                        return result;


                                                    case 999994:
                                                        try
                                                        {


                                                            string created = Session.GetHabbo().DataCadastro;
                                                            int createdint = Convert.ToInt32(created);
                                                            int createdcheck = createdint + (7 * 60 * 60 * 24);
                                                            int timenow = (int)HabboIM.GetUnixTimestamp();

                                                            if (Session.GetHabbo().AchievementScore > 1100 && timenow > createdcheck)
                                                            {

                                                                using (DatabaseClient client = HabboIM.GetDatabase().GetClient())
                                                                {
                                                                    DataRow dataRow = client.ReadDataRow("SELECT * FROM friend_stream WHERE userid='" + Session.GetHabbo().Id.ToString() + "' ORDER BY time DESC LIMIT 1");
                                                                    double unixTimestamp = HabboIM.GetUnixTimestamp();
                                                                    if (dataRow == null)
                                                                    {
                                                                        string str6 = HabboIM.FilterText(Input.Remove(0, 6));



                                                                        if (smethod_4b(Session, str6, "GC") != str6)
                                                                        {

                                                                            Session.GetHabbo().Whisper("Dein Eintrag wurde nicht gepostet: Verdacht auf Fremdwerbung!");

                                                                            return true;
                                                                        }


                                                                        if (smethod_4(str6) != str6)
                                                                        {

                                                                            Session.GetHabbo().Whisper("Dein Eintrag wurde nicht gepostet: Beleidigungen im Friendstream sind verboten!");
                                                                            return true;
                                                                        }



                                                                        string str14 = HabboIM.FilterText(Session.GetHabbo().Id.ToString());
                                                                        string str15 = HabboIM.FilterText(Session.GetHabbo().Gender.ToString());
                                                                        string str16 = HabboIM.FilterText(Session.GetHabbo().Figure.ToString());
                                                                        client.AddParamWithValue("@endstring", (object)str6);
                                                                        client.ExecuteQuery("INSERT INTO friend_stream (`type`,`userid`,`gender`,`look`,`time`,`data`,`data_extra`) VALUES ('5','" + str14 + "','" + str15 + "','" + str16 + "','" + (object)unixTimestamp + "', @endstring, '5');");
                                                                        Session.SendNotification("Friendstream Eintrag erfolgreich erstellt!");
                                                                    }
                                                                    else
                                                                    {
                                                                        double num10 = (double)dataRow["time"];
                                                                        if (unixTimestamp - num10 > 900.0)
                                                                        {
                                                                            string str6 = HabboIM.DoFilter(Input.Remove(0, 6), true, true);




                                                                            if (smethod_4b(Session, str6, "GC") != str6)
                                                                            {

                                                                                Session.GetHabbo().Whisper("Dein Eintrag wurde nicht gepostet: Verdacht auf Fremdwerbung!");

                                                                                return true;
                                                                            }


                                                                            if (smethod_4(str6) != str6)
                                                                            {

                                                                                Session.GetHabbo().Whisper("Dein Eintrag wurde nicht gepostet: Beleidigungen sind im Friendstream verboten!");
                                                                                return true;
                                                                            }







                                                                            string str14 = HabboIM.DoFilter(Session.GetHabbo().Id.ToString(), true, true);
                                                                            string str15 = HabboIM.DoFilter(Session.GetHabbo().Gender.ToString(), true, true);
                                                                            string str16 = HabboIM.DoFilter(Session.GetHabbo().Figure.ToString(), true, true);
                                                                            client.AddParamWithValue("@endstring", (object)str6);
                                                                            client.ExecuteQuery("INSERT INTO friend_stream (`type`,`userid`,`gender`,`look`,`time`,`data`,`data_extra`) VALUES ('5','" + str14 + "','" + str15 + "','" + str16 + "','" + (object)unixTimestamp + "', @endstring, '5');");
                                                                            Session.SendNotification("Friendstream Eintrag erfolgreich erstellt!");
                                                                        }
                                                                        else
                                                                            Session.SendNotification("Du kannst nur alle 15 Minuten einen Friendstream Eintrag erstellen! Versuchs später nochmal.");
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                Session.SendNotification("Um einen Friendstream Eintrag posten zu können, benötigst du 1100 Erfahrungspunkte und musst mindestens 7 Tage im Habbo registriert sein!");




                                                            }

                                                            return true;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.WriteLine(ex.ToString());
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            return false;
                                                        }

                                                    case 999995:
                                                        RoomUser roomUserByHabbo14 = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        if (int.Parse(Params[1]) > 0 && int.Parse(Params[1]) < 7)
                                                            roomUserByHabbo14.method_9(int.Parse(Params[1]));
                                                        return true;

                                                    /* case 194:
                                                         try
                                                         {
                                                             GameClient User2 = HabboIM.GetGame().GetClientManager().GetClientByHabbo(Params[1].ToString());
                                                             bool flag3 = false;
                                                             using (DatabaseClient client2 = HabboIM.GetDatabase().GetClient())
                                                             {
                                                                 DataTable dataTable = client2.ReadDataTable("SELECT * FROM messenger_friendships", 30);
                                                                 foreach (DataRow dataRow in dataTable.Rows)
                                                                 {
                                                                     if (((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)User2.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)Session.GetHabbo().Id)) || ((long)Convert.ToInt32(dataRow["user_one_id"].ToString()) == (long)((ulong)Session.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["user_two_id"]) == (long)((ulong)User2.GetHabbo().Id)))
                                                                     {
                                                                         flag3 = true;
                                                                     }
                                                                 }
                                                             }
                                                             if (flag3 == true)
                                                             {
                                                                 try
                                                                 {
                                                                     string TargetUser = Params[1];
                                                                     TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(TargetUser);
                                                                     bool areinlove = false;
                                                                     using (DatabaseClient client2 = HabboIM.GetDatabase().GetClient())
                                                                     {
                                                                         DataTable dataTable = client2.ReadDataTable("SELECT * FROM aff_beziehungen", 30);
                                                                         foreach (DataRow dataRow in dataTable.Rows)
                                                                         {
                                                                             if (((long)Convert.ToInt32(dataRow["spieler1"].ToString()) == (long)((ulong)User2.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["spieler2"]) == (long)((ulong)Session.GetHabbo().Id)) || ((long)Convert.ToInt32(dataRow["spieler1"].ToString()) == (long)((ulong)Session.GetHabbo().Id) && (long)Convert.ToInt32(dataRow["spieler2"]) == (long)((ulong)User2.GetHabbo().Id)))
                                                                             {
                                                                                 areinlove = true;
                                                                             }
                                                                         }
                                                                     }
                                                                     if (areinlove == false)
                                                                     {
                                                                         if (TargetClient == null)
                                                                         {
                                                                             Session.SendNotification("Konnte den User nicht finden: " + TargetUser);
                                                                             result = true;
                                                                             return result;
                                                                         }
                                                                         Session.GetHabbo().Whisper("Du hast " + TargetClient.GetHabbo().Username + " gefragt ob er/sie mit dir zusammen sein will!");

                                                                         //////
                                                                         TargetClient.GetHabbo().Whisper("Nachricht von " + Session.GetHabbo().Username + ": " + ChatCommandHandler.MergeParams(Params, 2));
                                                                     }
                                                                     else
                                                                     {
                                                                         Session.GetHabbo().Whisper("Du bist bereits in einer Beziehung!");
                                                                         result = true;
                                                                         return result;
                                                                     }
                                                                 }
                                                                 catch
                                                                 {
                                                                     Session.SendNotification("Du musst einen Namen eingeben");
                                                                 }

                                                                 result = true;
                                                                 return result;
                                                             }
                                                             else
                                                             {
                                                                 Session.GetHabbo().Whisper("Du musst mit dem User/ der Userin befreundet sein, um mit ihm/ihr eine Beziehung zu haben!");
                                                                 result = true;
                                                                 return result;
                                                             }

                                                         }
                                                         catch
                                                         {
                                                             return false;
                                                         } */
                                                    default:
                                                        goto IL_ABF2;
                                                }
                                                using (DatabaseClient stafflist = HabboIM.GetDatabase().GetClient())
                                                {
                                                    DataTable Staffs = stafflist.ReadDataTable("SELECT * FROM users WHERE rank > '11' AND online = '1'", 30);
                                                    if (Staffs.Rows.Count <= 0)
                                                    {
                                                        Session.SendNotification("Kein Mitarbeiter ist gerade online!");
                                                        result = true;
                                                        return result;
                                                    }
                                                    StringBuilder StringStaff = new StringBuilder();
                                                    DataTable Staff = stafflist.ReadDataTable("SELECT * FROM users WHERE rank > '11' AND online = '1'", 30);
                                                    foreach (DataRow StaffRow in Staff.Rows)
                                                    {
                                                        Habbo classUser = Authenticator.CreateHabbo((string)StaffRow["username"]);
                                                        GameClient class7 = HabboIM.GetGame().GetClientManager().GetClientByHabbo((string)StaffRow["username"]);
                                                        classUser = class7.GetHabbo();
                                                        if (classUser.CurrentRoom != null)
                                                        {
                                                            StringStaff.Append(string.Concat(new string[]
                                            {
                                                "-- ",
                                                (string)StaffRow["username"],
                                                " (Rank: ",
                                                classUser.working,
                                                ")\n-- Raum: ",
                                                classUser.CurrentRoom.Name,
                                                "\n\n"
                                            }));
                                                        }
                                                        else
                                                        {
                                                            StringStaff.Append(string.Concat(new string[]
                                            {
                                                "-- ",
                                                (string)StaffRow["username"],
                                                " (Rank: ",
                                                classUser.working,
                                                ")\n-- befindet sich in keinem Raum\n\n"
                                            }));
                                                        }
                                                    }
                                                    Session.SendNotification(string.Concat(new object[]
                                    {
                                        "Staffs Online:\n~~~~~~~~~~~~~~\n\n",
                                        StringStaff.ToString()
                                    }));
                                                    result = true;
                                                    return result;
                                                }
                                            IL_8648:
                                                if (!Session.GetHabbo().HasFuse("cmd_raumalert"))
                                                {
                                                    result = false;
                                                    return result;
                                                }
                                                if (!Session.GetHabbo().Raumalert)
                                                {
                                                    Session.SendNotification("Aufgrund eines Regelverstoßes wurde dir dieser Befehl entzogen!");
                                                    result = true;
                                                    return result;
                                                }
                                                if (!(Session.GetHabbo().CurrentRoom.Owner == Session.GetHabbo().Username))
                                                {
                                                    Session.SendNotification("Du kannst nur in deinen eigenen Räumen einen Alert versenden!");
                                                    result = true;
                                                    return result;
                                                }
                                                class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                if (class2 == null)
                                                {
                                                    result = false;
                                                    return result;
                                                }
                                                string_ = ChatCommandHandler.MergeParams(Params, 1);
                                                if (string_ != "")
                                                {
                                                    for (int i = 0; i < class2.RoomUsers.Length; i++)
                                                    {
                                                        RoomUser roomUser4 = class2.RoomUsers[i];
                                                        if (roomUser4 != null)
                                                        {
                                                            string roomowner = Session.GetHabbo().CurrentRoom.Owner;
                                                            string message = string.Concat(new object[]
                                            {
                                                "Raumalert vom Raumbesitzer:\n\n ",
                                                string_,
                                                "\n\n- ",
                                                roomowner
                                            });
                                                            roomUser4.GetClient().SendNotification(message);
                                                        }
                                                    }
                                                    HabboIM.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                    result = true;
                                                    return result;
                                                }
                                                Session.SendNotification("Raumalert darf nicht leer sein!");
                                                result = true;
                                                return result;
                                            IL_ABF2:
                                                goto IL_B3BE;
                                            }
                                    }
                                    try
                                    {
                                        if (!ServerConfiguration.UnknownBoolean1 && !Session.GetHabbo().HasFuse("cmd_push"))
                                        {
                                            Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_error_disabled"));
                                            result = true;
                                            return result;
                                        }
                                        if (!Session.GetHabbo().IsVIP && !Session.GetHabbo().HasFuse("cmd_push"))
                                        {
                                            Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_error_permission_vip"));
                                            result = true;
                                            return result;
                                        }
                                        string a = "down";
                                        string text = Params[1];
                                        TargetClient = HabboIM.GetGame().GetClientManager().GetClientByHabbo(text);
                                        class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                        if (Session == null || TargetClient == null)
                                        {
                                            result = false;
                                            return result;
                                        }
                                        class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                        RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                        if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                        {
                                            Session.GetHabbo().Whisper("Du kannst dich nicht selber pushen!");
                                            result = true;
                                            return result;
                                        }
                                        bool arg_3DD2_0;
                                        if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                        {
                                            if ((class5.X + 1 != class4.X || class5.Y != class4.Y) && (class5.X - 1 != class4.X || class5.Y != class4.Y) && (class5.Y + 1 != class4.Y || class5.X != class4.X))
                                            {
                                                if (class5.Y - 1 == class4.Y)
                                                {
                                                    if (class5.X == class4.X)
                                                    {
                                                        goto IL_AE78;
                                                    }
                                                }
                                                arg_3DD2_0 = (class5.X != class4.X || class5.Y != class4.Y);
                                                goto IL_AE83;
                                            }
                                        IL_AE78:
                                            arg_3DD2_0 = false;
                                        }
                                        else
                                        {
                                            arg_3DD2_0 = true;
                                        }
                                    IL_AE83:
                                        if (!arg_3DD2_0)
                                        {
                                            class5.HandleSpeech(Session, "*schubst " + TargetClient.GetHabbo().Username + "*", false);
                                            if (class5.BodyRotation == 0)
                                            {
                                                a = "up";
                                            }
                                            if (class5.BodyRotation == 2)
                                            {
                                                a = "right";
                                            }
                                            if (class5.BodyRotation == 4)
                                            {
                                                a = "down";
                                            }
                                            if (class5.BodyRotation == 6)
                                            {
                                                a = "left";
                                            }
                                            if (a == "up")
                                            {
                                                if (ServerConfiguration.PreventDoorPush)
                                                {
                                                    if (class4.X != class2.RoomModel.int_0 || class4.Y - 1 != class2.RoomModel.int_1 || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                    {
                                                        class4.MoveTo(class4.X, class4.Y - 1);
                                                    }
                                                }
                                                else
                                                {
                                                    class4.MoveTo(class4.X, class4.Y - 1);
                                                }
                                            }
                                            if (a == "right")
                                            {
                                                if (ServerConfiguration.PreventDoorPush)
                                                {
                                                    if (class4.X + 1 != class2.RoomModel.int_0 || class4.Y != class2.RoomModel.int_1 || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                    {
                                                        class4.MoveTo(class4.X + 1, class4.Y);
                                                    }
                                                }
                                                else
                                                {
                                                    class4.MoveTo(class4.X + 1, class4.Y);
                                                }
                                            }
                                            if (a == "down")
                                            {
                                                if (ServerConfiguration.PreventDoorPush)
                                                {
                                                    if (class4.X != class2.RoomModel.int_0 || class4.Y + 1 != class2.RoomModel.int_1 || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                    {
                                                        class4.MoveTo(class4.X, class4.Y + 1);
                                                    }
                                                }
                                                else
                                                {
                                                    class4.MoveTo(class4.X, class4.Y + 1);
                                                }
                                            }
                                            if (a == "left")
                                            {
                                                if (ServerConfiguration.PreventDoorPush)
                                                {
                                                    if (class4.X - 1 != class2.RoomModel.int_0 || class4.Y != class2.RoomModel.int_1 || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                    {
                                                        class4.MoveTo(class4.X - 1, class4.Y);
                                                    }
                                                }
                                                else
                                                {
                                                    class4.MoveTo(class4.X - 1, class4.Y);
                                                }
                                            }
                                        }
                                        result = true;
                                        return result;
                                    }
                                    catch
                                    {
                                        result = false;
                                        return result;
                                    }
                                IL_B1EA:
                                    class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    class5 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    if (class5.Boolean_3)
                                    {
                                        Session.GetHabbo().Whisper("Command kann nicht währrend eines Tauschvorgangs benutzt werden. ");
                                        result = true;
                                        return result;
                                    }
                                    if (ServerConfiguration.EnableRedeemCredits)
                                    {
                                        Session.GetHabbo().GetInventoryComponent().ConvertCoinsToCredits();
                                    }
                                    else
                                    {
                                        Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("cmd_error_disabled"));
                                    }
                                    result = true;
                                    return result;
                                }
                        }
                    }
                IL_B2A3:
                    DateTime now = DateTime.Now;
                    TimeSpan timeSpan = now - HabboIM.ServerStarted;
                    int clients = HabboIM.GetGame().GetClientManager().ClientCount;
                    int rooms = HabboIM.GetGame().GetRoomManager().LoadedRoomsCount;
                    string text9 = "";
                    if (ServerConfiguration.ShowUsersAndRoomsInAbout)
                    {
                        text9 = string.Concat(new object[]
                        {
                            "\n\nUsers Online: ",
                            clients,
                            "\nRäume geladen: ",
                            rooms
                        });
                    }
                    Session.method_10(string.Concat(new object[]
                    {
                        "Phoenix Emulator\nedited by Affectum for Hobba.IN\n\nThanks/Credits:\nAffectum [DEV]\nJuniori [GTE Emu]\nSojobo [Phoenix Emu]\nMatty [Phoenix Emu]\nRoy [Uber Emu]\n\n",

                        "\nOnline seit: ",
                        timeSpan.Days,
                        " Tagen, ",
                        timeSpan.Hours,
                        " Stunden und ",
                        timeSpan.Minutes,
                        " Minuten",
                        text9
                    }), "");
                    result = true;
                    return result;
                IL_B3BE:;
                }
                catch
                {
                }
                result = false;
            }
            return result;
        }

        public static string MergeParams(string[] Params, int Start)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < Params.Length; i++)
            {
                if (i >= Start)
                {
                    if (i > Start)
                    {
                        stringBuilder.Append(" ");
                    }
                    stringBuilder.Append(Params[i]);
                }
            }
            return stringBuilder.ToString();
        }
    }
}