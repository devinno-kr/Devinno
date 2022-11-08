using Devinno.Communications.Modbus.TCP;
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
            var v = new ExternalProgram("calc.exe");
            v.Start();
            Thread.Sleep(5000);
            v.Stop();

            Console.ReadKey();
        }
    }
}
