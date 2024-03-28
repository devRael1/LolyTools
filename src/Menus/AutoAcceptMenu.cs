using Alba.CsConsoleFormat;

using Loly.src.Menus.Core;
using Loly.src.Tools;
using Loly.src.Variables;
using Loly.src.Variables.Class;

using static Loly.src.Menus.Core.Interface;
using static Loly.src.Menus.ToolsMenu;
using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Menus;

public class AutoAcceptMenu
{
    public static void GetAutoAcceptMenu()
    {
        while (true)
        {
            ShowAutoAcceptMenu();

            var choice = 7;
            UpdateMenuTitle("aa");
            string[] choices = { $"Auto Accept Once    - {CheckBoolean(Global.CurrentSettings.AutoAccept.AutoAcceptOnce)}", "Back" };

            var autoAcceptMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop + 1);
            while (choice == 7) choice = autoAcceptMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Length) break;

            Global.CurrentSettings.AutoAccept.AutoAcceptOnce = !bool.Parse(Global.CurrentSettings.AutoAccept.AutoAcceptOnce.ToString());
            SettingsManager.SaveFileSettings();
            SettingsManager.CreateOrUpdateSettings();
        }

        GetToolsMenu();
    }

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
}