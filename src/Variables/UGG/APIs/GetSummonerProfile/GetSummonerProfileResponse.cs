using Loly.src.Variables.UGG.APIs.GetMultisearch;

namespace Loly.src.Variables.UGG.APIs.GetSummonerProfile;

public class GetSummonerProfileResponse
{
    public Data Data { get; set; }
    public List<Error> Errors { get; set; }
}

public class Data
{
    public FetchProfileRanks FetchProfileRanks { get; set; }
    public ProfileInitSimple ProfileInitSimple { get; set; }
}

public class ProfileInitSimple
{
    public CustomizationData CustomizationData { get; set; }
    public string LastModified { get; set; }
    public string MemberStatus { get; set; }
    public PlayerInfo PlayerInfo { get; set; }
}

public class PlayerInfo
{
    public int AccountIdV3 { get; set; }
    public string AccountIdV4 { get; set; }
    public string ExodiaUuid { get; set; }
    public int IconId { get; set; }
    public string PuuidV4 { get; set; }
    public string RegionId { get; set; }
    public string RiotTagLine { get; set; }
    public string RiotUserName { get; set; }
    public int SummonerIdV3 { get; set; }
    public string SummonerIdV4 { get; set; }
    public int SummonerLevel { get; set; }
}

public class CustomizationData
{
    public string HeaderBg { get; set; }
}

public class FetchProfileRanks
{
    public List<RankScore> RankScores { get; set; }
}