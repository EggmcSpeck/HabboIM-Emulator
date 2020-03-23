using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Quest
{
	internal sealed class GetQuestsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{

            try {
                
                Session.SendMessage(HabboIM.GetGame().GetQuestManager().method_5(Session));
            } catch
            {


            }
         
		}
	}
}
