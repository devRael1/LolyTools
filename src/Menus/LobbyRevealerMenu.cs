using Alba.CsConsoleFormat;
using Loly.src.Menus.Core;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using static Loly.src.Menus.Core.Interface;
using static Loly.src.Menus.ToolsMenu;
using static Loly.src.Tools.Utils;

namespace Loly.src.Menus;

public class LobbyRevealerMenu
{
    public static void GetLobbyRevealerMenu()
    {
        while (true)
        {
            int choice = 7;
            UpdateMenuTitle("lv");
            string[] choices = { "Get OP.GG", "Get Stats", "Back" };

            MenuBuilder lobbyRevealerMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop + 1);
            while (choice == 7)
            {
                choice = lobbyRevealerMenu.RunMenu();
            }

            ResetConsole();

            if (choice == choices.Length)
            {
                break;
            }

            switch (choice)
            {
                case 1:
                    GetOpggMenu();
                    break;
                case 2:
                    if (Global.PlayerList.Count == 0)
                    {
                        DisplayColor("No Players in the list.", Colors.WarningColor, Colors.PrimaryColor);
                        DisplayColor("Loly Tools did not detect a champ select in progress.", Colors.WarningColor, Colors.PrimaryColor);
                        DisplayColor("Press Enter to continue...", Colors.WarningColor, Colors.PrimaryColor);
                        Console.ReadKey();
                        ResetConsole();
                        continue;
                    }

                    GetStatsMenu();
                    break;
            }
        }

