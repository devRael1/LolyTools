using Newtonsoft.Json;

namespace Loly.src.Variables.UGG.APIs.HistoricRanks;

public class HistoricRanks : IPayload
{
    [JsonProperty("operationName")] public string OperationName { get; set; } = "historicRanks";
    [JsonProperty("variables")] internal HistoricRanksVariables Variables { get; set; } = new();
    [JsonProperty("query")] public string Query { get; set; } = Queries.HistoricRanks;

    internal string ToFormattedString()
    {
        return JsonConvert.SerializeObject(this, Formatting.None).Replace("\\r", "");
    }
}