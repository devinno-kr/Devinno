using System;
using System.Collections.Generic;
using System.Text;

namespace Devinno.Extensions
{
    public static class BitExtension
    {
        #region byte
        #region Bit(N)
        public static bool Bit0(this byte n) { const byte p = 0b_00000001; return (n & p) == p; }
        public static bool Bit1(this byte n) { const byte p = 0b_00000010; return (n & p) == p; }
        public static bool Bit2(this byte n) { const byte p = 0b_00000100; return (n & p) == p; }
        public static bool Bit3(this byte n) { const byte p = 0b_00001000; return (n & p) == p; }
        public static bool Bit4(this byte n) { const byte p = 0b_00010000; return (n & p) == p; }
        public static bool Bit5(this byte n) { const byte p = 0b_00100000; return (n & p) == p; }
        public static bool Bit6(this byte n) { const byte p = 0b_01000000; return (n & p) == p; }
        public static bool Bit7(this byte n) { const byte p = 0b_10000000; return (n & p) == p; }

        public static void Bit0(this ref byte n, bool v) { unchecked { const byte p = 0b_00000001; if (v) n |= p; else n &= (byte)~p; } }
        public static void Bit1(this ref byte n, bool v) { unchecked { const byte p = 0b_00000010; if (v) n |= p; else n &= (byte)~p; } }
        public static void Bit2(this ref byte n, bool v) { unchecked { const byte p = 0b_00000100; if (v) n |= p; else n &= (byte)~p; } }
        public static void Bit3(this ref byte n, bool v) { unchecked { const byte p = 0b_00001000; if (v) n |= p; else n &= (byte)~p; } }
        public static void Bit4(this ref byte n, bool v) { unchecked { const byte p = 0b_00010000; if (v) n |= p; else n &= (byte)~p; } }
        public static void Bit5(this ref byte n, bool v) { unchecked { const byte p = 0b_00100000; if (v) n |= p; else n &= (byte)~p; } }
        public static void Bit6(this ref byte n, bool v) { unchecked { const byte p = 0b_01000000; if (v) n |= p; else n &= (byte)~p; } }
        public static void Bit7(this ref byte n, bool v) { unchecked { const byte p = 0b_10000000; if (v) n |= p; else n &= (byte)~p; } }
        #endregion
        #region Bit
        public static bool Bit(this byte n, int index)
        {
            bool r = false;
            switch (index)
            {
                case 0: r = n.Bit0(); break;
                case 1: r = n.Bit1(); break;
                case 2: r = n.Bit2(); break;
                case 3: r = n.Bit3(); break;
                case 4: r = n.Bit4(); break;
                case 5: r = n.Bit5(); break;
                case 6: r = n.Bit6(); break;
                case 7: r = n.Bit7(); break;
            }
            return r;
        }

        public static void Bit(this ref byte n, int index, bool v)
        {
            switch (index)
            {
                case 0: n.Bit0(v); break;
                case 1: n.Bit1(v); break;
                case 2: n.Bit2(v); break;
                case 3: n.Bit3(v); break;
                case 4: n.Bit4(v); break;
                case 5: n.Bit5(v); break;
                case 6: n.Bit6(v); break;
                case 7: n.Bit7(v); break;
            }
        }
        #endregion
        #endregion

        #region short
        #region Bit(N)
        public static bool Bit0(this short n) { unchecked { const ushort p = 0b_00000000_00000001; return (n & p) == p; } }
        public static bool Bit1(this short n) { unchecked { const ushort p = 0b_00000000_00000010; return (n & p) == p; } }
        public static bool Bit2(this short n) { unchecked { const ushort p = 0b_00000000_00000100; return (n & p) == p; } }
        public static bool Bit3(this short n) { unchecked { const ushort p = 0b_00000000_00001000; return (n & p) == p; } }
        public static bool Bit4(this short n) { unchecked { const ushort p = 0b_00000000_00010000; return (n & p) == p; } }
        public static bool Bit5(this short n) { unchecked { const ushort p = 0b_00000000_00100000; return (n & p) == p; } }
        public static bool Bit6(this short n) { unchecked { const ushort p = 0b_00000000_01000000; return (n & p) == p; } }
        public static bool Bit7(this short n) { unchecked { const ushort p = 0b_00000000_10000000; return (n & p) == p; } }
        public static bool Bit8(this short n) { unchecked { const ushort p = 0b_00000001_00000000; return (n & p) == p; } }
        public static bool Bit9(this short n) { unchecked { const ushort p = 0b_00000010_00000000; return (n & p) == p; } }
        public static bool Bit10(this short n) { unchecked { const ushort p = 0b_00000100_00000000; return (n & p) == p; } }
        public static bool Bit11(this short n) { unchecked { const ushort p = 0b_00001000_00000000; return (n & p) == p; } }
        public static bool Bit12(this short n) { unchecked { const ushort p = 0b_00010000_00000000; return (n & p) == p; } }
        public static bool Bit13(this short n) { unchecked { const ushort p = 0b_00100000_00000000; return (n & p) == p; } }
        public static bool Bit14(this short n) { unchecked { const ushort p = 0b_01000000_00000000; return (n & p) == p; } }
        public static bool Bit15(this short n) { unchecked { const ushort p = 0b_10000000_00000000; return (n & p) == p; } }

