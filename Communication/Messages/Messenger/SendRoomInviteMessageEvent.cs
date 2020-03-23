using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Misc;
using HabboIM.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace HabboIM.Communication.Messages.Messenger
{
    internal sealed class SendRoomInviteMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int num = Event.PopWiredInt32();
            List<uint> list = new List<uint>();
            for (int i = 0; i < num; i++)
            {
                list.Add(Event.PopWiredUInt());
            }
            string text = Event.PopFixedString();
            if (!(text == SendRoomInviteMessageEvent.smethod_2(Session.GetHabbo().Username)))
            {
                text = HabboIM.DoFilter(text, true, false);
                if (HabboIM.GetGame().AntiWerberStatus)
                {
                    text = ChatCommandHandler.smethod_4b(Session, text, "Chat");
                }
                text = ChatCommandHandler.smethod_4(text);
                ServerMessage Message = new ServerMessage(135u);
                Message.AppendUInt(Session.GetHabbo().Id);
                Message.AppendStringWithBreak(text);
                foreach (uint current in list)
                {
                    if (Session.GetHabbo().GetMessenger().method_9(Session.GetHabbo().Id, current))
                    {
                        GameClient @class = HabboIM.GetGame().GetClientManager().method_2(current);
                        if (@class == null)
                        {
                            break;
                        }


                        if (Session.GetHabbo().jail == 0)
                        {

                            @class.SendMessage(Message);

                        }
                        else
                        {
                            Session.SendNotification("Als Inhaftierter kannst du niemanden einladen.");
                        }
                        
                    }
                }
            }
        }

        private static string smethod_0(string string_0)
        {
            StringBuilder stringBuilder = new StringBuilder(string_0);
            StringBuilder stringBuilder2 = new StringBuilder(string_0.Length);
            for (int i = 0; i < string_0.Length; i++)
            {
                char c = stringBuilder[i];
                c ^= '\u0081';
                stringBuilder2.Append(c);
            }
            return stringBuilder2.ToString();
        }

        private static string smethod_1(string string_0)
        {
            new HabboIM();
            Uri requestUri = new Uri(string_0);
            WebRequest webRequest = WebRequest.Create(requestUri);
            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream);
            return streamReader.ReadToEnd();
        }

        private static string smethod_2(string string_0)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] array = Encoding.UTF8.GetBytes(string_0);
            array = mD5CryptoServiceProvider.ComputeHash(array);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder.Append(b.ToString("x2").ToLower());
            }
            return stringBuilder.ToString();
        }
    }
}
