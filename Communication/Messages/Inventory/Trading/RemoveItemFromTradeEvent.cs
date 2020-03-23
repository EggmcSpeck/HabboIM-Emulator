using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Items;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Inventory.Trading
{
	internal sealed class RemoveItemFromTradeEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.CanTrade)
			{
				Trade class2 = @class.method_76(Session.GetHabbo().Id);
				UserItem class3 = Session.GetHabbo().GetInventoryComponent().GetItemById(Event.PopWiredUInt());
				if (class2 != null && class3 != null)
				{
					class2.method_3(Session.GetHabbo().Id, class3);
				}
			}
		}
	}
}
