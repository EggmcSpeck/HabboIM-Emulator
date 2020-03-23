using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Items;
using HabboIM.HabboHotel.Rooms;
namespace HabboIM.Communication.Messages.Rooms.Furniture
{
	internal sealed class UseFurnitureMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
				if (@class != null)
				{
					RoomItem class2 = @class.method_28(Event.PopWiredUInt());
					if (class2 != null)
					{
						bool bool_ = false;
						if (@class.method_26(Session))
						{
							bool_ = true;
						}
                        class2.Class69_0.OnTrigger(Session, class2, Event.PopWiredInt32(), bool_);
                        if (Session.GetHabbo().CurrentQuestId > 0 && HabboIM.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "SWITCHSTATE")
						{
                            HabboIM.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
						}
						else
						{
                            if (Session.GetHabbo().CurrentQuestId > 0 && HabboIM.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "FINDLIFEGUARDTOWER" && class2.GetBaseItem().Name == "bw_lgchair")
							{
                                HabboIM.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
							}
							else
							{
                                if (Session.GetHabbo().CurrentQuestId > 0 && HabboIM.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "FINDSURFBOARD" && class2.GetBaseItem().Name.Contains("bw_sboard"))
								{
                                    HabboIM.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
								}
								else
								{
                                    if (Session.GetHabbo().CurrentQuestId > 0 && HabboIM.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "FINDBEETLE" && class2.GetBaseItem().Name.Contains("bw_van"))
									{
                                        HabboIM.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
									}
									else
									{
                                        if (Session.GetHabbo().CurrentQuestId > 0 && HabboIM.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "FINDNEONFLOOR" && class2.GetBaseItem().Name.Contains("party_floor"))
										{
                                            HabboIM.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
										}
										else
										{
                                            if (Session.GetHabbo().CurrentQuestId > 0 && HabboIM.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "FINDDISCOBALL" && class2.GetBaseItem().Name.Contains("party_ball"))
											{
                                                HabboIM.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
											}
											else
											{
                                                if (Session.GetHabbo().CurrentQuestId > 0 && HabboIM.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "FINDJUKEBOX" && class2.GetBaseItem().Name.Contains("jukebox"))
                                                {
                                                    HabboIM.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
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
			catch
			{
			}
		}
	}
}
