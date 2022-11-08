using Devinno.Communications.Modbus.TCP;
using Devinno.Data;
using Devinno.Extensions;
using Devinno.Measure;
using Devinno.Timers;
using Devinno.Tools;
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
            var tmr = new HiResTimer { Interval = 10, Enabled = true };
            tmr.Elapsed += (o, s) => Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff"));

            Console.ReadKey();

            tmr.Enabled = false;
        }
    }
}
