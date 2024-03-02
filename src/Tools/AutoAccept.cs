using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Enums;
using static Loly.src.Tools.Requests;

namespace Loly.src.Tools;

public class AutoAccept
{
    public static void AutoAcceptQueue()
    {
        string[] response = ClientRequest("POST", "lol-matchmaking/v1/ready-check/accept", true);
        if (response[0] != "200")
        {
            Logger.Info(LogModule.AutoAccept, "Failed to auto accept the current match");
            return;
        }

        if (!Global.AcceptedCurrentMatch)
        {
            Logger.Info(LogModule.AutoAccept, "The current match has been auto accepted");
        }

        Global.AcceptedCurrentMatch = true;
    }
}