using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Devinno.Communications.Scheduler
{
    public abstract class SlaveScheduler
    {
        #region Properties
        #region Abstract Properties
        protected abstract int Available { get; }
        public abstract bool IsOpen { get; }
        #endregion
        #region Properties
        public bool IsStart => IsStartThread;
        protected bool IsStartThread { get; private set; } = false;
        #endregion
        #endregion

        #region Member Variable
        private System.Threading.Thread th;
        private bool bFinished = false;
        private byte[] baResponse = new byte[1024 * 8];
        #endregion

        #region Construct
        public SlaveScheduler() { }
        #endregion

        #region Method
        #region Abstract Method
        protected abstract void OnWrite(byte[] data, int offset, int count, int timeout);
        protected abstract int? OnRead(byte[] data, int offset, int count, int timeout);
        protected abstract void OnClearBuffer();
        protected abstract bool OnParsePacket(List<byte> Response);
        protected abstract void OnThreadEnd();

        public abstract bool Start();
        public abstract void Stop();
        #endregion
        #region Method
        #region StartThread
        protected bool StartThread()
        {
            bool ret = false;
            if (!IsStartThread)
            {
                th = new System.Threading.Thread(new System.Threading.ThreadStart(WorkProcess));
                th.IsBackground = true;
                th.Start();
                ret = true;
            }
            return ret;
        }
        #endregion
        #region StopThread
        protected void StopThread()
        {
            IsStartThread = false;
            DateTime dt = DateTime.Now;
            while (!bFinished && (DateTime.Now - dt).TotalMilliseconds < 3000) System.Threading.Thread.Sleep(100);
        }
        #endregion
        #endregion
        #region Thread
        void WorkProcess()
        {
            List<byte> lstResponse = new List<byte>();
            var prev = DateTime.Now;

            bFinished = false;
            IsStartThread = true;

            OnClearBuffer();

            while (IsStartThread)
            {
                if (IsOpen)
                {
                    try
                    {
                        #region DataRead
                        if (Available > 0)
                        {
                            var len = OnRead(baResponse, 0, baResponse.Length, 1000);
                            if (len.HasValue)
                            {
                                for (int i = 0; i < len.Value; i++) lstResponse.Add(baResponse[i]);
                            }
                            prev = DateTime.Now;
                        }
                        #endregion

                        #region Parse
                        if (OnParsePacket(lstResponse))
                        {
                            OnClearBuffer();
                            lstResponse.Clear();
                        }
                        #endregion

                        #region Buffer Clear
                        if ((DateTime.Now - prev).TotalMilliseconds >= 20 && lstResponse.Count > 0)
                        {
                            OnClearBuffer();
                            lstResponse.Clear();
                        }
                        #endregion
                    }
                    catch (SchedulerStopException) { }
                }
                else IsStartThread = false;
                Thread.Sleep(1);
            }

            bFinished = true;

            OnThreadEnd();
        }
        #endregion
        #endregion
    }
}
