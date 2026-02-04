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
- `TitleSceneTransition` (Scripts/Runtime/SceneTransition/TitleSceneTransition.cs)
  - `OnNewGameRequested()` メソッドの遷移先を変更
  - 変更前: `"ScenarioSelectionScene"`
  - 変更後: `"MainGameScene"`

#### 新規作成
- `MainGameScene.unity`
  - 空のSceneとして作成
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
3. `TitleSceneTransition.cs` の遷移先を変更
4. AssetDatabase.Refresh() を実行

### テスト方針
- Edit Modeテストでの検証は不要（Scene遷移のみ）
- 手動テスト:
  1. TitleScene を再生
  2. 「新規ゲーム」ボタンをクリック
  3. MainGameScene に遷移することを確認
  4. コンソールエラーが出ないことを確認

## 今後の拡張
本仕様実装後、以下を段階的に追加:
1. メインゲーム画面のUI配置（マップビュー、ヘッダー等）
2. ゲーム状態管理（GameState）
3. ターン進行システム（TurnManager）
4. 予算・政策システム
