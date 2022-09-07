using Devinno.Communications.Modbus.TCP;
using Devinno.Communications.TextComm.TCP;
using Devinno.Tools;
using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = ColorTool.GetName(Color.Red, ColorCodeType.ARGB);
            #region Loop
            while (true)
            {
                System.Threading.Thread.Sleep(100);
            }
            #endregion
        }

    }

}
