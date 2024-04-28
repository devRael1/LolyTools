namespace Loly.src.Variables.UGG;

internal class Error
{
    internal List<Location> Locations { get; set; }
    internal string Message { get; set; }
    internal List<string> Path { get; set; }
}

internal class Location
{
    internal int Column { get; set; } = 0;
    internal int Line { get; set; } = 0;
}