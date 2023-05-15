using System.Drawing;
using Loly.Variables;
using Console = Colorful.Console;

namespace Loly;

public class Logs
{
    public enum LogType
    {
        Global,
        LobbyRevealer,
        AutoChat,
        AutoAccept,
        PicknBan
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
            LogType.PicknBan => "PICK N BAN",
            _ => "Unknown"
        };
    }

    public static void Log(LogType logType, string message)
    {
        if (!Global.LogsMenuEnable) return;

        Console.Write(DateTime.Now.ToString("[hh:mm:ss]"), GetColor(logType));
        Console.Write($"[{GetPrefix(logType)}]» ", GetColor(logType));
        Console.Write(message, GetColor(logType));
        Console.WriteLine("");
    }
}