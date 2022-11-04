using Devinno.Communications.Modbus.TCP;
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
    var ba = new byte[1024];
    var M = new BitMemories("M", ba);
    var WM = new WordMemories("WM", ba);

    var s = "";
    WM[0] = 0x1234;
    for (int i = 0; i < 16; i++) s += (M[i] ? "1" : "0");

    Console.WriteLine($" M Count : {M.Size}");
    Console.WriteLine($"WM Count : {WM.Size}");
    Console.WriteLine("");
    Console.WriteLine($"WM000 = {WM[0].ToString("X4")}");
    Console.WriteLine($"M0:15 = {s}");

            Console.ReadKey();
        }
    }
}
