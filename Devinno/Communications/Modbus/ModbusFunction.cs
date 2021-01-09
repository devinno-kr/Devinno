using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Communications.Modbus
{
    public enum ModbusFunction
    {
        BITREAD_F1 = 1,
        BITREAD_F2 = 2,
        WORDREAD_F3 = 3,
        WORDREAD_F4 = 4,
        BITWRITE_F5 = 5,
        WORDWRITE_F6 = 6,
        MULTIBITWRITE_F15 = 15,
        MULTIWORDWRITE_F16 = 16,
        WORDBITSET_F26 = 26,
    }
}
