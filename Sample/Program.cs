using Devinno.Communications.TextComm.TCP;
using System;
using System.Text;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            TextCommTCPSlave comm;
            comm = new TextCommTCPSlave() { LocalPort = 7585, MessageEncoding = Encoding.UTF8 };
            comm.MessageRequest += Comm_MessageRequest; 
            comm.Start();

            while(true)
            {

                System.Threading.Thread.Sleep(100);
            }
        }

        private static void Comm_MessageRequest(object sender, TextCommTCPSlave.MessageRequestArgs e)
        {
        }
    }
}
