using System;
using System.Data;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Users
{
    internal sealed class LoadUserGroupsEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            DataTable dataTable_ = Session.GetHabbo().dataTable_0;
            if (dataTable_ != null)
            {
                ServerMessage Message = new ServerMessage(915u);
                Message.AppendInt32(dataTable_.Rows.Count);
                foreach (DataRow dataRow in dataTable_.Rows)
                {

                    if (Session.GetHabbo().boyfriend == 0 || Session.GetHabbo().lovepoints < 100)
                    {
                        Session.GetHabbo().int_0 = 0;     // LOVELEVEL SINGÖÖÖL

                    }
                    else
                    {


                        if (Session.GetHabbo().lovepoints > 100)
                        {
                            int code = Session.GetHabbo().lovepoints % 100;
                            if (Session.GetHabbo().lovepoints < 50)
                            {
                                Session.GetHabbo().int_0 = Session.GetHabbo().lovepoints - code;
                            }
                            else {
                                Session.GetHabbo().int_0 = Session.GetHabbo().lovepoints + (100 - code);


                            }
                        }
                    }



                        Message.AppendInt32(Session.GetHabbo().int_0);
                    Message.AppendStringWithBreak("Beziehungslevel " + Session.GetHabbo().int_0);
                    Message.AppendStringWithBreak("BZLVL" + Session.GetHabbo().int_0);

                    Message.AppendBoolean(true);

                    Session.SendMessage(Message);
                }
            }
        }
    }
}
