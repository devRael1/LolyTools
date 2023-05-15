using Loly.Variables;
using static Loly.LeagueClient.Requests;
using static Loly.Logs;

namespace Loly.Tools;

public class AutoAccept
{
    public static void AutoAcceptQueue()
    {
        ClientRequest("POST", "lol-matchmaking/v1/ready-check/accept", true);
        Log(LogType.AutoAccept, "Auto accept the current match...");
        if (!Settings.AutoAcceptOnce) return;
        Settings.AutoAccept = false;
        Settings.AutoAcceptOnce = false;
    }
}