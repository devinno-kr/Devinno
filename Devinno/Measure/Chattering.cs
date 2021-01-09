using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Measure
{
    public class Chattering
    {
        #region Event
        public event EventHandler<ChatteringStateChangedEventArgs> StateChanged;
        #endregion

        #region Properties
        public bool State { get; private set; }
        public int ChatteringTime { get; set; }
        #endregion
        
        #region Member Variable
        bool prevState = false;
        DateTime prevTime = DateTime.Now;
        #endregion
        
        #region Constructor
        public Chattering()
        {
            State = false;
            ChatteringTime = 300;
        }
        #endregion

        #region Method
        #region Set
        public void Set(bool Value)
        {
            bool v = Value;
            if (this.prevState == v)
            {
                if ((DateTime.Now - prevTime).TotalMilliseconds >= this.ChatteringTime)
                {
                    if (this.State != v)
                    {
                        if (StateChanged != null) StateChanged.Invoke(this, new ChatteringStateChangedEventArgs(v));
                        this.State = v;
                    }
                }
            }
            else
            {
                this.prevState = v;
                prevTime = DateTime.Now;
            }
        }
        #endregion
        #endregion
    }

    #region [class] ChatteringStateChangedEventArgs
    public class ChatteringStateChangedEventArgs : EventArgs
    {
        public bool Value { get; private set; }

        public ChatteringStateChangedEventArgs(bool Value)
        {
            this.Value = Value;
        }
    }
    #endregion
}
