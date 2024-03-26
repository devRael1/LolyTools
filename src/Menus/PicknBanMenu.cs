using Alba.CsConsoleFormat;

using Loly.src.Menus.Core;
using Loly.src.Tools;
using Loly.src.Variables;
using Loly.src.Variables.Class;

using Newtonsoft.Json;

using static Loly.src.Menus.Core.Interface;
using static Loly.src.Menus.ToolsMenu;
using static Loly.src.Tools.Utils;
using static Loly.src.Variables.Global;

namespace Loly.src.Menus;

public class PicknBanMenu
{
    private static string _role;
    private static InitRole _cachedRole;

    public static void GetPicknBanMenu()
    {
        while (true)
        {
            ShowRoleMenu();

            var choice = 10;
            UpdateMenuTitle("pnb");
            string[] choices = { "Default (Blind mode)", "Top", "Jungle", "Mid", "Adc", "Support", "Back" };

            var pickNBanMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop);
            while (choice == 10) choice = pickNBanMenu.RunMenu();
            ResetConsole();

            if (choice == choices.Length) break;

            _role = choice == 1 ? "Default" : choices[choice - 1];
            _cachedRole = (InitRole)Settings.LoLRoles.GetType().GetProperty(_role).GetValue(Settings.LoLRoles);
            GetChoicePickorBanMenu();
        }

