# Budget Allocation System — Phase 1 (Domain)

## Requirements

The main game screen currently displays city status (population, budget, approval rating) and allows advancing turns via "Next Month" button. However, the player has no way to make decisions or affect the city's trajectory. Per GDD §3-2, budget allocation is the primary decision-making mechanism where the mayor distributes limited financial resources across six policy categories to influence city development.

Without a budget allocation system:
- The core gameplay loop (Observe → Decide → Execute → Review) remains incomplete
- Clicking "Next Month" has no strategic meaning
- The player cannot express their policy priorities or playstyle
- City metrics remain static and unchanging

This spec covers the **domain layer** (models and business logic) that underpins the budget allocation feature. The presentation layer is specified separately in `2026-02-11-budget-allocation-system-presentation.md`.

### Functional Requirements

**Six Policy Categories**

Per GDD §3-2, the player allocates budget across these categories:
1. Welfare & Healthcare (福祉・医療)
2. Education & Childcare (教育・子育て)
3. Industry Development (産業振興)
4. Infrastructure (インフラ整備)
5. Disaster Prevention & Safety (防災・安全)
6. Tourism & Culture (観光・文化)

**Budget Allocation Model**
- An immutable value object representing the allocation across all six categories
- Each category stores its allocation as a `long` (non-negative integer yen)
- Computes the total allocated amount as the sum of all six categories
- Provides a factory method for equal distribution given a total budget

**Allocation Constraints (Simplified Phase 1)**
- No minimum required allocation per category (all categories can be ¥0)
- Total allocation must equal total budget (100% utilization required)
- Budget must be non-negative integer values (no fractional yen)

**Validation Logic**
- Determines whether a given allocation is valid (total allocated equals total budget)
- Computes the remaining unallocated budget (total budget minus total allocated)

**GameState Extension**
- GameState includes a `CurrentAllocation` property to track the active budget allocation
- `GameState.CreateInitial()` factory method produces an equal distribution as the default allocation

**Initial State**
- On first access, all categories default to equal distribution (¥16,666,666 each for ¥100M budget)

### Non-Functional Requirements

**Phase 1 Scope Limitations**
- Allocation does NOT yet affect city metrics (population, budget, approval rating remain static)
- No feedback on allocation quality or citizen reactions
- No annual budget revision cycle (allocation persists indefinitely)
- No budget income/expense simulation (total budget remains constant at ¥100M)
- No policy execution within categories (just allocation amounts)

These limitations will be addressed in subsequent phases.

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

Assets/Tests/EditMode/Domain/Models/BudgetAllocationTest.cs
Assets/Tests/EditMode/Domain/Systems/BudgetValidatorTest.cs
```

Modified files:
```
Assets/Scripts/Domain/Models/GameState.cs
```

## Test Cases

### Edit Mode Tests — Domain.Models.BudgetAllocation

#### BudgetAllocation_Constructor_WhenCalledWithValidValues_SetsAllProperties
- Verify all six category properties are set correctly with provided constructor arguments
- Verify TotalAllocated returns sum of all six categories

#### BudgetAllocation_TotalAllocated_WhenCategoriesAreZero_ReturnsZero
- Create allocation with all categories set to 0
- Verify TotalAllocated returns 0

#### BudgetAllocation_TotalAllocated_WhenCategoriesHaveValues_ReturnsCorrectSum
- Create allocation with mixed values (e.g., 10M, 20M, 15M, 25M, 5M, 25M)
- Verify TotalAllocated returns 100M

#### BudgetAllocation_TotalAllocated_WhenSingleCategoryHasAllBudget_ReturnsTotalBudget
- Create allocation with one category = 100M, others = 0
- Verify TotalAllocated returns 100M

#### BudgetAllocation_EqualDistribution_WhenTotalBudgetIs100Million_ReturnsEqualAllocation
- Call EqualDistribution(100000000L)
- Verify all six categories = 16666666
- Verify TotalAllocated = 99999996 (due to integer division)

#### BudgetAllocation_EqualDistribution_WhenTotalBudgetIs0_ReturnsZeroAllocation
- Call EqualDistribution(0L)
- Verify all six categories = 0

#### BudgetAllocation_EqualDistribution_WhenTotalBudgetIs1000_ReturnsEqualDistribution
- Call EqualDistribution(1000L)
- Verify all six categories = 166
- Verify TotalAllocated = 996

#### BudgetAllocation_Constructor_WhenCalledWithNegativeValues_AcceptsNegativeValues
- Create allocation with negative values (edge case test, system should handle at validation layer)
- Verify properties store the negative values as provided
- Note: validation is BudgetValidator's responsibility, not constructor's

### Edit Mode Tests — Domain.Models.GameState

#### GameState_Constructor_WhenCalledWithBudgetAllocation_SetsBudgetAllocationProperty
- Create GameState with a specific BudgetAllocation
- Verify CurrentAllocation property returns the provided allocation

#### GameState_CreateInitial_WhenCalled_SetsEqualDistributionBudgetAllocation
- Call GameState.CreateInitial()
- Verify CurrentAllocation is equal distribution across six categories
- Verify CurrentAllocation.TotalAllocated ≈ 100M (within rounding error)

#### GameState_CreateInitial_WhenCalled_BudgetAllocationMatchesTotalBudget
- Call GameState.CreateInitial()
- Verify Budget property = 100000000L
- Verify CurrentAllocation uses this budget for equal distribution

### Edit Mode Tests — Domain.Systems.BudgetValidator

#### BudgetValidator_IsValid_WhenTotalAllocatedEqualsTotalBudget_ReturnsTrue
- Create allocation totaling exactly 100M
- Call IsValid with totalBudget = 100M
- Verify returns true

#### BudgetValidator_IsValid_WhenTotalAllocatedExceedsTotalBudget_ReturnsFalse
- Create allocation totaling 110M
- Call IsValid with totalBudget = 100M
- Verify returns false

#### BudgetValidator_IsValid_WhenTotalAllocatedLessThanTotalBudget_ReturnsFalse
- Create allocation totaling 90M
- Call IsValid with totalBudget = 100M
- Verify returns false

#### BudgetValidator_IsValid_WhenTotalAllocatedIsZeroAndBudgetIsZero_ReturnsTrue
- Create allocation totaling 0
- Call IsValid with totalBudget = 0
- Verify returns true

#### BudgetValidator_GetRemaining_WhenTotalAllocatedEqualsTotalBudget_ReturnsZero
- Create allocation totaling 100M
- Call GetRemaining with totalBudget = 100M
- Verify returns 0

#### BudgetValidator_GetRemaining_WhenTotalAllocatedLessThanBudget_ReturnsPositiveValue
- Create allocation totaling 80M
- Call GetRemaining with totalBudget = 100M
- Verify returns 20M

#### BudgetValidator_GetRemaining_WhenTotalAllocatedExceedsBudget_ReturnsNegativeValue
- Create allocation totaling 120M
- Call GetRemaining with totalBudget = 100M
- Verify returns -20M

#### BudgetValidator_GetRemaining_WhenAllocationIsZero_ReturnsTotalBudget
- Create allocation totaling 0
- Call GetRemaining with totalBudget = 100M
- Verify returns 100M
