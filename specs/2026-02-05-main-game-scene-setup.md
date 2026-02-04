# メインゲーム画面の初期セットアップ

## 要件

### 背景
- Phase 1（MVP）の実装を進めるにあたり、シナリオ選択画面を一旦スキップして開発を簡略化する
- タイトル画面からメインゲーム画面への動線を確立し、コアループの実装に集中する
- まずは空の画面を用意し、段階的に機能を追加していく

### 目的
- タイトル画面の「新規ゲーム」ボタンから直接メインゲーム画面へ遷移できるようにする
- メインゲーム画面の器を用意し、今後の実装基盤を整える

## 仕様

### 機能要件

#### メインゲーム画面（MainGameScene）
- 白紙の画面として初期作成
- Scene名: `MainGameScene`
- 今後、以下の要素を段階的に追加予定（本仕様では未実装）:
  - マップビュー
  - 支持率・財政状況表示
  - 「次の月へ」ボタン
  - メニューアイコン

#### タイトル画面からの遷移
- 「新規ゲーム」ボタンクリック時、MainGameSceneへ遷移
- シナリオ選択画面（ScenarioSelectionScene）への遷移は一旦削除

### 非機能要件
- Scene遷移は既存の`TitleSceneTransition`を活用
- ゲームプレイロジックは本仕様では未実装

## 設計

### Scene構成
```
Scenes/
├── TitleScene.unity          # 既存
└── MainGameScene.unity       # 新規作成（白紙）
```

### コンポーネント設計

#### 変更対象
- `TitleSceneTransition` (Assets/TitleScreen/Scripts/Runtime/TitleSceneTransition.cs)
  - 定数 `ScenarioSelectSceneName` の値を変更
    - 変更前: `"ScenarioSelectScene"`
    - 変更後: `"MainGameScene"`
  - `TransitionToScenarioSelect()` メソッドは変更なし（内部で使用する定数の値が変わることで遷移先が変わる）
  - `TransitionToGameWithLatestSave()` メソッドは変更なし

#### 新規作成
- `MainGameScene.unity`
  - 空のSceneとして `CityInc/Assets/Scenes/` に作成
  - Build Settingsに追加

### クラス図
```
TitleMenuController
  └─ NewGameRequested (event)
       └─ TitleSceneTransition.OnNewGameRequested()
            └─ SceneManager.LoadScene("MainGameScene")
```

### 実装手順
1. `MainGameScene.unity` を `CityInc/Assets/Scenes/` 配下に作成
2. Build Settings に MainGameScene を追加
3. `TitleSceneTransition.cs` の `ScenarioSelectSceneName` 定数の値を `"MainGameScene"` に変更
4. `TitleSceneTransitionTest.cs` の既存テスト TST-001 の期待値を更新
5. AssetDatabase.Refresh() を実行してコンパイル
6. テストを実行して検証

### テスト方針
- Edit Modeテストでの検証は不要（Scene遷移のみ）
- 手動テスト:
  1. TitleScene を再生
  2. 「新規ゲーム」ボタンをクリック
  3. MainGameScene に遷移することを確認
  4. コンソールエラーが出ないことを確認

## テストケース

### テスト対象クラス: TitleSceneTransition

#### テスト技法
- 同値分割法: メソッド呼び出しによる遷移先の検証
- 状態遷移テスト: シーン遷移の動作検証

#### テストケース一覧

| ID | テストケース | 検証内容 |
|----|------------|---------|
| MGS-001 | TransitionToScenarioSelect_WhenCalled_TransitionsToMainGameScene | `TransitionToScenarioSelect()` 呼び出し時、MainGameScene へ遷移することを検証 |
| MGS-010 | TransitionToGameWithLatestSave_WhenCalled_TransitionsToGameScene | `TransitionToGameWithLatestSave()` 呼び出し時、GameScene へ遷移することを検証（変更なし） |

#### 詳細

**MGS-001: TransitionToScenarioSelect_WhenCalled_TransitionsToMainGameScene**
- 目的: 「新規ゲーム」ボタンから MainGameScene への遷移を検証
- 前提条件: TitleSceneTransition が初期化されている
- 操作: `TransitionToScenarioSelect()` を呼び出す
- 期待結果: `SceneTransitioner.TransitionTo("MainGameScene")` が呼び出される
- 備考: 既存テストケース TST-001 を更新（遷移先を ScenarioSelectScene から MainGameScene に変更）

**MGS-010: TransitionToGameWithLatestSave_WhenCalled_TransitionsToGameScene**
- 目的: 「続きから」ボタンから GameScene への遷移を検証
- 前提条件: TitleSceneTransition が初期化されている
- 操作: `TransitionToGameWithLatestSave()` を呼び出す
- 期待結果: `SceneTransitioner.TransitionTo("GameScene")` が呼び出される
- 備考: 既存テストケース TST-002 と同一（変更なし）

## 今後の拡張
本仕様実装後、以下を段階的に追加:
1. メインゲーム画面のUI配置（マップビュー、ヘッダー等）
2. ゲーム状態管理（GameState）
3. ターン進行システム（TurnManager）
4. 予算・政策システム
