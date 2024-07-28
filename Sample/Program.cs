using Devinno.Collections;
using Devinno.Communications.Modbus.TCP;
using Devinno.Communications.Mqtt;
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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var d = new WordMemories("D", 512);
            ModbusTCPSlave tcp = new ModbusTCPSlave { };
            tcp.WordAreas.Add(0x7000, d);
            tcp.Start();

            while (true)
            {
                d[0] = Convert.ToUInt16(DateTime.Now.Millisecond);
                Thread.Sleep(10);
            }
            /*
            var c = new MQClient { BrokerHostName = "127.0.0.1" };

            c.Start(Guid.NewGuid().ToString());

            while (!c.IsConnected) Thread.Sleep(1000);
            c.Subscribe("/sens/message");

            while (true)
            {
                c.Publish("TAG/DVFjTkacT2hM1LHm/1bf244bd-ad03-47ae-9855-21ecc86e4cd2/Time/SET", DateTime.Now.ToString());
                Thread.Sleep(1000);
            }
            */
        }

    }
}
