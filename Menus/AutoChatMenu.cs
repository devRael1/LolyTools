using Alba.CsConsoleFormat;
using Loly.Menus.Core;
using Loly.Variables;
using static Loly.Tools.Utils;
using static Loly.Menus.ToolsMenu;
using static Loly.Tools.AutoChat;
using Console = Colorful.Console;

namespace Loly.Menus;

public class AutoChatMenu
{
    public static void GetAutoChatMenu()
    {
        while (true)
        {
            ShowAutoChatMenu();

            int choice = 7;
            UpdateMenuTitle("ac");
            string[] choices = { "Add Message", "Delete Message", "See Messages", "Clear Messages", "Back" };

            MenuBuilder mainMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop);
            while (choice == 7) choice = mainMenu.RunMenu();

            Console.Clear();
            Interface.ShowArt();

            if (choice == choices.Length) break;

            switch (choice)
            {
                case 1:
                    AddMessage();
                    break;
                case 2:
                    // TODO: Delete a specific message
                    break;
                case 3:
                    // TODO: See all messages
                    break;
                case 4:
                    // TODO: Clear all messages
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

        string msg = "";
        while (msg == "")
        {
            Console.Write(DateTime.Now.ToString("[hh:mm:ss]"), Colors.PrimaryColor);
            Console.Write("» Enter message to send automatically when you enterring in lobby (max 200 characters):", Colors.InfoColor);
            Console.WriteLine("");
            Console.Write("» ");

            try
            {
                msg = Console.ReadLine();

                if (msg == "")
                {
                    Console.WriteLine("[WARNING]» Your message is empty ! Please try again... ", Colors.WarningColor);
                }
                else if (msg.Length > 200)
                {
                    Console.WriteLine("[WARNING]» Your message is too long ! Please try again... ", Colors.WarningColor);
                }
                else if (Settings.ChatMessages.Count >= 5)
                {
                    Console.WriteLine("[WARNING]» You can't add more than 5 messages ! Please remove 1 message before adding a new one... ", Colors.WarningColor);
                    Console.WriteLine("Press any key to continue...", Colors.SuccessColor);

                    Console.ReadKey();
                    Console.Clear();
                    Interface.ShowArt();

                    msg = "bypass";
                }
                else
                {
                    msg = FormatMessage(msg);
                    Settings.ChatMessages.Add(msg);

                    Settings.SaveSettings();

                    Console.WriteLine("");
                    Console.WriteLine("[SUCCESS]» Your message has been added successfully...", Colors.SuccessColor);
                    Console.WriteLine("Press any key to continue...", Colors.SuccessColor);

                    Console.ReadKey();
                    Console.Clear();
                    Interface.ShowArt();
                }
            }
            catch
            {
                Console.WriteLine("[ERROR]» An error has occured ! Please try again... ", Colors.ErrorColor);
                msg = "";
            }
        }
    }

    private static void DeleteMessage()
    {
    }
}