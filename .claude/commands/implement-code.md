$ARGUMENTS ファイルの仕様を満たすコードを実装する。

以下の手順に従うこと。

1. 仕様にテストケースが含まれていない場合、コマンドの実行を中止する
2. 製品コードに対して、コンパイル可能な型とパブリックメソッドシグネチャのみを作成する。動作しなくても問題ない
3. ドキュメントに記載されているテストケースに基づいてテストコードを実装する
4. 追加したテストを実行し、失敗することを確認する
5. Git にコミットする
6. 製品コードを実装する
7. テストを実行し、すべて合格する
8. Git にコミットする
9. KISS および SOLID 原則を念頭に置いてリファクタリングし、合格するようにテストを再実行する
10. 変更があればGit にコミットする
11. 開発で機能を追加した場合 `/specs/deliverables.md` を更新する

テスト実行は全て test-runner agent に委譲する。Task ツールで subagent_type="test-runner" を指定して実行すること。

## Test-Runner Agent の使用方法

各テスト実行ステップでは、以下のように test-runner agent を呼び出す:

- **Step 4** (テスト失敗確認):
  ```
  Task(subagent_type="test-runner", description="Run new tests",
       prompt="Run tests for [modified class name]. The tests should fail.")
  ```

- **Step 7** (全テスト合格確認):
  ```
  Task(subagent_type="test-runner", description="Verify all tests pass",
       prompt="Run all tests related to [feature/class]. All tests should pass.")
  ```

- **Step 9** (リファクタリング後テスト):
  ```
  Task(subagent_type="test-runner", description="Rerun tests after refactoring",
       prompt="Re-run tests for [modified class] to verify refactoring didn't break anything.")
  ```

Agent には変更したクラス名、名前空間、アセンブリ名を明示的に伝えること。Agent がテスト範囲を適切に決定できる。
