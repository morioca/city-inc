# NotoSansJP SDFフォントアセットの再生成手順

## 問題

現在の`NotoSansJP SDF.asset`には、都市ステータスバー表示に必要な文字が含まれていません。

### 不足している文字
- 人、口、財、政、支、持、率、円
- スペース、カンマ、パーセント記号
- 数字（0-9）

## 解決方法

TextMesh ProのFont Asset Creatorを使用してフォントアセットを再生成します。

### 手順

1. **Font Asset Creatorを開く**
   - Unity上部メニュー: `Window` > `TextMeshPro` > `Font Asset Creator`

2. **設定を行う**
   - **Source Font File**: `NotoSansJP.ttf` を選択
   - **Sampling Point Size**: `Auto Sizing`
   - **Padding**: `9`
   - **Packing Method**: `Fast`
   - **Atlas Resolution**: `1024 x 1024`
   - **Character Set**: `Custom Characters`を選択
   - **Custom Character List**に以下をコピー＆ペースト:
     ```
     人口財政支持率円年月0123456789 ,%次のへ
     ```

3. **フォントアセットを生成**
   - `Generate Font Atlas`ボタンをクリック
   - 生成が完了するまで待つ（数秒〜数十秒）

4. **保存**
   - `Save` または `Save as...`をクリック
   - 保存先: `Assets/TextMesh Pro/Fonts/NotoSansJP SDF.asset`（既存ファイルを上書き）

5. **確認**
   - ゲームを実行して、ステータスバーが正しく表示されることを確認
   - 表示されるべき内容:
     - 人口 50,000人
     - 財政 100,000,000円
     - 支持率 60%

## 備考

この手順は一度実行すれば、フォントアセットに必要な文字が永続的に含まれます。
