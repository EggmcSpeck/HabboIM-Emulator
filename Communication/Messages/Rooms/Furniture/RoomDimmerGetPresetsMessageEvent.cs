using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Items;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Rooms.Furniture
{
	internal sealed class RoomDimmerGetPresetsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                if (@class != null && @class.CheckRights(Session, true) && @class.MoodlightData != null)
				{
					ServerMessage Message = new ServerMessage(365u);
					Message.AppendInt32(@class.MoodlightData.Presets.Count);
					Message.AppendInt32(@class.MoodlightData.CurrentPreset);
					int num = 0;
					foreach (MoodlightPreset current in @class.MoodlightData.Presets)
					{
						num++;
						Message.AppendInt32(num);
						Message.AppendInt32(int.Parse(HabboIM.BooleanToString(current.BackgroundOnly)) + 1);
						Message.AppendStringWithBreak(current.ColorCode);
						Message.AppendInt32(current.ColorIntensity);
					}
					Session.SendMessage(Message);
				}
			}
			catch
			{
			}
		}
	}
}
