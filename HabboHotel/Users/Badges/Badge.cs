using System;
namespace HabboIM.HabboHotel.Users.Badges
{
	internal sealed class Badge
	{
		public string Code;

		public int Slot;

		public Badge(string code, int slot)
		{
			this.Code = code;
			this.Slot = slot;
		}
	}
}
