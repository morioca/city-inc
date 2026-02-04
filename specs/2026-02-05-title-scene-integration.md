# タイトル画面統合コンポーネント 仕様書

## 問題

タイトル画面のボタンを押しても画面遷移が発生しない。

### 原因

以下の3つのコンポーネントが欠けているため、実装済みのクラスが機能していない：

1. **ISceneTransitionerの実装が存在しない**
   - インターフェースは定義されているが、UnityのSceneManager.LoadSceneを呼び出す実装クラスがない

2. **統合MonoBehaviourコンポーネントが存在しない**
   - `TitleMenuController`、`TitleSceneTransition`、`SaveDataChecker`を接続するコンポーネントがない
   - `TitleMenuController.Initialize()`を呼び出すコードがない
   - イベントハンドラを登録するコードがない

3. **シーンに配置されていない**
   - Canvasに統合コンポーネントが配置されていない
   - ボタンがInspectorで接続されていない

### 現在の実装状況

実装済みだが機能していないクラス：
- `TitleMenuController` - ボタンの状態管理とイベント発火（Initialize()が呼ばれていない）
- `TitleSceneTransition` - シーン遷移ロジック（ISceneTransitionerの実装がない）
- `SaveDataChecker` - セーブデータ存在確認（使用されていない）
- `ISaveDataChecker` - インターフェース
- `ISceneTransitioner` - インターフェース（実装がない）

## 仕様

### 機能要件

#### UnitySceneTransitioner
- `ISceneTransitioner`インターフェースの実装
- UnityのSceneManager.LoadSceneを使用してシーン遷移を実行
- 非MonoBehaviourクラス（純粋なC#クラス）

#### TitleSceneInitializer
- MonoBehaviourとして実装
- TitleSceneの初期化を担当
- 以下のコンポーネントを統合：
  - `TitleMenuController` - ボタン管理
  - `TitleSceneTransition` - 遷移ロジック
  - `SaveDataChecker` - セーブデータ確認
  - `UnitySceneTransitioner` - シーン遷移実行
- Awake()またはStart()でInitialize()を実行
- イベントハンドラを登録：
  - `TitleMenuController.OnNewGameSelected` → `TitleSceneTransition.TransitionToMainGame()`
  - `TitleMenuController.OnContinueSelected` → `TitleSceneTransition.TransitionToGameWithLatestSave()`

#### シーン構成の更新
- TitleScene.unityのCanvasに`TitleSceneInitializer`をアタッチ
- `TitleMenuController`を`TitleSceneInitializer`経由で参照

### 非機能要件
- テスタビリティを維持（依存性注入パターンを継続使用）
- 既存のテストコードは変更不要
- SOLID原則に従う（特に単一責任原則と依存性逆転原則）

## 設計

### クラス図

```
TitleSceneInitializer (MonoBehaviour)
  ├─ TitleMenuController (参照)
  ├─ SaveDataChecker (生成)
  ├─ UnitySceneTransitioner (生成)
  └─ TitleSceneTransition (生成)
       └─ ISceneTransitioner (依存)

UnitySceneTransitioner : ISceneTransitioner
  └─ SceneManager.LoadScene() を使用
```

### コンポーネント責務

#### UnitySceneTransitioner
- **責務:** Unityのシーン遷移機能のラッパー
- **配置:** なし（コードから生成）
- **依存:** UnityEngine.SceneManagement.SceneManager

#### TitleSceneInitializer
- **責務:** タイトル画面の初期化と各コンポーネントの統合
- **配置:** TitleScene.unity の Canvas GameObject
- **依存:**
  - TitleMenuController（SerializeFieldで参照）
  - SaveDataChecker（生成）
  - UnitySceneTransitioner（生成）
  - TitleSceneTransition（生成）

### ファイル構成

```
CityInc/Assets/TitleScreen/
├── Scripts/Runtime/
│   ├── ISaveDataChecker.cs             # 既存
│   ├── ISceneTransitioner.cs           # 既存
│   ├── SaveDataChecker.cs              # 既存
│   ├── TitleMenuController.cs          # 既存
│   ├── TitleSceneTransition.cs         # 既存
│   ├── UnitySceneTransitioner.cs       # 新規作成
│   └── TitleSceneInitializer.cs        # 新規作成
└── Tests/Runtime/
    ├── SaveDataCheckerTest.cs          # 既存（変更なし）
    ├── TitleMenuControllerTest.cs      # 既存（変更なし）
    ├── TitleSceneTransitionTest.cs     # 既存（変更なし）
    ├── UnitySceneTransitionerTest.cs   # 新規作成
    └── TitleSceneInitializerTest.cs    # 新規作成
```

