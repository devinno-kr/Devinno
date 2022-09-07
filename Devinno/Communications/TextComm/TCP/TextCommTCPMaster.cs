using Devinno.Communications.Scheduler;
using Devinno.Extensions;
using Devinno.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Devinno.Communications.TextComm.TCP
{
    public class TextCommTCPMaster : MasterScheduler
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
            public byte Slave { get; private set; }
            public byte Command { get; private set; }
            public string Message { get; private set; }

            public ReceivedEventArgs(Work WorkItem, byte Slave, byte Command, string Message)
            {
                this.Slave = Slave;
                this.Command = Command;
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
        public string RemoteIP { get; set; } = "127.0.0.1";
        public int RemotePort { get; set; } = 7897;
        public Encoding MessageEncoding { get; set; } = Encoding.ASCII;
        public override bool IsOpen => bIsOpen;
        public bool AutoStart { get; set; }
        protected override int Available { get => client != null ? client.Available : 0; }
        #endregion

        #region Member Variable
        private Socket client;
        private Thread thAutoStart;

        private bool bIsOpen = false;
        #endregion

        #region Event
        public event EventHandler<ReceivedEventArgs> MessageReceived;
        public event EventHandler<TimeoutEventArgs> TimeoutReceived;

        public event EventHandler<SocketEventArgs> SocketConnected;
        public event EventHandler<SocketEventArgs> SocketDisconnected;
        #endregion

        #region Constructor
        public TextCommTCPMaster()
        {
            thAutoStart = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    if (AutoStart && !IsStartThread)
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

        private bool _Start()
        {
            bool ret = false;
            if (!IsOpen && !IsStart)
            {
                try
                {
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    client.ReceiveTimeout = Timeout;
                    client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, Timeout);
                    client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
                    client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                    //client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 2000);
                    //client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 1000);
                    //client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, 3);
                    client.Connect(RemoteIP, RemotePort);
                    SocketConnected?.Invoke(this, new SocketEventArgs(client));

                    bIsOpen = client.Connected;

                    ret = StartThread();
                    if (!ret && IsOpen) client.Close();
                }
                catch (Exception) { }
            }
            return ret;
        }

        private void _Stop()
        {
            StopThread();
            client = null;
        }
        #endregion

        #region Auto
        public void AutoSend(int MessageID, byte Slave, byte Command, string Message)
        {
            var ba = TextComm.MakePacket(MessageEncoding, Slave, Command, Message);
            AddAuto(new WorkTC(MessageID, ba, Slave, Command, Message));
        }
        #endregion
        #region Manual
        public void ManualSend(int MessageID, byte Slave, byte Command, string Message)
        {
            var ba = TextComm.MakePacket(MessageEncoding, Slave, Command, Message);
            AddManual(new WorkTC(MessageID, ba, Slave, Command, Message));
        }
        #endregion
        #endregion

        #region Override
        #region OnWrite
        protected override void OnWrite(byte[] data, int offset, int count, int timeout)
        {
            try
            {
                EndPoint ipep = new IPEndPoint(IPAddress.Parse(RemoteIP), RemotePort);
                client.SendTo(data, ipep);
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut) { }
                else if (ex.SocketErrorCode == SocketError.ConnectionReset) { }
                else if (ex.SocketErrorCode == SocketError.ConnectionAborted) { bIsOpen = false; }
            }
            catch { }

            if (!IsOpen) throw new SchedulerStopException();
        }
        #endregion
        #region OnRead
        protected override int? OnRead(byte[] data, int offset, int count, int timeout)
        {
            int? ret = null;

            try
            {
                if (client != null && client.Connected)
                {
                    client.ReceiveTimeout = Timeout;
                    client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, Timeout);

                    EndPoint ipep = new IPEndPoint(IPAddress.Parse(RemoteIP), RemotePort);

                    ret = client.ReceiveFrom(data, offset, count, SocketFlags.None, ref ipep);
                }
                else ret = 0;

                if (ret.HasValue) bIsOpen = ret.Value > 0;
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut) { }
                else if (ex.SocketErrorCode == SocketError.ConnectionReset) { }
                else if (ex.SocketErrorCode == SocketError.ConnectionAborted) { bIsOpen = false; }
            }
            catch { }

            if (!IsOpen) throw new SchedulerStopException();
            return ret;
        }
        #endregion
        #region OnFlush
        protected override void OnFlush()
        {

        }
        #endregion
        #region OnThreadEnd
        protected override void OnThreadEnd()
        {
            if (IsOpen) client.Close();
            bIsOpen = false;
            SocketDisconnected?.Invoke(this, new SocketEventArgs(client));
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
                    byte slave = ls[0];
                    byte cmd = ls[1];
                    var msg = MessageEncoding.GetString(ls.GetRange(2, ls.Count - 3).ToArray());
                    MessageReceived(this, new ReceivedEventArgs(wi, slave, cmd, msg));
                }
            }
        }
        #endregion
        #endregion
    }
}
