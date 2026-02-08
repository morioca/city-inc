# City Inc — Development Guidelines

## Mandatory Workflow

**IMPORTANT:** Do not write code directly. Always execute these commands in strict order:

1. `/create-doc` — Create a specification document from the conversation
2. `/create-test-cases` — Derive test cases from the specification
3. `/implement-code` — Implement code using TDD

Never skip any step. Never jump straight into coding.

## Project Context

- **Type:** iOS city management simulation (turn-based, monthly turns)
- **Player role:** Mayor making policy decisions for fictional cities
- **Target audience:** Urban-dwelling adults (20–50) interested in politics/governance
- **Current phase:** Design/planning with incremental implementation

## Rule Files

Rule files in `.claude/rules/` auto-load when editing matching file types. Do not duplicate their content here.

| File | Scope | Applies to |
|------|-------|------------|
| `01-coding.md` | Coding conventions | `**/*.cs` |
| `02-testing.md` | Testing conventions | `**/Tests/**/*.cs` |
| `10-unity-project.md` | Project and assembly structure | `**/*.cs` |
| `11-unity-documentation.md` | Official documentation references | `**/*.cs` |
| `12-unity.yaml` | Unity YAML file editing | `**/*.{meta,asset,prefab,unity}` |

## Game Design Documents

- `docs/01-concept.md` — Concept, vision, core experience, target personas
- `docs/02-overview.md` — Overview (world setting, rules, success metrics)
- `docs/03-gdd.md` — Game Design Document (core loop, systems, UI/UX, scenarios)
- `docs/04-tdd.md` — Technical Design Document (tech stack, architecture, roadmap)
- `docs/coffee-inc-2-analysis/` — Reference game analysis (Coffee Inc 2)
