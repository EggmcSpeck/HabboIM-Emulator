using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HabboIM.Messages;
namespace HabboIM.HabboHotel.Advertisements
{
    class Werberunde
    {
        public static short SUsers = 0;
        public static bool flag = false;

        public static void SET()
        {
            if (flag == true)
            {
                flag = false;
                SUsers = 0;
                WERBERUNDE_Alert("Dieses Werberundenziel wurde abgesetzt!");
                Misc.LowPriorityWorker.taktmod = 3000;
                return;
            }
            flag = true;
            WERBERUNDE_Alert("Das Werberundenziel wurde auf " + SUsers + " User gesetzt!\nLos gehts!");
            Misc.LowPriorityWorker.taktmod = 650;
        }
        public static void WERBERUNDE_Alert(string text)
        {
            ServerMessage Message2 = new ServerMessage(808u);
            Message2.AppendStringWithBreak("Habbo - Werberundensystem");
            Message2.AppendStringWithBreak(text);
            ServerMessage Message3 = new ServerMessage(161u);
            HabboIM.GetGame().GetClientManager().method_15(Message2, Message3);
        }
    }
}
