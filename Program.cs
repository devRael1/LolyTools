using Loly.LeagueClient;
using Loly.Menus;
using Loly.Menus.Core;
using Loly.Variables;

namespace Loly;

internal class Program
{
    private static void Main()
    {
        // Variables Variables Initialization
        Global.SoftName = "League of Legends - Loly Tools";
        Global.SoftAuthor = "devRael";
        Global.SoftAuthorDiscord = "devRael#0123";
        Global.SoftVersion = "1.0.0";
        Global.CurrentSummonerId = "";
        Global.LastChatRoom = "";

        // Create or Get Settings
        Settings.EnableAutoUpdate = true;
        Settings.LobbyRevealer = false;
        Settings.AutoAccept = false;
        Settings.AutoAcceptOnce = false;
        Settings.PicknBan = false;
        Settings.PickDelay = 1500;
        Settings.BanDelay = 1500;

        // Champ Selected (Pick n Ban)
        Settings.ChampSelected.Name = null;
        Settings.ChampSelected.Id = null;
        Settings.ChampSelected.Free = false;

        // Champ Banned (Pick n Ban)
        Settings.ChampBanned.Name = null;
        Settings.ChampBanned.Id = null;
        Settings.ChampBanned.Free = false;

        Settings.CreateOrUpdateSettings();

        // Run Updater before Start
        if (Settings.EnableAutoUpdate) Updater.CheckUpdate();

        // Create background thread and run all tasks
        new Thread(() =>
        {
            Task taskLeagueClient = new(Ux.LeagueClientTask);
            taskLeagueClient.Start();

            Task taskAutoAccept = new(Requests.AnalyzeSession);
            taskAutoAccept.Start();

            // Wait for all tasks to complete (Indefinitely)
            Task[] tasks = { taskLeagueClient, taskAutoAccept };
            Task.WaitAll(tasks);
        }).Start();

        // Start Menu
        Interface.ShowArt();
        MainMenu.StartMenu();
    }
}