using Devinno.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Communications.Redis
{
    public class RedisClient
    {
        #region Properties
        /// <summary>
        /// 호스트 주소
        /// </summary>
        public string Host { get; set; } = "127.0.0.1";

        /// <summary>
        /// 타임아웃 시간
        /// </summary>
        public int Timeout { get; set; } = 1000;

        /// <summary>
        /// 통신 시작 여부
        /// </summary>
        public bool IsStart { get; private set; }

        /// <summary>
        /// 접속 여부
        /// </summary>
        public bool IsConnected => conn != null && conn.IsConnected;

        public ConnectionMultiplexer Redis => conn;
        public IDatabase Database => db;
        public IServer Server => svr;
        #endregion

        #region Member Variable
        ConnectionMultiplexer conn = null;
        IDatabase db;
        IServer svr;

        System.Threading.Thread th;
        DateTime prev = DateTime.Now;
        #endregion

        #region Event
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        #endregion

        #region Constructor
        public RedisClient()
        {
        }
        #endregion

        #region Method
        #region Set
        public void Set(string key, string value) => SetValue(key, value);
        public void Set(string key, int value) => SetValue(key, value);
        public void Set(string key, uint value) => SetValue(key, value);
        public void Set(string key, double value) => SetValue(key, value);
        public void Set(string key, byte[] value) => SetValue(key, value);
        public void Set(string key, bool value) => SetValue(key, value);
        public void Set(string key, long value) => SetValue(key, value);
        public void Set(string key, ulong value) => SetValue(key, value);
        public void Set(string key, float value) => SetValue(key, value);

        public bool SetValue(string key, RedisValue value)
        {
            bool ret = false;
            if (conn != null && conn.IsConnected && db != null) ret = db.StringSet(key, value);
            return ret;
        }

        public void JsonSet(string key, object value) => SetValue(key, Serialize.JsonSerialize(value));
        #endregion
        #region Get
        public string GetString(string key) => (string)GetValue(key);
        public int GetInt32(string key) => (int)GetValue(key);
        public uint GetUInt32(string key) => (uint)GetValue(key);
        public double GetDouble(string key) => (double)GetValue(key);
        public byte[] GetBytes(string key) => (byte[])GetValue(key);
        public bool GetBoolean(string key) => (bool)GetValue(key);
        public long GetInt64(string key) => (long)GetValue(key);
        public ulong GetUInt64(string key) => (ulong)GetValue(key);
        public float GetSingle(string key) => (float)GetValue(key);

        public RedisValue GetValue(string key)
        {
            RedisValue ret = RedisValue.Null;
            if (conn != null && conn.IsConnected && db != null)
            {
                ret = db.StringGet(key);
            }
            return ret;
        }

        public T JsonGet<T>(string key) => Serialize.JsonDeserialize<T>(GetString(key));
        #endregion

        #region Hash
        #region Set
        public void HashSet(string key, HashEntry[] values)
        {
            if (conn != null && conn.IsConnected && db != null) db.HashSet(key, values);
        }
        #endregion
        #region Get
        public HashEntry[] HashGet(string key)
        {
            HashEntry[] ret = null;
            if (conn != null && conn.IsConnected && db != null)
            {
                ret = db.HashGetAll(key);
            }
            return ret;
        }

        public RedisValue HashGet(string key, string field)
        {
            RedisValue ret = RedisValue.Null;
            if (conn != null && conn.IsConnected && db != null)
            {
                ret = db.HashGet(key, field);
            }
            return ret;
        }
        #endregion
        #endregion

        #region KeyExists
        public bool? KeyExists(string key)
        {
            bool? ret = null;
            if (conn != null && conn.IsConnected && db != null)
            {
                ret = db.KeyExists(key);
            }
            return ret;
        }
        #endregion

        #region Start
        public void Start()
        {
            try
            {
                th = new System.Threading.Thread(new System.Threading.ThreadStart(run));
                th.IsBackground = true;
                th.Start();
            }
            catch (Exception) { }
        }
        #endregion
        #region Stop
        public void Stop()
        {
            IsStart = false;
        }
        #endregion

        #region Thread
        void run()
        {
            IsStart = true;
 
            while (IsStart)
            {
                if (conn == null || (conn != null && !conn.IsConnected))
                {
                    try
                    {
                        ConfigurationOptions conf = new ConfigurationOptions() { EndPoints = { Host }, ConnectTimeout = Timeout };
                        conn = ConnectionMultiplexer.Connect(conf);
                        db = conn.GetDatabase();

                        var endpoint = conn.GetEndPoints().Single();
                        svr = conn.GetServer(endpoint);

                    }
                    catch (Exception ex) { }
                }
                else
                {
                    if (db != null)
                    {
                        if (db.KeyExists("test-alive")) { var v = db.StringGet("test-alive"); }
                        else db.StringSet("test-alive", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        prev = DateTime.Now;
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }

            if (conn != null && conn.IsConnected)
            {
                try
                {
                    conn.Close();
                }
                catch (Exception ex) { }
            }

            svr = null;
            db = null;
            conn = null;

            
        }
        #endregion
        #endregion

    }

    #region attribute : RedisIgnore
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class RedisIgnoreAttribute : Attribute { }
    #endregion}
}
