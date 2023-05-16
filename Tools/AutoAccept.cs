using Loly.Variables;
using static Loly.Logs;
using static Loly.LeagueClient.Requests;

namespace Loly.Tools;

public class AutoAccept
{
    public static void AutoAcceptQueue()
    {
        ClientRequest("POST", "lol-matchmaking/v1/ready-check/accept", true);
        if (!Global.AcceptedCurrentMatch) Log(LogType.AutoAccept, "Auto accept the current match...");
        Global.AcceptedCurrentMatch = true;
        if (!Settings.AutoAcceptOnce) return;
        Settings.AutoAccept = false;
        Settings.AutoAcceptOnce = false;
    }
}