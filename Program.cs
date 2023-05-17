using Loly.LeagueClient;
using Loly.Menus;
using Loly.Menus.Core;
using Loly.Tools;
using Loly.Variables;

namespace Loly;

internal class Program
{
    private static void Main()
    {
        Global.SoftName = "League of Legends - Loly Tools";
        Global.SoftAuthor = "devRael";
        Global.SoftAuthorDiscord = "devRael#0123";
        Global.SoftVersion = "1.2";
        Global.CurrentSummonerId = "";
        Global.LastChatRoom = "";
        Global.AcceptedCurrentMatch = false;
        Global.LogsMenuEnable = false;
        Global.FetchedPlayers = false;

        Settings.EnableAutoUpdate = true;
        Settings.LobbyRevealer = false;
        Settings.AutoAccept = false;
        Settings.AutoAcceptOnce = false;
        Settings.AutoChat = false;
        Settings.PicknBan = false;
        Settings.PickDelay = 1500;
        Settings.BanDelay = 1500;

        Settings.PickChamp.Name = null;
        Settings.PickChamp.Id = null;
        Settings.PickChamp.Free = false;
        Settings.BanChamp.Name = null;
        Settings.BanChamp.Id = null;
        Settings.BanChamp.Free = false;

        Settings.CreateOrUpdateSettings();

        if (Settings.EnableAutoUpdate) Updater.CheckUpdate();

        new Thread(() =>
        {
            Task taskLeagueClient = new(Ux.LeagueClientTask);
            taskLeagueClient.Start();

            Task taskAutoAccept = new(Requests.AnalyzeSession);
            taskAutoAccept.Start();

            Task taskLobbyRevealer = new(LobbyRevealer.GetAllNames);
            taskLobbyRevealer.Start();

            Task[] tasks = { taskLeagueClient, taskAutoAccept, taskLobbyRevealer };
            Task.WaitAll(tasks);
        }).Start();

        Interface.ShowArt();
        MainMenu.StartMenu();
    }
}