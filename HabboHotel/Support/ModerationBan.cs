using System;
namespace HabboIM.HabboHotel.Support
{
	internal sealed class ModerationBan
	{
		public ModerationBanType Type;
		public string Variable;
		public string ReasonMessage;
		public double Expire;
		public bool Expired
		{
			get
			{
				return HabboIM.GetUnixTimestamp() >= this.Expire;
			}
		}
		public ModerationBan(ModerationBanType mType, string mVariable, string mReasonMessage, double mExpire)
		{
			this.Type = mType;
			this.Variable = mVariable;
			this.ReasonMessage = mReasonMessage;
			this.Expire = mExpire;
		}
	}
}
