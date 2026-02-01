using UnityEngine;
using UnityEngine.UI;
using Samples.TestableDesign.PureLogic;

namespace Samples.TestableDesign.MonoBehaviours
{
    /// <summary>
    /// MonoBehaviourはUIの橋渡しのみを担当。
    /// ビジネスロジックはCounter（純粋C#クラス）に委譲。
    ///
    /// この設計により：
    /// - Counterクラスは単体テスト可能
    /// - ViewはUnity依存の処理に専念
    /// - 関心の分離が明確
    /// </summary>
    public class CounterView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Text _valueText;
        [SerializeField] private Button _addButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;

        [Header("Settings")]
        [SerializeField] private int _incrementAmount = 1;

        private Counter _counter;
        private IDataStore _dataStore;

        private void Start()
        {
            // 本番環境では実際のデータストア実装を使用
            _dataStore = new PlayerPrefsDataStore();
            _counter = new Counter(_dataStore);

            SetupButtonListeners();
            UpdateDisplay();
        }

        /// <summary>
        /// テスト用や外部からの依存性注入用。
        /// </summary>
        public void Initialize(IDataStore dataStore, int initialValue = 0)
        {
            _dataStore = dataStore;
            _counter = new Counter(_dataStore, initialValue);
            UpdateDisplay();
        }

        private void SetupButtonListeners()
        {
            _addButton?.onClick.AddListener(OnAddClicked);
            _resetButton?.onClick.AddListener(OnResetClicked);
            _saveButton?.onClick.AddListener(OnSaveClicked);
            _loadButton?.onClick.AddListener(OnLoadClicked);
        }

        private void OnAddClicked()
        {
            _counter.Add(_incrementAmount);
            UpdateDisplay();
        }

        private void OnResetClicked()
        {
            _counter.Reset();
            UpdateDisplay();
        }

        private void OnSaveClicked()
        {
            _counter.Save();
            Debug.Log($"Counter saved: {_counter.Value}");
        }

        private void OnLoadClicked()
        {
            _counter.Load();
            UpdateDisplay();
            Debug.Log($"Counter loaded: {_counter.Value}");
        }

        private void UpdateDisplay()
        {
            if (_valueText != null)
            {
                _valueText.text = _counter.Value.ToString();
            }
        }

        private void OnDestroy()
        {
            _addButton?.onClick.RemoveListener(OnAddClicked);
            _resetButton?.onClick.RemoveListener(OnResetClicked);
            _saveButton?.onClick.RemoveListener(OnSaveClicked);
            _loadButton?.onClick.RemoveListener(OnLoadClicked);
        }
    }

    /// <summary>
    /// PlayerPrefsを使用したIDataStoreの実装。
    /// 本番環境で使用。
    /// </summary>
    public class PlayerPrefsDataStore : IDataStore
    {
        public void Save<T>(string key, T value)
        {
            var json = JsonUtility.ToJson(new Wrapper<T> { Value = value });
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public T Load<T>(string key)
        {
            if (!PlayerPrefs.HasKey(key))
                return default;

            var json = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<Wrapper<T>>(json).Value;
        }

        public void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public bool Exists(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T Value;
        }
    }
}
