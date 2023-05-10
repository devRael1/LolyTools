using System.Diagnostics;
using Alba.CsConsoleFormat;
using Loly.Variables;
using Console = Colorful.Console;

namespace Loly.Tools;

public class Utils
{
    public static int TopLength = 0;

    public static Span CreateSpan(string content, int espaces, ConsoleColor color)
    {
        string spaces = "";
        for (int i = 0; i < espaces; i++)
            spaces += " ";
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
            "tools" => $"{Global.SoftName} by {Global.SoftAuthor} - Tools Menu",
            "settings" => $"{Global.SoftName} by {Global.SoftAuthor} - Settings Menu",
            "credits" => $"{Global.SoftName} by {Global.SoftAuthor} - Credits Menu",
            "lv" => $"{Global.SoftName} by {Global.SoftAuthor} - Lobby Revealer Menu",
            "lv_get_names" => $"{Global.SoftName} by {Global.SoftAuthor} > Lobby Revealer - Get Names Menu",
            "lv_get_opgg" => $"{Global.SoftName} by {Global.SoftAuthor} > Lobby Revealer - Get OP.GG Menu",
            "lv_get_stats" => $"{Global.SoftName} by {Global.SoftAuthor} > Lobby Revealer - Get Stats Menu",
            "aa" => $"{Global.SoftName} by {Global.SoftAuthor} - Auto Accept Menu",
            "pnb" => $"{Global.SoftName} by {Global.SoftAuthor} - Pick'n'Ban Menu",
            "pnb_pick" => $"{Global.SoftName} by {Global.SoftAuthor} > Pick'n'Ban - Pick Champion Menu",
            "pnb_ban" => $"{Global.SoftName} by {Global.SoftAuthor} > Pick'n'Ban - Ban Champion Menu",
            "pnb_pick_delay" => $"{Global.SoftName} by {Global.SoftAuthor} > Pick'n'Ban - Pick Champion Delay Menu",
            "pnb_ban_delay" => $"{Global.SoftName} by {Global.SoftAuthor} > Pick'n'Ban - Ban Champion Delay Menu",
            _ => Console.Title
        };
    }

    public static string LrParse(string source, string left, string right, int innt = 0)
    {
        int length = left.Length;
        string result = "";
        int num = source.IndexOf(left, innt, StringComparison.Ordinal);
        int num2 = source.IndexOf(right, num + length, StringComparison.Ordinal);
        if ((num != -1) & (num2 != -1)) result = source.Substring(num + length, num2 - (num + length));
        return result;
    }

    public static void ShowError(Exception ex)
    {
        Console.Clear();
        Console.WriteLine("An unexpected error occurred!\n", Colors.ErrorColor);
        Console.WriteLine(ex.Message, Colors.ErrorColor);
        Console.WriteLine(ex.StackTrace, Colors.ErrorColor);
        Console.WriteLine("\n Press Enter to close application...", Colors.ErrorColor);
        Console.ReadKey();
        Environment.Exit(0);
    }

    public static string FormatStr(string str)
    {
        str = str.ToLower();
        return string.Concat(str[0].ToString().ToUpper(), str.AsSpan(1));
    }
}