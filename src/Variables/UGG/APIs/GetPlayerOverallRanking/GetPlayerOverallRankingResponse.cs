using Newtonsoft.Json;

namespace Loly.src.Variables.UGG.APIs.GetPlayerOverallRanking;

public class GetPlayerOverallRankingResponse
{
    public Data Data { get; set; }
    public List<Error> Errors { get; set; }
}

public class Data
{
    public OverallRanking OverallRanking { get; set; }
}

public class OverallRanking
{
    [JsonProperty("overallRanking")] public int? OverallRank { get; set; }
    public int? TotalPlayerCount { get; set; }
}