using Alba.CsConsoleFormat;
using Loly.src.Menus.Core;
using Loly.src.Variables;
using static Loly.src.Tools.Utils;

namespace Loly.src.Menus;

public class CreditsMenu
{
    public static void GetCreditsMenu()
    {
        while (true)
        {
            ShowCreditsMenu();

            int choice = 7;
            UpdateMenuTitle("credits");
            string[] choices = { "Discord Creator", "Back" };

            MenuBuilder creditsMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop);
            while (choice == 7) choice = creditsMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Length) break;

            ShowCreditsMenu();
            OpenUrl(Global.DiscordInvite);
        }

        MainMenu.StartMenu();
    }

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
                CreateSpan($"{Global.SoftAuthor}\n\n", 0, Colors.MenuPrimaryColor),
                CreateSpan("Discord     - ", 1, Colors.MenuTextColor),
                CreateSpan($"{Global.SoftAuthorDiscord}", 0, Colors.MenuPrimaryColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }
}