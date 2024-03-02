using Loly.src.Variables;
using Loly.src.Variables.Class;
using static Loly.src.Menus.Core.Interface;
using static Loly.src.Menus.MainMenu;
using static Loly.src.Tools.Utils;

namespace Loly.src.Menus;

public class LogsMenu
{
    public static void GetLogsMenu()
    {
        UpdateMenuTitle("logs");
        Console.Clear();
        ShowLogsArt();

        DisplayColor($"{DateTime.Now:[hh:mm:ss]}» Start Logs System", Colors.PrimaryColor, Colors.InfoColor);
        DisplayColor($"{DateTime.Now:[hh:mm:ss]}» All actions will be displayed in real time", Colors.PrimaryColor, Colors.InfoColor);
        DisplayColor($"{DateTime.Now:[hh:mm:ss]}» Press any key to leave logs system...", Colors.PrimaryColor, Colors.InfoColor);

        Global.LogsMenuEnable = true;

        Console.Write(Environment.NewLine);
        Console.ReadKey();

        Global.LogsMenuEnable = false;
        ResetConsole();
        StartMenu();
    }
}