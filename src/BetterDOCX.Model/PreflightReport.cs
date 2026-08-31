namespace BetterDOCX.Model;

public sealed record PreflightReport(
    DateTimeOffset GeneratedAtUtc,
    IReadOnlyList<PreflightCheck> Checks)
{
    public bool IsReady => Checks.All(check => check.Status != PreflightStatus.Fail);
}
