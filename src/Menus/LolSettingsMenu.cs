using Alba.CsConsoleFormat;

using Gommon;

using Loly.src.Logs;
using Loly.src.Menus.Core;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;

using static Loly.src.Menus.Core.Interface;
using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Menus;

public class LolSettingsMenu
{
    public static void GetLoLSettingsMenu()
    {
        while (true)
        {
            ShowLolSettings();

            var choice = 7;
            UpdateMenuTitle("lols");

            string[] choices = { "Import Settings", "Export Settings", "Back" };

            var pickNBanMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop);
            while (choice == 7) choice = pickNBanMenu.RunMenu();
            ResetConsole();

            if (choice == choices.Length) break;

            if (choice == 1) GetImportSettingsMenu();
            else StartExportSettingsMenu();
        }
    }

    private static void GetImportSettingsMenu()
    {
        UpdateMenuTitle("lols_import");
        ResetConsole();

        while (true)
        {
            ShowImportSettingMenu();

            var choice = 0;
            List<string> choices = new();

            var files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, Logger.LolSettingsFolder), "*.json");
            if (files.Length == 0)
            {
                Logger.Warn(LogModule.LolSettings, $"No settings found in the '{Logger.LolSettingsFolder}' folder", null, LogType.Console);
                Logger.Warn(LogModule.LolSettings, $"You need to export at least one settings file with 'Export Settings' menu before import settings...", null, LogType.Console);
                Logger.Warn(LogModule.LolSettings, $"Press any key to continue...", null, LogType.Console);
                Console.ReadKey();
                break;
            }

            files.ForEach(f => choices.Add(Path.GetFileNameWithoutExtension(f)));
            choices.Add("Back");

            var pickNBanMenu = MenuBuilder.BuildMenu(choices.ToArray(), Console.CursorTop + 1);
            while (choice == 0) choice = pickNBanMenu.RunMenu();
            ResetConsole();

            if (choice == choices.Count) break;

            try
            {
                File.SetAttributes($"{LeagueClientPath}\\Config\\PersistedSettings.json", FileAttributes.Normal);
                File.Copy($"{Logger.LolSettingsFolder}\\{choices.ElementAt(choice - 1)}.json", $"{LeagueClientPath}\\Config\\PersistedSettings.json", true);
                File.SetAttributes($"{LeagueClientPath}\\Config\\PersistedSettings.json", FileAttributes.ReadOnly);

                Logger.Info(LogModule.LolSettings, $"The settings has been imported !", LogType.Console);
                Logger.Info(LogModule.LolSettings, $"You need to RESTART the client to apply the settings !!!", LogType.Console);
                Logger.Info(LogModule.LolSettings, $"Press any key to continue...", LogType.Console);
                Console.ReadKey();
                break;
            }
            catch (Exception ex)
            {
                Logger.Error(LogModule.LolSettings, $"Unable to import settings", null, LogType.Both);
                Logger.Error(LogModule.LolSettings, "Please check the logs file for more information", null, LogType.Both);
                Logger.Error(LogModule.LolSettings, "Error : ", ex, LogType.Both);
                Logger.Error(LogModule.LolSettings, $"Pres any key to back to the 'LoL Settings' menu...", ex, LogType.Console);
                Console.ReadKey();
                break;
            }
        }

        ResetConsole();
    }

    private static void StartExportSettingsMenu()
    {
        UpdateMenuTitle("lols_export");
        ResetConsole();

        ShowExportSettingMenu();
        Console.WriteLine(Environment.NewLine);

        try
        {
            if (File.Exists($"{Logger.LolSettingsFolder}\\SETTINGS - {SummonerLogged.GetFullGameName()}.json"))
            {
                Logger.Warn(LogModule.LolSettings, $"The settings file of '{SummonerLogged.GetFullGameName()}' already exists in the '{Logger.LolSettingsFolder}' folder", null, LogType.Console);
                Logger.Warn(LogModule.LolSettings, $"Do you want to overwrite it ? (Y/N)", null, LogType.Console);
                MenuBuilder.SetCursorVisibility(true);
                Console.Write("» ");

                ConsoleKey key = Console.ReadKey().Key;
                if (key != ConsoleKey.Y)
                {
                    Console.WriteLine(Environment.NewLine);
                    Logger.Info(LogModule.LolSettings, $"The settings has not been exported", LogType.Console);
                    Logger.Info(LogModule.LolSettings, $"Press any key to continue...", LogType.Console);
                    Console.ReadKey();
                    ResetConsole();
                    return;
                }
            }

            File.Copy($"{LeagueClientPath}\\Config\\PersistedSettings.json", $"{Logger.LolSettingsFolder}\\SETTINGS - {SummonerLogged.GetFullGameName()}.json", true);

            Console.WriteLine(Environment.NewLine);
            Logger.Info(LogModule.LolSettings, $"The settings has been exported to the '{Logger.LolSettingsFolder}' folder", LogType.Console);
            Logger.Info(LogModule.LolSettings, $"You can use name of file settings to import them with the 'Import Settings' menu", LogType.Console);
            Logger.Info(LogModule.LolSettings, $"Press any key to continue...", LogType.Console);
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(Environment.NewLine);
            Logger.Error(LogModule.LolSettings, $"\nUnable to export settings in the '{Logger.LolSettingsFolder}' folder", null, LogType.Both);
            Logger.Error(LogModule.LolSettings, "Please check the logs file for more information", null, LogType.Both);
            Logger.Error(LogModule.LolSettings, "Error : ", ex, LogType.Both);
            Logger.Error(LogModule.LolSettings, $"Pres any key to back to the 'Lol Settings' menu...", ex, LogType.Console);
            Console.ReadKey();
        }

        ResetConsole();
    }

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
}
