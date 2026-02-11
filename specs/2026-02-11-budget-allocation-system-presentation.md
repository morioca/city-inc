# Budget Allocation System — Phase 1 (Presentation)

> **Dependency:** This spec depends on the domain layer defined in `2026-02-11-budget-allocation-system-domain.md`. The domain spec must be implemented first, as the presentation layer relies on `BudgetAllocation`, `GameState.CurrentAllocation`, and `BudgetValidator`.

## Requirements

This spec covers the **presentation layer** (UI, presenters, and integration) for the budget allocation feature. It implements the screens and interaction logic that allow the player to view and modify budget allocations through the game UI.

### Functional Requirements

**Budget Allocation Screen Access**
- Add a "Budget" button on the main game screen (positioned below the status bar)
- Tapping the Budget button opens a full-screen budget allocation modal
- The modal displays the budget allocation UI
- A "Close" or "Back" button returns to the main game screen

**Budget Display**
- Show total available budget at the top (e.g., "Total Budget: ¥100,000,000")
- Display remaining unallocated budget (e.g., "Remaining: ¥20,000,000")
- For each category, show:
  - Category name
  - Current allocation amount (in yen)
  - Current allocation percentage (0–100%)

**Allocation Input**
- Each category has a slider or input field to adjust allocation
- Player can allocate any amount from ¥0 to total budget per category
- Real-time validation: total allocation across all categories must not exceed total budget
- Display visual feedback when allocation exceeds budget (e.g., red text for "Remaining")

**Confirm Allocation**
- "Confirm" button at the bottom of the screen
- Button is disabled when total allocation ≠ total budget
- Tapping "Confirm" saves the allocation and closes the modal
- Allocation persists until the next budget planning cycle (future: annual budget revision)

**Initial State**
- On first access, all categories default to equal distribution (¥16,666,666 each for ¥100M budget)
- On subsequent access within the same budget cycle, display the most recent allocation

### Non-Functional Requirements

**Phase 1 Scope Limitations**
- Allocation does NOT yet affect city metrics (population, budget, approval rating remain static)
- No feedback on allocation quality or citizen reactions
- No annual budget revision cycle (allocation persists indefinitely)
- No budget income/expense simulation (total budget remains constant at ¥100M)
- No policy execution within categories (just allocation amounts)

These limitations will be addressed in subsequent phases.

**UI/UX Principles**
- Per GDD §5-1, maintain "subtraction design" — minimal decorative elements
- Use the established color scheme: dark navy (main), gold (accent), off-white (background)
- Slider controls should be large enough for comfortable thumb operation (portrait mode)
- Numeric values display thousand separators (e.g., "100,000,000" not "100000000")

## Design

### Presentation Layer (Presentation.Budget)

**New: BudgetAllocationPresenter (MonoBehaviour)**

Manages the budget allocation UI:

```csharp
public class BudgetAllocationPresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text totalBudgetLabel;
    [SerializeField] private TMP_Text remainingBudgetLabel;

    [SerializeField] private Slider welfareSlider;
    [SerializeField] private TMP_Text welfareAmountLabel;
    [SerializeField] private TMP_Text welfarePercentLabel;

    // ... (similar fields for other 5 categories)

    [SerializeField] private Button confirmButton;
    [SerializeField] private Button closeButton;

    private GameState currentGameState;
    private BudgetValidator validator;
    private BudgetAllocation workingAllocation;

    public void Initialize(GameState gameState, BudgetValidator validator)
    {
        this.currentGameState = gameState;
        this.validator = validator;
        this.workingAllocation = gameState.CurrentAllocation;

        UpdateAllLabels();
        SetupSliderListeners();
        ValidateAndUpdateConfirmButton();
    }

    private void OnSliderValueChanged(/* ... */)
    {
        // Update workingAllocation based on slider
        // Update amount/percent labels
        // Validate and update confirm button state
    }

    public void OnConfirmButtonClicked()
    {
        // Emit event or callback with workingAllocation
        // Close modal
    }
}
```

**New: BudgetScreen (GameObject)**

Full-screen modal containing:
- Panel with dark semi-transparent background
- Header: "Budget Allocation" title, Close button
- Content area:
  - Total budget label
  - Remaining budget label
  - Six category rows (each with slider + amount label + percent label)
- Footer: Confirm button (disabled state when invalid)

