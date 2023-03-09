using Devinno.Communications.Modbus.TCP;
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
            ModbusTCPMaster mb = new ModbusTCPMaster { RemoteIP = "192.168.0.109", AutoStart = true };
            mb.AutoBitRead_FC1(1, 1, 0x1000, 30);
            mb.AutoWordRead_FC3(1, 1, 0x7000 + 100, 30);
            mb.WordReadReceived += (o, s) => { Console.WriteLine(DateTime.Now.ToString("ss.fff")); };


            Console.ReadKey();
        }
    }
}
