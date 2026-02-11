# Budget Allocation System — Phase 1

## Requirements

The main game screen currently displays city status (population, budget, approval rating) and allows advancing turns via "Next Month" button. However, the player has no way to make decisions or affect the city's trajectory. Per GDD §3-2, budget allocation is the primary decision-making mechanism where the mayor distributes limited financial resources across six policy categories to influence city development.

Without a budget allocation system:
- The core gameplay loop (Observe → Decide → Execute → Review) remains incomplete
- Clicking "Next Month" has no strategic meaning
- The player cannot express their policy priorities or playstyle
- City metrics remain static and unchanging

This feature implements the decision-making layer that transforms the current passive observer experience into active mayoral governance.

## Specifications

### Functional Requirements

**Budget Allocation Screen Access**
- Add a "Budget" button on the main game screen (positioned below the status bar)
- Tapping the Budget button opens a full-screen budget allocation modal
- The modal displays the budget allocation UI
- A "Close" or "Back" button returns to the main game screen

**Six Policy Categories**

Per GDD §3-2, the player allocates budget across these categories:
1. Welfare & Healthcare (福祉・医療)
2. Education & Childcare (教育・子育て)
3. Industry Development (産業振興)
4. Infrastructure (インフラ整備)
5. Disaster Prevention & Safety (防災・安全)
6. Tourism & Culture (観光・文化)

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

**Allocation Constraints (Simplified Phase 1)**
- No minimum required allocation per category (all categories can be ¥0)
- Total allocation must equal total budget (100% utilization required)
- Budget must be non-negative integer values (no fractional yen)

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

### Model Layer (Domain.Models)

**New: BudgetAllocation**

Immutable value object representing the allocation across all six categories:

```csharp
public class BudgetAllocation
{
    public long WelfareHealthcare { get; }
    public long EducationChildcare { get; }
    public long IndustryDevelopment { get; }
    public long Infrastructure { get; }
    public long DisasterPrevention { get; }
    public long TourismCulture { get; }

    public BudgetAllocation(
        long welfareHealthcare,
        long educationChildcare,
        long industryDevelopment,
        long infrastructure,
        long disasterPrevention,
        long tourismCulture)
    {
        WelfareHealthcare = welfareHealthcare;
        EducationChildcare = educationChildcare;
        IndustryDevelopment = industryDevelopment;
        Infrastructure = infrastructure;
        DisasterPrevention = disasterPrevention;
        TourismCulture = tourismCulture;
    }

    public long TotalAllocated =>
        WelfareHealthcare + EducationChildcare + IndustryDevelopment +
        Infrastructure + DisasterPrevention + TourismCulture;

    public static BudgetAllocation EqualDistribution(long totalBudget)
    {
        long perCategory = totalBudget / 6;
        return new BudgetAllocation(
            perCategory, perCategory, perCategory,
            perCategory, perCategory, perCategory);
    }
}
```

**Extend: GameState**

Add `CurrentAllocation` property to track the active budget allocation:

```csharp
public class GameState
{
    public GameDate CurrentDate { get; }
    public int Population { get; }
    public long Budget { get; }
    public int ApprovalRating { get; }
    public BudgetAllocation CurrentAllocation { get; }

    // Constructor updated to include CurrentAllocation
    // CreateInitial() factory method updated to include default equal distribution
}
```

### Business Logic Layer (Domain.Systems)

**New: BudgetValidator**

Pure C# class for validation logic:

```csharp
public class BudgetValidator
{
    public bool IsValid(BudgetAllocation allocation, long totalBudget)
    {
        return allocation.TotalAllocated == totalBudget;
    }

    public long GetRemaining(BudgetAllocation allocation, long totalBudget)
    {
        return totalBudget - allocation.TotalAllocated;
    }
}
```

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
Assets/Scripts/Domain/Models/BudgetAllocation.cs
Assets/Scripts/Domain/Systems/BudgetValidator.cs
Assets/Scripts/Presentation/Budget/BudgetAllocationPresenter.cs
Assets/Scripts/Presentation/Budget/BudgetScreen.prefab

Assets/Tests/EditMode/Domain/Models/BudgetAllocationTest.cs
Assets/Tests/EditMode/Domain/Systems/BudgetValidatorTest.cs
Assets/Tests/PlayMode/Presentation/Budget/BudgetAllocationPresenterTest.cs
Assets/Tests/PlayMode/Presentation/Budget/BudgetScreenIntegrationTest.cs
```

Modified files:
```
Assets/Scripts/Domain/Models/GameState.cs
Assets/Scripts/Presentation/MainGameSceneInitializer.cs
```
