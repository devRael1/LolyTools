using Loly.src.Logs;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;

namespace Loly.src.Variables;

public class Global
{
    public const string DiscordInvite = "https://discord.gg/NAQNuBnSC3";
    public const string SoftName = "League of Legends - Loly Tools";
    public const string SoftAuthor = "devRael";

    public static List<Player> PlayerList = new();
    public static List<ChampItem> ChampionsList = new();

    public static Dictionary<string, string> AuthRiot = new();
    public static Dictionary<string, string> AuthClient = new();

    public static CurrentSummoner Summoner { get; set; } = new();
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
        get => Session;
        set
        {
            Session = value;
            Logger.Info(LogModule.Loly, $"Analyze session - Phase Detected: {Session}", LogsMenuEnable ? LogType.Both : LogType.File);
        }
    }
}