using HabboIM.HabboHotel.GameClients;
using HabboIM.HabboHotel.Rooms;
using HabboIM.Messages;
using HabboIM.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HabboIM.Communication.Messages.Rooms.Polls
{
    class AnswerInfobusPoll : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            Room Room = HabboIM.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            int AnswerId = Event.PopWiredInt32();
            //int QuestionId = Room.CurrentPollId;  <--- no needed?

            Room.InfobusAnswers.Add(AnswerId);

            /*using (DatabaseClient dbClient = HabboIM.GetDatabase().GetClient())
            {
                dbClient.ExecuteQuery("INSERT INTO `infobus_results` (`question_id`, `answer_id`) VALUES ('" + QuestionId + "', '" + AnswerId + "')");
            }*/
        }
    }
}
