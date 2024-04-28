using Alba.CsConsoleFormat;

using Loly.src.Menus.Core;
using Loly.src.Variables.Class;

using static Loly.src.Menus.AutoAcceptMenu;
using static Loly.src.Menus.AutoChatMenu;
using static Loly.src.Menus.Core.Interface;
using static Loly.src.Menus.LobbyRevealerMenu;
using static Loly.src.Menus.LolSettingsMenu;
using static Loly.src.Menus.PicknBanMenu;
using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Menus;

internal static class ToolsMenu
{
    #region Get Menus

    internal static void GetToolsMenu()
    {
        while (true)
        {
            ShowToolsMenu();
            UpdateMenuTitle("tools");

            string[] choices = { "Lobby Revealer", "Auto Accept", "Auto Chat", "Pick and Ban", "LoL Settings", "Back" };
            var toolsMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop + 1);
            var choice = 0;
            while (choice == 0) choice = toolsMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Length) break;

            switch (choice)
            {
                case 1:
                    if (!CurrentSettings.Tools.LobbyRevealer)
                    {
                        DisplayColor(" < Lobby Revealer > is not enabled in the settings.", Colors.WarningColor, Colors.PrimaryColor);
                        DisplayColor(" Please go to 'Settings' menu and enable it.", Colors.WarningColor, Colors.PrimaryColor);
                        DisplayColor(" Press Enter to continue...", Colors.WarningColor, Colors.PrimaryColor);
                        Console.ReadKey();
                        break;
                    }

                    GetLobbyRevealerMenu();
                    break;
                case 2:
                    if (!CurrentSettings.Tools.AutoAccept)
                    {
                        DisplayColor(" < Auto Accept > is not enabled in the settings.", Colors.WarningColor, Colors.PrimaryColor);
                        DisplayColor(" Please go to 'Settings' menu and enable it.", Colors.WarningColor, Colors.PrimaryColor);
                        DisplayColor(" Press Enter to continue...", Colors.WarningColor, Colors.PrimaryColor);
                        Console.ReadKey();
                        break;
                    }

                    GetAutoAcceptMenu();
                    break;
                case 3:
                    if (!CurrentSettings.Tools.AutoChat)
                    {
                        DisplayColor(" < Auto Chat > is not enabled in the settings.", Colors.WarningColor, Colors.PrimaryColor);
                        DisplayColor(" Please go to 'Settings' menu and enable it.", Colors.WarningColor, Colors.PrimaryColor);
                        DisplayColor(" Press Enter to continue...", Colors.WarningColor, Colors.PrimaryColor);
                        Console.ReadKey();
                        break;
                    }

                    GetAutoChatMenu();
                    break;
                case 4:
                    if (!CurrentSettings.Tools.PicknBan)
                    {
                        DisplayColor(" < Pick and Ban > is not enabled in the settings.", Colors.WarningColor, Colors.PrimaryColor);
                        DisplayColor(" Please go to 'Settings' menu and enable it.", Colors.WarningColor, Colors.PrimaryColor);
                        DisplayColor(" Press Enter to continue...", Colors.WarningColor, Colors.PrimaryColor);
                        Console.ReadKey();
                        break;
                    }

                    GetPicknBanMenu();
                    break;
                case 5:
                    GetLoLSettingsMenu();
                    break;
            }
            ResetConsole();
        }
    }

    #endregion

    #region Show Menus

    private static void ShowToolsMenu()
    {
        Console.SetCursorPosition(0, TopLength);

        Document rectangle = new();
        Border border1 = new()
        {
            MinWidth = 60,
            MaxWidth = 60,
            Stroke = LineThickness.None,
            Align = Align.Center,
            TextAlign = TextAlign.Center,
            Color = Colors.MenuPrimaryColor,
            TextWrap = TextWrap.WordWrap,
            Children =
            {
                CreateSpan("Tools", 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan("Select the tool you want to interact with", 0, Colors.MenuTextColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    #endregion
}