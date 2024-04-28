using Newtonsoft.Json;

namespace Loly.src.Variables.Class;

public class ParticipantsResponse
{
    public List<Participant> Participants { get; set; }
}

public class Participant
{
    public string ActivePlatform { get; set; }
    public string Cid { get; set; }
    [JsonProperty("game_name")] public string GameName { get; set; }
    [JsonProperty("game_tag")] public string GameTag { get; set; }
    public bool Muted { get; set; }
    public string Name { get; set; }
    public string Pid { get; set; }
    public string Puuid { get; set; }
    public string Region { get; set; }
}