﻿using Devinno.Collections;
using Devinno.Communications.Modbus.TCP;
using Devinno.Communications.Redis;
using Devinno.Communications.Restful;
using Devinno.Communications.TextComm.RTU;
using Devinno.Data;
using Devinno.Extensions;
using Devinno.Measure;
using Devinno.Timers;
using Devinno.Tools;
using Devinno.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new RedisClient { Host = "127.0.0.1" };
            c.Start();
            Thread.Sleep(1000);

            c.Set("test_byte", new byte[] { 1, 2, 3, 4, 5 });
            var ba = c.GetBytes("test_byte");

            Console.ReadKey();

        }

    }
}
