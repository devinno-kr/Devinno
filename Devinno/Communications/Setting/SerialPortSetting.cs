using Devinno.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Communications.Setting
{
    public class SerialPortSetting
    {
        #region Properties
        public string Port { get; set; } = "COM1";
        public int Baudrate { get; set; } = 115200;
        public int DataBit { get; set; } = 8;
        public Parity Parity { get; set; } = Parity.None;
        public StopBits StopBit { get; set; } = StopBits.One;
        #endregion

        #region Method
        public void Save(string Path)
        {
            Serialize.JsonSerializeToFile(Path, this);
        }

        public void Load(string Path)
        {
            if (File.Exists(Path))
            {
                var v = Serialize.JsonDeserializeFromFile<SerialPortSetting>(Path);
                if (v != null)
                {
                    this.Port = v.Port;
                    this.Baudrate = v.Baudrate;
                    this.DataBit = v.DataBit;
                    this.Parity = v.Parity;
                    this.StopBit = v.StopBit;
                }
            }
        }

        public static SerialPortSetting FromFile(string Path)
        {
            return File.Exists(Path) ? Serialize.JsonDeserializeFromFile<SerialPortSetting>(Path) : null;
        }
        #endregion
    }
}
