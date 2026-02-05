# 開発サマリー

## 実装済み機能

### タイトル画面（TitleScene）
- 3つのメニューボタン
  - 新規ゲーム: メインゲーム画面へ遷移
  - 続きから: 最新セーブをロード（セーブがない場合は非活性）
  - 設定: モーダル表示
- セーブデータの有無による「続きから」ボタンの状態制御

### メインゲーム画面（MainGameScene）
- タイトル画面の「新規ゲーム」から遷移
- 月次ターン進行システム
  - 現在の年月を「YYYY年MM月」形式で表示
  - 「次の月へ」ボタンで1ヶ月進める（12月→翌年1月の年跨ぎ対応）

### 実装コンポーネント

#### TitleScreen
- `SaveDataChecker` - セーブデータ存在確認
- `TitleMenuController` - メニュー状態管理・イベント発火
- `TitleSceneTransition` - 画面遷移制御（MainGameScene への遷移）
- `UnitySceneTransitioner` - Unity SceneManagerのラッパー（入力検証付き）
- `TitleSceneInitializer` - タイトル画面の初期化と各コンポーネントの統合

#### MainGame
- `GameDate` - ゲーム内年月を表現するイミュータブルな値オブジェクト
- `GameState` - ゲーム全体の状態を保持するイミュータブルなオブジェクト
- `TurnManager` - ターン進行ロジック（純粋C#クラス）
- `GameStatePresenter` - ゲーム状態のUI反映（MonoBehaviour）
