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
        public ChatteringMode Mode { get; set; } = ChatteringMode.Both;
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
            if (Mode == ChatteringMode.Both)
            {
                #region Both
                bool v = Value;
                if (this.prevState == v)
                {
                    if ((DateTime.Now - prevTime).TotalMilliseconds >= this.ChatteringTime)
                    {
                        if (this.State != v)
                        {
                            StateChanged?.Invoke(this, new ChatteringStateChangedEventArgs(v));
                            this.State = v;
                        }
                    }
                }
                else
                {
                    this.prevState = v;
                    prevTime = DateTime.Now;
                }
                #endregion
            }
            else if (Mode == ChatteringMode.On)
            {
                #region High
                bool v = Value;
                if (!v)
                {
                    prevTime = DateTime.Now;
                    if (this.State != v)
                    {
                        StateChanged?.Invoke(this, new ChatteringStateChangedEventArgs(v));
                        this.State = v;
                    }
                    this.prevState = v;
                }
                else
                {
                    if (this.prevState != v)
                    {
                        prevTime = DateTime.Now;
                        this.prevState = v;
                    }
                    else
                    {
                        if ((DateTime.Now - prevTime).TotalMilliseconds >= this.ChatteringTime)
                        {
                            if (this.State != v)
                            {
                                StateChanged?.Invoke(this, new ChatteringStateChangedEventArgs(v));
                                this.State = v;
                            }
                        }
                    }
                }

                #endregion
            }
            else if (Mode == ChatteringMode.Off)
            {
                #region Off
                bool v = Value;
                if (v)
                {
                    prevTime = DateTime.Now;
                    if (this.State != v)
                    {
                        StateChanged?.Invoke(this, new ChatteringStateChangedEventArgs(v));
                        this.State = v;
                    }
                    this.prevState = v;
                }
                else
                {
                    if (this.prevState != v)
                    {
                        prevTime = DateTime.Now;
                        this.prevState = v;
                    }
                    else
                    {
                        if ((DateTime.Now - prevTime).TotalMilliseconds >= this.ChatteringTime)
                        {
                            if (this.State != v)
                            {
                                StateChanged?.Invoke(this, new ChatteringStateChangedEventArgs(v));
                                this.State = v;
                            }
                        }
                    }
                }
                #endregion
            }
        }
        #endregion
        #endregion
    }

    #region [enum] ChatteringMode
    public enum ChatteringMode { Both, On, Off }
    #endregion
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
