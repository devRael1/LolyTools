using Loly.src.Logs;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;

namespace Loly.src.Variables;

internal static class Global
{
    internal const string DiscordWebhook = "https://discord.com/api/webhooks/1212659511030980658/kuU1G0aHikrC1VkGLXYzL49zG1sH5QsXRTMvCuXV4X0CIYEi0ClV6SIzS5K2cxnWGNAR";
    internal const string GithubPage = "https://github.com/devRael1/LolyTools";
    internal const string SoftName = "League of Legends - Loly Tools";
    internal const string SoftAuthor = "devRael";
    internal const string UrlAPI = "https://u.gg/api";

    private static SessionPhase _session = SessionPhase.None;

    internal static Settings CurrentSettings { get; set; } = new();

    internal static List<Player> PlayerList { get; set; } = new();
    internal static List<ChampItem> ChampionsList { get; set; } = new();
    internal static Dictionary<string, string> AuthRiot { get; set; } = new();
    internal static Dictionary<string, string> AuthClient { get; set; } = new();
    internal static CurrentSummoner SummonerLogged { get; set; } = new();
    internal static string LastChatRoom { get; set; } = "";
    internal static int LastActionId { get; set; } = 0;
    internal static bool AcceptedCurrentMatch { get; set; } = false;
    internal static bool LobbyRevealingStarted { get; set; } = false;
    internal static bool FetchedPlayers { get; set; } = false;
    internal static bool IsLeagueOpen { get; set; } = false;
    internal static bool LogsMenuEnable { get; set; } = false;
    internal static string Region { get; set; } = "";
    internal static int TopLength { get; set; } = 0;
    internal static string LeagueClientPath { get; set; } = "";

    internal static SessionPhase Session
    {
        get => _session;
        set
        {
            _session = value;
            Logger.Info(LogModule.Loly, $"Analyze session - Phase Detected: {Session}");
        }
    }
}