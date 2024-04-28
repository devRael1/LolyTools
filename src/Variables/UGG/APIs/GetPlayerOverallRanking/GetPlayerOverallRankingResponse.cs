using Newtonsoft.Json;

namespace Loly.src.Variables.UGG.APIs.GetPlayerOverallRanking;

internal class GetPlayerOverallRankingResponse
{
    internal Data Data { get; set; }
    internal List<Error> Errors { get; set; }
}

internal class Data
{
    internal OverallRanking OverallRanking { get; set; }
}

internal class OverallRanking
{
    [JsonProperty("overallRanking")] internal int? OverallRank { get; set; }
    internal int? TotalPlayerCount { get; set; }
}