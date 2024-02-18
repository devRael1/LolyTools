using Gommon;
using Loly.src.Menus.Core;
using System.Drawing;
using System.Text;
using Console = Colorful.Console;

namespace Loly.src.Logs;

public static class Logger
{
    public const string LogFolder = "Logs";
    private const string LogTempFile = $"{LogFolder}/temp_loly.log";

    private static readonly object Lock = new();
    private static readonly object Lock2 = new();
    private static bool _headerPrinted;

    static Logger()
    {
        _ = Directory.CreateDirectory(LogFolder);
    }

    private static void Log(LogSeverity s, LogModule from, string message, Exception e = null, bool onlyConsole = false)
    {
        if (!onlyConsole)
        {
            Lock.Lock(() => Execute(s, from, message, e));
        }
        else
        {
            Lock2.Lock(() => ExecuteWithoutConsole(s, from, message));
        }
    }

    internal static void PrintHeader()
    {
        if (_headerPrinted)
        {
            return;
        }

        Interface.ArtName.Split("\n", StringSplitOptions.TrimEntries).ForEach(ln => Info(LogModule.Loly, ln, true));
        Info(LogModule.Loly, $"Currently running Loly Tools V{Version.FullVersion}.", true);
        _headerPrinted = true;
    }

    public static void Debug(LogModule src, string message)
    {
        Log(LogSeverity.Debug, src, message);
    }

    public static void Info(LogModule src, string message, bool onlyConsole = false)
    {
        Log(LogSeverity.Info, src, message, null, onlyConsole);
    }

    public static void Error(LogModule src, string message, Exception e = null)
    {
        Log(LogSeverity.Error, src, message, e);
    }

    public static void Critical(LogModule src, string message, Exception e = null)
    {
        Log(LogSeverity.Critical, src, message, e);
    }

    public static void Warn(LogModule src, string message, Exception e = null)
    {
        Log(LogSeverity.Warning, src, message, e);
    }

    private static void Execute(LogSeverity s, LogModule src, string message, Exception e)
    {
        StringBuilder contentFile = new();
        (Color color, string value) = VerifySeverity(s);
        Append($"{value}".PadRight(22), color);

        DateTime dt = DateTime.Now.ToLocalTime();
        _ = contentFile.Append($"[{dt.FormatDate()}] {value}» ");

        (color, value) = VerifySource(src);
        Append($"{value}".PadRight(18), color);
        _ = contentFile.Append($"{value}» ");

        if (!string.IsNullOrWhiteSpace(message))
        {
            Append(message, Color.White, ref contentFile);
        }

        if (e != null)
        {
            string toWrite = $"{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}";
            Append(toWrite, Color.IndianRed, ref contentFile);
        }

        Console.Write(Environment.NewLine);

        _ = contentFile.AppendLine();
        File.AppendAllText(NormalizeLogFilePath(LogTempFile, DateTime.Now, LogFolder), contentFile.ToString());
        if (e != null)
        {
            File.AppendAllText(NormalizeLogFilePath(LogTempFile, DateTime.Now, LogFolder), e.ToString());
        }
    }

    private static void ExecuteWithoutConsole(LogSeverity s, LogModule src, string message)
    {
        StringBuilder contentFile = new();

        (_, string value) = VerifySeverity(s);
        DateTime dt = DateTime.Now.ToLocalTime();
        _ = contentFile.Append($"[{dt.FormatDate()}] {value}» ");

        (_, value) = VerifySource(src);
        _ = contentFile.Append($"{value}» ");

        if (!string.IsNullOrWhiteSpace(message))
        {
            _ = contentFile.Append(message);
        }

        _ = contentFile.AppendLine();
        File.AppendAllText(NormalizeLogFilePath(LogTempFile, DateTime.Now, LogFolder), contentFile.ToString());
    }

    private static string NormalizeLogFilePath(string logFile, DateTime date, string pathToCheck)
    {
        string today = $"{date.Month:00}-{date.Day:00}-{date.Year:00}";
        bool directoryExist = Directory.Exists($"{pathToCheck}/{today}");
        if (!directoryExist)
        {
            _ = Directory.CreateDirectory($"{pathToCheck}/{today}");
        }
        return logFile.Replace("temp_", $"{today}/");
    }

    private static void Append(string m, Color c)
    {
        Console.ForegroundColor = c;
        Console.Write(m);
    }

    private static void Append(string m, Color c, ref StringBuilder sb)
    {
        Console.ForegroundColor = c;
        Console.Write(m);
        _ = sb.Append(m);
    }

    private static (Color Color, string Source) VerifySource(LogModule source)
    {
        return source switch
        {
            LogModule.PickAndBan => (Color.RoyalBlue, "[PICK AND BAN]"),
            LogModule.AutoChat => (Color.RoyalBlue, "[AUTO CHAT]"),
            LogModule.AutoAccept => (Color.Gold, "[AUTO ACCEPT]"),
            LogModule.LobbyRevealer => (Color.LimeGreen, "[LOBBY REVEALER]"),
            LogModule.LanguageChanger => (Color.Red, "[LANGUAGE CHANGER]"),
            LogModule.Loly => (Color.DarkGreen, "[LOLY TOOLS]"),
            _ => throw new InvalidOperationException($"The specified LogSource {source} is invalid.")
        };
    }

    private static (Color Color, string Level) VerifySeverity(LogSeverity severity)
    {
        DateTime date = DateTime.Now.ToLocalTime();
        return severity switch
        {
            LogSeverity.Critical => (Color.Maroon, $"[{date:HH:mm:ss}][CRITICAL]"),
            LogSeverity.Error => (Color.DarkRed, $"[{date:HH:mm:ss}][ERROR]"),
            LogSeverity.Warning => (Color.Yellow, $"[{date:HH:mm:ss}][WARNING]"),
            LogSeverity.Info => (Color.SpringGreen, $"[{date:HH:mm:ss}][INFO]"),
            LogSeverity.Debug => (Color.SandyBrown, $"[{date:HH:mm:ss}][DEBUG]"),
            _ => throw new InvalidOperationException($"The specified LogSeverity ({severity}) is invalid.")
        };
    }
}