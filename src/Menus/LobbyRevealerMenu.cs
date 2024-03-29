using Alba.CsConsoleFormat;

using Loly.src.Menus.Core;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;

using static Loly.src.Menus.Core.Interface;
using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Menus;

public class LobbyRevealerMenu
{
    #region Get Menus

    public static void GetLobbyRevealerMenu()
    {
        while (true)
        {
            UpdateMenuTitle("lv");

            string[] choices = { "Get U.GG", "Get Stats", "Back" };
            var lobbyRevealerMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop + 1);
            var choice = 0;
            while (choice == 0) choice = lobbyRevealerMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Length) break;
            if (PlayerList.Count == 0)
            {
                DisplayColor("`[WARNING]»` No Players in the list.", Colors.WarningColor, Colors.PrimaryColor);
                DisplayColor("`[WARNING]»` Loly Tools did not detect a champ select in progress.", Colors.WarningColor, Colors.PrimaryColor);
                DisplayColor("`[WARNING]»` Press Enter to return to 'Lobby Revealer' menu", Colors.WarningColor, Colors.PrimaryColor);
                Console.ReadKey();
                ResetConsole();
                break;
            }

            if (choice == 1) GetUggMenu();
            else GetStatsMenu();
        }
    }

    private static void GetUggMenu()
    {
        while (true)
        {
            UpdateMenuTitle("lv_get_ugg");
            ShowUggMenu();

            var choices = PlayerList.Select(t => $"[U.GG] - {t.RiotUserTag}").ToList();
            if (PlayerList.Count >= 1) choices.Add("[GLOBAL] - All U.GG");
            choices.Add("Back");

            var uggMenu = MenuBuilder.BuildMenu(choices.ToArray(), Console.CursorTop + 1);
            var choice = 0;
            while (choice == 0) choice = uggMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Count) break;
            if (choice == choices.Count - 1)
            {
                var url = $"https://u.gg/multisearch?summoners=" +
                    $"{string.Join(",", PlayerList.Select(p => p.RiotUserTagEncoded))}&region={RegionId(Enum.Parse<Region>(Global.Region.ToUpper()))}";
                OpenUrl(url);
            }
            else OpenUrl(PlayerList[choice - 1].Link);
        }
    }

    private static void GetStatsMenu()
    {
        while (true)
        {
            UpdateMenuTitle("lv_get_stats");
            ShowGlobalStatsMenu();

            var choices = PlayerList.Select(t => $"[STATS] - {t.RiotUserTag}").ToList();
            if (PlayerList.Count > 0) choices.Add("[GLOBAL] - All Stats");
            choices.Add("Back");

            var statsMenu = MenuBuilder.BuildMenu(choices.ToArray(), Console.CursorTop + 1);
            var choice = 0;
            while (choice == 0) choice = statsMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Count) break;

            if (choice == choices.Count - 1) ShowGlobalStatsMenu();
            else ShowPlayerStats(PlayerList.ElementAt(choice - 1));
        }
    }

    #endregion

    #region Show Menus

    private static void ShowUggMenu()
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
                CreateSpan("U.GG", 36, Colors.MenuTextColor),
                new Separator()
            }
        };

        for (var i = 0; i < 5; i++)
        {
            border1.Children.Add(CreateSpan("Player " + (i + 1) + "     -  ", 1, Colors.MenuTextColor));
            var usertag = PlayerList.ElementAtOrDefault(i)?.RiotUserTag ?? "null\n";
            border1.Children.Add(CreateSpan(usertag, 0, Colors.MenuPrimaryColor));
            if (usertag == "null\n") continue;

            border1.Children.Add(CreateSpan($" ({PlayerList.ElementAtOrDefault(i)?.SoloDuoQ.Tier}", 0, Colors.MenuPrimaryColor));
            var f = PlayerList.ElementAtOrDefault(i)?.SoloDuoQ.Rank == "" ? ")\n" : $" {PlayerList.ElementAtOrDefault(i)?.SoloDuoQ.Rank})\n";
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

        foreach (Player player in PlayerList)
        {
            Cell levelCell = new(player.Level) { Color = Colors.MenuTextColor };
            Cell nameCell = new(player.RiotUserName) { Color = Colors.MenuTextColor };
            var rank = $"{player.SoloDuoQ.Tier} {player.SoloDuoQ.Rank} ({player.SoloDuoQ.Lp} LP)";

            var winrate = player.SoloDuoQ.Wins + player.SoloDuoQ.Losses > 0
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

        var winratesoloq = player.SoloDuoQ.Wins + player.SoloDuoQ.Losses > 0 ?
            (int)Math.Round((double)(player.SoloDuoQ.Wins * 100.0 / (player.SoloDuoQ.Wins + player.SoloDuoQ.Losses))) : 0;
        var winrateflex = player.FlexQ.Wins + player.FlexQ.Losses > 0 ?
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
                CreateSpan($"{player.RiotUserName} Stats (Ranked)", 0, Colors.MenuTextColor),
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
                        CreateSpan($"{FormatStr(player.SoloDuoQ.Tier)} {player.SoloDuoQ.Rank}", 0, Colors.MenuPrimaryColor),
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
                        CreateSpan($"{FormatStr(player.FlexQ.Tier)} {player.FlexQ.Rank}", 0, Colors.MenuPrimaryColor),
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

    #endregion
}