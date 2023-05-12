using Alba.CsConsoleFormat;
using Loly.Menus.Core;
using Loly.Variables;
using Newtonsoft.Json.Linq;
using static Loly.Tools.Utils;
using static Loly.Menus.MainMenu;
using Console = Colorful.Console;

namespace Loly.Menus;

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

            MenuBuilder creditsMenu = MenuBuilder.BuildMenu(choices.ToArray(), Console.CursorTop);
            while (choice == 7) choice = creditsMenu.RunMenu();

            Console.Clear();
            Interface.ShowArt();

            if (choice == choices.Count) break;

            JObject settings = Settings.GetSettings();

            if (variables[choice - 1] == "EnableAutoUpdate")
                settings[variables[choice - 1]] = !bool.Parse(settings[variables[choice - 1]]?.ToString() ?? string.Empty);
            else
                settings["Tools"][variables[choice - 1]] = !bool.Parse(settings["Tools"][variables[choice - 1]]?.ToString() ?? string.Empty);

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