### 実装詳細

#### UnitySceneTransitioner

```csharp
namespace TitleScreen
{
    /// <summary>
    /// Unity's SceneManager wrapper for scene transitions.
    /// </summary>
    public class UnitySceneTransitioner : ISceneTransitioner
    {
        /// <inheritdoc/>
        public void TransitionTo(string sceneName)
        {
            // SceneManager.LoadScene() を呼び出す
        }
    }
}
```

#### TitleSceneInitializer

```csharp
namespace TitleScreen
{
    /// <summary>
    /// Initializes the title scene and wires up components.
    /// </summary>
    public class TitleSceneInitializer : MonoBehaviour
    {
        [field: SerializeField]
        public TitleMenuController MenuController { get; private set; }

        private TitleSceneTransition _sceneTransition;

        private void Awake()
        {
            // コンポーネント生成
            var saveDataChecker = new SaveDataChecker();
            var sceneTransitioner = new UnitySceneTransitioner();
            _sceneTransition = new TitleSceneTransition(sceneTransitioner);

            // MenuController初期化
            MenuController.Initialize(saveDataChecker);

            // イベント登録
            MenuController.OnNewGameSelected += _sceneTransition.TransitionToMainGame;
            MenuController.OnContinueSelected += _sceneTransition.TransitionToGameWithLatestSave;
        }

        private void OnDestroy()
        {
            // イベント登録解除
            if (MenuController != null)
            {
                MenuController.OnNewGameSelected -= _sceneTransition.TransitionToMainGame;
                MenuController.OnContinueSelected -= _sceneTransition.TransitionToGameWithLatestSave;
            }
        }
    }
}
```

### シーン設定

TitleScene.unity の Canvas GameObject に以下を設定：

1. `TitleSceneInitializer` コンポーネントをアタッチ
2. Inspector で `MenuController` フィールドに `TitleMenuController` コンポーネントを設定
3. `TitleMenuController` の各ボタンフィールド（NewGameButton, ContinueButton, SettingsButton）が正しく設定されていることを確認

### 実装手順

1. `UnitySceneTransitioner.cs` を作成
2. `TitleSceneInitializer.cs` を作成
3. TitleScene.unity を更新
   - Canvas に `TitleSceneInitializer` を追加
   - `TitleMenuController` への参照を設定
4. Build Settings で MainGameScene が登録されていることを確認
5. AssetDatabase.Refresh() を実行
6. テストを作成・実行して検証
7. 手動テスト：
   - TitleScene を再生
   - 「新規ゲーム」ボタンをクリック → MainGameScene へ遷移
   - コンソールエラーが出ないことを確認

## テストケース

### テスト対象クラス: UnitySceneTransitioner

#### テスト技法
- 同値分割法: 有効なシーン名の遷移検証
- Spy パターン: SceneManager.LoadScene の呼び出し検証

#### テストケース一覧

| ID | テストケース | 検証内容 |
|----|------------|---------|
| UST-001 | TransitionTo_WhenCalledWithSceneName_LoadsScene | シーン名を指定してTransitionTo()を呼び出すと、SceneManager.LoadScene()が呼ばれることを検証 |
| UST-010 | TransitionTo_WhenCalledWithNullSceneName_ThrowsArgumentNullException | null を指定してTransitionTo()を呼び出すと、ArgumentNullException がスローされることを検証 |
| UST-020 | TransitionTo_WhenCalledWithEmptySceneName_ThrowsArgumentException | 空文字列を指定してTransitionTo()を呼び出すと、ArgumentException がスローされることを検証 |

### テスト対象クラス: TitleSceneInitializer

#### テスト技法
- 状態テスト: 初期化後の状態検証
- インテグレーションテスト: コンポーネント間の連携検証

#### テストケース一覧

