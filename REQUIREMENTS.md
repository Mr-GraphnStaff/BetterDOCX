# BetterDOCX Requirements

## Product objective

A user can describe a Word document in ordinary language and receive a polished,
editable DOCX that was generated with native Word structures and verified by
Microsoft Word on the local Windows machine.

## Version-one functional requirements

1. Accept a versioned, machine-readable document specification.
2. Compose native DOCX documents with the Open XML SDK.
3. Support title blocks, headings, paragraphs, lists, callouts, basic tables,
   page breaks, headers, footers, and page numbers.
4. Apply a reusable professional design system without requiring a finished
   template for each document type.
5. Open generated documents with installed Microsoft Word and export PDF.
6. Rasterize every Word-rendered PDF page for agent inspection.
7. Validate package structure, semantic styles, numbering, fonts, tables,
   sections, headers, and footers.
8. Return machine-readable results for generation, rendering, and validation.
9. Fail explicitly when native Word rendering or visual inspection did not run.

## Nonfunctional requirements

- Windows-first and local-first.
- Deterministic generation for the same specification and engine version.
- No macros in generated documents.
- No unattended server-side Word automation.
- Word operations are serialized and always cleaned up.
- Inputs are treated as untrusted.
- Generated and temporary artifacts are excluded from source control.
- The solution builds with .NET 8 and treats warnings as errors.
- Core composition logic remains independent of the chat client.

## Deferred requirements

- Remote hosting or Azure deployment
- Public MCP access
- Codex plugin packaging
- Google Docs integration
- Track Changes and comments
- Complex floating layouts
- Corporate template packs
