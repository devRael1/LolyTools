﻿using Gommon;
using Loly.src.Menus.Core;
using Loly.src.Variables.Enums;
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
    private static readonly object Lock3 = new();
    private static bool _headerPrinted;

    static Logger()
    {
        Directory.CreateDirectory(LogFolder);
    }

    private static void Log(LogSeverity s, LogModule from, string message, Exception e = null, LogType logType = LogType.Both)
    {
        if (logType == LogType.Both)
        {
            Lock.Lock(() => Execute(s, from, message, e));
        }
        else if (logType == LogType.File)
        {
            Lock2.Lock(() => ExecuteWithoutConsole(s, from, message));
        }
        else
        {
            Lock3.Lock(() => ExecuteOnlyInConsole(s, from, message));
        }
    }

    internal static void PrintHeader()
    {
        if (!_headerPrinted)
        {
            Interface.ArtName.Split("\n", StringSplitOptions.TrimEntries).ForEach(ln => Info(LogModule.Loly, ln, LogType.File));
            Info(LogModule.Loly, $"Currently running Loly Tools V{Version.FullVersion}.", LogType.File);
            _headerPrinted = true;
        }
    }

    public static void Debug(LogModule src, string message)
    {
        Log(LogSeverity.Debug, src, message);
    }

    public static void Info(LogModule src, string message, LogType logType = LogType.Both)
    {
        Log(LogSeverity.Info, src, message, null, logType);
    }

    public static void Error(LogModule src, string message, Exception e = null, LogType logType = LogType.Both)
    {
        Log(LogSeverity.Error, src, message, e, logType);
    }

    public static void Warn(LogModule src, string message, Exception e = null, LogType logType = LogType.Both)
    {
        Log(LogSeverity.Warning, src, message, e, logType);
    }

    private static void Execute(LogSeverity s, LogModule src, string message, Exception e)
    {
        StringBuilder contentFile = new();
        (Color color, string value) = VerifySeverity(s);
        Append($"{value}", color);

        DateTime dt = DateTime.Now.ToLocalTime();
        contentFile.Append($"[{dt.FormatDate()}] {value}» ");

        (color, value) = VerifySource(src);
        Append($"{value}» ", color);
        contentFile.Append($"{value}» ");

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

        contentFile.AppendLine();
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
        contentFile.Append($"[{dt.FormatDate()}] {value}» ");

        (_, value) = VerifySource(src);
        contentFile.Append($"{value}» ");

        if (!string.IsNullOrWhiteSpace(message))
        {
            contentFile.Append(message);
        }

        contentFile.AppendLine();
        File.AppendAllText(NormalizeLogFilePath(LogTempFile, DateTime.Now, LogFolder), contentFile.ToString());
    }

    private static void ExecuteOnlyInConsole(LogSeverity s, LogModule src, string message)
    {
        (Color color, string value) = VerifySeverity(s);
        Append($"{value}", color);

        DateTime dt = DateTime.Now.ToLocalTime();
        Append($"[{dt.FormatDate()}] {value}» ", color);

        (color, value) = VerifySource(src);
        Append($"{value}» ", color);

        if (!string.IsNullOrWhiteSpace(message))
        {
            Append(message, Color.White);
        }

        Console.Write(Environment.NewLine);
    }

    private static string NormalizeLogFilePath(string logFile, DateTime date, string pathToCheck)
    {
        string today = $"{date.Month:00}-{date.Day:00}-{date.Year:00}";
        bool directoryExist = Directory.Exists($"{pathToCheck}/{today}");
        if (!directoryExist)
        {
            Directory.CreateDirectory($"{pathToCheck}/{today}");
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
        sb.Append(m);
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
            LogModule.Tasks => (Color.Aqua, "[TASKS]"),
            _ => throw new InvalidOperationException($"The specified LogSource {source} is invalid.")
        };
    }

    private static (Color Color, string Level) VerifySeverity(LogSeverity severity)
    {
        DateTime date = DateTime.Now.ToLocalTime();
        return severity switch
        {
            LogSeverity.Error => (Color.DarkRed, $"[{date:HH:mm:ss}][ERROR]"),
            LogSeverity.Warning => (Color.Yellow, $"[{date:HH:mm:ss}][WARNING]"),
            LogSeverity.Info => (Color.SpringGreen, $"[{date:HH:mm:ss}][INFORMATION]"),
            LogSeverity.Debug => (Color.SandyBrown, $"[{date:HH:mm:ss}][DEBUG]"),
            _ => throw new InvalidOperationException($"The specified LogSeverity ({severity}) is invalid.")
        };
    }
}