| ID | テストケース | 検証内容 |
|----|------------|---------|
| TSI-001 | Awake_WhenCalled_InitializesMenuController | Awake()実行後、MenuControllerが初期化されていることを検証 |
| TSI-002 | Awake_WhenCalled_SubscribesToNewGameEvent | Awake()実行後、OnNewGameSelectedイベントが購読されていることを検証 |
| TSI-003 | Awake_WhenCalled_SubscribesToContinueEvent | Awake()実行後、OnContinueSelectedイベントが購読されていることを検証 |
| TSI-004 | NewGameButton_WhenClicked_TransitionsToMainGame | 「新規ゲーム」ボタンクリック時、MainGameSceneへ遷移することを検証（統合テスト） |
| TSI-010 | OnDestroy_WhenCalled_UnsubscribesFromNewGameEvent | OnDestroy()実行後、OnNewGameSelectedイベントの購読が解除されることを検証 |
| TSI-020 | OnDestroy_WhenCalled_UnsubscribesFromContinueEvent | OnDestroy()実行後、OnContinueSelectedイベントの購読が解除されることを検証 |
| TSI-030 | OnDestroy_WhenMenuControllerIsNull_DoesNotThrowException | MenuControllerがnullの状態でOnDestroy()を呼び出しても、例外がスローされないことを検証 |

#### 詳細

**UST-001: TransitionTo_WhenCalledWithSceneName_LoadsScene**
- 目的: UnitySceneTransitionerがSceneManager.LoadSceneを正しく呼び出すことを検証
- 前提条件: なし
- 操作: `TransitionTo("TestScene")` を呼び出す
- 期待結果: SceneManager.LoadScene("TestScene") が呼ばれる
- 備考: Play Modeテストで実装（SceneManager呼び出しのため）

**TSI-001: Awake_WhenCalled_InitializesMenuController**
- 目的: TitleSceneInitializerがMenuControllerを正しく初期化することを検証
- 前提条件: TitleSceneInitializerとTitleMenuControllerがシーンに配置されている
- 操作: シーンをロードしてAwake()を実行
- 期待結果: MenuControllerのボタンが活性化され、イベントリスナーが登録されている
- 備考: Play Modeテストで実装

**TSI-002, TSI-003: イベント購読の検証**
- 目的: イベントハンドラが正しく登録されることを検証
- 前提条件: TitleSceneInitializerが初期化されている
- 操作: MenuControllerのイベントを発火
- 期待結果: 対応する遷移メソッドが呼ばれる
- 備考: Spyパターンで検証

**TSI-004: NewGameButton_WhenClicked_TransitionsToMainGame**
- 目的: エンドツーエンドの動作検証
- 前提条件: TitleSceneがロードされている
- 操作: NewGameButtonをクリック
- 期待結果: MainGameSceneに遷移する
- 備考: Play Modeテストで実装

**UST-010: TransitionTo_WhenCalledWithNullSceneName_ThrowsArgumentNullException**
- 目的: null入力時の防御的プログラミングを検証
- 前提条件: なし
- 操作: `TransitionTo(null)` を呼び出す
- 期待結果: ArgumentNullException がスローされる
- 備考: Edit Modeテストで実装可能

**UST-020: TransitionTo_WhenCalledWithEmptySceneName_ThrowsArgumentException**
- 目的: 空文字列入力時の防御的プログラミングを検証
- 前提条件: なし
- 操作: `TransitionTo("")` を呼び出す
- 期待結果: ArgumentException がスローされる
- 備考: Edit Modeテストで実装可能

**TSI-010: OnDestroy_WhenCalled_UnsubscribesFromNewGameEvent**
- 目的: メモリリーク防止のためのイベント購読解除を検証
- 前提条件: TitleSceneInitializerが初期化されている
- 操作: OnDestroy()を呼び出す
- 期待結果: OnNewGameSelectedイベントの購読が解除される
- 備考: イベント発火時にハンドラが呼ばれないことで確認

**TSI-020: OnDestroy_WhenCalled_UnsubscribesFromContinueEvent**
- 目的: メモリリーク防止のためのイベント購読解除を検証
- 前提条件: TitleSceneInitializerが初期化されている
- 操作: OnDestroy()を呼び出す
- 期待結果: OnContinueSelectedイベントの購読が解除される
- 備考: イベント発火時にハンドラが呼ばれないことで確認

**TSI-030: OnDestroy_WhenMenuControllerIsNull_DoesNotThrowException**
- 目的: null参照例外の防止を検証
- 前提条件: MenuControllerがnull
- 操作: OnDestroy()を呼び出す
- 期待結果: 例外がスローされない
- 備考: 防御的プログラミングの検証

## 既存テストへの影響

既存のテストケースは変更不要：
- `SaveDataCheckerTest.cs` - 変更なし
- `TitleMenuControllerTest.cs` - 変更なし
- `TitleSceneTransitionTest.cs` - 変更なし

これらは単体テストとして独立しており、TitleSceneInitializerはこれらを統合する役割のみを持つため、既存テストには影響しない。
