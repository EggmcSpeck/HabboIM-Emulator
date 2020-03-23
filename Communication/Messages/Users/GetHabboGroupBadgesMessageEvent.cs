using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Users
{
	internal sealed class GetHabboGroupBadgesMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().uint_2 > 0u)
			{
                Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().uint_2);

                if (@class != null) // wenn Raum nicht NULL, Packet senden (Disconnect fix)
                {

                    ServerMessage Message = new ServerMessage(309u);
                    Message.AppendInt32(10);

                    Message.AppendInt32(1);
                    Message.AppendStringWithBreak("BZLVL1");

                    Message.AppendInt32(2);
                    Message.AppendStringWithBreak("BZLVL2");



                    Message.AppendInt32(3);
                    Message.AppendStringWithBreak("BZLVL3");

                    Message.AppendInt32(4);
                    Message.AppendStringWithBreak("BZLVL4");

                    Message.AppendInt32(5);
                    Message.AppendStringWithBreak("BZLVL5");

                    Message.AppendInt32(6);
                    Message.AppendStringWithBreak("BZLVL6");

                    Message.AppendInt32(7);
                    Message.AppendStringWithBreak("BZLVL7");

                    Message.AppendInt32(8);
                    Message.AppendStringWithBreak("BZLVL8");

                    Message.AppendInt32(9);
                    Message.AppendStringWithBreak("BZLVL9");

                    Message.AppendInt32(10);
                    Message.AppendStringWithBreak("BZLVL10");



                    @class.SendMessage(Message, null);
                }




                /*
                
                Session.SendNotification("GetHabboGroupBadgesMessageEvent");
				Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().uint_2);
				if (@class != null && Session.GetHabbo().int_0 > 0)
				{


                    GroupsManager class2 = Groups.smethod_2(Session.GetHabbo().int_0);
					if (class2 != null && !@class.list_17.Contains(class2))
					{
						@class.list_17.Add(class2);
						ServerMessage Message = new ServerMessage(309u);
						Message.AppendInt32(@class.list_17.Count);
						foreach (GroupsManager current in @class.list_17)
						{
							Message.AppendInt32(current.int_0);
							Message.AppendStringWithBreak(current.string_2);
						}
						@class.SendMessage(Message, null);
					}
					else
					{
						foreach (GroupsManager current2 in @class.list_17)
						{
							if (current2 == class2 && current2.string_2 != class2.string_2)
							{
								ServerMessage Message = new ServerMessage(309u);
								Message.AppendInt32(@class.list_17.Count);
								foreach (GroupsManager current in @class.list_17)
								{
									Message.AppendInt32(current.int_0);
									Message.AppendStringWithBreak(current.string_2);
								}
								@class.SendMessage(Message, null);
							}
						}
					}

    
				}
				if (@class != null && @class.list_17.Count > 0)
				{
					ServerMessage Message = new ServerMessage(309u);
					Message.AppendInt32(@class.list_17.Count);
					foreach (GroupsManager current in @class.list_17)
					{
						Message.AppendInt32(current.int_0);
						Message.AppendStringWithBreak(current.string_2);
					}
					Session.SendMessage(Message);
				}
			*/
            }
		}

	}
}
