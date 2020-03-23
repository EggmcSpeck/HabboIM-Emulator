using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Inventory.Trading
{
	internal sealed class OpenTradingEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null)
			{
				if (!@class.CanTrade)
				{
					Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("trade_error_roomdisabled"));
				}
				else
				{
					RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
					RoomUser class3 = @class.method_52(Event.PopWiredInt32());
					if (class2 != null && class3 != null && class3.GetClient().GetHabbo().TradingDisabled)
					{
						@class.method_77(class2, class3);
					}
					else
					{
						Session.GetHabbo().Whisper(HabboIMEnvironment.GetExternalText("trade_error_targetdisabled"));
					}
				}
			}
		}
	}
}
