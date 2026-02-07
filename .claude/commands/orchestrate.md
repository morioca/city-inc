---
skill: orchestrate
description: 仕様作成からテスト設計、実装、レビューまでを自動実行する指揮者スキル
invocable: user
arguments:
  topic:
    description: 実装するトピック名（オプション、省略時は会話から推測）
    required: false
---

# Orchestrate - Development Workflow Automation

あなたは開発チームを統括する指揮者です。以下のワークフローを自動実行し、仕様作成から実装・テスト・レビューまでを完了させます。

## ワークフロー

```
1. 前提条件チェック
   ↓
2. 仕様作成 (spec-writer)
   ↓
3. テストケース生成 (test-case-generator)
   ↓
4. 実装サイクル (implementer ↔ orchestrator)
   a. スケルトン + テストコード実装
   b. テスト実行（失敗確認）
   c. プロダクトコード実装
   d. テスト実行（成功確認）
   e. リファクタリング
   f. テスト実行（再確認）
   ↓
5. コードレビュー (reviewer)
   ↓
6. 結果報告
```

## 実行手順

### Phase 0: 前提条件チェック

会話履歴を確認し、以下が明確かをチェック：
- 実装する機能の要件（なぜ）
- 実装する機能の仕様（何を）
- 実装する機能の設計（どのように）

不明確な場合は、ユーザーに確認してから続行してください。

---

### Phase 1: 仕様作成

Task tool を使用して spec-writer エージェントを起動：

```typescript
Task({
  subagent_type: "spec-writer",
  description: "Create specification document",
  prompt: `会話履歴から要件・仕様・設計を抽出し、仕様書を作成してください。

${topic ? `トピック名: ${topic}` : ''}

以下の情報をJSON形式で出力してください：
{
  "status": "success" | "failed",
  "specFilePath": "specs/yyyy-mm-dd-topic.md",
  "message": "メッセージ"
}`
})
```

**結果処理**:
- `status: "failed"` → ユーザーに報告、中止
- `status: "success"` → `specFilePath` を保存、Phase 2へ

---

### Phase 2: テストケース生成

Task tool を使用して test-case-generator エージェントを起動：

```typescript
Task({
  subagent_type: "test-case-generator",
  description: "Generate test cases",
  prompt: `仕様書からテストケースを導出し、仕様書に追加してください。

仕様書パス: ${specFilePath}

以下の情報をJSON形式で出力してください：
{
  "status": "success" | "failed",
  "specFilePath": "specs/yyyy-mm-dd-topic.md",
  "testCaseCount": 10,
  "message": "メッセージ"
}`
})
```

**結果処理**:
- `status: "failed"` → ユーザーに報告、中止
- `status: "success"` → Phase 3へ

---

### Phase 3: 実装サイクル

実装サイクルは3つのサブフェーズで構成されます。

#### Phase 3-a: スケルトン + テストコード実装

Task tool を使用して implementer エージェントを起動：

```typescript
Task({
  subagent_type: "implementer",
  description: "Implement skeleton and tests",
  prompt: `スケルトンコードとテストコードを実装してください。

仕様書パス: ${specFilePath}
フェーズ: skeleton-and-test

以下の情報をJSON形式で出力してください：
{
  "status": "ready-for-test" | "completed",
  "implementedFiles": ["..."],
  "expectation": "expect-test-failure",
  "message": "メッセージ",
  "phase": "skeleton-and-test"
}`
})
```

**結果処理**:
- `status: "completed"` → エラーあり、ユーザーに報告、中止
- `status: "ready-for-test"` → Phase 3-bへ

#### Phase 3-b: テスト実行（失敗確認）

Task tool を使用して test-runner エージェントを起動：

```typescript
Task({
  subagent_type: "test-runner",
  description: "Run tests (expect failure)",
  prompt: `実装されたテストを実行してください。

実装ファイル: ${implementedFiles.join(', ')}

テストが失敗することを確認してください（NotImplementedException）。`
})
```

**結果処理**:
- テストが成功してしまった → 異常、ユーザーに報告、指示を仰ぐ
- テストが失敗（NotImplementedException） → 期待通り、Phase 3-cへ
- その他のエラー → ユーザーに報告、指示を仰ぐ

#### Phase 3-c: プロダクトコード実装

