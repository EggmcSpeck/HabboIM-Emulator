using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using HabboIM.Core;
using HabboIM.HabboHotel;
using HabboIM.Net;
using HabboIM.Storage;
using HabboIM.Util;
using HabboIM.Communication;
using HabboIM.Messages;
using System.Net;
using System.IO;
using System.Globalization;
namespace HabboIM.HabboHotel.AntiAd
{
    public class AntiAd
    {
        public List<string> IllegalWords = new List<string>();
        public AntiAd()
        {
            IllegalWords.Clear();
            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
            {
                DataTable dt1 = dbClient.ReadDataTable("SELECT * FROM wordfilter");
                foreach (DataRow dr in dt1.Rows)
                {
                    IllegalWords.Add((string)dr["word"]);
                }
            }
        }
        public void Refresh()
        {
            IllegalWords.Clear();
            using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
            {
                DataTable dt1 = dbClient.ReadDataTable("SELECT * FROM wordfilter");
                foreach (DataRow dr in dt1.Rows)
                {
                    Console.WriteLine(dr["word"].ToString());
                    IllegalWords.Add((string)dr["word"]);
                }
            }
        }
        public bool ContainsIllegalWord(string t)
        {
            string s = Utf8ToUtf16(t).ToLower();
            string txt = s.Replace(" ", "").Replace("+", "").Replace("«", "").Replace("»", "").Replace("?", "").Replace("'", "").Replace("*", "").Replace("~", "").Replace("¥", "").Replace("(", "").Replace(")", "").Replace("=", "").Replace("&", "").Replace("§", "").Replace(":", "").Replace(".", "").Replace(";", "").Replace(",", "").Replace("[", "").Replace("]", "").Replace("^", "").Replace("´", "").Replace("`", "").Replace("_", "").Replace("°", "").Replace("•", "").Replace("<", "").Replace(">", "").Replace("#", "").Replace("¦", "").Replace("{", "").Replace("}", "").Replace("¬", "").Replace("§", "").Replace("÷", "").Replace("%", "").Replace("|", "").Replace('"'.ToString(), "");

            string txt1 = txt.Replace("/-/", "h"); // H
            string txt2 = txt1.Replace("ø", "o").Replace("Ø", "o").Replace("ô", "o").Replace("ø", "o").Replace("ó", "o").Replace("ò", "o").Replace("ö", "o").Replace("0", "o").Replace("|", "o"); // o
            string txt3 = txt2.Replace("û", "u").Replace("ú", "u").Replace("ù", "u").Replace("ü", "u").Replace("Ü", "u"); // u
            string txt4 = txt3.Replace("î", "i").Replace("í", "i").Replace("ì", "i").Replace("!", "i").Replace("1", "i").Replace("ï", "i"); // i
            string txt5 = txt4.Replace("â", "a").Replace("å", "a").Replace("á", "a").Replace("à", "a").Replace("@", "a").Replace("ä", "a").Replace("Ä", "a"); // a
            string txt6 = txt5.Replace("ê", "e").Replace("é", "e").Replace("è", "e").Replace("3", "e").Replace("€", "e").Replace("ë", "e"); // e
            string txt7 = txt6.Replace("ñ", "n"); // n
            string txt8 = txt7.Replace("ÿ", "y").Replace("ý", "y"); // y
            string txt9 = txt8.Replace("ç", "c"); // c
            string txt10 = txt9.Replace("×", "x"); // x
            string txt11 = txt10.Replace("5", "s"); // s


            string txt50 = txt11.Replace("ie", "b"); // DOPPELZAHLEN
            string txt60 = txt11.Replace("/", "").Replace("-", "").Replace("!", "").Replace("$", "").Replace("|", "").Replace("5", "5");  // BB
            string txt70 = txt11.Replace("!", "i").Replace("$", "s").Replace("v", "u").Replace("/", ""); // sonstige veränderungen


            foreach (string word in IllegalWords)
            {
                string word2 = word.ToLower();
                if (txt11.Contains(word2) || txt50.Contains(word2) || txt60.Contains(word2) || txt70.Contains(word2))
                    return true;
            }
            return false;
        }
        public static string Utf8ToUtf16(string utf8String)
        {
            List<byte> list = new List<byte>(utf8String.Length);
            for (int i = 0; i < utf8String.Length; i++)
            {
                byte b = (byte)utf8String[i];
                if (b > 0)
                {
                    list.Add(b);
                }
            }
            return Encoding.UTF8.GetString(list.ToArray());
        }
        public void SendStaffAlert(String message, String Username)
        {
            ServerMessage Message = new ServerMessage(134u);
            Message.AppendUInt(0u);
            Message.AppendString("[AWS] %u:\"%t\"".Replace("%u", Username).Replace("%t", message));
            HabboIM.GetGame().GetClientManager().GetClientByHabbo(Username).GetHabbo().Whisper("AWS: Dein Satz konnte nicht abgeschickt werden. Bitte verwende keine Beleidigungen oder Links in deinen Sätzen!");
            HabboIM.GetGame().GetClientManager().SendToStaffs(Message, Message);
        }
    }
}
