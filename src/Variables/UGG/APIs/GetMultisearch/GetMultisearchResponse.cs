namespace Loly.src.Variables.UGG.APIs.GetMultisearch;

public class GetMultisearchResponse
{
    public Data Data { get; set; }
}

public class Data
{
    public List<Multisearch> GetMultisearch { get; set; }
}

public class Multisearch
{
    public List<ChampionStats> BestChamps { get; set; }
    public RankScore RankData { get; set; }
    public string RiotTagLine { get; set; }
    public string RiotUserName { get; set; }
    public List<MultisearchRoleStats> RoleStats { get; set; }
    public int? TotalGamesLastFifteen { get; set; }
    public double? Winperc { get; set; }
    public int? WinsLastFifteen { get; set; }
    public int? Winstreak { get; set; }
    public List<ChampionStats> WorstChamps { get; set; }
}

public class MultisearchRoleStats
{
    public int Games { get; set; }
    public string RoleName { get; set; }
    public int Wins { get; set; }
}

public class RankScore
{
    public int Losses { get; set; }
    public int Lp { get; set; }
    public string QueueType { get; set; }
    public string Rank { get; set; }
    public int SeasonId { get; set; }
    public string Tier { get; set; }
    public int Wins { get; set; }
}

public class ChampionStats
{
    public int Assists { get; set; }
    public int ChampId { get; set; }
    public int Deaths { get; set; }
    public int Games { get; set; }
    public int Kills { get; set; }
    public int Wins { get; set; }
}