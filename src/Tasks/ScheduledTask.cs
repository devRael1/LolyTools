using System.Timers;

using Loly.src.Tools;

using Timer = System.Timers.Timer;

namespace Loly.src.Tasks.Scheduled;

public class ScheduledTask
{
    private readonly Action taskAction;
    private readonly Timer timer;
    private readonly string taskName;
    private readonly bool runNow;
    private readonly bool infinite;

    public ScheduledTask(Action taskAction, string taskName, TimeSpan interval, bool runNow, bool infinite)
    {
        this.taskAction = taskAction;
        this.taskName = taskName;
        this.runNow = runNow;
        this.infinite = infinite;
        timer = new Timer
        {
            Interval = interval.TotalMilliseconds,
            AutoReset = !this.infinite,
            Enabled = true
        };
        if (!this.infinite)
        {
            timer.Elapsed += TimerElapsed;
        }
    }

    public void Start()
    {
        if (runNow)
        {
            Utils.CreateBackgroundTask(taskAction, $"Task [{taskName}]");
        }

        if (!infinite)
        {
            timer.Start();
        }
    }

    public void Stop()
    {
        timer.Stop();
        timer.Dispose();
    }

    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        Utils.CreateBackgroundTask(taskAction, $"Task [{taskName}]");
    }
}