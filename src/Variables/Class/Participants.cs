using Newtonsoft.Json;

namespace Loly.src.Variables.Class;

internal class ParticipantsResponse
{
    internal List<Participant> Participants { get; set; }
}

internal class Participant
{
    internal string ActivePlatform { get; set; }
    internal string Cid { get; set; }
    [JsonProperty("game_name")] internal string GameName { get; set; }
    [JsonProperty("game_tag")] internal string GameTag { get; set; }
    internal bool Muted { get; set; }
    internal string Name { get; set; }
    internal string Pid { get; set; }
    internal string Puuid { get; set; }
    internal string Region { get; set; }
}