using System.Text.Json;
using System.Text.Json.Serialization;
using BetterDOCX.Word;

if (args.Length == 0 || args[0] is "--help" or "-h")
{
    PrintUsage();
    return 0;
}

if (!string.Equals(args[0], "preflight", StringComparison.OrdinalIgnoreCase))
{
    Console.Error.WriteLine($"Unknown command: {args[0]}");
    PrintUsage();
    return 2;
}

var json = args.Contains("--json", StringComparer.OrdinalIgnoreCase);
var skipWordLaunch = args.Contains("--skip-word-launch", StringComparer.OrdinalIgnoreCase);
var report = new WordEnvironmentProbe().Run(exerciseWordAutomation: !skipWordLaunch);

if (json)
{
    Console.WriteLine(JsonSerializer.Serialize(report, new JsonSerializerOptions
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    }));
}
else
{
    Console.WriteLine("BetterDOCX environment preflight");
    Console.WriteLine();
    foreach (var check in report.Checks)
    {
        Console.WriteLine($"[{check.Status.ToString().ToUpperInvariant()}] {check.Name}: {check.Message}");
        if (!string.IsNullOrWhiteSpace(check.Evidence))
        {
            Console.WriteLine($"       {check.Evidence}");
        }
    }

    Console.WriteLine();
    Console.WriteLine(report.IsReady ? "Environment ready." : "Environment not ready.");
}

return report.IsReady ? 0 : 1;

static void PrintUsage()
{
    Console.WriteLine("BetterDOCX");
    Console.WriteLine();
    Console.WriteLine("Usage:");
    Console.WriteLine("  betterdocx preflight [--json] [--skip-word-launch]");
}
