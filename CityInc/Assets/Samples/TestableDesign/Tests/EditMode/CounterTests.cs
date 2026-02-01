using System;
using System.Collections.Generic;
using NUnit.Framework;
using Samples.TestableDesign;
using Samples.TestableDesign.PureLogic;

namespace Samples.TestableDesign.Tests.EditMode
{
    /// <summary>
    /// Counterクラスの単体テスト。
    /// Unity Test Framework（NUnit）を使用したEdit Modeテストの例。
    ///
    /// テストの構成：Arrange-Act-Assert（AAA）パターンを使用
    /// </summary>
    [TestFixture]
    public class CounterTests
    {
        private MockDataStore _mockDataStore;
        private Counter _counter;

        [SetUp]
        public void SetUp()
        {
            // Arrange: 各テストの前にモックとカウンターを初期化
            _mockDataStore = new MockDataStore();
            _counter = new Counter(_mockDataStore);
        }

        #region Constructor Tests

        [Test]
        public void Constructor_WithValidDataStore_InitializesToZero()
        {
            // Arrange & Act: SetUpで実行済み

            // Assert
            Assert.AreEqual(0, _counter.Value);
        }

        [Test]
        public void Constructor_WithInitialValue_SetsValue()
        {
            // Arrange & Act
            var counter = new Counter(_mockDataStore, initialValue: 42);

            // Assert
            Assert.AreEqual(42, counter.Value);
        }

        [Test]
        public void Constructor_WithNullDataStore_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Counter(null));
        }

        #endregion

        #region Add Tests

        [Test]
        public void Add_PositiveAmount_IncreasesValue()
        {
            // Arrange: 初期値0

            // Act
            _counter.Add(5);

            // Assert
            Assert.AreEqual(5, _counter.Value);
        }

        [Test]
        public void Add_MultipleTimes_AccumulatesValue()
        {
            // Act
            _counter.Add(3);
            _counter.Add(7);

            // Assert
            Assert.AreEqual(10, _counter.Value);
        }

        [Test]
        public void Add_Zero_ValueUnchanged()
        {
            // Arrange
            _counter.Add(5);

            // Act
            _counter.Add(0);

            // Assert
            Assert.AreEqual(5, _counter.Value);
        }

        [Test]
        public void Add_NegativeAmount_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _counter.Add(-1));
        }

        #endregion

        #region Reset Tests

        [Test]
        public void Reset_AfterAdd_SetsValueToZero()
        {
            // Arrange
            _counter.Add(100);

            // Act
            _counter.Reset();

            // Assert
            Assert.AreEqual(0, _counter.Value);
        }

        #endregion

        #region Save/Load Tests

        [Test]
        public void Save_CallsDataStoreWithCorrectKeyAndValue()
        {
            // Arrange
            _counter.Add(42);

            // Act
            _counter.Save();

            // Assert
            Assert.AreEqual("counter_value", _mockDataStore.LastSavedKey);
            Assert.AreEqual(42, _mockDataStore.LastSavedValue);
        }

        [Test]
        public void Load_RestoresValueFromDataStore()
        {
            // Arrange
            _mockDataStore.SetValueToReturn("counter_value", 99);

            // Act
            _counter.Load();

            // Assert
            Assert.AreEqual(99, _counter.Value);
        }

        [Test]
        public void SaveAndLoad_RoundTrip_PreservesValue()
        {
            // Arrange
            _counter.Add(123);

            // Act
            _counter.Save();
            _counter.Reset(); // 値をクリア
            _counter.Load();  // 復元

            // Assert
            Assert.AreEqual(123, _counter.Value);
        }

        #endregion
    }

    /// <summary>
    /// テスト用のIDataStoreモック実装。
    /// 外部依存（PlayerPrefs等）なしでテスト可能。
    /// </summary>
    public class MockDataStore : IDataStore
    {
        private readonly Dictionary<string, object> _storage = new Dictionary<string, object>();

        // テスト検証用のプロパティ
        public string LastSavedKey { get; private set; }
        public object LastSavedValue { get; private set; }

        public void Save<T>(string key, T value)
        {
            LastSavedKey = key;
            LastSavedValue = value;
            _storage[key] = value;
        }

        public T Load<T>(string key)
        {
            if (_storage.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            return default;
        }

        public void Delete(string key)
        {
            _storage.Remove(key);
        }

        public bool Exists(string key)
        {
            return _storage.ContainsKey(key);
        }

        /// <summary>
        /// テスト用：特定のキーに対して返す値を設定
        /// </summary>
        public void SetValueToReturn<T>(string key, T value)
        {
            _storage[key] = value;
        }
    }
}
