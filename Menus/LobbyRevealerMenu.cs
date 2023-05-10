﻿using System.Collections.Concurrent;
using Alba.CsConsoleFormat;
using Loly.Menus.Core;
using Loly.Tools;
using Loly.Variables;
using static Loly.Tools.Utils;
using static Loly.Menus.ToolsMenu;
using Console = Colorful.Console;

namespace Loly.Menus;

public class LobbyRevealerMenu
{
    public static void GetLobbyRevealerMenu()
    {
        while (true)
        {
            int choice = 7;
            UpdateMenuTitle("lv");
            string[] choices = { "Get Names", "Get OP.GG", "Get Stats", "Back" };

            MenuBuilder mainMenu = MenuBuilder.BuildMenu(choices, TopLength);
            while (choice == 7) choice = mainMenu.RunMenu();

            Console.Clear();
            Interface.ShowArt();

            if (choice == choices.Length) break;

            switch (choice)
            {
                case 1:
                    UpdateMenuTitle("lv_get_names");
                    Console.WriteLine(" Waiting to get all names...", Colors.InfoColor);
                    LobbyRevealer.GetAllNames();
                    Console.Clear();
                    Interface.ShowArt();
                    if (Global.PlayerList.Count == 0)
                    {
                        Console.WriteLine(" No Players found. Press Enter to continue...", Colors.ErrorColor);
                    }
                    else
                    {
                        foreach (Player player in Global.PlayerList)
                            Console.WriteLine($" [x] Player found: {player.Username}", Colors.SuccessColor);

                        Console.WriteLine(" \n Players found. Go to the 'Get OP.GG' Menu to get the OP.GG links.", Colors.SuccessColor);
                        Console.WriteLine(" Press Enter to continue...", Colors.SuccessColor);

                        foreach (Player player in Global.PlayerList) Global.UsernameList.Add(player.Username);

                        new Thread(() =>
                        {
                            OrderablePartitioner<string> source = Partitioner.Create(Global.UsernameList, EnumerablePartitionerOptions.NoBuffering);
                            ParallelOptions parallelOptions = new()
                            {
                                MaxDegreeOfParallelism = Global.PlayerList.Count
                            };

                            Parallel.ForEach(source, parallelOptions, delegate(string username, ParallelLoopState _)
                            {
                                Player player = Global.PlayerList.Find(x => string.Equals(x.Username, username, StringComparison.Ordinal));
                                LobbyRevealer.GetPlayerStats(player);
                            });

                            Thread.CurrentThread.Join();
                        }).Start();
                    }

                    Console.ReadKey();
                    Console.Clear();
                    Interface.ShowArt();
                    continue;
                case 2:
                    GetOpggMenu();
                    break;
                case 3:
                    if (Global.PlayerList.Count == 0)
                    {
                        Console.WriteLine(" No Players in the list. ", Colors.WarningColor);
                        Console.WriteLine(" Please use 'Get Names' to get players before trying to get the stats. ", Colors.WarningColor);
                        Console.WriteLine(" Press Enter to continue...", Colors.WarningColor);

                        Console.ReadKey();
                        Console.Clear();
                        Interface.ShowArt();
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
        List<string> choices = Global.PlayerList.Select(t => $"[OP.GG] - {t.Username}").ToList();

        if (Global.PlayerList.Count > 1)
            choices.Add("Get All OP.GG");
        choices.Add("Back");

        string[] choices2 = choices.ToArray();

        while (choice != choices2.Length)
        {
            ShowOpggMenu();

            MenuBuilder proxyMenu = MenuBuilder.BuildMenu(choices2, Console.CursorTop);
            choice = 10;
            while (choice == 10) choice = proxyMenu.RunMenu();

            System.Console.Clear();
            Interface.ShowArt();

            if (choice == choices2.Length) break;

            switch (choice)
            {
                case 1:
                    OpenUrl(Global.PlayerList[0].Link);
                    break;
                case 2:
                    OpenUrl(Global.PlayerList[1].Link);
                    break;
                case 3:
                    OpenUrl(Global.PlayerList[2].Link);
                    break;
                case 4:
                    OpenUrl(Global.PlayerList[3].Link);
                    break;
                case 5:
                    OpenUrl(Global.PlayerList[4].Link);
                    break;
                case 6:
                    string url = $"https://www.op.gg/multisearch/{Global.Region}?summoners=";
                    for (int i = 0; i < Global.PlayerList.Count; i++)
                    {
                        url += $"{Global.PlayerList[i].Username}";
                        if (i != Global.PlayerList.Count - 1)
                            url += ",";
                    }

                    OpenUrl(url);
                    break;
            }
        }

        GetLobbyRevealerMenu();
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
            border1.Children.Add(CreateSpan("Player " + (i + 1) + "      -  ", 1, Colors.MenuTextColor));
            string username = Global.PlayerList.ElementAtOrDefault(i)?.Username ?? "null\n";
            border1.Children.Add(CreateSpan($"{username}", 0, Colors.MenuPrimaryColor));
            if (username == "null\n") continue;
            border1.Children.Add(CreateSpan($" ({Global.PlayerList.ElementAtOrDefault(i)?.SoloDuoQ.Tier}", 0, Colors.MenuPrimaryColor));
            string f = Global.PlayerList.ElementAtOrDefault(i)?.SoloDuoQ.Division == 0 ? ")\n" : $" {Global.PlayerList.ElementAtOrDefault(i)?.SoloDuoQ.Division})\n";
            border1.Children.Add(CreateSpan(f, 0, Colors.MenuPrimaryColor));
        }

        border1.Children.Add(CreateSpan("\n Note: To get stats of players, go to the 'Get Stats' menu.", 1, Colors.MenuTextColor));

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void GetStatsMenu()
    {
        int choice = 10;
        UpdateMenuTitle("lv_get_stats");
        List<string> choices = Global.PlayerList.Select(t => $"[STATS] - {t.Username}").ToList();

        if (Global.PlayerList.Count > 0)
            choices.Add("Global Stats");
        choices.Add("Back");

        string[] choices2 = choices.ToArray();

        ShowGlobalStatsMenu();

        while (choice != choices2.Length)
        {
            MenuBuilder statsMenu = MenuBuilder.BuildMenu(choices2, Console.CursorTop);
            choice = 10;
            while (choice == 10) choice = statsMenu.RunMenu();

            Console.Clear();
            Interface.ShowArt();

            if (choice == choices2.Length) break;

            switch (choice)
            {
                case 1:
                    ShowPlayerStats(Global.PlayerList[0]);
                    break;
                case 2:
                    ShowPlayerStats(Global.PlayerList[1]);
                    break;
                case 3:
                    ShowPlayerStats(Global.PlayerList[2]);
                    break;
                case 4:
                    ShowPlayerStats(Global.PlayerList[3]);
                    break;
                case 5:
                    ShowPlayerStats(Global.PlayerList[4]);
                    break;
                case 6:
                    ShowGlobalStatsMenu();
                    break;
            }
        }

        MainMenu.StartMenu();
    }

    private static void ShowGlobalStatsMenu()
    {
        Console.SetCursorPosition(0, TopLength);

        int count = 0;
        Document rectangle = new();
        Grid grid = new()
        {
            Stroke = LineThickness.None,
            Align = Align.Center,
            Columns = { GridLength.Char(8), GridLength.Char(7), GridLength.Char(40), GridLength.Char(23) },
            Color = Colors.MenuPrimaryColor,
            MaxWidth = 110,
            Children =
            {
                new Cell("Player") { Stroke = LineThickness.Single },
                new Cell("Level") { Stroke = LineThickness.Single },
                new Cell("Name") { Stroke = LineThickness.Single },
                new Cell("Rank") { Stroke = LineThickness.Single }
            }
        };

        foreach (Player player in Global.PlayerList)
        {
            Cell playerCell = new($"N°{count + 1}") { Color = Colors.MenuTextColor };
            Cell levelCell = new(player.Level) { Color = Colors.MenuTextColor };
            Cell nameCell = new(player.Username) { Color = Colors.MenuTextColor };
            Cell rankCell = new($"{player.SoloDuoQ.Tier} {player.SoloDuoQ.Division} ({player.SoloDuoQ.Lp} LP)") { Color = Colors.MenuTextColor };

            grid.Children.Add(playerCell);
            grid.Children.Add(levelCell);
            grid.Children.Add(nameCell);
            grid.Children.Add(rankCell);

            count++;
        }

        rectangle.Children.Add(grid);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void ShowPlayerStats(Player player)
    {
        Console.SetCursorPosition(0, TopLength);

        int winratesoloq = (int)Math.Round((double)player.SoloDuoQ.Wins / (player.SoloDuoQ.Wins + player.SoloDuoQ.Losses) * 100);
        int winrateflex = (int)Math.Round((double)player.FlexQ.Wins / (player.FlexQ.Wins + player.FlexQ.Losses) * 100);

        if (winratesoloq <= 0) winratesoloq = 0;
        if (winrateflex <= 0) winrateflex = 0;

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
                        CreateSpan("[Ranked]", 11, Colors.MenuTextColor),
                        CreateSpan("\nRank       - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{FormatStr(player.SoloDuoQ.Tier)} {player.SoloDuoQ.Division}", 0, Colors.MenuPrimaryColor),
                        CreateSpan("\nLP         - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{player.SoloDuoQ.Lp}", 0, Colors.MenuPrimaryColor),
                        CreateSpan("\nWinrate    - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{winratesoloq}%\n\n", 0, Colors.MenuPrimaryColor),
                        CreateSpan("[Games Stats]", 8, Colors.MenuTextColor),
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
                        CreateSpan("  [Ranked]", 11, Colors.MenuTextColor),
                        CreateSpan("\n  Rank       - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{FormatStr(player.FlexQ.Tier)} {player.FlexQ.Division}", 0, Colors.MenuPrimaryColor),
                        CreateSpan("\n  LP         - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{player.FlexQ.Lp}", 0, Colors.MenuPrimaryColor),
                        CreateSpan("\n  Winrate    - ", 0, Colors.MenuTextColor),
                        CreateSpan($"{winrateflex}%\n\n", 0, Colors.MenuPrimaryColor),
                        CreateSpan("  [Games Stats]", 8, Colors.MenuTextColor),
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