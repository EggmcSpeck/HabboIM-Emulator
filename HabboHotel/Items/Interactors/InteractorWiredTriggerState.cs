using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Items;
namespace HabboIM.HabboHotel.Items.Interactors
{
	internal sealed class InteractorWiredTriggerState : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			if (bool_0 && Session != null)
			{
				RoomItem_0.method_10();
				ServerMessage Message = new ServerMessage(651u);
				Message.AppendInt32(0);
                if (Session.GetHabbo().HasFuse("wired_unlimitedselects"))
                {
                    Message.AppendInt32(50);
                }
                else
                {
                    Message.AppendInt32(5);
                }
				if (RoomItem_0.string_2.Length > 0)
				{
					Message.AppendString(RoomItem_0.string_2);
				}
				else
				{
					Message.AppendInt32(0);
				}
				Message.AppendInt32(RoomItem_0.GetBaseItem().Sprite);
				Message.AppendUInt(RoomItem_0.uint_0);
				Message.AppendStringWithBreak("");
				Message.AppendInt32(0);
				Message.AppendInt32(0);
				Message.AppendInt32(0);
				Message.AppendInt32(0);
				Message.AppendInt32(0);
				Message.AppendStringWithBreak("");
				Session.SendMessage(Message);
			}
		}
	}
}
