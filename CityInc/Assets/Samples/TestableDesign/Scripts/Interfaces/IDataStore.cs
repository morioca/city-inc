namespace Samples.TestableDesign
{
    /// <summary>
    /// データ永続化のためのインターフェース。
    /// 依存性注入（DI）により、テスト時にモック実装を使用可能。
    ///
    /// 本番環境: PlayerPrefsDataStore（PlayerPrefsを使用）
    /// テスト環境: MockDataStore（メモリ内で完結）
    /// </summary>
    public interface IDataStore
    {
        /// <summary>
        /// 指定したキーでデータを保存。
        /// </summary>
        /// <typeparam name="T">保存するデータの型</typeparam>
        /// <param name="key">一意のキー</param>
        /// <param name="value">保存する値</param>
        void Save<T>(string key, T value);

        /// <summary>
        /// 指定したキーからデータを読み込み。
        /// </summary>
        /// <typeparam name="T">読み込むデータの型</typeparam>
        /// <param name="key">一意のキー</param>
        /// <returns>保存されていた値（存在しない場合はdefault(T)）</returns>
        T Load<T>(string key);

        /// <summary>
        /// 指定したキーのデータを削除。
        /// </summary>
        /// <param name="key">削除するキー</param>
        void Delete(string key);

        /// <summary>
        /// 指定したキーのデータが存在するか確認。
        /// </summary>
        /// <param name="key">確認するキー</param>
        /// <returns>存在する場合true</returns>
        bool Exists(string key);
    }
}
