# ADR 0001: Local Windows and Microsoft Word Architecture

- Status: Accepted
- Date: 2026-08-31

## Context

BetterDOCX must create editable Word documents and verify the exact layout a
Windows Word user will see. Open XML libraries can construct native DOCX
packages, but they do not provide Word's authoritative pagination and layout
engine. Remote services would add authentication, storage, deployment, and
rendering differences before the core product is proven.

## Decision

Version one will run locally on Windows:

- .NET 8 hosts the engine and command-line interface.
- The Open XML SDK constructs and validates DOCX packages.
- Installed Microsoft Word opens generated documents and exports PDF.
- Poppler converts Word-exported PDF pages to images for agent review.
- The local Codex agent performs document planning and visual judgment.
- Word automation is isolated, serialized, and treated as an interactive-user
  desktop capability rather than a server workload.

## Consequences

### Benefits

- Native Word rendering is available immediately.
- No cloud infrastructure is required for the first proof.
- The engine remains directly observable and debuggable.
- User documents do not need to leave the local machine.

### Costs

- Full rendering requires Windows and installed Microsoft Word.
- Word automation cannot be used as a horizontally scaled server component.
- CI can validate build and deterministic logic but cannot claim visual fidelity
  without a licensed Word environment and page inspection.

## Revisit when

- The local vertical slice consistently produces high-quality documents.
- Another MCP-capable client needs access to BetterDOCX.
- A remote control plane offers concrete value beyond local execution.
- A supported remote renderer can meet the same fidelity requirements.
