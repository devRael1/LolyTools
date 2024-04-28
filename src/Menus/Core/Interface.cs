using Loly.src.Variables.Class;

using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Menus.Core;

internal static class Interface
{
    internal const string ArtName = @"
██╗      ██████╗ ██╗  ██╗   ██╗    ████████╗ ██████╗  ██████╗ ██╗     ███████╗    
██║     ██╔═══██╗██║  ╚██╗ ██╔╝    ╚══██╔══╝██╔═══██╗██╔═══██╗██║     ██╔════╝    
██║     ██║   ██║██║   ╚████╔╝        ██║   ██║   ██║██║   ██║██║     ███████╗    
██║     ██║   ██║██║    ╚██╔╝         ██║   ██║   ██║██║   ██║██║     ╚════██║    
███████╗╚██████╔╝███████╗██║          ██║   ╚██████╔╝╚██████╔╝███████╗███████║    
╚══════╝ ╚═════╝ ╚══════╝╚═╝          ╚═╝    ╚═════╝  ╚═════╝ ╚══════╝╚══════╝    ";

    internal const string ArtNameLogs = @"
██╗      ██████╗ ██╗  ██╗   ██╗    ██╗      ██████╗  ██████╗ ███████╗
██║     ██╔═══██╗██║  ╚██╗ ██╔╝    ██║     ██╔═══██╗██╔════╝ ██╔════╝
██║     ██║   ██║██║   ╚████╔╝     ██║     ██║   ██║██║  ███╗███████╗
██║     ██║   ██║██║    ╚██╔╝      ██║     ██║   ██║██║   ██║╚════██║
███████╗╚██████╔╝███████╗██║       ███████╗╚██████╔╝╚██████╔╝███████║
╚══════╝ ╚═════╝ ╚══════╝╚═╝       ╚══════╝ ╚═════╝  ╚═════╝ ╚══════╝";

    internal static void ShowArt()
    {
        var array = ArtName.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        uint num2 = 0;
        while (num2 < array.Length)
        {
            var text2 = array[num2];
            Console.SetCursorPosition((Console.WindowWidth - text2.Length) / 2, Console.CursorTop);
            DisplayColor(text2, Colors.PrimaryColor, Colors.InfoColor);
            num2 += 1;
        }

        const string text3 = "League Of Legends";
        Console.SetCursorPosition((Console.WindowWidth - text3.Length) / 2, Console.CursorTop);
        DisplayColor("`L`eague `O`f `L`egends ", Colors.InfoColor, Colors.PrimaryColor);
        ShowLoggedUser();
        Console.Write(Environment.NewLine);
        Console.Write(Environment.NewLine);

        TopLength = array.Length + 3;
    }

    internal static void ShowLogsArt()
    {
        var array = ArtNameLogs.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        uint num2 = 0;
        while (num2 < array.Length)
        {
            var text2 = array[num2];
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
        if (SummonerLogged.SummonerId != null)
        {
            var text = $"Logged in as {SummonerLogged.GetFullGameName()} [Summoner ID : {SummonerLogged.SummonerId}]";
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            DisplayColor($"Logged in as `{SummonerLogged.GetFullGameName()}` [Summoner ID : `{SummonerLogged.SummonerId}`]", Colors.InfoColor, Colors.PrimaryColor);
        }
        else
        {
            var text = "Not logged in for the moment";
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            DisplayColor("Not `logged in` for the moment", Colors.InfoColor, Colors.PrimaryColor);
        }
    }

    internal static void UpdateMenuTitle(string active)
    {
        Console.Title = active switch
        {
            "main" => $"{SoftName} by {SoftAuthor} - Main Menu",
            "logs" => $"{SoftName} by {SoftAuthor} - Logs Menu",
            "tools" => $"{SoftName} by {SoftAuthor} - Tools Menu",
            "settings" => $"{SoftName} by {SoftAuthor} - Settings Menu",
            "credits" => $"{SoftName} by {SoftAuthor} - Credits Menu",
            "lv" => $"{SoftName} by {SoftAuthor} - Lobby Revealer Menu",
            "lv_get_ugg" => $"{SoftName} by {SoftAuthor} > Lobby Revealer - Get U.GG Menu",
            "lv_get_stats" => $"{SoftName} by {SoftAuthor} > Lobby Revealer - Get Stats Menu",
            "aa" => $"{SoftName} by {SoftAuthor} - Auto Accept Menu",
            "pnb" => $"{SoftName} by {SoftAuthor} - Pick'n'Ban Menu",
            "pnb_pob" => $"{SoftName} by {SoftAuthor} > Pick'n'Ban - Pick or Ban Menu",
            "pnb_pick" => $"{SoftName} by {SoftAuthor} > Pick'n'Ban - Pick Menu",
            "pnb_ban" => $"{SoftName} by {SoftAuthor} > Pick'n'Ban - Ban Menu",
            "pnb_pick_c" => $"{SoftName} by {SoftAuthor} > Pick'n'Ban - Pick Champion Menu",
            "pnb_ban_c" => $"{SoftName} by {SoftAuthor} > Pick'n'Ban - Ban Champion Menu",
            "pnb_pick_delay" => $"{SoftName} by {SoftAuthor} > Pick'n'Ban - Pick Champion Delay Menu",
            "pnb_ban_delay" => $"{SoftName} by {SoftAuthor} > Pick'n'Ban - Ban Champion Delay Menu",
            "pnb_pick_del_c" => $"{SoftName} by {SoftAuthor} > Pick'n'Ban - Remove Pick Champion Menu",
            "pnb_ban_del_c" => $"{SoftName} by {SoftAuthor} > Pick'n'Ban - Remove Ban Champion Menu",
            "pnb_pick_list" => $"{SoftName} by {SoftAuthor} > Pick'n'Ban - Pick Champion List Menu",
            "pnb_ban_list" => $"{SoftName} by {SoftAuthor} > Pick'n'Ban - Ban Champion List Menu",
            "ac" => $"{SoftName} by {SoftAuthor} - Auto Chat Menu",
            "ac_add" => $"{SoftName} by {SoftAuthor} > Auto Chat - Add Message Menu",
            "ac_del" => $"{SoftName} by {SoftAuthor} > Auto Chat - Delete Message Menu",
            "ac_see" => $"{SoftName} by {SoftAuthor} > Auto Chat - See Messages Menu",
            "ac_clear" => $"{SoftName} by {SoftAuthor} > Auto Chat - Clear Messages Menu",
            "lols" => $"{SoftName} by {SoftAuthor} - League Of Legends Settings Menu",
            "lols_import" => $"{SoftName} by {SoftAuthor} > League Of Legends Settings - Import Settings Menu",
            "lols_export" => $"{SoftName} by {SoftAuthor} > League Of Legends Settings - Export Settings Menu",
            _ => Console.Title
        };
    }
}