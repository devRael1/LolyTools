namespace Loly.src.Variables.Class;

internal class Champion
{
    internal bool FreeToPlay { get; set; }
    internal int Id { get; set; }
    internal string Name { get; set; }
    internal Ownership Ownership { get; set; }
}

internal class Ownership
{
    internal bool LoyaltyReward { get; set; }
    internal bool Owned { get; set; }
    internal bool XboxGPReward { get; set; }
}

