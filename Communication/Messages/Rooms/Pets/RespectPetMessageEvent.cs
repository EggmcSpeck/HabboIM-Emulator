using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
using HabboIM.Storage;
namespace HabboIM.Communication.Messages.Rooms.Pets
{
	internal sealed class RespectPetMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && !@class.IsPublic)
			{
				uint uint_ = Event.PopWiredUInt();
				RoomUser class2 = @class.method_48(uint_);
				if (class2 != null && class2.PetData != null && Session.GetHabbo().PetRespectPoints > 0)
				{
					class2.PetData.OnRespect();
					Session.GetHabbo().PetRespectPoints--;
					using (DatabaseClient class3 = HabboIM.GetDatabase().GetClient())
					{
						class3.AddParamWithValue("userid", Session.GetHabbo().Id);
						class3.ExecuteQuery("UPDATE user_stats SET dailypetrespectpoints = dailypetrespectpoints - 1 WHERE Id = @userid LIMIT 1");
					}
				}
			}
		}
	}
}
