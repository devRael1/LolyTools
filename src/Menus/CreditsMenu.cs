using Alba.CsConsoleFormat;

using Loly.src.Menus.Core;
using Loly.src.Variables;
using Loly.src.Variables.Class;

using static Loly.src.Menus.Core.Interface;
using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Menus;

internal static class CreditsMenu
{
    #region Get Menus

    internal static void GetCreditsMenu()
    {
        while (true)
        {
            UpdateMenuTitle("credits");
            ShowCreditsMenu();

            string[] choices = { "Github Repository (Source Code)", "Back" };
            var creditsMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop + 1);
            var choice = 0;
            while (choice == 0) choice = creditsMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Length) break;

            ShowCreditsMenu();
            OpenUrl(GithubPage);
        }
    }

    #endregion

    #region Show Menus

    private static void ShowCreditsMenu()
    {
        Console.SetCursorPosition(0, TopLength);

        Document rectangle = new();
        Border border1 = new()
        {
            MinWidth = 60,
            MaxWidth = 60,
            Stroke = LineThickness.Single,
            Align = Align.Center,
            TextAlign = TextAlign.Justify,
            Color = Colors.MenuPrimaryColor,
            TextWrap = TextWrap.WordWrap,
            Children =
            {
                CreateSpan("Credits", 25, Colors.MenuTextColor),
                new Separator(),
                CreateSpan("Tool ", 1, Colors.MenuTextColor),
                CreateSpan("Coded", 0, Colors.MenuPrimaryColor),
                CreateSpan(" & ", 0, Colors.MenuTextColor),
                CreateSpan("Designed", 0, Colors.MenuPrimaryColor),
                CreateSpan(" by ", 0, Colors.MenuTextColor),
                CreateSpan($"{Global.SoftAuthor}\n", 0, Colors.MenuPrimaryColor),
                CreateSpan("Tool ", 1, Colors.MenuTextColor),
                CreateSpan("Optimized", 0, Colors.MenuPrimaryColor),
                CreateSpan(" by ", 0, Colors.MenuTextColor),
                CreateSpan($"{Global.SoftAuthor}", 0, Colors.MenuPrimaryColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    #endregion
}