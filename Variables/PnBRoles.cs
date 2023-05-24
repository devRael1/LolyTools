namespace Loly.Variables;

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