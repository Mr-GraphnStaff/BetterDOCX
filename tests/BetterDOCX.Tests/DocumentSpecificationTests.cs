using System.Text.Json;
using BetterDOCX.Model;
using BetterDOCX.Validation;

namespace BetterDOCX.Tests;

public sealed class DocumentSpecificationTests
{
    private readonly DocumentSpecificationValidator validator = new();

    [Fact]
    public void ValidFixture_DeserializesAndPassesSemanticValidation()
    {
        var specification = LoadFixture("valid-professional-brief.json");

        var result = validator.Validate(specification);

        Assert.True(result.IsValid);
        Assert.Collection(
            specification.Document.Blocks,
            block => Assert.IsType<TitleBlock>(block),
            block => Assert.IsType<HeadingBlock>(block),
            block => Assert.IsType<ParagraphBlock>(block),
            block => Assert.IsType<BulletListBlock>(block),
            block => Assert.IsType<PageBreakBlock>(block));
    }

    [Fact]
    public void InvalidFixture_ReportsEmptyBulletList()
    {
        var specification = LoadFixture("invalid-empty-bullets.json");

        var result = validator.Validate(specification);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal("$.document.blocks[0].items", error.Path);
        Assert.Equal("minimum", error.Code);
    }

    [Fact]
    public void UnknownBlockType_IsRejectedDuringDeserialization()
    {
        const string json = """
            {
              "schemaVersion": "1.0",
              "document": {
                "title": "Unknown block",
                "purpose": null,
                "audience": null,
                "design": {
                  "family": "professional",
                  "density": "standard",
                  "accentColor": "#1F4E79"
                },
                "blocks": [{ "type": "mystery" }]
              }
            }
            """;

        Assert.Throws<JsonException>(() => DocumentSpecificationJson.Deserialize(json));
    }

    [Fact]
    public void UnknownProperty_IsRejectedDuringDeserialization()
    {
        var json = File.ReadAllText(FixturePath("valid-professional-brief.json"));
        json = json.Replace(
            "\"schemaVersion\": \"1.0\"",
            "\"schemaVersion\": \"1.0\", \"unexpected\": true",
            StringComparison.Ordinal);

        Assert.Throws<JsonException>(() => DocumentSpecificationJson.Deserialize(json));
    }

    [Fact]
    public void ValidSpecification_RoundTripsWithoutChangingBlockTypes()
    {
        var original = LoadFixture("valid-professional-brief.json");

        var serialized = DocumentSpecificationJson.Serialize(original);
        var roundTripped = DocumentSpecificationJson.Deserialize(serialized);

        Assert.Equal(serialized, DocumentSpecificationJson.Serialize(roundTripped));
        Assert.Equal(
            original.Document.Blocks.Select(block => block.GetType()),
            roundTripped.Document.Blocks.Select(block => block.GetType()));
    }

    private static DocumentSpecification LoadFixture(string name) =>
        DocumentSpecificationJson.Deserialize(File.ReadAllText(FixturePath(name)));

    private static string FixturePath(string name) =>
        Path.Combine(AppContext.BaseDirectory, "Fixtures", name);
}
