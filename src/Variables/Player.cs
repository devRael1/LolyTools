namespace Loly.src.Variables;

public class Player
{
    public Player(string username, string link)
    {
        Username = username;
        Link = link;

        Id = "";
        Level = 0;
        SoloDuoQ = new SoloDuoQ();
        FlexQ = new FlexQ();
    }

    public string Username { get; }
    public string Link { get; }
    public string Id { get; set; }
    public int Level { get; set; }
    public SoloDuoQ SoloDuoQ { get; }
    public FlexQ FlexQ { get; }
}

public class SoloDuoQ
{
    public SoloDuoQ()
    {
        Wins = 0;
        Losses = 0;
        Lp = 0;
        Division = 0;
        Tier = "Unranked";
    }

    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Lp { get; set; }
    public int Division { get; set; }
    public string Tier { get; set; }
}

public class FlexQ
{
    public FlexQ()
    {
        Wins = 0;
        Losses = 0;
        Lp = 0;
        Division = 0;
        Tier = "Unranked";
    }

    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Lp { get; set; }
    public int Division { get; set; }
    public string Tier { get; set; }
}

public class PlayerRegion
{
    public PlayerRegion(string region)
    {
        Region = region;
    }

    public string Region { get; }
}