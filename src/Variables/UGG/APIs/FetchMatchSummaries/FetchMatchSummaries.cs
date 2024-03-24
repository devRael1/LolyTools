using Newtonsoft.Json;

namespace Loly.src.Variables.UGG.APIs.FetchMatchSummaries;

public class FetchMatchSummaries : IPayload
{
    [JsonProperty("operationName")] public string OperationName { get; set; } = "FetchMatchSummaries";
    [JsonProperty("variables")] public FetchMatchSummariesVariables Variables { get; set; } = new();
    [JsonProperty("query")] public string Query { get; set; } = Queries.FetchMatchSummaries;

    public string ToFormattedString()
    {
        return JsonConvert.SerializeObject(this, Formatting.None).Replace("\\r", "");
    }
}