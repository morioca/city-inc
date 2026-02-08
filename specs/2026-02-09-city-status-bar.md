# City Status Bar on Main Game Screen

## Requirements

The main game screen currently displays only the current date and a "Next Month" button. Per GDD §5-4, the top area of the main screen should show the mayor's key at-a-glance metrics: approval rating, financial balance, and current date. Without this information, the player has no basis for decision-making each turn.

## Specifications

- `GameState` holds three new city metrics:
  - `Population` (integer, e.g. 50,000)
  - `Budget` (long integer representing yen, e.g. 100,000,000)
  - `ApprovalRating` (integer percentage, 0–100, e.g. 60)
- The main game screen top area displays all three metrics alongside the existing date.
- Values are static (hardcoded initial defaults) — no delta/comparison display in this iteration.
- Initial defaults: Population = 50,000 / Budget = 100,000,000 / ApprovalRating = 60.

## Design

### Model layer (`Domain.Models`)

Extend `GameState` with three new read-only auto-properties:

```csharp
public int Population { get; }
public long Budget { get; }
public int ApprovalRating { get; }
```

Add a convenience factory method `GameState.CreateInitial()` that returns a `GameState` with:
- `CurrentDate` = year 1, month 1
- `Population` = 50,000
- `Budget` = 100,000,000
- `ApprovalRating` = 60

The existing constructor is extended to accept the new parameters (keep backward-compatible overload or update all call sites).

### Presentation layer (`Presentation`)

- `GameStatePresenter` gains three additional `TextMeshProUGUI` references (one per new metric) and formats them as:
  - Population: `"人口 {0:N0}人"` (e.g. "人口 50,000人")
  - Budget: `"財政 {0:N0}円"` (e.g. "財政 100,000,000円")
  - ApprovalRating: `"支持率 {0}%"` (e.g. "支持率 60%")
- `MainGameSceneInitializer` creates a horizontal status bar panel at the top of the safe area and instantiates the three `TextMeshProUGUI` labels within it.

### Unchanged

- `TurnManager` — turn advancement logic is unchanged; `GameState` immutability is preserved.
- `SafeAreaLayout` — reused as-is for the status bar panel.

## Test Cases

### GameState (Domain.Models.GameStateTest)

#### Constructor stores new metrics

- **GameState_Constructor_WhenGivenPopulation_StoresItAsPopulation**
  - Arrange: `population = 50000`
  - Act: `new GameState(new GameDate(1, 1), population, 100000000L, 60)`
  - Assert: `sut.Population == 50000`

- **GameState_Constructor_WhenGivenBudget_StoresItAsBudget**
  - Arrange: `budget = 100000000L`
  - Act: `new GameState(new GameDate(1, 1), 50000, budget, 60)`
  - Assert: `sut.Budget == 100000000L`

- **GameState_Constructor_WhenGivenApprovalRating_StoresItAsApprovalRating**
  - Arrange: `approvalRating = 60`
  - Act: `new GameState(new GameDate(1, 1), 50000, 100000000L, approvalRating)`
  - Assert: `sut.ApprovalRating == 60`

#### CreateInitial factory method

- **GameState_CreateInitial_WhenCalled_ReturnsYear1Month1**
  - Act: `GameState.CreateInitial()`
  - Assert: `sut.CurrentDate == new GameDate(1, 1)`

- **GameState_CreateInitial_WhenCalled_ReturnsPopulation50000**
  - Act: `GameState.CreateInitial()`
  - Assert: `sut.Population == 50000`

- **GameState_CreateInitial_WhenCalled_ReturnsBudget100000000**
  - Act: `GameState.CreateInitial()`
  - Assert: `sut.Budget == 100000000L`

- **GameState_CreateInitial_WhenCalled_ReturnsApprovalRating60**
  - Act: `GameState.CreateInitial()`
  - Assert: `sut.ApprovalRating == 60`

#### Immutability

- **GameState_Population_WhenAccessedTwice_ReturnsSameValue**
  - Arrange: `new GameState(new GameDate(1, 1), 50000, 100000000L, 60)`
  - Assert: `sut.Population` accessed twice returns the same value

