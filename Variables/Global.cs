namespace Loly.Variables;

public class Global
{
    public const string DiscordInvite = "https://discord.gg/NAQNuBnSC3";

    public static readonly List<Player> PlayerList = new();
    public static readonly List<string> UsernameList = new();
    public static readonly List<ChampItem> ChampionsList = new();

    public static readonly Dictionary<string, string> AuthRiot = new();
    public static readonly Dictionary<string, string> AuthClient = new();

    public static string CurrentSummonerId { get; set; } 
    public static string LastChatRoom { get; set; }
    public static int LastActionId { get; set; }
    public static long LastActStartTime { get; set; }
    public static bool IsLeagueOpen { get; set; }
    public static string Region { get; set; }
    public static string SoftName { get; set; }
    public static string SoftAuthor { get; set; }
    public static string SoftAuthorDiscord { get; set; }
    public static string SoftVersion { get; set; }

    // TODO:Auto send message System (Chat)
    // public static List<string> ChatMessages = new();
    // public static bool ChatMessagesEnabled { get; set; }
}

public class ChampItem
{
    public string Name { get; set; }
    public string Id { get; set; }
    public bool Free { get; set; }
}