using Loly.src.Tasks.Scheduled;

namespace Loly.src.Tasks;

internal class TaskCore
{
    private readonly List<ScheduledTask> _scheduledTasks = new();

    internal TaskCore()
    {
        _scheduledTasks.Add(new ScheduledTask(LeagueClientTask.LolClientTask, "LolClientTask", TimeSpan.FromSeconds(5), true, false));
        _scheduledTasks.Add(new ScheduledTask(AnalyzeSessionTask.AnalyzeSession, "AnalyzeSession", TimeSpan.FromMinutes(1), true, true));
        _scheduledTasks.Add(new ScheduledTask(ClearLogsFilesTask.RunClearLogsFiles, "RunClearLogsFiles", TimeSpan.FromHours(1), true, false));
    }

    internal void StartAllTasks()
    {
        foreach (ScheduledTask scheduledTask in _scheduledTasks) scheduledTask.Start();
    }

    internal void StopAllTasks()
    {
        foreach (ScheduledTask scheduledTask in _scheduledTasks) scheduledTask.Stop();
    }
}