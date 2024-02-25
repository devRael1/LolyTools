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
        SoloDuoQ = new QueueStats();
        FlexQ = new QueueStats();
    }

    public string Username { get; }
    public string Tag { get; set; }
    public string UserTag { get; set; }
    public string Link { get; }
    public int? Id { get; set; }
    public int? Level { get; set; }
    public QueueStats SoloDuoQ { get; }
    public QueueStats FlexQ { get; }
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
    public List<LeagueStat> LeagueStats { get; set; }
    public int Id { get; set; }
}

public class LeagueStat
{
    public QueueInfo QueueInfo { get; set; }
    public TierInfo TierInfo { get; set; }
    public int? Win { get; set; }
    public int? Lose { get; set; }
    public bool IsHotStreak { get; set; }
    public bool IsInactive { get; set; }
    public string UpdatedAt { get; set; }
}

public class QueueInfo
{
    public int? Id { get; set; }
    public string QueueTranslate { get; set; }
    public string GameType { get; set; }
}

public class TierInfo
{
    public string Tier { get; set; }
    public int? Division { get; set; }
    public int? Lp { get; set; }
}
