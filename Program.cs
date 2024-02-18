using Loly.src;
using Loly.src.LeagueClient;
using Loly.src.Menus;
using Loly.src.Menus.Core;
using Loly.src.Tools;
using Loly.src.Variables;

namespace Loly;

internal class Program
{
    private static void Main()
    {
        Global.SoftName = "League of Legends - Loly Tools";
        Global.SoftAuthor = "devRael";
        Global.SoftAuthorDiscord = "devRael#0123";
        Global.SoftVersion = "1.4";
        Global.AcceptedCurrentMatch = false;
        Global.LogsMenuEnable = false;
        Global.FetchedPlayers = false;

        Settings.EnableAutoUpdate = true;
        Settings.LobbyRevealer = false;
        Settings.AutoAccept = false;
        Settings.AutoAcceptOnce = false;
        Settings.AutoChat = false;
        Settings.PicknBan = false;

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

        Interface.ShowArt();
        MainMenu.StartMenu();
    }
}