using Alba.CsConsoleFormat;
using Loly.src.Menus.Core;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using Newtonsoft.Json.Linq;
using static Loly.src.Menus.Core.Interface;
using static Loly.src.Menus.MainMenu;
using static Loly.src.Tools.Utils;

namespace Loly.src.Menus;

public class SettingsMenu
{
    public static void GetSettingsMenu()
    {
        while (true)
        {
            ShowSettingsMenu();

            int choice = 7;
            UpdateMenuTitle("settings");
            List<string> choices = new()
            {
                $"Auto Update     - {CheckBoolean(Settings.EnableAutoUpdate)}",
                $"Lobby Revealer  - {CheckBoolean(Settings.LobbyRevealer)}",
                $"Auto Accept     - {CheckBoolean(Settings.AutoAccept)}",
                $"Auto Chat       - {CheckBoolean(Settings.AutoChat)}",
                $"Pick and Ban    - {CheckBoolean(Settings.PicknBan)}",
                "Back"
            };
            string[] variables = { "EnableAutoUpdate", "LobbyRevealer", "AutoAccept", "AutoChat", "PicknBan" };

            MenuBuilder settingsMenu = MenuBuilder.BuildMenu(choices.ToArray(), Console.CursorTop + 1);
            while (choice == 7)
            {
                choice = settingsMenu.RunMenu();
            }

            ResetConsole();

            if (choice == choices.Count)
            {
                break;
            }

            JObject settings = Settings.GetSettings();

            if (variables[choice - 1] == "EnableAutoUpdate")
            {
                settings[variables[choice - 1]] = !bool.Parse(settings[variables[choice - 1]]?.ToString() ?? string.Empty);
            }
            else
            {
                settings["Tools"][variables[choice - 1]] = !bool.Parse(settings["Tools"][variables[choice - 1]]?.ToString() ?? string.Empty);
            }

            Settings.SaveFileSettings(settings);
            Settings.CreateOrUpdateSettings();
        }

        StartMenu();
    }

    private static void ShowSettingsMenu()
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
                CreateSpan("Settings", 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan("Press 'Enter' to enable or disable a tool", 0, Colors.MenuTextColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }
}