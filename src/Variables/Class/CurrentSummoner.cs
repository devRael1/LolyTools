namespace Loly.src.Variables.Class;

public class CurrentSummoner
{
    public string AccountId { get; set; }
    public string DisplayName { get; set; }
    public string GameName { get; set; }
    public string TagLine { get; set; }
    public string Puuid { get; set; }
    public string SummonerId { get; set; }
    public string SummonerLevel { get; set; }

    public string GetFullGameName()
    {
        return $"{GameName}#{TagLine}";
    }
}