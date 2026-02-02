# Development Guidelines

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

City Inc (仮) is an iOS city management simulation game in the design/planning phase. The repository currently contains game design documentation only - no implementation code exists yet.


## 行動規範

Claude Code の行動規範、開発の進め方に関するグランドルールは、次のファイルを参照する。

- @.claude/rules/00-code-of-conduct.md


## コーディングおよびテストのガイドライン

次のファイルを参照する。

- @.claude/rules/01-coding.md
- @.claude/rules/02-testing.md


## Unityプロジェクトの構造およびガイドライン

必要に応じて次のファイルを参照する。

- @.claude/rules/10-unity-project.md
- @.claude/rules/11-unity-documentation.md
- @.claude/rules/12-unity-yaml.md


## Documentation Structure

- `docs/01-concept.md` - Game concept, vision, core experience, target personas
- `docs/02-overview.md` - Game overview (basic info, world setting, rules, success metrics)
- `docs/03-gdd.md` - Game Design Document (core loop, systems, UI/UX, scenarios, goals)
- `docs/04-tdd.md` - Technical Design Document (tech stack, architecture, implementation roadmap)
- `docs/coffee-inc-2-analysis/` - Reference game (Coffee Inc 2) analysis


## Key Design Decisions

**Game Concept:** Players become mayors of fictional cities, making policy decisions and experiencing the consequences through turn-based gameplay (monthly turns). Target audience is urban-dwelling adults (20-50) interested in politics/governance.

## Implementation Roadmap

When development begins, follow the 3-phase approach in `docs/04-tdd.md`:
1. **Phase 1 (MVP):** Core loop with 1 scenario, basic budget/policy systems
2. **Phase 2:** Delegation system, parliament, events, detailed statistics
3. **Phase 3:** Additional scenarios, progression system, achievements
