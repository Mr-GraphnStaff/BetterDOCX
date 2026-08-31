using BetterDOCX.Word;

namespace BetterDOCX.Tests;

public sealed class ExecutableLocatorTests
{
    [Fact]
    public void Find_ReturnsExplicitExistingCandidate()
    {
        var temporaryFile = Path.GetTempFileName();
        try
        {
            var result = ExecutableLocator.Find("not-on-path.exe", [temporaryFile]);

            Assert.Equal(Path.GetFullPath(temporaryFile), result);
        }
        finally
        {
            File.Delete(temporaryFile);
        }
    }

    [Fact]
    public void Find_ReturnsNull_WhenExecutableIsUnavailable()
    {
        var result = ExecutableLocator.Find($"missing-{Guid.NewGuid():N}.exe");

        Assert.Null(result);
    }
}
