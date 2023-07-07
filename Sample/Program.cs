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
            ModbusTCPMaster mb = new ModbusTCPMaster { RemoteIP = "172.30.1.123", AutoStart = true };
            mb.AutoBitRead_FC1(1, 1, 0x1000, 40);
            mb.AutoWordRead_FC3(1, 1, 0x7000, 10);
            mb.WordReadReceived += (o, s) => { Console.WriteLine(DateTime.Now.ToString("ss.fff")); };
            mb.ManualWordBitSet_FC26(1, 1, 0x7000, 3, true);

            Console.ReadKey();
        }
    }
}
