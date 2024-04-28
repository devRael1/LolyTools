using Loly.src.Variables.Enums;

using Newtonsoft.Json;

namespace Loly.src.Variables.UGG;

internal class FetchMatchSummariesVariables
{
    [JsonProperty("regionId")] internal string RegionId { get; set; } = "";
    [JsonProperty("riotUserName")] internal string RiotUserName { get; set; } = "";
    [JsonProperty("riotTagLine")] internal string RiotTagLine { get; set; } = "";
    [JsonProperty("queueType")] internal List<QueueTypes> QueueType { get; set; } = new();
    [JsonProperty("duoRiotUserName")] internal string DuoRiotUserName { get; set; } = "";
    [JsonProperty("duoRiotTagLine")] internal string DuoRiotTagLine { get; set; } = "";
    [JsonProperty("role")] internal List<int> Role { get; set; } = new();
    [JsonProperty("seasonIds")] internal List<int> SeasonIds { get; set; } = new List<int> { 22, 21 };
    [JsonProperty("championId")] internal List<int> ChampionId { get; set; } = new();
}

internal class GetSummonerProfileVariables
{
    [JsonProperty("regionId")] internal string RegionId { get; set; } = "";
    [JsonProperty("riotUserName")] internal string RiotUserName { get; set; } = "";
    [JsonProperty("riotTagLine")] internal string RiotTagLine { get; set; } = "";
    [JsonProperty("seasonId")] internal int SeasonId { get; set; } = 22;
}

internal class GetPlayerOverallRankingVariables
{
    [JsonProperty("regionId")] internal string RegionId { get; set; } = "";
    [JsonProperty("riotUserName")] internal string RiotUserName { get; set; } = "";
    [JsonProperty("riotTagLine")] internal string RiotTagLine { get; set; } = "";
    [JsonProperty("queueType")] internal int QueueType { get; set; } = (int)QueueTypes.RankedSolo5v5New;
}

internal class GetMultisearchVariables
{
    [JsonProperty("regionId")] internal List<string> RegionId { get; set; } = new();
    [JsonProperty("riotUserName")] internal List<string> RiotUserName { get; set; } = new();
    [JsonProperty("riotTagLine")] internal List<string> RiotTagLine { get; set; } = new();
}

internal class HistoricRanksVariables
{
    [JsonProperty("regionId")] internal string RegionId { get; set; } = "";
    [JsonProperty("riotUserName")] internal string RiotUserName { get; set; } = "";
    [JsonProperty("riotTagLine")] internal string RiotTagLine { get; set; } = "";
    [JsonProperty("queueType")] internal int QueueType { get; set; } = (int)QueueTypes.RankedSolo5v5New;
}