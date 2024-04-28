using Loly.src.Variables.Enums;

using Newtonsoft.Json;

namespace Loly.src.Variables.UGG;

public class FetchMatchSummariesVariables
{
    [JsonProperty("regionId")] public string RegionId { get; set; } = "";
    [JsonProperty("riotUserName")] public string RiotUserName { get; set; } = "";
    [JsonProperty("riotTagLine")] public string RiotTagLine { get; set; } = "";
    [JsonProperty("queueType")] public List<QueueTypes> QueueType { get; set; } = new();
    [JsonProperty("duoRiotUserName")] public string DuoRiotUserName { get; set; } = "";
    [JsonProperty("duoRiotTagLine")] public string DuoRiotTagLine { get; set; } = "";
    [JsonProperty("role")] public List<int> Role { get; set; } = new();
    [JsonProperty("seasonIds")] public List<int> SeasonIds { get; set; } = new List<int> { 22, 21 };
    [JsonProperty("championId")] public List<int> ChampionId { get; set; } = new();
}

public class GetSummonerProfileVariables
{
    [JsonProperty("regionId")] public string RegionId { get; set; } = "";
    [JsonProperty("riotUserName")] public string RiotUserName { get; set; } = "";
    [JsonProperty("riotTagLine")] public string RiotTagLine { get; set; } = "";
    [JsonProperty("seasonId")] public int SeasonId { get; set; } = 22;
}

public class GetPlayerOverallRankingVariables
{
    [JsonProperty("regionId")] public string RegionId { get; set; } = "";
    [JsonProperty("riotUserName")] public string RiotUserName { get; set; } = "";
    [JsonProperty("riotTagLine")] public string RiotTagLine { get; set; } = "";
    [JsonProperty("queueType")] public int QueueType { get; set; } = (int)QueueTypes.RankedSolo5v5New;
}

public class GetMultisearchVariables
{
    [JsonProperty("regionId")] public List<string> RegionId { get; set; } = new();
    [JsonProperty("riotUserName")] public List<string> RiotUserName { get; set; } = new();
    [JsonProperty("riotTagLine")] public List<string> RiotTagLine { get; set; } = new();
}

public class HistoricRanksVariables
{
    [JsonProperty("regionId")] public string RegionId { get; set; } = "";
    [JsonProperty("riotUserName")] public string RiotUserName { get; set; } = "";
    [JsonProperty("riotTagLine")] public string RiotTagLine { get; set; } = "";
    [JsonProperty("queueType")] public int QueueType { get; set; } = (int)QueueTypes.RankedSolo5v5New;
}