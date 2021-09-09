using System;
using System.Diagnostics;
using System.Threading;

namespace Devinno.Timers
{
    public class HiResTimer
    {
        #region Variable
        private static readonly float tickFrequency = 1000f / Stopwatch.Frequency;

        private volatile float interval;
        private volatile float ignoreElapsedThreshold = Single.PositiveInfinity;
        private volatile bool isRunning;
        #endregion

        #region Events
        public event EventHandler<HiResTimerElapsedEventArgs>? Elapsed;
        #endregion

        #region Properties
        public float Interval
        {
            get => interval;
            set
            {
                if (value < 0f || Single.IsNaN(value)) throw new ArgumentOutOfRangeException();
                interval = value;
            }
        }

        public float IgnoreElapsedThreshold
        {
            get => ignoreElapsedThreshold;
            set
            {
                if (value <= 0f || Single.IsNaN(value)) throw new ArgumentOutOfRangeException();
                ignoreElapsedThreshold = value;
            }
        }

        public bool Enabled
        {
            get => isRunning;
            set
            {
                if (value) Start();
                else Stop();
            }
        }
        #endregion

        #region Constructor
        public HiResTimer() : this(1f)
        {
        }

        public HiResTimer(float interval)
        {
            if (interval < 0f || Single.IsNaN(interval))
                throw new ArgumentOutOfRangeException();
            this.interval = interval;
        }
        #endregion

        #region Methods
        private static float ElapsedHiRes(Stopwatch stopwatch) => stopwatch.ElapsedTicks * tickFrequency;

        public void Start()
        {
            if (isRunning)
                return;

            isRunning = true;
            Thread thread = new Thread(ExecuteTimer) { Priority = ThreadPriority.Highest };
            thread.Start();
        }

        public void Stop() => isRunning = false;
        #endregion

        #region Private Methods
        private void ExecuteTimer()
        {
            int fallouts = 0;
            float nextTrigger = 0f;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (isRunning)
            {
                float intervalLocal = interval;
                nextTrigger += intervalLocal;
                float elapsed;

                while (true)
                {
                    elapsed = ElapsedHiRes(stopwatch);
                    float diff = nextTrigger - elapsed;
                    if (diff <= 0f)
                        break;

                    if (diff < 1f)
                        Thread.SpinWait(10);
                    else if (diff < 10f)
                        Thread.SpinWait(100);
                    else
                    {
                        if (diff >= 16f)
                            Thread.Sleep(diff >= 100f ? 50 : 1);
                        else
                        {
                            Thread.SpinWait(1000);
                            Thread.Sleep(0);
                        }

                        float newInterval = interval;

                        if (intervalLocal != newInterval)
                        {
                            nextTrigger += newInterval - intervalLocal;
                            intervalLocal = newInterval;
                        }
                    }

                    if (!isRunning)
                        return;
                }


                float delay = elapsed - nextTrigger;
                if (delay >= ignoreElapsedThreshold)
                {
                    fallouts += 1;
                    continue;
                }

                Elapsed?.Invoke(this, new HiResTimerElapsedEventArgs(delay, fallouts));
                fallouts = 0;

                if (stopwatch.Elapsed.TotalHours >= 1d)
                {
#if NET35
                    stopwatch.Reset();
                    stopwatch.Start();
#else
                    stopwatch.Restart();
#endif
                    nextTrigger = 0f;
                }
            }

            stopwatch.Stop();
        }
        #endregion
    }

    public class HiResTimerElapsedEventArgs : EventArgs
    {
        public float Delay { get; }
        public int Fallouts { get; }

        internal HiResTimerElapsedEventArgs(float delay, int fallouts)
        {
            Delay = delay;
            Fallouts = fallouts;
        }
    }
}
