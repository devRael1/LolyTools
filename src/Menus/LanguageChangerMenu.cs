using Alba.CsConsoleFormat;
using Loly.src.Menus.Core;
using Loly.src.Variables.Class;
using static Loly.src.Menus.Core.Interface;
using static Loly.src.Menus.ToolsMenu;
using static Loly.src.Tools.LanguageChanger;
using static Loly.src.Tools.Utils;

namespace Loly.src.Menus;

public class LanguageChangerMenu
{
    private static readonly string[] Languages =
    {
        "en_US", "en_GB", "fr_FR",
        "de_DE", "en_AU", "it_IT",
        "pl_PL", "pt_BR", "ru_RU",
        "es_MX", "es_ES", "tr_TR",
        "ja_JP", "ko_KR", "zh_CN"
    };

    private static string _languageChoosed;
    private static string _codeLanguageChoosed;
    private static string _exePath;

    public static void GetLanguageChangerMenu()
    {
        while (true)
        {
            ShowLanguageChangerMenu();

            int choice = 7;
            UpdateMenuTitle("lc");
            string[] choices = { "Create new Shortcut", "Back" };

            MenuBuilder languageChangerMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop + 1);
            while (choice == 7)
            {
                choice = languageChangerMenu.RunMenu();
            }

            ResetConsole();

            if (choice == choices.Length)
            {
                break;
            }

            GetLanguageMenu();
        }

        GetToolsMenu();
    }

    private static void GetLanguageMenu()
    {
        while (true)
        {
            ShowLanguageMenu();

            int choice = 40;
            UpdateMenuTitle("lc_languages");
            string[] choices =
            {
                "English USA", "English Great Britan", "French",
                "German ", "English Australian", "Italian",
                "Polish", "Portuguese ", "Russian",
                "Spanish Latin America", "Spanish Spain", "Turkish",
                "Japanese", "Korean", "Chinese",
                "Back"
            };

            MenuBuilder languagesMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop);
            while (choice == 40)
            {
                choice = languagesMenu.RunMenu();
            }

            ResetConsole();

            if (choice == choices.Length)
            {
                break;
            }

            _languageChoosed = choices[choice - 1];
            _codeLanguageChoosed = Languages[choice - 1];

            string exe = AutoDetectExePath();
            if (exe == null)
            {
                GetExeMenu();
            }
            else
            {
                _exePath = exe;
            }

            ChangeLanguage();
        }

        GetLanguageChangerMenu();
    }

    private static void GetExeMenu()
    {
        MenuBuilder.SetCursorVisibility(true);
        UpdateMenuTitle("lc_exe");

        string path = "";

        while (!File.Exists(path))
        {
            DisplayColor($"`{DateTime.Now:[hh:mm:ss]}`» Drag and drop your 'LeagueClient.exe' file:", Colors.InfoColor, Colors.PrimaryColor);
            Console.Write("» ");

            try
            {
                path = Console.ReadLine().Replace("\"", "");

                if (path == "")
                {
                    DisplayColor("[WARNING]» The path cannot by empty...", Colors.WarningColor, Colors.PrimaryColor);
                }
                else
                {
                    if (!File.Exists(path))
                    {
                        DisplayColor("[WARNING]» The file does not exist...", Colors.WarningColor, Colors.PrimaryColor);
                    }
                    else
                    {
                        _exePath = path;
                    }
                }
            }
            catch
            {
                DisplayColor("[WARNING]» Unable to get path of the file !", Colors.WarningColor, Colors.PrimaryColor);
                DisplayColor("[WARNING]» Please try again... ", Colors.WarningColor, Colors.PrimaryColor);
                path = "";
            }
        }
    }

    private static void ChangeLanguage()
    {
        ResetConsole();
        ShowConfirmMenu();

        int choice = 5;
        UpdateMenuTitle("lc_confirm");
        string[] choices = { "Yes", "No" };

        MenuBuilder languageMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop);
        while (choice == 5)
        {
            choice = languageMenu.RunMenu();
        }

        if (choice == 1)
        {
            ResetConsole();
            try
            {
                CreateShortcut(_codeLanguageChoosed, _exePath);
                DisplayColor("[SUCCESS]» Shortcut created successfully !", Colors.SuccessColor, Colors.PrimaryColor);
                DisplayColor("To play with the new language, you need to close your 'League of Legends' and the 'Riot Client' completely !", Colors.InfoColor, Colors.PrimaryColor);
                DisplayColor("After this done, you can use the new shortcut on your desktop !", Colors.InfoColor, Colors.PrimaryColor);
                DisplayColor("Press 'Enter' to continue...", Colors.InfoColor, Colors.PrimaryColor);
            }
            catch
            {
                DisplayColor("[ERROR]» Unable to create shortcut !", Colors.ErrorColor, Colors.PrimaryColor);
                DisplayColor("Please try again later...", Colors.ErrorColor, Colors.PrimaryColor);
                DisplayColor("Press 'Enter' to continue....", Colors.ErrorColor, Colors.PrimaryColor);
            }

            Console.ReadKey();
        }

        ResetConsole();
        GetLanguageChangerMenu();
    }

    private static void ShowConfirmMenu()
    {
        Console.SetCursorPosition(0, TopLength);

        Document rectangle = new();
        Border border1 = new()
        {
            MinWidth = 65,
            MaxWidth = 65,
            Stroke = LineThickness.None,
            Align = Align.Center,
            TextAlign = TextAlign.Center,
            Color = Colors.MenuPrimaryColor,
            TextWrap = TextWrap.WordWrap,
            Children =
            {
                CreateSpan("Confirmation", 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan("Confirm if you want to create the shortcut on your desktop\n\n", 0, Colors.MenuTextColor),
                CreateSpan("Language     - ", 0, Colors.MenuTextColor),
                CreateSpan(_languageChoosed, 0, Colors.MenuPrimaryColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void ShowLanguageChangerMenu()
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
                CreateSpan("Language Changer", 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan("Change language of your League of Legends (Shortcut on Desktop)", 0, Colors.MenuTextColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void ShowLanguageMenu()
    {
        Console.SetCursorPosition(0, TopLength);

        Document rectangle = new();
        Border border1 = new()
        {
            MinWidth = 65,
            MaxWidth = 65,
            Stroke = LineThickness.None,
            Align = Align.Center,
            TextAlign = TextAlign.Center,
            Color = Colors.MenuPrimaryColor,
            TextWrap = TextWrap.WordWrap,
            Children =
            {
                CreateSpan("Language Selection", 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan("Choose language to create new shortcut on your Desktop", 0, Colors.MenuTextColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }
}