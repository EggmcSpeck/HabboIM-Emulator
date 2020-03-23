using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
using HabboIM.HabboHotel.Users.Messenger;
using HabboIM.Storage;
namespace HabboIM.Communication.Messages.Messenger
{
	internal sealed class AcceptBuddyMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetMessenger() != null)
			{
				int num = Event.PopWiredInt32();
				for (int i = 0; i < num; i++)
				{
					uint uint_ = Event.PopWiredUInt();
					MessengerRequest @class = Session.GetHabbo().GetMessenger().method_4(uint_);
					if (@class != null)
					{
						if (@class.To != Session.GetHabbo().Id)
						{
							break;
						}

						if (!Session.GetHabbo().GetMessenger().method_9(@class.To, @class.From))
						{
							Session.GetHabbo().GetMessenger().method_12(@class.From);
						}

						Session.GetHabbo().GetMessenger().method_11(uint_);

                        if (Session.GetHabbo().FriendStreamEnabled)
                        {
                            using (DatabaseClient class2 = HabboIM.GetDatabase().GetClient())
                            {
                                string fromlook = HabboIM.FilterString(@class.SenderFigure);
                                string tolook = HabboIM.FilterString(Session.GetHabbo().Figure);
                                class2.AddParamWithValue("fromusername", @class.senderUsername);
                                class2.AddParamWithValue("tousername", Session.GetHabbo().Username);
                                class2.AddParamWithValue("fromlook", fromlook);
                                class2.AddParamWithValue("tolook", tolook);
                                class2.ExecuteQuery("INSERT INTO `friend_stream` (`id`, `type`, `userid`, `gender`, `look`, `time`, `data`, `data_extra`) VALUES (NULL, '0', '" + Session.GetHabbo().Id + "', '" + @class.SenderGender.ToUpper() + "', @fromlook, UNIX_TIMESTAMP(), '" + @class.From + "', @fromusername);");
                                class2.ExecuteQuery("INSERT INTO `friend_stream` (`id`, `type`, `userid`, `gender`, `look`, `time`, `data`, `data_extra`) VALUES (NULL, '0', '" + @class.From + "', '" + Session.GetHabbo().Gender.ToUpper() + "', @tolook, UNIX_TIMESTAMP(), '" + Session.GetHabbo().Id + "', @tousername);");
                            }
                        }
					}
				}
			}
		}
	}
}
