---
paths:
  - "**/Tests/**/*.cs"
---

# Testing Guidelines

## Placement

- Editor extension code (`Editor/`) → tests under `Tests/Editor/` (Edit Mode tests)
- Runtime code (`Runtime/`) → tests under `Tests/Runtime/` (Play Mode tests)
- Mirror the production code directory structure; one test class per production class
- Test doubles → `Tests/Editor/TestDoubles/` or `Tests/Runtime/TestDoubles/`
- Test scenes → `Tests/Scenes/`

## Naming

- Test assembly: target assembly name + `.Tests`
- Namespace: same as the production code under test
- Test class: target class name + `Test` — e.g., `CharacterControllerTest`
- Test method: `TargetMethodName_Condition_ExpectedResult` — e.g., `TakeDamage_WhenHealthIsZero_CharacterDies`
- Object under test: `sut`; actual value: `actual`; expected value: `expected`
- Test double variables: prefix with `stub`, `spy`, `dummy`, `fake`, or `mock` per xUnit Test Patterns

## Writing Tests

Use the NUnit3-based Unity Test Framework.
Reference: https://docs.unity3d.com/Packages/com.unity.test-framework@1.4/manual/index.html

### General Rules

- Add `[TestFixture]` to every test class
- Structure each test with Arrange / Act / Assert — separate sections with a blank line; no section comments
- One `Assert` per test method
- Use `Assert.That` (constraint model) for all assertions; never specify the `message` argument
  Reference: https://docs.nunit.org/api/NUnit.Framework.Constraints.html
- No control flow in tests: never use `if`, `switch`, `for`, `foreach`, or `? :`
- No XML documentation comments in test code
- Each test must run independently; never depend on another test's result
- Prefer real production code; use test doubles only when the dependency cannot be used directly (e.g., external systems, non-deterministic behavior)

### Parameterization

- Use `TestCase`, `TestCaseSource`, `Values`, or `ValueSource` when multiple inputs share the same Act and Assert
- Use `ParameterizedIgnore` to exclude specific combinations

### Instantiation

- Use creation methods for all test object setup — e.g., `private GameObject CreateSystemUnderTestObject()`
- Even when holding a field reference for `TearDown`, always call the creation method in the test body

### Unity-Specific

- Add `[CreateScene]` to any test that creates a `GameObject` (omit if `[LoadScene]` is already present)
- Do not use `LogAssert`; create a Spy to capture and assert on log output
- In async tests, use `yield return null` to wait one frame; never use `Delay` or `Wait`
- To verify an async method throws, use try-catch instead of the `Throws` constraint (Unity Test Framework limitation):

```csharp
try
{
    await Foo.Bar(-1);
    Assert.Fail("Expected an exception to be thrown");
}
catch (ArgumentException expectedException)
{
    Assert.That(expectedException.Message, Is.EqualTo("Semper Paratus!"));
}
```

## Running Tests

1. Call `RefreshAssets` to compile
2. Call `GetCompileLogs` — abort if any compile errors exist
3. Run tests:
   - Code under `Editor/` → `RunEditModeTests` MCP tool
   - Code under `Runtime/` → `RunPlayModeTests` MCP tool
4. Always filter by assembly, group, or test name to minimize execution scope

## Interpreting Results

- Passed: Continue
- Failed: Investigate and fix (see procedure below)
- Inconclusive: Treat as failure
- Skipped: Ignore

Failure resolution procedure:

1. Read the error message to identify the cause
2. Compare expected vs. actual values
3. Fix the issue and re-run the failing test
4. If still failing, review both the test and the implementation
5. If failing a second time, summarize the failure details and ask the user for guidance
