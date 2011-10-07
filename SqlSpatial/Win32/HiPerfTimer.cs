namespace Win32
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal class HiPerfTimer
    {
        private long freq;
        private long startTime = 0L;
        private long stopTime = 0L;

        public HiPerfTimer()
        {
            if (!QueryPerformanceFrequency(out this.freq))
            {
                throw new Win32Exception();
            }
        }

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);
        public void Start()
        {
            Thread.Sleep(0);
            QueryPerformanceCounter(out this.startTime);
        }

        public void Stop()
        {
            QueryPerformanceCounter(out this.stopTime);
        }

        public double CurrentDuration
        {
            get
            {
                long lpPerformanceCount = 0L;
                QueryPerformanceCounter(out lpPerformanceCount);
                return (((double) (lpPerformanceCount - this.startTime)) / ((double) this.freq));
            }
        }

        public double Duration
        {
            get
            {
                return (((double) (this.stopTime - this.startTime)) / ((double) this.freq));
            }
        }
    }
}

