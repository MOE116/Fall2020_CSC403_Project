using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Fall2020_CSC403_Project
{
    public class StopwatchHelper // lei
    {
        private Stopwatch stopwatch;

        public StopwatchHelper()
        {
            stopwatch = new Stopwatch();
        }

        public void Start()
        {
            stopwatch.Start();
        }

        public void Stop()
        {
            stopwatch.Stop();
        }

        public void Reset()
        {
            stopwatch.Reset();
        }

        public string GetElapsedTimeString()
        {
            TimeSpan elapsed = stopwatch.Elapsed;
            return string.Format("{0:D2}:{1:D2}:{2:D2}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds);
        }
    }
}
