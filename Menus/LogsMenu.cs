using Loly.Menus.Core;
using Loly.Variables;
using static Loly.Tools.Utils;
using static Loly.Menus.MainMenu;
using Console = Colorful.Console;

namespace Loly.Menus;

public class LogsMenu
{
    public static void GetLogsMenu()
    {
        UpdateMenuTitle("logs");
        Console.Clear();
        Interface.ShowLogsArt();

        Console.Write(DateTime.Now.ToString("[hh:mm:ss]"), Colors.PrimaryColor);
        Console.Write("» Start Logs System\n", Colors.PrimaryColor);
        Console.Write(DateTime.Now.ToString("[hh:mm:ss]"), Colors.PrimaryColor);
        Console.Write("» All actions will be displayed in real time\n", Colors.PrimaryColor);
        Console.Write(DateTime.Now.ToString("[hh:mm:ss]"), Colors.PrimaryColor);
        Console.Write("» Press any key to leave logs system...", Colors.PrimaryColor);

        Global.LogsMenuEnable = true;

        Console.WriteLine("");
        Console.WriteLine("");

        Console.ReadKey();

        Global.LogsMenuEnable = false;
        ResetConsole();
        StartMenu();
    }
}