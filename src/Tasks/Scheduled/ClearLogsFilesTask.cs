﻿using Loly.src.Logs;
using Loly.src.Tools;
using Loly.src.Variables;
using Loly.src.Variables.Enums;

using static Loly.src.Variables.Global;

namespace Loly.src.Tasks.Scheduled;

public class ClearLogsFilesTask
{
    public static void RunClearLogsFiles()
    {
        var filesBotLogs = ClearLogsFiles(Directory.GetDirectories(Logger.LogFolder));

        Logger.Info(LogModule.Loly, $"Clear {filesBotLogs} logs folder(s) older than [{CurrentSettings.ClearLogsFilesDays} days]...");
    }

    private static int ClearLogsFiles(string[] folders)
    {
        var count = 0;

        foreach (var folder in folders)
        {
            DirectoryInfo directory = new(folder);
            DateTime dateNow = DateTime.Now;
            TimeSpan diff = dateNow - directory.CreationTime;

            if (Math.Round(diff.TotalDays) < CurrentSettings.ClearLogsFilesDays) continue;

            try
            {
                var details = $"\n\tDelete folder '{directory.Name}' in '{directory.Parent}' folder.";
                details += $"\n\tDetails of deleted folder :";
                details += $"\n\t\tCreation time : {directory.CreationTime:G}";
                details += $"\n\t\tSize : {Utils.FormatBytes(directory.EnumerateFiles().Sum(file => file.Length), false)}";
                details += $"\n\t\tNumber of files : {directory.EnumerateFiles().Count()}";

                Logger.Info(LogModule.Tasks, details, Global.LogsMenuEnable ? LogType.Both : LogType.File);
                Directory.Delete(folder, true);
                count++;
            }
            catch (IOException ex)
            {
                Logger.Error(LogModule.Tasks, $"Cannot delete folder '{folder}' because it's in use by another process...", ex);
                continue;
            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.Error(LogModule.Tasks, $"Cannot delete folder '{folder}' because i don't have permission to access to this folder...", ex);
                continue;
            }
        }
        return count;
    }
}