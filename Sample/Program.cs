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
            Console.WriteLine($"시작 시간:{DateTime.Now.ToString()}");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                LauncherTool.Start("Sample", () =>
                {
                    Console.WriteLine($"정상 실행:{DateTime.Now.ToString()}");
                }, () =>
                {
                    Console.WriteLine($"중복 실행:{DateTime.Now.ToString()}");
                }, true, 1000);

                Console.ReadKey();
            }
        }
    }
}
