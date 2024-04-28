namespace Loly.src.Variables.UGG.APIs.FetchMatchSummaries;

internal class FetchMatchSummariesResponse
{
    internal Data Data { get; set; }
    internal List<Error> Errors { get; set; }
}

internal class Data
{
    internal FetchPlayerMatchSummaries FetchPlayerMatchSummaries { get; set; }
}

internal class FetchPlayerMatchSummaries
{
    internal bool FinishedMatchSummaries { get; set; }
    internal List<MatchSummary> MatchSummaries { get; set; }
    internal int? TotalNumMatches { get; set; }
}

internal class MatchSummary
{
    internal int? Kills { get; set; }
    internal int? PrimaryStyle { get; set; }
    internal int? SubStyle { get; set; }
    internal int? VisionScore { get; set; }
    internal List<TeamData> TeamA { get; set; }
    internal List<int> Items { get; set; }
    internal bool Win { get; set; }
    internal int? Deaths { get; set; }
    internal int? ChampionId { get; set; }
    internal List<int> Runes { get; set; }
    internal int? Level { get; set; }
    internal int? PsHardCarry { get; set; }
    internal List<int> SummonerSpells { get; set; }
    internal long MatchCreationTime { get; set; }
    internal int? KillParticipation { get; set; }
    internal int? Gold { get; set; }
    internal int? Cs { get; set; }
    internal string RiotUserName { get; set; }
    internal string Version { get; set; }
    internal List<int> Augments { get; set; }
    internal int? PsTeamPlay { get; set; }
    internal string RegionId { get; set; }
    internal int? MatchDuration { get; set; }
    internal List<TeamData> TeamB { get; set; }
    internal int? MaximumKillStreak { get; set; }
    internal int? Damage { get; set; }
    internal int? JungleCs { get; set; }
    internal long MatchId { get; set; }
    internal string RiotTagLine { get; set; }
    internal int? Role { get; set; }
    internal string SummonerName { get; set; }
    internal LpInfo LpInfo { get; set; }
    internal string QueueType { get; set; }
    internal int? Assists { get; set; }
}

internal class TeamData
{
    internal int? ChampionId { get; set; }
    internal double HardCarry { get; set; }
    internal int? Placement { get; set; }
    internal int? PlayerSubteamId { get; set; }
    internal string RiotTagLine { get; set; }
    internal string RiotUserName { get; set; }
    internal int? Role { get; set; }
    internal string SummonerName { get; set; }
    internal int? TeamId { get; set; }
    internal double Teamplay { get; set; }
}

internal class LpInfo
{
    internal int? Lp { get; set; }
    internal int? Placement { get; set; }
    internal string PromoProgress { get; set; }
    internal string PromoTarget { get; set; }
    internal PromotedRank PromotedTo { get; set; }
}

internal class PromotedRank
{
    internal string Rank { get; set; }
    internal string Tier { get; set; }
}