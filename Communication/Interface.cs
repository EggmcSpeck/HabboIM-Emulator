using System;
using HabboIM.HabboHotel.GameClients;
using HabboIM.Messages;
namespace HabboIM
{
	internal interface Interface
	{
		void Handle(GameClient Session, ClientMessage Event);
	}
}
