using Newtonsoft.Json;

namespace Loly.src.Variables.Class;

public class Player
{
    public Player(string username, string tag, string link)
    {
        Username = username;
        Tag = tag;
        Link = link;

        Id = 0;
        Level = 0;
        UserTag = $"{username}#{tag}";
        UserTagUrlReady = $"{username}-{tag}";
        SoloDuoQ = new QueueStats();
        FlexQ = new QueueStats();
    }

    public string Username { get; set; }
    public string Tag { get; set; }
    public string UserTag { get; set; }
    public string UserTagUrlReady { get; set; }
    public string Link { get; set; }
    public int? Id { get; set; }
    public int? Level { get; set; }
    public QueueStats SoloDuoQ { get; }
    public QueueStats FlexQ { get; set; }
}

public class QueueStats
{
    public QueueStats()
    {
        Wins = 0;
        Losses = 0;
        Lp = 0;
        Division = 0;
        Tier = "Unranked";
    }

    public int? Wins { get; set; }
    public int? Losses { get; set; }
    public int? Lp { get; set; }
    public int? Division { get; set; }
    public string Tier { get; set; }
}

public class PlayerRegion
{
    public PlayerRegion(string region)
    {
        Region = region;
    }

    public string Region { get; }
}

public class PlayerStatsResponse
{
    public PageProps PageProps { get; set; }
}

public class Data
{
    [JsonProperty("league_stats")] public List<LeagueStat> LeagueStats { get; set; }
    public int Id { get; set; }
}

public class LeagueStat
{
    [JsonProperty("tier_info")] public TierInfo TierInfo { get; set; }
    public int? Win { get; set; }
    public int? Lose { get; set; }
}

public class TierInfo
{
    public string Tier { get; set; }
    public int? Division { get; set; }
    public int? Lp { get; set; }
}
