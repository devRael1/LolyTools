namespace Loly.src.Variables.Class;

public class Settings
{
    public bool EnableAutoUpdate { get; set; }
    public bool EnableAutoSendLogs { get; set; }
    public int ClearLogsFilesDays { get; set; }
    public Tools Tools { get; set; } = new();
    public AutoAccept AutoAccept { get; set; } = new();
    public AutoChat AutoChat { get; set; } = new();
    public PicknBan PicknBan { get; set; } = new();
}

public class Tools
{
    public bool LobbyRevealer { get; set; }
    public bool AutoAccept { get; set; }
    public bool PicknBan { get; set; }
    public bool AutoChat { get; set; }
}

public class AutoAccept
{
    public bool AutoAcceptOnce { get; set; }
}

public class AutoChat
{
    public List<string> ChatMessages { get; set; } = new();
}

public class PicknBan
{
    public InitRole Default { get; set; } = new();
    public InitRole Top { get; set; } = new();
    public InitRole Jungle { get; set; } = new();
    public InitRole Mid { get; set; } = new();
    public InitRole Adc { get; set; } = new();
    public InitRole Support { get; set; } = new();
}