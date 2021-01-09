using Devinno.Communications.Scheduler;
using Devinno.Communications.Setting;
using Devinno.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Communications.Modbus.RTU
{
    public class ModbusRTUSlaveBase : SlaveScheduler
    {
        #region [class] EventArgs
        #region BitReadRequestArgs
        public class BitReadRequestArgs : EventArgs
        {
            byte[] Data;
            public int Slave { get { return Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)Data[1]; } }
            public int StartAddress { get { return (Data[2] << 8) | Data[3]; } }
            public int Length { get { return (Data[4] << 8) | Data[5]; } }
            public bool Success { get; set; }

            public bool[] ResponseData { get; set; }

            public BitReadRequestArgs(byte[] Data) { this.Data = Data; }
        }
        #endregion
        #region WordReadRequestArgs
        public class WordReadRequestArgs : EventArgs
        {
            byte[] Data;
            public int Slave { get { return Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)Data[1]; } }
            public int StartAddress { get { return (Data[2] << 8) | Data[3]; } }
            public int Length { get { return (Data[4] << 8) | Data[5]; } }
            public bool Success { get; set; }

            public int[] ResponseData { get; set; }

            public WordReadRequestArgs(byte[] Data) { this.Data = Data; }
        }
        #endregion
        #region BitWriteRequestArgs
        public class BitWriteRequestArgs : EventArgs
        {
            byte[] Data;
            public int Slave { get { return Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)Data[1]; } }
            public int StartAddress { get { return (Data[2] << 8) | Data[3]; } }
            public bool WriteValue { get { return ((Data[4] << 8) | Data[5]) == 0xFF00; } }
            public bool Success { get; set; }

            public BitWriteRequestArgs(byte[] Data) { this.Data = Data; }
        }
        #endregion
        #region WordWriteRequestArgs
        public class WordWriteRequestArgs : EventArgs
        {
            byte[] Data;
            public int Slave { get { return Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)Data[1]; } }
            public int StartAddress { get { return (Data[2] << 8) | Data[3]; } }
            public ushort WriteValue { get { return Convert.ToUInt16((Data[4] << 8) | Data[5]); } }
            public bool Success { get; set; }

            public WordWriteRequestArgs(byte[] Data) { this.Data = Data; }
        }
        #endregion
        #region MultiBitWriteRequestArgs
        public class MultiBitWriteRequestArgs : EventArgs
        {
            byte[] Data;
            public int Slave { get { return Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)Data[1]; } }
            public int StartAddress { get { return (Data[2] << 8) | Data[3]; } }
            public int Length { get { return ((Data[4] << 8) | Data[5]); } }
            public bool[] WriteValues { get; private set; }
            public bool Success { get; set; }

            public MultiBitWriteRequestArgs(byte[] Data)
            {
                this.Data = Data;
                #region WriteValues
                List<bool> ret = new List<bool>();
                for (int i = 7; i < Data.Length - 2; i++)
                    for (int j = 0; j < 8; j++)
                        if (ret.Count < Length) ret.Add(Data[i].Bit(j));
                WriteValues = ret.ToArray();
                #endregion
            }
        }
        #endregion
        #region MultiWordWriteRequestArgs
        public class MultiWordWriteRequestArgs : EventArgs
        {
            byte[] Data;
            public int Slave { get { return Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)Data[1]; } }
            public int StartAddress { get { return (Data[2] << 8) | Data[3]; } }
            public int Length { get { return ((Data[4] << 8) | Data[5]); } }
            public ushort[] WriteValues { get; private set; }
            public bool Success { get; set; }

            public MultiWordWriteRequestArgs(byte[] Data)
            {
                this.Data = Data;
                #region WriteValues
                List<ushort> ret = new List<ushort>();
                for (int i = 7; i < Data.Length - 2; i += 2)
                {
                    ret.Add(Convert.ToUInt16((Data[i] << 8) | Data[i + 1]));
                }
                WriteValues = ret.ToArray();
                #endregion
            }
        }
        #endregion
        #region WordBitSetRequestArgs
        public class WordBitSetRequestArgs : EventArgs
        {
            byte[] Data;
            public int Slave { get { return Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)Data[1]; } }
            public int StartAddress { get { return (Data[2] << 8) | Data[3]; } }
            public int BitIndex { get { return Data[4]; } }
            public bool WriteValue { get { return ((Data[5] << 8) | Data[6]) == 0xFF00; } }
            public bool Success { get; set; }

            public WordBitSetRequestArgs(byte[] Data)
            {
                this.Data = Data;
            }
        }
        #endregion
        #endregion

        #region Member Variable
        private SerialPort ser = new SerialPort() { PortName = "COM1", BaudRate = 115200 };
        #endregion

        #region Properties
        public int Baudrate { get => ser.BaudRate; set => ser.BaudRate = value; }
        public string Port { get => ser.PortName; set => ser.PortName = value; }
        protected override int Available
        {
            get
            {
                try
                {
                    return ser.BytesToRead;
                }
                catch (IOException) { throw new SchedulerStopException(); }
                catch (UnauthorizedAccessException) { throw new SchedulerStopException(); }
                catch (InvalidOperationException) { throw new SchedulerStopException(); }
            }
        }
        public override bool IsOpen => ser.IsOpen;
        #endregion

        #region Event
        public event EventHandler<BitReadRequestArgs> BitReadRequest;
        public event EventHandler<WordReadRequestArgs> WordReadRequest;
        public event EventHandler<BitWriteRequestArgs> BitWriteRequest;
        public event EventHandler<WordWriteRequestArgs> WordWriteRequest;
        public event EventHandler<MultiBitWriteRequestArgs> MultiBitWriteRequest;
        public event EventHandler<MultiWordWriteRequestArgs> MultiWordWriteRequest;
        public event EventHandler<WordBitSetRequestArgs> WordBitSetRequest;

        public event EventHandler DeviceOpened;
        public event EventHandler DeviceClosed;
        #endregion

        #region Constructor
        public ModbusRTUSlaveBase()
        {
        }
        #endregion

        #region Method
        #region Start
        public override bool Start()
        {
            bool ret = false;
            if (!IsOpen && !IsStart)
            {
                try
                {
                    ser.Open();
                    DeviceOpened?.Invoke(this, null);

                    ret = StartThread();
                    if (!ret && IsOpen) ser.Close();
                }
                catch (Exception) { }
            }
            return ret;
        }
        public bool Start(SerialPortSetting data)
        {
            ser.PortName = data.Port;
            ser.BaudRate = data.Baudrate;
            ser.Parity = data.Parity;
            ser.DataBits = data.DataBit;
            ser.StopBits = data.StopBit;
            return Start();
        }
        #endregion
        #region Stop
        public override void Stop()
        {
            StopThread();
        }
        #endregion
        #endregion

        #region Override
        #region OnWrite
        protected override void OnWrite(byte[] data, int offset, int count, int timeout)
        {
            try
            {
                ser.Write(data, offset, count);
            }
            catch (IOException) { throw new SchedulerStopException(); }
            catch (UnauthorizedAccessException) { throw new SchedulerStopException(); }
            catch (InvalidOperationException) { throw new SchedulerStopException(); }
        }
        #endregion
        #region OnRead
        protected override int? OnRead(byte[] data, int offset, int count, int timeout)
        {
            try
            {
                return ser.Read(data, offset, count);
            }
            catch (IOException) { throw new SchedulerStopException(); }
            catch (UnauthorizedAccessException) { throw new SchedulerStopException(); }
            catch (InvalidOperationException) { throw new SchedulerStopException(); }
            catch { return null; }
        }
        #endregion
        #region OnClearBuffer
        protected override void OnClearBuffer()
        {
            try
            {
                ser.DiscardInBuffer();
                ser.BaseStream.Flush();
            }
            catch (IOException) { throw new SchedulerStopException(); }
            catch (UnauthorizedAccessException) { throw new SchedulerStopException(); }
            catch (InvalidOperationException) { throw new SchedulerStopException(); }
        }
        #endregion
        #region OnThreadEnd
        protected override void OnThreadEnd()
        {
            if (IsOpen) ser.Close();
            DeviceClosed?.Invoke(this, null);
        }
        #endregion
        #region OnParsePacket
        protected override bool OnParsePacket(List<byte> lstResponse)
        {
            bool retstate = false;
            if (lstResponse.Count >= 4)
            {
                int Slave = lstResponse[0];
                ModbusFunction Function = (ModbusFunction)lstResponse[1];
                int StartAddress = (lstResponse[2] << 8) | lstResponse[3];

                switch (Function)
                {
                    case ModbusFunction.BITREAD_F1:
                    case ModbusFunction.BITREAD_F2:
                        #region BitRead
                        if (lstResponse.Count == 8)
                        {
                            byte hi = 0xFF, lo = 0xFF;
                            ModbusCRC.GetCRC(lstResponse, 0, 6, ref hi, ref lo);
                            if (lstResponse[6] == hi && lstResponse[7] == lo)
                            {
                                if (BitReadRequest != null)
                                {
                                    var args = new BitReadRequestArgs(lstResponse.ToArray());
                                    BitReadRequest.Invoke(this, args);

                                    if (args.Success && args.ResponseData != null && args.ResponseData.Length == args.Length)
                                    {
                                        #region MakeData
                                        List<byte> Datas = new List<byte>();
                                        int nlen = args.ResponseData.Length / 8;
                                        nlen += (args.ResponseData.Length % 8 == 0) ? 0 : 1;
                                        for (int i = 0; i < nlen; i++)
                                        {
                                            byte val = 0;
                                            for (int j = (i * 8), nTemp = 0; j < args.ResponseData.Length && j < (i * 8) + 8; j++, nTemp++)
                                                if (args.ResponseData[j])
                                                    val |= Convert.ToByte(Math.Pow(2, nTemp));
                                            Datas.Add(val);
                                        }
                                        #endregion
                                        #region Serial Write
                                        List<byte> ret = new List<byte>();
                                        ret.Add((byte)Slave);
                                        ret.Add((byte)Function);
                                        ret.Add((byte)Datas.Count);
                                        ret.AddRange(Datas.ToArray());
                                        byte nhi = 0xFF, nlo = 0xFF;
                                        ModbusCRC.GetCRC(ret, 0, ret.Count, ref nhi, ref nlo);
                                        ret.Add(nhi);
                                        ret.Add(nlo);
                                        byte[] send = ret.ToArray();
                                        ser.Write(send, 0, send.Length);
                                        ser.BaseStream.Flush();
                                        #endregion
                                        retstate = true;
                                    }
                                }
                                lstResponse.Clear();
                            }
                        }
                        #endregion
                        break;
                    case ModbusFunction.WORDREAD_F3:
                    case ModbusFunction.WORDREAD_F4:
                        #region WordRead
                        if (lstResponse.Count == 8)
                        {
                            byte hi = 0xFF, lo = 0xFF;
                            ModbusCRC.GetCRC(lstResponse, 0, 6, ref hi, ref lo);
                            if (lstResponse[6] == hi && lstResponse[7] == lo)
                            {
                                if (WordReadRequest != null)
                                {
                                    var args = new WordReadRequestArgs(lstResponse.ToArray());
                                    WordReadRequest.Invoke(this, args);

                                    if (args.Success && args.ResponseData != null && args.ResponseData.Length == args.Length)
                                    {
                                        #region MakeData
                                        List<byte> Datas = new List<byte>();
                                        for (int i = 0; i < args.ResponseData.Length; i++)
                                        {
                                            Datas.Add((byte)((args.ResponseData[i] & 0xFF00) >> 8));
                                            Datas.Add((byte)((args.ResponseData[i] & 0x00FF)));
                                        }
                                        #endregion
                                        #region Serial Write
                                        List<byte> ret = new List<byte>();
                                        ret.Add((byte)Slave);
                                        ret.Add((byte)Function);
                                        ret.Add((byte)Datas.Count);
                                        ret.AddRange(Datas.ToArray());
                                        byte nhi = 0xFF, nlo = 0xFF;
                                        ModbusCRC.GetCRC(ret, 0, ret.Count, ref nhi, ref nlo);
                                        ret.Add(nhi);
                                        ret.Add(nlo);
                                        byte[] send = ret.ToArray();
                                        ser.Write(send, 0, send.Length);
                                        ser.BaseStream.Flush();
                                        #endregion
                                    }
                                }
                                retstate = true;
                            }
                        }
                        #endregion
                        break;
                    case ModbusFunction.BITWRITE_F5:
                        #region BitWrite
                        if (lstResponse.Count == 8)
                        {
                            byte hi = 0xFF, lo = 0xFF;
                            ModbusCRC.GetCRC(lstResponse, 0, 6, ref hi, ref lo);
                            if (lstResponse[6] == hi && lstResponse[7] == lo)
                            {
                                if (BitWriteRequest != null)
                                {
                                    var args = new BitWriteRequestArgs(lstResponse.ToArray());
                                    BitWriteRequest.Invoke(this, args);

                                    if (args.Success)
                                    {
                                        #region Serial Write
                                        int nv = args.WriteValue ? 0xFF00 : 0;
                                        List<byte> ret = new List<byte>();
                                        ret.Add((byte)args.Slave);
                                        ret.Add((byte)args.Function);
                                        ret.Add((byte)((args.StartAddress & 0xFF00) >> 8));
                                        ret.Add((byte)((args.StartAddress & 0x00FF)));
                                        ret.Add((byte)((nv & 0xFF00) >> 8));
                                        ret.Add((byte)((nv & 0x00FF)));
                                        byte nhi = 0xFF, nlo = 0xFF;
                                        ModbusCRC.GetCRC(ret, 0, ret.Count, ref nhi, ref nlo);
                                        ret.Add(nhi);
                                        ret.Add(nlo);
                                        byte[] send = ret.ToArray();
                                        ser.Write(send, 0, send.Length);
                                        ser.BaseStream.Flush();
                                        #endregion
                                    }
                                }
                                retstate = true;
                            }
                        }
                        #endregion
                        break;
                    case ModbusFunction.WORDWRITE_F6:
                        #region WordWrite
                        if (lstResponse.Count == 8)
                        {
                            byte hi = 0xFF, lo = 0xFF;
                            ModbusCRC.GetCRC(lstResponse, 0, 6, ref hi, ref lo);
                            if (lstResponse[6] == hi && lstResponse[7] == lo)
                            {
                                if (WordWriteRequest != null)
                                {
                                    var args = new WordWriteRequestArgs(lstResponse.ToArray());
                                    WordWriteRequest.Invoke(this, args);

                                    if (args.Success)
                                    {
                                        #region Serial Write
                                        List<byte> ret = new List<byte>();
                                        ret.Add((byte)args.Slave);
                                        ret.Add((byte)args.Function);
                                        ret.Add((byte)((args.StartAddress & 0xFF00) >> 8));
                                        ret.Add((byte)((args.StartAddress & 0x00FF)));
                                        ret.Add((byte)((args.WriteValue & 0xFF00) >> 8));
                                        ret.Add((byte)((args.WriteValue & 0x00FF)));
                                        byte nhi = 0xFF, nlo = 0xFF;
                                        ModbusCRC.GetCRC(ret, 0, ret.Count, ref nhi, ref nlo);
                                        ret.Add(nhi);
                                        ret.Add(nlo);
                                        byte[] send = ret.ToArray();
                                        ser.Write(send, 0, send.Length);
                                        ser.BaseStream.Flush();
                                        #endregion
                                    }
                                }
                                retstate = true;
                            }
                        }
                        #endregion
                        break;
                    case ModbusFunction.MULTIBITWRITE_F15:
                        #region MultiBitWrite
                        if (lstResponse.Count >= 7)
                        {
                            int Length = (lstResponse[4] << 8) | lstResponse[5];
                            int ByteCount = lstResponse[6];
                            if (lstResponse.Count >= 9 + ByteCount)
                            {
                                byte hi = 0xFF, lo = 0xFF;
                                ModbusCRC.GetCRC(lstResponse, 0, 7 + ByteCount, ref hi, ref lo);
                                if (lstResponse[9 + ByteCount - 2] == hi && lstResponse[9 + ByteCount - 1] == lo)
                                {
                                    var args = new MultiBitWriteRequestArgs(lstResponse.ToArray());
                                    if (MultiBitWriteRequest != null)
                                    {
                                        MultiBitWriteRequest.Invoke(this, args);

                                        if (args.Success)
                                        {
                                            #region Serial Write
                                            List<byte> ret = new List<byte>();
                                            ret.Add((byte)args.Slave);
                                            ret.Add((byte)args.Function);
                                            ret.Add((byte)((args.StartAddress & 0xFF00) >> 8));
                                            ret.Add((byte)((args.StartAddress & 0x00FF)));
                                            ret.Add((byte)((args.Length & 0xFF00) >> 8));
                                            ret.Add((byte)((args.Length & 0x00FF)));
                                            byte nhi = 0xFF, nlo = 0xFF;
                                            ModbusCRC.GetCRC(ret, 0, ret.Count, ref nhi, ref nlo);
                                            ret.Add(nhi);
                                            ret.Add(nlo);
                                            byte[] send = ret.ToArray();
                                            ser.Write(send, 0, send.Length);
                                            ser.BaseStream.Flush();
                                            #endregion
                                        }
                                    }
                                    retstate = true;
                                }
                            }
                        }
                        #endregion
                        break;
                    case ModbusFunction.MULTIWORDWRITE_F16:
                        #region MultiWordWrite
                        if (lstResponse.Count >= 7)
                        {
                            int Length = (lstResponse[4] << 8) | lstResponse[5];
                            int ByteCount = lstResponse[6];
                            if (lstResponse.Count >= 9 + ByteCount)
                            {
                                byte hi = 0xFF, lo = 0xFF;
                                ModbusCRC.GetCRC(lstResponse, 0, 7 + ByteCount, ref hi, ref lo);
                                if (lstResponse[9 + ByteCount - 2] == hi && lstResponse[9 + ByteCount - 1] == lo)
                                {
                                    if (MultiWordWriteRequest != null)
                                    {
                                        var args = new MultiWordWriteRequestArgs(lstResponse.ToArray());
                                        MultiWordWriteRequest.Invoke(this, args);

                                        if (args.Success)
                                        {
                                            #region Serial Write
                                            List<byte> ret = new List<byte>();
                                            ret.Add((byte)args.Slave);
                                            ret.Add((byte)args.Function);
                                            ret.Add((byte)((args.StartAddress & 0xFF00) >> 8));
                                            ret.Add((byte)((args.StartAddress & 0x00FF)));
                                            ret.Add((byte)((args.Length & 0xFF00) >> 8));
                                            ret.Add((byte)((args.Length & 0x00FF)));
                                            byte nhi = 0xFF, nlo = 0xFF;
                                            ModbusCRC.GetCRC(ret, 0, ret.Count, ref nhi, ref nlo);
                                            ret.Add(nhi);
                                            ret.Add(nlo);
                                            byte[] send = ret.ToArray();
                                            ser.Write(send, 0, send.Length);
                                            ser.BaseStream.Flush();
                                            #endregion
                                        }
                                    }
                                    retstate = true;
                                }
                            }
                        }
                        #endregion
                        break;
                    case ModbusFunction.WORDBITSET_F26:
                        #region WordBitSet
                        if (lstResponse.Count == 9)
                        {
                            byte hi = 0xFF, lo = 0xFF;
                            ModbusCRC.GetCRC(lstResponse, 0, 7, ref hi, ref lo);
                            if (lstResponse[7] == hi && lstResponse[8] == lo)
                            {
                                if (WordBitSetRequest != null)
                                {
                                    var args = new WordBitSetRequestArgs(lstResponse.ToArray());
                                    WordBitSetRequest.Invoke(this, args);

                                    if (args.Success)
                                    {
                                        #region Serial Write
                                        int nv = args.WriteValue ? 0xFF00 : 0;
                                        List<byte> ret = new List<byte>();
                                        ret.Add((byte)args.Slave);
                                        ret.Add((byte)args.Function);
                                        ret.Add((byte)((args.StartAddress & 0xFF00) >> 8));
                                        ret.Add((byte)((args.StartAddress & 0x00FF)));
                                        ret.Add((byte)((nv & 0xFF00) >> 8));
                                        ret.Add((byte)((nv & 0x00FF)));
                                        byte nhi = 0xFF, nlo = 0xFF;
                                        ModbusCRC.GetCRC(ret, 0, ret.Count, ref nhi, ref nlo);
                                        ret.Add(nhi);
                                        ret.Add(nlo);
                                        byte[] send = ret.ToArray();
                                        ser.Write(send, 0, send.Length);
                                        ser.BaseStream.Flush();
                                        #endregion
                                    }
                                }
                                retstate = true;
                            }
                        }
                        #endregion
                        break;
                }
            }
            return retstate;
        }
        #endregion
        #endregion

        #region Static Method
        #region ProcessBitReads
        public static void ProcessBitReads(BitReadRequestArgs args, int BaseAddress, bool[] BaseArray)
        {
            var BA = new bool[Convert.ToInt32(Math.Ceiling((double)BaseArray.Length / 8.0) * 8.0)];
            Array.Copy(BaseArray, BA, BaseArray.Length);
            if (args.StartAddress >= BaseAddress && args.StartAddress + args.Length < BaseAddress + BA.Length)
            {
                var ret = new bool[args.Length];
                Array.Copy(BA, args.StartAddress - BaseAddress, ret, 0, args.Length);
                args.ResponseData = ret;
                args.Success = true;
            }
        }
        #endregion
        #region ProcessWordReads
        public static void ProcessWordReads(WordReadRequestArgs args, int BaseAddress, int[] BaseArray)
        {
            if (args.StartAddress >= BaseAddress && args.StartAddress + args.Length < BaseAddress + BaseArray.Length)
            {
                var ret = new int[args.Length];
                Array.Copy(BaseArray, args.StartAddress - BaseAddress, ret, 0, args.Length);
                args.ResponseData = ret;
                args.Success = true;
            }
        }
        #endregion
        #region ProcessBitWrite
        public static void ProcessBitWrite(BitWriteRequestArgs args, int BaseAddress, bool[] BaseArray)
        {
            if (args.StartAddress >= BaseAddress && args.StartAddress < BaseAddress + BaseArray.Length)
            {
                BaseArray[args.StartAddress - BaseAddress] = args.WriteValue;
                args.Success = true;
            }
        }
        #endregion
        #region ProcessWordWrite
        public static void ProcessWordWrite(WordWriteRequestArgs args, int BaseAddress, int[] BaseArray)
        {
            if (args.StartAddress >= BaseAddress && args.StartAddress < BaseAddress + BaseArray.Length)
            {
                BaseArray[args.StartAddress - BaseAddress] = args.WriteValue;
                args.Success = true;
            }
        }
        #endregion
        #region ProcessMultiBitWrite
        public static void ProcessMultiBitWrite(MultiBitWriteRequestArgs args, int BaseAddress, bool[] BaseArray)
        {
            if (args.StartAddress >= BaseAddress && args.StartAddress + args.Length < BaseAddress + BaseArray.Length)
            {
                for (int i = 0; i < args.WriteValues.Length; i++) BaseArray[args.StartAddress - BaseAddress + i] = args.WriteValues[i];
                args.Success = true;
            }
        }
        #endregion
        #region ProcessMultiWordWrite
        public static void ProcessMultiWordWrite(MultiWordWriteRequestArgs args, int BaseAddress, int[] BaseArray)
        {
            if (args.StartAddress >= BaseAddress && args.StartAddress + args.Length < BaseAddress + BaseArray.Length)
            {
                for (int i = 0; i < args.WriteValues.Length; i++) BaseArray[args.StartAddress - BaseAddress + i] = args.WriteValues[i];
                args.Success = true;
            }
        }
        #endregion
        #region ProcessWordBitSet
        public static void ProcessWordBitSet(WordBitSetRequestArgs args, int BaseAddress, int[] BaseArray)
        {
            if (args.StartAddress >= BaseAddress && args.StartAddress < BaseAddress + BaseArray.Length && (args.BitIndex >= 0 && args.BitIndex < 16))
            {
                var p = Convert.ToInt32(Math.Pow(2, args.BitIndex));
                if (args.WriteValue) BaseArray[args.StartAddress - BaseAddress] |= p;
                else BaseArray[args.StartAddress - BaseAddress] &= (ushort)~p;
                args.Success = true;
            }
        }
        #endregion
        #endregion
    }
}
