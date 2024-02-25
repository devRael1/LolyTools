using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Enums;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Loly.src.Tasks.Scheduled
{
    public class ScheduledTask
    {
        private readonly Action taskAction;
        private readonly Timer timer;
        private readonly bool runNow;
        private readonly bool infinite;

        public ScheduledTask(Action taskAction, TimeSpan interval, bool runNow, bool infinite)
        {
            this.taskAction = taskAction;
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
                Task.Run(taskAction).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Logger.Error(LogModule.Tasks, $"An error occured while executing the task", task.Exception, Global.LogsMenuEnable ? LogType.Both : LogType.File);
                    }
                });
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
            Task.Run(taskAction).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Logger.Error(LogModule.Tasks, $"An error occured while executing the task", task.Exception, Global.LogsMenuEnable ? LogType.Both : LogType.File);
                }
            });
        }
    }
}
