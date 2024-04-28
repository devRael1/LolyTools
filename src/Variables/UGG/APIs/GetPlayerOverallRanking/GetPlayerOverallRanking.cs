﻿using Newtonsoft.Json;

namespace Loly.src.Variables.UGG.APIs.GetPlayerOverallRanking;

public class GetPlayerOverallRanking : IPayload
{
    [JsonProperty("operationName")] public string OperationName { get; set; } = "getPlayerOverallRanking";
    [JsonProperty("variables")] internal GetPlayerOverallRankingVariables Variables { get; set; } = new();
    [JsonProperty("query")] public string Query { get; set; } = Queries.GetPlayerOverallRanking;

    internal string ToFormattedString()
    {
        return JsonConvert.SerializeObject(this, Formatting.None).Replace("\\r", "");
    }
}