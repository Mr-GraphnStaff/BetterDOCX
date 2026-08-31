# Version-One Acceptance Criteria

BetterDOCX version one is accepted when a local agent can take a natural-language
request and complete the following workflow without technical input from the
user:

1. Produce a valid versioned document specification.
2. Generate an editable DOCX with native Word styles and numbering.
3. Open the DOCX in Microsoft Word without a repair warning.
4. Export the document to PDF through Microsoft Word.
5. Record the native Word page count.
6. Rasterize every page into a reviewable image.
7. Inspect every rendered page and correct visible defects.
8. Pass structural and portability validation.
9. Return the final DOCX with an honest QA result.

## Structural pass conditions

- Required package parts and relationships resolve.
- Headings use semantic Word heading styles.
- Lists use native numbering definitions.
- Fonts are explicit and have defined fallbacks.
- Tables use consistent explicit geometry.
- Headers, footers, sections, and page fields are valid.
- Direct formatting is limited to documented inline exceptions.

## Visual pass conditions

- No content is clipped, overlapping, or missing.
- No heading is stranded from its following content.
- Wrapped list items align correctly.
- Tables fit their page and remain readable.
- Page density and whitespace are intentional.
- Headers, footers, and page numbers are positioned correctly.
- Every delivered page has been inspected from the latest Word render.

## Foundation milestone pass conditions

- The solution restores and builds in Release with zero warnings.
- Automated tests pass locally and in CI.
- The local preflight verifies .NET, Windows, Word registration, actual Word
  automation startup/shutdown, and Poppler.
- CI runs on every pull request targeting `main`.
- Repository contribution and security expectations are documented.
