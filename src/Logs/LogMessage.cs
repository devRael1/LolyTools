using Loly.src.Variables.Enums;

namespace Loly.src.Logs;

public struct LogMessage
{
    public LogSeverity Severity { get; private set; }
    public LogModule Module { get; private set; }
    public string Message { get; private set; }
    public Exception Exception { get; private set; }
}