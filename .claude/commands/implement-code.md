Implement code that satisfies the specifications in $ARGUMENTS using TDD.

@.claude/rules/01-coding.md
@.claude/rules/10-unity-project.md

1. Verify $ARGUMENTS contains test cases. If not, stop and tell the user.

2. Red Phase — Write failing tests:
   - Create production code stubs — only compilable type declarations and public method signatures (no implementation)
   - Delegate test code writing to the test-writer agent, providing the spec file path
   - Run tests via the test-runner agent; confirm they fail
   - Repeat until all tests compile and fail as expected, then commit to Git

3. Green Phase — Make tests pass:
   - Implement all production code listed in the spec (new files and modified files)
   - Run tests via the test-runner agent; confirm all pass
   - Repeat until all tests pass, then commit to Git

4. Refactor Phase:
   - Refactor with KISS and SOLID principles in mind
   - Run tests via the test-runner agent; confirm all still pass
   - Commit to Git if there are any changes

5. If new functionality was added, update `/specs/deliverables.md`.

For test writing, use the Task tool with `subagent_type="test-writer"`, providing the spec file path.
For test execution, use the Task tool with `subagent_type="test-runner"`, providing all test class names, namespaces, and assembly names listed in the spec.
