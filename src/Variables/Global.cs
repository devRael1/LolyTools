using Loly.src.Logs;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;

namespace Loly.src.Variables;

public class Global
{
    public const string GithubPage = "https://github.com/devRael1/LolyTools";
    public const string SoftName = "League of Legends - Loly Tools";
    public const string SoftAuthor = "devRael";
    private static string _session = "";

    public static List<Player> PlayerList = new();
    public static List<ChampItem> ChampionsList = new();

    public static Dictionary<string, string> AuthRiot = new();
    public static Dictionary<string, string> AuthClient = new();

    public static CurrentSummoner SummonerLogged { get; set; } = new();
    public static string LastChatRoom { get; set; } = "";
    public static int LastActionId { get; set; } = 0;
    public static bool AcceptedCurrentMatch { get; set; } = false;
    public static bool FetchedPlayers { get; set; } = false;
    public static bool IsLeagueOpen { get; set; } = false;
    public static bool LogsMenuEnable { get; set; } = false;
    public static string Region { get; set; } = "";
    public static bool IsProdEnvironment { get; set; } = false;

    public static string Session
    {
        get => _session;
        set
        {
            _session = value;
            Logger.Info(LogModule.Loly, $"Analyze session - Phase Detected: {Session}");
        }
    }
}