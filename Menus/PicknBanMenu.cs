﻿using Alba.CsConsoleFormat;
using Loly.Menus.Core;
using Loly.Variables;
using static Loly.Tools.Utils;
using static Loly.Tools.PicknBan;
using static Loly.Menus.ToolsMenu;
using Console = Colorful.Console;

namespace Loly.Menus;

public class PicknBanMenu
{
    public static void GetPicknBanMenu()
    {
        while (true)
        {
            ShowPicknBanMenu();

            int choice = 7;
            UpdateMenuTitle("pnb");
            string[] choices = { "Pick Champion", "Pick Delay", "Ban Champion", "Ban Delay", "Back" };

            MenuBuilder mainMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop);
            while (choice == 7) choice = mainMenu.RunMenu();

            Console.Clear();
            Interface.ShowArt();

            if (choice == choices.Length) break;

            LoadChampionsList();

            switch (choice)
            {
                case 1:
                    AskChampionName(true);
                    break;
                case 2:
                    AskDelay(true);
                    break;
                case 3:
                    AskChampionName(false);
                    break;
                case 4:
                    AskDelay(false);
                    break;
            }
        }

        GetToolsMenu();
    }

    private static void ShowPicknBanMenu()
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
                CreateSpan("Pick and Ban", 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan("Configure which champion to auto pick / ban and delays.", 0, Colors.MenuTextColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void AskChampionName(bool pick)
    {
        MenuBuilder.SetCursorVisibility(true);
        UpdateMenuTitle(pick ? "pnb_pick" : "pnb_ban");

        string action = pick ? "pick" : "ban";
        string champName = "";
        while (champName == "")
        {
            Console.Write(DateTime.Now.ToString("[hh:mm:ss]"), Colors.PrimaryColor);
            Console.Write($"» Enter name of champion you want to auto {action}:", Colors.InfoColor);
            Console.WriteLine("");
            Console.Write("» ");

            try
            {
                champName = Console.ReadLine().ToLower();

                if (champName == "")
                {
                    Console.WriteLine("[WARNING]» Champion name cannot be empty !", Colors.WarningColor);
                    continue;
                }

                ChampItem champ = Global.ChampionsList.Find(x => x.Name.ToLower() == champName);
                if (champ == null)
                {
                    Console.WriteLine($"[WARNING]» Champion \"{FormatStr(champName)}\" does not exist !", Colors.WarningColor);
                    champName = "";
                }
                else if (!champ.Free && action != "ban")
                {
                    Console.WriteLine($"[WARNING]» You can't select the '{FormatStr(champName)}' champion because you don't own it.", Colors.WarningColor);
                    Console.WriteLine("Please buy the champion, restart the software and try again !", Colors.WarningColor);
                    champName = "";
                }
                else
                {
                    if (action == "pick")
                    {
                        Settings.PickChamp.Name = champ.Name;
                        Settings.PickChamp.Id = champ.Id;
                        Settings.PickChamp.Free = champ.Free;
                    }
                    else
                    {
                        Settings.BanChamp.Name = champ.Name;
                        Settings.BanChamp.Id = champ.Id;
                        Settings.BanChamp.Free = champ.Free;
                    }

                    Settings.SaveSettings();

                    Console.WriteLine("");
                    Console.WriteLine($"[SUCCESS]» Champion '{FormatStr(champName)}' selected successfully for {action} !", Colors.SuccessColor);
                    Console.WriteLine("Press any key to continue...", Colors.SuccessColor);

                    Console.ReadKey();
                    Console.Clear();
                    Interface.ShowArt();
                }
            }
            catch
            {
                Console.WriteLine($"[WARNING]» Champion \"{FormatStr(champName)}\" does not exist !", Colors.WarningColor);
                champName = "";
            }
        }
    }

    private static void AskDelay(bool pick)
    {
        MenuBuilder.SetCursorVisibility(true);
        UpdateMenuTitle(pick ? "pnb_pick_delay" : "pnb_ban_delay");

        string action = pick ? "pick" : "ban";
        string input = "";
        int delay = 0;
        int minDelay = pick ? 1000 : 1500;
        int maxDelay = pick ? 20000 : 15000;

        while (delay == 0 || delay < minDelay || delay > maxDelay)
        {
            Console.Write(DateTime.Now.ToString("[hh:mm:ss]"), Colors.PrimaryColor);
            Console.Write($"» Enter delay to auto {action} champion (recommanded {minDelay}-{maxDelay}ms):\n", Colors.InfoColor);
            Console.Write(DateTime.Now.ToString("[hh:mm:ss]"), Colors.PrimaryColor);
            Console.Write("» Enter the delay in miliseconds (ex: 1500 = 1,5sec, 4000 = 4sec...)", Colors.InfoColor);
            Console.WriteLine("");
            Console.Write("» ");

            try
            {
                input = Console.ReadLine();
                delay = Convert.ToInt32(input);

                if (delay == 0 || delay < minDelay || delay > maxDelay)
                {
                    Console.WriteLine($"[WARNING]» Delay must be between {minDelay}ms and {maxDelay}ms !", Colors.WarningColor);
                }
                else
                {
                    if (action == "pick")
                        Settings.PickDelay = delay;
                    else
                        Settings.BanDelay = delay;

                    Settings.SaveSettings();

                    Console.WriteLine("");
                    Console.WriteLine($"[SUCCESS]» The {delay}ms delay has been configured correctly for auto {action} !", Colors.SuccessColor);
                    Console.WriteLine("Press any key to continue...", Colors.SuccessColor);

                    Console.ReadKey();
                    Console.Clear();
                    Interface.ShowArt();
                }
            }
            catch
            {
                Console.WriteLine($"[WARNING]» Unable to convert '{input}' to delay !", Colors.WarningColor);
                Console.WriteLine("Please try again... ", Colors.WarningColor);
                input = "";
                delay = 0;
            }
        }
    }
}