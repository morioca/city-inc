# City Inc（仮） - テクニカルデザインドキュメント（TDD）

---

## 第1章：技術選定

### 1-1. 開発環境

| 項目 | 選定 | 備考 |
|------|------|------|
| プラットフォーム | iOS | 初期リリース対象 |
| 開発言語 | （未定） | Swift / Unity (C#) を検討 |
| ゲームエンジン | （未定） | SwiftUI / Unity / Godot を検討 |
| 最低対応OS | （未定） | iOS 15以降を想定 |

### 1-2. 選定時の考慮点

生成AIがプログラムを把握し、テストやデバッグを駆使しながら自律的に開発を進められること

- 2Dベースのシミュレーションに適した技術スタック
- データ駆動型のUI更新が容易であること

---

## 第2章：システム構成

### 2-1. アーキテクチャ概要

```
[View Layer]
    ↓ ↑
[ViewModel / Controller]
    ↓ ↑
[Game Logic / Domain]
    ↓ ↑
[Data Layer / Persistence]
```

### 2-2. 主要コンポーネント（予定）

| コンポーネント | 責務 |
|---------------|------|
| GameState | ゲーム全体の状態管理 |
| TurnManager | ターン進行・イベント発火 |
| BudgetSystem | 予算配分・収支計算 |
| PolicySystem | 政策の実行・効果計算 |
| SupportRateSystem | 支持率の計算・住民層管理 |
| EventSystem | 定期・ランダムイベント管理 |
| SaveManager | セーブ/ロード処理 |

---

## 第3章：データ構造（方針）

### 3-1. ゲームデータ

- シナリオ定義（JSON/YAML）
- 政策マスタ（効果、コスト、影響範囲）
- イベントマスタ（条件、効果、選択肢）
- 住民層定義（重視する政策、支持率係数）

### 3-2. セーブデータ

- 現在のターン数・日付
- 財政状況（収入、支出、公債残高）
- 各指標の現在値と履歴
- 実行中の政策
- 発生済みイベント

---

## 第4章：実装ロードマップ

### Phase 1：コアループの確立（MVP）

**実装内容：**
- タイトル画面、初期設定画面
- メインマップ画面（1都市）
- 基本的な予算配分システム
- 基本政策（各カテゴリ3〜5個）
- 月次ターン進行
- 支持率システム（簡易版）
- 1シナリオ（地方衰退都市）

**検証ポイント：**
- コアループが面白いか
- ターンのテンポは適切か
- 政策の効果実感があるか

### Phase 2：深みの追加

**実装内容：**
- 委譲システム（部局長）
- 議会システム
- イベントシステム（定期・ランダム）
- 追加政策（各カテゴリ10〜15個）
- 住民の声システム
- 詳細統計画面

### Phase 3：スケールと多様性

**実装内容：**
- 追加シナリオ（3〜4個）
- 町→市→県の昇格システム
- 国との折衝システム
- 実績・称号システム
- エンディングバリエーション

---

## 第5章：テスト戦略

### 5-1. 設計原則

生成AIによる自律的な開発を可能にするため、以下の原則を採用する：

1. **ロジックとUnity依存の分離**: ビジネスロジックを純粋なC#クラス（POCO）として実装し、MonoBehaviourから分離
2. **依存性注入（DI）**: インターフェースを通じて依存関係を注入し、テスト時にモック可能にする
3. **イミュータブルなデータ構造**: 状態変更は新しいオブジェクトを返す形式で、副作用を最小化

### 5-2. レイヤー別テスト方針

| レイヤー | テスト種別 | 説明 |
|---------|-----------|------|
| Domain Logic | Edit Mode テスト | 最重要。BudgetSystem, PolicySystem等の純粋ロジック |
| ViewModel | Edit Mode テスト | 状態管理とロジック呼び出しの検証 |
| Data Layer | Edit Mode テスト | インターフェース経由でモック化 |
| View Layer | Play Mode テスト | UI統合テスト（優先度低） |

### 5-3. フォルダ構成

```
Assets/
├── Scripts/
│   ├── Domain/           # 純粋C#ロジック（テスト対象の中心）
│   │   ├── Models/       # データモデル
│   │   └── Systems/      # ビジネスロジック
│   ├── Interfaces/       # インターフェース定義
│   ├── Infrastructure/   # 外部依存（セーブ、API等）の実装
│   └── Presentation/     # MonoBehaviour、UI関連
├── Tests/
│   ├── EditMode/         # Edit Modeテスト
│   └── PlayMode/         # Play Modeテスト（UIテスト）
```

### 5-4. テスト可能な設計パターン

```csharp
// ❌ テストしにくい例
public class BadBudgetManager : MonoBehaviour {
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            PlayerPrefs.SetInt("budget", budget - 100);  // 外部依存が直接
        }
    }
}

// ✅ テストしやすい例
public class BudgetSystem {  // 純粋C#クラス
    public BudgetState AllocateBudget(BudgetState current, AllocationRequest request) {
        // 純粋な計算のみ、外部依存なし
        return new BudgetState(current.Total - request.Amount, ...);
    }
}
```

### 5-5. モック戦略

```csharp
// インターフェース定義
public interface IGameRepository {
    GameState Load();
    void Save(GameState state);
}

// 本番実装
public class PlayerPrefsGameRepository : IGameRepository { ... }

// テスト用モック
public class MockGameRepository : IGameRepository {
    public GameState StateToReturn { get; set; }
    public GameState LastSavedState { get; private set; }

    public GameState Load() => StateToReturn;
    public void Save(GameState state) => LastSavedState = state;
}
```

---

## 第6章：今後検討が必要な事項

- [ ] 開発言語・エンジンの最終決定
- [ ] CI/CD環境の構築
- [x] テスト戦略（単体テスト、UIテスト）
- [ ] アナリティクス・クラッシュレポートの導入
- [ ] マネタイズ方式（買い切り / 広告 / IAP）
- [ ] ローカライズ対応の要否
