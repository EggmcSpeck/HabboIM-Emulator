using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM.Communication.Messages.Help
{
	internal sealed class GetFaqCategoriesMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.SendMessage(HabboIM.GetGame().GetHelpTool().method_8());
		}
	}
}
