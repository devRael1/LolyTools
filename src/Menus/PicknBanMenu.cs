using System.Data;

using Alba.CsConsoleFormat;

using Loly.src.Menus.Core;
using Loly.src.Tools;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;

using static Loly.src.Menus.Core.Interface;
using static Loly.src.Tools.PicknBan;
using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Menus;

public class PicknBanMenu
{
    private static Role CachedRole { get; set; }
    private static Dictionary<ActionType, List<ChampItem>> PickBanChampions { get; set; } = new();
    private static ActionType ActionType { get; set; }

    #region Get Menus

    public static void GetPicknBanMenu()
    {
        while (true)
        {
            UpdateMenuTitle("pnb");
            ShowRoleMenu();

            string[] choices = { "Default (Blind mode)", "Top", "Jungle", "Mid", "ADC", "Support", "Back" };
            var pickNBanMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop);
            var choice = 0;
            while (choice == 0) choice = pickNBanMenu.RunMenu();
            ResetConsole();

            if (choice == choices.Length) break;

            var role = choice == 1 ? "Default" : choices[choice - 1];
            var _rolePicknBan = (RolePicknBan)CurrentSettings.PicknBan.GetType().GetProperty(role).GetValue(CurrentSettings.PicknBan);
            CachedRole = Enum.Parse<Role>(role);
            PickBanChampions.Clear();
            PickBanChampions.Add(ActionType.Pick, _rolePicknBan.Picks);
            PickBanChampions.Add(ActionType.Ban, _rolePicknBan.Bans);

            GetChoicePickorBanMenu();
        }
    }

    private static void GetChoicePickorBanMenu()
    {
        while (true)
        {
            UpdateMenuTitle("pnb_pob");
            ShowPickorBanMenu();

            string[] choices = { "Pick(s) Options", "Ban(s) Options", "Back" };
            var pickOrBanMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop);
            var choice = 0;
            while (choice == 0) choice = pickOrBanMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Length) break;

            ActionType = choice == 1 ? ActionType.Pick : ActionType.Ban;
            GetOptionPicknBanMenu();
        }
    }

    private static void GetOptionPicknBanMenu()
    {
        while (true)
        {
            UpdateMenuTitle(ActionType == ActionType.Pick ? "pnb_pick" : "pnb_ban");
            ShowPicknBanMenu();

            var choices = ActionType == ActionType.Pick
                ? new[] { "Pick Champion (Add)", "Pick Champion (Remove)", "Pick Delay", "Pick List", "Back" }
                : new[] { "Ban Champion (Add)", "Ban Champion (Remove)", "Ban Delay", "Ban List", "Back" };
            var pickNBanMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop + 1);
            var choice = 0;
            while (choice == 0) choice = pickNBanMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Length) break;

            switch (choice)
            {
                case 1:
                    LoadChampionsList();

                    if (!ChampionsList.Any())
                    {
                        ResetConsole();

                        DisplayColor("`[WARNING]»` To configure pick / ban champion, you need to open 'League of Legends' game", Colors.InfoColor, Colors.WarningColor);
                        DisplayColor("`[WARNING]»` Please start your 'League of Legends' game and try again...", Colors.InfoColor, Colors.WarningColor);
                        DisplayColor("`[WARNING]»` Press any key to continue...", Colors.InfoColor, Colors.WarningColor);
                        Console.ReadKey();
                        break;
                    }

                    GetAddChampionByFirstCharacterMenu();
                    break;
                case 2:
                    if (PickBanChampions.GetValueOrDefault(ActionType).Count == 0)
                    {
                        DisplayColor($"`[WARNING]»` You can't remove a champion because there aren't any configured for '{actionType}' (Role : {CachedRole})", Colors.InfoColor, Colors.WarningColor);
                        DisplayColor("`[WARNING]»` You must add at least 1 champion before removing one", Colors.InfoColor, Colors.WarningColor);
                        DisplayColor("`[WARNING]»` Press any key to continue...", Colors.InfoColor, Colors.WarningColor);
                        Console.ReadKey();
                        ResetConsole();
                        break;
                    }

                    GetRemoveChampionMenu();
                    break;
                case 3:
                    if (PickBanChampions.GetValueOrDefault(ActionType).Count == 0)
                    {
                        DisplayColor($"`[WARNING]»` You can't configure delay because there aren't any champion(s) configured for '{actionType}' (Role : {CachedRole})", Colors.InfoColor, Colors.WarningColor);
                        DisplayColor("`[WARNING]»` You must add at least 1 champion before configuring delay", Colors.InfoColor, Colors.WarningColor);
                        DisplayColor("`[WARNING]»` Press any key to continue...", Colors.InfoColor, Colors.WarningColor);
                        Console.ReadKey();
                        ResetConsole();
                        break;
                    }

                    GetAskDelayMenu();
                    break;
                case 4:
                    if (PickBanChampions.GetValueOrDefault(ActionType).Count == 0)
                    {
                        DisplayColor($"`[WARNING]»` You can't show the list because there aren't any champion(s) configured for '{actionType}' (Role : {CachedRole})", Colors.InfoColor, Colors.WarningColor);
                        DisplayColor("`[WARNING]»` You must add at least 1 champion before showing the list", Colors.InfoColor, Colors.WarningColor);
                        DisplayColor("`[WARNING]»` Press any key to continue...", Colors.InfoColor, Colors.WarningColor);
                        Console.ReadKey();
                        ResetConsole();
                        break;
                    }

                    GetListChampionMenu();
                    break;
            }
        }
    }

    private static void GetAddChampionByFirstCharacterMenu()
    {
        UpdateMenuTitle(ActionType == ActionType.Pick ? "pnb_pick_c" : "pnb_ban_c");
        MenuBuilder.SetCursorVisibility(true);

        var role = (RolePicknBan)CurrentSettings.PicknBan.GetType().GetProperty(CachedRole.ToString()).GetValue(CurrentSettings.PicknBan);
        var character = "";
        var action = ActionType == ActionType.Pick ? "PICK" : "BAN";

        DisplayColor($"`{DateTime.Now:[hh:mm:ss]}»` Enter the `FIRST` letter of champion name you want to add in AUTO-{action} (Ex: Yone = Y, Annie = A...) :", Colors.InfoColor, Colors.PrimaryColor);
        if (ActionType == ActionType.Pick)
        {
            DisplayColor($"`{DateTime.Now:[hh:mm:ss]}»` To AUTO-PICK a champion, he must meet the following `conditions` :", Colors.InfoColor, Colors.PrimaryColor);
            DisplayColor($" - Free rotation", Colors.InfoColor, Colors.PrimaryColor);
            DisplayColor($" - Champion gifted", Colors.InfoColor, Colors.PrimaryColor);
            DisplayColor($" - Champion owned (buyed)", Colors.InfoColor, Colors.PrimaryColor);
            DisplayColor($" - Champion not already added", Colors.InfoColor, Colors.PrimaryColor);
        }

        while (character == "")
        {
            Console.Write("» ");

            character = Console.ReadLine()?.Trim().ToLower();
            if (character.Length != 1)
            {
                character = "";
                DisplayColor("`[WARNING]»` You must enter only `ONE` character", Colors.InfoColor, Colors.WarningColor);
                DisplayColor("`[WARNING]»` Please try again...", Colors.InfoColor, Colors.WarningColor);
                continue;
            }

            if (!char.IsLetter(character[0]))
            {
                character = "";
                DisplayColor("`[WARNING]»` You must enter a `LETTER` character like 'e' / 'a' / 'y'...", Colors.InfoColor, Colors.WarningColor);
                DisplayColor("`[WARNING]»` Please try again...", Colors.InfoColor, Colors.WarningColor);
                continue;
            }

            if (ActionType == ActionType.Pick && !ChampionsList.Any(x => x.Name.ToLower().StartsWith(character) && x.Usable && !role.Picks.Any(r => r.Id == x.Id)))
            {
                DisplayColor($"`[WARNING]»` No champion usable found with the letter : '{character}'", Colors.InfoColor, Colors.WarningColor);
                DisplayColor("`[WARNING]»` Please try again with an another letter...", Colors.InfoColor, Colors.WarningColor);
                character = "";
                continue;
            }
            break;
        }

        ResetConsole();
        DisplayColor($"`{DateTime.Now:[hh:mm:ss]}»` Select the champion you want to AUTO-{action} :", Colors.InfoColor, Colors.PrimaryColor);

        List<ChampItem> champions = ActionType == ActionType.Pick
            ? ChampionsList.Where(x => x.Name.ToLower().StartsWith(character) && x.Usable && !role.Picks.Any(r => r.Id == x.Id)).ToList()
            : ChampionsList.Where(x => x.Name.ToLower().StartsWith(character) && !role.Bans.Any(r => r.Id == x.Id)).ToList();
        var choices = champions.Select(c => c.Name).ToList();
        choices.Add("[BACK] Cancel");

        var addChampionMenu = MenuBuilder.BuildMenu(choices.ToArray(), Console.CursorTop, true);
        var choice = 0;
        while (choice == 0) choice = addChampionMenu.RunMenu();

        if (choice != choices.Count)
        {
            ChampItem selectedChampion = champions[choice - 1];

            if (ActionType == ActionType.Pick) role.Picks.Add(selectedChampion);
            else role.Bans.Add(selectedChampion);

            CurrentSettings.PicknBan.GetType().GetProperty(CachedRole.ToString()).SetValue(CurrentSettings.PicknBan, role);
            SettingsManager.SaveFileSettings();

            ResetConsole();
            DisplayColor($"`[SUCCESS]»` Champion '{FormatStr(selectedChampion.Name)}' added successfully for AUTO-{action} !", Colors.InfoColor, Colors.SuccessColor);
            DisplayColor("`[SUCCESS]»` Press any key to return to the 'Pick' and 'Ban' option menu...", Colors.InfoColor, Colors.SuccessColor);
            Console.ReadKey();
        }

        ResetConsole();
    }

    private static void GetAskDelayMenu()
    {
        UpdateMenuTitle(ActionType == ActionType.Pick ? "pnb_pick_delay" : "pnb_ban_delay");

        var action = ActionType == ActionType.Pick ? "PICK" : "BAN";
        var role = (RolePicknBan)CurrentSettings.PicknBan.GetType().GetProperty(CachedRole.ToString()).GetValue(CurrentSettings.PicknBan);

        DisplayColor($"`{DateTime.Now:[hh:mm:ss]}»` Select the champion you want to configure delay for AUTO-{action} :", Colors.InfoColor, Colors.PrimaryColor);
        List<ChampItem> champions = ActionType == ActionType.Pick ? role.Picks : role.Bans;
        var choices = champions.Select(c => c.Name).ToList();
        choices.Add("[BACK] Cancel");

        var addDelayMenu = MenuBuilder.BuildMenu(choices.ToArray(), Console.CursorTop, true);
        var choice = 0;
        while (choice == 0) choice = addDelayMenu.RunMenu();

        if (choice == choices.Count)
        {
            ResetConsole();
            return;
        }

        ChampItem selectedChampion = champions[choice - 1];

        var input = "";
        var delay = 0;
        var minDelay = 1500;
        var maxDelay = 15000;

        ResetConsole();
        MenuBuilder.SetCursorVisibility(true);

        DisplayColor($"`{DateTime.Now:[hh:mm:ss]}»` Enter the delay to {action} '{selectedChampion.Name}' (recommanded {minDelay}-{maxDelay} in ms) :", Colors.InfoColor, Colors.PrimaryColor);
        DisplayColor($"`{DateTime.Now:[hh:mm:ss]}»` The delay is the time between the champion selection and the confirmation in milliseconds", Colors.InfoColor, Colors.PrimaryColor);
        DisplayColor($"`{DateTime.Now:[hh:mm:ss]}»` Enter the delay in miliseconds (ex: 1500 = 1,5sec, 4000 = 4sec...)", Colors.InfoColor, Colors.PrimaryColor);

        while (delay == 0 || delay < minDelay || delay > maxDelay)
        {
            Console.Write("» ");

            try
            {
                input = Console.ReadLine()?.Trim();
                delay = Convert.ToInt32(input);

                if (delay == 0 || delay < minDelay || delay > maxDelay)
                {
                    DisplayColor($"`[WARNING]»` Delay must be between {minDelay}ms and {maxDelay}ms !", Colors.InfoColor, Colors.WarningColor);
                    DisplayColor("`[WARNING]»` Please try again...", Colors.InfoColor, Colors.WarningColor);
                    continue;
                }

                if (ActionType == ActionType.Pick) role.Picks.FirstOrDefault(x => x.Id == selectedChampion.Id).Delay = delay;
                else role.Bans.FirstOrDefault(x => x.Id == selectedChampion.Id).Delay = delay;
                CurrentSettings.PicknBan.GetType().GetProperty(CachedRole.ToString()).SetValue(CurrentSettings.PicknBan, role);
                SettingsManager.SaveFileSettings();

                DisplayColor($"`[SUCCESS]»` Delay '{delay}ms' added successfully for AUTO-{action} '{selectedChampion.Name}' !", Colors.InfoColor, Colors.SuccessColor);
                DisplayColor("`[SUCCESS]»` Press any key to return to the 'Pick' and 'Ban' option menu...", Colors.InfoColor, Colors.SuccessColor);
                Console.ReadKey();
                ResetConsole();
            }
            catch
            {
                DisplayColor($"`[WARNING]»` Unable to convert '{input}' to delay !", Colors.InfoColor, Colors.WarningColor);
                DisplayColor("`[WARNING]»` Please try again... ", Colors.InfoColor, Colors.WarningColor);
                delay = 0;
                continue;
            }
        }
    }

    private static void GetRemoveChampionMenu()
    {
        UpdateMenuTitle(ActionType == ActionType.Pick ? "pnb_pick_del_c" : "pnb_ban_del_c");

        var action = ActionType == ActionType.Pick ? "PICK" : "BAN";
        var role = (RolePicknBan)CurrentSettings.PicknBan.GetType().GetProperty(CachedRole.ToString()).GetValue(CurrentSettings.PicknBan);

        DisplayColor($"`{DateTime.Now:[hh:mm:ss]}»` Select the champion you want to remove for AUTO-{action} :", Colors.InfoColor, Colors.PrimaryColor);
        List<ChampItem> champions = ActionType == ActionType.Pick ? role.Picks : role.Bans;
        var choices = champions.Select(c => c.Name).ToList();
        choices.Add("[BACK] Cancel");

        var addDelayMenu = MenuBuilder.BuildMenu(choices.ToArray(), Console.CursorTop, true);
        var choice = 0;
        while (choice == 0) choice = addDelayMenu.RunMenu();

        if (choice == choices.Count)
        {
            ResetConsole();
            return;
        }

        ChampItem selectedChampion = champions[choice - 1];

        ResetConsole();
        ShowConfirmMenu(selectedChampion);

        string[] yesOrNo = { "Yes", "No" };
        var removeChampMenu = MenuBuilder.BuildMenu(yesOrNo, Console.CursorTop);
        choice = 0;
        while (choice == 0) choice = removeChampMenu.RunMenu();

        if (choice != yesOrNo.Length)
        {
            if (ActionType == ActionType.Pick) role.Picks.Remove(selectedChampion);
            else role.Bans.Remove(selectedChampion);

            CurrentSettings.PicknBan.GetType().GetProperty(CachedRole.ToString()).SetValue(CurrentSettings.PicknBan, role);
            SettingsManager.SaveFileSettings();

            DisplayColor($"`[SUCCESS]»` Champion '{FormatStr(selectedChampion.Name)}' removed successfully for AUTO-{action} !", Colors.InfoColor, Colors.SuccessColor);
            DisplayColor("`[SUCCESS]»` Press any key to continue...", Colors.InfoColor, Colors.SuccessColor);
            Console.ReadKey();
        }

        ResetConsole();
    }

    private static void GetListChampionMenu()
    {
        UpdateMenuTitle(ActionType == ActionType.Pick ? "pnb_pick_list" : "pnb_ban_list");

        while (true)
        {
            ShowListMenu();

            string[] choices = { "Back" };
            var choice = 0;
            var seeMessageMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop + 1);
            while (choice == 0) choice = seeMessageMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Length) break;
        }
    }

    #endregion

    #region Show Menus

    private static void ShowPickorBanMenu()
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
                CreateSpan("Pick or Ban\nRole: ", 0, Colors.MenuTextColor),
                CreateSpan(CachedRole.ToString(), 0, Colors.MenuPrimaryColor),
                new Separator(),
                CreateSpan("Choose 'Pick' or 'Ban' option to manage champion(s) and delay(s)\n", 0, Colors.MenuTextColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void ShowRoleMenu()
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
                CreateSpan("Choose role to configure Pick and Ban", 0, Colors.MenuTextColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void ShowPicknBanMenu()
    {
        Console.SetCursorPosition(0, TopLength);

        var action = ActionType == ActionType.Pick ? "Pick" : "Ban";
        List<ChampItem> selected = ActionType == ActionType.Pick
            ? PickBanChampions.GetValueOrDefault(ActionType.Pick)
            : PickBanChampions.GetValueOrDefault(ActionType.Ban);

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
                CreateSpan("Pick and Ban", 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan($"Manage champion(s) to AUTO-{action} and delay(s) for '{CachedRole}' role\n", 0, Colors.MenuTextColor),
                CreateSpan("For more information, go to the 'Pick List' menu", 0, Colors.MenuTextColor),
                new Separator() { Color = ConsoleColor.Black },
                CreateSpan($"Current {action}      - ", 0, Colors.MenuTextColor),
                CreateSpan($"{selected.Count} champion(s)", 0, Colors.MenuPrimaryColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void ShowConfirmMenu(ChampItem champ)
    {
        Console.SetCursorPosition(0, TopLength);

        var action = ActionType == ActionType.Pick ? "Pick" : "Ban";
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
                CreateSpan("Confirmation", 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan($"Confirm if you want to delete the champion for '", 0, Colors.MenuTextColor),
                CreateSpan(action, 0, Colors.MenuPrimaryColor),
                CreateSpan("'", 0, Colors.MenuTextColor),
                new Separator() { Color = ConsoleColor.Black },
                CreateSpan("Champion     - ", 0, Colors.MenuTextColor),
                CreateSpan(FormatStr(champ.Name), 0, Colors.MenuPrimaryColor),
                CreateSpan(" (Role : ", 0, Colors.MenuTextColor),
                CreateSpan(CachedRole.ToString(), 0, Colors.MenuPrimaryColor),
                CreateSpan(")", 0, Colors.MenuTextColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void ShowListMenu()
    {
        Console.SetCursorPosition(0, TopLength);

        var action = ActionType == ActionType.Pick ? "PICK" : "BAN";
        Document rectangle = new();
        Border border1 = new()
        {
            MinWidth = 90,
            MaxWidth = 90,
            Stroke = LineThickness.None,
            Align = Align.Center,
            TextAlign = TextAlign.Center,
            Color = Colors.MenuPrimaryColor,
            TextWrap = TextWrap.WordWrap,
            Children =
            {
                CreateSpan("Pick and Ban", 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan($"List of all champion(s) to AUTO-{action} for '{CachedRole}' role\n", 0, Colors.MenuTextColor),
                CreateSpan("The order lets the application know which champion to select first\n", 0, Colors.MenuTextColor),
                CreateSpan("If champion is banned or already pick, we try to select the next champ", 0, Colors.MenuTextColor),
            }
        };
        Grid grid = new()
        {
            Stroke = LineThickness.None,
            Align = Align.Center,
            Columns = { GridLength.Char(20), GridLength.Char(30) },
            Color = Colors.MenuPrimaryColor,
            MaxWidth = 50,
            Children =
            {
                new Cell("Order") { Stroke = LineThickness.None, Padding = new Thickness(0, 1), TextAlign = TextAlign.Center },
                new Cell("Champion") { Stroke = LineThickness.None, Padding = new Thickness(0, 1), TextAlign = TextAlign.Justify }
            }
        };

        var count = 0;
        var role = (RolePicknBan)CurrentSettings.PicknBan.GetType().GetProperty(CachedRole.ToString()).GetValue(CurrentSettings.PicknBan);
        List<ChampItem> champions = ActionType == ActionType.Pick ? role.Picks : role.Bans;
        foreach (ChampItem champ in champions)
        {
            count++;
            grid.Children.Add(new Cell($"N°{count}") { Color = Colors.MenuTextColor, Stroke = LineThickness.None, TextAlign = TextAlign.Center });
            grid.Children.Add(new Cell($"{champ.Name} (ID : {champ.Id})") { Color = Colors.MenuTextColor, Stroke = LineThickness.None, TextAlign = TextAlign.Justify });
        }

        border1.Children.Add(grid);
        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    #endregion
}