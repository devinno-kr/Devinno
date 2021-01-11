﻿using Devinno.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Devinno.Communications.TextComm.TCP
{
    public class TextCommTCPSlave
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

        #region Properties
        public int LocalPort { get; set; } = 7897;
        public bool IsStart { get; private set; }
        public Encoding MessageEncoding { get; set; } = Encoding.ASCII;
        #endregion

        #region Event
        public event EventHandler<MessageRequestArgs> MessageRequest;
        public event EventHandler<SocketEventArgs> SocketConnected;
        public event EventHandler<SocketEventArgs> SocketDisconnected;
        #endregion

        #region Member Variable
        private byte[] baResponse = new byte[1024 * 8];

        private Socket server;
        private Thread th;
        #endregion

        #region Construct
        public TextCommTCPSlave() { }
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

                var lstResponse = new List<byte>();
                var prev = DateTime.Now;
                bool bDLE = false, bValid = false;
                bool IsThStart = true;
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
                                for (int i = 0; i < n; i++)
                                {
                                    var d = baResponse[i];
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
                                                                MessageRequest.Invoke(this, args);

                                                                if (!string.IsNullOrEmpty(args.ResponseMessage))
                                                                    server.Send(TextComm.MakePacket(MessageEncoding, slave, cmd, args.ResponseMessage));
                                                            }
                                                        }
                                                    }
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
                            }
                            catch (TimeoutException) { }
                        }
                        #endregion

                        #region Buffer Clear
                        if ((DateTime.Now - prev).TotalMilliseconds >= 20 && lstResponse.Count > 0) lstResponse.Clear();
                        #endregion

                        IsThStart = NetworkTool.IsSocketConnected(server);
                    }
                    catch (Exception) { }
                    Thread.Sleep(1);
                }

                #region Socket Closed
                if (NetworkTool.IsSocketConnected(server)) server.Close();
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
    }
}
