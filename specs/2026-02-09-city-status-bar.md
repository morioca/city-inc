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
