namespace Loly.src.Variables.Class;

public interface IRequest
{
    public string Method { get; set; }
    public string Url { get; set; }
    public Exception Exception { get; set; }
}