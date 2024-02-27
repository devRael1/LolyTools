using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Enums;
using static Loly.src.Tools.Requests;

namespace Loly.src.Tools;

public class AutoAccept
{
    public static void AutoAcceptQueue()
    {
        ClientRequest("POST", "lol-matchmaking/v1/ready-check/accept", true);
        if (!Global.AcceptedCurrentMatch)
        {
            Logger.Info(LogModule.AutoAccept, "Auto accept the current match...");
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