**Modified: MainGameSceneInitializer**

Add instantiation of Budget button and BudgetScreen modal:
- Create "Budget" button below status bar
- Create BudgetScreen GameObject (initially inactive)
- Wire button click to show modal
- Initialize BudgetAllocationPresenter with current GameState

### UI Layout

**Main Game Screen**
```
┌─────────────────────────────────┐
│ SafeAreaPanel                   │
│ ┌─────────────────────────────┐ │
│ │ StatusBarPanel              │ │
│ │ 人口 50,000人               │ │
│ │ 財政 100,000,000円          │ │
│ │ 支持率 60%                  │ │
│ └─────────────────────────────┘ │
│ ┌─────────────────────────────┐ │
│ │ [Budget] Button             │ │ ← New
│ └─────────────────────────────┘ │
│                                 │
│ (Date and Next Month button     │
│  remain below)                  │
└─────────────────────────────────┘
```

**Budget Allocation Screen (Modal)**
```
┌─────────────────────────────────┐
│ Budget Allocation         [X]   │
├─────────────────────────────────┤
│ Total Budget: ¥100,000,000      │
│ Remaining:    ¥0                │
├─────────────────────────────────┤
│ 福祉・医療                       │
│ ━━━━━━━━━━ slider              │
│ ¥16,666,666 (16.7%)             │
├─────────────────────────────────┤
│ 教育・子育て                     │
│ ━━━━━━━━━━ slider              │
│ ¥16,666,666 (16.7%)             │
├─────────────────────────────────┤
│ (... 4 more categories)         │
├─────────────────────────────────┤
│        [Confirm]                │
└─────────────────────────────────┘
```

### Domain Dependencies

This presentation layer depends on the following domain types (defined in the domain spec):

- `BudgetAllocation` — Immutable value object with six category properties and `TotalAllocated`
- `BudgetAllocation.EqualDistribution(long)` — Factory method for default allocation
- `GameState.CurrentAllocation` — Property providing the current allocation
- `BudgetValidator.IsValid(BudgetAllocation, long)` — Validation logic
- `BudgetValidator.GetRemaining(BudgetAllocation, long)` — Remaining budget calculation

### Integration Points

**GameState Evolution**
- `GameState.CreateInitial()` now includes `BudgetAllocation.EqualDistribution(100000000L)`
- Future: When turn advances, allocation persists (no changes to TurnManager yet)

**Future Phases**
- Phase 2: Budget allocation affects city metrics via policy effects
- Phase 3: Annual budget revision cycle (changes to TurnManager)
- Phase 4: Income/expense simulation (budget grows/shrinks based on policies)

### File Structure

New files:
```
Assets/Scripts/Presentation/Budget/BudgetAllocationPresenter.cs
Assets/Scripts/Presentation/Budget/BudgetScreen.prefab

Assets/Tests/PlayMode/Presentation/Budget/BudgetAllocationPresenterTest.cs
Assets/Tests/PlayMode/Presentation/Budget/BudgetScreenIntegrationTest.cs
```

Modified files:
```
Assets/Scripts/Presentation/MainGameSceneInitializer.cs
```

## Test Cases

### Play Mode Tests — Presentation.Budget.BudgetAllocationPresenter

#### BudgetAllocationPresenter_Initialize_WhenCalledWithGameState_DisplaysTotalBudget
- Create GameState with Budget = 100M
- Call Initialize()
- Verify totalBudgetLabel.text contains "100,000,000" with proper formatting

#### BudgetAllocationPresenter_Initialize_WhenCalledWithEqualDistribution_DisplaysEqualAllocationOnSliders
- Create GameState with equal distribution (16666666 per category)
- Call Initialize()
- Verify all six sliders display correct initial values
- Verify all six amount labels show "16,666,666"
- Verify all six percent labels show approximately "16.7%"

#### BudgetAllocationPresenter_Initialize_WhenCalledWithValidAllocation_ShowsZeroRemaining
- Create GameState with allocation totaling exactly 100M
- Call Initialize()
- Verify remainingBudgetLabel.text shows "0"
- Verify confirmButton.interactable = true

#### BudgetAllocationPresenter_Initialize_WhenCalledWithInvalidAllocation_ShowsNonZeroRemaining
- Create GameState with allocation totaling 90M
- Call Initialize()
- Verify remainingBudgetLabel.text shows "10,000,000"
- Verify confirmButton.interactable = false

