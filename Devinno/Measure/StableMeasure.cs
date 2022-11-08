using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Measure
{
    public class StableMeasure
    {
        #region Event
        public event EventHandler<StableMeasureEventArgs> Measured;
        public event EventHandler<StableMeasureEventArgs> Measuring;
        #endregion

        #region Properties
        public double Value { get; private set; }
        public double ErrorRange { get; set; }
        public int MeasureTime { get; set; }
        #endregion

        #region Member Variable
        private bool IsComplete = false, IsStart = false;
        private DateTime StarTime = DateTime.MinValue, CompleteTIme = DateTime.MinValue;
        #endregion

        #region Constructor
        public StableMeasure()
        {
            Value = 0;
            ErrorRange = 1;
            MeasureTime = 1000;
        }
        #endregion

        #region Method
        #region Set
        public void Set(double Value)
        {
            var old = this.Value;
            #region Measuring
            if (this.Value != Value)
            {
                this.Value = Value;
                if (Measuring != null) Measuring.Invoke(this, new StableMeasureEventArgs(Value));
            }
            #endregion

            if (IsStart)
            {
                if (!IsComplete)
                {
                    #region Complete Start
                    if (Math.Abs(Value - old) <= ErrorRange)
                    {
                        IsComplete = true;
                        CompleteTIme = DateTime.Now;
                    }
                    #endregion
                }
                else
                {
                    #region Complete Instance
                    if (Math.Abs(Value - old) > ErrorRange) { IsComplete = false; }
                    else
                    {
                        var dv = (DateTime.Now - CompleteTIme).TotalMilliseconds;
                        if (dv > MeasureTime)
                        {
                            IsComplete = IsStart = false;
                            if (Measured != null) Measured.Invoke(this, new StableMeasureEventArgs(Value));
                        }
                    }
                    #endregion
                }
            }
            else
            {
                #region Start
                if (Math.Abs(old - Value) > ErrorRange)
                {
                    IsStart = true;
                    StarTime = DateTime.Now;
                }
                #endregion
            }
        }
        #endregion
        #endregion
    }

    #region [class] StableMeasureEventArgs
    public class StableMeasureEventArgs : EventArgs
    {
        public double Value { get; private set; }

        public StableMeasureEventArgs(double Value)
        {
            this.Value = Value;
        }
    }
    #endregion
}
