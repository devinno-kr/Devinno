﻿using Devinno.Collections;
using Devinno.Communications.Modbus.TCP;
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
            /*
            ModbusTCPMaster mb = new ModbusTCPMaster { RemoteIP = "172.30.1.123", AutoStart = true };
            mb.AutoWordRead_FC3(1, 1, 0x7000, 10);
            mb.WordReadReceived += (o, s) =>
            {
                Console.CursorLeft = 0; 
                Console.CursorTop = 0; 
                Console.WriteLine(s.ReceiveData[0].ToString().PadLeft(5));
            };

            while (true)
            {
                var i = Convert.ToByte(DateTime.Now.Second % 16);
                mb.ManualWordWrite_FC6(1, 1, 0x7000, 0);
                mb.ManualWordBitSet_FC26(1, 1, 0x7000, i, true);
                Thread.Sleep(1000);
            }
            */

            /*
            ModbusTCPSlave mb = new ModbusTCPSlave { Slave = 1 };
            BitMemories P = new BitMemories("P", new byte[512]);
            BitMemories M = new BitMemories("M", new byte[512]);
            WordMemories C = new WordMemories("C", new byte[8192]);
            WordMemories D = new WordMemories("D", new byte[8192]);
            mb.BitAreas.Add(0x0000, P);
            mb.BitAreas.Add(0x1000, M);
            mb.WordAreas.Add(0x6000, C);
            mb.WordAreas.Add(0x7000, D);
            mb.Start();

            M[0] = true;
            M[2] = true;
            M[4] = true;
            */

            /*
            var comm = new TextCommRTUMaster { Port = "COM13", Baudrate = 115200, AutoStart = true };
            comm.MessageReceived += (o, s) =>
            {
                Console.WriteLine(s.Command + " : " + s.Message);
            };
            comm.AutoSend(1, 1, 4, "");
            byte cmd = 1;
            */

            /*
            var lns = File.ReadAllLines(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "tet.json")).Select(x => x.Trim());
            var lns2 = lns.Where(x => x.StartsWith("\"type\":")).Select(x => x.Split(':')).ToLookup(x => x[1]);
            var ks = string.Concat(lns2.Select(X => X.Key + ","));

            var via = new Dictionary<string, object>();
            via.Add("test1", 10);
            via.Add("test2", 3.141592);
            via.Add("test3", true);
            via.Add("test4", new int[] { 1, 2, 3 });
            via.Add("test5", "text");

            var s = Serialize.JsonSerialize(via);
            */

            var dic = new EventDictionary<int, string>();
            dic.Changed += (o, s) => Console.WriteLine(dic.Count.ToString());

            int i = 0;
            while (true)
            {
                dic.Add(i, "");
                Thread.Sleep(1000);
                i++;
            }
        }

        class V 
        {
            public short Data1;
            public string Data2;
            public float Data3;
        }
    }
}
