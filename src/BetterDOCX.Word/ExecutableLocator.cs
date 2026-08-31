namespace BetterDOCX.Word;

public static class ExecutableLocator
{
    public static string? Find(string executableName, IEnumerable<string>? additionalCandidates = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(executableName);

        foreach (var candidate in additionalCandidates ?? [])
        {
            if (!string.IsNullOrWhiteSpace(candidate) && File.Exists(candidate))
            {
                return Path.GetFullPath(candidate);
            }
        }

        var path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
        foreach (var directory in path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var candidate = Path.Combine(directory.Trim('"'), executableName);
            if (File.Exists(candidate))
            {
                return Path.GetFullPath(candidate);
            }
        }

        return null;
    }
}
