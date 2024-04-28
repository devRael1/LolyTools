namespace Loly.src.Variables.Class;

public class Champion
{
    public bool FreeToPlay { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public Ownership Ownership { get; set; }
}

public class Ownership
{
    public bool LoyaltyReward { get; set; }
    public bool Owned { get; set; }
    public bool XboxGPReward { get; set; }
}

