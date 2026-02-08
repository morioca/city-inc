## Test Review Policy

### Purpose

Tests exist to **detect future regressions quickly and reliably**.
Tests that do not serve this goal should be removed or consolidated.

---

### Keep — if any of the following apply:

- Verifies behavior likely to break during future changes
- Catches failures that would not be immediately obvious to humans
- Expresses an invariant or critical initial condition that must hold
- Produces a clear failure message pinpointing what broke

---

### Remove or consolidate — if any of the following apply:

- Confirms a value is merely stored or returned unchanged
  (constructor, getter, or trivial factory pass-through)
- Validates something immediately visible without a test
  (missing UI element, label not rendering)
- Duplicates detection of the same failure as another test
  (separate tests per property when one combined test suffices)
- Only asserts that something "does not change"
  (blocks legitimate future changes without safety benefit)
- Depends on implementation details rather than observable behavior

---

### Granularity Principle

- One test = one behavior
- Merge checks for tightly related simple behavior into one test
- Prefer high detection power per test over a large number of tests

---

### Decision Prompt

For every test, ask:

> **What regression would escape detection if this test were deleted?**
> Can another existing test already catch it?

If the answer is unclear or "nothing meaningful," the test is a deletion candidate.

---

### Heuristics

| Situation | Action |
|---|---|
| Bug obvious to human at a glance | Delete |
| Code changes frequently or has wide impact | Keep |
| Simple but critical default value | Keep (one test) |
| Multiple tests checking same property | Consolidate to one |