        GetToolsMenu();
    }

    private static void GetOpggMenu()
    {
        int choice = 10;
        UpdateMenuTitle("lv_get_opgg");
        List<string> choices = Global.PlayerList.Select(t => $"[OP.GG] - {t.UserTag}").ToList();

        if (Global.PlayerList.Count >= 1)
        {
            choices.Add("Get All OP.GG");
        }

        choices.Add("Back");

        while (choice != choices.Count)
        {
            ShowOpggMenu();

            MenuBuilder opGgMenu = MenuBuilder.BuildMenu(choices.ToArray(), Console.CursorTop + 1);
            choice = 10;
            while (choice == 10)
            {
                choice = opGgMenu.RunMenu();
            }

            ResetConsole();

            if (choice == choices.Count)
            {
                break;
            }

            if (choice == choices.Count - 1)
            {
                string url = $"https://www.op.gg/multisearch/{Global.Region}?summoners=";
                for (int i = 0; i < Global.PlayerList.Count; i++)
                {
                    url += $"{Global.PlayerList[i].Username}";
                    if (i != Global.PlayerList.Count - 1)
                    {
                        url += ",";
                    }
                }

                OpenUrl(url);
            }
            else
            {
                OpenUrl(Global.PlayerList[choice - 1].Link);
            }
        }
    }

    private static void GetStatsMenu()
    {
        int choice = 10;
        UpdateMenuTitle("lv_get_stats");
        List<string> choices = Global.PlayerList.Select(t => $"{t.Username}'s Stats").ToList();

        if (Global.PlayerList.Count > 0)
        {
            choices.Add("[GLOBAL] Stats");
        }

        choices.Add("Back");

        ShowGlobalStatsMenu();

        while (choice != choices.Count)
        {
            MenuBuilder statsMenu = MenuBuilder.BuildMenu(choices.ToArray(), Console.CursorTop + 1);
            choice = 10;
            while (choice == 10)
            {
                choice = statsMenu.RunMenu();
            }

            ResetConsole();

            if (choice == choices.Count)
            {
                break;
            }

            if (choice == choices.Count - 1)
            {
                ShowGlobalStatsMenu();
            }
            else
            {
                ShowPlayerStats(Global.PlayerList[choice - 1]);
            }
        }
    }

    private static void ShowOpggMenu()
    {
        Console.SetCursorPosition(0, TopLength);

        Document rectangle = new();
        Border border1 = new()
        {
            MinWidth = 80,
            Stroke = LineThickness.Single,
            Align = Align.Center,
            TextAlign = TextAlign.Justify,
            Color = Colors.MenuPrimaryColor,
            TextWrap = TextWrap.WordWrap,
            Children =
            {
                CreateSpan("OP.GG", 36, Colors.MenuTextColor),
                new Separator()
            }
        };

        for (int i = 0; i < 5; i++)
        {
            border1.Children.Add(CreateSpan("Player " + (i + 1) + "     -  ", 1, Colors.MenuTextColor));
            string usertag = Global.PlayerList.ElementAtOrDefault(i)?.UserTag ?? "null\n";
            border1.Children.Add(CreateSpan($"{usertag}", 0, Colors.MenuPrimaryColor));
            if (usertag == "null\n")
            {
                continue;
            }

            border1.Children.Add(CreateSpan($" ({Global.PlayerList.ElementAtOrDefault(i)?.SoloDuoQ.Tier}", 0, Colors.MenuPrimaryColor));
            string f = Global.PlayerList.ElementAtOrDefault(i)?.SoloDuoQ.Division == 0 ? ")\n" : $" {Global.PlayerList.ElementAtOrDefault(i)?.SoloDuoQ.Division})\n";
            border1.Children.Add(CreateSpan(f, 0, Colors.MenuPrimaryColor));
        }

        border1.Children.Add(CreateSpan("\n Note: To get stats of players, go to the 'Get Stats' menu.", 1, Colors.MenuTextColor));

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void ShowGlobalStatsMenu()
    {
        Console.SetCursorPosition(0, TopLength);

        Document rectangle = new();
        Grid grid = new()
        {
            Stroke = LineThickness.None,
            Align = Align.Center,
            Columns = { GridLength.Char(7), GridLength.Char(40), GridLength.Char(28) },
            Color = Colors.MenuPrimaryColor,
            MaxWidth = 110,
            Children =
            {
                new Cell("Level") { Stroke = LineThickness.Single },
                new Cell("Name") { Stroke = LineThickness.Single },
                new Cell("Rank") { Stroke = LineThickness.Single }
            }
        };

        foreach (Player player in Global.PlayerList)
        {
            Cell levelCell = new(player.Level) { Color = Colors.MenuTextColor };
            Cell nameCell = new(player.Username) { Color = Colors.MenuTextColor };
            string rank = $"{player.SoloDuoQ.Tier} {player.SoloDuoQ.Division} ({player.SoloDuoQ.Lp} LP)";

            int winrate = player.SoloDuoQ.Wins + player.SoloDuoQ.Losses > 0 
                ? (int)Math.Round((double)(player.SoloDuoQ.Wins * 100.0 / (player.SoloDuoQ.Wins + player.SoloDuoQ.Losses)))
                : 0;
            rank += $" | {winrate}%";

            Cell rankCell = new(rank) { Color = Colors.MenuTextColor };

            grid.Children.Add(levelCell);
            grid.Children.Add(nameCell);
            grid.Children.Add(rankCell);
        }

        rectangle.Children.Add(grid);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void ShowPlayerStats(Player player)
    {
        Console.SetCursorPosition(0, TopLength);

        int winratesoloq = player.SoloDuoQ.Wins + player.SoloDuoQ.Losses > 0 ?
            (int)Math.Round((double)(player.SoloDuoQ.Wins * 100.0 / (player.SoloDuoQ.Wins + player.SoloDuoQ.Losses))) : 0;
        int winrateflex = player.FlexQ.Wins + player.FlexQ.Losses > 0 ?
            (int)Math.Round((double)(player.FlexQ.Wins * 100.0 / (player.FlexQ.Wins + player.FlexQ.Losses))) : 0;

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
                CreateSpan($"{player.Username} Stats (Ranked)", 0, Colors.MenuTextColor),
                new Separator()
            }
        };
        Grid grid1 = new()
        {
            Stroke = LineThickness.None,
            Align = Align.Center,
            Columns = { GridLength.Char(30), GridLength.Char(30) },
            Color = Colors.MenuPrimaryColor,
            MaxWidth = 78,
            Children =
            {
                new Cell("< SOLOQ / DUOQ >") { Stroke = LineThickness.None, TextAlign = TextAlign.Center },
                new Cell("< FLEXQ >") { Stroke = LineThickness.None, TextAlign = TextAlign.Center },
                new Cell
                {
                    Color = Colors.MenuTextColor,
                    Stroke = new LineThickness(LineWidth.None, LineWidth.None, LineWidth.Single, LineWidth.None),
                    Children =
                    {
                        CreateSpan("\n", 0, Colors.MenuTextColor),
                        CreateSpan("───────── Ranked ─────────", 1, Colors.MenuTextColor),
                        CreateSpan("\nRank       - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{FormatStr(player.SoloDuoQ.Tier)} {player.SoloDuoQ.Division}", 0, Colors.MenuPrimaryColor),
                        CreateSpan("\nLP         - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{player.SoloDuoQ.Lp}", 0, Colors.MenuPrimaryColor),
                        CreateSpan("\nWinrate    - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{winratesoloq}%\n\n", 0, Colors.MenuPrimaryColor),
                        CreateSpan("────── Games Stats ──────", 1, Colors.MenuTextColor),
                        CreateSpan("\nWins       - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{player.SoloDuoQ.Wins}", 0, Colors.MenuPrimaryColor),
                        CreateSpan("\nLosses     - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{player.SoloDuoQ.Losses}", 0, Colors.MenuPrimaryColor),
                        CreateSpan("\nTotal      - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{player.SoloDuoQ.Wins + player.SoloDuoQ.Losses}", 0, Colors.MenuPrimaryColor)
                    }
                },
                new Cell
                {
                    Color = Colors.MenuTextColor,
                    Stroke = LineThickness.None,
                    Children =
                    {
                        CreateSpan("\n", 0, Colors.MenuTextColor),
                        CreateSpan("  ───────── Ranked ─────────", 1, Colors.MenuTextColor),
                        CreateSpan("\n  Rank       - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{FormatStr(player.FlexQ.Tier)} {player.FlexQ.Division}", 0, Colors.MenuPrimaryColor),
                        CreateSpan("\n  LP         - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{player.FlexQ.Lp}", 0, Colors.MenuPrimaryColor),
                        CreateSpan("\n  Winrate    - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{winrateflex}%\n\n", 0, Colors.MenuPrimaryColor),
                        CreateSpan("  ────── Games Stats ──────", 1, Colors.MenuTextColor),
                        CreateSpan("\n  Wins       - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{player.FlexQ.Wins}", 0, Colors.MenuPrimaryColor),
                        CreateSpan("\n  Losses     - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{player.FlexQ.Losses}", 0, Colors.MenuPrimaryColor),
                        CreateSpan("\n  Total      - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{player.FlexQ.Wins + player.FlexQ.Losses}", 0, Colors.MenuPrimaryColor)
                    }
                }
            }
        };

        border1.Children.Add(grid1);
        border1.Children.Add(new Separator());

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }
}