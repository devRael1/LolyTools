using Loly.src;
using Loly.src.LeagueClient;
using Loly.src.Logs;
using Loly.src.Menus;
using Loly.src.Menus.Core;
using Loly.src.Tools;
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

        new Thread(() =>
        {
            Task taskLeagueClient = new(Ux.LeagueClientTask);
            taskLeagueClient.Start();

            Task taskAutoAccept = new(Requests.AnalyzeSession);
            taskAutoAccept.Start();

            Task taskLobbyRevealer = new(LobbyRevealer.GetLobbyRevealing);
            taskLobbyRevealer.Start();

            Task[] tasks = { taskLeagueClient, taskAutoAccept, taskLobbyRevealer };
            Task.WaitAll(tasks);
        }).Start();

        // Wait for the League Client to be ready
        Thread.Sleep(3000);

        Logger.PrintHeader();
        Interface.ShowArt();
        MainMenu.StartMenu();
    }
}