# タイトル画面ボタン修正 仕様書

## 問題

タイトル画面の実装において、仕様で定義された3つのメニューボタンがシーンに配置されていない。

### 現状

- シーン（TitleScene.unity）には「StartButton」（テキスト: "Start"）が1つのみ存在
- `TitleMenuController.cs` は3ボタン対応で実装済みだが、シーンに配置されていない
- 古い `TitleManager.cs` がシーンにアタッチされている

### 期待される状態

仕様書 `specs/2026-02-03-title-screen.md` に基づき、以下の3つのボタンが表示される：

| ボタン | テキスト | 遷移先 |
|-------|---------|-------|
| NewGameButton | 新規ゲーム | シナリオ選択画面 |
| ContinueButton | 続きから | 最新セーブをロード（セーブがない場合は非活性） |
| SettingsButton | 設定 | モーダル表示 |

## 仕様

### UI構成

Canvas 内に以下の要素を配置する：

1. Background - 背景（既存）
2. TitleText - タイトルロゴ（既存）
3. NewGameButton - 新規ゲームボタン
4. ContinueButton - 続きからボタン
5. SettingsButton - 設定ボタン

### ボタン配置

- 画面中央に縦並び
- ボタン間隔: 20px
- ボタンサイズ: 300x80

### ボタンの状態

- NewGameButton: 常に活性
- ContinueButton: セーブデータの有無で活性/非活性が決定
- SettingsButton: 常に活性

## 設計

### 削除対象

- `Assets/Scripts/TitleManager.cs` - 不要な古い実装
- シーン内の `StartButton` オブジェクト
- シーン内の `TitleManager` オブジェクト

### 変更対象

TitleScene.unity を以下のように変更：

1. Canvas に `TitleMenuController` コンポーネントをアタッチ
2. 3つのボタン（NewGameButton, ContinueButton, SettingsButton）を追加
3. 各ボタンを `TitleMenuController` の対応するフィールドに設定

### ファイル構成

```
CityInc/Assets/
├── Scenes/
│   └── TitleScene.unity          # 修正
├── Scripts/
│   └── TitleManager.cs           # 削除
└── TitleScreen/
    └── Scripts/Runtime/
        └── TitleMenuController.cs # 既存（変更なし）
```

## テストケース

### TitleSceneIntegration

シーン構成が仕様通りであることをインテグレーションテストで検証する。

| ID | テスト内容 | テスト技法 |
|----|-----------|-----------|
| TSI-010 | TitleScene をロードすると、TitleMenuController コンポーネントが存在する | 状態テスト |
| TSI-020 | TitleScene をロードすると、NewGameButton が TitleMenuController に設定されている | 状態テスト |
| TSI-030 | TitleScene をロードすると、ContinueButton が TitleMenuController に設定されている | 状態テスト |
| TSI-040 | TitleScene をロードすると、SettingsButton が TitleMenuController に設定されている | 状態テスト |
