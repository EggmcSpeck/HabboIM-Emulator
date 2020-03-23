using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Support;
using HabboIM.HabboHotel.Rooms;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Help
{
	internal sealed class GetCfhChatlogMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				SupportTicket @class = HabboIM.GetGame().GetModerationTool().method_5(Event.PopWiredUInt());
				if (@class != null)
				{
                    RoomData class2 = HabboIM.GetGame().GetRoomManager().method_11(@class.RoomId);
					if (class2 != null)
					{
                        Session.SendMessage(HabboIM.GetGame().GetModerationTool().method_21(@class, class2, @class.Timestamp));
					}
				}
			}
		}
	}
}
