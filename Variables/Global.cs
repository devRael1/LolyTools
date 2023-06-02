namespace Loly.Variables;

public class Global
{
    public const string DiscordInvite = "https://discord.gg/NAQNuBnSC3";
    private static string _session;

    public static readonly List<Player> PlayerList = new();
    public static readonly List<ChampItem> ChampionsList = new();

    public static readonly Dictionary<string, string> AuthRiot = new();
    public static readonly Dictionary<string, string> AuthClient = new();

    public static string CurrentSummonerId { get; set; }
    public static string LastChatRoom { get; set; }
    public static int LastActionId { get; set; }
    public static bool AcceptedCurrentMatch { get; set; }
    public static bool FetchedPlayers { get; set; }
    public static bool IsLeagueOpen { get; set; }
    public static bool LogsMenuEnable { get; set; }
    public static string Region { get; set; }
    public static string SoftName { get; set; }
    public static string SoftAuthor { get; set; }
    public static string SoftAuthorDiscord { get; set; }
    public static string SoftVersion { get; set; }

    public static string Session
    {
        get => _session;
        set
        {
            _session = value;
            Logs.Log(Logs.LogType.Global, $"Getting session - Phase Detected: {_session}");
        }
    }

    public static Player FindPlayer(string value)
    {
        return PlayerList.Find(x => x.Id == value);
    }
}