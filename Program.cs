using Loly.LeagueClient;
using Loly.Menus;
using Loly.Menus.Core;
using Loly.Variables;

namespace Loly;

internal class Program
{
    private static void Main()
    {
        Global.SoftName = "League of Legends - Loly Tools";
        Global.SoftAuthor = "devRael";
        Global.SoftAuthorDiscord = "devRael#0123";
        Global.SoftVersion = "1.1";
        Global.CurrentSummonerId = "";
        Global.LastChatRoom = "";

        Settings.EnableAutoUpdate = true;
        Settings.LobbyRevealer = false;
        Settings.AutoAccept = false;
        Settings.AutoAcceptOnce = false;
        Settings.PicknBan = false;
        Settings.PickDelay = 1500;
        Settings.BanDelay = 1500;

        Settings.ChampSelected.Name = null;
        Settings.ChampSelected.Id = null;
        Settings.ChampSelected.Free = false;
        Settings.ChampBanned.Name = null;
        Settings.ChampBanned.Id = null;
        Settings.ChampBanned.Free = false;

        Settings.CreateOrUpdateSettings();

        if (Settings.EnableAutoUpdate) Updater.CheckUpdate();

        new Thread(() =>
        {
            Task taskLeagueClient = new(Ux.LeagueClientTask);
            taskLeagueClient.Start();

            Task taskAutoAccept = new(Requests.AnalyzeSession);
            taskAutoAccept.Start();

            Task[] tasks = { taskLeagueClient, taskAutoAccept };
            Task.WaitAll(tasks);
        }).Start();

        Interface.ShowArt();
        MainMenu.StartMenu();
    }
}