#### BudgetAllocationPresenter_OnSliderValueChanged_WhenWelfareSliderChanges_UpdatesWelfareLabels
- Initialize presenter
- Change welfare slider value to 20M
- Verify welfareAmountLabel shows "20,000,000"
- Verify welfarePercentLabel shows "20%"

#### BudgetAllocationPresenter_OnSliderValueChanged_WhenSliderChanges_UpdatesRemainingBudget
- Initialize presenter with equal distribution
- Change welfare slider from 16666666 to 30000000
- Verify remaining budget decreases by (30000000 - 16666666)
- Verify remainingBudgetLabel updates in real-time

#### BudgetAllocationPresenter_OnSliderValueChanged_WhenTotalExceedsBudget_DisablesConfirmButton
- Initialize presenter
- Adjust sliders so total > 100M
- Verify confirmButton.interactable = false
- Verify remainingBudgetLabel shows negative value with visual feedback (e.g., red text)

#### BudgetAllocationPresenter_OnSliderValueChanged_WhenTotalMatchesBudget_EnablesConfirmButton
- Initialize presenter with invalid allocation
- Adjust sliders so total = 100M exactly
- Verify confirmButton.interactable = true
- Verify remainingBudgetLabel shows "0"

#### BudgetAllocationPresenter_OnSliderValueChanged_WhenMultipleSlidersAdjusted_CalculatesCorrectTotal
- Initialize presenter
- Adjust welfare to 25M, education to 30M, industry to 15M, infrastructure to 20M, disaster to 5M, tourism to 5M
- Verify TotalAllocated = 100M
- Verify confirmButton.interactable = true

#### BudgetAllocationPresenter_OnSliderValueChanged_WhenSliderSetToZero_AcceptsZeroAllocation
- Initialize presenter
- Set welfare slider to 0
- Verify welfareAmountLabel shows "0"
- Verify welfarePercentLabel shows "0%"
- Verify no errors or validation issues (zero is valid)

#### BudgetAllocationPresenter_OnSliderValueChanged_WhenSliderSetToMaxBudget_AcceptsFullAllocation
- Initialize presenter
- Set welfare slider to 100M, all others to 0
- Verify welfareAmountLabel shows "100,000,000"
- Verify welfarePercentLabel shows "100%"
- Verify confirmButton.interactable = true

#### BudgetAllocationPresenter_OnConfirmButtonClicked_WhenAllocationIsValid_EmitsAllocationEvent
- Initialize presenter with valid allocation (100M total)
- Click confirm button
- Verify event/callback is invoked with current workingAllocation
- Note: Event system to be defined during implementation

#### BudgetAllocationPresenter_OnConfirmButtonClicked_WhenAllocationIsValid_ClosesModal
- Initialize presenter with valid allocation
- Click confirm button
- Verify modal GameObject becomes inactive or is destroyed

#### BudgetAllocationPresenter_Initialize_WhenCalledMultipleTimes_DisplaysMostRecentAllocation
- Create GameState with custom allocation (30M, 20M, 15M, 10M, 15M, 10M)
- Call Initialize()
- Verify sliders/labels reflect the custom allocation, not equal distribution

#### BudgetAllocationPresenter_Initialize_WhenCalledWithZeroBudget_DisplaysZeroValues
- Create GameState with Budget = 0
- Call Initialize()
- Verify all sliders at 0
- Verify confirmButton.interactable = true (0 total = 0 budget is valid)

### Play Mode Tests — Presentation.Budget.BudgetScreenIntegration

#### BudgetScreen_BudgetButton_WhenClicked_OpensBudgetModal
- Load main game scene
- Find and click "Budget" button
- Verify BudgetScreen GameObject becomes active
- Verify BudgetAllocationPresenter is initialized

#### BudgetScreen_CloseButton_WhenClicked_ClosesModalAndReturnsToMainScreen
- Open budget modal
- Click close button
- Verify BudgetScreen GameObject becomes inactive
- Verify main game screen remains visible

#### BudgetScreen_ConfirmButton_WhenClicked_SavesAllocationAndClosesModal
- Open budget modal
- Adjust allocation to valid state (100M total)
- Click confirm button
- Verify modal closes
- Verify GameState.CurrentAllocation is updated with new values

