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

namespace Devinno.Communications.Mitsubishi
{
    #region enum : MITSUBISHI FX Function
    public enum MCFunc
    {
        None,
        BitRead,
        WordRead,
        BitWrite,
        WordWrite,
    }
    #endregion

    public class MC : MasterScheduler
    {
        #region const : Special Code
        private const byte ENQ = 0x05;
        private const byte EOT = 0x04;
        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        private const byte ACK = 0x06;
        private const byte NAK = 0x15;
        private const byte CR = 0x0D;
        private const byte LF = 0x0A;
        private const byte CL = 0x0C;
        #endregion

        #region class : EventArgs
        #region WordDataReadEventArgs
        public class WordDataReadEventArgs : EventArgs
        {
            public int MessageID { get; private set; }
            public int Slave { get; private set; }
            public MCFunc Function { get; private set; }
            public int[] Data { get; private set; }

            public WordDataReadEventArgs(int ID, int Slave, MCFunc Func, int[] Data)
            {
                this.MessageID = ID;
                this.Slave = Slave;
                this.Function = Func;
                this.Data = Data;
            }
        }
        #endregion
        #region BitDataReadEventArgs
        public class BitDataReadEventArgs : EventArgs
        {
            public int MessageID { get; private set; }
            public int Slave { get; private set; }
            public MCFunc Function { get; private set; }
            public bool[] Data { get; private set; }

            public BitDataReadEventArgs(int ID, int Slave, MCFunc Func, bool[] Data)
            {
                this.MessageID = ID;
                this.Slave = Slave;
                this.Function = Func;
                this.Data = Data;
            }
        }
        #endregion
        #region WriteEventArgs
        public class WriteEventArgs : EventArgs
        {
            public int MessageID { get; private set; }
            public int Slave { get; private set; }
            public MCFunc Function { get; private set; }

            public WriteEventArgs(int ID, int Slave, MCFunc Func)
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
            public MCFunc Function { get; private set; }

            public TimeoutEventArgs(int ID, int Slave, MCFunc Func)
            {
                this.MessageID = ID;
                this.Slave = Slave;
                this.Function = Func;
            }
        }
        #endregion
        #region CheckSumErrorEventArgs
        public class CheckSumErrorEventArgs : EventArgs
        {
            public int MessageID { get; private set; }
            public int Slave { get; private set; }
            public MCFunc Function { get; private set; }

            public CheckSumErrorEventArgs(int ID, int Slave, MCFunc Func)
            {
                this.MessageID = ID;
                this.Slave = Slave;
                this.Function = Func;
            }
        }
        #endregion
        #region NakErrorEventArgs
        public class NakErrorEventArgs : EventArgs
        {
            public int MessageID { get; private set; }
            public int Slave { get; private set; }
            public MCFunc Function { get; private set; }

            public NakErrorEventArgs(int ID, int Slave, MCFunc Func)
            {
                this.MessageID = ID;
                this.Slave = Slave;
                this.Function = Func;
            }
        }
        #endregion
        #endregion

        #region Member Variable
        private SerialPort ser = new SerialPort() { PortName = "COM1", BaudRate = 115200 };
        private Thread thAutoStart;
        #endregion

        #region Properties
        /// <summary>
        /// 흐름제어 사용여부
        /// </summary>
        public bool UseControlSequence { get; set; } = false;
        
        /// <summary>
        /// 체크섬 사용여부
        /// </summary>
        public bool UseCheckSum { get; set; } = false;

        /// <summary>
        /// 통신 속도
        /// </summary>
        public int Baudrate { get => ser.BaudRate; set => ser.BaudRate = value; }
        
        /// <summary>
        /// 통신 포트
        /// </summary>
        public string Port { get => ser.PortName; set => ser.PortName = value; }
        
        /// <summary>
        /// 포트 상태
        /// </summary>
        public override bool IsOpen => ser.IsOpen;
        
        /// <summary>
        /// 자동 시작
        /// </summary>
        public bool AutoStart { get; set; }

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
        #endregion

