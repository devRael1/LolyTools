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
                    SessionPhase phase = (SessionPhase)Enum.Parse(typeof(SessionPhase), gameSession[1].Replace("\\", "").Replace("\"", ""));
                    if (Global.Session != phase) Global.Session = phase;
                    HandlePhaseLogic(phase);
                }
            }
        }

        private static void HandlePhaseLogic(SessionPhase phase)
        {
            switch (phase)
            {
                case SessionPhase.Lobby:
                    Global.FetchedPlayers = false;
                    Global.AcceptedCurrentMatch = false;
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    break;
                case SessionPhase.Matchmaking:
                    Global.FetchedPlayers = false;
                    Global.AcceptedCurrentMatch = false;
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    break;
                case SessionPhase.ReadyCheck:
                    HandleReadyCheckPhase();
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    break;
                case SessionPhase.ChampSelect:
                    HandleChampSelectPhase();
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    break;
                case SessionPhase.InProgress:
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    break;
                case SessionPhase.WaitingForStats:
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                    break;
                case SessionPhase.PreEndOfGame:
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    break;
                case SessionPhase.EndOfGame:
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                    break;
                case SessionPhase.None:
                    Global.FetchedPlayers = false;
                    Global.AcceptedCurrentMatch = false;
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    break;
                default:
                    Global.FetchedPlayers = false;
                    Global.AcceptedCurrentMatch = false;
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    break;
            }

            if (phase != SessionPhase.ChampSelect) Global.LastChatRoom = "";
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
            Global.AcceptedCurrentMatch = false;

            if (Settings.AutoAccept && Settings.AutoAcceptOnce) Settings.AutoAccept = false;
            if (Settings.LobbyRevealer && !Global.FetchedPlayers) CreateBackgroundTask(LobbyRevealer.GetLobbyRevealing, $"LobbyRevealing the current lobby", LogModule.Loly);
            if (Settings.AutoChat || Settings.PicknBan) ChampSelectSession.HandleChampSelect();
        }
    }
}
