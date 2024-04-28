using Loly.src.Variables.Enums;

namespace Loly.src.Logs;

internal struct LogMessage
{
    internal LogSeverity Severity { get; private set; }
    internal LogModule Module { get; private set; }
    internal string Message { get; private set; }
    internal Exception Exception { get; private set; }
}