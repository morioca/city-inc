Implement code that satisfies the specifications in $ARGUMENTS using TDD.

1. Verify $ARGUMENTS contains test cases. If not, stop and tell the user.

2. Red Phase — Write failing tests:
   - From the test cases, create only compilable type declarations and public method signatures (no implementation)
   - Run tests via the test-runner agent; confirm they fail
   - Repeat until all tests compile and fail as expected, then commit to Git

3. Green Phase — Make tests pass:
   - Implement production code to pass the tests
   - Run tests via the test-runner agent; confirm all pass
   - Repeat until all tests pass, then commit to Git

4. Refactor Phase:
   - Refactor with KISS and SOLID principles in mind
   - Run tests via the test-runner agent; confirm all still pass
   - Commit to Git if there are any changes

5. If new functionality was added, update `/specs/deliverables.md`.

For test execution, use the Task tool with `subagent_type="test-runner"`, providing the affected class names, namespaces, and assembly names.
