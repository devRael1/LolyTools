using Loly.src.Tools;
using Loly.src.Variables;
using Loly.src.Variables.Enums;
using static Loly.src.Tools.Utils;

namespace Loly.src.Tasks.Scheduled
{
    public class AnalyzeSessionTask
    {
        public static void AnalyzeSession()
        {
            while (true)
            {
                if (!Global.IsLeagueOpen)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    continue;
                }

                string[] gameSession = Requests.ClientRequest("GET", "lol-gameflow/v1/gameflow-phase", true);
                if (gameSession[0] == "200")
                {
                    string phase = gameSession[1].Replace("\\", "").Replace("\"", "");
                    if (Global.Session != phase)
                    {
                        Global.Session = phase;
                    }

                    HandlePhaseLogic(phase);
                }
            }
        }

        private static void HandlePhaseLogic(string phase)
        {
            switch (phase)
            {
                case "Lobby":
                    Global.FetchedPlayers = false;
                    Global.AcceptedCurrentMatch = false;
                    Global.ChampSelectInProgress = false;
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    break;
                case "Matchmaking":
                    Global.FetchedPlayers = false;
                    Global.ChampSelectInProgress = false;
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    break;
                case "ReadyCheck":
                    HandleReadyCheckPhase();
                    break;
                case "ChampSelect":
                    HandleChampSelectPhase();
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    break;
                case "InProgress":
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    break;
                case "WaitingForStats":
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                    break;
                case "PreEndOfGame":
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    break;
                case "EndOfGame":
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                    break;
                case "None":
                    Global.ChampSelectInProgress = false;
                    Global.FetchedPlayers = false;
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    break;
                default:
                    Global.ChampSelectInProgress = false;
                    Global.FetchedPlayers = false;
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    break;
            }

            if (phase != "ChampSelect")
            {
                Global.LastChatRoom = "";
            }
        }

        private static void HandleReadyCheckPhase()
        {
            Global.FetchedPlayers = false;
            Global.ChampSelectInProgress = false;

            if (!Settings.AutoAccept)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                return;
            }
            AutoAccept.AutoAcceptQueue();
        }

        private static void HandleChampSelectPhase()
        {
            // TODO: Create the detection of dodge champ select system
            // ...

            Global.AcceptedCurrentMatch = false;

            if (Settings.AutoAcceptOnce)
            {
                Settings.AutoAcceptOnce = false;
                Settings.AutoAccept = false;
            }

            if (Settings.LobbyRevealer && !Global.FetchedPlayers)
            {
                CreateTask(LobbyRevealer.GetLobbyRevealing, $"LobbyRevealing the current lobby", LogModule.Loly);
            }

            if (Settings.AutoChat || Settings.PicknBan)
            {
                CreateTask(ChampSelectSession.HandleChampSelect, $"ChampSelect session analyze", LogModule.Loly);
            }

            Thread.Sleep(TimeSpan.FromSeconds(2));
            Global.ChampSelectInProgress = true;
        }
    }
}
