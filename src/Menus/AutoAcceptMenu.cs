using Alba.CsConsoleFormat;

using Loly.src.Menus.Core;
using Loly.src.Tools;
using Loly.src.Variables.Class;

using static Loly.src.Menus.Core.Interface;
using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Menus;

internal static class AutoAcceptMenu
{
    #region Get Menus

    internal static void GetAutoAcceptMenu()
    {
        while (true)
        {
            ShowAutoAcceptMenu();
            UpdateMenuTitle("aa");

            string[] choices = { $"Auto Accept Once    - {CheckBoolean(CurrentSettings.AutoAccept.AutoAcceptOnce)}", "Back" };
            var autoAcceptMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop + 1);
            var choice = 0;
            while (choice == 0) choice = autoAcceptMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Length) break;

            CurrentSettings.AutoAccept.AutoAcceptOnce = !CurrentSettings.AutoAccept.AutoAcceptOnce;
            SettingsManager.SaveFileSettings();
            SettingsManager.CreateOrUpdateSettings();
        }
    }

    #endregion

    #region Show Menus

    private static void ShowAutoAcceptMenu()
    {
        Console.SetCursorPosition(0, TopLength);

        Document rectangle = new();
        Border border1 = new()
        {
            MinWidth = 80,
            MaxWidth = 80,
            Stroke = LineThickness.None,
            Align = Align.Center,
            TextAlign = TextAlign.Center,
            Color = Colors.MenuPrimaryColor,
            TextWrap = TextWrap.WordWrap,
            Children =
            {
                CreateSpan("Auto Accept", 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan("Press 'Enter' to enable or disable the 'Auto Accept Once'\n", 0, Colors.MenuTextColor),
                CreateSpan("This allows you to activate the 'Auto Accept' tool only once.\n", 0, Colors.MenuTextColor),
                CreateSpan("After the first auto accept, the tool will be disabled.", 0, Colors.MenuTextColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    #endregion
}