- **GameState_Budget_WhenAccessedTwice_ReturnsSameValue**
  - Arrange: `new GameState(new GameDate(1, 1), 50000, 100000000L, 60)`
  - Assert: `sut.Budget` accessed twice returns the same value

- **GameState_ApprovalRating_WhenAccessedTwice_ReturnsSameValue**
  - Arrange: `new GameState(new GameDate(1, 1), 50000, 100000000L, 60)`
  - Assert: `sut.ApprovalRating` accessed twice returns the same value

### GameStatePresenter (Presentation.GameStatePresenterTest)

#### Initialize displays all metric labels

- **Initialize_WhenCalled_DisplaysPopulationLabel**
  - Arrange: create presenter with all four TMP labels wired; `GameState.CreateInitial()`
  - Act: `presenter.Initialize(state)`
  - Assert: `PopulationLabel.text == "人口 50,000人"`

- **Initialize_WhenCalled_DisplaysBudgetLabel**
  - Arrange: same setup; `GameState.CreateInitial()`
  - Act: `presenter.Initialize(state)`
  - Assert: `BudgetLabel.text == "財政 100,000,000円"`

- **Initialize_WhenCalled_DisplaysApprovalRatingLabel**
  - Arrange: same setup; `GameState.CreateInitial()`
  - Act: `presenter.Initialize(state)`
  - Assert: `ApprovalRatingLabel.text == "支持率 60%"`

#### Metric labels do not change after Next Month button click (static values)

- **OnNextMonthButtonClicked_WhenCalled_PopulationLabelRemainsUnchanged**
  - Arrange: initialize with `GameState.CreateInitial()`; note initial population text
  - Act: `presenter.OnNextMonthButtonClicked()`
  - Assert: `PopulationLabel.text == "人口 50,000人"`

- **OnNextMonthButtonClicked_WhenCalled_BudgetLabelRemainsUnchanged**
  - Arrange: initialize with `GameState.CreateInitial()`
  - Act: `presenter.OnNextMonthButtonClicked()`
  - Assert: `BudgetLabel.text == "財政 100,000,000円"`

- **OnNextMonthButtonClicked_WhenCalled_ApprovalRatingLabelRemainsUnchanged**
  - Arrange: initialize with `GameState.CreateInitial()`
  - Act: `presenter.OnNextMonthButtonClicked()`
  - Assert: `ApprovalRatingLabel.text == "支持率 60%"`

### MainGameScene integration (Presentation.MainGameSceneIntegrationTest)

- **MainGameScene_WhenLoaded_CreatesStatusBarPanel**
  - Load "MainGameScene"
  - Assert: `GameObject.Find("StatusBarPanel")` is not null

- **MainGameScene_WhenLoaded_StatusBarPanelIsUnderSafeAreaPanel**
  - Load "MainGameScene"
  - Assert: `statusBarPanel.transform.parent.name == "SafeAreaPanel"`

- **MainGameScene_WhenLoaded_PopulationLabelExists**
  - Load "MainGameScene"
  - Assert: `GameObject.Find("PopulationLabel")?.GetComponent<TMP_Text>()` is not null

- **MainGameScene_WhenLoaded_BudgetLabelExists**
  - Load "MainGameScene"
  - Assert: `GameObject.Find("BudgetLabel")?.GetComponent<TMP_Text>()` is not null

- **MainGameScene_WhenLoaded_ApprovalRatingLabelExists**
  - Load "MainGameScene"
  - Assert: `GameObject.Find("ApprovalRatingLabel")?.GetComponent<TMP_Text>()` is not null

- **MainGameScene_WhenLoaded_PopulationLabelShowsInitialValue**
  - Load "MainGameScene"
  - Assert: `populationLabel.text == "人口 50,000人"`

- **MainGameScene_WhenLoaded_BudgetLabelShowsInitialValue**
  - Load "MainGameScene"
  - Assert: `budgetLabel.text == "財政 100,000,000円"`

- **MainGameScene_WhenLoaded_ApprovalRatingLabelShowsInitialValue**
  - Load "MainGameScene"
  - Assert: `approvalRatingLabel.text == "支持率 60%"`
