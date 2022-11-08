using Devinno.Communications.Modbus.TCP;
using Devinno.Data;
using Devinno.Extensions;
using Devinno.Measure;
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
            var stable = new StableMeasure { MeasureTime = 300, ErrorRange = 0 };
            stable.Measured += (o, s) => Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : " + stable.Value);

            var v = 0.0;
            var r = new Random();
            while (true)
            {
                var now = DateTime.Now;

                if (now.Second % 5 != 0) v = MathTool.Constrain(v + (r.Next() % 2 == 0 ? 1 : -1), -500, 500);
                stable.Set(v);

                //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : " + stable.Value);

                Thread.Sleep(10);
            }

        }
    }
}
