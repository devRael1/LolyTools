using Loly.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loly.Variables;

public class Settings
{
    private const string FileName = "Loly Settings.json";

    public static readonly ChampItem PickChamp = new();
    public static readonly ChampItem BanChamp = new();
    public static readonly List<string> ChatMessages = new();

    public static bool EnableAutoUpdate { get; set; }
    public static bool LobbyRevealer { get; set; }
    public static bool AutoAccept { get; set; }
    public static bool AutoAcceptOnce { get; set; }
    public static bool PicknBan { get; set; }
    public static bool AutoChat { get; set; }
    public static int PickDelay { get; set; }
    public static int BanDelay { get; set; }

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
        JObject picknban = new()
        {
            { nameof(PickDelay), PickDelay },
            { nameof(BanDelay), BanDelay },
            { nameof(PickChamp), JObject.FromObject(PickChamp) },
            { nameof(BanChamp), JObject.FromObject(BanChamp) }
        };
        JObject autochat = new()
        {
            { nameof(ChatMessages), JArray.FromObject(ChatMessages) }
        };


        return new JObject
        {
            { nameof(EnableAutoUpdate), EnableAutoUpdate },
            { nameof(Tools), tools },
            { nameof(PicknBan), picknban },
            { nameof(AutoAccept), autoaccept },
            { nameof(AutoChat), autochat }
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
            LobbyRevealer = (bool)obj[nameof(Tools)][nameof(LobbyRevealer)];
            AutoAccept = (bool)obj[nameof(Tools)][nameof(AutoAccept)];
            AutoAcceptOnce = (bool)obj[nameof(AutoAccept)][nameof(AutoAcceptOnce)];
            PicknBan = (bool)obj[nameof(Tools)][nameof(PicknBan)];
            AutoChat = (bool)obj[nameof(Tools)][nameof(AutoChat)];

            PickDelay = (int)obj[nameof(PicknBan)][nameof(PickDelay)];
            BanDelay = (int)obj[nameof(PicknBan)][nameof(BanDelay)];

            PickChamp.Id = (string)obj[nameof(PicknBan)][nameof(PickChamp)][nameof(PickChamp.Id)];
            PickChamp.Name = (string)obj[nameof(PicknBan)][nameof(PickChamp)][nameof(PickChamp.Name)];
            PickChamp.Free = (bool)obj[nameof(PicknBan)][nameof(PickChamp)][nameof(PickChamp.Free)];

            BanChamp.Id = (string)obj[nameof(PicknBan)][nameof(BanChamp)][nameof(BanChamp.Id)];
            BanChamp.Name = (string)obj[nameof(PicknBan)][nameof(BanChamp)][nameof(BanChamp.Name)];
            BanChamp.Free = (bool)obj[nameof(PicknBan)][nameof(BanChamp)][nameof(BanChamp.Free)];

            List<string> allMessages = obj[nameof(AutoChat)][nameof(ChatMessages)].ToObject<List<string>>();
            ChatMessages.AddRange(allMessages);
        }
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
            Utils.ShowError(ex);
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
                Utils.ShowError(ex);
            }
        }
    }
}