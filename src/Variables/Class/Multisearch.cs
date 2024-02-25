using Newtonsoft.Json;

namespace Loly.src.Variables.Class;

public class MultisearchResponse
{
    public PageProps PageProps { get; set; }
}

public class PageProps
{
    public List<Summoner> Summoners { get; set; }
    public Data Data { get; set; }
}

public class SoloTierInfo
{
    public string Tier { get; set; }
    public int? Division { get; set; }
    public int? Lp { get; set; }
}

public class Summoner
{
    public int Id { get; set; }
    public string Tagline { get; set; }
    public string Name { get; set; }
    public int? Level { get; set; }
    [JsonProperty("solo_tier_info")] public SoloTierInfo SoloTierInfo { get; set; }
}