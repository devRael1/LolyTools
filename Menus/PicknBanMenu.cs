using Alba.CsConsoleFormat;
using Loly.LeagueClient;
using Loly.Menus.Core;
using Loly.Variables;
using Newtonsoft.Json;
using static Loly.Tools.Utils;
using static Loly.Menus.ToolsMenu;
using Console = Colorful.Console;

namespace Loly.Menus;

public class PicknBanMenu
{
    private static string _role;

    public static void GetPicknBanMenu()
    {
        while (true)
        {
            ShowRoleMenu();

            int choice = 10;
            UpdateMenuTitle("pnb");
            string[] choices = { "Default (Blind mode)", "Top", "Jungle", "Mid", "Adc", "Support", "Back" };

            MenuBuilder mainMenu = MenuBuilder.BuildMenu(choices, Console.CursorTop);
            while (choice == 10) choice = mainMenu.RunMenu();

            Console.Clear();
            Interface.ShowArt();

            if (choice == choices.Length) break;

            _role = choice == 1 ? "Default" : choices[choice - 1];
            GetOptionPicknBanMenu();
        }

        GetToolsMenu();
    }

    private static void GetOptionPicknBanMenu()
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

        GetPicknBanMenu();
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

    private static void ShowPicknBanMenu()
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

        InitRole role = (InitRole)Settings.LoLRoles.GetType().GetProperty(_role).GetValue(Settings.LoLRoles);
        if (role.PickChamp.Id != null || role.BanChamp.Id != null)
        {
            Border border2 = new()
            {
                MinWidth = 60,
                MaxWidth = 60,
                Stroke = LineThickness.None,
                Align = Align.Center,
                TextAlign = TextAlign.Justify,
                Color = Colors.MenuPrimaryColor,
                TextWrap = TextWrap.WordWrap
            };

            if (role.PickChamp.Id != null)
                border2.Children.Add(CreateSpan($"Current Pick      - {FormatStr(role.PickChamp.Name)} (Delay: {role.PickChamp.Delay}ms)\n", 7, Colors.MenuTextColor));
            if (role.BanChamp.Id != null)
                border2.Children.Add(CreateSpan($"Current Ban       - {FormatStr(role.BanChamp.Name)} (Delay: {role.BanChamp.Delay}ms)", 7, Colors.MenuTextColor));

            border1.Children.Add(border2);
        }

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
                    InitRole role = (InitRole)Settings.LoLRoles.GetType().GetProperty(_role).GetValue(Settings.LoLRoles);

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

                    Settings.LoLRoles.GetType().GetProperty(_role).SetValue(Settings.LoLRoles, role);
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
                    InitRole role = (InitRole)Settings.LoLRoles.GetType().GetProperty(_role).GetValue(Settings.LoLRoles);

                    if (action == "pick")
                        role.PickChamp.Delay = delay;
                    else
                        role.BanChamp.Delay = delay;

                    Settings.LoLRoles.GetType().GetProperty(_role).SetValue(Settings.LoLRoles, role);
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

    private static void LoadChampionsList()
    {
        if (Global.ChampionsList.Any()) return;

        List<ChampItem> champs = new();

        string[] ownedChamps = Requests.WaitSuccessClientRequest("GET", "lol-champions/v1/inventories/" + Global.CurrentSummonerId + "/champions-minimal", true);
        dynamic champsSplit = JsonConvert.DeserializeObject(ownedChamps[1]);

        foreach (dynamic champ in champsSplit)
        {
            if (champ.id == -1) continue;

            string champName = champ.name;
            string champId = champ.id;
            bool champOwned = champ.ownership.owned;
            bool champFreeXboxPass = champ.ownership.xboxGPReward;
            bool champFree = champ.freeToPlay;

            if (champName == "Nunu & Willump") champName = "Nunu";

            bool isAvailable;
            if (champOwned || champFree || champFreeXboxPass)
                isAvailable = true;
            else
                isAvailable = false;

            champs.Add(new ChampItem { Name = champName, Id = champId, Free = isAvailable });
        }

        foreach (ChampItem champ in champs) Global.ChampionsList.Add(champ);
    }
}