# Document Specification Version 1

The document specification is the stable contract between an authoring agent
and the BetterDOCX composition engine. Chat users describe the desired document
in natural language; the agent produces this specification internally.

The normative machine-readable contract is
[`schemas/document-specification.schema.json`](../schemas/document-specification.schema.json).

## Compatibility

- `schemaVersion` is required and must be `1.0`.
- Unknown JSON properties are rejected.
- Unknown block discriminators are rejected.
- A v1 document requires metadata, a design selection, and at least one block.
- New block types require a future compatible schema revision or version.

## Document metadata

| Field | Required | Meaning |
|---|---:|---|
| `title` | Yes | Stable document name and output metadata |
| `purpose` | No | Business or communication objective |
| `audience` | No | Intended readers |
| `design` | Yes | Design family, density, and accent color |
| `blocks` | Yes | Ordered semantic document content |

## Supported blocks

### `title`

Displayed title text and an optional subtitle.

### `heading`

A semantic heading with level 1, 2, or 3.

### `paragraph`

A non-empty prose paragraph.

### `bulletList`

One or more non-empty list items. The composition engine will translate this
block into a native Word numbering definition rather than Unicode bullet text.

### `pageBreak`

An explicit page break with no additional properties.

## Validation layers

1. JSON deserialization rejects malformed JSON, unknown fields, and unknown
   block types.
2. JSON Schema defines the portable external contract.
3. Semantic validation enforces version support, design values, text content,
   heading ranges, and non-empty lists.

See [`examples/valid-professional-brief.json`](../examples/valid-professional-brief.json)
for a complete v1 example.
