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
        public string Host { get; set; } = "127.0.0.1";
        public int Timeout { get; set; } = 1000;
        public bool IsConnected => conn != null && conn.IsConnected;
        #endregion

        #region Member Variable
        ConnectionMultiplexer conn = null;
        IDatabase db;
        #endregion

        #region Constructor
        public RedisClient()
        {
        }
        #endregion

        #region Method
        #region Set
        public void Set(string key, int value) => Set(key, value);
        public void Set(string key, long value) => Set(key, value);
        public void Set(string key, string value) => Set(key, value);
        public void Set(string key, byte[] value) => Set(key, value);

        public void Set(string key, RedisValue value)
        {
            if (conn != null && conn.IsConnected && db != null)
            {
                db.StringSet(key, value);
            }
        }
        #endregion
        #region Get
        public long GetInt64(string key) => (long)Get(key);
        public int GetInt32(string key) => (int)Get(key);
        public string GetString(string key) => (string)Get(key);
        public  byte[] GetBytes(string key) => (byte[])Get(key);

        public RedisValue Get(string key)
        {
            string ret = null;
            if (conn != null && conn.IsConnected && db != null)
            {
                ret = db.StringGet(key);
            }
            return ret;
        }
        #endregion

        #region Open
        public void Open()
        {
            ConfigurationOptions conf = new ConfigurationOptions() { EndPoints = { Host }, ConnectTimeout = Timeout };
            conn = ConnectionMultiplexer.Connect(conf);
            db = conn.GetDatabase();
        }
        #endregion
        #region Close
        public void Close()
        {
            if (conn != null) conn.Dispose();
        }
        #endregion
        #endregion

    }

}
