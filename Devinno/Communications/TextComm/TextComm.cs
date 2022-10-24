using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Communications.TextComm
{
    public class TextComm
    {
        public static byte[] MakePacket(Encoding enc, byte id, byte cmd, string message)
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

        public static byte[] ParsePacket(byte[] data)
        {
            var bDLE = false;
            var bValid = true;
            bool bComplete = false;

            List<byte> ls = new List<byte>();
            foreach (var d in data)
            {
                byte v = d;
                if (bDLE)
                {
                    bDLE = false;
                    if (v >= 0x10) v -= 0x10;
                    else bValid = false;
                }

                switch (d)
                {
                    case 0x02: ls.Clear(); bValid = true; break;
                    case 0x03: bComplete = true; break;
                    case 0x10: bDLE = true; break;
                    default: ls.Add(v); break;
                }
                if (bComplete) break;
            }

            if (bComplete && bValid) return ls.ToArray();
            else return null;
        }
    }
}
