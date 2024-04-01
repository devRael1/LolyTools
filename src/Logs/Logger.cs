using System.Text;

using Gommon;

using Loly.src.Menus.Core;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;

namespace Loly.src.Logs;

public static class Logger
{
    public const string LogFolder = "Logs";
    public const string LolSettingsFolder = "LoLSettings";
    private const string LogLolyFile = $"{LogFolder}/temp_loly.log";
    private const string LogReqsFile = $"{LogFolder}/temp_requests.log";
    private const string LogErrorsFile = $"{LogFolder}/temp_errors.log";

    private static readonly object Lock = new();
    private static readonly object Lock2 = new();
    private static readonly object Lock3 = new();

    private static bool _headerPrinted;

    static Logger()
    {
        Directory.CreateDirectory(LogFolder);
        Directory.CreateDirectory(LolSettingsFolder);
    }

    private static void Log(LogSeverity s, LogModule from, string message, LogType logType = LogType.None)
    {
        if (logType == LogType.Both) Lock.Lock(() => Execute(s, from, message, null));
        else if (logType == LogType.File) Lock.Lock(() => ExecuteWithoutConsole(s, from, message, null));
        else Lock.Lock(() => ExecuteOnlyInConsole(s, from, message));
    }

    private static void Log(IRequest value)
    {
        Lock2.Lock(() => ExecuteLogRequest(value));
    }

    private static void Log(string message, Exception e, LogType logType = LogType.None)
    {
        if (logType == LogType.Both) Lock3.Lock(() => Execute(LogSeverity.Error, LogModule.Loly, message, e));
        else if (logType == LogType.File) Lock3.Lock(() => ExecuteWithoutConsole(LogSeverity.Error, LogModule.Loly, message, e));
        else Lock3.Lock(() => ExecuteOnlyInConsole(LogSeverity.Error, LogModule.Loly, message));
    }

    public static void PrintHeader()
    {
        if (!_headerPrinted)
        {
            Interface.ArtName.Split("\n", StringSplitOptions.TrimEntries).ForEach(ln => Info(LogModule.Loly, ln, LogType.File));
            Info(LogModule.Loly, $"Currently running Loly Tools V{Version.FullVersion}.", LogType.File);
            _headerPrinted = true;
        }
    }

    public static void Info(LogModule src, string message, LogType logType = LogType.None)
    {
        if (logType == LogType.None) logType = Global.LogsMenuEnable ? LogType.Both : LogType.File;
        Log(LogSeverity.Info, src, message, logType);
    }

    public static void Error(string message, Exception e = null, LogType logType = LogType.None)
    {
        if (logType == LogType.None) logType = Global.LogsMenuEnable ? LogType.Both : LogType.File;
        Log(message, e, logType);
    }

    public static void Warn(LogModule src, string message, LogType logType = LogType.None)
    {
        if (logType == LogType.None) logType = Global.LogsMenuEnable ? LogType.Both : LogType.File;
        Log(LogSeverity.Warning, src, message, logType);
    }

    public static void Request(IRequest value)
    {
        Log(value);
    }

    private static void Execute(LogSeverity s, LogModule module, string message, Exception e)
    {
        StringBuilder contentFile = new();
        (ConsoleColor color, var value) = VerifySeverity(s);
        Append($"{value}", color);

        DateTime dt = DateTime.Now.ToLocalTime();
        contentFile.Append($"[{dt.FormatDate()}]{value}");

        (color, value) = VerifySource(module);
        Append($"{value}» ", color);
        contentFile.Append($"{value}» ");

        if (!string.IsNullOrWhiteSpace(message)) Append(message, color, ref contentFile);
        if (e != null) Append(Environment.NewLine + e.ToString(), Colors.ErrorColor);

        Console.Write(Environment.NewLine);

        contentFile.AppendLine();
        if (s != LogSeverity.Error) File.AppendAllText(NormalizeLogFilePath(LogLolyFile, DateTime.Now, LogFolder), contentFile.ToString());
        else File.AppendAllText(NormalizeLogFilePath(LogErrorsFile, DateTime.Now, LogFolder), contentFile.ToString());

        if (e != null) File.AppendAllText(NormalizeLogFilePath(LogErrorsFile, DateTime.Now, LogFolder), e.ToString() + Environment.NewLine);
    }

    private static void ExecuteWithoutConsole(LogSeverity s, LogModule module, string message, Exception e)
    {
        StringBuilder contentFile = new();

        (_, var value) = VerifySeverity(s);
        DateTime dt = DateTime.Now.ToLocalTime();
        contentFile.Append($"[{dt.FormatDate()}]{value}");

        (_, value) = VerifySource(module);
        contentFile.Append($"{value}» ");

        if (!string.IsNullOrWhiteSpace(message)) contentFile.Append(message);

        contentFile.AppendLine();
        if (s != LogSeverity.Error) File.AppendAllText(NormalizeLogFilePath(LogLolyFile, DateTime.Now, LogFolder), contentFile.ToString());
        else File.AppendAllText(NormalizeLogFilePath(LogErrorsFile, DateTime.Now, LogFolder), contentFile.ToString());

        if (e != null) File.AppendAllText(NormalizeLogFilePath(LogErrorsFile, DateTime.Now, LogFolder), e.ToString() + Environment.NewLine);
    }

