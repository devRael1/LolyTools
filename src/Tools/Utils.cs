using System.Diagnostics;

using Alba.CsConsoleFormat;

using Loly.src.Menus.Core;
using Loly.src.Variables.Enums;

namespace Loly.src.Tools;

internal static class Utils
{
    internal static Span CreateSpan(string content, int espaces, ConsoleColor color)
    {
        var spaces = "";
        for (var i = 0; i < espaces; i++)
        {
            spaces += " ";
        }

        return new Span(spaces + content) { Color = color };
    }

    internal static string CheckBoolean(bool value)
    {
        return value ? "On" : "Off";
    }

    internal static void OpenUrl(string url)
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

    internal static string FormatStr(string str)
    {
        str = str.ToLower();
        return string.Concat(str[0].ToString().ToUpper(), str.AsSpan(1));
    }

    internal static string FormatMessage(string message)
    {
        return message.Replace("\"", "'").Replace("\\", "");
    }

    internal static void ResetConsole()
    {
        Console.Clear();
        Interface.ShowArt();
    }

    internal static string FindString(string text, string from, string to)
    {
        var pFrom = text.IndexOf(from, StringComparison.Ordinal) + from.Length;
        var pTo = text.LastIndexOf(to, StringComparison.Ordinal);

        return text[pFrom..pTo];
    }

    internal static string FormatBytes(long bytes, bool seconds, int decimals = 2)
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

    internal static void CreateBackgroundTask(Action action, string errorMessage)
    {
        Task.Run(action)
            .ContinueWith(t =>
            {
                if (t.IsFaulted) throw new Exception(errorMessage, t.Exception);
            });
    }

    internal static void DisplayColor(string message, ConsoleColor color, ConsoleColor color2)
    {
        var secondaryColor = false;

        foreach (var @char in message)
        {
            if (@char == '`')
            {
                secondaryColor = !secondaryColor;
                continue;
            }

            Console.ForegroundColor = secondaryColor ? color2 : color;
            Console.Write(@char);
        }
        Console.Write(Environment.NewLine);
        Console.ResetColor();
    }

    internal static string RegionId(Region region)
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