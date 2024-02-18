using Loly.src.Variables;
using System.Drawing;
using Console = Colorful.Console;

namespace Loly.src.Logs;

public class Logger
{
    public static void Log(LogType logType, string message)
    {
        if (!Global.LogsMenuEnable)
        {
            return;
        }

        Console.Write(DateTime.Now.ToString("[hh:mm:ss]"), GetColor(logType));
        Console.Write($"[{GetPrefix(logType)}]» ", GetColor(logType));
        Console.Write(message, GetColor(logType));
        Console.WriteLine("");
    }

    private static Color GetColor(LogType logType)
    {
        return logType switch
        {
            LogType.Global => Color.White,
            LogType.LobbyRevealer => Color.Orange,
            LogType.AutoChat => Color.Moccasin,
            LogType.AutoAccept => Color.Khaki,
            LogType.PicknBan => Color.Pink,
            _ => Colors.PrimaryColor
        };
    }

    private static string GetPrefix(LogType logType)
    {
        return logType switch
        {
            LogType.Global => "GLOBAL",
            LogType.LobbyRevealer => "LOBBY REVEALER",
            LogType.AutoChat => "AUTO CHAT",
            LogType.AutoAccept => "AUTO ACCEPT",
            LogType.PicknBan => "PICK AND BAN",
            _ => "Unknown"
        };
    }
}