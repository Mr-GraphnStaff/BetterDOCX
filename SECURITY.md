# Security Policy

## Supported versions

BetterDOCX is pre-release software. Security fixes are applied to the latest
revision on `main`.

## Reporting a vulnerability

Do not open a public issue for a suspected vulnerability involving document
processing, local command execution, Office automation, or sensitive file
exposure. Use GitHub's private vulnerability reporting feature for this
repository.

Include reproduction steps, affected revision, impact, and any suggested
mitigation. Avoid attaching documents containing real confidential data.

## Security boundaries

- Treat all input documents and document specifications as untrusted.
- Never execute macros from input documents.
- Word automation must run with prompts suppressed and documents opened with
  the minimum required access.
- Generated and rendered artifacts remain local unless explicitly published.
- Temporary artifacts must not be committed to source control.
