using System.Text.RegularExpressions;
using BetterDOCX.Model;

namespace BetterDOCX.Validation;

public sealed record SpecificationValidationError(
    string Path,
    string Code,
    string Message);

public sealed record SpecificationValidationResult(
    IReadOnlyList<SpecificationValidationError> Errors)
{
    public bool IsValid => Errors.Count == 0;
}

public sealed class DocumentSpecificationValidator
{
    private static readonly HashSet<string> SupportedFamilies =
        new(StringComparer.Ordinal) { "professional", "executive", "technical", "minimal", "editorial" };

    private static readonly HashSet<string> SupportedDensities =
        new(StringComparer.Ordinal) { "compact", "standard", "relaxed" };

    public SpecificationValidationResult Validate(DocumentSpecification? specification)
    {
        var errors = new List<SpecificationValidationError>();
        if (specification is null)
        {
            errors.Add(new("$", "required", "A document specification is required."));
            return new SpecificationValidationResult(errors);
        }

        if (!string.Equals(specification.SchemaVersion, "1.0", StringComparison.Ordinal))
        {
            errors.Add(new("$.schemaVersion", "unsupported", "schemaVersion must be exactly '1.0'."));
        }

        if (specification.Document is null)
        {
            errors.Add(new("$.document", "required", "document is required."));
            return new SpecificationValidationResult(errors);
        }

        ValidateRequiredText(specification.Document.Title, "$.document.title", errors);
        ValidateDesign(specification.Document.Design, errors);
        ValidateBlocks(specification.Document.Blocks, errors);

        return new SpecificationValidationResult(errors);
    }

    private static void ValidateDesign(
        DesignSpecification? design,
        ICollection<SpecificationValidationError> errors)
    {
        if (design is null)
        {
            errors.Add(new("$.document.design", "required", "design is required."));
            return;
        }

        if (!SupportedFamilies.Contains(design.Family))
        {
            errors.Add(new(
                "$.document.design.family",
                "unsupported",
                $"Unsupported design family '{design.Family}'."));
        }

        if (!SupportedDensities.Contains(design.Density))
        {
            errors.Add(new(
                "$.document.design.density",
                "unsupported",
                $"Unsupported design density '{design.Density}'."));
        }

        if (string.IsNullOrWhiteSpace(design.AccentColor)
            || !Regex.IsMatch(design.AccentColor, "^#[0-9A-Fa-f]{6}$", RegexOptions.CultureInvariant))
        {
            errors.Add(new(
                "$.document.design.accentColor",
                "format",
                "accentColor must be a six-digit hexadecimal color such as '#1F4E79'."));
        }
    }

    private static void ValidateBlocks(
        IReadOnlyList<DocumentBlock>? blocks,
        ICollection<SpecificationValidationError> errors)
    {
        if (blocks is null || blocks.Count == 0)
        {
            errors.Add(new("$.document.blocks", "minimum", "At least one document block is required."));
            return;
        }

        for (var index = 0; index < blocks.Count; index++)
        {
            var path = $"$.document.blocks[{index}]";
            switch (blocks[index])
            {
                case TitleBlock title:
                    ValidateRequiredText(title.Text, $"{path}.text", errors);
                    break;
                case HeadingBlock heading:
                    if (heading.Level is < 1 or > 3)
                    {
                        errors.Add(new($"{path}.level", "range", "Heading level must be between 1 and 3."));
                    }

                    ValidateRequiredText(heading.Text, $"{path}.text", errors);
                    break;
                case ParagraphBlock paragraph:
                    ValidateRequiredText(paragraph.Content, $"{path}.content", errors);
                    break;
                case BulletListBlock bulletList:
                    if (bulletList.Items is null || bulletList.Items.Count == 0)
                    {
                        errors.Add(new($"{path}.items", "minimum", "A bullet list requires at least one item."));
                        break;
                    }

                    for (var itemIndex = 0; itemIndex < bulletList.Items.Count; itemIndex++)
                    {
                        ValidateRequiredText(
                            bulletList.Items[itemIndex],
                            $"{path}.items[{itemIndex}]",
                            errors);
                    }

                    break;
                case PageBreakBlock:
                    break;
                case null:
                    errors.Add(new(path, "required", "Document blocks cannot be null."));
                    break;
                default:
                    errors.Add(new(path, "unsupported", $"Unsupported block type '{blocks[index].GetType().Name}'."));
                    break;
            }
        }
    }

    private static void ValidateRequiredText(
        string? value,
        string path,
        ICollection<SpecificationValidationError> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add(new(path, "required", "A non-empty text value is required."));
        }
    }
}
