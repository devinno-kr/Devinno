using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Communications.TextComm
{
    internal class TextComm
    {
        internal static byte[] MakePacket(Encoding enc, byte id, byte cmd, string message)
        {
            List<byte> ls = new List<byte>();
            ls.Add(id);
            ls.Add(cmd);
            if (!string.IsNullOrEmpty(message)) ls.AddRange(enc.GetBytes(message));
            ls.Add(Convert.ToByte(ls.Select(x => (int)x).Sum() & 0xFF));

            List<byte> ret = new List<byte>();
            ret.Add(0x02);
            foreach (var v in ls)
                if (v == 0x02 || v == 0x03 || v == 0x10) { ret.Add(0x10); ret.Add(Convert.ToByte(v + 0x10)); }
                else ret.Add(v);
            ret.Add(0x03);

            return ret.ToArray();
        }
    }
}
