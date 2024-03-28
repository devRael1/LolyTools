using Loly.src;
using Loly.src.Logs;
using Loly.src.Menus;
using Loly.src.Menus.Core;
using Loly.src.Tasks;
using Loly.src.Tools;
using Loly.src.Variables;
using Loly.src.Variables.Enums;

namespace Loly;

internal class Program
{
    private static void Main()
    {
        AppDomain.CurrentDomain.UnhandledException += UnhandledException;

        SettingsManager.CreateOrUpdateSettings();

        if (Global.CurrentSettings.EnableAutoUpdate) Updater.CheckUpdate();

        TaskCore tasks = new();
        tasks.StartAllTasks();

        // Wait for the League Client (and app) to be ready
        Thread.Sleep(TimeSpan.FromSeconds(3));

        Logger.PrintHeader();
        Interface.ShowArt();
        MainMenu.StartMenu();
    }

    private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Utils.LogNewError("Unhandled exception", LogModule.Loly, (Exception)e.ExceptionObject);
    }
}