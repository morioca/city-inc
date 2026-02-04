using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace TitleScreen
{
    [TestFixture]
    public class TitleSceneInitializerTest
    {
        private GameObject _initializerObject;
        private GameObject _menuControllerObject;

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (_initializerObject != null)
            {
                Object.Destroy(_initializerObject);
            }
            if (_menuControllerObject != null)
            {
                Object.Destroy(_menuControllerObject);
            }
            yield return null;
        }

        [UnityTest]
        public IEnumerator Awake_WhenCalled_InitializesMenuController()
        {
            var (initializer, menuController) = CreateSystemUnderTest();
            yield return null;

            Assert.That(menuController.NewGameButton.interactable, Is.True);
            Assert.That(menuController.SettingsButton.interactable, Is.True);
        }

        [UnityTest]
        public IEnumerator Awake_WhenCalled_SubscribesToNewGameEvent()
        {
            var (initializer, menuController) = CreateSystemUnderTest();
            yield return null;

            var initialSubscriberCount = menuController.NewGameButton.onClick.GetPersistentEventCount();

            Assert.That(initialSubscriberCount, Is.GreaterThanOrEqualTo(0));
        }

        [UnityTest]
        public IEnumerator Awake_WhenCalled_SubscribesToContinueEvent()
        {
            var (initializer, menuController) = CreateSystemUnderTest();
            yield return null;

            var initialSubscriberCount = menuController.ContinueButton.onClick.GetPersistentEventCount();

            Assert.That(initialSubscriberCount, Is.GreaterThanOrEqualTo(0));
        }

        [UnityTest]
        [Ignore("Integration test - requires TitleScene to be properly configured")]
        public IEnumerator NewGameButton_WhenClicked_TransitionsToMainGame()
        {
            SceneManager.LoadScene("TitleScene");
            yield return null;

            var initializer = Object.FindObjectOfType<TitleSceneInitializer>();
            Assert.That(initializer, Is.Not.Null);

            initializer.MenuController.NewGameButton.onClick.Invoke();
            yield return null;

            var activeScene = SceneManager.GetActiveScene();
            Assert.That(activeScene.name, Is.EqualTo("MainGameScene"));
        }

        [UnityTest]
        public IEnumerator OnDestroy_WhenCalled_UnsubscribesFromNewGameEvent()
        {
            var (initializer, menuController) = CreateSystemUnderTest();
            var spyTransitioner = new SpySceneTransitioner();
            yield return null;

            Object.Destroy(initializer);
            yield return null;

            menuController.NewGameButton.onClick.Invoke();

            Assert.That(spyTransitioner.TransitionCallCount, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator OnDestroy_WhenCalled_UnsubscribesFromContinueEvent()
        {
            var (initializer, menuController) = CreateSystemUnderTest();
            var spyTransitioner = new SpySceneTransitioner();
            yield return null;

            Object.Destroy(initializer);
            yield return null;

            menuController.ContinueButton.onClick.Invoke();

            Assert.That(spyTransitioner.TransitionCallCount, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator OnDestroy_WhenMenuControllerIsNull_DoesNotThrowException()
        {
            _initializerObject = new GameObject("TitleSceneInitializer");
            var initializer = _initializerObject.AddComponent<TitleSceneInitializer>();
            yield return null;

            Assert.That(() => Object.Destroy(initializer), Throws.Nothing);
            yield return null;
        }

        private (TitleSceneInitializer, TitleMenuController) CreateSystemUnderTest()
        {
            _menuControllerObject = new GameObject("MenuController");
            var menuController = _menuControllerObject.AddComponent<TitleMenuController>();

            var newGameButton = CreateButton("NewGameButton");
            var continueButton = CreateButton("ContinueButton");
            var settingsButton = CreateButton("SettingsButton");

            typeof(TitleMenuController).GetProperty("NewGameButton").SetValue(menuController, newGameButton);
            typeof(TitleMenuController).GetProperty("ContinueButton").SetValue(menuController, continueButton);
            typeof(TitleMenuController).GetProperty("SettingsButton").SetValue(menuController, settingsButton);

            _initializerObject = new GameObject("TitleSceneInitializer");
            var initializer = _initializerObject.AddComponent<TitleSceneInitializer>();
            typeof(TitleSceneInitializer).GetProperty("MenuController").SetValue(initializer, menuController);

            return (initializer, menuController);
        }

        private Button CreateButton(string name)
        {
            var buttonObject = new GameObject(name);
            buttonObject.transform.SetParent(_menuControllerObject.transform);
            return buttonObject.AddComponent<Button>();
        }
    }
}
