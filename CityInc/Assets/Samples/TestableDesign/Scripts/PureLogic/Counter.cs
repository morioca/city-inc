using System;

namespace Samples.TestableDesign.PureLogic
{
    /// <summary>
    /// 純粋なC#クラスの例。
    /// MonoBehaviourに依存せず、テストが容易。
    /// </summary>
    public class Counter
    {
        private readonly IDataStore _dataStore;

        public int Value { get; private set; }

        /// <summary>
        /// コンストラクタで依存性を注入（DI）。
        /// テスト時にモックを渡すことが可能。
        /// </summary>
        public Counter(IDataStore dataStore, int initialValue = 0)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            Value = initialValue;
        }

        /// <summary>
        /// 指定した値を加算する純粋な操作。
        /// </summary>
        public void Add(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be non-negative", nameof(amount));

            Value += amount;
        }

        /// <summary>
        /// カウンターをリセット。
        /// </summary>
        public void Reset()
        {
            Value = 0;
        }

        /// <summary>
        /// 現在の値を永続化。
        /// IDataStore経由で外部依存を抽象化。
        /// </summary>
        public void Save()
        {
            _dataStore.Save("counter_value", Value);
        }

        /// <summary>
        /// 保存された値を読み込み。
        /// </summary>
        public void Load()
        {
            Value = _dataStore.Load<int>("counter_value");
        }
    }
}
