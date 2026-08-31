# BetterDOCX

BetterDOCX is a local, Word-native document-authoring engine for agent-driven
DOCX creation on Windows.

The target experience is simple:

> Create a Word document for me that explains our Zero Trust modernization
> strategy to executive leadership.

The local agent interprets the request, composes the document with native Word
structures, renders it through installed Microsoft Word, reviews the rendered
pages, corrects defects, and returns an editable `.docx`.

The initial architecture and implementation roadmap are documented in
[DESIGN-PLAN.md](./DESIGN-PLAN.md).

## Status

Foundation sprint. The repository contains the .NET solution boundaries,
Windows/Word/Poppler environment preflight, automated tests, and CI.

## Core principles

- Microsoft Word is the authoritative renderer.
- .NET and the Open XML SDK create native DOCX structure.
- A reusable document grammar replaces an endless template catalog.
- The agent owns content, design judgment, and the revision loop.
- The engine owns deterministic document construction and validation.
- The first release is local and Windows-first.

## Developer quick start

```powershell
dotnet restore BetterDOCX.sln
dotnet build BetterDOCX.sln --configuration Release --no-restore
dotnet test BetterDOCX.sln --configuration Release --no-build
dotnet run --project .\src\BetterDOCX.Cli -- preflight
```

Use `--json` for a machine-readable preflight result. Use
`--skip-word-launch` when you want registration checks without starting and
closing Microsoft Word.
