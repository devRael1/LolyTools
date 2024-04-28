using Loly.src.Variables;
using Loly.src.Variables.Class;

using Newtonsoft.Json;

namespace Loly.src.Tools;

internal static class SettingsManager
{
    private const string SettingsFile = "LolySettings.json";

    private static Settings LoadJson()
    {
        using StreamReader file = File.OpenText(SettingsFile);
        var json = file.ReadToEnd();
        Settings items = JsonConvert.DeserializeObject<Settings>(json);
        return items;
    }

    internal static void CreateOrUpdateSettings()
    {
        if (!File.Exists(SettingsFile))
        {
            SetDefaultSettings();
            SaveFileSettings();
        }
        else
        {
            Settings settings = LoadJson();
            Global.CurrentSettings = settings;
        }
    }

    internal static void SetDefaultSettings()
    {
        Global.CurrentSettings.EnableAutoUpdate = true;
        Global.CurrentSettings.EnableAutoSendLogs = true;
        Global.CurrentSettings.ClearLogsFilesDays = 5;
        Global.CurrentSettings.Tools.LobbyRevealer = false;
        Global.CurrentSettings.Tools.AutoAccept = false;
        Global.CurrentSettings.AutoAccept.AutoAcceptOnce = false;
        Global.CurrentSettings.Tools.AutoChat = false;
        Global.CurrentSettings.Tools.PicknBan = false;
    }

    internal static void SaveFileSettings()
    {
        var json = JsonConvert.SerializeObject(Global.CurrentSettings, Formatting.Indented);
        File.WriteAllText(SettingsFile, json);
    }
}