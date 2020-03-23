using System;
using System.Collections;
using System.Collections.Generic;
using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Rooms;
using HabboIM.Messages;
using HabboIM.HabboHotel.Items;
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
namespace HabboIM.Communication.Messages.Rooms.Engine
{
    internal sealed class GetRoomEntryDataMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session.GetHabbo().uint_2 > 0u && Session.GetHabbo().bool_5)
            {
                RoomData @class = HabboIM.GetGame().GetRoomManager().method_12(Session.GetHabbo().uint_2);
                if (@class != null)
                {
                    Session.GetHabbo().collector = false;
                    Session.GetHabbo().knastarbeit = false;
                    Session.GetHabbo().last_gearbeitet = 0.0;

                    if (Session.GetHabbo().jail == 1 && Session.GetHabbo().jailtime < 1)
                    {
                        Session.GetHabbo().jail = 0;
                        Session.GetHabbo().jailtime = 0.0;
                        Session.GetHabbo().UpdateJail(true);
                        Session.GetHabbo().UpdateJailTime(true);
                    } 

                    if (Session.GetHabbo().jail == 1)
                    {
                        if (Session.GetHabbo().jailtime > 1)
                        {
                            HabboIM.GetWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("myh_arrest");
                        }
                    }
                   




                    if (HabboIM.GetGame().GetClientManager().wartung == true && @class.Id != 0u && @class.Id != 1732u && @class.Id != 1732 && @class.Id != 901 && @class.Id != 901u && Session.GetHabbo().Rank < 6)
                    {

                        if (Session.GetHabbo().jail == 1)
                        {
                            uint num2 = Convert.ToUInt32(1732);
                            Room class3 = HabboIM.GetGame().GetRoomManager().method_15(num2);
                            ServerMessage Message2 = new ServerMessage(286u);
                            Message2.AppendBoolean(class3.IsPublic);
                            Message2.AppendUInt(1732);
                            Session.SendMessage(Message2);


                        }
                        else {
                            uint num2 = Convert.ToUInt32(901);
                            Room class3 = HabboIM.GetGame().GetRoomManager().method_15(num2);
                            ServerMessage Message2 = new ServerMessage(286u);
                            Message2.AppendBoolean(class3.IsPublic);
                            Message2.AppendUInt(901);
                            Session.SendMessage(Message2);
                            Session.SendNotification("Zugang verweigert!\r\rDas Hotel befindet sich momentan im Wartungszustand.");
                        }

                    } else { 








                        if (Session.GetHabbo().jail == 1 && Session.GetHabbo().jailtime > 0 && @class.Id != 1732 && @class.Id != 0u && @class.Id != 1732u)
                    {

                        uint num2 = Convert.ToUInt32(1732);
                        Room class3 = HabboIM.GetGame().GetRoomManager().method_15(num2);
                        ServerMessage Message2 = new ServerMessage(286u);
                        Message2.AppendBoolean(class3.IsPublic);
                        Message2.AppendUInt(1732);
                        Session.SendMessage(Message2);
                            
                           


                            if (Session.GetHabbo().jailtime != 0.0 && Session.GetHabbo().jailtime > 0)
                        {
                            double seconds = (Session.GetHabbo().jailtime) / 60;
                            decimal newtimexx = Convert.ToDecimal(seconds);
                            decimal rundesecs = Math.Round(newtimexx);
                            Session.SendNotification("Versuche nicht, aus dem MyHuBBa Gefängnis auszubrechen...!\r\rDu hast noch eine Haftzeit in höhe von " + rundesecs + " Minuten.\r\rHalte dich das nächste mal an die Regeln des Hotels!");
                        }








                    }
                    else {

                        if (@class.Id == 1732 && Session.GetHabbo().jail == 0 && @class.Id != 0u && Session.GetHabbo().Rank < 3)
                        {
                            Room @classx = HabboIM.GetGame().GetRoomManager().GetRoom(@class.Id);
                            @classx.method_47(Session, true, false);

                            Session.SendNotification("Du kannst das MyHuBBa Gefängnis nicht betreten da du keine Haftstrafe absitzen musst.");

                        }
                        else
                        {


                                if (@class.Model == null)
                                {
                                    Session.SendNotification("Error loading room, please try again soon! (Error Code: MdlData)");
                                    Session.SendMessage(new ServerMessage(18u));
                                    Session.GetClientMessageHandler().method_7();
                                }
                                else
                                {
                                    Session.SendMessage(@class.Model.method_1());
                                    Session.SendMessage(@class.Model.method_2());
                                    Room class2 = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().uint_2);
                                    if (class2 != null)
                                    {
                                        Session.GetClientMessageHandler().method_7();
                                        ServerMessage Message = new ServerMessage(30u);
                                        if (class2.RoomModel.string_2 != "")
                                        {
                                            Message.AppendStringWithBreak(class2.RoomModel.string_2);
                                        }
                                        else
                                        {
                                            Message.AppendInt32(0);
                                        }
                                        Session.SendMessage(Message);
                                        if (class2.Type == "private")
                                        {
                                            Hashtable hashtable_ = class2.Hashtable_0;
                                            Hashtable hashtable_2 = class2.Hashtable_1;
                                            ServerMessage Message2 = new ServerMessage(32u);
                                            Message2.AppendInt32(hashtable_.Count);
                                            foreach (RoomItem class3 in hashtable_.Values)
                                            {
                                                class3.method_6(Message2);
                                            }
                                            Session.SendMessage(Message2);
                                            ServerMessage Message3 = new ServerMessage(45u);
                                            Message3.AppendInt32(hashtable_2.Count);
                                            foreach (RoomItem class3 in hashtable_2.Values)
                                            {
                                                class3.method_6(Message3);
                                            }
                                            Session.SendMessage(Message3);
                                        }
                                        class2.method_46(Session, Session.GetHabbo().bool_8);
                                        List<RoomUser> list = new List<RoomUser>();
                                        for (int i = 0; i < class2.RoomUsers.Length; i++)
                                        {
                                            RoomUser class4 = class2.RoomUsers[i];
                                            if (class4 != null && (!class4.bool_11 && class4.bool_12))
                                            {
                                                list.Add(class4);
                                            }
                                        }
                                        ServerMessage Message4 = new ServerMessage(28u);
                                        Message4.AppendInt32(list.Count);
                                        foreach (RoomUser class4 in list)
                                        {
                                            class4.method_14(Message4);
                                        }
                                        Session.SendMessage(Message4);
                                        ServerMessage Message5 = new ServerMessage(472u);
                                        Message5.AppendBoolean(class2.Hidewall);
                                        Message5.AppendInt32(class2.Wallthick);
                                        Message5.AppendInt32(class2.Floorthick);
                                        Session.SendMessage(Message5);
                                        if (class2.Type == "public")
                                        {
                                            ServerMessage Message6 = new ServerMessage(471u);
                                            Message6.AppendBoolean(false);
                                            Message6.AppendStringWithBreak(class2.ModelName);
                                            Message6.AppendBoolean(false);
                                            Session.SendMessage(Message6);
                                        }
                                        else
                                        {
                                            if (class2.Type == "private")
                                            {
                                                ServerMessage Message6 = new ServerMessage(471u);
                                                Message6.AppendBoolean(true);
                                                Message6.AppendUInt(class2.Id);
                                                if (class2.CheckRights(Session, true))
                                                {
                                                    Message6.AppendBoolean(true);
                                                }
                                                else
                                                {
                                                    Message6.AppendBoolean(false);
                                                }
                                                Session.SendMessage(Message6);
                                                ServerMessage Message7 = new ServerMessage(454u);
                                                Message7.AppendBoolean(false);
                                                @class.method_3(Message7, false, false);
                                                Session.SendMessage(Message7);
                                            }
                                        }
                                        ServerMessage Message8 = class2.method_67(true);
                                        if (Message8 != null)
                                        {
                                            Session.SendMessage(Message8);
                                        }
                                        for (int i = 0; i < class2.RoomUsers.Length; i++)
                                        {
                                            RoomUser class4 = class2.RoomUsers[i];
                                            if (class4 != null && !class4.bool_11)
                                            {
                                                if (class4.IsDancing)
                                                {
                                                    ServerMessage Message9 = new ServerMessage(480u);
                                                    Message9.AppendInt32(class4.VirtualId);
                                                    Message9.AppendInt32(class4.DanceId);
                                                    Session.SendMessage(Message9);
                                                }
                                                if (class4.bool_8)
                                                {
                                                    ServerMessage Message10 = new ServerMessage(486u);
                                                    Message10.AppendInt32(class4.VirtualId);
                                                    Message10.AppendBoolean(true);
                                                    Session.SendMessage(Message10);
                                                }
                                                if (class4.CarryItemID > 0 && class4.int_6 > 0)
                                                {
                                                    ServerMessage Message11 = new ServerMessage(482u);
                                                    Message11.AppendInt32(class4.VirtualId);
                                                    Message11.AppendInt32(class4.CarryItemID);
                                                    Session.SendMessage(Message11);
                                                }
                                                if (!class4.IsBot)
                                                {
                                                    try
                                                    {
                                                        if (class4.GetClient().GetHabbo() != null && class4.GetClient().GetHabbo().GetEffectsInventoryComponent() != null && class4.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0 >= 1)
                                                        {
                                                            ServerMessage Message12 = new ServerMessage(485u);
                                                            Message12.AppendInt32(class4.VirtualId);
                                                            Message12.AppendInt32(class4.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0);
                                                            Session.SendMessage(Message12);
                                                        }
                                                        goto IL_5C5;
                                                    }
                                                    catch
                                                    {
                                                        goto IL_5C5;
                                                    }
                                                }
                                                if (!class4.IsPet && class4.RoomBot.EffectId != 0)
                                                {
                                                    ServerMessage Message12 = new ServerMessage(485u);
                                                    Message12.AppendInt32(class4.VirtualId);
                                                    Message12.AppendInt32(class4.RoomBot.EffectId);
                                                    Session.SendMessage(Message12);
                                                }
                                            }
                                            IL_5C5:;
                                        }
                                        if (class2 != null && Session != null && Session.GetHabbo().CurrentRoom != null)
                                        {
                                            Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                            class2.method_8(Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id));
                                        }
                                        if (class2.Achievement > 0u)
                                        {
                                            HabboIM.GetGame().GetAchievementManager().addAchievement(Session, class2.Achievement, 1);
                                        }
                                        if (Session.GetHabbo().IsMuted && Session.GetHabbo().int_4 > 0)
                                        {
                                            ServerMessage Message13 = new ServerMessage(27u);
                                            Message13.AppendInt32(Session.GetHabbo().int_4);
                                            Session.SendMessage(Message13);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
           
        }
    }
}