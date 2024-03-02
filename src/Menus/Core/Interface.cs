using Loly.src.Variables;
using Loly.src.Variables.Class;
using static Loly.src.Tools.Utils;

namespace Loly.src.Menus.Core;

public class Interface
{
    public const string ArtName = @"
██╗      ██████╗ ██╗  ██╗   ██╗    ████████╗ ██████╗  ██████╗ ██╗     ███████╗    
██║     ██╔═══██╗██║  ╚██╗ ██╔╝    ╚══██╔══╝██╔═══██╗██╔═══██╗██║     ██╔════╝    
██║     ██║   ██║██║   ╚████╔╝        ██║   ██║   ██║██║   ██║██║     ███████╗    
██║     ██║   ██║██║    ╚██╔╝         ██║   ██║   ██║██║   ██║██║     ╚════██║    
███████╗╚██████╔╝███████╗██║          ██║   ╚██████╔╝╚██████╔╝███████╗███████║    
╚══════╝ ╚═════╝ ╚══════╝╚═╝          ╚═╝    ╚═════╝  ╚═════╝ ╚══════╝╚══════╝    ";

    public const string ArtNameLogs = @"
██╗      ██████╗ ██╗  ██╗   ██╗    ██╗      ██████╗  ██████╗ ███████╗
██║     ██╔═══██╗██║  ╚██╗ ██╔╝    ██║     ██╔═══██╗██╔════╝ ██╔════╝
██║     ██║   ██║██║   ╚████╔╝     ██║     ██║   ██║██║  ███╗███████╗
██║     ██║   ██║██║    ╚██╔╝      ██║     ██║   ██║██║   ██║╚════██║
███████╗╚██████╔╝███████╗██║       ███████╗╚██████╔╝╚██████╔╝███████║
╚══════╝ ╚═════╝ ╚══════╝╚═╝       ╚══════╝ ╚═════╝  ╚═════╝ ╚══════╝";

    public static void ShowArt()
    {
        string[] array = ArtName.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        uint num2 = 0;
        while (num2 < array.Length)
        {
            string text2 = array[num2];
            Console.SetCursorPosition((Console.WindowWidth - text2.Length) / 2, Console.CursorTop);
            DisplayColor(text2, Colors.PrimaryColor, Colors.InfoColor);
            num2 += 1;
        }

        const string text3 = "League Of Legends";
        Console.SetCursorPosition((Console.WindowWidth - text3.Length) / 2, Console.CursorTop);
        DisplayColor("`L`eague `O`f `L`egends ", Colors.InfoColor, Colors.PrimaryColor);
        string text4 = $"Loly Tools by {Global.SoftAuthor}";
        Console.SetCursorPosition((Console.WindowWidth - text4.Length) / 2, Console.CursorTop);
        DisplayColor($"Loly Tools by `{Global.SoftAuthor}`", Colors.InfoColor, Colors.PrimaryColor);
        ShowLoggedUser();
        Console.Write(Environment.NewLine);
        Console.Write(Environment.NewLine);

        TopLength = array.Length + 5;
    }

    public static void ShowLogsArt()
    {
        string[] array = ArtNameLogs.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        uint num2 = 0;
        while (num2 < array.Length)
        {
            string text2 = array[num2];
            Console.SetCursorPosition((Console.WindowWidth - text2.Length) / 2, Console.CursorTop);
            DisplayColor(text2, Colors.PrimaryColor, Colors.InfoColor);
            num2 += 1;
        }

        const string text1 = "Loly Tools Logs";
        Console.SetCursorPosition((Console.WindowWidth - text1.Length) / 2, Console.CursorTop);
        DisplayColor("`L`oly `T`ools `L`ogs", Colors.InfoColor, Colors.PrimaryColor);
        ShowLoggedUser();
        Console.Write(Environment.NewLine);
        Console.Write(Environment.NewLine);

        TopLength = array.Length + 5;
    }

    private static void ShowLoggedUser()
    {
        if (Global.SummonerLogged.SummonerId != null)
        {
            string text = $"Logged in as {Global.SummonerLogged.GetFullGameName()} [Summoner ID : {Global.SummonerLogged.SummonerId}]";
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            DisplayColor($"Logged in as `{Global.SummonerLogged.GetFullGameName()}` [Summoner ID : `{Global.SummonerLogged.SummonerId}`]", Colors.InfoColor, Colors.PrimaryColor);
        }
        else
        {
            string text = "Not logged in for the moment";
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            DisplayColor("Not `logged in` for the moment", Colors.InfoColor, Colors.PrimaryColor);
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
}