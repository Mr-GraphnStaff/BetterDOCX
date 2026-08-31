using System.Text.Json;
using System.Text.Json.Serialization;

namespace BetterDOCX.Model;

public static class DocumentSpecificationJson
{
    public static JsonSerializerOptions Options { get; } = CreateOptions();

    public static DocumentSpecification Deserialize(string json)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(json);

        return JsonSerializer.Deserialize<DocumentSpecification>(json, Options)
            ?? throw new JsonException("The document specification was empty.");
    }

    public static string Serialize(DocumentSpecification specification)
    {
        ArgumentNullException.ThrowIfNull(specification);

        return JsonSerializer.Serialize(specification, Options);
    }

    private static JsonSerializerOptions CreateOptions() => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        WriteIndented = true
    };
}
