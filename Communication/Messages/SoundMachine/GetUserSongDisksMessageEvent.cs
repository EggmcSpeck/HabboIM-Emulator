using System;
using System.Collections.Generic;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Items;
using HabboIM.Messages;
using HabboIM.HabboHotel.SoundMachine;

namespace HabboIM.Communication.Messages.SoundMachine
{
    internal sealed class GetUserSongDisksMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            List<UserItem> list = new List<UserItem>();
            foreach (UserItem current in Session.GetHabbo().GetInventoryComponent().Items)
            {
                if (current != null && !(current.method_1().Name != "song_disk") && !Session.GetHabbo().GetInventoryComponent().list_1.Contains(current.uint_0))
                {
                    list.Add(current);
                }
            }
            /*ServerMessage Message = new ServerMessage(333u);
            Message.AppendInt32(list.Count);
            foreach (UserItem current2 in list) //PHOENIX SEN OMA
            {
                int int_ = 0;
                if (current2.string_0.Length > 0)
                {
                    int_ = int.Parse(current2.string_0);
                }
                Soundtrack @class = HabboIM.GetGame().GetItemManager().method_4(int_);
                if (@class == null)
                {
                    return;
                }
                Message.AppendUInt(current2.uint_0);
                Message.AppendInt32(@class.Id);
            }*/

            ServerMessage Message = new ServerMessage(333u);
            Message.AppendInt32(list.Count);
            foreach (UserItem current2 in list) //MUN OMA
            {
                int int_ = 0;
                if (current2.string_0.Length > 0)
                {
                    int_ = int.Parse(current2.string_0);
                }
                SongData SongData = SongManager.GetSong(int_);
                if (SongData == null)
                {
                    return;
                }
                Message.AppendUInt(current2.uint_0);
                Message.AppendInt32(SongData.Id);
            }
            Session.SendMessage(Message);
        }
    }
}
