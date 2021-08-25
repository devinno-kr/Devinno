using Devinno.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Devinno.Data
{
    #region interface : IMemories
    public interface IMemories
    {
        string Code { get; }
        byte[] RawData { get; }
        int Size { get; }
    }
    #endregion
    #region interface : IOBJ
    public interface IOBJ
    {
        MemoryKinds MemoryType { get; }
    }
    #endregion

    #region class : BitMemories
    public class BitMemories : IMemories
    {
        #region Properties
        public string Code { get; private set; }
        public byte[] RawData { get; private set; } = new byte[1024];
        public int Size => RawData.Length * 8;

        public bool this[int index]
        {
            get
            {
                bool ret = false;
                var Index = Convert.ToInt32(Math.Floor(index / 8.0));
                var BitIndex = index % 8;
                if (Index >= 0 && Index < RawData.Length)
                {
                    switch (BitIndex)
                    {
                        case 0: ret = RawData[Index].Bit0(); break;
                        case 1: ret = RawData[Index].Bit1(); break;
                        case 2: ret = RawData[Index].Bit2(); break;
                        case 3: ret = RawData[Index].Bit3(); break;
                        case 4: ret = RawData[Index].Bit4(); break;
                        case 5: ret = RawData[Index].Bit5(); break;
                        case 6: ret = RawData[Index].Bit6(); break;
                        case 7: ret = RawData[Index].Bit7(); break;
                    }
                }
                else throw new Exception("인덱스 범위 초과");
                return ret;
            }
            set
            {
                var Index = Convert.ToInt32(Math.Floor(index / 8.0));
                var BitIndex = index % 8;
                if (Index >= 0 && Index < RawData.Length)
                {
                    switch (BitIndex)
                    {
                        case 0: RawData[Index].Bit0(value); break;
                        case 1: RawData[Index].Bit1(value); break;
                        case 2: RawData[Index].Bit2(value); break;
                        case 3: RawData[Index].Bit3(value); break;
                        case 4: RawData[Index].Bit4(value); break;
                        case 5: RawData[Index].Bit5(value); break;
                        case 6: RawData[Index].Bit6(value); break;
                        case 7: RawData[Index].Bit7(value); break;
                    }
                }
                else throw new Exception("인덱스 범위 초과");
            }
        }

        #endregion

        #region Constructor
        public BitMemories(string Code, int Size)
        {
            if (Size < 1) throw new Exception("Size는 1이상");
            if (!Regex.IsMatch(Code.ToString(), "[a-zA-Z]", RegexOptions.IgnoreCase)) throw new Exception("Code는 알파벳으로");

            this.RawData = new byte[Convert.ToInt32(Math.Ceiling(Size / 8.0 / 2.0)) * 2];
            this.Code = Code.ToUpper();
        }

        public BitMemories(string Code, byte[] RawData)
        {
            if (RawData == null) throw new Exception("RawData는 null일 수 없음");
            if (!Regex.IsMatch(Code.ToString(), "[a-zA-Z]", RegexOptions.IgnoreCase)) throw new Exception("Code는 알파벳으로");

            this.RawData = RawData;
            this.Code = Code.ToUpper();
        }
        #endregion
    }
    #endregion
    #region class : WordMemories
    public class WordMemories : IMemories
    {
        #region Properties
        public string Code { get; private set; }
        public byte[] RawData { get; private set; } = new byte[1024];
        public int Size => RawData.Length / 2;

        public ushort this[int index]
        {
            get
            {
                if (index >= 0 && index < W.Length) return W[index].Value;
                else throw new Exception("인덱스 범위 초과");
            }
            set
            {
                if (index >= 0 && index < W.Length) W[index].Value = value;
                else throw new Exception("인덱스 범위 초과");
            }
        }

        public WORD[] W { get; private set; }
        #endregion

        #region Constructor
        public WordMemories(string Code, int Size)
        {
            if (Size < 1) throw new Exception("Size는 1이상");
            if (!Regex.IsMatch(Code.ToString(), "[a-zA-Z]", RegexOptions.IgnoreCase)) throw new Exception("Code는 알파벳으로");

            this.RawData = new byte[Size * 2];
            this.Code = Code.ToUpper();

            W = new WORD[Size];
            for (int i = 0; i < W.Length; i++) W[i] = new WORD(this, i * 2);
        }

        public WordMemories(string Code, byte[] RawData)
        {
            if (RawData == null) throw new Exception("RawData는 null일 수 없음");
            if (RawData.Length % 2 != 0) throw new Exception("RawData의 크기는 2의 배수이어야 함");
            if (!Regex.IsMatch(Code.ToString(), "[a-zA-Z]", RegexOptions.IgnoreCase)) throw new Exception("Code는 알파벳으로");

            this.RawData = RawData;
            this.Code = Code.ToUpper();

            W = new WORD[Size];
            for (int i = 0; i < W.Length; i++) W[i] = new WORD(this, i * 2);
        }
        #endregion
    }
    #endregion
    #region class : RealMemories
    public class RealMemories : IMemories
    {
        #region Properties
        public string Code { get; private set; }
        public byte[] RawData { get; private set; } = new byte[1024];
        public int Size => RawData.Length / 4;

        public float this[int index]
        {
            get
            {
                if (index >= 0 && index < R.Length) return R[index].Value;
                else throw new Exception("인덱스 범위 초과");
            }
            set
            {
                if (index >= 0 && index < R.Length) R[index].Value = value;
                else throw new Exception("인덱스 범위 초과");
            }
        }

        public REAL[] R { get; private set; }
        #endregion

        #region Constructor
        public RealMemories(string Code, int Size)
        {
            if (Size < 1) throw new Exception("Size는 1이상");
            if (!Regex.IsMatch(Code.ToString(), "[a-zA-Z]", RegexOptions.IgnoreCase)) throw new Exception("Code는 알파벳으로");

            this.RawData = new byte[Size * 4];
            this.Code = Code.ToUpper();

            R = new REAL[Size];
            for (int i = 0; i < R.Length; i++) R[i] = new REAL(this, i * 4);
        }

        public RealMemories(string Code, byte[] RawData)
        {
            if (RawData == null) throw new Exception("RawData는 null일 수 없음");
            if (RawData.Length % 2 != 0) throw new Exception("RawData의 크기는 4의 배수이어야 함");
            if (!Regex.IsMatch(Code.ToString(), "[a-zA-Z]", RegexOptions.IgnoreCase)) throw new Exception("Code는 알파벳으로");

            this.RawData = RawData;
            this.Code = Code.ToUpper();

            R = new REAL[Size];
            for (int i = 0; i < R.Length; i++) R[i] = new REAL(this, i * 4);
        }
        #endregion
    }
    #endregion

    #region Remark
    /*
    #region class : PlcMemories
    public class PlcMemories : IMemories
    {
        #region Properties
        public char Code { get; private set; }
        public byte[] RawData { get; private set; } = new byte[1024];
        public int Size => RawData.Length;

        public WORD[] W => _WORDS;
        public DWORD[] D => _DWORDS;
        public REAL[] R => _REALS;
        #endregion

        #region Indexer
        public BYTE this[int i] => _BYTES[i];
        public object this[string address]
        {
            #region get
            get
            {
                object ret = null;

                PlcAddress ad = null;
                try { ad = new PlcAddress(address); } catch (Exception e) { throw new Exception(e.Message); }
                if (ad != null)
                {
                    if (char.ToUpper(ad.Code) == char.ToUpper(this.Code))
                    {
                        switch (ad.Type)
                        {
                            #region MemoryKinds.BYTE
                            case MemoryKinds.BYTE:
                                {
                                    if (ad.Address < 0 || ad.Address >= _BYTES.Length) throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                    if (ad.Bit.HasValue && ad.Bit.Value >= 8) throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점범위 )");

                                    var v = _BYTES[ad.Address];
                                    if (v != null)
                                    {
                                        if (ad.Bit.HasValue)
                                        {
                                            switch (ad.Bit.Value)
                                            {
                                                case 0: ret = v.Bit0; break;
                                                case 1: ret = v.Bit1; break;
                                                case 2: ret = v.Bit2; break;
                                                case 3: ret = v.Bit3; break;
                                                case 4: ret = v.Bit4; break;
                                                case 5: ret = v.Bit5; break;
                                                case 6: ret = v.Bit6; break;
                                                case 7: ret = v.Bit7; break;

                                                default: throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점범위 )");
                                            }
                                        }
                                        else ret = v.Value;
                                    }
                                    else throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                }
                                break;
                            #endregion
                            #region  MemoryKinds.WORD
                            case MemoryKinds.WORD:
                                {
                                    if (ad.Address < 0 || ad.Address >= _WORDS.Length) throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                    if (ad.Bit.HasValue && ad.Bit.Value >= 16) throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점범위 )");

                                    var v = _WORDS[ad.Address];
                                    if (v != null)
                                    {
                                        if (ad.Bit.HasValue)
                                        {
                                            switch (ad.Bit.Value)
                                            {
                                                case 0: ret = v.Bit0; break;
                                                case 1: ret = v.Bit1; break;
                                                case 2: ret = v.Bit2; break;
                                                case 3: ret = v.Bit3; break;
                                                case 4: ret = v.Bit4; break;
                                                case 5: ret = v.Bit5; break;
                                                case 6: ret = v.Bit6; break;
                                                case 7: ret = v.Bit7; break;
                                                case 8: ret = v.Bit8; break;
                                                case 9: ret = v.Bit9; break;
                                                case 10: ret = v.Bit10; break;
                                                case 11: ret = v.Bit11; break;
                                                case 12: ret = v.Bit12; break;
                                                case 13: ret = v.Bit13; break;
                                                case 14: ret = v.Bit14; break;
                                                case 15: ret = v.Bit15; break;

                                                default: throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점범위 )");
                                            }
                                        }
                                        else ret = v.Value;
                                    }
                                    else throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                }
                                break;
                            #endregion
                            #region  MemoryKinds.DWORD
                            case MemoryKinds.DWORD:
                                {
                                    if (ad.Address < 0 || ad.Address >= _DWORDS.Length) throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                    if (ad.Bit.HasValue && ad.Bit.Value >= 32) throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점범위 )");

                                    var v = _DWORDS[ad.Address];
                                    if (v != null)
                                    {
                                        if (ad.Bit.HasValue)
                                        {
                                            switch (ad.Bit.Value)
                                            {
                                                case 0: ret = v.Bit0; break;
                                                case 1: ret = v.Bit1; break;
                                                case 2: ret = v.Bit2; break;
                                                case 3: ret = v.Bit3; break;
                                                case 4: ret = v.Bit4; break;
                                                case 5: ret = v.Bit5; break;
                                                case 6: ret = v.Bit6; break;
                                                case 7: ret = v.Bit7; break;
                                                case 8: ret = v.Bit8; break;
                                                case 9: ret = v.Bit9; break;
                                                case 10: ret = v.Bit10; break;
                                                case 11: ret = v.Bit11; break;
                                                case 12: ret = v.Bit12; break;
                                                case 13: ret = v.Bit13; break;
                                                case 14: ret = v.Bit14; break;
                                                case 15: ret = v.Bit15; break;
                                                case 16: ret = v.Bit16; break;
                                                case 17: ret = v.Bit17; break;
                                                case 18: ret = v.Bit18; break;
                                                case 19: ret = v.Bit19; break;
                                                case 20: ret = v.Bit20; break;
                                                case 21: ret = v.Bit21; break;
                                                case 22: ret = v.Bit22; break;
                                                case 23: ret = v.Bit23; break;
                                                case 24: ret = v.Bit24; break;
                                                case 25: ret = v.Bit25; break;
                                                case 26: ret = v.Bit26; break;
                                                case 27: ret = v.Bit27; break;
                                                case 28: ret = v.Bit28; break;
                                                case 29: ret = v.Bit29; break;
                                                case 30: ret = v.Bit30; break;
                                                case 31: ret = v.Bit31; break;

                                                default: throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점범위 )");
                                            }
                                        }
                                        else ret = v.Value;
                                    }
                                    else throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                }
                                break;
                            #endregion
                            #region  MemoryKinds.REAL
                            case MemoryKinds.REAL:
                                {
                                    if (ad.Address < 0 || ad.Address >= _REALS.Length) throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                    if (ad.Bit.HasValue) throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점사용불가 )");

                                    var v = _REALS[ad.Address];
                                    if (v != null)
                                    {
                                        if (ad.Bit.HasValue) throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점사용불가 )");
                                        else ret = v.Value;
                                    }
                                    else throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                }
                                break;
                                #endregion
                        }
                    }
                    else throw new Exception("유효하지 않은 주소 : " + address + " ( 영역코드 )");
                }

                return ret;
            }
            #endregion
            #region set
            set
            {
                if (value != null && (value is bool || value.IsNumber()))
                {
                    PlcAddress ad = null;
                    try { ad = new PlcAddress(address); } catch (Exception e) { throw new Exception(e.Message); }
                    if (ad != null)
                    {
                        if (char.ToUpper(ad.Code) == char.ToUpper(this.Code))
                        {
                            switch (ad.Type)
                            {
                                #region MemoryKinds.BYTE
                                case MemoryKinds.BYTE:
                                    {
                                        if (ad.Address < 0 || ad.Address >= Size) throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                        if (ad.Bit.HasValue && ad.Bit.Value >= 8) throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점범위 )");

                                        var v = _BYTES[ad.Address];
                                        if (v != null)
                                        {
                                            if (ad.Bit.HasValue)
                                            {
                                                var val = Convert.ToBoolean(value);
                                                switch (ad.Bit.Value)
                                                {
                                                    case 0: v.Bit0 = val; break;
                                                    case 1: v.Bit1 = val; break;
                                                    case 2: v.Bit2 = val; break;
                                                    case 3: v.Bit3 = val; break;
                                                    case 4: v.Bit4 = val; break;
                                                    case 5: v.Bit5 = val; break;
                                                    case 6: v.Bit6 = val; break;
                                                    case 7: v.Bit7 = val; break;

                                                    default: throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점범위 )");
                                                }
                                            }
                                            else
                                            {
                                                var val = Convert.ToByte(value);
                                                v.Value = val;
                                            }
                                        }
                                        else throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                    }
                                    break;
                                #endregion
                                #region  MemoryKinds.WORD
                                case MemoryKinds.WORD:
                                    {
                                        if (ad.Address < 0 || ad.Address >= Size - 1) throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                        if (ad.Bit.HasValue && ad.Bit.Value >= 16) throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점범위 )");

                                        var v = _WORDS[ad.Address];
                                        if (v != null)
                                        {
                                            if (ad.Bit.HasValue)
                                            {
                                                var val = Convert.ToBoolean(value);
                                                switch (ad.Bit.Value)
                                                {
                                                    case 0: v.Bit0 = val; break;
                                                    case 1: v.Bit1 = val; break;
                                                    case 2: v.Bit2 = val; break;
                                                    case 3: v.Bit3 = val; break;
                                                    case 4: v.Bit4 = val; break;
                                                    case 5: v.Bit5 = val; break;
                                                    case 6: v.Bit6 = val; break;
                                                    case 7: v.Bit7 = val; break;
                                                    case 8: v.Bit8 = val; break;
                                                    case 9: v.Bit9 = val; break;
                                                    case 10: v.Bit10 = val; break;
                                                    case 11: v.Bit11 = val; break;
                                                    case 12: v.Bit12 = val; break;
                                                    case 13: v.Bit13 = val; break;
                                                    case 14: v.Bit14 = val; break;
                                                    case 15: v.Bit15 = val; break;

                                                    default: throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점범위 )");
                                                }
                                            }
                                            else
                                            {
                                                var val = Convert.ToUInt16(value);
                                                v.Value = val;
                                            }
                                        }
                                        else throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                    }
                                    break;
                                #endregion
                                #region  MemoryKinds.DWORD
                                case MemoryKinds.DWORD:
                                    {
                                        if (ad.Address < 0 || ad.Address >= Size - 3) throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                        if (ad.Bit.HasValue && ad.Bit.Value >= 32) throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점범위 )");

                                        var v = _DWORDS[ad.Address];
                                        if (v != null)
                                        {
                                            if (ad.Bit.HasValue)
                                            {
                                                var val = Convert.ToBoolean(value);
                                                switch (ad.Bit.Value)
                                                {
                                                    case 0: v.Bit0 = val; break;
                                                    case 1: v.Bit1 = val; break;
                                                    case 2: v.Bit2 = val; break;
                                                    case 3: v.Bit3 = val; break;
                                                    case 4: v.Bit4 = val; break;
                                                    case 5: v.Bit5 = val; break;
                                                    case 6: v.Bit6 = val; break;
                                                    case 7: v.Bit7 = val; break;
                                                    case 8: v.Bit8 = val; break;
                                                    case 9: v.Bit9 = val; break;
                                                    case 10: v.Bit10 = val; break;
                                                    case 11: v.Bit11 = val; break;
                                                    case 12: v.Bit12 = val; break;
                                                    case 13: v.Bit13 = val; break;
                                                    case 14: v.Bit14 = val; break;
                                                    case 15: v.Bit15 = val; break;
                                                    case 16: v.Bit16 = val; break;
                                                    case 17: v.Bit17 = val; break;
                                                    case 18: v.Bit18 = val; break;
                                                    case 19: v.Bit19 = val; break;
                                                    case 20: v.Bit20 = val; break;
                                                    case 21: v.Bit21 = val; break;
                                                    case 22: v.Bit22 = val; break;
                                                    case 23: v.Bit23 = val; break;
                                                    case 24: v.Bit24 = val; break;
                                                    case 25: v.Bit25 = val; break;
                                                    case 26: v.Bit26 = val; break;
                                                    case 27: v.Bit27 = val; break;
                                                    case 28: v.Bit28 = val; break;
                                                    case 29: v.Bit29 = val; break;
                                                    case 30: v.Bit30 = val; break;
                                                    case 31: v.Bit31 = val; break;

                                                    default: throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점범위 )");
                                                }
                                            }
                                            else
                                            {
                                                var val = Convert.ToUInt32(value);
                                                v.Value = val;
                                            }
                                        }
                                        else throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                    }
                                    break;
                                #endregion
                                #region  MemoryKinds.REAL:
                                case MemoryKinds.REAL:
                                    {
                                        if (ad.Address < 0 || ad.Address >= Size - 3) throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                        if (ad.Bit.HasValue) throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점사용불가 )");

                                        var v = _REALS[ad.Address];
                                        if (v != null)
                                        {
                                            if (ad.Bit.HasValue) throw new Exception("유효하지 않은 주소 : " + address + " ( 소수점사용불가 )");
                                            else
                                            {
                                                var val = Convert.ToSingle(v);
                                                v.Value = val;
                                            }
                                        }
                                        else throw new Exception("유효하지 않은 주소 : " + address + " ( 주소범위 )");
                                    }
                                    break;
                                    #endregion
                            }
                        }
                        else throw new Exception("유효하지 않은 주소 : " + address + " ( 영역코드 )");
                    }
                }
                else throw new Exception("유효한 값은 bool 혹은 number");
            }
            #endregion
        }
        #endregion

        #region Member Variable
        private BYTE[] _BYTES;
        private WORD[] _WORDS;
        private DWORD[] _DWORDS;
        private REAL[] _REALS;
        #endregion

        #region Constructor
        public PlcMemories(char Code, int Size)
        {
            if (Size < 1) throw new Exception("Size는 1이상");
            if (!Regex.IsMatch(Code.ToString(), "[a-zA-Z]", RegexOptions.IgnoreCase)) throw new Exception("Code는 알파벳으로");

            this.RawData = new byte[Size];
            this.Code = char.ToUpper(Code);

            _BYTES = new BYTE[Size]; for (int i = 0; i < _BYTES.Length; i++) _BYTES[i] = new BYTE(this, i);
            _WORDS = new WORD[Size - 1]; for (int i = 0; i < _WORDS.Length; i++) _WORDS[i] = new WORD(this, i);
            _DWORDS = new DWORD[Size - 3]; for (int i = 0; i < _DWORDS.Length; i++) _DWORDS[i] = new DWORD(this, i);
            _REALS = new REAL[Size - 3]; for (int i = 0; i < _REALS.Length; i++) _REALS[i] = new REAL(this, i);
        }
        #endregion
    }
    #endregion
    #region class : PlcAddress
    public class PlcAddress
    {
        #region Properties
        public char Code { get; private set; }
        public MemoryKinds Type { get; private set; }
        public int Address { get; private set; }
        public int? Bit { get; private set; }
        #endregion

        #region Contructor
        public PlcAddress(string address)
        {
            if (!string.IsNullOrWhiteSpace(address) && address.Length >= 2 && address.Where(x => x == '.').Count() <= 1)
            {
                int n;

                var ss = address.Split('.');
                var body = ss.FirstOrDefault();
                var bits = ss.Length > 1 ? ss.LastOrDefault() : null;

                this.Code = char.ToUpper(body[0]);
                this.Bit = (bits != null && int.TryParse(bits, out n) ? n : (int?)null);

                if (char.IsDigit(body[1]))
                {
                    this.Address = int.TryParse(body.Substring(1), out n) ? n : 0;
                    this.Type = MemoryKinds.BYTE;
                }
                else
                {
                    this.Address = (int.TryParse(body.Substring(2), out n) ? n : 0);
                    switch (char.ToUpper(body[1]))
                    {
                        case 'W': this.Type = MemoryKinds.WORD; break;
                        case 'D': this.Type = MemoryKinds.DWORD; break;
                        case 'R': this.Type = MemoryKinds.REAL; break;
                        default: throw new Exception("유효하지 않은 주소 : " + address + "( 미지정타입 )");
                    }
                }
            }
            else throw new Exception("유효하지 않은 주소 : " + address + "( 틀린유형 )");
        }
        #endregion
    }
    #endregion
    */
    #endregion

    #region class : BYTE
    public class BYTE : IOBJ
    {
        #region Common Properties
        public IMemories Memories { get; set; }
        public int Index { get; set; }
        public MemoryKinds MemoryType => MemoryKinds.BYTE;
        #endregion
        #region Properties
        public byte Value { get => Memories.RawData[Index]; set => Memories.RawData[Index] = value; }
        public sbyte Sign { get => unchecked((sbyte)Value); set => Value = unchecked((byte)value); }

        public bool Bit0 { get => Memories.RawData[Index].Bit0(); set => Memories.RawData[Index].Bit0(value); }
        public bool Bit1 { get => Memories.RawData[Index].Bit1(); set => Memories.RawData[Index].Bit1(value); }
        public bool Bit2 { get => Memories.RawData[Index].Bit2(); set => Memories.RawData[Index].Bit2(value); }
        public bool Bit3 { get => Memories.RawData[Index].Bit3(); set => Memories.RawData[Index].Bit3(value); }
        public bool Bit4 { get => Memories.RawData[Index].Bit4(); set => Memories.RawData[Index].Bit4(value); }
        public bool Bit5 { get => Memories.RawData[Index].Bit5(); set => Memories.RawData[Index].Bit5(value); }
        public bool Bit6 { get => Memories.RawData[Index].Bit6(); set => Memories.RawData[Index].Bit6(value); }
        public bool Bit7 { get => Memories.RawData[Index].Bit7(); set => Memories.RawData[Index].Bit7(value); }
        #endregion

        public BYTE(IMemories mem, int idx) { this.Memories = mem; this.Index = idx; }
    }
    #endregion
    #region class : WORD
    public class WORD
    {
        #region Common Properties
        public IMemories Memories { get; set; }
        public int Index { get; set; }
        public MemoryKinds MemoryType => MemoryKinds.WORD;
        #endregion
        #region Properties
        public ushort Value
        {
            get => unchecked((ushort)(Byte1 << 8 | Byte0));
            set
            {
                Byte1 = (byte)((value & 0xFF00) >> 8);
                Byte0 = (byte)(value & 0x00FF);
            }
        }

        public short Sign { get => unchecked((short)Value); set => Value = unchecked((ushort)value); }

        public byte Byte0 { get => Memories.RawData[Index + 0]; set => Memories.RawData[Index + 0] = value; }
        public byte Byte1 { get => Memories.RawData[Index + 1]; set => Memories.RawData[Index + 1] = value; }

        public bool Bit0 { get => Memories.RawData[Index + 0].Bit0(); set => Memories.RawData[Index + 0].Bit0(value); }
        public bool Bit1 { get => Memories.RawData[Index + 0].Bit1(); set => Memories.RawData[Index + 0].Bit1(value); }
        public bool Bit2 { get => Memories.RawData[Index + 0].Bit2(); set => Memories.RawData[Index + 0].Bit2(value); }
        public bool Bit3 { get => Memories.RawData[Index + 0].Bit3(); set => Memories.RawData[Index + 0].Bit3(value); }
        public bool Bit4 { get => Memories.RawData[Index + 0].Bit4(); set => Memories.RawData[Index + 0].Bit4(value); }
        public bool Bit5 { get => Memories.RawData[Index + 0].Bit5(); set => Memories.RawData[Index + 0].Bit5(value); }
        public bool Bit6 { get => Memories.RawData[Index + 0].Bit6(); set => Memories.RawData[Index + 0].Bit6(value); }
        public bool Bit7 { get => Memories.RawData[Index + 0].Bit7(); set => Memories.RawData[Index + 0].Bit7(value); }
        public bool Bit8 { get => Memories.RawData[Index + 1].Bit0(); set => Memories.RawData[Index + 1].Bit0(value); }
        public bool Bit9 { get => Memories.RawData[Index + 1].Bit1(); set => Memories.RawData[Index + 1].Bit1(value); }
        public bool Bit10 { get => Memories.RawData[Index + 1].Bit2(); set => Memories.RawData[Index + 1].Bit2(value); }
        public bool Bit11 { get => Memories.RawData[Index + 1].Bit3(); set => Memories.RawData[Index + 1].Bit3(value); }
        public bool Bit12 { get => Memories.RawData[Index + 1].Bit4(); set => Memories.RawData[Index + 1].Bit4(value); }
        public bool Bit13 { get => Memories.RawData[Index + 1].Bit5(); set => Memories.RawData[Index + 1].Bit5(value); }
        public bool Bit14 { get => Memories.RawData[Index + 1].Bit6(); set => Memories.RawData[Index + 1].Bit6(value); }
        public bool Bit15 { get => Memories.RawData[Index + 1].Bit7(); set => Memories.RawData[Index + 1].Bit7(value); }
        #endregion

        public WORD(IMemories mem, int idx) { this.Memories = mem; this.Index = idx; }
    }
    #endregion
    #region class : DWORD
    public class DWORD
    {
        #region Common Properties
        public IMemories Memories { get; set; }
        public int Index { get; set; }
        public MemoryKinds MemoryType => MemoryKinds.DWORD;
        #endregion
        #region Properties
        public uint Value
        {
            get => unchecked((uint)(Byte3 << 24 | Byte2 << 16 | Byte1 << 8 | Byte0));
            set
            {
                Byte3 = (byte)((value & 0xFF000000) >> 24);
                Byte2 = (byte)((value & 0x00FF0000) >> 16);
                Byte1 = (byte)((value & 0x0000FF00) >> 8);
                Byte0 = (byte)((value & 0x000000FF));
            }
        }

        public int Sign { get => unchecked((int)Value); set => Value = unchecked((uint)value); }

        public byte Byte0 { get => Memories.RawData[Index + 0]; set => Memories.RawData[Index + 0] = value; }
        public byte Byte1 { get => Memories.RawData[Index + 1]; set => Memories.RawData[Index + 1] = value; }
        public byte Byte2 { get => Memories.RawData[Index + 2]; set => Memories.RawData[Index + 2] = value; }
        public byte Byte3 { get => Memories.RawData[Index + 3]; set => Memories.RawData[Index + 3] = value; }

        public ushort Word0
        {
            get => unchecked((ushort)(Byte0 << 8 | Byte1));
            set
            {
                Byte1 = (byte)((value & 0xFF00) >> 8);
                Byte0 = (byte)(value & 0x00FF);
            }
        }
        public ushort Word1
        {
            get => unchecked((ushort)(Byte2 << 8 | Byte3));
            set
            {
                Byte3 = (byte)((value & 0xFF00) >> 8);
                Byte2 = (byte)(value & 0x00FF);
            }
        }

        public bool Bit0 { get => Memories.RawData[Index + 0].Bit0(); set => Memories.RawData[Index + 0].Bit0(value); }
        public bool Bit1 { get => Memories.RawData[Index + 0].Bit1(); set => Memories.RawData[Index + 0].Bit1(value); }
        public bool Bit2 { get => Memories.RawData[Index + 0].Bit2(); set => Memories.RawData[Index + 0].Bit2(value); }
        public bool Bit3 { get => Memories.RawData[Index + 0].Bit3(); set => Memories.RawData[Index + 0].Bit3(value); }
        public bool Bit4 { get => Memories.RawData[Index + 0].Bit4(); set => Memories.RawData[Index + 0].Bit4(value); }
        public bool Bit5 { get => Memories.RawData[Index + 0].Bit5(); set => Memories.RawData[Index + 0].Bit5(value); }
        public bool Bit6 { get => Memories.RawData[Index + 0].Bit6(); set => Memories.RawData[Index + 0].Bit6(value); }
        public bool Bit7 { get => Memories.RawData[Index + 0].Bit7(); set => Memories.RawData[Index + 0].Bit7(value); }
        public bool Bit8 { get => Memories.RawData[Index + 1].Bit0(); set => Memories.RawData[Index + 1].Bit0(value); }
        public bool Bit9 { get => Memories.RawData[Index + 1].Bit1(); set => Memories.RawData[Index + 1].Bit1(value); }
        public bool Bit10 { get => Memories.RawData[Index + 1].Bit2(); set => Memories.RawData[Index + 1].Bit2(value); }
        public bool Bit11 { get => Memories.RawData[Index + 1].Bit3(); set => Memories.RawData[Index + 1].Bit3(value); }
        public bool Bit12 { get => Memories.RawData[Index + 1].Bit4(); set => Memories.RawData[Index + 1].Bit4(value); }
        public bool Bit13 { get => Memories.RawData[Index + 1].Bit5(); set => Memories.RawData[Index + 1].Bit5(value); }
        public bool Bit14 { get => Memories.RawData[Index + 1].Bit6(); set => Memories.RawData[Index + 1].Bit6(value); }
        public bool Bit15 { get => Memories.RawData[Index + 1].Bit7(); set => Memories.RawData[Index + 1].Bit7(value); }
        public bool Bit16 { get => Memories.RawData[Index + 2].Bit0(); set => Memories.RawData[Index + 2].Bit0(value); }
        public bool Bit17 { get => Memories.RawData[Index + 2].Bit1(); set => Memories.RawData[Index + 2].Bit1(value); }
        public bool Bit18 { get => Memories.RawData[Index + 2].Bit2(); set => Memories.RawData[Index + 2].Bit2(value); }
        public bool Bit19 { get => Memories.RawData[Index + 2].Bit3(); set => Memories.RawData[Index + 2].Bit3(value); }
        public bool Bit20 { get => Memories.RawData[Index + 2].Bit4(); set => Memories.RawData[Index + 2].Bit4(value); }
        public bool Bit21 { get => Memories.RawData[Index + 2].Bit5(); set => Memories.RawData[Index + 2].Bit5(value); }
        public bool Bit22 { get => Memories.RawData[Index + 2].Bit6(); set => Memories.RawData[Index + 2].Bit6(value); }
        public bool Bit23 { get => Memories.RawData[Index + 2].Bit7(); set => Memories.RawData[Index + 2].Bit7(value); }
        public bool Bit24 { get => Memories.RawData[Index + 3].Bit0(); set => Memories.RawData[Index + 3].Bit0(value); }
        public bool Bit25 { get => Memories.RawData[Index + 3].Bit1(); set => Memories.RawData[Index + 3].Bit1(value); }
        public bool Bit26 { get => Memories.RawData[Index + 3].Bit2(); set => Memories.RawData[Index + 3].Bit2(value); }
        public bool Bit27 { get => Memories.RawData[Index + 3].Bit3(); set => Memories.RawData[Index + 3].Bit3(value); }
        public bool Bit28 { get => Memories.RawData[Index + 3].Bit4(); set => Memories.RawData[Index + 3].Bit4(value); }
        public bool Bit29 { get => Memories.RawData[Index + 3].Bit5(); set => Memories.RawData[Index + 3].Bit5(value); }
        public bool Bit30 { get => Memories.RawData[Index + 3].Bit6(); set => Memories.RawData[Index + 3].Bit6(value); }
        public bool Bit31 { get => Memories.RawData[Index + 3].Bit7(); set => Memories.RawData[Index + 3].Bit7(value); }
        #endregion

        public DWORD(IMemories mem, int idx) { this.Memories = mem; this.Index = idx; }
    }
    #endregion
    #region class : REAL
    public class REAL
    {
        #region Common Properties
        public IMemories Memories { get; set; }
        public int Index { get; set; }
        public MemoryKinds MemoryType => MemoryKinds.REAL;
        #endregion
        #region Properties
        public float Value
        {
            get => BitConverter.ToSingle(Memories.RawData, Index);
            set
            {
                var ba = BitConverter.GetBytes(value);
                Array.Copy(ba, 0, Memories.RawData, Index, ba.Length);
            }
        }
        #endregion

        public REAL(IMemories mem, int idx) { this.Memories = mem; this.Index = idx; }
    }
    #endregion

    #region enum : MemoryKinds
    public enum MemoryKinds { BYTE, WORD, DWORD, REAL }
    #endregion

    #region Remark
    /*
    public class BYTE : CustomValueType<BYTE, Byte>
    {
        private BYTE(byte value) : base(value) { }
        public static implicit operator BYTE(byte value) { return new BYTE(value); }
        public static implicit operator byte(BYTE custom) { return custom._value; }

        public string ToString(string format) => _value.ToString(format);
    }

    public class CustomValueType<TCustom, TValue>
    {
        protected readonly TValue _value;

        public CustomValueType(TValue value) => _value = value;

        public override string ToString() => _value.ToString();
        public static bool operator <(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b) => Comparer<TValue>.Default.Compare(a._value, b._value) < 0;
        public static bool operator >(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b) => !(a < b);
        public static bool operator <=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b) => (a < b) || (a == b);
        public static bool operator >=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b) => (a > b) || (a == b);
        public static bool operator ==(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b) => a.Equals((object)b);
        public static bool operator !=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b) => !(a == b);
        public static TCustom operator +(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b) => (dynamic)a._value + b._value;
        public static TCustom operator -(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b) => ((dynamic)a._value - b._value);

        protected bool Equals(CustomValueType<TCustom, TValue> other) => EqualityComparer<TValue>.Default.Equals(_value, other._value);
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CustomValueType<TCustom, TValue>)obj);
        }
        public override int GetHashCode() => EqualityComparer<TValue>.Default.GetHashCode(_value);
    }
    */
    #endregion
}
