using Loly.src.Menus.Core;

using static Loly.src.Menus.Core.Interface;
using static Loly.src.Menus.CreditsMenu;
using static Loly.src.Menus.LogsMenu;
using static Loly.src.Menus.SettingsMenu;
using static Loly.src.Menus.ToolsMenu;
using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Menus;

public class MainMenu
{
    public static void StartMenu()
    {
        while (true)
        {
            var choice = 7;
            UpdateMenuTitle("main");
            string[] choices = { "Use Tools", "Show Logs", "Settings", "Credits", "Exit" };

            var startMenu = MenuBuilder.BuildMenu(choices, TopLength);
            while (choice == 7)
            {
                choice = startMenu.RunMenu();
            }

            ResetConsole();

            switch (choice)
            {
                case 1:
                    GetToolsMenu();
                    break;
                case 2:
                    GetLogsMenu();
                    break;
                case 3:
                    GetSettingsMenu();
                    break;
                case 4:
                    GetCreditsMenu();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
            }

            break;
        }
    }
}