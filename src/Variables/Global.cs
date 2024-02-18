using Loly.src.Logs;
using Loly.src.Variables.Class;

namespace Loly.src.Variables;

public class Global
{
    public const string DiscordInvite = "https://discord.gg/NAQNuBnSC3";
    private static string _session;

    public static List<Player> PlayerList = new();
    public static List<ChampItem> ChampionsList = new();

    public static Dictionary<string, string> AuthRiot = new();
    public static Dictionary<string, string> AuthClient = new();

    public static CurrentSummoner Summoner { get; set; } = new();
    public static string LastChatRoom { get; set; }
    public static int LastActionId { get; set; }
    public static bool AcceptedCurrentMatch { get; set; }
    public static bool FetchedPlayers { get; set; }
    public static bool IsLeagueOpen { get; set; }
    public static bool LogsMenuEnable { get; set; }
    public static string Region { get; set; }
    public static string SoftName { get; set; }
    public static string SoftAuthor { get; set; }
    public static bool IsProdEnvironment { get; set; }

    public static string Session
    {
        get => _session;
        set
        {
            _session = value;
            Logger.Info(LogModule.Loly, $"Getting session - Phase Detected: {_session}", true);
        }
    }

    public static Player FindPlayer(string value)
    {
        return PlayerList.Find(x => long.Parse(x.Id) == long.Parse(value));
    }
}