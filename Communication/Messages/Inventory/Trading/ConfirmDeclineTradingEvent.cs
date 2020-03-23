using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Inventory.Trading
{
	internal sealed class ConfirmDeclineTradingEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CanTrade)
			{
				@class.method_78(Session.GetHabbo().Id);
			}
		}
	}
}