Task tool を使用して implementer エージェントを起動：

```typescript
Task({
  subagent_type: "implementer",
  description: "Implement production code",
  prompt: `プロダクトコードを実装してください。

仕様書パス: ${specFilePath}
フェーズ: implement
テスト結果: ${JSON.stringify(testResult)}

以下の情報をJSON形式で出力してください：
{
  "status": "ready-for-test" | "completed",
  "implementedFiles": ["..."],
  "expectation": "expect-test-success",
  "message": "メッセージ",
  "phase": "implement"
}`
})
```

**結果処理**:
- `status: "completed"` → エラーあり、ユーザーに報告、中止
- `status: "ready-for-test"` → Phase 3-dへ

#### Phase 3-d: テスト実行（成功確認）

Task tool を使用して test-runner エージェントを起動：

```typescript
Task({
  subagent_type: "test-runner",
  description: "Run tests (expect success)",
  prompt: `実装されたテストを実行してください。

実装ファイル: ${implementedFiles.join(', ')}

すべてのテストが成功することを確認してください。`
})
```

**結果処理**:
- テストが失敗 → implementer に戻って修正（最大2回リトライ）
- テストが成功 → Phase 3-eへ

#### Phase 3-e: リファクタリング

Task tool を使用して implementer エージェントを起動：

```typescript
Task({
  subagent_type: "implementer",
  description: "Refactor code",
  prompt: `リファクタリングと最終調整を行ってください。

仕様書パス: ${specFilePath}
フェーズ: refactor
テスト結果: ${JSON.stringify(testResult)}

以下の情報をJSON形式で出力してください：
{
  "status": "ready-for-test" | "completed",
  "implementedFiles": ["..."],
  "expectation": "expect-test-success",
  "message": "メッセージ",
  "phase": "refactor"
}`
})
```

**結果処理**:
- `status: "completed"` → エラーあり、ユーザーに報告、中止
- `status: "ready-for-test"` → Phase 3-fへ

#### Phase 3-f: テスト実行（再確認）

Task tool を使用して test-runner エージェントを起動：

```typescript
Task({
  subagent_type: "test-runner",
  description: "Run tests (reconfirm)",
  prompt: `リファクタリング後のテストを実行してください。

実装ファイル: ${implementedFiles.join(', ')}

すべてのテストが引き続き成功することを確認してください。`
})
```

**結果処理**:
- テストが失敗 → ユーザーに報告、指示を仰ぐ
- テストが成功 → Phase 4へ

---

### Phase 4: コードレビュー

Task tool を使用して reviewer エージェントを起動：

```typescript
Task({
  subagent_type: "reviewer",
  description: "Review code quality",
  prompt: `実装されたコードをレビューしてください。

仕様書パス: ${specFilePath}
実装ファイル: ${implementedFiles.join(', ')}

以下の情報をJSON形式で出力してください：
{
  "status": "APPROVED" | "APPROVED_WITH_WARNINGS" | "CHANGES_REQUIRED",
  "criticalIssues": 0,
  "warnings": 2,
  "reviewReport": "...",
  "message": "メッセージ"
}`
})
```

**結果処理**:
- `status: "CHANGES_REQUIRED"` (Critical issues > 0) → レビューレポートをユーザーに提示、中止
- `status: "APPROVED_WITH_WARNINGS"` → レビューレポートをユーザーに提示、Phase 5へ
- `status: "APPROVED"` → Phase 5へ

---

### Phase 5: 完了報告

ユーザーに以下のサマリーを報告：

```markdown
# 🎉 開発ワークフロー完了

## 実行結果

- **仕様書**: ${specFilePath}
- **テストケース数**: ${testCaseCount}
- **実装ファイル**: ${implementedFiles.length}件
- **テスト結果**: すべて成功
- **レビュー結果**: ${reviewStatus}
  - Critical Issues: ${criticalIssues}
  - Warnings: ${warnings}

## 実装されたファイル

${implementedFiles.map(f => `- ${f}`).join('\n')}

## レビューレポート

${reviewReport}

## 次のステップ

${reviewStatus === 'APPROVED'
  ? '実装が完了しました。動作確認を行ってください。'
  : 'Warningがあります。必要に応じて修正を検討してください。'}
```

---

## エラーハンドリング

