using Loly.Menus.Core;
using static Loly.Tools.Utils;
using static Loly.Menus.CreditsMenu;
using static Loly.Menus.ToolsMenu;
using static Loly.Menus.LogsMenu;
using static Loly.Menus.SettingsMenu;
using Console = Colorful.Console;

namespace Loly.Menus;

public class MainMenu
{
    public static void StartMenu()
    {
        while (true)
        {
            int choice = 7;
            UpdateMenuTitle("main");
            string[] choices = { "Use Tools", "Settings", "Show Logs", "Credits", "Exit" };

            MenuBuilder mainMenu = MenuBuilder.BuildMenu(choices, TopLength);
            while (choice == 7) choice = mainMenu.RunMenu();

            Console.Clear();
            Interface.ShowArt();

            switch (choice)
            {
                case 1:
                    GetToolsMenu();
                    break;
                case 2:
                    GetSettingsMenu();
                    break;
                case 3:
                    GetLogsMenu();
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