    private static void ExecuteOnlyInConsole(LogSeverity s, LogModule module, string message, Exception e = null)
    {
        (ConsoleColor color, var value) = VerifySeverity(s);
        Append($"{value}", color);

        (color, value) = VerifySource(module);
        Append($"{value}» ", color);

        if (!string.IsNullOrWhiteSpace(message))
        {
            Append(message, color);
        }

        if (e != null) Append(Environment.NewLine + e.ToString(), Colors.ErrorColor);
        Console.Write(Environment.NewLine);
    }

    private static void ExecuteLogRequest(IRequest valueOfRequest)
    {
        StringBuilder contentFile = new();

        (_, var value) = VerifySeverity(LogSeverity.Debug);
        DateTime dt = DateTime.Now.ToLocalTime();
        contentFile.Append($"[{dt.FormatDate()}]{value}");

        (_, value) = VerifySource(LogModule.Request);
        contentFile.Append($"{value}» ");

        if (valueOfRequest is Request request)
        {
            if (!string.IsNullOrWhiteSpace(request.Method) || !string.IsNullOrWhiteSpace(request.Url))
            {
                contentFile.Append($"Request to API > Endpoint: {request.Url} - Method: {request.Method}");
                if (!string.IsNullOrWhiteSpace(request.Body)) contentFile.Append($" - Payload: {request.Body}");
            }
        }

        if (valueOfRequest is Response response)
        {
            if (!string.IsNullOrWhiteSpace(response.Method))
            {
                contentFile.Append($"Response from API > Endpoint: {response.Url} - Status: {response.StatusCode} - Method: {response.Method}");
                if (response.Data != null) contentFile.Append($" - Response: {response.Data[1]}");
            }
        }

        if (valueOfRequest.Exception != null)
        {
            var toWrite = $"{Environment.NewLine}{valueOfRequest.Exception.Message}{Environment.NewLine}{valueOfRequest.Exception.StackTrace}";
            contentFile.Append(toWrite);
        }

        contentFile.AppendLine();
        File.AppendAllText(NormalizeLogFilePath(LogReqsFile, DateTime.Now, LogFolder), contentFile.ToString());
        if (valueOfRequest.Exception != null) File.AppendAllText(NormalizeLogFilePath(LogReqsFile, DateTime.Now, LogFolder), valueOfRequest.Exception.ToString() + Environment.NewLine);
    }

    private static string NormalizeLogFilePath(string logFile, DateTime date, string pathToCheck)
    {
        var today = $"{date.Month:00}-{date.Day:00}-{date.Year:00}";
        var directoryExist = Directory.Exists($"{pathToCheck}/{today}");
        if (!directoryExist) Directory.CreateDirectory($"{pathToCheck}/{today}");
        return logFile.Replace("temp_", $"{today}/");
    }

    private static void Append(string m, ConsoleColor c)
    {
        Console.ForegroundColor = c;
        Console.Write(m);
    }

    private static void Append(string m, ConsoleColor c, ref StringBuilder sb)
    {
        Console.ForegroundColor = c;
        Console.Write(m);
        sb.Append(m);
    }

    private static (ConsoleColor Color, string Source) VerifySource(LogModule source)
    {
        return source switch
        {
            LogModule.PickAndBan => (ConsoleColor.Blue, "[PICK AND BAN]"),
            LogModule.AutoChat => (ConsoleColor.Yellow, "[AUTO CHAT]"),
            LogModule.AutoAccept => (ConsoleColor.Yellow, "[AUTO ACCEPT]"),
            LogModule.LobbyRevealer => (ConsoleColor.Magenta, "[LOBBY REVEALER]"),
            LogModule.Loly => (ConsoleColor.Cyan, "[LOLY TOOLS]"),
            LogModule.Tasks => (ConsoleColor.DarkCyan, "[TASKS]"),
            LogModule.Request => (ConsoleColor.Red, "[REQUEST]"),
            LogModule.LolSettings => (ConsoleColor.DarkCyan, "[LOL SETTINGS]"),
            LogModule.Updater => (ConsoleColor.White, "[UPDATER]"),
            _ => throw new InvalidOperationException($"The specified LogSource {source} is invalid.")
        };
    }

    private static (ConsoleColor Color, string Level) VerifySeverity(LogSeverity severity)
    {
        DateTime date = DateTime.Now.ToLocalTime();
        return severity switch
        {
            LogSeverity.Error => (ConsoleColor.DarkRed, $"[{date:HH:mm:ss}][ERROR]"),
            LogSeverity.Warning => (ConsoleColor.DarkYellow, $"[{date:HH:mm:ss}][WARNING]"),
            LogSeverity.Info => (ConsoleColor.DarkGreen, $"[{date:HH:mm:ss}][INFORMATION]"),
            LogSeverity.Debug => (ConsoleColor.DarkGray, $"[{date:HH:mm:ss}][DEBUG]"),
            _ => throw new InvalidOperationException($"The specified LogSeverity ({severity}) is invalid.")
        };
    }
}