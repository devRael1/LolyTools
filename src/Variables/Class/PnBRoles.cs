namespace Loly.src.Variables.Class;

public class PnBRoles
{
    public InitRole Default { get; set; } = new();
    public InitRole Top { get; set; } = new();
    public InitRole Jungle { get; set; } = new();
    public InitRole Mid { get; set; } = new();
    public InitRole Adc { get; set; } = new();
    public InitRole Support { get; set; } = new();
}

public class InitRole
{
    public ChampItem PickChamp { get; } = new();
    public ChampItem BanChamp { get; } = new();
}

public class ChampItem
{
    public string Name { get; set; }
    public string Id { get; set; }
    public bool Free { get; set; }
    public int Delay { get; set; } = 2000;
}