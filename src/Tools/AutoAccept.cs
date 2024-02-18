using Loly.src.Variables;
using static Loly.src.LeagueClient.Requests;
using static Loly.src.Logs.Logger;

namespace Loly.src.Tools;

public class AutoAccept
{
    public static void AutoAcceptQueue()
    {
        _ = ClientRequest("POST", "lol-matchmaking/v1/ready-check/accept", true);
        if (!Global.AcceptedCurrentMatch)
        {
            Log(LogType.AutoAccept, "Auto accept the current match...");
        }

        Global.AcceptedCurrentMatch = true;
        if (!Settings.AutoAcceptOnce)
        {
            return;
        }

        Settings.AutoAccept = false;
        Settings.AutoAcceptOnce = false;
    }
}