using Devinno.Communications.Modbus.TCP;
using Devinno.Communications.TextComm.TCP;
using System;
using System.Text;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            ModbusTCPMaster comm;
            comm = new ModbusTCPMaster() { RemoteIP = "127.0.0.1", AutoStart = true };
            comm.AutoWordRead_FC3(1, 1, 0x7000, 5);
            comm.WordReadReceived += (o, s) => { 
            };

            /*
            TextCommTCPMaster comm;
            comm = new TextCommTCPMaster() { RemotePort = 25851, MessageEncoding = Encoding.UTF8, AutoStart = false, BufferSize = 8 * 1024 * 1024 };
            comm.MessageReceived += Comm_MessageReceived;
            comm.TimeoutReceived += Comm_TimeoutReceived;
            comm.Timeout = 1000;
            comm.Interval = 1;
            comm.RemoteIP = "127.0.0.1";
            comm.AutoSend(4, 1, 4, "");
            comm.AutoStart = true;
            */

            while (true)
            {
                bool b = comm.IsOpen;
                if (!b) Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " : " + (b ? "T" : "F"));
                System.Threading.Thread.Sleep(10);
            }
        }
        private static void Comm_TimeoutReceived(object sender, TextCommTCPMaster.TimeoutEventArgs e)
        {
            try
            {
               
            }
            catch { }
        }

        private static void Comm_MessageReceived(object sender, TextCommTCPMaster.ReceivedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex) { }
        }
    }
}
