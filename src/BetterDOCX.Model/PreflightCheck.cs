namespace BetterDOCX.Model;

public enum PreflightStatus
{
    Pass,
    Warning,
    Fail
}

public sealed record PreflightCheck(
    string Name,
    PreflightStatus Status,
    string Message,
    string? Evidence = null);
