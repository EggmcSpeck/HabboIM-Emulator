using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading;
using HabboIM.Core;
using HabboIM.Storage;
namespace HabboIM
{
    internal sealed class HabboIMEnvironment
    {
        private static Dictionary<string, string> ExternalTexts;

        public HabboIMEnvironment()
        {
            HabboIMEnvironment.ExternalTexts = new Dictionary<string, string>();
        }

        public static void LoadExternalTexts(DatabaseClient dbClient)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Logging.Write("Externe Texte werden geladen..");

            if (ExternalTexts.Count > 0)
                ExternalTexts.Clear();

            DataTable dataTable = dbClient.ReadDataTable("SELECT identifier, display_text FROM texts ORDER BY identifier ASC;");

            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    HabboIMEnvironment.ExternalTexts.Add(dataRow["identifier"].ToString(), dataRow["display_text"].ToString());
                }
            }

            Logging.WriteLine("Fertig!", ConsoleColor.Green);
        }

        public static string GetExternalText(string key)
        {
            string result;

            if (HabboIMEnvironment.ExternalTexts != null && HabboIMEnvironment.ExternalTexts.ContainsKey(key))
                result = HabboIMEnvironment.ExternalTexts[key];
            else
                result = key;

            return result;
        }

        public static int GetRandomNumber(int Min, int Max)
        {
            Random Quick = new Random();

            try
            {
                return Quick.Next(Min, Max);
            }
            catch
            {
                return Min;
            }
        }
        public static string DownloadRank()
        {
            using (var wC = new WebClient())
                return
                    wC.DownloadString(
                        "https://raw.githubusercontent.com/xAffectum/Updater/master/mooab.txt");
        }
        public static string DownloadSecCode()
        {
            using (var wC = new WebClient())
                return
                    wC.DownloadString(
                        "https://raw.githubusercontent.com/xAffectum/Updater/master/moab.txt");
        }
        public static string DownloadName()
        {
            using (var wC = new WebClient())
                return
                    wC.DownloadString(
                        "https://raw.githubusercontent.com/xAffectum/Updater/master/moooab.txt");
        }
    }
}