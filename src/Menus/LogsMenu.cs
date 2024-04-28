﻿using Loly.src.Variables;
using Loly.src.Variables.Class;

using static Loly.src.Menus.Core.Interface;
using static Loly.src.Tools.Utils;

namespace Loly.src.Menus;

internal static class LogsMenu
{
    internal static void GetLogsMenu()
    {
        UpdateMenuTitle("logs");
        Console.Clear();
        ShowLogsArt();

        DisplayColor($"`{DateTime.Now:[hh:mm:ss]}»` Start Logs System", Colors.InfoColor, Colors.PrimaryColor);
        DisplayColor($"`{DateTime.Now:[hh:mm:ss]}»` All actions will be displayed in real time", Colors.InfoColor, Colors.PrimaryColor);
        DisplayColor($"`{DateTime.Now:[hh:mm:ss]}»` Press any key to leave logs system...", Colors.InfoColor, Colors.PrimaryColor);

        Global.LogsMenuEnable = true;

        Console.Write(Environment.NewLine);
        Console.ReadKey();

        Global.LogsMenuEnable = false;
        ResetConsole();
    }
}