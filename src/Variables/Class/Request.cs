namespace Loly.src.Variables.Class;

public class Request : IRequest
{
    public string Method { get; set; }
    public string Url { get; set; }
    public string Body { get; set; }
    public Exception Exception { get; set; }
}

public class Response : IRequest
{
    public string Method { get; set; }
    public string Url { get; set; }
    public int StatusCode { get; set; }
    public string[] Data { get; set; }
    public Exception Exception { get; set; }
}