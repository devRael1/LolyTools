using Loly.src.Variables.UGG.APIs.GetMultisearch;

namespace Loly.src.Variables.Class;

internal class Player
{
    internal string RiotTagLine { get; set; }
    internal string RiotUserName { get; set; }
    internal string RiotUserTag { get; set; }
    internal string RiotUserTagEncoded { get; set; }
    internal int Level { get; set; }
    internal List<MultisearchRoleStats> RoleStats { get; set; }
    internal int Winstreak { get; set; }
    internal string Link { get; set; }
    internal RankScore SoloDuoQ { get; set; }
    internal RankScore FlexQ { get; set; }
}