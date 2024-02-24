using Loly.src.Tasks.Scheduled;

namespace Loly.src.Tasks
{
    public class TaskCore
    {
        private readonly List<ScheduledTask> _scheduledTasks = new();

        public TaskCore()
        {
            _scheduledTasks.Add(new ScheduledTask(LeagueClientTask.LolClientTask, TimeSpan.FromSeconds(5), true, false));
            _scheduledTasks.Add(new ScheduledTask(AnalyzeSessionTask.AnalyzeSession, TimeSpan.FromSeconds(30), true, true));
            _scheduledTasks.Add(new ScheduledTask(LobbyRevealingTask.GetLobbyRevealing, TimeSpan.FromSeconds(10), true, false));
            _scheduledTasks.Add(new ScheduledTask(ClearLogsFilesTask.RunClearLogsFiles, TimeSpan.FromHours(6), true, false));
        }

        public void StartAllTasks()
        {
            foreach (ScheduledTask scheduledTask in _scheduledTasks)
            {
                scheduledTask.Start();
            }
        }

        public void StopAllTasks()
        {
            foreach (ScheduledTask scheduledTask in _scheduledTasks)
            {
                scheduledTask.Stop();
            }
        }
    }
}
