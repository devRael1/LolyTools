namespace Loly.src.Variables.UGG;

public class Error
{
    public List<Location> Locations { get; set; }
    public string Message { get; set; }
    public List<string> Path { get; set; }
}

public class Location
{
    public int Column { get; set; } = 0;
    public int Line { get; set; } = 0;
}