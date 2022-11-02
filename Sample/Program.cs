using Devinno.Data;
using Devinno.Extensions;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Sample
{
    class Program
    {
        static string PER(double v) => v.ToString("0%");
        static void Main(string[] args)
        {
            var c = Color.LimeGreen;
            var h = c.ToHSV();
            Console.WriteLine($"RGB({c.R}, {c.G}, {c.B}) => HSV({h.H}, {PER(h.S)}, {PER(h.V)})");

            var h2 = new HsvColor() { A = 1, H = 200, S = 0.45, V = 0.6 };
            var c2 = h2.ToRGB();
            Console.WriteLine($"HSV({h2.H}, {PER(h2.S)}, {PER(h2.V)}) => RGB({c2.R}, {c2.G}, {c2.B})");

            Console.ReadKey();
        }
    }
}
