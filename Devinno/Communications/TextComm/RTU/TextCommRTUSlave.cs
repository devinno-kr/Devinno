using Devinno.Communications.Scheduler;
using Devinno.Communications.Setting;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Communications.TextComm.RTU
{
    public class TextCommRTUSlave : SlaveScheduler
    {
        #region class : EventArgs
        #region MessageRequestArgs
        public class MessageRequestArgs : EventArgs
        {
            public int Slave { get; private set; }
            public int Command { get; private set; }
            public string RequestMessage { get; private set; }
            public string ResponseMessage { get; set; }

            public MessageRequestArgs(int Slave, int Command, string Message)
            {
                this.Slave = Slave;
                this.Command = Command;
                this.RequestMessage = Message;
                this.ResponseMessage = null;
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
        public Encoding MessageEncoding { get; set; } = Encoding.ASCII;
        #endregion

        #region Event
        public event EventHandler<MessageRequestArgs> MessageRequest;

        public event EventHandler DeviceOpened;
        public event EventHandler DeviceClosed;
        #endregion

        #region Constructor
        public TextCommRTUSlave()
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
            bool bDLE = false, bValid = false, bComplete = false;
            foreach(var d in lstResponse)
            {
                var v = d;
                if (bDLE)
                {
                    bDLE = false;
                    if (v >= 0x10) v -= 0x10;
                    else bValid = false;
                }

                switch (d)
                {
                    #region STX
                    case 0x02:
                        lstResponse.Clear();
                        bValid = true;
                        break;
                    #endregion
                    #region ETX
                    case 0x03:
                        {
                            if (bValid)
                            {
                                if (lstResponse.Count >= 3)
                                {
                                    var sum = (byte)(lstResponse.GetRange(0, lstResponse.Count - 1).Select(x => (int)x).Sum() & 0xFF);
                                    if (sum == lstResponse[lstResponse.Count - 1])
                                    {
                                        byte slave = lstResponse[0];
                                        byte cmd = lstResponse[1];
                                        string msg = MessageEncoding.GetString(lstResponse.ToArray(), 2, lstResponse.Count - 3);

                                        if (MessageRequest != null)
                                        {
                                            var args = new MessageRequestArgs(slave, cmd, msg);
                                            MessageRequest?.Invoke(this, args);

                                            if (!string.IsNullOrEmpty(args.ResponseMessage))
                                            {
                                                var snd = TextComm.MakePacket(MessageEncoding, slave, cmd, args.ResponseMessage);
                                                ser.Write(snd.ToArray(), 0, snd.Length);
                                                ser.BaseStream.Flush();
                                            }
                                        }
                                    }
                                }
                                bComplete = true;
                            }
                        }
                        break;
                    #endregion
                    #region DLE
                    case 0x10:
                        bDLE = true;
                        break;
                    #endregion
                    #region Default
                    default:
                        lstResponse.Add(v);
                        break;
                        #endregion
                }
            }

            return bComplete && bValid;
        }
        #endregion
        #endregion
    }
}
