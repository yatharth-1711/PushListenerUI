using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MeterReader.TestHelperClasses
{
    public class TestStopWatch : IDisposable
    {
        Stopwatch stopWatch;
        private Timer monitorTimer;
        private bool thresholdExceeded = false;
        // Event to notify when elapsed time exceeds threshold
        public event EventHandler ElapsedTimeExceeded;
        // Threshold in seconds (120 by default)
        public double thresholdSeconds = 100;
        public TestStopWatch(double thresholdSeconds = 100)
        {
            stopWatch = new Stopwatch();
            this.thresholdSeconds = thresholdSeconds;

            monitorTimer = new Timer(1000); // check every second
            monitorTimer.Elapsed += MonitorTimer_Elapsed;
        }
        private void MonitorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!thresholdExceeded && stopWatch.Elapsed.TotalSeconds > thresholdSeconds)
            {
                thresholdExceeded = true;
                ElapsedTimeExceeded?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Start()
        {
            if ((stopWatch.Elapsed).TotalSeconds > 0)
                stopWatch.Reset();
            stopWatch.Start();

            thresholdExceeded = false;
            monitorTimer.Start();
        }

        public void Stop()
        {
            stopWatch.Stop();
            monitorTimer.Stop();
        }

        public string GetElapsedTime()
        {
            // Format the TimeSpan to "Hour:min:sec"
            return string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}",
                stopWatch.Elapsed.Hours, stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds);
        }
        public void Reset()
        {
            stopWatch.Reset();
            thresholdExceeded = false;
        }
        public bool IsRunning()
        {
            return stopWatch.IsRunning;
        }

        public double GetElapsedSeconds()
        {
            return stopWatch.Elapsed.TotalSeconds;
        }
        public TimeSpan GetElapsedSpan()
        {
            return stopWatch.Elapsed;
        }

        public void Dispose()
        {
            if (stopWatch != null)
            {
                if (stopWatch.IsRunning)
                    stopWatch.Stop();
                stopWatch = null;
            }

            if (monitorTimer != null)
            {
                monitorTimer.Stop();
                monitorTimer.Dispose();
                monitorTimer = null;
            }
        }
    }
}
