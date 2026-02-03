using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace TitleScreen
{
    [TestFixture]
    public class TitleMenuControllerTest
    {
        private GameObject _sutObject;
        private TitleMenuController _sut;
        private Button _newGameButton;
        private Button _continueButton;
        private Button _settingsButton;

        [SetUp]
        public void SetUp()
        {
            _sutObject = CreateSystemUnderTestObject();
            _sut = _sutObject.GetComponent<TitleMenuController>();
            _newGameButton = _sut.NewGameButton;
            _continueButton = _sut.ContinueButton;
            _settingsButton = _sut.SettingsButton;
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_sutObject);
        }

        private GameObject CreateSystemUnderTestObject()
        {
            var gameObject = new GameObject("TitleMenuController");
            var controller = gameObject.AddComponent<TitleMenuController>();

            var newGameButtonObj = CreateButtonObject("NewGameButton");
            newGameButtonObj.transform.SetParent(gameObject.transform);
            SetPrivateSerializedField(controller, "<NewGameButton>k__BackingField", newGameButtonObj.GetComponent<Button>());

            var continueButtonObj = CreateButtonObject("ContinueButton");
            continueButtonObj.transform.SetParent(gameObject.transform);
            SetPrivateSerializedField(controller, "<ContinueButton>k__BackingField", continueButtonObj.GetComponent<Button>());

            var settingsButtonObj = CreateButtonObject("SettingsButton");
            settingsButtonObj.transform.SetParent(gameObject.transform);
            SetPrivateSerializedField(controller, "<SettingsButton>k__BackingField", settingsButtonObj.GetComponent<Button>());

            return gameObject;
        }

        private GameObject CreateButtonObject(string name)
        {
            var buttonObj = new GameObject(name);
            buttonObj.AddComponent<Image>();
            buttonObj.AddComponent<Button>();
            return buttonObj;
        }

        private void SetPrivateSerializedField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(obj, value);
        }

        // TMC-001: On initialization, NewGame button is enabled
        [UnityTest]
        public IEnumerator Initialize_OnInitialization_NewGameButtonIsEnabled()
        {
            yield return null;

            var stubSaveDataChecker = new StubSaveDataChecker(false);

            _sut.Initialize(stubSaveDataChecker);

            Assert.That(_newGameButton.interactable, Is.True);
        }

        // TMC-002: On initialization, Settings button is enabled
        [UnityTest]
        public IEnumerator Initialize_OnInitialization_SettingsButtonIsEnabled()
        {
            yield return null;

            var stubSaveDataChecker = new StubSaveDataChecker(false);

            _sut.Initialize(stubSaveDataChecker);

            Assert.That(_settingsButton.interactable, Is.True);
        }

        // TMC-003: When save data exists, Continue button is enabled
        [UnityTest]
        public IEnumerator Initialize_WhenSaveDataExists_ContinueButtonIsEnabled()
        {
            yield return null;

            var stubSaveDataChecker = new StubSaveDataChecker(true);

            _sut.Initialize(stubSaveDataChecker);

            Assert.That(_continueButton.interactable, Is.True);
        }

        // TMC-004: When save data does not exist, Continue button is disabled
        [UnityTest]
        public IEnumerator Initialize_WhenSaveDataDoesNotExist_ContinueButtonIsDisabled()
        {
            yield return null;

            var stubSaveDataChecker = new StubSaveDataChecker(false);

            _sut.Initialize(stubSaveDataChecker);

            Assert.That(_continueButton.interactable, Is.False);
        }

        // TMC-005: When NewGame button is pressed, OnNewGameSelected event fires
        [UnityTest]
        public IEnumerator NewGameButton_WhenPressed_OnNewGameSelectedEventFires()
        {
            yield return null;

            var stubSaveDataChecker = new StubSaveDataChecker(false);
            _sut.Initialize(stubSaveDataChecker);
            var eventFired = false;
            _sut.OnNewGameSelected += () => eventFired = true;

            _newGameButton.onClick.Invoke();

            Assert.That(eventFired, Is.True);
        }

        // TMC-006: When Continue button is pressed (enabled), OnContinueSelected event fires
        [UnityTest]
        public IEnumerator ContinueButton_WhenPressedAndEnabled_OnContinueSelectedEventFires()
        {
            yield return null;

            var stubSaveDataChecker = new StubSaveDataChecker(true);
            _sut.Initialize(stubSaveDataChecker);
            var eventFired = false;
            _sut.OnContinueSelected += () => eventFired = true;

            _continueButton.onClick.Invoke();

            Assert.That(eventFired, Is.True);
        }

        // TMC-007: When Settings button is pressed, OnSettingsSelected event fires
        [UnityTest]
        public IEnumerator SettingsButton_WhenPressed_OnSettingsSelectedEventFires()
        {
            yield return null;

            var stubSaveDataChecker = new StubSaveDataChecker(false);
            _sut.Initialize(stubSaveDataChecker);
            var eventFired = false;
            _sut.OnSettingsSelected += () => eventFired = true;

            _settingsButton.onClick.Invoke();

            Assert.That(eventFired, Is.True);
        }
    }
}
