using Alba.CsConsoleFormat;

using Gommon;

using Loly.src.Logs;
using Loly.src.Menus.Core;
using Loly.src.Variables.Class;

using static Loly.src.Menus.Core.Interface;
using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Menus;

public class LolSettingsMenu
{
    #region Get Menus

    public static void GetLoLSettingsMenu()
    {
        while (true)
        {
            UpdateMenuTitle("lols");
            ShowLolSettings();

            string[] choices = { "Import Settings", "Export Settings", "Back" };

            var pickNBanMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop);
            var choice = 0;
            while (choice == 0) choice = pickNBanMenu.RunMenu();
            ResetConsole();

            if (choice == choices.Length) break;

            if (choice == 1) GetImportSettingsMenu();
            else GetExportSettingsMenu();
        }
    }

    private static void GetImportSettingsMenu()
    {
        while (true)
        {
            UpdateMenuTitle("lols_import");
            ShowImportSettingMenu();

            List<string> choices = new();

            var files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, Logger.LolSettingsFolder), "*.json");
            if (files.Length == 0)
            {
                DisplayColor($"`[WARNING]»` No settings found in the '{Logger.LolSettingsFolder}' folder", Colors.InfoColor, Colors.WarningColor);
                DisplayColor($"`[WARNING]»` You need to export at least one settings file with 'Export Settings' menu before import settings...", Colors.InfoColor, Colors.WarningColor);
                DisplayColor($"`[WARNING]»` Press any key to continue...", Colors.InfoColor, Colors.WarningColor);
                Console.ReadKey();
                break;
            }

            files.ForEach(f => choices.Add(Path.GetFileNameWithoutExtension(f)));
            choices.Add("Back");

            var pickNBanMenu = MenuBuilder.BuildMenu(choices.ToArray(), Console.CursorTop + 1);
            var choice = 0;
            while (choice == 0) choice = pickNBanMenu.RunMenu();
            ResetConsole();

            if (choice == choices.Count) break;

            File.SetAttributes($"{LeagueClientPath}\\Config\\PersistedSettings.json", FileAttributes.Normal);
            File.Copy($"{Logger.LolSettingsFolder}\\{choices.ElementAt(choice - 1)}.json", $"{LeagueClientPath}\\Config\\PersistedSettings.json", true);
            File.SetAttributes($"{LeagueClientPath}\\Config\\PersistedSettings.json", FileAttributes.ReadOnly);

            DisplayColor($"`[SUCCESS]»` The settings has been imported !", Colors.InfoColor, Colors.SuccessColor);
            DisplayColor($"`[SUCCESS]»` You need to `RESTART` the client to apply the settings !!!", Colors.InfoColor, Colors.SuccessColor);
            DisplayColor($"`[SUCCESS]»` Press any key to continue...", Colors.InfoColor, Colors.SuccessColor);
            Console.ReadKey();
            break;
        }

        ResetConsole();
    }

    private static void GetExportSettingsMenu()
    {
        UpdateMenuTitle("lols_export");
        ShowExportSettingMenu();
        Console.WriteLine(Environment.NewLine);

        if (File.Exists($"{Logger.LolSettingsFolder}\\SETTINGS - {SummonerLogged.GetFullGameName()}.json"))
        {
            DisplayColor($"`[INFORMATION]»` The settings file of '{SummonerLogged.GetFullGameName()}' already exists in the '{Logger.LolSettingsFolder}' folder", Colors.InfoColor, Colors.PrimaryColor);
            DisplayColor($"`[INFORMATION]»` Do you want to overwrite it ? (Y/N)", Colors.InfoColor, Colors.PrimaryColor);
            MenuBuilder.SetCursorVisibility(true);
            Console.Write("» ");

            ConsoleKey key = Console.ReadKey().Key;

            Console.WriteLine(Environment.NewLine);
            MenuBuilder.SetCursorVisibility(false);

            if (key != ConsoleKey.Y)
            {
                DisplayColor($"`[INFORMATION]»` The settings has not been exported", Colors.InfoColor, Colors.PrimaryColor);
                DisplayColor($"`[INFORMATION]»` Press any key to continue...", Colors.InfoColor, Colors.PrimaryColor);
                Console.ReadKey();
                ResetConsole();
                return;
            }
        }

        File.Copy($"{LeagueClientPath}\\Config\\PersistedSettings.json", $"{Logger.LolSettingsFolder}\\SETTINGS - {SummonerLogged.GetFullGameName()}.json", true);

        DisplayColor($"`[SUCCESS]»` The settings has been exported to the '{Logger.LolSettingsFolder}' folder", Colors.InfoColor, Colors.SuccessColor);
        DisplayColor($"`[SUCCESS]»` You can use name of file settings to import them with the 'Import Settings' menu", Colors.InfoColor, Colors.SuccessColor);
        DisplayColor($"`[SUCCESS]»` Press any key to continue...", Colors.InfoColor, Colors.SuccessColor);
        Console.ReadKey();

        ResetConsole();
    }

    #endregion

    #region Show Menus

    private static void ShowLolSettings()
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
                CreateSpan("Import/Export LoL Settings", 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan("Choose the option (Import/Export) settings of League of Legends", 0, Colors.MenuTextColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void ShowImportSettingMenu()
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
                CreateSpan("Import Settings", 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan("Choose the settings file to import\n", 0, Colors.MenuTextColor),
                CreateSpan($"All the files listed here come from the '{Logger.LolSettingsFolder}' folder", 0, Colors.MenuTextColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void ShowExportSettingMenu()
    {
        Console.SetCursorPosition(0, TopLength);

        Document rectangle = new();
        Border border1 = new()
        {
            MinWidth = 75,
            MaxWidth = 75,
            Stroke = LineThickness.None,
            Align = Align.Center,
            TextAlign = TextAlign.Center,
            Color = Colors.MenuPrimaryColor,
            TextWrap = TextWrap.WordWrap,
            Children =
            {
                CreateSpan("Export Settings", 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan($"The name of settings file will be the summoner game name currently logged\n", 0, Colors.MenuTextColor),
                CreateSpan($"You can import settings later in another LoL account\n", 0, Colors.MenuTextColor),
                CreateSpan($"All the settings are listed in '{Logger.LolSettingsFolder}' folder", 0, Colors.MenuTextColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    #endregion
}
