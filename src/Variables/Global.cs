using Loly.src.Logs;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;

namespace Loly.src.Variables;

public class Global
{
    public const string DiscordWebhook = "https://discord.com/api/webhooks/1212659511030980658/kuU1G0aHikrC1VkGLXYzL49zG1sH5QsXRTMvCuXV4X0CIYEi0ClV6SIzS5K2cxnWGNAR";
    public const string GithubPage = "https://github.com/devRael1/LolyTools";
    public const string SoftName = "League of Legends - Loly Tools";
    public const string SoftAuthor = "devRael";
    public const string UrlAPI = "https://u.gg/api";

    private static SessionPhase _session = SessionPhase.None;

    public static Settings CurrentSettings { get; set; } = new();

    public static List<Player> PlayerList { get; set; } = new();
    public static List<ChampItem> ChampionsList { get; set; } = new();
    public static Dictionary<string, string> AuthRiot { get; set; } = new();
    public static Dictionary<string, string> AuthClient { get; set; } = new();
    public static CurrentSummoner SummonerLogged { get; set; } = new();
    public static string LastChatRoom { get; set; } = "";
    public static int LastActionId { get; set; } = 0;
    public static bool AcceptedCurrentMatch { get; set; } = false;
    public static bool LobbyRevealingStarted { get; set; } = false;
    public static bool FetchedPlayers { get; set; } = false;
    public static bool IsLeagueOpen { get; set; } = false;
    public static bool LogsMenuEnable { get; set; } = false;
    public static string Region { get; set; } = "";
    public static bool IsProdEnvironment { get; set; } = false;
    public static int TopLength { get; set; } = 0;
    public static string LeagueClientPath { get; set; } = "";

    public static SessionPhase Session
    {
        get => _session;
        set
        {
            _session = value;
            Logger.Info(LogModule.Loly, $"Analyze session - Phase Detected: {Session}");
        }
    }
}