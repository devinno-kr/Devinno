using Devinno.Extensions;
using Devinno.Measure;
using Devinno.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Devinno.Communications.Modbus.TCP
{
    public class ModbusTCPSlaveBase
    {
        #region class : EventArgs
        #region BitReadRequestArgs
        public class BitReadRequestArgs : EventArgs
        {
            byte[] Data;
            public int Slave { get { return Data[6]; } }
            public ModbusFunction Function { get { return (ModbusFunction)Data[7]; } }
            public int StartAddress { get { return (Data[8] << 8) | Data[9]; } }
            public int Length { get { return (Data[10] << 8) | Data[11]; } }
            public bool Success { get; set; }

            public bool[] ResponseData { get; set; }

            public BitReadRequestArgs(byte[] Data) { this.Data = Data; }
        }
        #endregion
        #region WordReadRequestArgs
        public class WordReadRequestArgs : EventArgs
        {
            byte[] Data;
            public int Slave { get { return Data[6]; } }
            public ModbusFunction Function { get { return (ModbusFunction)Data[7]; } }
            public int StartAddress { get { return (Data[8] << 8) | Data[9]; } }
            public int Length { get { return (Data[10] << 8) | Data[11]; } }
            public bool Success { get; set; }

            public int[] ResponseData { get; set; }

            public WordReadRequestArgs(byte[] Data) { this.Data = Data; }
        }
        #endregion
        #region BitWriteRequestArgs
        public class BitWriteRequestArgs : EventArgs
        {
            byte[] Data;
            public int Slave { get { return Data[6]; } }
            public ModbusFunction Function { get { return (ModbusFunction)Data[7]; } }
            public int StartAddress { get { return (Data[8] << 8) | Data[9]; } }
            public bool WriteValue { get { return ((Data[10] << 8) | Data[11]) == 0xFF00; } }
            public bool Success { get; set; }

            public BitWriteRequestArgs(byte[] Data) { this.Data = Data; }
        }
        #endregion
        #region WordWriteRequestArgs
        public class WordWriteRequestArgs : EventArgs
        {
            byte[] Data;
            public int Slave { get { return Data[6]; } }
            public ModbusFunction Function { get { return (ModbusFunction)Data[7]; } }
            public int StartAddress { get { return (Data[8] << 8) | Data[9]; } }
            public ushort WriteValue { get { return Convert.ToUInt16((Data[10] << 8) | Data[11]); } }
            public bool Success { get; set; }

            public WordWriteRequestArgs(byte[] Data) { this.Data = Data; }
        }
        #endregion
        #region MultiBitWriteRequestArgs
        public class MultiBitWriteRequestArgs : EventArgs
        {
            byte[] Data;
            public int Slave { get { return Data[6]; } }
            public ModbusFunction Function { get { return (ModbusFunction)Data[7]; } }
            public int StartAddress { get { return (Data[8] << 8) | Data[9]; } }
            public int Length { get { return (Data[10] << 8) | Data[11]; } }
            public int ByteCount { get { return Data[12]; } }
            public bool[] WriteValues { get; private set; }
            public bool Success { get; set; }

            public MultiBitWriteRequestArgs(byte[] Data)
            {
                this.Data = Data;
                #region WriteValues
                List<bool> ret = new List<bool>();
                for (int i = 13; i < Data.Length; i++)
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
            public int Slave { get { return Data[6]; } }
            public ModbusFunction Function { get { return (ModbusFunction)Data[7]; } }
            public int StartAddress { get { return (Data[8] << 8) | Data[9]; } }
            public int Length { get { return (Data[10] << 8) | Data[11]; } }
            public int ByteCount { get { return Data[12]; } }
            public ushort[] WriteValues { get; private set; }
            public bool Success { get; set; }

            public MultiWordWriteRequestArgs(byte[] Data)
            {
                this.Data = Data;
                #region WriteValues
                List<ushort> ret = new List<ushort>();
                for (int i = 13; i < Data.Length; i += 2)
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
            public int Slave { get { return Data[6]; } }
            public ModbusFunction Function { get { return (ModbusFunction)Data[7]; } }
            public int StartAddress { get { return (Data[8] << 8) | Data[9]; } }
            public int BitIndex { get { return Data[10]; } }
            public bool WriteValue { get { return ((Data[11] << 8) | Data[12]) == 0xFF00; } }
            public bool Success { get; set; }

            public WordBitSetRequestArgs(byte[] Data)
            {
                this.Data = Data;
            }
        }
        #endregion
        #endregion

        #region Properties
        public int LocalPort { get; set; } = 502;
        public bool IsStart { get; private set; }

        public int DisconnectCheckTime { get; set; } = 1000;
        #endregion

        #region Event
        public event EventHandler<BitReadRequestArgs> BitReadRequest;
        public event EventHandler<WordReadRequestArgs> WordReadRequest;
        public event EventHandler<BitWriteRequestArgs> BitWriteRequest;
        public event EventHandler<WordWriteRequestArgs> WordWriteRequest;
        public event EventHandler<MultiBitWriteRequestArgs> MultiBitWriteRequest;
        public event EventHandler<MultiWordWriteRequestArgs> MultiWordWriteRequest;
        public event EventHandler<WordBitSetRequestArgs> WordBitSetRequest;

        public event EventHandler<SocketEventArgs> SocketConnected;
        public event EventHandler<SocketEventArgs> SocketDisconnected;
        #endregion

        #region Member Variable
        private byte[] baResponse = new byte[1024 * 8];

        private Socket server;
        private Thread th;
        #endregion

        #region Construct
        public ModbusTCPSlaveBase() { }
        #endregion

        #region Thread
        void WorkProc()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, LocalPort);
            server.Bind(ipEndPoint);
            server.Listen(10);

            IsStart = true;

            while (IsStart)
            {
                Socket hserver = server.Accept();
                Thread th = new Thread(new ParameterizedThreadStart(Run));
                th.IsBackground = true;
                th.Start(hserver);
            }
        }

        void Run(object obj)
        {
            var server = obj as Socket;
            if (server != null)
            {
                #region SocketConnected
                SocketConnected?.Invoke(this, new SocketEventArgs(server));
                #endregion

                List<byte> lstResponse = new List<byte>();
                var prev = DateTime.Now;
                var chr = new Chattering() { ChatteringTime = DisconnectCheckTime };

                bool IsThStart = true;

                chr.StateChanged += (o, s) => { if (!s.Value) IsThStart = false; };
                while (IsThStart)
                {
                    try
                    {
                        #region DataRead
                        if (server.Available > 0)
                        {
                            try
                            {
                                int n = server.Receive(baResponse);
                                for (int i = 0; i < n; i++) lstResponse.Add(baResponse[i]);
                                prev = DateTime.Now;
                            }
                            catch (TimeoutException) { }
                        }
                        #endregion

                        #region Modbus Parse
                        if (lstResponse.Count >= 10)
                        {
                            int Slave = lstResponse[6];
                            ModbusFunction Function = (ModbusFunction)lstResponse[7];
                            int StartAddress = (lstResponse[8] << 8) | lstResponse[9];

                            switch (Function)
                            {
                                case ModbusFunction.BITREAD_F1:
                                case ModbusFunction.BITREAD_F2:
                                    #region BitRead
                                    if (lstResponse.Count == 12)
                                    {
                                        int Length = (lstResponse[10] << 8) | lstResponse[11];

                                        if (BitReadRequest != null)
                                        {
                                            var args = new BitReadRequestArgs(lstResponse.ToArray());
                                            BitReadRequest?.Invoke(this, args);

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
                                                #region Write
                                                List<byte> ret = new List<byte>();
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add((byte)(((nlen + 3) & 0xFF00) >> 8));
                                                ret.Add((byte)(((nlen + 3) & 0x00FF)));
                                                ret.Add((byte)Slave);
                                                ret.Add((byte)Function);
                                                ret.Add((byte)Datas.Count);
                                                ret.AddRange(Datas.ToArray());

                                                byte[] send = ret.ToArray();
                                                server.Send(send);
                                                #endregion
                                            }
                                        }
                                        lstResponse.Clear();
                                    }
                                    #endregion
                                    break;
                                case ModbusFunction.WORDREAD_F3:
                                case ModbusFunction.WORDREAD_F4:
                                    #region WordRead
                                    if (lstResponse.Count == 12)
                                    {
                                        int Length = (lstResponse[10] << 8) | lstResponse[11];

                                        if (WordReadRequest != null)
                                        {
                                            var args = new WordReadRequestArgs(lstResponse.ToArray());
                                            WordReadRequest?.Invoke(this, args);

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
                                                #region Write
                                                int nlen = Length * 2;
                                                List<byte> ret = new List<byte>();
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add((byte)(((nlen + 3) & 0xFF00) >> 8));
                                                ret.Add((byte)(((nlen + 3) & 0x00FF)));
                                                ret.Add((byte)Slave);
                                                ret.Add((byte)Function);
                                                ret.Add((byte)Datas.Count);
                                                ret.AddRange(Datas.ToArray());

                                                byte[] send = ret.ToArray();
                                                server.Send(send);
                                                #endregion
                                            }
                                        }
                                        lstResponse.Clear();
                                    }
                                    #endregion
                                    break;
                                case ModbusFunction.BITWRITE_F5:
                                    #region BitWrite
                                    if (lstResponse.Count == 12)
                                    {
                                        int WriteValue = (lstResponse[10] << 8) | lstResponse[11];
                                        if (BitWriteRequest != null)
                                        {
                                            var args = new BitWriteRequestArgs(lstResponse.ToArray());
                                            BitWriteRequest?.Invoke(this, args);

                                            if (args.Success)
                                            {
                                                #region Write
                                                int nv = args.WriteValue ? 0xFF00 : 0;
                                                List<byte> ret = new List<byte>();
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(6);
                                                ret.Add((byte)Slave);
                                                ret.Add((byte)Function);
                                                ret.Add((byte)((StartAddress & 0xFF00) >> 8));
                                                ret.Add((byte)((StartAddress & 0x00FF)));
                                                ret.Add((byte)((nv & 0xFF00) >> 8));
                                                ret.Add((byte)((nv & 0x00FF)));

                                                byte[] send = ret.ToArray();
                                                server.Send(send);
                                                #endregion
                                            }
                                        }
                                        lstResponse.Clear();
                                    }
                                    #endregion
                                    break;
                                case ModbusFunction.WORDWRITE_F6:
                                    #region WordWrite
                                    if (lstResponse.Count == 12)
                                    {
                                        int WriteValue = (lstResponse[10] << 8) | lstResponse[11];

                                        if (WordWriteRequest != null)
                                        {
                                            var args = new WordWriteRequestArgs(lstResponse.ToArray());
                                            WordWriteRequest?.Invoke(this, args);

                                            if (args.Success)
                                            {
                                                #region Write
                                                List<byte> ret = new List<byte>();
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(6);
                                                ret.Add((byte)args.Slave);
                                                ret.Add((byte)args.Function);
                                                ret.Add((byte)((args.StartAddress & 0xFF00) >> 8));
                                                ret.Add((byte)((args.StartAddress & 0x00FF)));
                                                ret.Add((byte)((args.WriteValue & 0xFF00) >> 8));
                                                ret.Add((byte)((args.WriteValue & 0x00FF)));

                                                byte[] send = ret.ToArray();
                                                server.Send(send);
                                                #endregion
                                            }
                                        }
                                        lstResponse.Clear();
                                    }
                                    #endregion
                                    break;
                                case ModbusFunction.MULTIBITWRITE_F15:
                                    #region MultiBitWrite
                                    if (lstResponse.Count >= 13)
                                    {
                                        int ByteCount = lstResponse[12];
                                        if (lstResponse.Count >= 13 + ByteCount)
                                        {
                                            var args = new MultiBitWriteRequestArgs(lstResponse.ToArray());
                                            if (MultiBitWriteRequest != null)
                                            {
                                                MultiBitWriteRequest?.Invoke(this, args);

                                                if (args.Success)
                                                {
                                                    #region Write
                                                    List<byte> ret = new List<byte>();
                                                    ret.Add(0);
                                                    ret.Add(0);
                                                    ret.Add(0);
                                                    ret.Add(0);
                                                    ret.Add(0);
                                                    ret.Add(6);
                                                    ret.Add((byte)args.Slave);
                                                    ret.Add((byte)args.Function);
                                                    ret.Add((byte)((args.StartAddress & 0xFF00) >> 8));
                                                    ret.Add((byte)((args.StartAddress & 0x00FF)));
                                                    ret.Add((byte)((args.Length & 0xFF00) >> 8));
                                                    ret.Add((byte)((args.Length & 0x00FF)));

                                                    byte[] send = ret.ToArray();
                                                    server.Send(send);
                                                    #endregion
                                                }
                                            }
                                            lstResponse.Clear();
                                        }
                                    }
                                    #endregion
                                    break;
                                case ModbusFunction.MULTIWORDWRITE_F16:
                                    #region MultiWordWrite
                                    if (lstResponse.Count >= 13)
                                    {
                                        int ByteCount = lstResponse[12];
                                        if (lstResponse.Count >= 13 + ByteCount)
                                        {
                                            if (MultiWordWriteRequest != null)
                                            {
                                                var args = new MultiWordWriteRequestArgs(lstResponse.ToArray());
                                                MultiWordWriteRequest?.Invoke(this, args);

                                                if (args.Success)
                                                {
                                                    #region Write
                                                    List<byte> ret = new List<byte>();
                                                    ret.Add(0);
                                                    ret.Add(0);
                                                    ret.Add(0);
                                                    ret.Add(0);
                                                    ret.Add(0);
                                                    ret.Add(6);
                                                    ret.Add((byte)args.Slave);
                                                    ret.Add((byte)args.Function);
                                                    ret.Add((byte)((args.StartAddress & 0xFF00) >> 8));
                                                    ret.Add((byte)((args.StartAddress & 0x00FF)));
                                                    ret.Add((byte)((args.Length & 0xFF00) >> 8));
                                                    ret.Add((byte)((args.Length & 0x00FF)));

                                                    byte[] send = ret.ToArray();
                                                    server.Send(send);
                                                    #endregion
                                                }
                                            }
                                            lstResponse.Clear();
                                        }
                                    }
                                    #endregion
                                    break;
                                case ModbusFunction.WORDBITSET_F26:
                                    #region WordBitSet
                                    if (lstResponse.Count == 13)
                                    {
                                        if (WordBitSetRequest != null)
                                        {
                                            var args = new WordBitSetRequestArgs(lstResponse.ToArray());
                                            WordBitSetRequest?.Invoke(this, args);

                                            if (args.Success)
                                            {
                                                #region Write
                                                int nv = args.WriteValue ? 0xFF00 : 0;
                                                List<byte> ret = new List<byte>();
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(0);
                                                ret.Add(6);
                                                ret.Add((byte)args.Slave);
                                                ret.Add((byte)args.Function);
                                                ret.Add((byte)((args.StartAddress & 0xFF00) >> 8));
                                                ret.Add((byte)((args.StartAddress & 0x00FF)));
                                                ret.Add((byte)((nv & 0xFF00) >> 8));
                                                ret.Add((byte)((nv & 0x00FF)));

                                                byte[] send = ret.ToArray();
                                                server.Send(send);
                                                #endregion
                                            }
                                        }
                                        lstResponse.Clear();
                                    }
                                    #endregion
                                    break;
                            }
                        }
                        #endregion

                        #region Buffer Clear
                        if ((DateTime.Now - prev).TotalMilliseconds >= 50 && lstResponse.Count > 0) lstResponse.Clear();
                        #endregion

                        chr.Set(NetworkTool.IsSocketConnected(server));
                    }
                    catch(SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.TimedOut) { }
                        else if (ex.SocketErrorCode == SocketError.ConnectionReset) { IsThStart = false; }
                        else if (ex.SocketErrorCode == SocketError.ConnectionAborted) { IsThStart = false; }
                    }
                    catch { }
                    Thread.Sleep(10);
                }

                #region Socket Closed
                if (server.Connected) server.Close();
                SocketDisconnected?.Invoke(this, new SocketEventArgs(server));
                #endregion
            }
        }
        #endregion

        #region Method
        #region Start
        public bool Start()
        {
            try
            {
                th = new Thread(new ThreadStart(WorkProc));
                th.IsBackground = true;
                th.Start();
                return true;
            }
            catch (Exception e) { System.IO.File.AppendAllText("er.txt", e.ToString() + "\r\n" + e.StackTrace.ToString()); }
            return false;
        }
        #endregion
        #region Stop
        public bool Stop()
        {
            try
            {
                IsStart = false;
                return true;
            }
            catch (Exception e) { System.IO.File.AppendAllText("er.txt", e.ToString() + "\r\n" + e.StackTrace.ToString()); }
            return false;
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
