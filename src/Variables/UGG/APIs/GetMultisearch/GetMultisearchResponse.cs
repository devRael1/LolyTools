namespace Loly.src.Variables.UGG.APIs.GetMultisearch;

internal class GetMultisearchResponse
{
    internal Data Data { get; set; }
}

internal class Data
{
    internal List<Multisearch> GetMultisearch { get; set; }
}

internal class Multisearch
{
    internal List<ChampionStats> BestChamps { get; set; }
    internal RankScore RankData { get; set; }
    internal string RiotTagLine { get; set; }
    internal string RiotUserName { get; set; }
    internal List<MultisearchRoleStats> RoleStats { get; set; }
    internal int? TotalGamesLastFifteen { get; set; }
    internal double? Winperc { get; set; }
    internal int? WinsLastFifteen { get; set; }
    internal int? Winstreak { get; set; }
    internal List<ChampionStats> WorstChamps { get; set; }
}

internal class MultisearchRoleStats
{
    internal int Games { get; set; }
    internal string RoleName { get; set; }
    internal int Wins { get; set; }
}

internal class RankScore
{
    internal int Losses { get; set; }
    internal int Lp { get; set; }
    internal string QueueType { get; set; }
    internal string Rank { get; set; }
    internal int SeasonId { get; set; }
    internal string Tier { get; set; }
    internal int Wins { get; set; }
}

internal class ChampionStats
{
    internal int Assists { get; set; }
    internal int ChampId { get; set; }
    internal int Deaths { get; set; }
    internal int Games { get; set; }
    internal int Kills { get; set; }
    internal int Wins { get; set; }
}