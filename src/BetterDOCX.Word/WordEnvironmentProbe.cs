using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using BetterDOCX.Model;
using Microsoft.Win32;

namespace BetterDOCX.Word;

public sealed class WordEnvironmentProbe
{
    public PreflightReport Run(bool exerciseWordAutomation = true)
    {
        var checks = new List<PreflightCheck>
        {
            new(
                "dotnet-runtime",
                Environment.Version.Major >= 8 ? PreflightStatus.Pass : PreflightStatus.Fail,
                $".NET runtime {Environment.Version} is active.",
                RuntimeInformation.FrameworkDescription),
            new(
                "operating-system",
                OperatingSystem.IsWindows() ? PreflightStatus.Pass : PreflightStatus.Fail,
                OperatingSystem.IsWindows()
                    ? "Windows is available for native Word automation."
                    : "BetterDOCX Word rendering requires Windows in version one.",
                RuntimeInformation.OSDescription)
        };

        if (!OperatingSystem.IsWindows())
        {
            checks.Add(new("microsoft-word", PreflightStatus.Fail, "Microsoft Word can only be probed on Windows."));
            checks.Add(CheckPoppler());
            return new PreflightReport(DateTimeOffset.UtcNow, checks);
        }

        checks.Add(CheckWordRegistration());
        checks.Add(exerciseWordAutomation
            ? ExerciseWordAutomation()
            : new PreflightCheck("word-automation", PreflightStatus.Warning, "Word automation launch was skipped by request."));
        checks.Add(CheckPoppler());

        return new PreflightReport(DateTimeOffset.UtcNow, checks);
    }

    [SupportedOSPlatform("windows")]
    private static PreflightCheck CheckWordRegistration()
    {
        var path = ReadWordPath(Registry.LocalMachine) ?? ReadWordPath(Registry.CurrentUser);
        return path is not null && File.Exists(path)
            ? new PreflightCheck("microsoft-word", PreflightStatus.Pass, "Microsoft Word is registered.", path)
            : new PreflightCheck("microsoft-word", PreflightStatus.Fail, "WINWORD.EXE was not found through Windows App Paths registration.");
    }

    [SupportedOSPlatform("windows")]
    private static string? ReadWordPath(RegistryKey hive)
    {
        using var key = hive.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WINWORD.EXE");
        return key?.GetValue(null) as string;
    }

    [SupportedOSPlatform("windows")]
    private static PreflightCheck ExerciseWordAutomation()
    {
        object? application = null;
        try
        {
            var wordType = Type.GetTypeFromProgID("Word.Application", throwOnError: false);
            if (wordType is null)
            {
                return new PreflightCheck("word-automation", PreflightStatus.Fail, "The Word.Application COM registration is unavailable.");
            }

            application = Activator.CreateInstance(wordType);
            if (application is null)
            {
                return new PreflightCheck("word-automation", PreflightStatus.Fail, "Word.Application could not be created.");
            }

            dynamic word = application;
            word.Visible = false;
            word.DisplayAlerts = 0;
            string version = word.Version;
            word.Quit();

            return new PreflightCheck("word-automation", PreflightStatus.Pass, "Word automation started and exited successfully.", $"Word {version}");
        }
        catch (Exception exception)
        {
            return new PreflightCheck("word-automation", PreflightStatus.Fail, "Word automation failed.", exception.Message);
        }
        finally
        {
            if (application is not null && Marshal.IsComObject(application))
            {
                Marshal.FinalReleaseComObject(application);
            }
        }
    }

    private static PreflightCheck CheckPoppler()
    {
        var explicitPath = Environment.GetEnvironmentVariable("BETTERDOCX_PDFTOPPM");
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var codexBundledPath = Path.Combine(
            userProfile,
            ".cache",
            "codex-runtimes",
            "codex-primary-runtime",
            "dependencies",
            "native",
            "poppler",
            "Library",
            "bin",
            OperatingSystem.IsWindows() ? "pdftoppm.exe" : "pdftoppm");

        var executable = ExecutableLocator.Find(
            OperatingSystem.IsWindows() ? "pdftoppm.exe" : "pdftoppm",
            [explicitPath ?? string.Empty, codexBundledPath]);

        if (executable is null)
        {
            return new PreflightCheck(
                "pdf-rasterizer",
                PreflightStatus.Fail,
                "pdftoppm was not found. Set BETTERDOCX_PDFTOPPM or add Poppler to PATH.");
        }

        try
        {
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = executable,
                Arguments = "-v",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });

            if (process is null)
            {
                return new PreflightCheck("pdf-rasterizer", PreflightStatus.Fail, "pdftoppm could not be started.", executable);
            }

            var output = process.StandardError.ReadToEnd() + process.StandardOutput.ReadToEnd();
            if (!process.WaitForExit(5000))
            {
                process.Kill(entireProcessTree: true);
                return new PreflightCheck("pdf-rasterizer", PreflightStatus.Fail, "pdftoppm did not respond within five seconds.", executable);
            }

            var firstLine = output.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            return process.ExitCode == 0
                ? new PreflightCheck("pdf-rasterizer", PreflightStatus.Pass, "Poppler rasterization is available.", firstLine ?? executable)
                : new PreflightCheck("pdf-rasterizer", PreflightStatus.Fail, "pdftoppm returned a nonzero exit code.", output.Trim());
        }
        catch (Exception exception)
        {
            return new PreflightCheck("pdf-rasterizer", PreflightStatus.Fail, "pdftoppm validation failed.", exception.Message);
        }
    }
}