        public static void Bit0(this ref short n, bool v) { unchecked { const ushort p = 0b_00000000_00000001; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit1(this ref short n, bool v) { unchecked { const ushort p = 0b_00000000_00000010; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit2(this ref short n, bool v) { unchecked { const ushort p = 0b_00000000_00000100; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit3(this ref short n, bool v) { unchecked { const ushort p = 0b_00000000_00001000; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit4(this ref short n, bool v) { unchecked { const ushort p = 0b_00000000_00010000; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit5(this ref short n, bool v) { unchecked { const ushort p = 0b_00000000_00100000; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit6(this ref short n, bool v) { unchecked { const ushort p = 0b_00000000_01000000; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit7(this ref short n, bool v) { unchecked { const ushort p = 0b_00000000_10000000; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit8(this ref short n, bool v) { unchecked { const ushort p = 0b_00000001_00000000; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit9(this ref short n, bool v) { unchecked { const ushort p = 0b_00000010_00000000; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit10(this ref short n, bool v) { unchecked { const ushort p = 0b_00000100_00000000; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit11(this ref short n, bool v) { unchecked { const ushort p = 0b_00001000_00000000; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit12(this ref short n, bool v) { unchecked { const ushort p = 0b_00010000_00000000; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit13(this ref short n, bool v) { unchecked { const ushort p = 0b_00100000_00000000; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit14(this ref short n, bool v) { unchecked { const ushort p = 0b_01000000_00000000; if (v) n |= (short)p; else n &= (short)~p; } }
        public static void Bit15(this ref short n, bool v) { unchecked { const ushort p = 0b_10000000_00000000; if (v) n |= (short)p; else n &= (short)~p; } }
        #endregion
        #region Bit
        public static bool Bit(this short n, int index)
        {
            bool r = false;
            switch (index)
            {
                case 0: r = n.Bit0(); break;
                case 1: r = n.Bit1(); break;
                case 2: r = n.Bit2(); break;
                case 3: r = n.Bit3(); break;
                case 4: r = n.Bit4(); break;
                case 5: r = n.Bit5(); break;
                case 6: r = n.Bit6(); break;
                case 7: r = n.Bit7(); break;
                case 8: r = n.Bit8(); break;
                case 9: r = n.Bit9(); break;
                case 10: r = n.Bit10(); break;
                case 11: r = n.Bit11(); break;
                case 12: r = n.Bit12(); break;
                case 13: r = n.Bit13(); break;
                case 14: r = n.Bit14(); break;
                case 15: r = n.Bit15(); break;
            }
            return r;
        }

        public static void Bit(this ref short n, int index, bool v)
        {
            switch (index)
            {
                case 0: n.Bit0(v); break;
                case 1: n.Bit1(v); break;
                case 2: n.Bit2(v); break;
                case 3: n.Bit3(v); break;
                case 4: n.Bit4(v); break;
                case 5: n.Bit5(v); break;
                case 6: n.Bit6(v); break;
                case 7: n.Bit7(v); break;
                case 8: n.Bit8(v); break;
                case 9: n.Bit9(v); break;
                case 10: n.Bit10(v); break;
                case 11: n.Bit11(v); break;
                case 12: n.Bit12(v); break;
                case 13: n.Bit13(v); break;
                case 14: n.Bit14(v); break;
                case 15: n.Bit15(v); break;
            }
        }
        #endregion
        #region Byte(N)
        public static byte Byte0(this short n) { unchecked { return (byte)(n & 0x00FF); } }
        public static byte Byte1(this short n) { unchecked { return (byte)((n & 0xFF00) >> 8); } }

        public static void Byte0(this ref short n, byte v) { unchecked { n = (short)((0xFF00 & n) + (v)); } }
        public static void Byte1(this ref short n, byte v) { unchecked { n = (short)((0x00FF & n) + (v << 8)); } }
        #endregion
        #region GetBytes / FromBytes
        public static byte[] GetBytes(this short n) { return BitConverter.GetBytes(n); }
        public static void FromBytes(this ref short n, byte[] data, int index = 0) { n = BitConverter.ToInt16(data, index); }
        #endregion

        public static ushort Unsign(this short n) { unchecked { return (ushort)n; } }
        #endregion

        #region ushort
        #region Bit(N)
        public static bool Bit0(this ushort n) { unchecked { const ushort p = 0b_00000000_00000001; return (n & p) == p; } }
        public static bool Bit1(this ushort n) { unchecked { const ushort p = 0b_00000000_00000010; return (n & p) == p; } }
        public static bool Bit2(this ushort n) { unchecked { const ushort p = 0b_00000000_00000100; return (n & p) == p; } }
        public static bool Bit3(this ushort n) { unchecked { const ushort p = 0b_00000000_00001000; return (n & p) == p; } }
        public static bool Bit4(this ushort n) { unchecked { const ushort p = 0b_00000000_00010000; return (n & p) == p; } }
        public static bool Bit5(this ushort n) { unchecked { const ushort p = 0b_00000000_00100000; return (n & p) == p; } }
        public static bool Bit6(this ushort n) { unchecked { const ushort p = 0b_00000000_01000000; return (n & p) == p; } }
        public static bool Bit7(this ushort n) { unchecked { const ushort p = 0b_00000000_10000000; return (n & p) == p; } }
        public static bool Bit8(this ushort n) { unchecked { const ushort p = 0b_00000001_00000000; return (n & p) == p; } }
        public static bool Bit9(this ushort n) { unchecked { const ushort p = 0b_00000010_00000000; return (n & p) == p; } }
        public static bool Bit10(this ushort n) { unchecked { const ushort p = 0b_00000100_00000000; return (n & p) == p; } }
        public static bool Bit11(this ushort n) { unchecked { const ushort p = 0b_00001000_00000000; return (n & p) == p; } }
        public static bool Bit12(this ushort n) { unchecked { const ushort p = 0b_00010000_00000000; return (n & p) == p; } }
        public static bool Bit13(this ushort n) { unchecked { const ushort p = 0b_00100000_00000000; return (n & p) == p; } }
        public static bool Bit14(this ushort n) { unchecked { const ushort p = 0b_01000000_00000000; return (n & p) == p; } }
        public static bool Bit15(this ushort n) { unchecked { const ushort p = 0b_10000000_00000000; return (n & p) == p; } }

        public static void Bit0(this ref ushort n, bool v) { unchecked { const ushort p = 0b_00000000_00000001; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit1(this ref ushort n, bool v) { unchecked { const ushort p = 0b_00000000_00000010; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit2(this ref ushort n, bool v) { unchecked { const ushort p = 0b_00000000_00000100; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit3(this ref ushort n, bool v) { unchecked { const ushort p = 0b_00000000_00001000; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit4(this ref ushort n, bool v) { unchecked { const ushort p = 0b_00000000_00010000; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit5(this ref ushort n, bool v) { unchecked { const ushort p = 0b_00000000_00100000; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit6(this ref ushort n, bool v) { unchecked { const ushort p = 0b_00000000_01000000; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit7(this ref ushort n, bool v) { unchecked { const ushort p = 0b_00000000_10000000; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit8(this ref ushort n, bool v) { unchecked { const ushort p = 0b_00000001_00000000; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit9(this ref ushort n, bool v) { unchecked { const ushort p = 0b_00000010_00000000; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit10(this ref ushort n, bool v) { unchecked { const ushort p = 0b_00000100_00000000; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit11(this ref ushort n, bool v) { unchecked { const ushort p = 0b_00001000_00000000; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit12(this ref ushort n, bool v) { unchecked { const ushort p = 0b_00010000_00000000; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit13(this ref ushort n, bool v) { unchecked { const ushort p = 0b_00100000_00000000; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit14(this ref ushort n, bool v) { unchecked { const ushort p = 0b_01000000_00000000; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        public static void Bit15(this ref ushort n, bool v) { unchecked { const ushort p = 0b_10000000_00000000; if (v) n |= (ushort)p; else n &= (ushort)~p; } }
        #endregion
        #region Bit
        public static bool Bit(this ushort n, int index)
        {
            bool r = false;
            switch (index)
            {
                case 0: r = n.Bit0(); break;
                case 1: r = n.Bit1(); break;
                case 2: r = n.Bit2(); break;
                case 3: r = n.Bit3(); break;
                case 4: r = n.Bit4(); break;
                case 5: r = n.Bit5(); break;
                case 6: r = n.Bit6(); break;
                case 7: r = n.Bit7(); break;
                case 8: r = n.Bit8(); break;
                case 9: r = n.Bit9(); break;
                case 10: r = n.Bit10(); break;
                case 11: r = n.Bit11(); break;
                case 12: r = n.Bit12(); break;
                case 13: r = n.Bit13(); break;
                case 14: r = n.Bit14(); break;
                case 15: r = n.Bit15(); break;
            }
            return r;
        }

        public static void Bit(this ref ushort n, int index, bool v)
        {
            switch (index)
            {
                case 0: n.Bit0(v); break;
                case 1: n.Bit1(v); break;
                case 2: n.Bit2(v); break;
                case 3: n.Bit3(v); break;
                case 4: n.Bit4(v); break;
                case 5: n.Bit5(v); break;
                case 6: n.Bit6(v); break;
                case 7: n.Bit7(v); break;
                case 8: n.Bit8(v); break;
                case 9: n.Bit9(v); break;
                case 10: n.Bit10(v); break;
                case 11: n.Bit11(v); break;
                case 12: n.Bit12(v); break;
                case 13: n.Bit13(v); break;
                case 14: n.Bit14(v); break;
                case 15: n.Bit15(v); break;
            }
        }
        #endregion
        #region Byte(N)
        public static byte Byte1(this ushort n) { unchecked { return (byte)(n & 0x00FF); } }
        public static byte Byte0(this ushort n) { unchecked { return (byte)((n & 0xFF00) >> 8); } }

        public static void Byte1(this ref ushort n, byte v) { unchecked { n = (ushort)((0xFF00 & n) + (v)); } }
        public static void Byte0(this ref ushort n, byte v) { unchecked { n = (ushort)((0x00FF & n) + (v << 8)); } }
        #endregion 
        #region GetBytes / FromBytes
        public static byte[] GetBytes(this ushort n) { return BitConverter.GetBytes(n); }
        public static void FromBytes(this ref ushort n, byte[] data, int index = 0) { n = BitConverter.ToUInt16(data, index); }
        #endregion

        public static short Sign(this ushort n) { unchecked { return (short)n; } }
        #endregion

        #region int
        #region Bit(N)
        public static bool Bit0(this int n) { unchecked { const uint p = 0b_00000000_00000000_00000000_00000001; return (n & p) == p; } }
        public static bool Bit1(this int n) { unchecked { const uint p = 0b_00000000_00000000_00000000_00000010; return (n & p) == p; } }
        public static bool Bit2(this int n) { unchecked { const uint p = 0b_00000000_00000000_00000000_00000100; return (n & p) == p; } }
        public static bool Bit3(this int n) { unchecked { const uint p = 0b_00000000_00000000_00000000_00001000; return (n & p) == p; } }
        public static bool Bit4(this int n) { unchecked { const uint p = 0b_00000000_00000000_00000000_00010000; return (n & p) == p; } }
        public static bool Bit5(this int n) { unchecked { const uint p = 0b_00000000_00000000_00000000_00100000; return (n & p) == p; } }
        public static bool Bit6(this int n) { unchecked { const uint p = 0b_00000000_00000000_00000000_01000000; return (n & p) == p; } }
        public static bool Bit7(this int n) { unchecked { const uint p = 0b_00000000_00000000_00000000_10000000; return (n & p) == p; } }
        public static bool Bit8(this int n) { unchecked { const uint p = 0b_00000000_00000000_00000001_00000000; return (n & p) == p; } }
        public static bool Bit9(this int n) { unchecked { const uint p = 0b_00000000_00000000_00000010_00000000; return (n & p) == p; } }
        public static bool Bit10(this int n) { unchecked { const uint p = 0b_00000000_00000000_00000100_00000000; return (n & p) == p; } }
        public static bool Bit11(this int n) { unchecked { const uint p = 0b_00000000_00000000_00001000_00000000; return (n & p) == p; } }
        public static bool Bit12(this int n) { unchecked { const uint p = 0b_00000000_00000000_00010000_00000000; return (n & p) == p; } }
        public static bool Bit13(this int n) { unchecked { const uint p = 0b_00000000_00000000_00100000_00000000; return (n & p) == p; } }
        public static bool Bit14(this int n) { unchecked { const uint p = 0b_00000000_00000000_01000000_00000000; return (n & p) == p; } }
        public static bool Bit15(this int n) { unchecked { const uint p = 0b_00000000_00000000_10000000_00000000; return (n & p) == p; } }
        public static bool Bit16(this int n) { unchecked { const uint p = 0b_00000000_00000001_00000000_00000000; return (n & p) == p; } }
        public static bool Bit17(this int n) { unchecked { const uint p = 0b_00000000_00000010_00000000_00000000; return (n & p) == p; } }
        public static bool Bit18(this int n) { unchecked { const uint p = 0b_00000000_00000100_00000000_00000000; return (n & p) == p; } }
        public static bool Bit19(this int n) { unchecked { const uint p = 0b_00000000_00001000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit20(this int n) { unchecked { const uint p = 0b_00000000_00010000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit21(this int n) { unchecked { const uint p = 0b_00000000_00100000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit22(this int n) { unchecked { const uint p = 0b_00000000_01000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit23(this int n) { unchecked { const uint p = 0b_00000000_10000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit24(this int n) { unchecked { const uint p = 0b_00000001_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit25(this int n) { unchecked { const uint p = 0b_00000010_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit26(this int n) { unchecked { const uint p = 0b_00000100_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit27(this int n) { unchecked { const uint p = 0b_00001000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit28(this int n) { unchecked { const uint p = 0b_00010000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit29(this int n) { unchecked { const uint p = 0b_00100000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit30(this int n) { unchecked { const uint p = 0b_01000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit31(this int n) { unchecked { const uint p = 0b_10000000_00000000_00000000_00000000; return (n & p) == p; } }

        public static void Bit0(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_00000001; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit1(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_00000010; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit2(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_00000100; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit3(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_00001000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit4(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_00010000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit5(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_00100000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit6(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_01000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit7(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_10000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit8(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000001_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit9(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000010_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit10(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000100_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit11(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00001000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit12(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00010000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit13(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00100000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit14(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_01000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit15(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000000_10000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit16(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000001_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit17(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000010_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit18(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00000100_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit19(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00001000_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit20(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00010000_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit21(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_00100000_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit22(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_01000000_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit23(this ref int n, bool v) { unchecked { const uint p = 0b_00000000_10000000_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit24(this ref int n, bool v) { unchecked { const uint p = 0b_00000001_00000000_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit25(this ref int n, bool v) { unchecked { const uint p = 0b_00000010_00000000_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit26(this ref int n, bool v) { unchecked { const uint p = 0b_00000100_00000000_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit27(this ref int n, bool v) { unchecked { const uint p = 0b_00001000_00000000_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit28(this ref int n, bool v) { unchecked { const uint p = 0b_00010000_00000000_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit29(this ref int n, bool v) { unchecked { const uint p = 0b_00100000_00000000_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit30(this ref int n, bool v) { unchecked { const uint p = 0b_01000000_00000000_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        public static void Bit31(this ref int n, bool v) { unchecked { const uint p = 0b_10000000_00000000_00000000_00000000; if (v) n |= (int)p; else n &= (int)~p; } }
        #endregion
        #region Bit
        public static bool Bit(this int n, int index)
        {
            bool r = false;
            switch (index)
            {
                case 0: r = n.Bit0(); break;
                case 1: r = n.Bit1(); break;
                case 2: r = n.Bit2(); break;
                case 3: r = n.Bit3(); break;
                case 4: r = n.Bit4(); break;
                case 5: r = n.Bit5(); break;
                case 6: r = n.Bit6(); break;
                case 7: r = n.Bit7(); break;
                case 8: r = n.Bit8(); break;
                case 9: r = n.Bit9(); break;
                case 10: r = n.Bit10(); break;
                case 11: r = n.Bit11(); break;
                case 12: r = n.Bit12(); break;
                case 13: r = n.Bit13(); break;
                case 14: r = n.Bit14(); break;
                case 15: r = n.Bit15(); break;
                case 16: r = n.Bit16(); break;
                case 17: r = n.Bit17(); break;
                case 18: r = n.Bit18(); break;
                case 19: r = n.Bit19(); break;
                case 20: r = n.Bit20(); break;
                case 21: r = n.Bit21(); break;
                case 22: r = n.Bit22(); break;
                case 23: r = n.Bit23(); break;
                case 24: r = n.Bit24(); break;
                case 25: r = n.Bit25(); break;
                case 26: r = n.Bit26(); break;
                case 27: r = n.Bit27(); break;
                case 28: r = n.Bit28(); break;
                case 29: r = n.Bit29(); break;
                case 30: r = n.Bit30(); break;
                case 31: r = n.Bit31(); break;

            }
            return r;
        }

        public static void Bit(this ref int n, int index, bool v)
        {
            switch (index)
            {
                case 0: n.Bit0(v); break;
                case 1: n.Bit1(v); break;
                case 2: n.Bit2(v); break;
                case 3: n.Bit3(v); break;
                case 4: n.Bit4(v); break;
                case 5: n.Bit5(v); break;
                case 6: n.Bit6(v); break;
                case 7: n.Bit7(v); break;
                case 8: n.Bit8(v); break;
                case 9: n.Bit9(v); break;
                case 10: n.Bit10(v); break;
                case 11: n.Bit11(v); break;
                case 12: n.Bit12(v); break;
                case 13: n.Bit13(v); break;
                case 14: n.Bit14(v); break;
                case 15: n.Bit15(v); break;
                case 16: n.Bit16(v); break;
                case 17: n.Bit17(v); break;
                case 18: n.Bit18(v); break;
                case 19: n.Bit19(v); break;
                case 20: n.Bit20(v); break;
                case 21: n.Bit21(v); break;
                case 22: n.Bit22(v); break;
                case 23: n.Bit23(v); break;
                case 24: n.Bit24(v); break;
                case 25: n.Bit25(v); break;
                case 26: n.Bit26(v); break;
                case 27: n.Bit27(v); break;
                case 28: n.Bit28(v); break;
                case 29: n.Bit29(v); break;
                case 30: n.Bit30(v); break;
                case 31: n.Bit31(v); break;
            }
        }
        #endregion
        #region Byte(N)
        public static byte Byte0(this int n) { unchecked { return (byte)((n & 0x000000FF)); } }
        public static byte Byte1(this int n) { unchecked { return (byte)((n & 0x0000FF00) >> 8); } }
        public static byte Byte2(this int n) { unchecked { return (byte)((n & 0x00FF0000) >> 16); } }
        public static byte Byte3(this int n) { unchecked { return (byte)((n & 0xFF000000) >> 24); } }

        public static void Byte0(this ref int n, byte v) { unchecked { n = (int)((0xFFFFFF00 & n) + (v)); } }
        public static void Byte1(this ref int n, byte v) { unchecked { n = (int)((0xFFFF00FF & n) + (v << 8)); } }
        public static void Byte2(this ref int n, byte v) { unchecked { n = (int)((0xFF00FFFF & n) + (v << 16)); } }
        public static void Byte3(this ref int n, byte v) { unchecked { n = (int)((0x00FFFFFF & n) + (v << 24)); } }
        #endregion
        #region Word(N)
        public static ushort Word0(this int n) { unchecked { return (ushort)((n & 0x0000FFFF)); } }
        public static ushort Word1(this int n) { unchecked { return (ushort)((n & 0xFFFF0000) >> 16); } }

        public static void Word0(this ref int n, ushort v) { unchecked { n = (int)((0xFFFF0000 & n) + (v)); } }
        public static void Word1(this ref int n, ushort v) { unchecked { n = (int)((0x0000FFFF & n) + (v << 16)); } }
        #endregion
        #region GetBytes / FromBytes
        public static byte[] GetBytes(this int n) { return BitConverter.GetBytes(n); }
        public static void FromBytes(this ref int n, byte[] data, int index = 0) { n = BitConverter.ToInt32(data, index); }
        #endregion

        public static uint Unsign(this int n) { unchecked { return (uint)n; } }
        #endregion

        #region uint
        #region Bit(N)
        public static bool Bit0(this uint n) { unchecked { const uint p = 0b_00000000_00000000_00000000_00000001; return (n & p) == p; } }
        public static bool Bit1(this uint n) { unchecked { const uint p = 0b_00000000_00000000_00000000_00000010; return (n & p) == p; } }
        public static bool Bit2(this uint n) { unchecked { const uint p = 0b_00000000_00000000_00000000_00000100; return (n & p) == p; } }
        public static bool Bit3(this uint n) { unchecked { const uint p = 0b_00000000_00000000_00000000_00001000; return (n & p) == p; } }
        public static bool Bit4(this uint n) { unchecked { const uint p = 0b_00000000_00000000_00000000_00010000; return (n & p) == p; } }
        public static bool Bit5(this uint n) { unchecked { const uint p = 0b_00000000_00000000_00000000_00100000; return (n & p) == p; } }
        public static bool Bit6(this uint n) { unchecked { const uint p = 0b_00000000_00000000_00000000_01000000; return (n & p) == p; } }
        public static bool Bit7(this uint n) { unchecked { const uint p = 0b_00000000_00000000_00000000_10000000; return (n & p) == p; } }
        public static bool Bit8(this uint n) { unchecked { const uint p = 0b_00000000_00000000_00000001_00000000; return (n & p) == p; } }
        public static bool Bit9(this uint n) { unchecked { const uint p = 0b_00000000_00000000_00000010_00000000; return (n & p) == p; } }
        public static bool Bit10(this uint n) { unchecked { const uint p = 0b_00000000_00000000_00000100_00000000; return (n & p) == p; } }
        public static bool Bit11(this uint n) { unchecked { const uint p = 0b_00000000_00000000_00001000_00000000; return (n & p) == p; } }
        public static bool Bit12(this uint n) { unchecked { const uint p = 0b_00000000_00000000_00010000_00000000; return (n & p) == p; } }
        public static bool Bit13(this uint n) { unchecked { const uint p = 0b_00000000_00000000_00100000_00000000; return (n & p) == p; } }
        public static bool Bit14(this uint n) { unchecked { const uint p = 0b_00000000_00000000_01000000_00000000; return (n & p) == p; } }
        public static bool Bit15(this uint n) { unchecked { const uint p = 0b_00000000_00000000_10000000_00000000; return (n & p) == p; } }
        public static bool Bit16(this uint n) { unchecked { const uint p = 0b_00000000_00000001_00000000_00000000; return (n & p) == p; } }
        public static bool Bit17(this uint n) { unchecked { const uint p = 0b_00000000_00000010_00000000_00000000; return (n & p) == p; } }
        public static bool Bit18(this uint n) { unchecked { const uint p = 0b_00000000_00000100_00000000_00000000; return (n & p) == p; } }
        public static bool Bit19(this uint n) { unchecked { const uint p = 0b_00000000_00001000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit20(this uint n) { unchecked { const uint p = 0b_00000000_00010000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit21(this uint n) { unchecked { const uint p = 0b_00000000_00100000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit22(this uint n) { unchecked { const uint p = 0b_00000000_01000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit23(this uint n) { unchecked { const uint p = 0b_00000000_10000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit24(this uint n) { unchecked { const uint p = 0b_00000001_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit25(this uint n) { unchecked { const uint p = 0b_00000010_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit26(this uint n) { unchecked { const uint p = 0b_00000100_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit27(this uint n) { unchecked { const uint p = 0b_00001000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit28(this uint n) { unchecked { const uint p = 0b_00010000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit29(this uint n) { unchecked { const uint p = 0b_00100000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit30(this uint n) { unchecked { const uint p = 0b_01000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit31(this uint n) { unchecked { const uint p = 0b_10000000_00000000_00000000_00000000; return (n & p) == p; } }

        public static void Bit0(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_00000001; if (v) n |= p; else n &= ~p; } }
        public static void Bit1(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_00000010; if (v) n |= p; else n &= ~p; } }
        public static void Bit2(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_00000100; if (v) n |= p; else n &= ~p; } }
        public static void Bit3(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_00001000; if (v) n |= p; else n &= ~p; } }
        public static void Bit4(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_00010000; if (v) n |= p; else n &= ~p; } }
        public static void Bit5(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_00100000; if (v) n |= p; else n &= ~p; } }
        public static void Bit6(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_01000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit7(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000000_10000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit8(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000001_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit9(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000010_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit10(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00000100_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit11(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00001000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit12(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00010000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit13(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_00100000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit14(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_01000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit15(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000000_10000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit16(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000001_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit17(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000010_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit18(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00000100_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit19(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00001000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit20(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00010000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit21(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_00100000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit22(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_01000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit23(this ref uint n, bool v) { unchecked { const uint p = 0b_00000000_10000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit24(this ref uint n, bool v) { unchecked { const uint p = 0b_00000001_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit25(this ref uint n, bool v) { unchecked { const uint p = 0b_00000010_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit26(this ref uint n, bool v) { unchecked { const uint p = 0b_00000100_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit27(this ref uint n, bool v) { unchecked { const uint p = 0b_00001000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit28(this ref uint n, bool v) { unchecked { const uint p = 0b_00010000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit29(this ref uint n, bool v) { unchecked { const uint p = 0b_00100000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit30(this ref uint n, bool v) { unchecked { const uint p = 0b_01000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit31(this ref uint n, bool v) { unchecked { const uint p = 0b_10000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        #endregion
        #region Bit
        public static bool Bit(this uint n, uint index)
        {
            bool r = false;
            switch (index)
            {
                case 0: r = n.Bit0(); break;
                case 1: r = n.Bit1(); break;
                case 2: r = n.Bit2(); break;
                case 3: r = n.Bit3(); break;
                case 4: r = n.Bit4(); break;
                case 5: r = n.Bit5(); break;
                case 6: r = n.Bit6(); break;
                case 7: r = n.Bit7(); break;
                case 8: r = n.Bit8(); break;
                case 9: r = n.Bit9(); break;
                case 10: r = n.Bit10(); break;
                case 11: r = n.Bit11(); break;
                case 12: r = n.Bit12(); break;
                case 13: r = n.Bit13(); break;
                case 14: r = n.Bit14(); break;
                case 15: r = n.Bit15(); break;
                case 16: r = n.Bit16(); break;
                case 17: r = n.Bit17(); break;
                case 18: r = n.Bit18(); break;
                case 19: r = n.Bit19(); break;
                case 20: r = n.Bit20(); break;
                case 21: r = n.Bit21(); break;
                case 22: r = n.Bit22(); break;
                case 23: r = n.Bit23(); break;
                case 24: r = n.Bit24(); break;
                case 25: r = n.Bit25(); break;
                case 26: r = n.Bit26(); break;
                case 27: r = n.Bit27(); break;
                case 28: r = n.Bit28(); break;
                case 29: r = n.Bit29(); break;
                case 30: r = n.Bit30(); break;
                case 31: r = n.Bit31(); break;

            }
            return r;
        }

        public static void Bit(this ref uint n, uint index, bool v)
        {
            switch (index)
            {
                case 0: n.Bit0(v); break;
                case 1: n.Bit1(v); break;
                case 2: n.Bit2(v); break;
                case 3: n.Bit3(v); break;
                case 4: n.Bit4(v); break;
                case 5: n.Bit5(v); break;
                case 6: n.Bit6(v); break;
                case 7: n.Bit7(v); break;
                case 8: n.Bit8(v); break;
                case 9: n.Bit9(v); break;
                case 10: n.Bit10(v); break;
                case 11: n.Bit11(v); break;
                case 12: n.Bit12(v); break;
                case 13: n.Bit13(v); break;
                case 14: n.Bit14(v); break;
                case 15: n.Bit15(v); break;
                case 16: n.Bit16(v); break;
                case 17: n.Bit17(v); break;
                case 18: n.Bit18(v); break;
                case 19: n.Bit19(v); break;
                case 20: n.Bit20(v); break;
                case 21: n.Bit21(v); break;
                case 22: n.Bit22(v); break;
                case 23: n.Bit23(v); break;
                case 24: n.Bit24(v); break;
                case 25: n.Bit25(v); break;
                case 26: n.Bit26(v); break;
                case 27: n.Bit27(v); break;
                case 28: n.Bit28(v); break;
                case 29: n.Bit29(v); break;
                case 30: n.Bit30(v); break;
                case 31: n.Bit31(v); break;
            }
        }
        #endregion
        #region Byte(N)
        public static byte Byte0(this uint n) { unchecked { return (byte)((n & 0x000000FF)); } }
        public static byte Byte1(this uint n) { unchecked { return (byte)((n & 0x0000FF00) >> 8); } }
        public static byte Byte2(this uint n) { unchecked { return (byte)((n & 0x00FF0000) >> 16); } }
        public static byte Byte3(this uint n) { unchecked { return (byte)((n & 0xFF000000) >> 24); } }

        public static void Byte0(this ref uint n, byte v) { unchecked { n = ((0xFFFFFF00 & n) + (uint)(v)); } }
        public static void Byte1(this ref uint n, byte v) { unchecked { n = ((0xFFFF00FF & n) + (uint)(v << 8)); } }
        public static void Byte2(this ref uint n, byte v) { unchecked { n = ((0xFF00FFFF & n) + (uint)(v << 16)); } }
        public static void Byte3(this ref uint n, byte v) { unchecked { n = ((0x00FFFFFF & n) + (uint)(v << 24)); } }
        #endregion
        #region Word(N)
        public static ushort Word0(this uint n) { unchecked { return (ushort)((n & 0x0000FFFF)); } }
        public static ushort Word1(this uint n) { unchecked { return (ushort)((n & 0xFFFF0000) >> 16); } }

        public static void Word0(this ref uint n, ushort v) { unchecked { n = ((0xFFFF0000 & n) + (uint)(v)); } }
        public static void Word1(this ref uint n, ushort v) { unchecked { n = ((0x0000FFFF & n) + (uint)(v << 16)); } }
        #endregion
        #region GetBytes / FromBytes
        public static byte[] GetBytes(this uint n) { return BitConverter.GetBytes(n); }
        public static void FromBytes(this ref uint n, byte[] data, int index = 0) { n = BitConverter.ToUInt32(data, index); }
        #endregion

        public static int Sign(this uint n) { unchecked { return (int)n; } }
        #endregion

        #region long
        #region Bit(N)
        public static bool Bit0(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000001; return ((ulong)n & p) == p; } }
        public static bool Bit1(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000010; return ((ulong)n & p) == p; } }
        public static bool Bit2(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000100; return ((ulong)n & p) == p; } }
        public static bool Bit3(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00001000; return ((ulong)n & p) == p; } }
        public static bool Bit4(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00010000; return ((ulong)n & p) == p; } }
        public static bool Bit5(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00100000; return ((ulong)n & p) == p; } }
        public static bool Bit6(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_01000000; return ((ulong)n & p) == p; } }
        public static bool Bit7(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_10000000; return ((ulong)n & p) == p; } }
        public static bool Bit8(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000001_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit9(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000010_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit10(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000100_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit11(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00001000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit12(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00010000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit13(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00100000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit14(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_01000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit15(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_10000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit16(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000001_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit17(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000010_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit18(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000100_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit19(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00001000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit20(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00010000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit21(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00100000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit22(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_01000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit23(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_10000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit24(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000001_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit25(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000010_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit26(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000100_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit27(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00001000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit28(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00010000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit29(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00100000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit30(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_01000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit31(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_10000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit32(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000001_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit33(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000010_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit34(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000100_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit35(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00001000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit36(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00010000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit37(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00100000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit38(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_01000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit39(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_10000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit40(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000001_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit41(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000010_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit42(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00000100_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit43(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00001000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit44(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00010000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit45(this long n) { unchecked { const ulong p = 0b_00000000_00000000_00100000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit46(this long n) { unchecked { const ulong p = 0b_00000000_00000000_01000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit47(this long n) { unchecked { const ulong p = 0b_00000000_00000000_10000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit48(this long n) { unchecked { const ulong p = 0b_00000000_00000001_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit49(this long n) { unchecked { const ulong p = 0b_00000000_00000010_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit50(this long n) { unchecked { const ulong p = 0b_00000000_00000100_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit51(this long n) { unchecked { const ulong p = 0b_00000000_00001000_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit52(this long n) { unchecked { const ulong p = 0b_00000000_00010000_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit53(this long n) { unchecked { const ulong p = 0b_00000000_00100000_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit54(this long n) { unchecked { const ulong p = 0b_00000000_01000000_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit55(this long n) { unchecked { const ulong p = 0b_00000000_10000000_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit56(this long n) { unchecked { const ulong p = 0b_00000001_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit57(this long n) { unchecked { const ulong p = 0b_00000010_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit58(this long n) { unchecked { const ulong p = 0b_00000100_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit59(this long n) { unchecked { const ulong p = 0b_00001000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit60(this long n) { unchecked { const ulong p = 0b_00010000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit61(this long n) { unchecked { const ulong p = 0b_00100000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit62(this long n) { unchecked { const ulong p = 0b_01000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }
        public static bool Bit63(this long n) { unchecked { const ulong p = 0b_10000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return ((ulong)n & p) == p; } }

        public static void Bit0(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000001; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit1(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000010; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit2(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000100; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit3(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00001000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit4(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00010000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit5(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00100000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit6(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_01000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit7(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_10000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit8(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000001_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit9(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000010_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit10(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000100_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit11(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00001000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit12(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00010000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit13(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00100000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit14(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_01000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit15(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_10000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit16(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000001_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit17(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000010_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit18(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000100_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit19(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00001000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit20(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00010000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit21(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00100000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit22(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_01000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit23(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_10000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit24(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000001_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit25(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000010_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit26(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000100_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit27(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00001000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit28(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00010000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit29(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00100000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit30(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_01000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit31(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_10000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit32(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000001_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit33(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000010_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit34(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000100_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit35(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00001000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit36(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00010000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit37(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00100000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit38(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_01000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit39(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_10000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit40(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000001_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit41(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000010_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit42(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000100_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit43(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00001000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit44(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00010000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit45(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00100000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit46(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_01000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit47(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_10000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit48(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000001_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit49(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000010_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit50(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00000100_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit51(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00001000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit52(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00010000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit53(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_00100000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit54(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_01000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit55(this ref long n, bool v) { unchecked { const ulong p = 0b_00000000_10000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit56(this ref long n, bool v) { unchecked { const ulong p = 0b_00000001_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit57(this ref long n, bool v) { unchecked { const ulong p = 0b_00000010_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit58(this ref long n, bool v) { unchecked { const ulong p = 0b_00000100_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit59(this ref long n, bool v) { unchecked { const ulong p = 0b_00001000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit60(this ref long n, bool v) { unchecked { const ulong p = 0b_00010000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit61(this ref long n, bool v) { unchecked { const ulong p = 0b_00100000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit62(this ref long n, bool v) { unchecked { const ulong p = 0b_01000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        public static void Bit63(this ref long n, bool v) { unchecked { const ulong p = 0b_10000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= (long)p; else n &= (long)~p; } }
        #endregion
        #region Bit
        public static bool Bit(this long n, long index)
        {
            bool r = false;
            switch (index)
            {
                case 0: r = n.Bit0(); break;
                case 1: r = n.Bit1(); break;
                case 2: r = n.Bit2(); break;
                case 3: r = n.Bit3(); break;
                case 4: r = n.Bit4(); break;
                case 5: r = n.Bit5(); break;
                case 6: r = n.Bit6(); break;
                case 7: r = n.Bit7(); break;
                case 8: r = n.Bit8(); break;
                case 9: r = n.Bit9(); break;
                case 10: r = n.Bit10(); break;
                case 11: r = n.Bit11(); break;
                case 12: r = n.Bit12(); break;
                case 13: r = n.Bit13(); break;
                case 14: r = n.Bit14(); break;
                case 15: r = n.Bit15(); break;
                case 16: r = n.Bit16(); break;
                case 17: r = n.Bit17(); break;
                case 18: r = n.Bit18(); break;
                case 19: r = n.Bit19(); break;
                case 20: r = n.Bit20(); break;
                case 21: r = n.Bit21(); break;
                case 22: r = n.Bit22(); break;
                case 23: r = n.Bit23(); break;
                case 24: r = n.Bit24(); break;
                case 25: r = n.Bit25(); break;
                case 26: r = n.Bit26(); break;
                case 27: r = n.Bit27(); break;
                case 28: r = n.Bit28(); break;
                case 29: r = n.Bit29(); break;
                case 30: r = n.Bit30(); break;
                case 31: r = n.Bit31(); break;
                case 32: r = n.Bit32(); break;
                case 33: r = n.Bit33(); break;
                case 34: r = n.Bit34(); break;
                case 35: r = n.Bit35(); break;
                case 36: r = n.Bit36(); break;
                case 37: r = n.Bit37(); break;
                case 38: r = n.Bit38(); break;
                case 39: r = n.Bit39(); break;
                case 40: r = n.Bit40(); break;
                case 41: r = n.Bit41(); break;
                case 42: r = n.Bit42(); break;
                case 43: r = n.Bit43(); break;
                case 44: r = n.Bit44(); break;
                case 45: r = n.Bit45(); break;
                case 46: r = n.Bit46(); break;
                case 47: r = n.Bit47(); break;
                case 48: r = n.Bit48(); break;
                case 49: r = n.Bit49(); break;
                case 50: r = n.Bit50(); break;
                case 51: r = n.Bit51(); break;
                case 52: r = n.Bit52(); break;
                case 53: r = n.Bit53(); break;
                case 54: r = n.Bit54(); break;
                case 55: r = n.Bit55(); break;
                case 56: r = n.Bit56(); break;
                case 57: r = n.Bit57(); break;
                case 58: r = n.Bit58(); break;
                case 59: r = n.Bit59(); break;
                case 60: r = n.Bit60(); break;
                case 61: r = n.Bit61(); break;
                case 62: r = n.Bit62(); break;
                case 63: r = n.Bit63(); break;
            }
            return r;
        }

        public static void Bit(this ref long n, long index, bool v)
        {
            switch (index)
            {
                case 0: n.Bit0(v); break;
                case 1: n.Bit1(v); break;
                case 2: n.Bit2(v); break;
                case 3: n.Bit3(v); break;
                case 4: n.Bit4(v); break;
                case 5: n.Bit5(v); break;
                case 6: n.Bit6(v); break;
                case 7: n.Bit7(v); break;
                case 8: n.Bit8(v); break;
                case 9: n.Bit9(v); break;
                case 10: n.Bit10(v); break;
                case 11: n.Bit11(v); break;
                case 12: n.Bit12(v); break;
                case 13: n.Bit13(v); break;
                case 14: n.Bit14(v); break;
                case 15: n.Bit15(v); break;
                case 16: n.Bit16(v); break;
                case 17: n.Bit17(v); break;
                case 18: n.Bit18(v); break;
                case 19: n.Bit19(v); break;
                case 20: n.Bit20(v); break;
                case 21: n.Bit21(v); break;
                case 22: n.Bit22(v); break;
                case 23: n.Bit23(v); break;
                case 24: n.Bit24(v); break;
                case 25: n.Bit25(v); break;
                case 26: n.Bit26(v); break;
                case 27: n.Bit27(v); break;
                case 28: n.Bit28(v); break;
                case 29: n.Bit29(v); break;
                case 30: n.Bit30(v); break;
                case 31: n.Bit31(v); break;
                case 32: n.Bit32(v); break;
                case 33: n.Bit33(v); break;
                case 34: n.Bit34(v); break;
                case 35: n.Bit35(v); break;
                case 36: n.Bit36(v); break;
                case 37: n.Bit37(v); break;
                case 38: n.Bit38(v); break;
                case 39: n.Bit39(v); break;
                case 40: n.Bit40(v); break;
                case 41: n.Bit41(v); break;
                case 42: n.Bit42(v); break;
                case 43: n.Bit43(v); break;
                case 44: n.Bit44(v); break;
                case 45: n.Bit45(v); break;
                case 46: n.Bit46(v); break;
                case 47: n.Bit47(v); break;
                case 48: n.Bit48(v); break;
                case 49: n.Bit49(v); break;
                case 50: n.Bit50(v); break;
                case 51: n.Bit51(v); break;
                case 52: n.Bit52(v); break;
                case 53: n.Bit53(v); break;
                case 54: n.Bit54(v); break;
                case 55: n.Bit55(v); break;
                case 56: n.Bit56(v); break;
                case 57: n.Bit57(v); break;
                case 58: n.Bit58(v); break;
                case 59: n.Bit59(v); break;
                case 60: n.Bit60(v); break;
                case 61: n.Bit61(v); break;
                case 62: n.Bit62(v); break;
                case 63: n.Bit63(v); break;
            }
        }
        #endregion
        #region Byte(N)
        public static byte Byte0(this long n) { unchecked { return (byte)(((ulong)n & 0x00000000000000FF)); } }
        public static byte Byte1(this long n) { unchecked { return (byte)(((ulong)n & 0x000000000000FF00) >> 8); } }
        public static byte Byte2(this long n) { unchecked { return (byte)(((ulong)n & 0x0000000000FF0000) >> 16); } }
        public static byte Byte3(this long n) { unchecked { return (byte)(((ulong)n & 0x00000000FF000000) >> 24); } }
        public static byte Byte4(this long n) { unchecked { return (byte)(((ulong)n & 0x000000FF00000000)); } }
        public static byte Byte5(this long n) { unchecked { return (byte)(((ulong)n & 0x0000FF0000000000) >> 8); } }
        public static byte Byte6(this long n) { unchecked { return (byte)(((ulong)n & 0x00FF000000000000) >> 16); } }
        public static byte Byte7(this long n) { unchecked { return (byte)(((ulong)n & 0xFF00000000000000) >> 24); } }

        public static void Byte0(this ref long n, byte v) { unchecked { n = (long)((0xFFFFFFFFFFFFFF00 & (ulong)n) + (ulong)(v)); } }
        public static void Byte1(this ref long n, byte v) { unchecked { n = (long)((0xFFFFFFFFFFFF00FF & (ulong)n) + (ulong)(v << 8)); } }
        public static void Byte2(this ref long n, byte v) { unchecked { n = (long)((0xFFFFFFFFFF00FFFF & (ulong)n) + (ulong)(v << 16)); } }
        public static void Byte3(this ref long n, byte v) { unchecked { n = (long)((0xFFFFFFFF00FFFFFF & (ulong)n) + (ulong)(v << 24)); } }
        public static void Byte4(this ref long n, byte v) { unchecked { n = (long)((0xFFFFFF00FFFFFFFF & (ulong)n) + (ulong)(v << 32)); } }
        public static void Byte5(this ref long n, byte v) { unchecked { n = (long)((0xFFFF00FFFFFFFFFF & (ulong)n) + (ulong)(v << 40)); } }
        public static void Byte6(this ref long n, byte v) { unchecked { n = (long)((0xFF00FFFFFFFFFFFF & (ulong)n) + (ulong)(v << 48)); } }
        public static void Byte7(this ref long n, byte v) { unchecked { n = (long)((0x00FFFFFFFFFFFFFF & (ulong)n) + (ulong)(v << 56)); } }
        #endregion
        #region Word(N)
        public static ushort Word0(this long n) { unchecked { return (byte)(((ulong)n & 0x000000000000FFFF)); } }
        public static ushort Word1(this long n) { unchecked { return (byte)(((ulong)n & 0x00000000FFFF0000) >> 16); } }
        public static ushort Word2(this long n) { unchecked { return (byte)(((ulong)n & 0x0000FFFF00000000)); } }
        public static ushort Word3(this long n) { unchecked { return (byte)(((ulong)n & 0xFFFF000000000000) >> 16); } }

        public static void Word0(this ref long n, ushort v) { unchecked { n = (long)((0x00000000FFFF0000 & (ulong)n) + (ulong)(v)); } }
        public static void Word1(this ref long n, ushort v) { unchecked { n = (long)((0xFFFFFFFF0000FFFF & (ulong)n) + (ulong)(v << 16)); } }
        public static void Word2(this ref long n, ushort v) { unchecked { n = (long)((0xFFFF0000FFFFFFFF & (ulong)n) + (ulong)(v << 32)); } }
        public static void Word3(this ref long n, ushort v) { unchecked { n = (long)((0x0000FFFFFFFFFFFF & (ulong)n) + (ulong)(v << 48)); } }
        #endregion
        #region DWord(N)
        public static uint DWord0(this long n) { unchecked { return (byte)(((ulong)n & 0x00000000FFFFFFFF)); } }
        public static uint DWord1(this long n) { unchecked { return (byte)(((ulong)n & 0xFFFFFFFF00000000) >> 16); } }

        public static void DWord0(this ref long n, uint v) { unchecked { n = (long)((0xFFFFFFFF00000000 & (ulong)n) + (ulong)(v)); } }
        public static void DWord1(this ref long n, uint v) { unchecked { n = (long)((0x00000000FFFFFFFF & (ulong)n) + (ulong)(v << 32)); } }
        #endregion
        #region GetBytes / FromBytes
        public static byte[] GetBytes(this long n) { return BitConverter.GetBytes(n); }
        public static void FromBytes(this ref long n, byte[] data, int index = 0) { n = BitConverter.ToInt64(data, index); }
        #endregion

        public static ulong Unsign(this long n) { unchecked { return (ulong)n; } }
        #endregion

        #region ulong
        #region Bit(N)
        public static bool Bit0(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000001; return (n & p) == p; } }
        public static bool Bit1(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000010; return (n & p) == p; } }
        public static bool Bit2(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000100; return (n & p) == p; } }
        public static bool Bit3(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00001000; return (n & p) == p; } }
        public static bool Bit4(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00010000; return (n & p) == p; } }
        public static bool Bit5(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00100000; return (n & p) == p; } }
        public static bool Bit6(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_01000000; return (n & p) == p; } }
        public static bool Bit7(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_10000000; return (n & p) == p; } }
        public static bool Bit8(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000001_00000000; return (n & p) == p; } }
        public static bool Bit9(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000010_00000000; return (n & p) == p; } }
        public static bool Bit10(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000100_00000000; return (n & p) == p; } }
        public static bool Bit11(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00001000_00000000; return (n & p) == p; } }
        public static bool Bit12(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00010000_00000000; return (n & p) == p; } }
        public static bool Bit13(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00100000_00000000; return (n & p) == p; } }
        public static bool Bit14(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_01000000_00000000; return (n & p) == p; } }
        public static bool Bit15(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_10000000_00000000; return (n & p) == p; } }
        public static bool Bit16(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000001_00000000_00000000; return (n & p) == p; } }
        public static bool Bit17(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000010_00000000_00000000; return (n & p) == p; } }
        public static bool Bit18(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000100_00000000_00000000; return (n & p) == p; } }
        public static bool Bit19(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00001000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit20(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00010000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit21(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00100000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit22(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_01000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit23(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_10000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit24(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000001_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit25(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000010_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit26(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000100_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit27(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00001000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit28(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00010000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit29(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00100000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit30(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_01000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit31(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_10000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit32(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000001_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit33(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000010_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit34(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000100_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit35(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00001000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit36(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00010000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit37(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00100000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit38(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_01000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit39(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000000_10000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit40(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000001_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit41(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000010_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit42(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00000100_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit43(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00001000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit44(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00010000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit45(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_00100000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit46(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_01000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit47(this ulong n) { unchecked { const ulong p = 0b_00000000_00000000_10000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit48(this ulong n) { unchecked { const ulong p = 0b_00000000_00000001_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit49(this ulong n) { unchecked { const ulong p = 0b_00000000_00000010_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit50(this ulong n) { unchecked { const ulong p = 0b_00000000_00000100_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit51(this ulong n) { unchecked { const ulong p = 0b_00000000_00001000_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit52(this ulong n) { unchecked { const ulong p = 0b_00000000_00010000_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit53(this ulong n) { unchecked { const ulong p = 0b_00000000_00100000_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit54(this ulong n) { unchecked { const ulong p = 0b_00000000_01000000_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit55(this ulong n) { unchecked { const ulong p = 0b_00000000_10000000_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit56(this ulong n) { unchecked { const ulong p = 0b_00000001_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit57(this ulong n) { unchecked { const ulong p = 0b_00000010_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit58(this ulong n) { unchecked { const ulong p = 0b_00000100_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit59(this ulong n) { unchecked { const ulong p = 0b_00001000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit60(this ulong n) { unchecked { const ulong p = 0b_00010000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit61(this ulong n) { unchecked { const ulong p = 0b_00100000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit62(this ulong n) { unchecked { const ulong p = 0b_01000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }
        public static bool Bit63(this ulong n) { unchecked { const ulong p = 0b_10000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; return (n & p) == p; } }

        public static void Bit0(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000001; if (v) n |= p; else n &= ~p; } }
        public static void Bit1(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000010; if (v) n |= p; else n &= ~p; } }
        public static void Bit2(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000100; if (v) n |= p; else n &= ~p; } }
        public static void Bit3(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00001000; if (v) n |= p; else n &= ~p; } }
        public static void Bit4(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00010000; if (v) n |= p; else n &= ~p; } }
        public static void Bit5(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_00100000; if (v) n |= p; else n &= ~p; } }
        public static void Bit6(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_01000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit7(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000000_10000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit8(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000001_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit9(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000010_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit10(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00000100_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit11(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00001000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit12(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00010000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit13(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_00100000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit14(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_01000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit15(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000000_10000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit16(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000001_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit17(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000010_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit18(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00000100_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit19(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00001000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit20(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00010000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit21(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_00100000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit22(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_01000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit23(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000000_10000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit24(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000001_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit25(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000010_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit26(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00000100_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit27(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00001000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit28(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00010000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit29(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_00100000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit30(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_01000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit31(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000000_10000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit32(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000001_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit33(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000010_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit34(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00000100_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit35(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00001000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit36(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00010000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit37(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_00100000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit38(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_01000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit39(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000000_10000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit40(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000001_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit41(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000010_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit42(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00000100_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit43(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00001000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit44(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00010000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit45(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_00100000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit46(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_01000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit47(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000000_10000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit48(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000001_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit49(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000010_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit50(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00000100_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit51(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00001000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit52(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00010000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit53(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_00100000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit54(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_01000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit55(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000000_10000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit56(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000001_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit57(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000010_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit58(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00000100_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit59(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00001000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit60(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00010000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit61(this ref ulong n, bool v) { unchecked { const ulong p = 0b_00100000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit62(this ref ulong n, bool v) { unchecked { const ulong p = 0b_01000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        public static void Bit63(this ref ulong n, bool v) { unchecked { const ulong p = 0b_10000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000; if (v) n |= p; else n &= ~p; } }
        #endregion
        #region Bit
        public static bool Bit(this ulong n, ulong index)
        {
            bool r = false;
            switch (index)
            {
                case 0: r = n.Bit0(); break;
                case 1: r = n.Bit1(); break;
                case 2: r = n.Bit2(); break;
                case 3: r = n.Bit3(); break;
                case 4: r = n.Bit4(); break;
                case 5: r = n.Bit5(); break;
                case 6: r = n.Bit6(); break;
                case 7: r = n.Bit7(); break;
                case 8: r = n.Bit8(); break;
                case 9: r = n.Bit9(); break;
                case 10: r = n.Bit10(); break;
                case 11: r = n.Bit11(); break;
                case 12: r = n.Bit12(); break;
                case 13: r = n.Bit13(); break;
                case 14: r = n.Bit14(); break;
                case 15: r = n.Bit15(); break;
                case 16: r = n.Bit16(); break;
                case 17: r = n.Bit17(); break;
                case 18: r = n.Bit18(); break;
                case 19: r = n.Bit19(); break;
                case 20: r = n.Bit20(); break;
                case 21: r = n.Bit21(); break;
                case 22: r = n.Bit22(); break;
                case 23: r = n.Bit23(); break;
                case 24: r = n.Bit24(); break;
                case 25: r = n.Bit25(); break;
                case 26: r = n.Bit26(); break;
                case 27: r = n.Bit27(); break;
                case 28: r = n.Bit28(); break;
                case 29: r = n.Bit29(); break;
                case 30: r = n.Bit30(); break;
                case 31: r = n.Bit31(); break;
                case 32: r = n.Bit32(); break;
                case 33: r = n.Bit33(); break;
                case 34: r = n.Bit34(); break;
                case 35: r = n.Bit35(); break;
                case 36: r = n.Bit36(); break;
                case 37: r = n.Bit37(); break;
                case 38: r = n.Bit38(); break;
                case 39: r = n.Bit39(); break;
                case 40: r = n.Bit40(); break;
                case 41: r = n.Bit41(); break;
                case 42: r = n.Bit42(); break;
                case 43: r = n.Bit43(); break;
                case 44: r = n.Bit44(); break;
                case 45: r = n.Bit45(); break;
                case 46: r = n.Bit46(); break;
                case 47: r = n.Bit47(); break;
                case 48: r = n.Bit48(); break;
                case 49: r = n.Bit49(); break;
                case 50: r = n.Bit50(); break;
                case 51: r = n.Bit51(); break;
                case 52: r = n.Bit52(); break;
                case 53: r = n.Bit53(); break;
                case 54: r = n.Bit54(); break;
                case 55: r = n.Bit55(); break;
                case 56: r = n.Bit56(); break;
                case 57: r = n.Bit57(); break;
                case 58: r = n.Bit58(); break;
                case 59: r = n.Bit59(); break;
                case 60: r = n.Bit60(); break;
                case 61: r = n.Bit61(); break;
                case 62: r = n.Bit62(); break;
                case 63: r = n.Bit63(); break;
            }
            return r;
        }

        public static void Bit(this ref ulong n, ulong index, bool v)
        {
            switch (index)
            {
                case 0: n.Bit0(v); break;
                case 1: n.Bit1(v); break;
                case 2: n.Bit2(v); break;
                case 3: n.Bit3(v); break;
                case 4: n.Bit4(v); break;
                case 5: n.Bit5(v); break;
                case 6: n.Bit6(v); break;
                case 7: n.Bit7(v); break;
                case 8: n.Bit8(v); break;
                case 9: n.Bit9(v); break;
                case 10: n.Bit10(v); break;
                case 11: n.Bit11(v); break;
                case 12: n.Bit12(v); break;
                case 13: n.Bit13(v); break;
                case 14: n.Bit14(v); break;
                case 15: n.Bit15(v); break;
                case 16: n.Bit16(v); break;
                case 17: n.Bit17(v); break;
                case 18: n.Bit18(v); break;
                case 19: n.Bit19(v); break;
                case 20: n.Bit20(v); break;
                case 21: n.Bit21(v); break;
                case 22: n.Bit22(v); break;
                case 23: n.Bit23(v); break;
                case 24: n.Bit24(v); break;
                case 25: n.Bit25(v); break;
                case 26: n.Bit26(v); break;
                case 27: n.Bit27(v); break;
                case 28: n.Bit28(v); break;
                case 29: n.Bit29(v); break;
                case 30: n.Bit30(v); break;
                case 31: n.Bit31(v); break;
                case 32: n.Bit32(v); break;
                case 33: n.Bit33(v); break;
                case 34: n.Bit34(v); break;
                case 35: n.Bit35(v); break;
                case 36: n.Bit36(v); break;
                case 37: n.Bit37(v); break;
                case 38: n.Bit38(v); break;
                case 39: n.Bit39(v); break;
                case 40: n.Bit40(v); break;
                case 41: n.Bit41(v); break;
                case 42: n.Bit42(v); break;
                case 43: n.Bit43(v); break;
                case 44: n.Bit44(v); break;
                case 45: n.Bit45(v); break;
                case 46: n.Bit46(v); break;
                case 47: n.Bit47(v); break;
                case 48: n.Bit48(v); break;
                case 49: n.Bit49(v); break;
                case 50: n.Bit50(v); break;
                case 51: n.Bit51(v); break;
                case 52: n.Bit52(v); break;
                case 53: n.Bit53(v); break;
                case 54: n.Bit54(v); break;
                case 55: n.Bit55(v); break;
                case 56: n.Bit56(v); break;
                case 57: n.Bit57(v); break;
                case 58: n.Bit58(v); break;
                case 59: n.Bit59(v); break;
                case 60: n.Bit60(v); break;
                case 61: n.Bit61(v); break;
                case 62: n.Bit62(v); break;
                case 63: n.Bit63(v); break;
            }
        }
        #endregion
        #region Byte(N)
        public static byte Byte0(this ulong n) { unchecked { return (byte)((n & 0x00000000000000FF)); } }
        public static byte Byte1(this ulong n) { unchecked { return (byte)((n & 0x000000000000FF00) >> 8); } }
        public static byte Byte2(this ulong n) { unchecked { return (byte)((n & 0x0000000000FF0000) >> 16); } }
        public static byte Byte3(this ulong n) { unchecked { return (byte)((n & 0x00000000FF000000) >> 24); } }
        public static byte Byte4(this ulong n) { unchecked { return (byte)((n & 0x000000FF00000000)); } }
        public static byte Byte5(this ulong n) { unchecked { return (byte)((n & 0x0000FF0000000000) >> 8); } }
        public static byte Byte6(this ulong n) { unchecked { return (byte)((n & 0x00FF000000000000) >> 16); } }
        public static byte Byte7(this ulong n) { unchecked { return (byte)((n & 0xFF00000000000000) >> 24); } }

        public static void Byte0(this ref ulong n, byte v) { unchecked { n = ((0xFFFFFFFFFFFFFF00 & n) + (ulong)(v)); } }
        public static void Byte1(this ref ulong n, byte v) { unchecked { n = ((0xFFFFFFFFFFFF00FF & n) + (ulong)(v << 8)); } }
        public static void Byte2(this ref ulong n, byte v) { unchecked { n = ((0xFFFFFFFFFF00FFFF & n) + (ulong)(v << 16)); } }
        public static void Byte3(this ref ulong n, byte v) { unchecked { n = ((0xFFFFFFFF00FFFFFF & n) + (ulong)(v << 24)); } }
        public static void Byte4(this ref ulong n, byte v) { unchecked { n = ((0xFFFFFF00FFFFFFFF & n) + (ulong)(v << 32)); } }
        public static void Byte5(this ref ulong n, byte v) { unchecked { n = ((0xFFFF00FFFFFFFFFF & n) + (ulong)(v << 40)); } }
        public static void Byte6(this ref ulong n, byte v) { unchecked { n = ((0xFF00FFFFFFFFFFFF & n) + (ulong)(v << 48)); } }
        public static void Byte7(this ref ulong n, byte v) { unchecked { n = ((0x00FFFFFFFFFFFFFF & n) + (ulong)(v << 56)); } }
        #endregion
        #region Word(N)
        public static ushort Word0(this ulong n) { unchecked { return (byte)((n & 0x000000000000FFFF)); } }
        public static ushort Word1(this ulong n) { unchecked { return (byte)((n & 0x00000000FFFF0000) >> 16); } }
        public static ushort Word2(this ulong n) { unchecked { return (byte)((n & 0x0000FFFF00000000)); } }
        public static ushort Word3(this ulong n) { unchecked { return (byte)((n & 0xFFFF000000000000) >> 16); } }

        public static void Word0(this ref ulong n, ushort v) { unchecked { n = ((0x00000000FFFF0000 & n) + (ulong)(v)); } }
        public static void Word1(this ref ulong n, ushort v) { unchecked { n = ((0xFFFFFFFF0000FFFF & n) + (ulong)(v << 16)); } }
        public static void Word2(this ref ulong n, ushort v) { unchecked { n = ((0xFFFF0000FFFFFFFF & n) + (ulong)(v << 32)); } }
        public static void Word3(this ref ulong n, ushort v) { unchecked { n = ((0x0000FFFFFFFFFFFF & n) + (ulong)(v << 48)); } }
        #endregion
        #region DWord(N)
        public static uint DWord0(this ulong n) { unchecked { return (byte)((n & 0x00000000FFFFFFFF)); } }
        public static uint DWord1(this ulong n) { unchecked { return (byte)((n & 0xFFFFFFFF00000000) >> 16); } }

        public static void DWord0(this ref ulong n, uint v) { unchecked { n = ((0xFFFFFFFF00000000 & n) + (v)); } }
        public static void DWord1(this ref ulong n, uint v) { unchecked { n = ((0x00000000FFFFFFFF & n) + (v << 32)); } }
        #endregion
        #region GetBytes / FromBytes
        public static byte[] GetBytes(this ulong n) { return BitConverter.GetBytes(n); }
        public static void FromBytes(this ref ulong n, byte[] data, int index = 0) { n = BitConverter.ToUInt64(data, index); }
        #endregion

        public static long Sign(this ulong n) { unchecked { return (long)n; } }
        #endregion

        #region object
        public static bool IsNumber(this object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }
        public static bool IsInteger(this object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong;
        }
        public static bool IsDecimal(this object value)
        {
            return value is float
                    || value is double
                    || value is decimal;
        }
        #endregion
    }
}