        GetToolsMenu();
    }

    private static void GetChoicePickorBanMenu()
    {
        while (true)
        {
            ShowPickorBanMenu();

            var choice = 7;
            UpdateMenuTitle("pnb_pob");
            string[] choices = { "Pick Options", "Ban Options", "Back" };

            var pickOrBanMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop);
            while (choice == 7) choice = pickOrBanMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Length) break;

            GetOptionPicknBanMenu(choice == 1);
        }

        GetPicknBanMenu();
    }

    private static void GetOptionPicknBanMenu(bool pick)
    {
        while (true)
        {
            ShowPicknBanMenu(pick);

            var choice = 7;
            UpdateMenuTitle(pick ? "pnb_pick" : "pnb_ban");
            var choices = pick ? new[] { "Pick Champion", "Remove Champion", "Pick Delay", "Back" } : new[] { "Ban Champion", "Remove Champion", "Ban Delay", "Back" };

            var pickNBanMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop);
            while (choice == 7) choice = pickNBanMenu.RunMenu();

            ResetConsole();

            if (choice == choices.Length) break;

            switch (choice)
            {
                case 1:
                    LoadChampionsList();

                    if (ChampionsList.Any())
                    {
                        AskChampionName(pick);
                    }
                    else
                    {
                        ResetConsole();

                        DisplayColor("[WARNING]» To configure pick / ban champion, you need to open 'League of Legends' game.", Colors.WarningColor, Colors.PrimaryColor);
                        DisplayColor("[WARNING]» Please start your 'League of Legends' game and try again...", Colors.WarningColor, Colors.PrimaryColor);
                        DisplayColor("[WARNING]» Press any key to continue...", Colors.WarningColor, Colors.PrimaryColor);
                        Console.ReadKey();
                    }

                    break;
                case 2:
                    ChampItem champ = pick ? _cachedRole.PickChamp : _cachedRole.BanChamp;
                    if (champ.Id == null)
                    {
                        ResetConsole();

                        DisplayColor("[WARNING]» You can't remove the champion because it is not configured.", Colors.WarningColor, Colors.PrimaryColor);
                        DisplayColor("[WARNING]» Press any key to continue...", Colors.WarningColor, Colors.PrimaryColor);
                        Console.ReadKey();
                        ResetConsole();
                    }
                    else
                    {
                        ConfirmRemoveChampion(pick);
                    }

                    break;
                default:
                    AskDelay(pick);
                    break;
            }
        }

        GetChoicePickorBanMenu();
    }

    private static void ShowPickorBanMenu()
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
                CreateSpan("Pick or Ban\nRole: ", 0, Colors.MenuTextColor),
                CreateSpan(_role, 0, Colors.MenuPrimaryColor),
                new Separator(),
                CreateSpan("Choose pick or ban to configure champion and delay\n", 0, Colors.MenuTextColor)
            }
        };

        Border border2 = new()
        {
            MinWidth = 60,
            MaxWidth = 60,
            Stroke = LineThickness.None,
            Align = Align.Center,
            TextAlign = TextAlign.Justify,
            Color = Colors.MenuPrimaryColor,
            TextWrap = TextWrap.WordWrap,
            Children =
            {
                CreateSpan("Current Pick    - ", 8, Colors.MenuTextColor),
                CreateSpan($"{FormatStr(_cachedRole.PickChamp.Name ?? "None")} (Delay: {_cachedRole.PickChamp.Delay}ms)\n", 0, Colors.MenuPrimaryColor),
                CreateSpan("Current Ban     - ", 8, Colors.MenuTextColor),
                CreateSpan($"{FormatStr(_cachedRole.BanChamp.Name ?? "None")} (Delay: {_cachedRole.BanChamp.Delay}ms)\n", 0, Colors.MenuPrimaryColor)
            }
        };

        border1.Children.Add(border2);

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
                CreateSpan("Choose role to configure Pick and Ban.", 0, Colors.MenuTextColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void ShowPicknBanMenu(bool pick)
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
                CreateSpan("Pick and Ban\n", 0, Colors.MenuTextColor),
                CreateSpan(_role, 0, Colors.MenuTextColor),
                new Separator(),
                CreateSpan("Configure which champion to auto pick ban and delay\n", 0, Colors.MenuTextColor)
            }
        };

        var action = pick ? "Pick" : "Ban";
        ChampItem selected = pick ? _cachedRole.PickChamp : _cachedRole.BanChamp;

        Border border2 = new()
        {
            MinWidth = 60,
            MaxWidth = 60,
            Stroke = LineThickness.None,
            Align = Align.Center,
            TextAlign = TextAlign.Justify,
            Color = Colors.MenuPrimaryColor,
            TextWrap = TextWrap.WordWrap,
            Children =
            {
                CreateSpan($"Current {action}      - ", 8, Colors.MenuTextColor),
                CreateSpan($"{FormatStr(selected.Name ?? "None")} (Delay: {selected.Delay}ms)\n", 0, Colors.MenuPrimaryColor)
            }
        };

        border1.Children.Add(border2);
        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void ShowConfirmMenu(bool pick)
    {
        Console.SetCursorPosition(0, TopLength);

        var action = pick ? "Pick" : "Ban";

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
                CreateSpan($"Confirm if you want to delete the champion for '{action}'\n\n", 0, Colors.MenuTextColor),
                CreateSpan("Champion     - ", 0, Colors.MenuTextColor),
                CreateSpan(pick ? FormatStr(_cachedRole.PickChamp.Name) : FormatStr(_cachedRole.BanChamp.Name), 0, Colors.MenuPrimaryColor)
            }
        };

        rectangle.Children.Add(border1);
        ConsoleRenderer.RenderDocument(rectangle);
    }

    private static void AskChampionName(bool pick)
    {
        MenuBuilder.SetCursorVisibility(true);
        UpdateMenuTitle(pick ? "pnb_pick_c" : "pnb_ban_c");

        var action = pick ? "pick" : "ban";
        var champName = "";
        while (champName == "")
        {
            DisplayColor($"`{DateTime.Now:[hh:mm:ss]}`» Enter name of champion you want to auto '{action}':", Colors.InfoColor, Colors.PrimaryColor);
            Console.Write("» ");

            try
            {
                champName = Console.ReadLine().ToLower();

                if (champName == "")
                {
                    DisplayColor("[WARNING]» Champion name cannot be empty !", Colors.WarningColor, Colors.PrimaryColor);
                    continue;
                }

                ChampItem champ = ChampionsList.Find(x => x.Name.ToLower() == champName);
                if (champ == null)
                {
                    DisplayColor($"[WARNING]» Champion \"{FormatStr(champName)}\" does not exist !", Colors.WarningColor, Colors.PrimaryColor);
                    champName = "";
                }
                else if (!champ.Free && action != "ban")
                {
                    DisplayColor($"[WARNING]» You can't select the '{FormatStr(champName)}' champion because you don't own it.", Colors.WarningColor, Colors.PrimaryColor);
                    DisplayColor("[WARNING]» Please buy the champion, restart the software and try again !", Colors.WarningColor, Colors.PrimaryColor);
                    champName = "";
                }
                else
                {
                    var role = (InitRole)Settings.LoLRoles.GetType().GetProperty(_role).GetValue(Settings.LoLRoles);

                    if (action == "pick")
                    {
                        role.PickChamp.Name = champ.Name;
                        role.PickChamp.Id = champ.Id;
                        role.PickChamp.Free = champ.Free;
                    }
                    else
                    {
                        role.BanChamp.Name = champ.Name;
                        role.BanChamp.Id = champ.Id;
                        role.BanChamp.Free = champ.Free;
                    }

                    Settings.LoLRoles.GetType().GetProperty(_role).SetValue(Settings.LoLRoles, _cachedRole);
                    Settings.SaveSettings();
                    _cachedRole = role;

                    Console.Write(Environment.NewLine);

                    DisplayColor($"[SUCCESS]» Champion '{FormatStr(champName)}' selected successfully for {action} !", Colors.SuccessColor, Colors.PrimaryColor);
                    DisplayColor("[SUCCESS]» Press any key to continue...", Colors.SuccessColor, Colors.PrimaryColor);
                    Console.ReadKey();
                    ResetConsole();
                }
            }
            catch
            {
                DisplayColor($"[WARNING]» Champion \"{FormatStr(champName)}\" does not exist !", Colors.WarningColor, Colors.PrimaryColor);
                champName = "";
            }
        }
    }

    private static void AskDelay(bool pick)
    {
        MenuBuilder.SetCursorVisibility(true);
        UpdateMenuTitle(pick ? "pnb_pick_delay" : "pnb_ban_delay");

        var action = pick ? "pick" : "ban";
        var input = "";
        var delay = 0;
        var minDelay = pick ? 1000 : 1500;
        var maxDelay = pick ? 20000 : 15000;

        while (delay == 0 || delay < minDelay || delay > maxDelay)
        {
            DisplayColor($"`{DateTime.Now:[hh:mm:ss]}`» Enter the delay to auto '{action}' champion (recommanded {minDelay}-{maxDelay}ms):", Colors.InfoColor, Colors.PrimaryColor);
            DisplayColor($"`{DateTime.Now:[hh:mm:ss]}`» Enter the delay in miliseconds (ex: 1500 = 1,5sec, 4000 = 4sec...)", Colors.InfoColor, Colors.PrimaryColor);
            Console.Write("» ");

            try
            {
                input = Console.ReadLine();
                delay = Convert.ToInt32(input);

                if (delay == 0 || delay < minDelay || delay > maxDelay)
                {
                    DisplayColor($"[WARNING]» Delay must be between {minDelay}ms and {maxDelay}ms !", Colors.WarningColor, Colors.PrimaryColor);
                }
                else
                {
                    if (action == "pick") _cachedRole.PickChamp.Delay = delay;
                    else _cachedRole.BanChamp.Delay = delay;

                    Settings.LoLRoles.GetType().GetProperty(_role).SetValue(Settings.LoLRoles, _cachedRole);
                    Settings.SaveSettings();

                    Console.Write(Environment.NewLine);
                    DisplayColor($"[SUCCESS]» The {delay}ms delay has been configured correctly for auto {action} !", Colors.SuccessColor, Colors.PrimaryColor);
                    DisplayColor("[SUCCESS]» Press any key to continue...", Colors.SuccessColor, Colors.PrimaryColor);

                    Console.ReadKey();
                    ResetConsole();
                }
            }
            catch
            {
                DisplayColor($"[WARNING]» Unable to convert '{input}' to delay !", Colors.WarningColor, Colors.PrimaryColor);
                DisplayColor("[WARNING]» Please try again... ", Colors.WarningColor, Colors.PrimaryColor);
                input = "";
                delay = 0;
            }
        }
    }

    private static void ConfirmRemoveChampion(bool pick)
    {
        UpdateMenuTitle(pick ? "pnb_pick_del_c" : "pnb_ban_del_c");
        ResetConsole();
        ShowConfirmMenu(pick);

        var choice = 5;
        string[] choices = { "Yes", "No" };

        var removeChampMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop);
        while (choice == 5) choice = removeChampMenu.RunMenu();

        if (choice == 1)
        {
            var role = (InitRole)Settings.LoLRoles.GetType().GetProperty(_role).GetValue(Settings.LoLRoles);
            var cacheName = pick ? role.PickChamp.Name : role.BanChamp.Name;
            var action = pick ? "pick" : "ban";

            if (pick)
            {
                role.PickChamp.Name = null;
                role.PickChamp.Id = null;
                role.PickChamp.Free = false;
            }
            else
            {
                role.BanChamp.Name = null;
                role.BanChamp.Id = null;
                role.BanChamp.Free = false;
            }

            Settings.LoLRoles.GetType().GetProperty(_role).SetValue(Settings.LoLRoles, role);
            Settings.SaveSettings();
            _cachedRole = role;
            ResetConsole();

            DisplayColor($"[SUCCESS]» Champion '{FormatStr(cacheName)}' removed successfully for {action} !", Colors.SuccessColor, Colors.PrimaryColor);
            DisplayColor("[SUCCESS]» Press any key to continue...", Colors.SuccessColor, Colors.PrimaryColor);

            Console.ReadKey();
        }

        ResetConsole();
        GetOptionPicknBanMenu(pick);
    }

    private static void LoadChampionsList()
    {
        List<ChampItem> champs = new();

        var ownedChamps = Requests.WaitSuccessClientRequest("GET", "lol-champions/v1/inventories/" + SummonerLogged.SummonerId + "/champions-minimal", true);
        dynamic champsSplit = JsonConvert.DeserializeObject(ownedChamps[1]);
        if (champsSplit == null) return;

        foreach (dynamic champ in champsSplit)
        {
            if (champ.id == -1) continue;

            string champName = champ.name;
            string champId = champ.id;
            bool champOwned = champ.ownership.owned;
            bool champFreeXboxPass = champ.ownership.xboxGPReward;
            bool champFree = champ.freeToPlay;

            if (champName == "Nunu & Willump") champName = "Nunu";

            var isAvailable = champOwned || champFree || champFreeXboxPass;
            champs.Add(new ChampItem { Name = champName, Id = champId, Free = isAvailable });
        }

        foreach (ChampItem champ in champs) ChampionsList.Add(champ);
    }
}