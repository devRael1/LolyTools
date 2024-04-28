namespace Loly.src.Variables.UGG.APIs.HistoricRanks;

internal class HistoricRanksResponse
{
    internal Data Data { get; set; }
    internal List<Error> Errors { get; set; }
}

internal class Data
{
    internal List<HistoricPlayerRank> GetHistoricRanks { get; set; }
}

internal class HistoricPlayerRank
{
    internal int? Lp { get; set; }
    internal int? QueueId { get; set; }
    internal string Rank { get; set; }
    internal string RegionId { get; set; }
    internal int? Season { get; set; }
    internal string Tier { get; set; }
}