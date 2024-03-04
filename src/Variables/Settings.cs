using Loly.src.Logs;
using Loly.src.Tools;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loly.src.Variables;

public class Settings
{
    private const string FileName = "Loly Settings.json";

    public static readonly PnBRoles LoLRoles = new();
    public static readonly List<string> ChatMessages = new();

    public static bool EnableAutoUpdate { get; set; }
    public static int ClearLogsFilesDays { get; set; }
    public static bool LobbyRevealer { get; set; }
    public static bool AutoAccept { get; set; }
    public static bool AutoAcceptOnce { get; set; }
    public static bool PicknBan { get; set; }
    public static bool AutoChat { get; set; }

    private static JObject GetJObj()
    {
        JObject tools = new()
        {
            { nameof(LobbyRevealer), LobbyRevealer },
            { nameof(AutoAccept), AutoAccept },
            { nameof(PicknBan), PicknBan },
            { nameof(AutoChat), AutoChat }
        };
        JObject autoaccept = new()
        {
            { nameof(AutoAcceptOnce), AutoAcceptOnce }
        };
        JObject autochat = new()
        {
            { nameof(ChatMessages), JArray.FromObject(ChatMessages) }
        };
        JObject picknban = new()
        {
            { nameof(LoLRoles.Default), JObject.FromObject(LoLRoles.Default) },
            { nameof(LoLRoles.Top), JObject.FromObject(LoLRoles.Top) },
            { nameof(LoLRoles.Jungle), JObject.FromObject(LoLRoles.Jungle) },
            { nameof(LoLRoles.Mid), JObject.FromObject(LoLRoles.Mid) },
            { nameof(LoLRoles.Adc), JObject.FromObject(LoLRoles.Adc) },
            { nameof(LoLRoles.Support), JObject.FromObject(LoLRoles.Support) }
        };

        return new JObject
        {
            { nameof(EnableAutoUpdate), EnableAutoUpdate },
            { nameof(ClearLogsFilesDays), ClearLogsFilesDays },
            { nameof(Tools), tools },
            { nameof(AutoAccept), autoaccept },
            { nameof(AutoChat), autochat },
            { nameof(PicknBan), picknban }
        };
    }

    private static void PrettyWrite(JToken obj)
    {
        using StreamWriter file = File.CreateText(FileName);
        JsonTextWriter jsonWriter = new(file)
        {
            Formatting = Formatting.Indented
        };
        obj.WriteTo(jsonWriter);
    }

    private static object LoadJson()
    {
        using StreamReader file = File.OpenText(FileName);
        string json = file.ReadToEnd();
        object items = JsonConvert.DeserializeObject(json);
        return items;
    }

    public static void CreateOrUpdateSettings()
    {
        if (!File.Exists(FileName))
        {
            JObject obj = GetJObj();
            PrettyWrite(obj);
        }
        else
        {
            JObject obj = JObject.Parse(LoadJson().ToString() ?? throw new InvalidOperationException());
            EnableAutoUpdate = (bool)obj[nameof(EnableAutoUpdate)];
            ClearLogsFilesDays = (int)obj[nameof(ClearLogsFilesDays)];
            LobbyRevealer = (bool)obj[nameof(Tools)][nameof(LobbyRevealer)];
            AutoAccept = (bool)obj[nameof(Tools)][nameof(AutoAccept)];
            AutoAcceptOnce = (bool)obj[nameof(AutoAccept)][nameof(AutoAcceptOnce)];
            PicknBan = (bool)obj[nameof(Tools)][nameof(PicknBan)];
            AutoChat = (bool)obj[nameof(Tools)][nameof(AutoChat)];

            List<string> allMessages = obj[nameof(AutoChat)][nameof(ChatMessages)].ToObject<List<string>>();
            ChatMessages.AddRange(allMessages);

            LoLRoles.Default = obj[nameof(PicknBan)][nameof(LoLRoles.Default)].ToObject<InitRole>();
            LoLRoles.Top = obj[nameof(PicknBan)][nameof(LoLRoles.Top)].ToObject<InitRole>();
            LoLRoles.Jungle = obj[nameof(PicknBan)][nameof(LoLRoles.Jungle)].ToObject<InitRole>();
            LoLRoles.Mid = obj[nameof(PicknBan)][nameof(LoLRoles.Mid)].ToObject<InitRole>();
            LoLRoles.Adc = obj[nameof(PicknBan)][nameof(LoLRoles.Adc)].ToObject<InitRole>();
            LoLRoles.Support = obj[nameof(PicknBan)][nameof(LoLRoles.Support)].ToObject<InitRole>();
        }
    }

    public static void SetDefaultSettings()
    {
        EnableAutoUpdate = true;
        ClearLogsFilesDays = 7;
        LobbyRevealer = false;
        AutoAccept = false;
        AutoAcceptOnce = false;
        AutoChat = false;
        PicknBan = false;
    }

    public static JObject GetSettings()
    {
        JObject obj = GetJObj();
        return obj;
    }

    public static void SaveSettings()
    {
        try
        {
            JObject settings = GetJObj();
            PrettyWrite(settings);
        }
        catch (Exception ex)
        {
            Logger.Error(LogModule.Loly, "Error while saving settings...", null, LogType.Console);
            Utils.LogNewError("Save Settings", LogModule.Loly, ex);
            Logger.Error(LogModule.Loly, "Press Enter to close application...", null, LogType.Console);
            Console.ReadKey();
            Environment.Exit(0);
        }
    }

    public static void SaveFileSettings(JObject settings)
    {
        {
            try
            {
                PrettyWrite(settings);
            }
            catch (Exception ex)
            {
                Logger.Error(LogModule.Loly, "Error while saving file settings...", null, LogType.Console);
                Utils.LogNewError("Save File Settings", LogModule.Loly, ex);
                Logger.Error(LogModule.Loly, "Press Enter to close application...", null, LogType.Console);
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}