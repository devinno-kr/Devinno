using Devinno.Communications.Scheduler;
using Devinno.Communications.Setting;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Devinno.Communications.LS
{
    #region enum : MASTERK Function
    public enum MasterKFunc { NONE, READ_SINGLE, READ_BLOCK, WRITE_SINGLE, WRITE_BLOCK, }
    #endregion

    public class CNet : MasterScheduler
    {
        #region const : Special Code
        private const byte ENQ = 0x05;
        private const byte EOT = 0x04;
        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        private const byte ACK = 0x06;
        private const byte NAK = 0x15;
        #endregion

        #region class : Work
        private class WorkCN : MasterScheduler.Work
        {
            public int Slave { get { int n; return int.TryParse(Encoding.ASCII.GetString(Data, 1, 2), out n) ? n : -1; } }
            public MasterKFunc Function { get { return StringToFunc(Encoding.ASCII.GetString(Data, 3, 3)); } }
            public string[] Devices { get; set; }

            public WorkCN(int id, byte[] data)
                : base(id, data)
            {
            }
        }
        #endregion

        #region class : EventArgs
        #region DataReadEventArgs
        public class DataReadEventArgs : EventArgs
        {
            public int MessageID { get; private set; }
            public int Slave { get; private set; }
            public MasterKFunc Function { get; private set; }
            public int[] Data { get; private set; }
            public string[] ReadAddress { get; private set; }

            public DataReadEventArgs(int ID, int Slave, MasterKFunc Func, int[] Data, string[] ReadAddress)
            {
                this.MessageID = ID;
                this.Slave = Slave;
                this.Function = Func;
                this.Data = Data;
                this.ReadAddress = ReadAddress;
            }
        }
        #endregion
        #region WriteEventArgs
        public class WriteEventArgs : EventArgs
        {
            public int MessageID { get; private set; }
            public int Slave { get; private set; }
            public MasterKFunc Function { get; private set; }

            public WriteEventArgs(int ID, int Slave, MasterKFunc Func)
            {
                this.MessageID = ID;
                this.Slave = Slave;
                this.Function = Func;
            }
        }
        #endregion
        #region TimeoutEventArgs
        public class TimeoutEventArgs : EventArgs
        {
            public int MessageID { get; private set; }
            public int Slave { get; private set; }
            public MasterKFunc Function { get; private set; }

            public string[] Device { get; set; }

            public TimeoutEventArgs(int ID, int Slave, MasterKFunc Func)
            {
                this.MessageID = ID;
                this.Slave = Slave;
                this.Function = Func;
            }
        }
        #endregion
        #region BCCErrorEventArgs
        public class BCCErrorEventArgs : EventArgs
        {
            public int MessageID { get; private set; }
            public int Slave { get; private set; }
            public MasterKFunc Function { get; private set; }

            public BCCErrorEventArgs(int ID, int Slave, MasterKFunc Func)
            {
                this.MessageID = ID;
                this.Slave = Slave;
                this.Function = Func;
            }
        }
        #endregion
        #region NAKEventArgs
        public class NAKEventArgs : EventArgs
        {
            public int MessageID { get; private set; }
            public int Slave { get; private set; }
            public MasterKFunc Function { get; private set; }
            public int ErrorCode { get; private set; }

            public NAKEventArgs(int ID, int Slave, MasterKFunc Func, int ErrCode)
            {
                this.MessageID = ID;
                this.Slave = Slave;
                this.Function = Func;
                this.ErrorCode = ErrCode;
            }
        }
        #endregion
        #endregion

        #region Member Variable
        private SerialPort ser = new SerialPort() { PortName = "COM1", BaudRate = 115200 };
        private Thread thAutoStart;
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
        public bool AutoStart { get; set; }
        #endregion

        #region Construct
        public CNet()
        {
            thAutoStart = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    if (!IsStart)
                    {
                        _Start();
                    }
                    Thread.Sleep(1000);
                }
            }))
            { IsBackground = true };
            thAutoStart.Start();
        }
        #endregion

        #region Event
        public event EventHandler<DataReadEventArgs> DataReceived;
        public event EventHandler<WriteEventArgs> WriteResponseReceived;
        public event EventHandler<TimeoutEventArgs> TimeoutReceived;
        public event EventHandler<BCCErrorEventArgs> BCCErrorReceived;
        public event EventHandler<NAKEventArgs> NAKReceived;

        public event EventHandler DeviceOpened;
        public event EventHandler DeviceClosed;
        #endregion

        #region Method
        #region Start / Stop
        public bool Start(SerialPortSetting data)
        {
            ser.PortName = data.Port;
            ser.BaudRate = data.Baudrate;
            ser.Parity = data.Parity;
            ser.DataBits = data.DataBit;
            ser.StopBits = data.StopBit;
            return Start();
        }

        public override bool Start()
        {
            if (AutoStart) throw new Exception("AutoStart가 true일 땐 Start/Stop 을 할 수 없습니다.");
            return _Start();
        }
        public override void Stop()
        {
            if (AutoStart) throw new Exception("AutoStart가 true일 땐 Start/Stop 을 할 수 없습니다.");
            _Stop();
        }

        bool _Start()
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

        private void _Stop() => StopThread();
        #endregion

        #region AutoRSS(id, slave, device)
        public void AutoRSS(int id, int slave, string device)
        {
            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(slave.ToString("X2"));
            strbul.Append("rSS");
            strbul.Append("01");
            strbul.Append(device.Length.ToString("X2"));
            strbul.Append(device);
            strbul.Append((char)EOT);
            strbul.Append("00");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            int nBCC = 0;
            for (int i = 0; i < data.Length; i++)
            {
                nBCC += data[i];
                if (data[i] == EOT) break;
            }
            nBCC &= 0xFF;
            string bcc = nBCC.ToString("X2");
            data[data.Length - 2] = (byte)bcc[0];
            data[data.Length - 1] = (byte)bcc[1];

            AddAuto(new WorkCN(id, data) { Devices = new string[] { device } });
            data = null;
        }
        #endregion
        #region AutoRSS(id, slave, devices)
        public void AutoRSS(int id, int Slave, string[] devices)
        {
            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(Slave.ToString("X2"));
            strbul.Append("rSS");
            strbul.Append(devices.Length.ToString("X2"));
            for (int i = 0; i < devices.Length; i++)
            {
                strbul.Append(devices[i].Length.ToString("X2"));
                strbul.Append(devices[i]);
            }
            strbul.Append((char)EOT);
            strbul.Append("00");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            int nBCC = 0;
            for (int i = 0; i < data.Length; i++)
            {
                nBCC += data[i];
                if (data[i] == EOT) break;
            }
            nBCC &= 0xFF;
            string bcc = nBCC.ToString("X2");
            data[data.Length - 2] = (byte)bcc[0];
            data[data.Length - 1] = (byte)bcc[1];

            AddAuto(new WorkCN(id, data) { Devices = devices });
            data = null;
        }
        #endregion
        #region AutoRSB
        public void AutoRSB(int id, int Slave, string device, int Length)
        {
            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(Slave.ToString("X2"));
            strbul.Append("rSB");
            strbul.Append(device.Length.ToString("X2"));
            strbul.Append(device);
            strbul.Append(Length.ToString("X2"));
            strbul.Append((char)EOT);
            strbul.Append("00");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            int nBCC = 0;
            for (int i = 0; i < data.Length; i++)
            {
                nBCC += data[i];
                if (data[i] == EOT) break;
            }
            nBCC &= 0xFF;
            string bcc = nBCC.ToString("X2");
            data[data.Length - 2] = (byte)bcc[0];
            data[data.Length - 1] = (byte)bcc[1];

            List<string> d = new List<string>();
            int n = Convert.ToInt32(device.Substring(3));
            for (int i = 0; i < Length; i++) d.Add(device.Substring(0, 3) + (n + i));

            AddAuto(new WorkCN(id, data) { Devices = d.ToArray() });
            data = null;
        }
        #endregion

        #region ManualRSS(id, slave, device)
        public void ManualRSS(int id, int slave, string device)
        {
            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(slave.ToString("X2"));
            strbul.Append("rSS");
            strbul.Append("01");
            strbul.Append(device.Length.ToString("X2"));
            strbul.Append(device);
            strbul.Append((char)EOT);
            strbul.Append("00");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            int nBCC = 0;
            for (int i = 0; i < data.Length; i++)
            {
                nBCC += data[i];
                if (data[i] == EOT) break;
            }
            nBCC &= 0xFF;
            string bcc = nBCC.ToString("X2");
            data[data.Length - 2] = (byte)bcc[0];
            data[data.Length - 1] = (byte)bcc[1];

            AddManual(new WorkCN(id, data) { Devices = new string[] { device } });
            data = null;
        }
        #endregion
        #region ManualRSS(id, slave, devices)
        public void ManualRSS(int id, int slave, string[] devices)
        {
            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(slave.ToString("X2"));
            strbul.Append("rSS");
            strbul.Append(devices.Length.ToString("X2"));
            for (int i = 0; i < devices.Length; i++)
            {
                strbul.Append(devices[i].Length.ToString("X2"));
                strbul.Append(devices[i]);
            }
            strbul.Append((char)EOT);
            strbul.Append("00");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            int nBCC = 0;
            for (int i = 0; i < data.Length; i++)
            {
                nBCC += data[i];
                if (data[i] == EOT) break;
            }
            nBCC &= 0xFF;
            string bcc = nBCC.ToString("X2");
            data[data.Length - 2] = (byte)bcc[0];
            data[data.Length - 1] = (byte)bcc[1];

            AddManual(new WorkCN(id, data) { Devices = devices });
            data = null;
        }
        #endregion
        #region ManualRSB 
        public void ManualRSB(int id, int Slave, string device, int Length)
        {
            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(Slave.ToString("X2"));
            strbul.Append("rSB");
            strbul.Append(device.Length.ToString("X2"));
            strbul.Append(device);
            strbul.Append(Length.ToString("X2"));
            strbul.Append((char)EOT);
            strbul.Append("00");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            int nBCC = 0;
            for (int i = 0; i < data.Length; i++)
            {
                nBCC += data[i];
                if (data[i] == EOT) break;
            }
            nBCC &= 0xFF;
            string bcc = nBCC.ToString("X2");
            data[data.Length - 2] = (byte)bcc[0];
            data[data.Length - 1] = (byte)bcc[1];

            List<string> d = new List<string>();
            int n = Convert.ToInt32(device.Substring(3));
            for (int i = 0; i < Length; i++)
                d.Add(device.Substring(0, 3) + (n + i));

            AddManual(new WorkCN(id, data) { Devices = d.ToArray() });
            data = null;
        }
        #endregion
        #region ManualWSS
        public void ManualWSS(int id, int Slave, string device, int value)
        {
            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(Slave.ToString("X2"));
            strbul.Append("wSS");
            strbul.Append("01");
            strbul.Append(device.Length.ToString("X2"));
            strbul.Append(device);
            switch (device[2])
            {
                case 'X': strbul.Append(value.ToString("X2")); break;
                case 'B': strbul.Append(value.ToString("X2")); break;
                case 'W': strbul.Append(value.ToString("X4")); break;
                case 'D': strbul.Append(value.ToString("X8")); break;
                case 'L': strbul.Append(value.ToString("X16")); break;
            }
            strbul.Append((char)EOT);
            strbul.Append("00");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            int nBCC = 0;
            for (int i = 0; i < data.Length; i++)
            {
                nBCC += data[i];
                if (data[i] == EOT) break;
            }
            nBCC &= 0xFF;
            string bcc = nBCC.ToString("X2");
            data[data.Length - 2] = (byte)bcc[0];
            data[data.Length - 1] = (byte)bcc[1];

            AddManual(new WorkCN(id, data));
            data = null;
        }
        #endregion
        #region ManualWSS
        public void ManualWSS(int id, int Slave, string[] devices, int[] value)
        {
            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(Slave.ToString("X2"));
            strbul.Append("wSS");
            strbul.Append(devices.Length.ToString("X2"));
            for (int i = 0; i < devices.Length; i++)
            {
                strbul.Append(devices[i].Length.ToString("X2"));
                strbul.Append(devices[i]);
                if (devices[i][2] == 'W')
                    strbul.Append(value[i].ToString("X4"));
                if (devices[i][2] == 'X')
                    strbul.Append(value[i].ToString("X2"));
            }
            strbul.Append((char)EOT);
            strbul.Append("00");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            int nBCC = 0;
            for (int i = 0; i < data.Length; i++)
            {
                nBCC += data[i];
                if (data[i] == EOT) break;
            }
            nBCC &= 0xFF;
            string bcc = nBCC.ToString("X2");
            data[data.Length - 2] = (byte)bcc[0];
            data[data.Length - 1] = (byte)bcc[1];

            AddManual(new WorkCN(id, data));
            data = null;
        }
        #endregion
        #region ManualWSB
        public void ManualWSB(int id, int Slave, string device, int[] values)
        {
            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(Slave.ToString("X2"));
            strbul.Append("wSB");
            strbul.Append(device.Length.ToString("X2"));
            strbul.Append(values.Length);
            for (int i = 0; i < values.Length; i++)
                strbul.Append(values[i].ToString("X4"));
            strbul.Append((char)EOT);
            strbul.Append("00");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            int nBCC = 0;
            for (int i = 0; i < data.Length; i++)
            {
                nBCC += data[i];
                if (data[i] == EOT) break;
            }
            nBCC &= 0xFF;
            string bcc = nBCC.ToString("X2");
            data[data.Length - 2] = (byte)bcc[0];
            data[data.Length - 1] = (byte)bcc[1];

            AddManual(new WorkCN(id, data));
            data = null;
        }
        #endregion
        #endregion

        #region Static Method
        #region FuncToString
        public static string FuncToString(MasterKFunc func)
        {
            string ret = "";
            switch (func)
            {
                case MasterKFunc.READ_SINGLE:
                    ret = "rSS";
                    break;
                case MasterKFunc.READ_BLOCK:
                    ret = "rSB";
                    break;
                case MasterKFunc.WRITE_SINGLE:
                    ret = "wSS";
                    break;
                case MasterKFunc.WRITE_BLOCK:
                    ret = "wSB";
                    break;
            }
            return ret;
        }
        #endregion
        #region StringToFunc
        public static MasterKFunc StringToFunc(string func)
        {
            MasterKFunc ret = MasterKFunc.NONE;
            switch (func)
            {
                case "rSS":
                    ret = MasterKFunc.READ_SINGLE;
                    break;
                case "rSB":
                    ret = MasterKFunc.READ_BLOCK;
                    break;
                case "wSS":
                    ret = MasterKFunc.WRITE_SINGLE;
                    break;
                case "wSB":
                    ret = MasterKFunc.WRITE_BLOCK;
                    break;
            }
            return ret;
        }
        #endregion
        #endregion

        #region Override
        #region OnWrite
        protected override void OnWrite(byte[] data, int offset, int count, int timeout)
        {
            try
            {
                ser.DiscardInBuffer();
                ser.DiscardOutBuffer();
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
                ser.ReadTimeout = timeout;
                return ser.Read(data, offset, count);
            }
            catch (IOException) { throw new SchedulerStopException(); }
            catch (UnauthorizedAccessException) { throw new SchedulerStopException(); }
            catch (InvalidOperationException) { throw new SchedulerStopException(); }
            catch { return null; }
        }
        #endregion
        #region OnFlush
        protected override void OnFlush()
        {
            try
            {
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
        #region OnTimeout
        public override void OnTimeout(Work w)
        {
            base.OnTimeout(w);

            var v = w as WorkCN;
            if (v != null) TimeoutReceived?.Invoke(this, new TimeoutEventArgs(v.MessageID, v.Slave, v.Function));
        }
        #endregion
        #region OnCheckCollectComplete
        protected override bool OnCheckCollectComplete(byte[] data, int count, Work w)
        {
            bool ret = false;
            var v = w as WorkCN;
            if (v != null)
                if (count > 3 && count < 256)
                    if (data[count - 3] == ETX)
                        ret = true;
            return ret;
        }
        #endregion
        #region OnParsePacket
        protected override void OnParsePacket(byte[] baResponse, int nRecv, Work wi)
        {
            var w = wi as WorkCN;
            if (w != null && nRecv > 6)
            {
                try
                {
                    #region Proc
                    #region Variable set
                    string strSlave = new string(new char[] { (char)baResponse[1], (char)baResponse[2] });
                    string strFunc = new string(new char[] { (char)baResponse[3], (char)baResponse[4], (char)w.Data[5] });
                    string strBCC = new string(new char[] { (char)baResponse[nRecv - 2], (char)baResponse[nRecv - 1] });

                    int slave = Convert.ToInt32(strSlave, 16);
                    MasterKFunc func = StringToFunc(strFunc);
                    int nBCC = Convert.ToInt32(strBCC, 16);
                    #endregion
                    #region Calc BCC
                    int nCalcBCC = 0;
                    for (int i = 0; i < nRecv; i++)
                    {
                        nCalcBCC += baResponse[i];
                        if (baResponse[i] == ETX) break;
                    }
                    nCalcBCC &= 0xFF;
                    #endregion

                    if (nCalcBCC == nBCC)
                    {
                        if (baResponse[0] == ACK)
                        {
                            switch (func)
                            {
                                #region READ_BLACK
                                case MasterKFunc.READ_BLOCK:
                                    {
                                        int BlockCount = int.Parse(new string(new char[] { (char)baResponse[6], (char)baResponse[7] }));
                                        List<int> Data = new List<int>();
                                        int nIndexOffset = 0;
                                        int DataType = 0;
                                        switch ((char)w.Data[10])
                                        {
                                            case 'X': DataType = 1; break;
                                            case 'B': DataType = 1; break;
                                            case 'W': DataType = 2; break;
                                            case 'D': DataType = 4; break;
                                            case 'L': DataType = 8; break;
                                        }
                                        for (int i = 0; i < BlockCount; i++)
                                        {
                                            int nSize = int.Parse(new string(new char[] { (char)baResponse[8 + nIndexOffset], (char)baResponse[9 + nIndexOffset] }), System.Globalization.NumberStyles.AllowHexSpecifier);
                                            for (int j = 0; j < nSize * 2; j += (DataType * 2))
                                            {
                                                string str = "";
                                                for (int k = j; k < j + (DataType * 2); k++) str += (char)baResponse[10 + k];
                                                Data.Add(Convert.ToInt32(str, 16));
                                            }
                                            nIndexOffset += ((nSize * 2) + 2);
                                        }
                                        DataReceived?.Invoke(this, new DataReadEventArgs(w.MessageID, slave, func, Data.ToArray(), w.Devices));
                                    }
                                    break;
                                #endregion
                                #region READ_SINGLE
                                case MasterKFunc.READ_SINGLE:
                                    {
                                        int BlockCount = int.Parse(new string(new char[] { (char)baResponse[6], (char)baResponse[7] }));
                                        List<int> Data = new List<int>();
                                        int nIndexOffset = 0;
                                        string[] street = Encoding.ASCII.GetString(w.Data).Split('%');
                                        for (int i = 0; i < BlockCount; i++)
                                        {
                                            int DataType = 0;
                                            switch ((char)street[i + 1][1])
                                            {
                                                case 'X': DataType = 1; break;
                                                case 'B': DataType = 1; break;
                                                case 'W': DataType = 2; break;
                                                case 'D': DataType = 4; break;
                                                case 'L': DataType = 8; break;
                                            }
                                            string stre = Encoding.ASCII.GetString(baResponse).Trim();
                                            int nSize = int.Parse(new string(new char[] { (char)baResponse[8 + nIndexOffset], (char)baResponse[9 + nIndexOffset] }), System.Globalization.NumberStyles.AllowHexSpecifier);
                                            for (int j = 0; j < nSize * 2; j += (DataType * 2))
                                            {
                                                string str = "";
                                                for (int k = j; k < j + (DataType * 2); k++) str += (char)baResponse[10 + nIndexOffset + k];
                                                Data.Add(int.Parse(str, System.Globalization.NumberStyles.AllowHexSpecifier));
                                            }
                                            nIndexOffset += ((nSize * 2) + 2);
                                        }
                                        DataReceived?.Invoke(this, new DataReadEventArgs(w.MessageID, slave, func, Data.ToArray(), w.Devices));
                                    }
                                    break;
                                #endregion
                                #region WRITE_BLOCK
                                case MasterKFunc.WRITE_BLOCK:
                                    {
                                        WriteResponseReceived?.Invoke(this, new WriteEventArgs(w.MessageID, slave, func));
                                    }
                                    break;
                                #endregion
                                #region WRITE_SINGLE
                                case MasterKFunc.WRITE_SINGLE:
                                    {
                                        WriteResponseReceived?.Invoke(this, new WriteEventArgs(w.MessageID, slave, func));
                                    }
                                    break;
                                    #endregion
                            }
                        }
                        else if (baResponse[0] == NAK)
                        {
                            #region NAK
                            int ErrorCode = 0;
                            string str = "";
                            for (int i = 6; i < nRecv - 3; i++) str += (char)baResponse[i];
                            ErrorCode = Convert.ToInt32(str, 16);

                            NAKReceived?.Invoke(this, new NAKEventArgs(w.MessageID, slave, func, ErrorCode));
                            #endregion
                        }
                    }
                    else
                    {
                        #region BCC ERROR
                        BCCErrorReceived?.Invoke(this, new BCCErrorEventArgs(w.MessageID, slave, func));
                        #endregion
                    }
                    #endregion
                }
                catch (Exception) { }
            }
        }
        #endregion
        #endregion
    }
}
