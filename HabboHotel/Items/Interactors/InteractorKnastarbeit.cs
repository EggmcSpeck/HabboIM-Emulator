using HabboIM;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Items;
using System;
using HabboIM.Core;
using HabboIM.HabboHotel.Achievements;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Items;
using HabboIM.HabboHotel.Rooms;
using HabboIM.HabboHotel.Users;
using HabboIM.HabboHotel.Users.Authenticator;
using HabboIM.Messages;
using HabboIM.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HabboIM.WebSocket;

namespace HabboIM.HabboHotel.Items.Interactors
{

    internal class InteractorKnastarbeit : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            uint num2 = Convert.ToUInt32(1732);
            Room class3 = HabboIM.GetGame().GetRoomManager().method_15(num2);

            if (Session.GetHabbo().CurrentRoom == class3 && Session.GetHabbo().jailtime > 180)
            {
                Room class2 = Session.GetHabbo().CurrentRoom;
                RoomUser Aktuelleruser = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (Math.Abs(Aktuelleruser.X - 13) < 2 && Math.Abs(Aktuelleruser.Y - 9) < 2)
                {

                    if (Session.GetHabbo().knastarbeit == false)
                    {
                        Session.GetHabbo().last_gearbeitet = HabboIM.GetUnixTimestamp();

                        Session.GetHabbo().knastarbeit = true;
                        Session.SendNotification("Du hast begonnen zu arbeiten!");
                        Session.GetHabbo().GetEffectsInventoryComponent().method_2(169, true);
                        var t = Task.Run(async delegate
                        {
                            await Task.Delay(3150);

                        X:
                            await Task.Delay(2025);
                            if (Session.GetHabbo().last_gearbeitet + 1 * 5 < HabboIM.GetUnixTimestamp())
                            {
                                Session.GetHabbo().knastarbeit = false;
                                Session.GetHabbo().Whisper("Du hast deine Arbeit beendet und eine Haftverkürzung von 2 Minuten erhalten!");
                                Session.GetHabbo().jailtime -= 120;
                                Session.GetHabbo().UpdateJailTime(true);
                                Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                            }
                            return true;

                        });
                    }
                }
                else
                {
                    Session.SendNotification("Du bist nicht nah genug am Taler Denkmal um diese Funktion nutzen zu können!");
                }
            }
            else
            {
                if (Session.GetHabbo().CurrentRoom == class3 && Session.GetHabbo().jail == 0)
                {
                    Session.SendNotification("Du musst inhaftiert sein um diese Funktion verwenden zu können!");
                }
                if (Session.GetHabbo().CurrentRoom == class3 && Session.GetHabbo().jail == 1)
                {
                    Session.SendNotification("Du musst mindestens 3 Minuten lang inhaftiert sein um deine Haftzeit mit dieser Funktion zu verkürzen!");
                }

            }
        }
    }
}