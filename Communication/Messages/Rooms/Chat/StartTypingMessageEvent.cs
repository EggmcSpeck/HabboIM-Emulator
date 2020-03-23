using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Rooms.Chat
{
    internal sealed class StartTypingMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                if (@class != null)
                {
                    RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
                    if (class2 != null)
                    {
                        ServerMessage Message = new ServerMessage(361u);
                        Message.AppendInt32(class2.VirtualId);
                        Message.AppendBoolean(true);
                        @class.SendMessage(Message, null);
                    }
                }
            }
        }
    }
}
