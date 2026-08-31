using BetterDOCX.Model;

namespace BetterDOCX.Tests;

public sealed class PreflightReportTests
{
    [Fact]
    public void IsReady_IsTrue_WhenNoCheckFails()
    {
        var report = new PreflightReport(
            DateTimeOffset.UtcNow,
            [
                new PreflightCheck("one", PreflightStatus.Pass, "ready"),
                new PreflightCheck("two", PreflightStatus.Warning, "optional warning")
            ]);

        Assert.True(report.IsReady);
    }

    [Fact]
    public void IsReady_IsFalse_WhenAnyCheckFails()
    {
        var report = new PreflightReport(
            DateTimeOffset.UtcNow,
            [
                new PreflightCheck("one", PreflightStatus.Pass, "ready"),
                new PreflightCheck("two", PreflightStatus.Fail, "missing")
            ]);

        Assert.False(report.IsReady);
    }
}
