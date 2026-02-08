Derive test cases to verify the specifications in $ARGUMENTS, and append them to that file.

@.claude/rules/02-test-writing.md

1. Read $ARGUMENTS. If the specifications are unclear, stop and tell the user what is missing.
2. Select the public classes and methods under test, starting from those with the fewest dependencies.
3. Choose appropriate testing techniques (e.g., equivalence partitioning, boundary value analysis, state transition testing).
4. Write test cases in natural language:
   - Do not assign sequential IDs
   - State what is being verified; remove any test case that cannot be verified
5. Repeat steps 2–4 until all classes under test are covered.
6. Append the test cases to $ARGUMENTS.
7. Commit to Git.

After completing, propose running `/implement-code`.
