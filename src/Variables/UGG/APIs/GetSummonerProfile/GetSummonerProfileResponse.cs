using Loly.src.Variables.UGG.APIs.GetMultisearch;

namespace Loly.src.Variables.UGG.APIs.GetSummonerProfile;

internal class GetSummonerProfileResponse
{
    internal Data Data { get; set; }
    internal List<Error> Errors { get; set; }
}

internal class Data
{
    internal FetchProfileRanks FetchProfileRanks { get; set; }
    internal ProfileInitSimple ProfileInitSimple { get; set; }
}

internal class ProfileInitSimple
{
    internal CustomizationData CustomizationData { get; set; }
    internal string LastModified { get; set; }
    internal string MemberStatus { get; set; }
    internal PlayerInfo PlayerInfo { get; set; }
}

internal class PlayerInfo
{
    internal int AccountIdV3 { get; set; }
    internal string AccountIdV4 { get; set; }
    internal string ExodiaUuid { get; set; }
    internal int IconId { get; set; }
    internal string PuuidV4 { get; set; }
    internal string RegionId { get; set; }
    internal string RiotTagLine { get; set; }
    internal string RiotUserName { get; set; }
    internal int SummonerIdV3 { get; set; }
    internal string SummonerIdV4 { get; set; }
    internal int SummonerLevel { get; set; }
}

internal class CustomizationData
{
    internal string HeaderBg { get; set; }
}

internal class FetchProfileRanks
{
    internal List<RankScore> RankScores { get; set; }
}