### エージェント失敗時

各エージェントが失敗した場合：
1. エラーメッセージをキャプチャ
2. リトライカウンタを確認
3. リトライ回数 < 2 → 同じ入力で再実行
4. リトライ回数 == 2 → ユーザーに報告、手動介入を提案

**報告形式**:
```markdown
## ⚠️ エージェント実行エラー

**エージェント**: ${agentType}
**フェーズ**: ${phase}
**エラー**: ${errorMessage}

**これまでの進捗**:
- ✅ Phase 1: 仕様作成完了 (${specFilePath})
- ✅ Phase 2: テストケース生成完了
- ❌ Phase 3-c: プロダクトコード実装で失敗

**次のステップ**:
1. エラー内容を確認してください
2. 必要に応じて手動で修正してください
3. 続行する場合は、以下のコマンドで再開できます:
   - `/implement-code ${specFilePath}` - 実装のみ再実行
   - `/orchestrate` - 最初から再実行
```

### テスト結果が期待と異なる場合

#### Phase 3-b でテストが成功してしまった場合

```markdown
## ⚠️ 予期しないテスト成功

Phase 3-b（スケルトン実装後）でテストが成功しました。
通常は `NotImplementedException` でテストが失敗するはずです。

**考えられる原因**:
1. テストケースが不十分（実装なしでもパスする）
2. スケルトンに実装が含まれている

**次のステップ**:
1. テストコードを確認してください
2. 実装を続行しますか？ (Y/n)
```

#### Phase 3-d でテストが失敗した場合

```markdown
## ⚠️ テスト失敗

Phase 3-d（プロダクトコード実装後）でテストが失敗しました。

**失敗したテスト**:
${failedTests.map(t => `- ${t.name}: ${t.error}`).join('\n')}

**リトライ回数**: ${retryCount} / 2

**次のステップ**:
- 自動修正を試行します...
```

### Critical Issues がある場合

```markdown
## ⚠️ コードレビューでCritical Issuesが見つかりました

**Critical Issues**: ${criticalIssues}件
**Warnings**: ${warnings}件

レビューレポートを確認し、修正してください。

${reviewReport}

**次のステップ**:
1. 上記の Critical Issues を修正してください
2. 修正後、以下のコマンドで再レビューできます:
   - `/orchestrate` - 実装から再実行
3. または手動で修正し、git commit してください
```

---

## 実行例

### 成功ケース

```
User: /orchestrate タイトル画面のボタンアニメーション

Orchestrator:
✅ Phase 1: 仕様書作成完了 (specs/2026-02-07-title-button-animation.md)
✅ Phase 2: テストケース生成完了 (8件)
✅ Phase 3-a: スケルトン + テストコード実装完了
✅ Phase 3-b: テスト実行（失敗確認）
✅ Phase 3-c: プロダクトコード実装完了
✅ Phase 3-d: テスト実行（成功確認）
✅ Phase 3-e: リファクタリング完了
✅ Phase 3-f: テスト実行（再確認）
✅ Phase 4: コードレビュー完了 (APPROVED_WITH_WARNINGS)

# 🎉 開発ワークフロー完了
[サマリー表示]
```

### 失敗ケース（要件不明確）

```
User: /orchestrate

Orchestrator:
❌ 要件が不明確です。以下を明確にしてください：
- 実装する機能は何ですか？
- どのような動作が期待されますか？
- どのようなクラス・メソッドが必要ですか？

会話で要件を明確にしてから、再度 `/orchestrate` を実行してください。
```

---

## 注意事項

- **テスト実行タイミング**: implementer はテスト実行せず、orchestrator が test-runner を呼び出します
- **リトライ管理**: 各エージェントで最大2回までリトライします
- **状態管理**: 各フェーズの結果を保存し、次のフェーズに渡します
- **エラー報告**: エラー時は進捗状況と次のステップを明確に提示します
- **後方互換性**: 既存スキル（`/create-doc`, `/implement-code` など）は引き続き単独実行可能です

---

## 拡張可能性

将来的に以下の拡張が可能：
- 自動修正エージェント（reviewer の問題を自動修正）
- 中断・再開機能（State Machine化）
- 並行実行（独立したテストの並列実行）
- カスタムワークフロー（ユーザー定義のフェーズ順序）
