using Alba.CsConsoleFormat;

using Loly.src.Menus.Core;
using Loly.src.Tools;
using Loly.src.Variables.Class;

using static Loly.src.Menus.Core.Interface;
using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Menus;

internal static class AutoChatMenu
{
    #region Get Menus

    internal static void GetAutoChatMenu()
    {
        while (true)
        {
            ShowAutoChatMenu();
            UpdateMenuTitle("ac");

            string[] choices = { "Add Message", "Delete Message", "See Messages", "Clear Messages", "Back" };
            var autoChatMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop + 1);
            var choice = 0;
            while (choice == 0) choice = autoChatMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Length) break;

            switch (choice)
            {
                case 1:
                    GetAddMessageMenu();
                    break;
                case 2:
                    GetDeleteMessageMenu();
                    break;
                case 3:
                    GetSeeMessagesMenu();
                    break;
                case 4:
                    GetClearMessagesMenu();
                    break;
            }
        }
    }

    private static void GetAddMessageMenu()
    {
        MenuBuilder.SetCursorVisibility(true);
        UpdateMenuTitle("ac_add");

        var msg = "";
        while (msg == "")
        {
            DisplayColor($"`{DateTime.Now:[hh:mm:ss]}»` Enter message to send automatically when you enterring in lobby (max 200 characters):", Colors.InfoColor, Colors.PrimaryColor);
            Console.Write("» ");

            msg = Console.ReadLine();

            if (msg == "" || msg.Length > 200 || CurrentSettings.AutoChat.ChatMessages.Count >= 5)
            {
                if (msg == "") DisplayColor("`[WARNING]»` Your message is empty ! Please try again...", Colors.InfoColor, Colors.WarningColor);
                else if (msg.Length > 200) DisplayColor("`[WARNING]»` Your message is too long ! Please try again...", Colors.InfoColor, Colors.WarningColor);
                else if (CurrentSettings.AutoChat.ChatMessages.Count >= 5)
                {
                    DisplayColor("`[WARNING]»` You can't add more than 5 messages ! Please remove 1 message before adding a new one", Colors.InfoColor, Colors.WarningColor);
                    DisplayColor("`[WARNING]»` Press any key to continue...", Colors.InfoColor, Colors.WarningColor);
                    Console.ReadKey();
                    ResetConsole();
                }

                break;
            }

            msg = FormatMessage(msg);
            CurrentSettings.AutoChat.ChatMessages.Add(msg);
            SettingsManager.SaveFileSettings();

            Console.Write(Environment.NewLine);
            DisplayColor("`[SUCCESS]»` Your message has been added successfully", Colors.InfoColor, Colors.SuccessColor);
            DisplayColor("`[SUCCESS]»` Press any key to continue...", Colors.InfoColor, Colors.SuccessColor);
            Console.ReadKey();
            ResetConsole();
        }
    }

    private static void GetDeleteMessageMenu()
    {
        UpdateMenuTitle("ac_del");

        while (true)
        {
            ShowMessages();

            var choices = CurrentSettings.AutoChat.ChatMessages.Select((x, index) => $"Message N°{index + 1}").ToList();
            choices.Add("Back");

            var choice = 0;
            var delMessageMenu = MenuBuilder.BuildMenu(choices.ToArray(), Console.CursorTop + 1);
            while (choice == 0) choice = delMessageMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Count) break;

            CurrentSettings.AutoChat.ChatMessages.RemoveAt(choice - 1);
            SettingsManager.SaveFileSettings();
        }
    }

    private static void GetSeeMessagesMenu()
    {
        UpdateMenuTitle("ac_see");

        while (true)
        {
            ShowMessages();

            string[] choices = { "Back" };
            var choice = 0;
            var seeMessageMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop + 1);
            while (choice == 0) choice = seeMessageMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Length) break;
        }
    }

    private static void GetClearMessagesMenu()
    {
        MenuBuilder.SetCursorVisibility(true);
        UpdateMenuTitle("ac_clear");

        DisplayColor($"`{DateTime.Now:[hh:mm:ss]}`» Are you sure you want to clear all messages ? (Y/N):", Colors.InfoColor, Colors.PrimaryColor);
        Console.Write("» ");

        ConsoleKey key = Console.ReadKey().Key;
        if (key == ConsoleKey.Y)
        {
            CurrentSettings.AutoChat.ChatMessages.Clear();
            SettingsManager.SaveFileSettings();

            Console.Write(Environment.NewLine);
            DisplayColor("`[SUCCESS]»` All messages have been cleared successfully", Colors.InfoColor, Colors.SuccessColor);
            DisplayColor("`[SUCCESS]»` Press any key to continue...", Colors.InfoColor, Colors.SuccessColor);
            Console.ReadKey();
        }

        ResetConsole();
    }

    #endregion

    #region Show Menus

    private static void ShowAutoChatMenu()
    {
        Console.SetCursorPosition(0, TopLength);

        Document rectangle = new();
        Border border1 = new()
        {
            MinWidth = 70,
            MaxWidth = 70,
            Stroke = LineThickness.None,
            Align = Align.Center,
            TextAlign = TextAlign.Center,
            Color = Colors.MenuPrimaryColor,
            TextWrap = TextWrap.WordWrap,
            Children =
            {
                CreateSpan("Auto Chat", 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan("Configure the messages you want to send automatically in the chat.", 0, Colors.MenuTextColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void ShowMessages()
    {
        Console.SetCursorPosition(0, TopLength);

        Document rectangle = new();
        Grid grid = new()
        {
            Stroke = LineThickness.None,
            Align = Align.Center,
            Columns = { GridLength.Char(10), GridLength.Char(90) },
            Color = Colors.MenuPrimaryColor,
            MaxWidth = 110,
            Children =
            {
                new Cell("Number") { Stroke = LineThickness.None },
                new Cell("Message Body") { Stroke = LineThickness.None }
            }
        };

        var count = 0;
        foreach (var message in CurrentSettings.AutoChat.ChatMessages)
        {
            count++;
            grid.Children.Add(new Cell($"N°{count}") { Color = Colors.MenuTextColor, Stroke = LineThickness.None, Padding = new Thickness(1) });

            if (message.Length > 90)
            {
                var msg = message[..90];
                msg += "...";
                grid.Children.Add(new Cell(msg) { Color = Colors.MenuTextColor, Stroke = LineThickness.None, Padding = new Thickness(1) });
            }
            else grid.Children.Add(new Cell(message) { Color = Colors.MenuTextColor, Stroke = LineThickness.None, Padding = new Thickness(1) });
        }

        rectangle.Children.Add(grid);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    #endregion
}