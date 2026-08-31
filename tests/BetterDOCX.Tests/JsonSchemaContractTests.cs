using System.Text.Json;

namespace BetterDOCX.Tests;

public sealed class JsonSchemaContractTests
{
    [Fact]
    public void Schema_IsDraft202012AndDefinesFiveBlockAlternatives()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", "document-specification.schema.json");
        using var schema = JsonDocument.Parse(File.ReadAllText(path));

        var root = schema.RootElement;
        Assert.Equal("https://json-schema.org/draft/2020-12/schema", root.GetProperty("$schema").GetString());
        Assert.Equal(
            5,
            root.GetProperty("$defs")
                .GetProperty("document")
                .GetProperty("properties")
                .GetProperty("blocks")
                .GetProperty("items")
                .GetProperty("oneOf")
                .GetArrayLength());
    }

    [Fact]
    public void Schema_RejectsAdditionalPropertiesAtRootAndBlockLevel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", "document-specification.schema.json");
        using var schema = JsonDocument.Parse(File.ReadAllText(path));

        var root = schema.RootElement;
        Assert.False(root.GetProperty("additionalProperties").GetBoolean());

        foreach (var blockName in new[] { "titleBlock", "headingBlock", "paragraphBlock", "bulletListBlock", "pageBreakBlock" })
        {
            Assert.False(
                root.GetProperty("$defs")
                    .GetProperty(blockName)
                    .GetProperty("additionalProperties")
                    .GetBoolean());
        }
    }
}
