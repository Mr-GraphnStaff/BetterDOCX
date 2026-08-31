# Contributing to BetterDOCX

BetterDOCX is in its foundation stage. Contributions should preserve its core
goal: deterministic, Word-native document creation verified by Microsoft Word.

## Development workflow

1. Create a focused branch from `main`.
2. Keep changes scoped to one issue or coherent capability.
3. Add or update tests for behavioral changes.
4. Run `dotnet build BetterDOCX.sln --configuration Release`.
5. Run `dotnet test BetterDOCX.sln --configuration Release --no-build`.
6. Open a pull request describing the user value, implementation, validation,
   and known limitations.

## Engineering expectations

- Treat warnings as errors.
- Keep Word automation isolated in `BetterDOCX.Word`.
- Keep document models independent of Open XML implementation details.
- Do not introduce direct-formatting shortcuts where a native Word style or
  numbering definition is appropriate.
- Never claim that a document passed visual QA unless every Word-rendered page
  was inspected.
- Do not commit generated documents, PDFs, page images, secrets, or local build
  artifacts.

## Commit style

Use concise, imperative commit subjects, such as:

```text
Add Windows preflight diagnostics
Implement native bullet numbering
Validate Word-rendered page count
```
