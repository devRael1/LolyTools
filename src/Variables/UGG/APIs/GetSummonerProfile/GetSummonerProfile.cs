using Newtonsoft.Json;

namespace Loly.src.Variables.UGG.APIs.GetSummonerProfile;

public class GetSummonerProfile : IPayload
{
    [JsonProperty("operationName")] public string OperationName { get; set; } = "getSummonerProfile";
    [JsonProperty("variables")] public GetSummonerProfileVariables Variables { get; set; } = new();
    [JsonProperty("query")] public string Query { get; set; } = Queries.GetSummonerProfile;

    public string ToFormattedString()
    {
        return JsonConvert.SerializeObject(this, Formatting.None).Replace("\\r", "");
    }
}