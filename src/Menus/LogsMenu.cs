using Loly.src.Menus.Core;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using static Loly.src.Menus.MainMenu;
using static Loly.src.Tools.Utils;
using Console = Colorful.Console;

namespace Loly.src.Menus;

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

        Console.Write(Environment.NewLine);
        Console.Write(Environment.NewLine);

        _ = Console.ReadKey();

        Global.LogsMenuEnable = false;
        ResetConsole();
        StartMenu();
    }
}