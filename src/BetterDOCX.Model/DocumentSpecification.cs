using System.Text.Json.Serialization;

namespace BetterDOCX.Model;

public sealed record DocumentSpecification(
    string SchemaVersion,
    DocumentDefinition Document);

public sealed record DocumentDefinition(
    string Title,
    string? Purpose,
    string? Audience,
    DesignSpecification Design,
    IReadOnlyList<DocumentBlock> Blocks);

public sealed record DesignSpecification(
    string Family,
    string Density,
    string AccentColor);

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(TitleBlock), "title")]
[JsonDerivedType(typeof(HeadingBlock), "heading")]
[JsonDerivedType(typeof(ParagraphBlock), "paragraph")]
[JsonDerivedType(typeof(BulletListBlock), "bulletList")]
[JsonDerivedType(typeof(PageBreakBlock), "pageBreak")]
public abstract record DocumentBlock;

public sealed record TitleBlock(
    string Text,
    string? Subtitle = null) : DocumentBlock;

public sealed record HeadingBlock(
    int Level,
    string Text) : DocumentBlock;

public sealed record ParagraphBlock(
    string Content) : DocumentBlock;

public sealed record BulletListBlock(
    IReadOnlyList<string> Items) : DocumentBlock;

public sealed record PageBreakBlock : DocumentBlock;
