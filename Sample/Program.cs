using Devinno.Communications.Modbus.TCP;
using Devinno.Data;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            ModbusTCPSlaveBase tcp = new ModbusTCPSlaveBase();
            ModbusTCPSlaveBase tcp2 = new ModbusTCPSlaveBase();

            tcp.Start();
            Thread.Sleep(5000);
            tcp.Stop();
            Thread.Sleep(5000);
            tcp2.Start();
            Thread.Sleep(5000);
            tcp2.Stop();

            Console.ReadKey();
        }
    }
}
