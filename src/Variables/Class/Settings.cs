namespace Loly.src.Variables.Class;

internal class Settings
{
    internal bool EnableAutoUpdate { get; set; }
    internal bool EnableAutoSendLogs { get; set; }
    internal int ClearLogsFilesDays { get; set; }
    internal Tools Tools { get; set; } = new();
    internal AutoAccept AutoAccept { get; set; } = new();
    internal AutoChat AutoChat { get; set; } = new();
    internal PicknBan PicknBan { get; set; } = new();
}

internal class Tools
{
    internal bool LobbyRevealer { get; set; }
    internal bool AutoAccept { get; set; }
    internal bool PicknBan { get; set; }
    internal bool AutoChat { get; set; }
}

internal class AutoAccept
{
    internal bool AutoAcceptOnce { get; set; }
}

internal class AutoChat
{
    internal List<string> ChatMessages { get; set; } = new();
}

internal class PicknBan
{
    internal RolePicknBan Default { get; set; } = new();
    internal RolePicknBan Top { get; set; } = new();
    internal RolePicknBan Jungle { get; set; } = new();
    internal RolePicknBan Mid { get; set; } = new();
    internal RolePicknBan ADC { get; set; } = new();
    internal RolePicknBan Support { get; set; } = new();
}

internal class RolePicknBan
{
    internal List<ChampItem> Picks { get; set; } = new();
    internal List<ChampItem> Bans { get; set; } = new();
}