using Alba.CsConsoleFormat;

using Loly.src.Menus.Core;
using Loly.src.Tools;
using Loly.src.Variables.Class;

using static Loly.src.Menus.Core.Interface;
using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Menus;

public class SettingsMenu
{
    #region Get Menus

    public static void GetSettingsMenu()
    {
        while (true)
        {
            UpdateMenuTitle("settings");
            ShowSettingsMenu();

            List<string> choices = new()
            {
                $"Auto Update     - {CheckBoolean(CurrentSettings.EnableAutoUpdate)}",
                $"Lobby Revealer  - {CheckBoolean(CurrentSettings.Tools.LobbyRevealer)}",
                $"Auto Accept     - {CheckBoolean(CurrentSettings.Tools.AutoAccept)}",
                $"Auto Chat       - {CheckBoolean(CurrentSettings.Tools.AutoChat)}",
                $"Pick and Ban    - {CheckBoolean(CurrentSettings.Tools.PicknBan)}",
                "Back"
            };

            var settingsMenu = MenuBuilder.BuildMenu(choices.ToArray(), Console.CursorTop + 1);
            var choice = 0;
            while (choice == 0) choice = settingsMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Count) break;

            switch (choice)
            {
                case 1: CurrentSettings.EnableAutoUpdate = !CurrentSettings.EnableAutoUpdate; break;
                case 2: CurrentSettings.Tools.LobbyRevealer = !CurrentSettings.Tools.LobbyRevealer; break;
                case 3: CurrentSettings.Tools.AutoAccept = !CurrentSettings.Tools.AutoAccept; break;
                case 4: CurrentSettings.Tools.AutoChat = !CurrentSettings.Tools.AutoChat; break;
                case 5: CurrentSettings.Tools.PicknBan = !CurrentSettings.Tools.PicknBan; break;
            }

            SettingsManager.SaveFileSettings();
            SettingsManager.CreateOrUpdateSettings();
        }
    }

    #endregion

    #region Show Menus

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

    #endregion
}