using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF_Force
{
    internal class StopwatchController
    {
        public Stopwatch Stopwatch { get; private set; }
        public string Time { get; private set; }
        public StopwatchController(Stopwatch stopwatch)
        {
            Stopwatch = stopwatch;
        }
        public void Start()
        {
            Stopwatch.Start();
        }
        public void Stop()
        {
            Stopwatch.Stop();
            var resultTime = Stopwatch.Elapsed;
            Time = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                                                resultTime.Hours,
                                                resultTime.Minutes,
                                                resultTime.Seconds,
                                                resultTime.Milliseconds);
        }
    }
}
