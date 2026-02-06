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
