Split the specification in $ARGUMENTS into separate domain and presentation specs.

1. Read the spec file at $ARGUMENTS. Verify it contains both Domain (Model/System) and Presentation (UI/MonoBehaviour) sections. If it only covers one layer, stop and tell the user.

2. Create the domain spec `<original-name>-domain.md`:
   - Copy Requirements and Specifications sections verbatim (shared context)
   - From Design, include only Domain-related subsections (Models, Systems, business logic)
   - From Test Cases, include only Edit Mode tests
   - From File Structure, include only domain-related files

3. Create the presentation spec `<original-name>-presentation.md`:
   - Copy Requirements and Specifications sections verbatim (shared context)
   - From Design, include only Presentation-related subsections (Presenters, UI layout, scene setup)
   - From Test Cases, include only Play Mode tests
   - From File Structure, include only presentation-related files
   - Add a note at the top: "Depends on: `<domain-spec-filename>`"

4. Delete the original spec file.

5. Commit to Git.

After completing, propose running `/implement-code` on the domain spec first.
