using Devinno.Communications.Modbus.TCP;
using Devinno.Communications.TextComm.TCP;
using Devinno.Database;
using Devinno.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(100);

            #region Loop
            while (true)
            {
                System.Threading.Thread.Sleep(100);
            }
            #endregion
        }

    }
    #region class : EquipmentDB
    public enum AlarmMode : int { NONE = 0, TC_OP = 1, TC_RE = 2, AL_H = 3, AL_L = 4, NOUSE = 5 }
    public class EquipmentDb : MyData
    {
        #region Properties
        public DateTime WorkTime { get; set; }

        public string? EqpName { get; set; }

        public bool? State1 { get; set; }
        public bool? State2 { get; set; }
        public bool? State3 { get; set; }

        public double? NowTemp1 { get; set; }
        public double? SetTemp1 { get; set; }
        public double? AlarmHi1 { get; set; }
        public double? AlarmLo1 { get; set; }
        public AlarmMode? Alarm1 { get; set; }

        public double? NowTemp2 { get; set; }
        public double? SetTemp2 { get; set; }
        public double? AlarmHi2 { get; set; }
        public double? AlarmLo2 { get; set; }
        public AlarmMode? Alarm2 { get; set; }

        public double? NowTemp3 { get; set; }
        public double? SetTemp3 { get; set; }
        public double? AlarmHi3 { get; set; }
        public double? AlarmLo3 { get; set; }
        public AlarmMode? Alarm3 { get; set; }

        public double? NowTemp4 { get; set; }
        public double? SetTemp4 { get; set; }
        public double? AlarmHi4 { get; set; }
        public double? AlarmLo4 { get; set; }
        public AlarmMode? Alarm4 { get; set; }

        public double? NowTemp5 { get; set; }
        public double? SetTemp5 { get; set; }
        public double? AlarmHi5 { get; set; }
        public double? AlarmLo5 { get; set; }
        public AlarmMode? Alarm5 { get; set; }

        public double? NowTemp6 { get; set; }
        public double? SetTemp6 { get; set; }
        public double? AlarmHi6 { get; set; }
        public double? AlarmLo6 { get; set; }
        public AlarmMode? Alarm6 { get; set; }

        public double? NowTemp7 { get; set; }
        public double? SetTemp7 { get; set; }
        public double? AlarmHi7 { get; set; }
        public double? AlarmLo7 { get; set; }
        public AlarmMode? Alarm7 { get; set; }

        public double? NowTemp8 { get; set; }
        public double? SetTemp8 { get; set; }
        public double? AlarmHi8 { get; set; }
        public double? AlarmLo8 { get; set; }
        public AlarmMode? Alarm8 { get; set; }

        public double? NowTemp9 { get; set; }
        public double? SetTemp9 { get; set; }
        public double? AlarmHi9 { get; set; }
        public double? AlarmLo9 { get; set; }
        public AlarmMode? Alarm9 { get; set; }

        public double? NowTemp10 { get; set; }
        public double? SetTemp10 { get; set; }
        public double? AlarmHi10 { get; set; }
        public double? AlarmLo10 { get; set; }
        public AlarmMode? Alarm10 { get; set; }
        #endregion
    }
    #endregion
}
