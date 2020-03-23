using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Rooms.Engine
{
    internal sealed class MoveAvatarMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                Room class14_ = Session.GetHabbo().CurrentRoom;
                if(class14_ != null && Session.GetHabbo().collector == true)
                {

                    Session.GetHabbo().Whisper("Sage erneut :sammeln um mit dem Sammeln aufzuhören und wieder normal laufen zu können.");

                }


                if (class14_ != null && Session.GetHabbo().collector == false)
                {
                    RoomUser @class = class14_.GetRoomUserByHabbo(Session.GetHabbo().Id);
                    if (@class != null && @class.bool_0)
                    {
                        int num = Event.PopWiredInt32();
                        int num2 = Event.PopWiredInt32();
                        if (num != @class.X || num2 != @class.Y)
                        {
                            if (@class.RoomUser_0 != null)
                            {
                                try
                                {
                                    if (@class.RoomUser_0.IsBot)
                                    {
                                        @class.Unidle();
                                    }
                                    @class.RoomUser_0.MoveTo(num, num2);
                                    return;
                                }
                                catch
                                {
                                    @class.RoomUser_0 = null;
                                    @class.class34_1 = null;
                                    @class.MoveTo(num, num2);
                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(-1, true);
                                    return;
                                }
                            }
                            if (@class.TeleportMode)
                            {
                                @class.X = num;
                                @class.Y = num2;
                                @class.UpdateNeeded = true;
                            }
                            else
                            {
                                @class.MoveTo(num, num2);
                            }
                        }
                    }
                }
            }

            if (Session != null && Session.GetHabbo() != null)
            {
                Room class14_ = Session.GetHabbo().CurrentRoom;
                if (class14_ != null && Session.GetHabbo().knastarbeit == true)
                {
                    Session.GetHabbo().knastarbeit = false;
                    Session.GetHabbo().Whisper("Du hast das Arbeiten abgebrochen! (nicht herumlaufen während du arbeitest...)");
                    Session.GetHabbo().last_gearbeitet = 0.0;
                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                }

            }

        }
    }
}
