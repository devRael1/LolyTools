using Newtonsoft.Json;

namespace Loly.src.Variables.UGG.APIs.GetMultisearch;

public class GetMultisearch : IPayload
{
    [JsonProperty("operationName")] public string OperationName { get; set; } = "GetMultisearch";
    [JsonProperty("variables")] public GetMultisearchVariables Variables { get; set; } = new();
    [JsonProperty("query")] public string Query { get; set; } = Queries.GetMultisearch;

    internal string ToFormattedString()
    {
        return JsonConvert.SerializeObject(this, Formatting.None).Replace("\\r", "");
    }
}