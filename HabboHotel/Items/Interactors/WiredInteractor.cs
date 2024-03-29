using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Items;
namespace HabboIM.HabboHotel.Items.Interactors
{
	internal sealed class WiredInteractor : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			if (bool_0)
			{
				ServerMessage Message = new ServerMessage(651u);
				Message.AppendInt32(0);
				Message.AppendInt32(50);
				Message.AppendInt32(1);
				Message.AppendUInt(RoomItem_0.uint_0);
				Message.AppendInt32(RoomItem_0.GetBaseItem().Sprite);
				Message.AppendUInt(RoomItem_0.uint_0);
				Session.SendMessage(Message);
			}
		}
	}
}
