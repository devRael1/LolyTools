namespace Loly.src.Variables.Class;

internal class CurrentSummoner
{
    internal string AccountId { get; set; }
    internal string DisplayName { get; set; }
    internal string GameName { get; set; }
    internal string TagLine { get; set; }
    internal string Puuid { get; set; }
    internal string SummonerId { get; set; }
    internal string SummonerLevel { get; set; }

    internal string GetFullGameName()
    {
        return $"{GameName}#{TagLine}";
    }
}