﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Communications.Scheduler
{
    public abstract class MasterScheduler
    {
        #region Class : Work
        public class Work
        {
            public int MessageID { get; private set; }
            public byte[] Data { get; private set; }

            public int? RepeatCount { get; set; } = null;
            public int? Timeout { get; set; } = null;

            public Work(int ID, byte[] Data)
            {
                this.MessageID = ID;
                this.Data = Data;
            }
        }
        #endregion

        #region Properties
        #region Abstract Properties
        protected abstract int Available { get; }
        public abstract bool IsOpen { get; }
        #endregion
        #region Properties
        public int Timeout { get; set; } = 100;
        public int Interval { get; set; } = 10;
        public int BufferSize { get; set; } = 1024;
        protected bool IsStartThread { get; private set; } = false;
        public bool IsStart => IsStartThread;
        #endregion
        #endregion

        #region Member Variable
        private System.Threading.Thread th;
        private bool bFinished = false;

        private Queue<Work> WorkQueue = new Queue<Work>();
        private List<Work> AutoWorkList = new List<Work>();
        private List<Work> ManualWorkList = new List<Work>();
        protected byte[] baResponse = new byte[1024];
        #endregion

        #region Abstract Method
        protected abstract void OnWrite(byte[] data, int offset, int count, int timeout);
        protected abstract int? OnRead(byte[] data, int offset, int count, int timeout);
        protected abstract void OnFlush();
        protected abstract bool OnCheckCollectComplete(byte[] data, int count, Work w);
        protected abstract void OnParsePacket(byte[] data, int count, Work w);
        protected abstract void OnThreadEnd();

        public abstract bool Start();
        public abstract void Stop();
        #endregion

        #region Virtual Method
        public virtual void OnTimeout(Work w)
        {
        }
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
            if (IsStartThread)
            {
                IsStartThread = false;
                DateTime dt = DateTime.Now;
                while (!bFinished && (DateTime.Now - dt).TotalMilliseconds < 3000) System.Threading.Thread.Sleep(100);
            }
        }
        #endregion

        #region Schedule
        #region ContainAutoID
        public bool ContainAutoID(int MessageID)
        {
            bool ret = false;
            for (int i = AutoWorkList.Count - 1; i >= 0; i--)
            {
                if (AutoWorkList[i].MessageID == MessageID)
                {
                    ret = true;
                }
            }
            return ret;
        }
        #endregion
        #region RemoveManual
        public bool RemoveManual(int MessageID)
        {
            bool ret = false;
            for (int i = ManualWorkList.Count - 1; i >= 0; i--)
            {
                if (ManualWorkList[i].MessageID == MessageID)
                {
                    ManualWorkList.RemoveAt(i);
                    ret = true;
                }
            }
            return ret;
        }
        #endregion
        #region RemoveAuto
        public bool RemoveAuto(int MessageID)
        {
            bool ret = false;
            for (int i = AutoWorkList.Count - 1; i >= 0; i--)
            {
                if (AutoWorkList[i].MessageID == MessageID)
                {
                    AutoWorkList.RemoveAt(i);
                    ret = true;
                }
            }
            return ret;
        }
        #endregion
        #region Clear
        public void ClearManual() { ManualWorkList.Clear(); }
        public void ClearAuto() { AutoWorkList.Clear(); }
        public void ClearWorkSchedule() { WorkQueue.Clear(); }
        #endregion
        #region Add
        protected void AddAuto(Work w) { AutoWorkList.Add(w); }
        protected void AddManual(Work w) { ManualWorkList.Add(w); }
        #endregion
        #endregion
        #endregion

        #region Thread
        void WorkProcess()
        {
            baResponse = new byte[BufferSize];

            bFinished = false;
            IsStartThread = true;

            while (IsStartThread)
            {
                if (IsOpen)
                {
                    if (WorkQueue.Count > 0 || ManualWorkList.Count > 0)
                    {
                        Work w = null;
                        #region Get Work
                        if (ManualWorkList.Count > 0)
                        {
                            w = ManualWorkList[0];
                            ManualWorkList.RemoveAt(0);
                        }
                        else w = WorkQueue.Dequeue();
                        #endregion
                        #region Process
                        if (w != null)
                        {
                            #region Default Value
                            bool bRepeat = true;
                            int nTimeoutCount = 0;
                            int Timeout = w.Timeout ?? this.Timeout;
                            #endregion

                            try
                            {
                                while (bRepeat)
                                {
                                    #region Write
                                    OnWrite(w.Data, 0, w.Data.Length, Timeout);
                                    OnFlush();
                                    #endregion
                                    #region Variable
                                    var prev = DateTime.Now;
                                    double gap = 0;
                                    int nRecv = 0;
                                    #endregion
                                    #region Receive
                                    bool Collecting = true;
                                    while (Collecting)
                                    {
                                        if (Available > 0)
                                        {
                                            var len = OnRead(baResponse, nRecv, baResponse.Length - nRecv, Timeout);
                                            if (len.HasValue)
                                            {
                                                nRecv += len.Value;
                                                if (OnCheckCollectComplete(baResponse, nRecv, w))
                                                {
                                                    bRepeat = false;
                                                    break;
                                                }
                                            }
                                        }

                                        gap = (DateTime.Now - prev).TotalMilliseconds;
                                        if (gap >= Timeout) break;
                                    }
                                    #endregion
                                    #region Analyze
                                    if (gap < Timeout)
                                    {
                                        #region Parse
                                        OnParsePacket(baResponse, nRecv, w);
                                        bRepeat = false;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Timeout
                                        OnTimeout(w);
                                        if (w.RepeatCount.HasValue)
                                        {
                                            nTimeoutCount++;
                                            if (nTimeoutCount >= w.RepeatCount.Value) bRepeat = false;
                                        }
                                        else bRepeat = false;
                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                            catch (SchedulerStopException) { }
                        }
                        #endregion
                    }
                    else
                    {
                        #region Fill Work
                        for (int i = 0; i < AutoWorkList.Count; i++) WorkQueue.Enqueue(AutoWorkList[i]);
                        #endregion
                    }
                }
                else IsStartThread = false;
                System.Threading.Thread.Sleep(Interval);
            }

            bFinished = true;

            OnThreadEnd();
        }
        #endregion

    }
}