        #region Construct
        public MC()
        {
            thAutoStart = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    if (!IsStart && AutoStart)
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
        public event EventHandler<WordDataReadEventArgs> WordDataReceived;
        public event EventHandler<BitDataReadEventArgs> BitDataReceived;
        public event EventHandler<WriteEventArgs> WriteResponseReceived;
        public event EventHandler<TimeoutEventArgs> TimeoutReceived;
        public event EventHandler<CheckSumErrorEventArgs> CheckSumErrorReceived;
        public event EventHandler<NakErrorEventArgs> NakErrorReceived;

        public event EventHandler DeviceOpened;
        public event EventHandler DeviceClosed;
        #endregion

        #region Method
        #region Start / Stop
        /// <summary>
        /// 지정한 포트 정보로 통신 시작
        /// </summary>
        /// <param name="data">포트 정보</param>
        /// <returns>시작 여부</returns>
        public bool Start(SerialPortSetting data)
        {
            ser.PortName = data.Port;
            ser.BaudRate = data.Baudrate;
            ser.Parity = data.Parity;
            ser.DataBits = data.DataBit;
            ser.StopBits = data.StopBit;
            return Start();
        }

        /// <summary>
        /// 통신 시작
        /// </summary>
        /// <returns>시작 여부</returns>
        public override bool Start()
        {
            if (AutoStart) throw new Exception("AutoStart가 true일 땐 Start/Stop 을 할 수 없습니다.");
            return _Start();
        }

        /// <summary>
        /// 통신 정지
        /// </summary>
        public override void Stop()
        {
            if (AutoStart) throw new Exception("AutoStart가 true일 땐 Start/Stop 을 할 수 없습니다.");
            _Stop();
        }

        private bool _Start()
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

        #region AutoBitRead
        /// <summary>
        /// 지정한 국번의 연속되는 주소에 비트값을 자동으로 읽어 오는 기능
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="slave">국번</param>
        /// <param name="device">주소</param>
        /// <param name="length">개수</param>
        /// <param name="waitTime">응답 대기 시간</param>
        /// <param name="timeout">타임아웃</param>
        public void AutoBitRead(int id, int slave, string device, int length, int waitTime = 0, int? timeout = null)
        {
            device = device.Substring(0, 1) + Convert.ToInt32(device.Substring(1)).ToString("0000");

            if (waitTime > 15) waitTime = 15;
            if (length > 255) length = 255;
            if (slave > 255) slave = 255;

            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(slave.ToString("X2"));
            strbul.Append("FF");
            strbul.Append("BR");
            strbul.Append(waitTime.ToString("X"));
            strbul.Append(device);
            strbul.Append(length.ToString("X2"));
            if (UseCheckSum) strbul.Append("00");
            if (UseControlSequence) strbul.Append("\r\n");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            if (UseCheckSum)
            {
                int CheckSum = 0;
                int lastIndex = data.Length - 1;
                if (UseCheckSum) lastIndex -= 2;
                if (UseControlSequence) lastIndex -= 2;

                for (int i = 1; i <= lastIndex; i++) CheckSum += data[i];
                CheckSum &= 0xFF;
                string bcc = CheckSum.ToString("X2");
                if (UseControlSequence)
                {
                    data[data.Length - 4] = (byte)bcc[0];
                    data[data.Length - 3] = (byte)bcc[1];
                }
                else
                {
                    data[data.Length - 2] = (byte)bcc[0];
                    data[data.Length - 1] = (byte)bcc[1];
                }
            }
            AddAuto(new Work(id, data) { Timeout = timeout });
            data = null;
        }
        #endregion
        #region AutoWordRead
        /// <summary>
        /// 지정한 국번의 연속되는 주소에 워드값을 자동으로 읽어오는 기능
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="slave">국번</param>
        /// <param name="device">주소</param>
        /// <param name="length">개수</param>
        /// <param name="waitTime">응답 대기 시간</param>
        /// <param name="timeout">타임아웃</param>
        public void AutoWordRead(int id, int slave, string device, int length, int waitTime = 0, int? timeout = null)
        {
            device = device.Substring(0, 1) + Convert.ToInt32(device.Substring(1)).ToString("0000");

            if (waitTime > 15) waitTime = 15;
            if (length > 255) length = 255;
            if (slave > 255) slave = 255;

            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(slave.ToString("X2"));
            strbul.Append("FF");
            strbul.Append("WR");
            strbul.Append(waitTime.ToString("X"));
            strbul.Append(device);
            strbul.Append(length.ToString("X2"));
            if (UseCheckSum) strbul.Append("00");
            if (UseControlSequence) strbul.Append("\r\n");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            if (UseCheckSum)
            {
                int CheckSum = 0;
                int lastIndex = data.Length - 1;
                if (UseCheckSum) lastIndex -= 2;
                if (UseControlSequence) lastIndex -= 2;

                for (int i = 1; i <= lastIndex; i++) CheckSum += data[i];
                CheckSum &= 0xFF;
                string bcc = CheckSum.ToString("X2");

                if (UseControlSequence)
                {
                    data[data.Length - 4] = (byte)bcc[0];
                    data[data.Length - 3] = (byte)bcc[1];
                }
                else
                {
                    data[data.Length - 2] = (byte)bcc[0];
                    data[data.Length - 1] = (byte)bcc[1];
                }
            }
            AddAuto(new Work(id, data) { Timeout = timeout });
            data = null;
        }
        #endregion

        #region ManualBitRead
        /// <summary>
        /// 지정한 국번의 연속되는 주소에 비트값을 읽어오는 기능
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="slave">국번</param>
        /// <param name="device">주소</param>
        /// <param name="length">개수</param>
        /// <param name="waitTime">응답 대기 시간</param>
        /// <param name="repeatCount">실패 시 반복횟수</param>
        /// <param name="timeout">타임아웃</param>
        public void ManualBitRead(int id, int slave, string device, int length, int waitTime = 0, int? repeatCount = null, int? timeout = null)
        {
            device = device.Substring(0, 1) + Convert.ToInt32(device.Substring(1)).ToString("0000");

            if (waitTime > 15) waitTime = 15;
            if (length > 255) length = 255;
            if (slave > 255) slave = 255;

            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(slave.ToString("X2"));
            strbul.Append("FF");
            strbul.Append("BR");
            strbul.Append(waitTime.ToString("X"));
            strbul.Append(device);
            strbul.Append(length.ToString("X2"));
            if (UseCheckSum) strbul.Append("00");
            if (UseControlSequence) strbul.Append("\r\n");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            if (UseCheckSum)
            {
                int CheckSum = 0;
                int lastIndex = data.Length - 1;
                if (UseCheckSum) lastIndex -= 2;
                if (UseControlSequence) lastIndex -= 2;

                for (int i = 1; i <= lastIndex; i++) CheckSum += data[i];
                CheckSum &= 0xFF;
                string bcc = CheckSum.ToString("X2");

                if (UseControlSequence)
                {
                    data[data.Length - 4] = (byte)bcc[0];
                    data[data.Length - 3] = (byte)bcc[1];
                }
                else
                {
                    data[data.Length - 2] = (byte)bcc[0];
                    data[data.Length - 1] = (byte)bcc[1];
                }
            }

            AddManual(new Work(id, data) { RepeatCount = repeatCount, Timeout = timeout });
            data = null;
        }
        #endregion
        #region ManualBitWrite
        /// <summary>
        /// 지정한 국번의 주소에 비트값을 쓰는 기능 
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="slave">국번</param>
        /// <param name="device">주소</param>
        /// <param name="value">값</param>
        /// <param name="waitTime">응답 대기 시간</param>
        /// <param name="repeatCount">실패 시 반복횟수</param>
        /// <param name="timeout">타임아웃</param>
        public void ManualBitWrite(int id, int slave, string device, bool value, int waitTime = 0, int? repeatCount = null, int? timeout = null) => ManualBitWrite(id, slave, device, new bool[] { value }, waitTime, repeatCount, timeout);

        /// <summary>
        /// 지정한 국번의 연속되는 주소에 비트값을 쓰는 기능
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="slave">국번</param>
        /// <param name="device">주소</param>
        /// <param name="value">값</param>
        /// <param name="waitTime">응답 대기 시간</param>
        /// <param name="repeatCount">실패 시 반복횟수</param>
        /// <param name="timeout">타임아웃</param>
        public void ManualBitWrite(int id, int slave, string device, bool[] value, int waitTime = 0, int? repeatCount = null, int? timeout = null)
        {
            device = device.Substring(0, 1) + Convert.ToInt32(device.Substring(1)).ToString("0000");
            int Length = value.Length;
            string strValue = "";
            for (int i = 0; i < value.Length; i++) strValue += value[i] ? "1" : "0";

            if (waitTime > 15) waitTime = 15;
            if (Length > 255) Length = 255;
            if (slave > 255) slave = 255;

            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(slave.ToString("X2"));
            strbul.Append("FF");
            strbul.Append("BW");
            strbul.Append(waitTime.ToString("X"));
            strbul.Append(device);
            strbul.Append(Length.ToString("X2"));
            strbul.Append(strValue);

            if (UseCheckSum) strbul.Append("00");
            if (UseControlSequence) strbul.Append("\r\n");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            if (UseCheckSum)
            {
                int CheckSum = 0;
                int lastIndex = data.Length - 1;
                if (UseCheckSum) lastIndex -= 2;
                if (UseControlSequence) lastIndex -= 2;

                for (int i = 1; i <= lastIndex; i++) CheckSum += data[i];
                CheckSum &= 0xFF;
                string bcc = CheckSum.ToString("X2");
                if (UseControlSequence)
                {
                    data[data.Length - 4] = (byte)bcc[0];
                    data[data.Length - 3] = (byte)bcc[1];
                }
                else
                {
                    data[data.Length - 2] = (byte)bcc[0];
                    data[data.Length - 1] = (byte)bcc[1];
                }
            }

            AddManual(new Work(id, data) { RepeatCount = repeatCount, Timeout = timeout });
            data = null;
        }
        #endregion
        #region ManualWordRead
        /// <summary>
        /// 지정한 국번의 연속되는 주소에 워드값을 읽어오는 기능
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="slave">국번</param>
        /// <param name="device">주소</param>
        /// <param name="length">개수</param>
        /// <param name="waitTime">응답 대기 시간</param>
        public void ManualWordRead(int id, int slave, string device, int length, int waitTime = 0, int? repeatCount = null, int? timeout = null)
        {
            device = device.Substring(0, 1) + Convert.ToInt32(device.Substring(1)).ToString("0000");

            if (waitTime > 15) waitTime = 15;
            if (length > 255) length = 255;
            if (slave > 255) slave = 255;

            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(slave.ToString("X2"));
            strbul.Append("FF");
            strbul.Append("WR");
            strbul.Append(waitTime.ToString("X"));
            strbul.Append(device);
            strbul.Append(length.ToString("X2"));

            if (UseCheckSum) strbul.Append("00");
            if (UseControlSequence) strbul.Append("\r\n");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            if (UseCheckSum)
            {
                int CheckSum = 0;
                int lastIndex = data.Length - 1;
                if (UseCheckSum) lastIndex -= 2;
                if (UseControlSequence) lastIndex -= 2;

                for (int i = 1; i <= lastIndex; i++) CheckSum += data[i];
                CheckSum &= 0xFF;
                string bcc = CheckSum.ToString("X2");
                if (UseControlSequence)
                {
                    data[data.Length - 4] = (byte)bcc[0];
                    data[data.Length - 3] = (byte)bcc[1];
                }
                else
                {
                    data[data.Length - 2] = (byte)bcc[0];
                    data[data.Length - 1] = (byte)bcc[1];
                }
            }

            AddManual(new Work(id, data) { RepeatCount = repeatCount, Timeout = timeout });
            data = null;
        }
        #endregion
        #region ManualWordWrite
        /// <summary>
        /// 지정한 국번의 주소에 워드값을 쓰는 기능
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="slave">국번</param>
        /// <param name="device">주소</param>
        /// <param name="value">값</param>
        /// <param name="waitTime">응답 대기 시간</param>
        /// <param name="repeatCount">실패 시 반복횟수</param>
        /// <param name="timeout">타임아웃</param>
        public void ManualWordWrite(int id, int slave, string device, int value, int waitTime = 0, int? repeatCount = null, int? timeout = null) { ManualWordWrite(id, slave, device, new int[] { value }, waitTime, repeatCount, timeout); }

        /// <summary>
        /// 지정한 국번의 연속되는 주소에 워드값을 쓰는 기능
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="slave">국번</param>
        /// <param name="device">주소</param>
        /// <param name="value">값</param>
        /// <param name="waitTime">응답 대기 시간</param>
        /// <param name="repeatCount">실패 시 반복횟수</param>
        /// <param name="timeout">타임아웃</param>
        public void ManualWordWrite(int id, int slave, string device, int[] value, int waitTime = 0, int? repeatCount = null, int? timeout = null)
        {
            device = device.Substring(0, 1) + Convert.ToInt32(device.Substring(1)).ToString("0000");
            int Length = value.Length;
            string strValue = "";
            for (int i = 0; i < value.Length; i++) strValue += value[i].ToString("X4");

            if (waitTime > 15) waitTime = 15;
            if (Length > 255) Length = 255;
            if (slave > 255) slave = 255;

            StringBuilder strbul = new StringBuilder();
            strbul.Append((char)ENQ);
            strbul.Append(slave.ToString("X2"));
            strbul.Append("FF");
            strbul.Append("WW");
            strbul.Append(waitTime.ToString("X"));
            strbul.Append(device);
            strbul.Append(Length.ToString("X2"));
            strbul.Append(strValue);

            if (UseCheckSum) strbul.Append("00");
            if (UseControlSequence) strbul.Append("\r\n");

            byte[] data = Encoding.ASCII.GetBytes(strbul.ToString());

            if (UseCheckSum)
            {
                int CheckSum = 0;
                int lastIndex = data.Length - 1;
                if (UseCheckSum) lastIndex -= 2;
                if (UseControlSequence) lastIndex -= 2;

                for (int i = 1; i <= lastIndex; i++)
                {
                    CheckSum += data[i];
                }
                CheckSum &= 0xFF;
                string bcc = CheckSum.ToString("X2");
                if (UseControlSequence)
                {
                    data[data.Length - 4] = (byte)bcc[0];
                    data[data.Length - 3] = (byte)bcc[1];
                }
                else
                {
                    data[data.Length - 2] = (byte)bcc[0];
                    data[data.Length - 1] = (byte)bcc[1];
                }
            }

            AddManual(new Work(id, data) { RepeatCount = repeatCount, Timeout = timeout });
            data = null;
        }
        #endregion
        #endregion

        #region Static Method
        internal static string FuncToString(MCFunc func)
        {
            string ret = "";
            switch (func)
            {
                case MCFunc.BitRead: ret = "BR"; break;
                case MCFunc.BitWrite: ret = "BW"; break;
                case MCFunc.WordRead: ret = "WR"; break;
                case MCFunc.WordWrite: ret = "WW"; break;
            }
            return ret;
        }
        internal static MCFunc StringToFunc(string func)
        {
            MCFunc ret = MCFunc.None;
            switch (func)
            {
                case "BR": ret = MCFunc.BitRead; break;
                case "BW": ret = MCFunc.BitWrite; break;
                case "WR": ret = MCFunc.WordRead; break;
                case "WW": ret = MCFunc.WordWrite; break;
            }
            return ret;
        }
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

            if (w.Data.Length > 6)
            {
                string strSlave = new string(new char[] { (char)w.Data[1], (char)w.Data[2] });
                string strFunc = new string(new char[] { (char)w.Data[5], (char)w.Data[6] });
                int slave = int.Parse(strSlave, System.Globalization.NumberStyles.AllowHexSpecifier);
                MCFunc func = StringToFunc(strFunc);

                TimeoutReceived?.Invoke(this, new TimeoutEventArgs(w.MessageID, slave, func));
            }
        }
        #endregion
        #region OnCheckCollectComplete
        protected override bool OnCheckCollectComplete(byte[] baResponse, int nRecv, Work w)
        {
            bool ret = false;
            var v = w;
            if (v != null)
            {
                string Func = Encoding.ASCII.GetString(w.Data, 5, 2);
                if (Func == "WR" || Func == "BR")
                {
                    int nLastOffset = UseCheckSum ? 3 : 1;
                    if (nRecv > 0 && nRecv - nLastOffset >= 0)
                        if (baResponse[nRecv - nLastOffset] == ETX)
                            ret = true;
                }
                else
                {
                    if (nRecv >= 5) ret = true;
                }
            }
            return ret;
        }
        #endregion
        #region OnParsePacket
        protected override void OnParsePacket(byte[] baResponse, int nRecv, Work wi)
        {
            var w = wi;
            if (w != null && nRecv > 7)
            {
                try
                {
                    #region Proc
                    string Func = Encoding.ASCII.GetString(w.Data, 5, 2);
                    switch (Func)
                    {
                        #region BR
                        case "BR":
                            if (UseCheckSum)
                            {
                                #region Set
                                string Data = Encoding.ASCII.GetString(baResponse, 5, nRecv - (8 + (UseControlSequence ? 2 : 0)));
                                string CheckSum = Encoding.ASCII.GetString(baResponse, nRecv - (2 + (UseControlSequence ? 2 : 0)), 2);
                                #endregion
                                #region Calc Checksum
                                int nChksum = 0;
                                for (int i = 1; i < nRecv; i++)
                                {
                                    nChksum += baResponse[i];
                                    if (baResponse[i] == ETX) break;
                                }
                                nChksum = nChksum & 0xFF;
                                string CalcCheckSum = nChksum.ToString("X2");
                                #endregion

                                if (CalcCheckSum == CheckSum)
                                {
                                    #region BitDataReceived
                                    if (BitDataReceived != null)
                                    {
                                        string strSlave = new string(new char[] { (char)w.Data[1], (char)w.Data[2] });
                                        string strFunc = new string(new char[] { (char)w.Data[5], (char)w.Data[6] });
                                        int slave = int.Parse(strSlave, System.Globalization.NumberStyles.AllowHexSpecifier);
                                        MCFunc func = StringToFunc(strFunc);

                                        bool[] realData = new bool[Data.Length];
                                        for (int i = 0; i < realData.Length; i++) realData[i] = (Data[i] == '1');

                                        if (BitDataReceived != null)
                                            BitDataReceived.Invoke(this, new BitDataReadEventArgs(w.MessageID, slave, func, realData));
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Checksum Error
                                    if (CheckSumErrorReceived != null)
                                    {
                                        string strSlave = new string(new char[] { (char)w.Data[1], (char)w.Data[2] });
                                        string strFunc = new string(new char[] { (char)w.Data[5], (char)w.Data[6] });
                                        int slave = Convert.ToInt32(strSlave, 16);
                                        MCFunc func = StringToFunc(strFunc);
                                        if (CheckSumErrorReceived != null)
                                            CheckSumErrorReceived.Invoke(this, new CheckSumErrorEventArgs(w.MessageID, slave, func));
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                #region BitDataReceived
                                string Data = Encoding.ASCII.GetString(baResponse, 5, nRecv - (6 + (UseControlSequence ? 2 : 0)));
                                if (BitDataReceived != null)
                                {
                                    string strSlave = new string(new char[] { (char)w.Data[1], (char)w.Data[2] });
                                    string strFunc = new string(new char[] { (char)w.Data[5], (char)w.Data[6] });
                                    int slave = int.Parse(strSlave, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    MCFunc func = StringToFunc(strFunc);

                                    bool[] realData = new bool[Data.Length];
                                    for (int i = 0; i < realData.Length; i++)
                                        realData[i] = (Data[i] == '1');

                                    if (BitDataReceived != null)
                                        BitDataReceived.Invoke(this, new BitDataReadEventArgs(w.MessageID, slave, func, realData));
                                }
                                #endregion
                            }
                            break;
                        #endregion
                        #region WR
                        case "WR":
                            if (UseCheckSum)
                            {
                                #region Set
                                string Data = Encoding.ASCII.GetString(baResponse, 5, nRecv - (8 + (UseControlSequence ? 2 : 0)));
                                string CheckSum = Encoding.ASCII.GetString(baResponse, nRecv - (2 + (UseControlSequence ? 2 : 0)), 2);
                                #endregion
                                #region Calc Checksum
                                int nChksum = 0;
                                for (int i = 1; i < nRecv; i++)
                                {
                                    nChksum += baResponse[i];
                                    if (baResponse[i] == ETX) break;
                                }
                                nChksum = nChksum & 0xFF;
                                string CalcCheckSum = nChksum.ToString("X2");
                                #endregion
                                if (CalcCheckSum == CheckSum)
                                {
                                    #region WordDataReceived
                                    if (WordDataReceived != null)
                                    {
                                        string strSlave = new string(new char[] { (char)w.Data[1], (char)w.Data[2] });
                                        string strFunc = new string(new char[] { (char)w.Data[5], (char)w.Data[6] });
                                        int slave = int.Parse(strSlave, System.Globalization.NumberStyles.AllowHexSpecifier);
                                        MCFunc func = StringToFunc(strFunc);

                                        int[] realData = new int[Data.Length / 4];
                                        for (int i = 0; i < Data.Length; i += 4) realData[i / 4] = Convert.ToInt32(Data.Substring(i, 4), 16);

                                        if (WordDataReceived != null)
                                            WordDataReceived.Invoke(this, new WordDataReadEventArgs(w.MessageID, slave, func, realData));
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Checksum Error
                                    if (CheckSumErrorReceived != null)
                                    {
                                        string strSlave = new string(new char[] { (char)w.Data[1], (char)w.Data[2] });
                                        string strFunc = new string(new char[] { (char)w.Data[5], (char)w.Data[6] });
                                        int slave = int.Parse(strSlave, System.Globalization.NumberStyles.AllowHexSpecifier);
                                        MCFunc func = StringToFunc(strFunc);
                                        if (CheckSumErrorReceived != null)
                                            CheckSumErrorReceived.Invoke(this, new CheckSumErrorEventArgs(w.MessageID, slave, func));
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                #region WordDataReceived
                                string Data = Encoding.ASCII.GetString(baResponse, 5, nRecv - (6 + (UseControlSequence ? 2 : 0)));
                                if (WordDataReceived != null)
                                {
                                    string strSlave = new string(new char[] { (char)w.Data[1], (char)w.Data[2] });
                                    string strFunc = new string(new char[] { (char)w.Data[5], (char)w.Data[6] });
                                    int slave = int.Parse(strSlave, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    MCFunc func = StringToFunc(strFunc);

                                    int[] realData = new int[Data.Length / 4];
                                    for (int i = 0; i < Data.Length; i += 4)
                                        realData[i / 4] = int.Parse(Data.Substring(i, 4), System.Globalization.NumberStyles.AllowHexSpecifier);

                                    if (BitDataReceived != null)
                                        WordDataReceived.Invoke(this, new WordDataReadEventArgs(w.MessageID, slave, func, realData));
                                }
                                #endregion
                            }
                            break;
                        #endregion
                        #region BW/WW
                        case "BW":
                        case "WW":
                            if (baResponse[0] == ACK)
                            {
                                #region WriteResponseReceived
                                if (WriteResponseReceived != null)
                                {
                                    string strSlave = new string(new char[] { (char)w.Data[1], (char)w.Data[2] });
                                    string strFunc = new string(new char[] { (char)w.Data[5], (char)w.Data[6] });
                                    int slave = int.Parse(strSlave, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    MCFunc func = StringToFunc(strFunc);
                                    if (WriteResponseReceived != null)
                                        WriteResponseReceived.Invoke(this, new WriteEventArgs(w.MessageID, slave, func));
                                }
                                #endregion
                            }
                            else
                            {
                                #region NakErrorReceived
                                if (NakErrorReceived != null)
                                {
                                    string strSlave = new string(new char[] { (char)w.Data[1], (char)w.Data[2] });
                                    string strFunc = new string(new char[] { (char)w.Data[5], (char)w.Data[6] });
                                    int slave = int.Parse(strSlave, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    MCFunc func = StringToFunc(strFunc);
                                    if (NakErrorReceived != null)
                                        NakErrorReceived.Invoke(this, new NakErrorEventArgs(w.MessageID, slave, func));
                                }
                                #endregion
                            }
                            break;
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
