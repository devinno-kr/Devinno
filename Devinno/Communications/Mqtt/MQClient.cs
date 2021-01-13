using Devinno.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Devinno.Communications.Mqtt
{
    #region enum : MQQos
    public enum MQQos : byte
    {
        LeastOnce = MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
        MostOnce = MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
        ExactlyOnce = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
        GrantedFailure = MqttMsgBase.QOS_LEVEL_GRANTED_FAILURE
    }
    #endregion
    #region class : MQSubscribe
    public class MQSubscribe
    {
        public string Topic { get; private set; }
        public MQQos Qos { get; private set; }
        public Type ParseType { get; private set; }

        public MQSubscribe(string Topic) { this.Topic = Topic; this.Qos = MQQos.ExactlyOnce; this.ParseType = null; }
        public MQSubscribe(string Topic, MQQos Qos) { this.Topic = Topic; this.Qos = Qos; this.ParseType = null; }
        public MQSubscribe(string Topic, Type ParseType) { this.Topic = Topic; this.Qos = MQQos.ExactlyOnce; this.ParseType = ParseType; }
        public MQSubscribe(string Topic, MQQos Qos, Type ParseType) { this.Topic = Topic; this.Qos = Qos; this.ParseType = ParseType; }
    }
    #endregion
    #region class : MQReceiveArgs
    public class MQReceiveArgs : EventArgs
    {
        public string Topic { get; private set; }
        public byte[] Datas { get; private set; }

        public bool IsParse { get; private set; }
        public Type ParseType { get; private set; }
        public object ParseObject { get; private set; }

        public MQReceiveArgs(string Topic, byte[] Datas)
        {
            this.Topic = Topic;
            this.Datas = Datas;
            this.IsParse = false;
        }

        public MQReceiveArgs(MQSubscribe sub, byte[] Data)
        {
            this.Topic = sub.Topic;
            this.Datas = Data;
            this.IsParse = true;
            if (sub.ParseType != null)
            {
                IsParse = true;
                ParseType = sub.ParseType;
                ParseObject = Serialize.JsonDeserialize(Encoding.UTF8.GetString(Data), sub.ParseType);
            }
        }
    }
    #endregion

    public class MQClient
    {
        #region Properties
        /// <summary>
        /// 브로커 주소
        /// </summary>
        public string BrokerHostName { get; set; } = "127.0.0.1";
        
        /// <summary>
        /// 통신 시작 여부
        /// </summary>
        public bool IsStart { get; private set; }
        
        /// <summary>
        /// 브로커 접속 여부
        /// </summary>
        public bool IsConnected => client != null ? client.IsConnected : false;
        
        /// <summary>
        /// 구독중인 토픽
        /// </summary>
        public MQSubscribe[] SubscribeArray => Subscribes.Values.ToArray();
        #endregion

        #region Member Variable
        MqttClient client;
        Dictionary<string, MQSubscribe> Subscribes = new Dictionary<string, MQSubscribe>();

        string strClientID = null, strUserName = null, strPassword = null;
        System.Threading.Thread th;
        DateTime prev = DateTime.Now;
        #endregion

        #region Event
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<MQReceiveArgs> Received;
        #endregion

        #region Constructor
        public MQClient()
        {
            //System.Windows.Forms.Application.ApplicationExit += (o, s) => { IsStart = false; };
        }
        #endregion

        #region EventHandler
        #region client_ConnectionClosed
        void client_ConnectionClosed(object sender, EventArgs e) => Disconnected?.Invoke(this, new EventArgs());
        #endregion
        #region client_MqttMsgPublishReceived
        void client_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            try
            {
                if (Subscribes.ContainsKey(e.Topic))
                {
                    Received?.Invoke(this, new MQReceiveArgs(Subscribes[e.Topic], e.Message));
                }
                else
                {
                    Received?.Invoke(this, new MQReceiveArgs(e.Topic, e.Message));
                }
            }
            catch (Exception ex) { }
        }
        #endregion
        #endregion

        #region Method
        #region Start(ClientID)
        /// <summary>
        /// 통신 시작
        /// </summary>
        /// <param name="ClientID">클라이언트 ID</param>
        public void Start(string ClientID)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ClientID) && !IsStart)
                {
                    this.strClientID = ClientID;
                    this.strUserName = null;
                    this.strPassword = null;

                    th = new System.Threading.Thread(new System.Threading.ThreadStart(run));
                    th.IsBackground = true;
                    th.Start();
                }
            }
            catch (Exception) { }
        }
        #endregion
        #region Start(ClientID, UserName, Password)
        /// <summary>
        /// 통신 시작
        /// </summary>
        /// <param name="ClientID">클라이언트 ID</param>
        /// <param name="UserName">사용자 이름</param>
        /// <param name="Password">암호</param>
        public void Start(string ClientID, string UserName, string Password)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ClientID) && !IsStart)
                {
                    this.strClientID = ClientID;
                    this.strUserName = UserName;
                    this.strPassword = Password;

                    th = new System.Threading.Thread(new System.Threading.ThreadStart(run));
                    th.IsBackground = true;
                    th.Start();
                }
            }
            catch (Exception ex) { }
        }
        #endregion

        #region Stop
        /// <summary>
        /// 통신 정지
        /// </summary>
        public void Stop()
        {
            IsStart = false;
        }
        #endregion

        #region Publish(Topic, byte[])
        /// <summary>
        /// 메시지 발행
        /// </summary>
        /// <param name="Topic">토픽</param>
        /// <param name="Data">byte[] 데이터</param>
        public void Publish(string Topic, byte[] Data)
        {
            if (client != null && client.IsConnected)
                client.Publish(Topic, Data);
        }
        #endregion
        #region Publish(Topic, String)
        /// <summary>
        /// 메시지 발행
        /// </summary>
        /// <param name="Topic">토픽</param>
        /// <param name="Data">string 데이터</param>
        public void Publish(string Topic, string Data)
        {
            Publish(Topic, Encoding.UTF8.GetBytes(Data));
        }
        #endregion
        #region Publish(Topic, Object)
        /// <summary>
        /// 메시지 발행
        /// </summary>
        /// <param name="Topic">토픽</param>
        /// <param name="Data">Object 데이터</param>
        public void Publish(string Topic, object Data)
        {
            Publish(Topic, Serialize.JsonSerialize(Data));
        }
        #endregion

        #region Subscribe
        /// <summary>
        /// 메시지 구독
        /// </summary>
        /// <param name="Sub">구독 정보</param>
        public void Subscribe(MQSubscribe Sub)
        {
            Subscribes.Add(Sub.Topic, Sub);
            if (client != null && client.IsConnected)
            {
                client.Subscribe(new string[] { Sub.Topic }, new byte[] { (byte)Sub.Qos });
            }
        }
        #endregion
        #region Subscribe(Topic)
        /// <summary>
        /// 메시지 구독
        /// </summary>
        /// <param name="Topic">토픽</param>
        public void Subscribe(string Topic)
        {
            Subscribe(new MQSubscribe(Topic));
        }
        #endregion
        #region Subscribe(Topic, Qos)
        /// <summary>
        /// 메시지 구독
        /// </summary>
        /// <param name="Topic">토픽</param>
        /// <param name="Qos">Qos</param>
        public void Subscribe(string Topic, MQQos Qos)
        {
            Subscribe(new MQSubscribe(Topic, Qos));
        }
        #endregion
        #region Subscribe(Topic, ParseType)
        /// <summary>
        /// 메시지 구독
        /// </summary>
        /// <param name="Topic"></param>
        /// <param name="ParseType"></param>
        public void Subscribe(string Topic, Type ParseType)
        {
            Subscribe(new MQSubscribe(Topic, ParseType));
        }
        #endregion
        #region Subscribe(Topic, Qos, ParseType)
        /// <summary>
        /// 메시지 구독
        /// </summary>
        /// <param name="Topic">토픽</param>
        /// <param name="Qos">Qos</param>
        /// <param name="ParseType">변환 타입</param>
        public void Subscribe(string Topic, MQQos Qos, Type ParseType)
        {
            Subscribe(new MQSubscribe(Topic, Qos, ParseType));
        }
        #endregion
        #region Subscribe<T>(Topic)
        /// <summary>
        /// 메시지 구독
        /// </summary>
        /// <typeparam name="T">변환 타입</typeparam>
        /// <param name="Topic">토픽</param>
        public void Subscribe<T>(string Topic)
        {
            Subscribe(Topic, typeof(T));
        }
        #endregion
        #region Subscribe<T>(Topic, Qos)
        /// <summary>
        /// 메시지 구독
        /// </summary>
        /// <typeparam name="T">변환 타입</typeparam>
        /// <param name="Topic">토픽</param>
        /// <param name="Qos">Qos</param>
        public void Subscribe<T>(string Topic, MQQos Qos)
        {
            Subscribe(Topic, Qos, typeof(T));
        }
        #endregion

        #region Unsubscribe(Topic)
        /// <summary>
        /// 구독 해지
        /// </summary>
        /// <param name="Topic">토픽</param>
        public void Unsubscribe(string Topic)
        {
            if (Subscribes.ContainsKey(Topic))
            {
                Subscribes.Remove(Topic);

                if (client != null && client.IsConnected)
                {
                    client.Unsubscribe(new string[] { Topic });
                }
            }
        }
        #endregion
        #region UnsubscribeClear
        /// <summary>
        /// 구독 전체 해지
        /// </summary>
        public void UnsubscribeClear()
        {
            if (Subscribes.Count > 0)
            {
                var tops = Subscribes.Values.Select(x => x.Topic).ToArray();
                Subscribes.Clear();
                if (client != null && client.IsConnected)
                {
                    client.Unsubscribe(tops);
                }
            }
        }
        #endregion
        #endregion

        #region Test
        /// <summary>
        /// 브로커 접속 테스트
        /// </summary>
        /// <param name="BrokerIP">브로커 주소</param>
        /// <param name="ClientID">클라이언트 ID</param>
        /// <returns>테스트 결과</returns>
        public static bool Test(string BrokerIP, string ClientID)
        {
            bool ret = false;
            try
            {
                var client = new uPLibrary.Networking.M2Mqtt.MqttClient(BrokerIP);
                if (client.Connect(ClientID) == 0)
                {
                    ret = true;
                    System.Threading.Thread.Sleep(500);
                    client.Disconnect();
                }
            }
            catch (Exception) { }
            return ret;
        }
        #endregion

        #region Thread
        void run()
        {
            IsStart = true;
            client = new MqttClient(BrokerHostName);

            while (IsStart)
            {
                if (!client.IsConnected)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(strPassword) && !string.IsNullOrWhiteSpace(strUserName))
                        {
                            if (client.Connect(strClientID, strUserName, strPassword) == 0)
                            {
                                Connected?.Invoke(this, null);

                                #region Subscribe
                                if (Subscribes.Count > 0)
                                {
                                    var ts = Subscribes.Values.Select(x => x.Topic).ToArray();
                                    var qs = Subscribes.Values.Select(x => (byte)x.Qos).ToArray();
                                    client.Subscribe(ts, qs);
                                }
                                #endregion

                                client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                            }
                        }
                        else
                        {
                            if (client.Connect(strClientID) == 0)
                            {
                                Connected?.Invoke(this, null);

                                #region Subscribe
                                if (Subscribes.Count > 0)
                                {
                                    var ts = Subscribes.Values.Select(x => x.Topic).ToArray();
                                    var qs = Subscribes.Values.Select(x => (byte)x.Qos).ToArray();
                                    client.Subscribe(ts, qs);
                                }
                                #endregion

                                client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                            }
                        }
                    }
                    catch (Exception ex) { }
                    System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    if ((DateTime.Now - prev).TotalSeconds > 1)
                    {
                        client.Publish("/test-alive", new byte[] { 0 });
                        prev = DateTime.Now;
                    }
                    System.Threading.Thread.Sleep(10);
                }
            }

            if (client != null && client.IsConnected)
            {
                try
                {
                    client.Disconnect();
                }
                catch (Exception ex) { }
            }
            client = null;
        }
        #endregion
    }
}
