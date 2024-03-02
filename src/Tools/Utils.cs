using Alba.CsConsoleFormat;
using Loly.src.Logs;
using Loly.src.Menus.Core;
using Loly.src.Variables;
using Loly.src.Variables.Enums;
using System.Diagnostics;
using Console = Colorful.Console;

namespace Loly.src.Tools;

public class Utils
{
    public static int TopLength = 0;

    public static Span CreateSpan(string content, int espaces, ConsoleColor color)
    {
        string spaces = "";
        for (int i = 0; i < espaces; i++)
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
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }).Close();
        }
    }

    public static void UpdateMenuTitle(string active)
    {
        Console.Title = active switch
        {
            "main" => $"{Global.SoftName} by {Global.SoftAuthor} - Main Menu",
            "logs" => $"{Global.SoftName} by {Global.SoftAuthor} - Logs Menu",
            "tools" => $"{Global.SoftName} by {Global.SoftAuthor} - Tools Menu",
            "settings" => $"{Global.SoftName} by {Global.SoftAuthor} - Settings Menu",
            "credits" => $"{Global.SoftName} by {Global.SoftAuthor} - Credits Menu",
            "lv" => $"{Global.SoftName} by {Global.SoftAuthor} - Lobby Revealer Menu",
            "lv_get_opgg" => $"{Global.SoftName} by {Global.SoftAuthor} > Lobby Revealer - Get OP.GG Menu",
            "lv_get_stats" => $"{Global.SoftName} by {Global.SoftAuthor} > Lobby Revealer - Get Stats Menu",
            "aa" => $"{Global.SoftName} by {Global.SoftAuthor} - Auto Accept Menu",
            "pnb" => $"{Global.SoftName} by {Global.SoftAuthor} - Pick'n'Ban Menu",
            "pnb_pob" => $"{Global.SoftName} by {Global.SoftAuthor} > Pick'n'Ban - Pick or Ban Menu",
            "pnb_pick" => $"{Global.SoftName} by {Global.SoftAuthor} > Pick'n'Ban - Pick Menu",
            "pnb_ban" => $"{Global.SoftName} by {Global.SoftAuthor} > Pick'n'Ban - Ban Menu",
            "pnb_pick_c" => $"{Global.SoftName} by {Global.SoftAuthor} > Pick'n'Ban - Pick Champion Menu",
            "pnb_ban_c" => $"{Global.SoftName} by {Global.SoftAuthor} > Pick'n'Ban - Ban Champion Menu",
            "pnb_pick_delay" => $"{Global.SoftName} by {Global.SoftAuthor} > Pick'n'Ban - Pick Champion Delay Menu",
            "pnb_ban_delay" => $"{Global.SoftName} by {Global.SoftAuthor} > Pick'n'Ban - Ban Champion Delay Menu",
            "pnb_pick_del_c" => $"{Global.SoftName} by {Global.SoftAuthor} > Pick'n'Ban - Remove Pick Champion Menu",
            "pnb_ban_del_c" => $"{Global.SoftName} by {Global.SoftAuthor} > Pick'n'Ban - Remove Ban Champion Menu",
            "ac" => $"{Global.SoftName} by {Global.SoftAuthor} - Auto Chat Menu",
            "ac_add" => $"{Global.SoftName} by {Global.SoftAuthor} > Auto Chat - Add Message Menu",
            "ac_del" => $"{Global.SoftName} by {Global.SoftAuthor} > Auto Chat - Delete Message Menu",
            "ac_see" => $"{Global.SoftName} by {Global.SoftAuthor} > Auto Chat - See Messages Menu",
            "ac_clear" => $"{Global.SoftName} by {Global.SoftAuthor} > Auto Chat - Clear Messages Menu",
            "lc" => $"{Global.SoftName} by {Global.SoftAuthor} - Language Changer Menu",
            "lc_languages" => $"{Global.SoftName} by {Global.SoftAuthor} > Language Changer - Languages Menu",
            "lc_exe" => $"{Global.SoftName} by {Global.SoftAuthor} > Language Changer - Get League Client Exe Menu",
            "lc_confirm" => $"{Global.SoftName} by {Global.SoftAuthor} > Language Changer - Confirm Menu",
            _ => Console.Title
        };
    }

    public static string LrParse(string source, string left, string right, int innt = 0)
    {
        int length = left.Length;
        string result = "";
        int num = source.IndexOf(left, innt, StringComparison.Ordinal);
        int num2 = source.IndexOf(right, num + length, StringComparison.Ordinal);
        if (num != -1 & num2 != -1)
        {
            result = source[(num + length)..num2];
        }

        return result;
    }

    public static void LogNewError(string actionName, LogModule logModule, Exception ex)
    {
        Logger.Error(logModule, $"An error occured to execute : {actionName}", null, Global.LogsMenuEnable ? LogType.Both : LogType.File);
        Logger.Error(logModule, "Please check the logs for more information.", null, Global.LogsMenuEnable ? LogType.Both : LogType.File);
        Logger.Error(logModule, "Error message : ", ex, Global.LogsMenuEnable ? LogType.Both : LogType.File);
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
        System.Console.Clear();
        Interface.ShowArt();
    }

    public static string FindString(string text, string from, string to)
    {
        int pFrom = text.IndexOf(from, StringComparison.Ordinal) + from.Length;
        int pTo = text.LastIndexOf(to, StringComparison.Ordinal);

        return text[pFrom..pTo];
    }

    public static string FormatBytes(long bytes, bool seconds, int decimals = 2)
    {
        if (bytes == 0)
        {
            return "0 octets";
        }

        const int k = 1024;
        int dm = decimals < 0 ? 0 : decimals;
        string[] sizes = { "Octets", "Ko", "Mo", "Go", "To", "Po", "Eo", "Zo", "Yo" };

        int i = (int)Math.Floor(Math.Log(bytes) / Math.Log(k));
        return $"{(bytes / Math.Pow(k, i)).ToString($"F{dm}")} {sizes[i]}{(seconds ? "/s" : "")}";
    }

    public static void CreateTask(Action action, string errorMessage, LogModule logModule)
    {
        Task.Run(action)
            .ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    LogNewError(errorMessage, logModule, t.Exception);
                }
            });
    }
}