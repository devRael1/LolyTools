using Loly.src.Tools;
using Loly.src.Variables;
using Loly.src.Variables.Enums;

using static Loly.src.Tools.Utils;

namespace Loly.src.Tasks.Scheduled;

public class AnalyzeSessionTask
{
    public static void AnalyzeSession()
    {
        while (true)
        {
            if (!Global.IsLeagueOpen)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                continue;
            }

            var gameSession = Requests.ClientRequest("GET", "lol-gameflow/v1/gameflow-phase", true);
            if (gameSession[0] == "200")
            {
                var phaseString = gameSession[1].Replace("\\", "").Replace("\"", "");
                if (!Enum.TryParse(phaseString, out SessionPhase phase)) phase = SessionPhase.None;
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
                Global.LobbyRevealingStarted = false;
                Thread.Sleep(TimeSpan.FromSeconds(5));
                break;
            case SessionPhase.Matchmaking:
                Global.FetchedPlayers = false;
                Global.AcceptedCurrentMatch = false;
                Global.LobbyRevealingStarted = false;
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
                Thread.Sleep(TimeSpan.FromSeconds(30));
                break;
            case SessionPhase.Reconnect:
                Thread.Sleep(TimeSpan.FromSeconds(5));
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
                Global.LobbyRevealingStarted = false;
                Thread.Sleep(TimeSpan.FromSeconds(10));
                break;
        }

        if (phase != SessionPhase.ChampSelect)
        {
            Global.LastChatRoom = "";
            Global.LobbyRevealingStarted = false;
        }
    }

    private static void HandleReadyCheckPhase()
    {
        Global.FetchedPlayers = false;
        Global.LobbyRevealingStarted = false;

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
        if (Settings.LobbyRevealer && !Global.FetchedPlayers && !Global.LobbyRevealingStarted)
        {
            CreateBackgroundTask(LobbyRevealer.GetLobbyRevealing, $"LobbyRevealing the current lobby", LogModule.Loly);
            Global.LobbyRevealingStarted = true;
        }
        if (Settings.AutoChat || Settings.PicknBan) ChampSelectSession.HandleChampSelect();
    }
}