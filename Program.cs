using Loly.src;
using Loly.src.Logs;
using Loly.src.Menus;
using Loly.src.Menus.Core;
using Loly.src.Tasks;
using Loly.src.Variables;

namespace Loly;

internal class Program
{
    private static void Main()
    {
        Settings.SetDefaultSettings();
        Settings.CreateOrUpdateSettings();

        if (Settings.EnableAutoUpdate)
        {
            Updater.CheckUpdate();
        }

        ExecuteMultipleTasks();

        // Wait for the League Client (and app) to be ready
        Thread.Sleep(3000);

        Logger.PrintHeader();
        Interface.ShowArt();
        MainMenu.StartMenu();
    }

    public static void ExecuteMultipleTasks()
    {
        Task task1 = Task.Run(() => LeagueClientTask.LolClientTask());
        Task task2 = Task.Run(() => AnalyzeSessionTask.AnalyzeSession());
        Task task3 = Task.Run(() => AnalyzeSessionTask.AnalyzeSession());

        Task.WhenAll(task1, task2, task3);
    }
}