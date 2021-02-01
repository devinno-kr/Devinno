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

namespace Devinno.Communications.TextComm.RTU
{
    public class TextCommRTUMaster : MasterScheduler
    {
        #region class : WorkTC
        class WorkTC : MasterScheduler.Work
        {
            public int ResponseCount { get; private set; }
            public byte Slave { get; set; }
            public byte Command { get; set; }
            public string Message { get; set; }

            public WorkTC(int id, byte[] data, byte Slave, byte Command, string Message)
                : base(id, data)
            {
                this.Slave = Slave;
                this.Command = Command;
                this.Message = Message;
            }
        }
        #endregion
        #region class : EventArgs
        #region ReceivedEventArgs
        public class ReceivedEventArgs : EventArgs
        {
            private Work WorkItem { get; set; }

            public int MessageID => WorkItem.MessageID;
            public byte Slave => WorkItem.Data[0];
            public byte Command => WorkItem.Data[1];
            public string Message { get; private set; }

            public ReceivedEventArgs(Work WorkItem, string Message)
            {
                this.WorkItem = WorkItem;
                this.Message = Message;
            }
        }
        #endregion
        #region TimeoutEventArgs
        public class TimeoutEventArgs : EventArgs
        {
            private Work WorkItem { get; set; }

            public int MessageID => WorkItem.MessageID;
            public byte Slave => WorkItem.Data[0];
            public byte Command => WorkItem.Data[1];

            public TimeoutEventArgs(Work WorkItem)
            {
                this.WorkItem = WorkItem;
            }
        }
        #endregion
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
        public Encoding MessageEncoding { get; set; } = Encoding.ASCII;
        #endregion

        #region Member Variable
        private SerialPort ser = new SerialPort() { PortName = "COM1", BaudRate = 115200 };
        private Thread thAutoStart;
        #endregion

        #region Event
        public event EventHandler<ReceivedEventArgs> MessageReceived;
        public event EventHandler<TimeoutEventArgs> TimeoutReceived;

        public event EventHandler DeviceOpened;
        public event EventHandler DeviceClosed;
        #endregion

        #region Constructor
        public TextCommRTUMaster()
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

        #region Auto
        public void AutoSend(int MessageID, byte Slave, byte Command, string Message)
        {
            var ba = TextComm.MakePacket(MessageEncoding, Slave, Command, Message);
            AddAuto(new WorkTC(MessageID, ba, Slave, Command, Message));
        }
        #endregion
        #region Manual
        public void ManualSend(int MessageID, byte Slave, byte Command, string Message, int? repeatCount = null)
        {
            var ba = TextComm.MakePacket(MessageEncoding, Slave, Command, Message);
            AddManual(new WorkTC(MessageID, ba, Slave, Command, Message) { UseRepeat = repeatCount.HasValue, RepeatCount = (repeatCount.HasValue ? repeatCount.Value : 0) });
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
            TimeoutReceived?.Invoke(this, new TimeoutEventArgs(w));
        }
        #endregion
        #region OnCheckCollectComplete
        protected override bool OnCheckCollectComplete(byte[] data, int count, Work w)
        {
            return data[count - 1] == 0x03;
        }
        #endregion
        #region OnParsePacket
        protected override void OnParsePacket(byte[] baResponse, int nRecv, Work wi)
        {
            var ls = new List<byte>();
            #region Collect
            var bDLE = false;
            var bValid = true;
            bool bComplete = false;

            for (int i = 0; i < nRecv; i++)
            {
                byte d = (byte)baResponse[i];
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
            #endregion

            if (bValid && bComplete)
            {
                byte sum = (byte)(ls.GetRange(0, ls.Count - 1).Select(x => (int)x).Sum() & 0xFF);

                if (sum == ls[ls.Count - 1])
                {
                    var msg = MessageEncoding.GetString(ls.GetRange(2, ls.Count - 3).ToArray());
                    MessageReceived(this, new ReceivedEventArgs(wi, msg));
                }
            }
        }
        #endregion
        #endregion
    }
}
