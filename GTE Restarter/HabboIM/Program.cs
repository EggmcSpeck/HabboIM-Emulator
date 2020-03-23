using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restarter
{
    class Program
    {
        public static string Version
        {
            get
            {
                return "HabboIM Restarter v1.0.0.1";
            }
        }
        public static string Developer
        {
            get
            {
                return "Made with <3 by Baumstamm & Jimmy.";
            }
        }
        private static void Main(string[] args)
        {
           
            Console.CursorVisible = false;
            Console.Title = "HabboIM - Restart Tool";
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(100, 20);
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

            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("////*///*///*///*///*///*///*///*////"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("////*///////*///////*///////*////////"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("//*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*//"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.WriteLine("");

            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("////*///////*////"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*///////*////////"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("////*///*///*///*"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*///*///*///*////"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("////*///////*////"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*///////*////////"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
            Console.Write("                               @@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("/*///*///*/*///**"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("*///*///***/**/*/"); Console.ForegroundColor = ConsoleColor.Black; Console.Write("@@@ "); Console.WriteLine("");
            Console.WriteLine("                               @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("                             ");
            Console.WriteLine("                                       " + Version);
            Console.WriteLine("                                   " + Developer);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("____________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("     [");
            string str2 = DateTime.Now.ToString();
            Console.Write(str2 + "]  ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("HabboIM Restarter ist bereit!");
            Console.ResetColor();
            Console.WriteLine();
            while (true)
            {
                Program.StartHabboIM();
            }
        }

        private static void StartHabboIM()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "HabboIM Emulator.exe";
                process.Start();
                process.WaitForExit();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("     [");
                string str2 = DateTime.Now.ToString();
                Console.Write(str2 + "]  ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Emulator wird neugestartet...\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Beep();
                Program.StartHabboIM();
            }
            catch
            {
                Console.Beep();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("     [");
                string str2 = DateTime.Now.ToString();
                Console.Write(str2 + "]  ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Pfad fehlerhaft konfiguriert!\n");
                Console.WriteLine("      Überprüfe deine Konfiguration!");
            }
        }

    }
}
