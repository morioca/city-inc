# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

City Inc (仮) is an iOS city management simulation game in the design/planning phase. The repository currently contains game design documentation only - no implementation code exists yet.

**Game Concept:** Players become mayors of fictional cities, making policy decisions and experiencing the consequences through turn-based gameplay (monthly turns). Target audience is urban-dwelling adults (20-50) interested in politics/governance.

## Project Status

This is a documentation-only repository. Implementation has not started.

## Documentation Structure

- `docs/01-concept.md` - Game concept, vision, core experience, target personas
- `docs/02-overview.md` - Game overview (basic info, world setting, rules, success metrics)
- `docs/03-gdd.md` - Game Design Document (core loop, systems, UI/UX, scenarios, goals)
- `docs/04-tdd.md` - Technical Design Document (tech stack, architecture, implementation roadmap)
- `docs/coffee-inc-2-analysis/` - Reference game (Coffee Inc 2) analysis

## Key Design Decisions

**Core Loop:** Observe metrics → Decide budget allocation → Execute policies → Review results (30 sec - 3 min per turn)

**6 Policy Categories:** Welfare/Healthcare, Education, Industry, Infrastructure, Safety, Tourism/Culture

**Progression System:** Town mayor → City mayor → Prefecture governor (no traditional tutorial)

**UI Philosophy:** "Mayor's Desk" metaphor - dark navy/gold color scheme, data-driven interface with period-over-period comparisons

**Design Principle:** Support multiple political ideologies (liberal/conservative/tech-forward/tradition-focused) - no single "correct" playstyle

## Implementation Roadmap

When development begins, follow the 3-phase approach in `docs/04-tdd.md`:
1. **Phase 1 (MVP):** Core loop with 1 scenario, basic budget/policy systems
2. **Phase 2:** Delegation system, parliament, events, detailed statistics
3. **Phase 3:** Additional scenarios, progression system, achievements
