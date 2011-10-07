namespace SqlSpatial
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    internal class QueryPerfCounter
    {
        private long frequency;
        private decimal multiplier = 1000000000M;
        private long start;
        private long stop;

        public QueryPerfCounter()
        {
            if (!QueryPerformanceFrequency(out this.frequency))
            {
                throw new Win32Exception();
            }
        }

        public double Duration(int iterations)
        {
            return ((((this.stop - this.start) * ((double) this.multiplier)) / ((double) this.frequency)) / ((double) iterations));
        }

        [DllImport("KERNEL32")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);
        public void Start()
        {
            QueryPerformanceCounter(out this.start);
        }

        public void Stop()
        {
            QueryPerformanceCounter(out this.stop);
        }
    }
}

