using HabboIM;
using HabboIM.Util;
using System.Collections.Generic;
using System.Text;

namespace HabboIM.Messages
{
    public sealed class ServerMessage
    {
        private uint MessageId;
        private List<byte> Body;

        public uint Id
        {
            get
            {
                return this.MessageId;
            }
        }

        public string Header
        {
            get
            {
                return HabboIM.GetDefaultEncoding().GetString(Base64Encoding.Encodeuint(this.MessageId, 2));
            }
        }

        public int Length
        {
            get
            {
                return this.Body.Count;
            }
        }

        public ServerMessage()
        {
        }

        public ServerMessage(uint _MessageId)
        {
            this.Init(_MessageId);
        }

        public override string ToString()
        {
            return this.Header + HabboIM.GetDefaultEncoding().GetString(this.Body.ToArray());
        }

        public string ToBodyString()
        {
            return HabboIM.GetDefaultEncoding().GetString(this.Body.ToArray());
        }

        public void Clear()
        {
            this.Body.Clear();
        }

        public void Init(uint _MessageId)
        {
            this.MessageId = _MessageId;
            this.Body = new List<byte>();
        }

        public void AppendByte(byte b)
        {
            this.Body.Add(b);
        }

        public void AppendBytes(byte[] Data)
        {
            if (Data == null || Data.Length == 0)
                return;
            this.Body.AddRange((IEnumerable<byte>)Data);
        }

        public void AppendString(string s, Encoding Encoding)
        {
            if (s == null || s.Length == 0)
                return;
            this.AppendBytes(Encoding.GetBytes(s));
        }

        public void AppendString(string s)
        {
            this.AppendString(s, HabboIM.GetDefaultEncoding());
        }

        public void AppendStringWithBreak(string s)
        {
            this.AppendStringWithBreak(s, (byte)2);
        }

        public void AppendStringWithBreak(string s, byte BreakChar)
        {
            this.AppendString(s);
            this.AppendByte(BreakChar);
        }

        public void AppendInt32(int i)
        {
            this.AppendBytes(WireEncoding.EncodeInt32(i));
        }

        public void AppendRawInt32(int i)
        {
            this.AppendString(i.ToString(), Encoding.ASCII);
        }

        public void AppendUInt(uint i)
        {
            this.AppendInt32((int)i);
        }

        public void AppendRawUInt(uint i)
        {
            this.AppendRawInt32((int)i);
        }

        public void AppendBoolean(bool Bool)
        {
            if (Bool)
                this.Body.Add((byte)73);
            else
                this.Body.Add((byte)72);
        }

        public byte[] GetBytes()
        {
            byte[] numArray1 = new byte[this.Length + 3];
            byte[] numArray2 = Base64Encoding.Encodeuint(this.MessageId, 2);
            numArray1[0] = numArray2[0];
            numArray1[1] = numArray2[1];
            for (int index = 0; index < this.Length; ++index)
                numArray1[index + 2] = this.Body[index];
            numArray1[numArray1.Length - 1] = (byte)1;
            return numArray1;
        }
    }
}
