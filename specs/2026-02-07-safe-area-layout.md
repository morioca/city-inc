# Safe Area Layout 仕様書

## 問題

iPhoneの画面上部にあるノッチやダイナミックアイランドにUIが重なり、一部のUI要素が見えなくなっている。

この問題は以下の機種で発生する：
- iPhone X以降のノッチ付き機種
- iPhone 14 Pro以降のダイナミックアイランド搭載機種

## 要件

- シンプルかつ確実にSafe Area問題を解決する
- 今後UIを追加する際も個別対応が不要
- 一度の実装で完全に解決する

## 仕様

Safe Areaを使用して、全UIが安全領域内に収まるようにする。

### 機能要件

1. **Safe Area自動調整機能**
   - `Screen.safeArea`を使用してデバイスの安全領域を取得
   - Canvas直下のパネルのRectTransformを安全領域に合わせて自動調整
   - 画面回転時も自動的に再調整

2. **全UI適用**
   - 既存の全UIをSafe Area Panel配下に配置
   - 今後作成するUIも同パネル配下に配置することで自動対応

### 非機能要件

1. **パフォーマンス**
   - 画面サイズ変更時のみ調整処理を実行（毎フレーム実行しない）

2. **保守性**
   - コンポーネントは単一責任（Safe Area調整のみ）
   - テスタブルな設計

## 設計

### コンポーネント設計

#### SafeAreaLayout コンポーネント

**責務**: アタッチされたRectTransformをデバイスのSafe Areaに合わせて調整する

**配置場所**: `CityInc/Assets/MainGame/Scripts/Runtime/UI/SafeAreaLayout.cs`

**プロパティ**:
- なし（自動動作）

**動作**:
1. `Awake`時に初回調整を実行
2. 画面サイズ変更を検出したら再調整
3. `Screen.safeArea`からアンカー位置を計算
4. RectTransformのanchorMin/anchorMaxを設定

**実装の詳細**:
```csharp
public class SafeAreaLayout : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Rect _lastSafeArea;
    private Vector2Int _lastScreenSize;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    private void Update()
    {
        if (IsScreenSizeChanged())
        {
            ApplySafeArea();
        }
    }

    private bool IsScreenSizeChanged()
    {
        return Screen.safeArea != _lastSafeArea
            || Screen.width != _lastScreenSize.x
            || Screen.height != _lastScreenSize.y;
    }

    private void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;

        _lastSafeArea = safeArea;
        _lastScreenSize = new Vector2Int(Screen.width, Screen.height);
    }
}
```

### シーン構造

```
Canvas (Screen Space - Overlay)
└── SafeAreaPanel (SafeAreaLayoutコンポーネント付き)
    ├── Header
    ├── MainContent
    └── Footer
```

### 既存UIの移行手順

1. Canvas直下に新規Panel（SafeAreaPanel）を作成
2. SafeAreaLayoutコンポーネントをアタッチ
3. 既存の全UI要素をSafeAreaPanel配下に移動
4. RectTransformの設定を確認・調整

### 対応デバイス

- iOS全機種（Safe Areaが定義されている機種で自動適用）
- Android（必要に応じて適用）
- Unityエディタ（Device Simulatorで動作確認可能）

## テスト方針

1. **単体テスト**
   - Screen.safeAreaの値に基づいてanchorMin/anchorMaxが正しく計算されること
   - 画面サイズ変更時に再調整されること

2. **結合テスト**
   - 実機またはDevice Simulatorで各種iPhone機種での表示確認
   - 画面回転時の動作確認

## テストケース

### SafeAreaLayoutクラス

#### 初期化時の動作

**使用するテスト技法**: 同値分割法（Safe Areaの位置・サイズの組み合わせ）

| ID | テストケース | 入力条件 | 期待される結果 | 検証方法 |
|----|------------|---------|---------------|---------|
| TC-01 | Safe Areaが画面全体の場合にanchorが正しく設定される | Screen.safeArea=(0,0,1920,1080), Screen.size=(1920,1080) | anchorMin=(0,0), anchorMax=(1,1) | RectTransformのanchorMin/anchorMaxを確認 |
| TC-03 | Safe Areaが画面上部にオフセットがある場合にanchorが正しく設定される（ノッチ対応） | Screen.safeArea=(0,100,1920,980), Screen.size=(1920,1080) | anchorMin=(0,0.0926), anchorMax=(1,1) | RectTransformのanchorMin.y/anchorMax.yを確認 |
| TC-05 | Safe Areaが左右にオフセットがある場合にanchorが正しく設定される | Screen.safeArea=(50,0,1820,1080), Screen.size=(1920,1080) | anchorMin=(0.026,0), anchorMax=(0.974,1) | RectTransformのanchorMin.x/anchorMax.xを確認 |
| TC-07 | Safe Areaが四辺すべてにオフセットがある場合にanchorが正しく設定される | Screen.safeArea=(50,100,1820,880), Screen.size=(1920,1080) | anchorMin=(0.026,0.0926), anchorMax=(0.974,0.907) | RectTransformのanchorMin/anchorMaxを確認 |

#### 画面サイズ変更時の動作

**使用するテスト技法**: 状態遷移テスト（画面サイズ・Safe Areaの変化）

| ID | テストケース | 入力条件 | 期待される結果 | 検証方法 |
|----|------------|---------|---------------|---------|
| TC-10 | 画面サイズが変更されたときにanchorが再計算される | 初期: Screen.size=(1920,1080), SafeArea=(0,100,1920,980)<br>変更後: Screen.size=(2436,1125), SafeArea=(0,59,2436,1066) | anchorMin=(0,0.052), anchorMax=(1,1) | Update呼び出し後のRectTransformを確認 |
| TC-12 | Safe Areaのみが変更されたときにanchorが再計算される | 初期: SafeArea=(0,100,1920,980)<br>変更後: SafeArea=(0,0,1920,1080) | anchorMin=(0,0), anchorMax=(1,1) | Update呼び出し後のRectTransformを確認 |
| TC-14 | 画面サイズとSafe Areaが変更されないときに再計算されない | Screen.size=(1920,1080), SafeArea=(0,100,1920,980)で固定 | anchorは変更されない | Update呼び出し前後でRectTransformが同じ値 |

#### エッジケース

**使用するテスト技法**: 境界値分析

| ID | テストケース | 入力条件 | 期待される結果 | 検証方法 |
|----|------------|---------|---------------|---------|
| TC-20 | 極小画面サイズでも正しく動作する | Screen.size=(100,100), SafeArea=(10,10,80,80) | anchorMin=(0.1,0.1), anchorMax=(0.9,0.9) | RectTransformのanchorMin/anchorMaxを確認 |
| TC-22 | Safe Areaのwidthが0の場合でも例外が発生しない | Screen.size=(1920,1080), SafeArea=(0,0,0,1080) | anchorMin=(0,0), anchorMax=(0,1) | 例外が発生せず、anchorMin.x == anchorMax.xとなる |
| TC-24 | Safe Areaのheightが0の場合でも例外が発生しない | Screen.size=(1920,1080), SafeArea=(0,0,1920,0) | anchorMin=(0,0), anchorMax=(1,0) | 例外が発生せず、anchorMin.y == anchorMax.yとなる |
