using Devinno.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Data
{
    [SupportedOSPlatform("windows")]
    public class INI
    {
        #region Properties
        public string Path { get; private set; }
        #endregion

        #region Constructor
        public INI(string Path) => this.Path = Path;
        #endregion

        #region Method
        public void DeleteSection(string strSection) => Win32Tool.WritePrivateProfileString(strSection, null, null, Path);

        public bool ExistsINI() => File.Exists(Path);

        public void Write(string Section, string Key, string Value) => Win32Tool.WritePrivateProfileString(Section, Key, Value, Path);

        public string Read(string Section, string Key)
        {
            StringBuilder strValue = new StringBuilder();
            int i = Win32Tool.GetPrivateProfileString(Section, Key, "", strValue, 255, Path);
            return strValue.ToString();
        }
        #endregion
    }
}
