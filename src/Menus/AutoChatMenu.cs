using Alba.CsConsoleFormat;

using Loly.src.Menus.Core;
using Loly.src.Variables;
using Loly.src.Variables.Class;

using static Loly.src.Menus.Core.Interface;
using static Loly.src.Menus.ToolsMenu;
using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Menus;

public class AutoChatMenu
{
    public static void GetAutoChatMenu()
    {
        while (true)
        {
            ShowAutoChatMenu();

            var choice = 7;
            UpdateMenuTitle("ac");
            string[] choices = { "Add Message", "Delete Message", "See Messages", "Clear Messages", "Back" };

            var autoChatMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop + 1);
            while (choice == 7) choice = autoChatMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Length) break;

            switch (choice)
            {
                case 1:
                    AddMessage();
                    break;
                case 2:
                    DeleteMessage();
                    break;
                case 3:
                    SeeMessages();
                    break;
                case 4:
                    ClearMessages();
                    break;
            }
        }

        GetToolsMenu();
    }

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

    private static void AddMessage()
    {
        MenuBuilder.SetCursorVisibility(true);
        UpdateMenuTitle("ac_add");

        var msg = "";
        while (msg == "")
        {
            DisplayColor($"`{DateTime.Now:[hh:mm:ss]}`» Enter message to send automatically when you enterring in lobby (max 200 characters):", Colors.InfoColor, Colors.PrimaryColor);
            Console.Write("» ");

            try
            {
                msg = Console.ReadLine();

                if (msg == "")
                {
                    DisplayColor("[WARNING]» Your message is empty ! Please try again... ", Colors.WarningColor, Colors.PrimaryColor);
                }
                else if (msg.Length > 200)
                {
                    DisplayColor("[WARNING]» Your message is too long ! Please try again... ", Colors.WarningColor, Colors.PrimaryColor);
                }
                else if (Settings.ChatMessages.Count >= 5)
                {
                    DisplayColor("[WARNING]» You can't add more than 5 messages ! Please remove 1 message before adding a new one... ", Colors.WarningColor, Colors.PrimaryColor);
                    DisplayColor("[WARNING]» Press any key to continue...", Colors.WarningColor, Colors.PrimaryColor);
                    Console.ReadKey();
                    ResetConsole();
                    msg = "bypass";
                }
                else
                {
                    msg = FormatMessage(msg);
                    Settings.ChatMessages.Add(msg);

                    Settings.SaveSettings();

                    Console.Write(Environment.NewLine);
                    DisplayColor("[SUCCESS]» Your message has been added successfully...", Colors.SuccessColor, Colors.PrimaryColor);
                    DisplayColor("[SUCCESS]» Press any key to continue...", Colors.SuccessColor, Colors.PrimaryColor);
                    Console.ReadKey();
                    ResetConsole();
                }
            }
            catch (Exception ex)
            {
                DisplayColor("[ERROR]» An error has occured !", Colors.ErrorColor, Colors.PrimaryColor);
                DisplayColor(ex.ToString(), Colors.ErrorColor, Colors.PrimaryColor);
                DisplayColor("[ERROR]» Press any key to retry...", Colors.ErrorColor, Colors.PrimaryColor);
                Console.ReadKey();
                msg = "";
            }
        }
    }

    private static void DeleteMessage()
    {
    START:
        var choice = 10;
        UpdateMenuTitle("ac_del");
        var choices = Settings.ChatMessages.Select((x, index) => $"Message N°{index + 1}").ToList();
        choices.Add("Back");

        while (choice != choices.Count)
        {
            ShowMessages();

            var delMessageMenu = MenuBuilder.BuildMenu(choices.ToArray(), Console.CursorTop + 1);
            choice = 10;
            while (choice == 10)
            {
                choice = delMessageMenu.RunMenu();
            }

            ResetConsole();

            if (choice == choices.Count)
            {
                break;
            }

            Settings.ChatMessages.RemoveAt(choice - 1);
            Settings.SaveSettings();
            goto START;
        }
    }

    private static void SeeMessages()
    {
        var choice = 10;
        UpdateMenuTitle("ac_see");
        string[] choices = { "Back" };

        while (choice == 10)
        {
            ShowMessages();

            var seeMessageMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop + 1);
            choice = 10;
            while (choice == 10)
            {
                choice = seeMessageMenu.RunMenu();
            }

            ResetConsole();

            if (choice == choices.Length)
            {
                break;
            }
        }
    }

    private static void ClearMessages()
    {
        MenuBuilder.SetCursorVisibility(true);

        UpdateMenuTitle("ac_clear");

        DisplayColor($"`{DateTime.Now:[hh:mm:ss]}`» Are you sure you want to clear all messages ? (y/n):", Colors.InfoColor, Colors.PrimaryColor);
        Console.Write("» ");

        var choice = Console.ReadLine().ToLower();
        if (choice == "y")
        {
            Settings.ChatMessages.Clear();
            Settings.SaveSettings();

            Console.Write(Environment.NewLine);
            DisplayColor("[SUCCESS]» All messages have been cleared successfully", Colors.SuccessColor, Colors.PrimaryColor);
            DisplayColor("[SUCCESS]» Press any key to continue...", Colors.SuccessColor, Colors.PrimaryColor);
            Console.ReadKey();
            ResetConsole();
        }
        else
        {
            ResetConsole();
        }
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
        foreach (var message in Settings.ChatMessages)
        {
            count++;
            grid.Children.Add(new Cell($"N°{count}") { Color = Colors.MenuTextColor, Stroke = LineThickness.None, Padding = new Thickness(1) });

            if (message.Length > 90)
            {
                var msg = message[..90];
                msg += "...";
                grid.Children.Add(new Cell(msg) { Color = Colors.MenuTextColor, Stroke = LineThickness.None, Padding = new Thickness(1) });
            }
            else
            {
                grid.Children.Add(new Cell(message) { Color = Colors.MenuTextColor, Stroke = LineThickness.None, Padding = new Thickness(1) });
            }
        }

        rectangle.Children.Add(grid);
        ConsoleRenderer.RenderDocument(rectangle);
    }
}