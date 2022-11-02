using Devinno.Communications.Redis;
using Devinno.Communications.TextComm.RTU;
using Devinno.Communications.TextComm.TCP;
using Devinno.Timers;
using System;
using System.Threading;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = new RedisClient { Host = "127.0.0.1" };
            redis.Open();

            var tmr = new HiResTimer { Interval = 1000, Enabled = true };
            tmr.Elapsed += (o, s) => redis.Set("Time", DateTime.Now.ToString("HH:mm:ss.fff"));

            redis.Set("Time", DateTime.Now.ToString("HH:mm:ss.fff"));
            while (true)
            {
                Console.WriteLine(redis.GetString("Time"));
                System.Threading.Thread.Sleep(500);
            }
        }
    }
}
