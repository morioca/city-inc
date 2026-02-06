# MainGame シーンへの Safe Area 適用

## 問題

SafeAreaLayout コンポーネントを実装したにもかかわらず、メイン画面の年月日表示（DateLabel）がノッチやダイナミックアイランドに隠れている。

原因: `MainGameSceneInitializer` が動的に作成する UI 要素が Canvas 直下に配置されており、SafeAreaPanel の配下になっていない。

現在の構造:
```
Canvas
├── DateLabel (画面上端から-50pxに配置)
├── NextMonthButton
└── GameStatePresenter
```

## 仕様

MainGameScene で動的に作成されるすべての UI 要素を SafeAreaLayout の配下に配置し、Safe Area 調整を適用する。

### 機能要件

1. **SafeAreaPanel の自動作成**
   - Canvas 直下に SafeAreaPanel を作成
   - SafeAreaLayout コンポーネントをアタッチ
   - RectTransform を画面全体に stretch 設定

2. **UI 要素の配置**
   - DateLabel、NextMonthButton、GameStatePresenter を SafeAreaPanel 配下に配置
   - 既存のアンカー設定は維持（SafeAreaPanel 内での相対位置として機能）

### 期待される結果

- ノッチ/ダイナミックアイランド搭載デバイスで年月日表示が正しく表示される
- Safe Area 非対応デバイスでは従来通りの表示

## 設計

### 変更対象

`CityInc/Assets/MainGame/Scripts/Runtime/Presentation/MainGameSceneInitializer.cs`

### シーン構造（変更後）

```
Canvas
└── SafeAreaPanel (SafeAreaLayoutコンポーネント付き)
    ├── DateLabel
    ├── NextMonthButton
    └── GameStatePresenter
```

### 実装方針

1. `SetupScene` メソッドで Canvas 作成後に SafeAreaPanel を作成
2. SafeAreaPanel に SafeAreaLayout コンポーネントをアタッチ
3. 既存の CreateDateLabel、CreateNextMonthButton、CreateGameStatePresenter の parent 引数を SafeAreaPanel の transform に変更

### コード変更

```csharp
private void SetupScene()
{
    var canvas = CreateCanvas();
    var safeAreaPanel = CreateSafeAreaPanel(canvas.transform);
    var dateLabel = CreateDateLabel(safeAreaPanel.transform);
    var nextMonthButton = CreateNextMonthButton(safeAreaPanel.transform);
    CreateGameStatePresenter(safeAreaPanel.transform, dateLabel, nextMonthButton);
}

private GameObject CreateSafeAreaPanel(Transform parent)
{
    var panelObject = new GameObject("SafeAreaPanel");
    panelObject.transform.SetParent(parent, false);

    var rectTransform = panelObject.AddComponent<RectTransform>();
    rectTransform.anchorMin = Vector2.zero;
    rectTransform.anchorMax = Vector2.one;
    rectTransform.offsetMin = Vector2.zero;
    rectTransform.offsetMax = Vector2.zero;

    panelObject.AddComponent<SafeAreaLayout>();

    return panelObject;
}
```

## テスト方針

1. **単体テスト**
   - SafeAreaPanel が Canvas 直下に作成されること
   - SafeAreaLayout コンポーネントがアタッチされていること
   - DateLabel が SafeAreaPanel 配下に配置されていること

2. **結合テスト**
   - Device Simulator で iPhone 14 Pro 等のノッチ/ダイナミックアイランド搭載機種をシミュレートし、DateLabel が Safe Area 内に表示されることを確認

## テストケース

### MainGameSceneInitializer クラス

#### SafeAreaPanel の作成

**使用するテスト技法**: 構造検証（オブジェクト階層の確認）

| ID | テストケース | 入力条件 | 期待される結果 | 検証方法 |
|----|------------|---------|---------------|---------|
| TC-10 | SafeAreaPanel が Canvas 直下に作成される | MainGameScene をロード | "SafeAreaPanel" という名前の GameObject が存在し、親が Canvas である | GameObject.Find と transform.parent の確認 |
| TC-12 | SafeAreaPanel に SafeAreaLayout コンポーネントがアタッチされている | MainGameScene をロード | SafeAreaPanel に SafeAreaLayout コンポーネントが存在する | GetComponent<SafeAreaLayout>() が null でないことを確認 |
| TC-14 | SafeAreaPanel の RectTransform が画面全体に stretch 設定されている | MainGameScene をロード | anchorMin=(0,0), anchorMax=(1,1), offsetMin=(0,0), offsetMax=(0,0) | RectTransform の各プロパティを確認 |

#### UI 要素の配置

**使用するテスト技法**: 構造検証（親子関係の確認）

| ID | テストケース | 入力条件 | 期待される結果 | 検証方法 |
|----|------------|---------|---------------|---------|
| TC-20 | DateLabel が SafeAreaPanel 配下に配置されている | MainGameScene をロード | DateLabel の親が SafeAreaPanel である | transform.parent.name が "SafeAreaPanel" であることを確認 |
| TC-22 | NextMonthButton が SafeAreaPanel 配下に配置されている | MainGameScene をロード | NextMonthButton の親が SafeAreaPanel である | transform.parent.name が "SafeAreaPanel" であることを確認 |
| TC-24 | GameStatePresenter が SafeAreaPanel 配下に配置されている | MainGameScene をロード | GameStatePresenter の親が SafeAreaPanel である | transform.parent.name が "SafeAreaPanel" であることを確認 |
