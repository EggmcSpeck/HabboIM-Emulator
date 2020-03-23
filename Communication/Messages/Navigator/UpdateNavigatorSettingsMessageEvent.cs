using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Rooms;
using HabboIM.Messages;
using HabboIM.Storage;
namespace HabboIM.Communication.Messages.Navigator
{
	internal sealed class UpdateNavigatorSettingsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
            RoomData @class = HabboIM.GetGame().GetRoomManager().method_12(num);
			if (num == 0u || (@class != null && !(@class.Owner.ToLower() != Session.GetHabbo().Username.ToLower())))
			{
				Session.GetHabbo().HomeRoomId = num;
				using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
				{
					class2.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE users SET home_room = '",
						num,
						"' WHERE Id = '",
						Session.GetHabbo().Id,
						"' LIMIT 1;"
					}));
                    Session.GetHabbo().Whisper("Dein Hauptwohnsitz wurde erfolgreich aktualisiert!");
                }
				ServerMessage Message = new ServerMessage(455u);
				Message.AppendUInt(num);
				Session.SendMessage(Message);
			}
		}
	}
}