#### BudgetScreen_ConfirmButton_WhenAllocationInvalid_RemainsDisabled
- Open budget modal
- Adjust allocation to invalid state (90M total)
- Attempt to click confirm button
- Verify button remains disabled and unresponsive
- Verify modal stays open

#### BudgetScreen_ReopenAfterConfirm_WhenOpened_DisplaysPreviousAllocation
- Open budget modal
- Set custom allocation (e.g., 40M welfare, 60M education, 0 others)
- Click confirm
- Reopen budget modal
- Verify sliders/labels show the previously confirmed allocation (40M, 60M, 0, 0, 0, 0)

#### BudgetScreen_ReopenAfterClose_WhenOpenedWithoutConfirm_DisplaysUnchangedAllocation
- Open budget modal
- Adjust sliders (but don't confirm)
- Click close button
- Reopen budget modal
- Verify sliders show original allocation (changes discarded)

#### BudgetScreen_Layout_WhenRendered_DisplaysAllSixCategories
- Open budget modal
- Verify six category rows are visible
- Verify each row has: category name label, slider, amount label, percent label
- Verify categories appear in correct order (Welfare, Education, Industry, Infrastructure, Disaster, Tourism)

#### BudgetScreen_Layout_WhenRendered_DisplaysTotalAndRemainingLabels
- Open budget modal
- Verify "Total Budget" label is visible at top
- Verify "Remaining" label is visible below total

#### BudgetScreen_Layout_WhenRendered_DisplaysConfirmAndCloseButtons
- Open budget modal
- Verify "Confirm" button exists at bottom
- Verify "Close" button exists in header

#### BudgetScreen_Slider_WhenDragged_UpdatesInRealTime
- Open budget modal
- Drag welfare slider to new position
- Verify amount label updates immediately (no delay)
- Verify percent label updates immediately
- Verify remaining budget label updates immediately

#### BudgetScreen_NumericFormatting_WhenDisplayed_ShowsThousandSeparators
- Open budget modal with 100M budget
- Verify totalBudgetLabel shows "100,000,000" (not "100000000")
- Verify all category amount labels use thousand separators

#### BudgetScreen_PercentageDisplay_WhenSliderAt50Percent_Shows50Percent
- Open budget modal
- Set welfare slider to 50M
- Verify welfarePercentLabel shows "50%" or "50.0%"

#### BudgetScreen_PercentageDisplay_WhenSliderAt0_Shows0Percent
- Open budget modal
- Set welfare slider to 0
- Verify welfarePercentLabel shows "0%"

#### BudgetScreen_PercentageDisplay_WhenSliderAtFractionalPercent_ShowsDecimalPercent
- Open budget modal
- Set welfare slider to 16666666 (16.666666%)
- Verify welfarePercentLabel shows "16.7%" or "16.67%" (reasonable rounding)

#### BudgetScreen_RemainingBudget_WhenPositive_DisplaysNormalStyle
- Open budget modal
- Adjust sliders so remaining = 20M (positive)
- Verify remainingBudgetLabel has normal text color (not red)

#### BudgetScreen_RemainingBudget_WhenNegative_DisplaysWarningStyle
- Open budget modal
- Adjust sliders so total > 100M (remaining negative)
- Verify remainingBudgetLabel has warning/error styling (e.g., red text color)

#### BudgetScreen_RemainingBudget_WhenZero_DisplaysNormalStyle
- Open budget modal
- Adjust sliders so remaining = 0
- Verify remainingBudgetLabel has normal text color
- Verify confirm button enabled

### Play Mode Tests — MainGameSceneIntegration

#### MainGameScene_OnLoad_WhenInitialized_DisplaysBudgetButton
- Load main game scene
- Verify "Budget" button exists below status bar
- Verify button is interactable

#### MainGameScene_BudgetButton_WhenClicked_MaintainsGameState
- Load main game scene with GameState (population 50000, budget 100M, approval 60%)
- Click budget button
- Open modal and confirm allocation
- Return to main screen
- Verify status bar still shows population 50000, budget 100M, approval 60% (unchanged in Phase 1)

#### MainGameScene_GameStateInitialization_WhenSceneLoads_IncludesEqualDistribution
- Load main game scene for first time
- Access GameState
- Verify CurrentAllocation is set to equal distribution
- Verify all six categories ≈ 16.67M each
