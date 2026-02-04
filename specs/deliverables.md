# 開発サマリー

## 実装済み機能

### タイトル画面（TitleScene）
- 3つのメニューボタン
  - 新規ゲーム: メインゲーム画面へ遷移
  - 続きから: 最新セーブをロード（セーブがない場合は非活性）
  - 設定: モーダル表示
- セーブデータの有無による「続きから」ボタンの状態制御

### メインゲーム画面（MainGameScene）
- 白紙の画面として作成（今後UI要素を追加予定）
- タイトル画面の「新規ゲーム」から遷移

### 実装コンポーネント
- `SaveDataChecker` - セーブデータ存在確認
- `TitleMenuController` - メニュー状態管理・イベント発火
- `TitleSceneTransition` - 画面遷移制御（MainGameScene への遷移）
- `UnitySceneTransitioner` - Unity SceneManagerのラッパー（入力検証付き）
- `TitleSceneInitializer` - タイトル画面の初期化と各コンポーネントの統合
