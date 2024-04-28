namespace Loly.src.Variables.UGG.APIs.HistoricRanks;

public class HistoricRanksResponse
{
    public Data Data { get; set; }
    public List<Error> Errors { get; set; }
}

public class Data
{
    public List<HistoricPlayerRank> GetHistoricRanks { get; set; }
}

public class HistoricPlayerRank
{
    public int? Lp { get; set; }
    public int? QueueId { get; set; }
    public string Rank { get; set; }
    public string RegionId { get; set; }
    public int? Season { get; set; }
    public string Tier { get; set; }
}