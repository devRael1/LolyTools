namespace Loly.src.Variables.UGG.APIs.FetchMatchSummaries;

public class FetchMatchSummariesResponse
{
    public Data Data { get; set; }
    public List<Error> Errors { get; set; }
}

public class Data
{
    public FetchPlayerMatchSummaries FetchPlayerMatchSummaries { get; set; }
}

public class FetchPlayerMatchSummaries
{
    public bool FinishedMatchSummaries { get; set; }
    public List<MatchSummary> MatchSummaries { get; set; }
    public int? TotalNumMatches { get; set; }
}

public class MatchSummary
{
    public int? Kills { get; set; }
    public int? PrimaryStyle { get; set; }
    public int? SubStyle { get; set; }
    public int? VisionScore { get; set; }
    public List<TeamData> TeamA { get; set; }
    public List<int> Items { get; set; }
    public bool Win { get; set; }
    public int? Deaths { get; set; }
    public int? ChampionId { get; set; }
    public List<int> Runes { get; set; }
    public int? Level { get; set; }
    public int? PsHardCarry { get; set; }
    public List<int> SummonerSpells { get; set; }
    public long MatchCreationTime { get; set; }
    public int? KillParticipation { get; set; }
    public int? Gold { get; set; }
    public int? Cs { get; set; }
    public string RiotUserName { get; set; }
    public string Version { get; set; }
    public List<int> Augments { get; set; }
    public int? PsTeamPlay { get; set; }
    public string RegionId { get; set; }
    public int? MatchDuration { get; set; }
    public List<TeamData> TeamB { get; set; }
    public int? MaximumKillStreak { get; set; }
    public int? Damage { get; set; }
    public int? JungleCs { get; set; }
    public long MatchId { get; set; }
    public string RiotTagLine { get; set; }
    public int? Role { get; set; }
    public string SummonerName { get; set; }
    public LpInfo LpInfo { get; set; }
    public string QueueType { get; set; }
    public int? Assists { get; set; }
}

public class TeamData
{
    public int? ChampionId { get; set; }
    public double HardCarry { get; set; }
    public int? Placement { get; set; }
    public int? PlayerSubteamId { get; set; }
    public string RiotTagLine { get; set; }
    public string RiotUserName { get; set; }
    public int? Role { get; set; }
    public string SummonerName { get; set; }
    public int? TeamId { get; set; }
    public double Teamplay { get; set; }
}

public class LpInfo
{
    public int? Lp { get; set; }
    public int? Placement { get; set; }
    public string PromoProgress { get; set; }
    public string PromoTarget { get; set; }
    public PromotedRank PromotedTo { get; set; }
}

public class PromotedRank
{
    public string Rank { get; set; }
    public string Tier { get; set; }
}