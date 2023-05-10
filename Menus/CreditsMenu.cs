using Alba.CsConsoleFormat;
using Loly.Menus.Core;
using Loly.Variables;
using static Loly.Tools.Utils;

namespace Loly.Menus;

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

            Console.Clear();
            Interface.ShowArt();

            if (choice == 2)
            {
                MainMenu.StartMenu();
            }
            else
            {
                ShowCreditsMenu();
                OpenUrl(Global.DiscordInvite);
                continue;
            }

            break;
        }
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