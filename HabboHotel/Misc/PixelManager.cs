using System;
using System.Threading;
using HabboIM.Core;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Util;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.HabboHotel.Misc
{
	internal sealed class PixelManager
	{
		public bool KeepAlive;
		private Thread WorkerThread;
		public PixelManager()
		{
			this.KeepAlive = true;
			this.WorkerThread = new Thread(new ThreadStart(this.method_1));
			this.WorkerThread.Name = "Pixel Manager";
			this.WorkerThread.Priority = ThreadPriority.Lowest;
		}
		public void method_0()
		{
            Console.BackgroundColor = ConsoleColor.Black;
            Logging.Write("Reward Timer wird gestartet..");
			this.WorkerThread.Start();
			Logging.WriteLine("Fertig!", ConsoleColor.Green);
		}
		private void method_1()
		{
			try
			{
				while (this.KeepAlive)
				{
					if (HabboIM.GetGame() != null && HabboIM.GetGame().GetClientManager() != null)
					{
						HabboIM.GetGame().GetClientManager().method_29();
					}
					Thread.Sleep(15000);
				}
			}
			catch (ThreadAbortException)
			{
			}
		}
		public bool method_2(GameClient Session)
		{
			double num = (HabboIM.GetUnixTimestamp() - Session.GetHabbo().LastActivityPointsUpdate) / 60.0;
			return num >= (double)ServerConfiguration.CreditingInterval;
		}
		public void method_3(GameClient Session)
		{
			try
			{
                if (Session.GetHabbo().InRoom)
				{
					RoomUser @class = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (@class.int_1 <= ServerConfiguration.SleepTimer)
					{
						double double_ = HabboIM.GetUnixTimestamp();
						Session.GetHabbo().LastActivityPointsUpdate = double_;
						if (ServerConfiguration.PointingAmount > 0 && (Session.GetHabbo().ActivityPoints < ServerConfiguration.PixelLimit || ServerConfiguration.PixelLimit == 0))
						{
							Session.GetHabbo().ActivityPoints += ServerConfiguration.PointingAmount;
							Session.GetHabbo().method_16(ServerConfiguration.PointingAmount);
						}
						if (ServerConfiguration.CreditingAmount > 0 && (Session.GetHabbo().Credits < ServerConfiguration.CreditLimit || ServerConfiguration.CreditLimit == 0))
						{
							Session.GetHabbo().Credits += ServerConfiguration.CreditingAmount;
							if (Session.GetHabbo().IsVIP)
							{
								Session.GetHabbo().Credits += ServerConfiguration.CreditingAmount;
							}
							Session.GetHabbo().UpdateCredits(true);
						}
						if (ServerConfiguration.PixelingAmount > 0 && (Session.GetHabbo().VipPoints < ServerConfiguration.PointLimit || ServerConfiguration.PointLimit == 0))
						{
							Session.GetHabbo().VipPoints += ServerConfiguration.PixelingAmount;
							Session.GetHabbo().UpdateVipPoints(false, true);
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
