using Alba.CsConsoleFormat;
using Loly.src.Menus.Core;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using static Loly.src.Menus.AutoAcceptMenu;
using static Loly.src.Menus.AutoChatMenu;
using static Loly.src.Menus.Core.Interface;
using static Loly.src.Menus.LanguageChangerMenu;
using static Loly.src.Menus.LobbyRevealerMenu;
using static Loly.src.Menus.MainMenu;
using static Loly.src.Menus.PicknBanMenu;
using static Loly.src.Tools.Utils;

namespace Loly.src.Menus;

public class ToolsMenu
{
    public static void GetToolsMenu()
    {
        while (true)
        {
            ShowToolsMenu();

            int choice = 7;
            UpdateMenuTitle("tools");
            string[] choices = { "Lobby Revealer", "Auto Accept", "Auto Chat", "Pick and Ban", "Language Changer", "Back" };

            MenuBuilder toolsMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop + 1);
            while (choice == 7)
            {
                choice = toolsMenu.RunMenu();
            }

            ResetConsole();

            if (choice == choices.Length)
            {
                break;
            }

            switch (choice)
            {
                case 1:
                    if (!Settings.LobbyRevealer)
                    {
                        Console.WriteLine(" < Lobby Revealer > is not enabled in the settings.", Colors.WarningColor);
                        Console.WriteLine(" Please go to 'Settings' menu and enable it.", Colors.WarningColor);
                        Console.WriteLine(" Press Enter to continue...", Colors.WarningColor);
                        Console.ReadKey();
                    }
                    else
                    {
                        GetLobbyRevealerMenu();
                    }

                    break;
                case 2:
                    if (!Settings.AutoAccept)
                    {
                        Console.WriteLine(" < Auto Accept > is not enabled in the settings.", Colors.WarningColor);
                        Console.WriteLine(" Please go to 'Settings' menu and enable it.", Colors.WarningColor);
                        Console.WriteLine(" Press Enter to continue...", Colors.WarningColor);
                        Console.ReadKey();
                    }
                    else
                    {
                        GetAutoAcceptMenu();
                    }

                    break;
                case 3:
                    if (!Settings.AutoChat)
                    {
                        Console.WriteLine(" < Auto Chat > is not enabled in the settings.", Colors.WarningColor);
                        Console.WriteLine(" Please go to 'Settings' menu and enable it.", Colors.WarningColor);
                        Console.WriteLine(" Press Enter to continue...", Colors.WarningColor);
                        Console.ReadKey();
                    }
                    else
                    {
                        GetAutoChatMenu();
                    }

                    break;
                case 4:
                    if (!Settings.PicknBan)
                    {
                        Console.WriteLine(" < Pick and Ban > is not enabled in the settings.", Colors.WarningColor);
                        Console.WriteLine(" Please go to 'Settings' menu and enable it.", Colors.WarningColor);
                        Console.WriteLine(" Press Enter to continue...", Colors.WarningColor);
                        Console.ReadKey();
                    }
                    else
                    {
                        GetPicknBanMenu();
                    }

                    break;
                case 5:
                    GetLanguageChangerMenu();
                    break;
            }
        }

        StartMenu();
    }

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
}