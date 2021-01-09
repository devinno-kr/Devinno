using Devinno.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Communications.Setting
{
    public class IPSetting
    {
        #region Properties
        public int Port { get; set; } = 80;
        public string IP { get; set; } = "127.0.0.1";
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
                var v = Serialize.JsonDeserializeFromFile<IPSetting>(Path);
                if (v != null)
                {
                    this.Port = v.Port;
                    this.IP = v.IP;
                }
            }
        }

        public static IPSetting FromFile(string Path)
        {
            return File.Exists(Path) ? Serialize.JsonDeserializeFromFile<IPSetting>(Path) : null;
        }
        #endregion
    }
}
