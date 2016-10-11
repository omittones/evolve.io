using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace Util
{
    public class HiResTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(
            out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(
            out long lpFrequency);

        private long startTime;
        private readonly long freq;

        public HiResTimer()
        {
            startTime = 0;
            if (QueryPerformanceFrequency(out freq) == false)
            {
                throw new Win32Exception("High-performance counter not supported.");
            }
        }

        public double Reset()
        {
            var local = this.LocalTimeInSeconds;
            QueryPerformanceCounter(out startTime);
            return local;
        }

        public double LocalTimeInSeconds
        {
            get
            {
                long lTime;
                QueryPerformanceCounter(out lTime);
                return (double) (lTime - startTime)/(double) freq;
            }
        }

        public long GlobalTime
        {
            get
            {
                long lTime;
                QueryPerformanceCounter(out lTime);
                return lTime;
            }
        }


        public long Frequency => freq;

        public long LocalTime
        {
            get
            {
                long lTime;
                QueryPerformanceCounter(out lTime);
                return lTime - startTime;
            }
        }

        public double GlobalTimeInSeconds
        {
            get
            {
                long lTime;
                QueryPerformanceCounter(out lTime);
                return (double) lTime/(double) freq;
            }
        }
    }
}
