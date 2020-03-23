using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using HabboIM.Storage;
using HabboIM.Messages;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Pathfinding;
using HabboIM.HabboHotel.Rooms;
using System.Threading;
using System.Threading.Tasks;
using HabboIM.HabboHotel.Items.Interactors;
using HabboIM.HabboHotel.Items;

namespace HabboIM.HabboHotel.Items.Interactors
{
    class InteractorCasinoMachine : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem Item)
        {
        }
        public override void OnRemove(GameClient Session, RoomItem Item)
        {
        }
        public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (Session.GetHabbo().Credits >= 100)
            {
                Session.GetHabbo().Whisper("Einsatz 100 Taler.");
                Session.GetHabbo().Credits = Session.GetHabbo().Credits - 100;
                Session.GetHabbo().UpdateCredits(true);
                System.Threading.Thread.Sleep(2000);
                Random rnd = new Random();
                int Credits = rnd.Next(-100, 500);
                Session.GetHabbo().Credits = Session.GetHabbo().Credits + Credits;
                Session.GetHabbo().UpdateCredits(true);
                if (Credits < 100)
                {
                    Session.GetHabbo().Whisper("Du hast " + (100 - Credits) + " Taler verloren!");
                }
                else
                {
                    Session.GetHabbo().Whisper("Du hast " + (Credits - 100) + " Taler gewonnen!");
                }
            }
            else
            {
                Session.GetHabbo().Whisper("Du hast zu wenig Taler. Du brauchst mindestens 100 Taler!");
            }
        }
    }
}
