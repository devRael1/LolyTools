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

        TaskCore tasks = new();
        tasks.StartAllTasks();

        // Wait for the League Client (and app) to be ready
        Thread.Sleep(3000);

        Logger.PrintHeader();
        Interface.ShowArt();
        MainMenu.StartMenu();
    }
}