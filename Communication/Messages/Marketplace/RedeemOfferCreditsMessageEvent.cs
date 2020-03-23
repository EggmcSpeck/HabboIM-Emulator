using System;
using System.Data;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.Storage;
namespace HabboIM.Communication.Messages.Marketplace
{
	internal sealed class RedeemOfferCreditsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			DataTable dataTable = null;
			using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
			{
				dataTable = @class.ReadDataTable("SELECT asking_price FROM catalog_marketplace_offers WHERE user_id = '" + Session.GetHabbo().Id + "' AND state = '2'");
			}
			if (dataTable != null)
			{
				int num = 0;
				foreach (DataRow dataRow in dataTable.Rows)
				{
					num += (int)dataRow["asking_price"];
				}
				if (num >= 1)
				{
					Session.GetHabbo().Credits += num;
					Session.GetHabbo().UpdateCredits(true);
				}
				using (DatabaseClient @class = HabboIM.GetDatabase().GetClient())
				{
					@class.ExecuteQuery("DELETE FROM catalog_marketplace_offers WHERE user_id = '" + Session.GetHabbo().Id + "' AND state = '2'");
				}
			}
		}
	}
}
