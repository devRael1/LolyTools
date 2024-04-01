using Loly.src.Tools;
using Loly.src.Variables.Enums;

using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Tasks.Scheduled;

public class AnalyzeSessionTask
{
    public static void AnalyzeSession()
    {
        while (true)
        {
            if (!IsLeagueOpen)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                continue;
            }

            var gameSession = Requests.ClientRequest("GET", "lol-gameflow/v1/gameflow-phase", true);
            if (gameSession[0] == "200")
            {
                var phaseString = gameSession[1].Replace("\\", "").Replace("\"", "");
                if (!Enum.TryParse(phaseString, out SessionPhase phase)) phase = SessionPhase.None;
                if (Session != phase) Session = phase;
                HandlePhaseLogic(phase);
            }
        }
    }

    private static void HandlePhaseLogic(SessionPhase phase)
    {
        switch (phase)
        {
            case SessionPhase.Lobby:
                FetchedPlayers = false;
                AcceptedCurrentMatch = false;
                LobbyRevealingStarted = false;
                Thread.Sleep(TimeSpan.FromSeconds(5));
                break;
            case SessionPhase.Matchmaking:
                FetchedPlayers = false;
                AcceptedCurrentMatch = false;
                LobbyRevealingStarted = false;
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
                FetchedPlayers = false;
                AcceptedCurrentMatch = false;
                Thread.Sleep(TimeSpan.FromSeconds(5));
                break;
            default:
                FetchedPlayers = false;
                AcceptedCurrentMatch = false;
                LobbyRevealingStarted = false;
                Thread.Sleep(TimeSpan.FromSeconds(10));
                break;
        }

        if (phase != SessionPhase.ChampSelect)
        {
            LastChatRoom = "";
            LobbyRevealingStarted = false;
        }
    }

    private static void HandleReadyCheckPhase()
    {
        FetchedPlayers = false;
        LobbyRevealingStarted = false;

        if (!CurrentSettings.Tools.AutoAccept)
        {
            Thread.Sleep(TimeSpan.FromSeconds(5));
            return;
        }

        AutoAccept.AutoAcceptQueue();
    }

    private static void HandleChampSelectPhase()
    {
        AcceptedCurrentMatch = false;

        if (CurrentSettings.Tools.AutoAccept && CurrentSettings.AutoAccept.AutoAcceptOnce) CurrentSettings.Tools.AutoAccept = false;
        if (CurrentSettings.Tools.LobbyRevealer && !FetchedPlayers && !LobbyRevealingStarted)
        {
            CreateBackgroundTask(LobbyRevealer.GetLobbyRevealing, $"LobbyRevealing the current lobby");
            LobbyRevealingStarted = true;
        }
        if (CurrentSettings.Tools.AutoChat || CurrentSettings.Tools.PicknBan) ChampSelectSession.HandleChampSelect();
    }
}