using Loly.src.Tools;
using Loly.src.Variables;

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
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    break;
                case "Matchmaking":
                    Global.FetchedPlayers = false;
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    break;
                case "ReadyCheck":
                    HandleReadyCheckPhase();
                    break;
                case "ChampSelect":
                    HandleChampSelectPhase();
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
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    break;
                default:
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
                LobbyRevealer.GetLobbyRevealing();
            }

            if (Settings.AutoChat || Settings.PicknBan)
            {
                ChampSelectSession.HandleChampSelect();
            }
        }
    }
}
