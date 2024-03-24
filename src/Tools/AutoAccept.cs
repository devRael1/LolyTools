using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Enums;

using static Loly.src.Tools.Requests;

namespace Loly.src.Tools;

public class AutoAccept
{
    public static void AutoAcceptQueue()
    {
        if (Global.AcceptedCurrentMatch) return;

        var response = ClientRequest("POST", "lol-matchmaking/v1/ready-check/accept", true);
        if (response[0] != "204")
        {
            Logger.Warn(LogModule.AutoAccept, "Failed to auto accept the current match");
            Logger.Warn(LogModule.AutoAccept, "Check Requests logs to get more informations.");
            return;
        }

        Logger.Info(LogModule.AutoAccept, "The current match has been auto accepted");
        Global.AcceptedCurrentMatch = true;
    }
}