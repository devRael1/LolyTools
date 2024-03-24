using Loly.src.Variables.UGG.APIs.GetMultisearch;

namespace Loly.src.Variables.Class;

public class Player
{
    public string RiotTagLine { get; set; }
    public string RiotUserName { get; set; }
    public string RiotUserTag { get; set; }
    public string RiotUserTagEncoded { get; set; }
    public int Level { get; set; }
    public List<MultisearchRoleStats> RoleStats { get; set; }
    public int Winstreak { get; set; }
    public string Link { get; set; }
    public RankScore SoloDuoQ { get; set; }
    public RankScore FlexQ { get; set; }
}