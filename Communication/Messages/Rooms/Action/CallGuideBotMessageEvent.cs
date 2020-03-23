using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.RoomBots;
using HabboIM.HabboHotel.Rooms;
using HabboIM.HabboHotel.Pathfinding;
namespace HabboIM.Communication.Messages.Rooms.Action
{
	internal sealed class CallGuideBotMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true))
			{
				for (int i = 0; i < @class.RoomUsers.Length; i++)
				{
					RoomUser class2 = @class.RoomUsers[i];
					if (class2 != null && (class2.IsBot && class2.RoomBot.AiType == AIType.const_1))
					{
						ServerMessage Message = new ServerMessage(33u);
						Message.AppendInt32(4009);
						Session.SendMessage(Message);
						return;
					}
				}
				if (Session.GetHabbo().bool_10)
				{
					ServerMessage Message = new ServerMessage(33u);
					Message.AppendInt32(4010);
					Session.SendMessage(Message);
				}
				else
				{
					RoomUser class3 = @class.BotToRoomUser(HabboIM.GetGame().GetBotManager().method_3(2u));
					class3.method_7(@class.RoomModel.int_0, @class.RoomModel.int_1, @class.RoomModel.double_0);
					class3.UpdateNeeded = true;
					RoomUser class4 = @class.method_56(@class.Owner);
					if (class4 != null)
					{
						class3.MoveTo(class4.Position);
						class3.method_9(Class107.smethod_0(class3.X, class3.Y, class4.X, class4.Y));
					}
                    Session.GetHabbo().CallGuideBotAchievementsCompleted();
					Session.GetHabbo().bool_10 = true;
				}
			}
		}
	}
}
