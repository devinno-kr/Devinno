﻿using Devinno.Communications.Modbus.TCP;
using Devinno.Communications.Restful;
using Devinno.Data;
using Devinno.Extensions;
using Devinno.Measure;
using Devinno.Timers;
using Devinno.Tools;
using Devinno.Utils;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

            while (true)
            {
                 
                Thread.Sleep(1000);
            }
        }
    }
}
