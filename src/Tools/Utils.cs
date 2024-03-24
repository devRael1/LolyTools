using System.Diagnostics;

using Alba.CsConsoleFormat;

using Loly.src.Logs;
using Loly.src.Menus.Core;
using Loly.src.Variables.Enums;

namespace Loly.src.Tools;

public static class Utils
{
    public static Span CreateSpan(string content, int espaces, ConsoleColor color)
    {
        var spaces = "";
        for (var i = 0; i < espaces; i++)
        {
            spaces += " ";
        }

        return new Span(spaces + content) { Color = color };
    }

    public static string CheckBoolean(bool value)
    {
        return value ? "On" : "Off";
    }

    public static void OpenUrl(string url)
    {
        try
        {
            Process.Start(url).Close();
        }
        catch
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }).Close();
        }
    }

    public static string LrParse(string source, string left, string right, int innt = 0)
    {
        if (source == null) return null;

        var length = left.Length;
        var result = "";
        var num = source.IndexOf(left, innt, StringComparison.Ordinal);
        var num2 = source.IndexOf(right, num + length, StringComparison.Ordinal);
        if (num != -1 & num2 != -1)
        {
            result = source[(num + length)..num2];
        }

        return result;
    }

    public static void LogNewError(string actionName, LogModule logModule, Exception ex)
    {
        Logger.Error(logModule, $"An error occured to execute : {actionName}", null);
        Logger.Error(logModule, "Please check the logs file for more information.", null);
        Logger.Error(logModule, "Error : ", ex);
    }

    public static string FormatStr(string str)
    {
        str = str.ToLower();
        return string.Concat(str[0].ToString().ToUpper(), str.AsSpan(1));
    }

    public static string FormatMessage(string message)
    {
        return message.Replace("\"", "'").Replace("\\", "");
    }

    public static void ResetConsole()
    {
        Console.Clear();
        Interface.ShowArt();
    }

    public static string FindString(string text, string from, string to)
    {
        var pFrom = text.IndexOf(from, StringComparison.Ordinal) + from.Length;
        var pTo = text.LastIndexOf(to, StringComparison.Ordinal);

        return text[pFrom..pTo];
    }

    public static string FormatBytes(long bytes, bool seconds, int decimals = 2)
    {
        if (bytes == 0)
        {
            return "0 octets";
        }

        const int k = 1024;
        var dm = decimals < 0 ? 0 : decimals;
        string[] sizes = { "Octets", "Ko", "Mo", "Go", "To", "Po", "Eo", "Zo", "Yo" };

        var i = (int)Math.Floor(Math.Log(bytes) / Math.Log(k));
        return $"{(bytes / Math.Pow(k, i)).ToString($"F{dm}")} {sizes[i]}{(seconds ? "/s" : "")}";
    }

    public static void CreateBackgroundTask(Action action, string errorMessage, LogModule logModule)
    {
        Task.Run(action)
            .ContinueWith(t =>
            {
                if (t.IsFaulted) LogNewError(errorMessage, logModule, t.Exception);
            });
    }

    public static void DisplayColor(string message, ConsoleColor color, ConsoleColor color2)
    {
        var estEnCouleurSecondaire = false;

        foreach (var caractere in message)
        {
            if (caractere == '`')
            {
                estEnCouleurSecondaire = !estEnCouleurSecondaire;
                continue;
            }

            Console.ForegroundColor = estEnCouleurSecondaire ? color2 : color;
            Console.Write(caractere);
        }
        Console.Write(Environment.NewLine);
        Console.ResetColor();
    }

    public static string RegionId(Region region)
    {
        return region switch
        {
            Region.BR => "br1",
            Region.EUN => "eun1",
            Region.EUW => "euw1",
            Region.JP => "jp1",
            Region.KR => "kr",
            Region.LA1 => "la1",
            Region.LA2 => "la2",
            Region.NA => "na1",
            Region.OC => "oc1",
            Region.TR => "tr1",
            Region.RU => "ru",
            Region.PH => "ph2",
            Region.SG => "sg2",
            Region.TH => "th2",
            Region.TW => "tw2",
            Region.VN => "vn2",
            _ => "euw1"
        };
    }
}