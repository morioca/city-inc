using System.Collections;
using NUnit.Framework;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Presentation
{
    [TestFixture]
    public class MainGameSceneIntegrationTest
    {
        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_CreatesUIElements()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var canvas = Object.FindFirstObjectByType<Canvas>();
            Assert.That(canvas, Is.Not.Null);

            var dateLabel = GameObject.Find("DateLabel")?.GetComponent<TMP_Text>();
            Assert.That(dateLabel, Is.Not.Null);
            Assert.That(dateLabel.text, Is.EqualTo("2024年04月"));

            var nextMonthButton = GameObject.Find("NextMonthButton")?.GetComponent<Button>();
            Assert.That(nextMonthButton, Is.Not.Null);

            var presenter = Object.FindFirstObjectByType<GameStatePresenter>();
            Assert.That(presenter, Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenButtonClicked_UpdatesDateDisplay()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var dateLabel = GameObject.Find("DateLabel")?.GetComponent<TMP_Text>();
            var nextMonthButton = GameObject.Find("NextMonthButton")?.GetComponent<Button>();

            Assert.That(dateLabel.text, Is.EqualTo("2024年04月"));

            nextMonthButton.onClick.Invoke();
            yield return null;

            Assert.That(dateLabel.text, Is.EqualTo("2024年05月"));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenButtonClickedInDecember_ShowsNextYearJanuary()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var dateLabel = GameObject.Find("DateLabel")?.GetComponent<TMP_Text>();
            var nextMonthButton = GameObject.Find("NextMonthButton")?.GetComponent<Button>();

            for (var i = 0; i < 8; i++)
            {
                nextMonthButton.onClick.Invoke();
                yield return null;
            }

            Assert.That(dateLabel.text, Is.EqualTo("2024年12月"));

            nextMonthButton.onClick.Invoke();
            yield return null;

            Assert.That(dateLabel.text, Is.EqualTo("2025年01月"));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_CreatesSafeAreaPanelUnderCanvas()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var safeAreaPanel = GameObject.Find("SafeAreaPanel");

            Assert.That(safeAreaPanel.transform.parent.name, Is.EqualTo("Canvas"));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_SafeAreaPanelHasSafeAreaLayoutComponent()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var safeAreaPanel = GameObject.Find("SafeAreaPanel");

            Assert.That(safeAreaPanel.GetComponent<SafeAreaLayout>(), Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_SafeAreaPanelHasAnchorMinZero()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var safeAreaPanel = GameObject.Find("SafeAreaPanel");
            var rectTransform = safeAreaPanel.GetComponent<RectTransform>();

            Assert.That(rectTransform.anchorMin, Is.EqualTo(Vector2.zero));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_SafeAreaPanelHasAnchorMaxOne()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var safeAreaPanel = GameObject.Find("SafeAreaPanel");
            var rectTransform = safeAreaPanel.GetComponent<RectTransform>();

            Assert.That(rectTransform.anchorMax, Is.EqualTo(Vector2.one));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_SafeAreaPanelHasOffsetMinZero()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var safeAreaPanel = GameObject.Find("SafeAreaPanel");
            var rectTransform = safeAreaPanel.GetComponent<RectTransform>();

            Assert.That(rectTransform.offsetMin, Is.EqualTo(Vector2.zero));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_SafeAreaPanelHasOffsetMaxZero()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var safeAreaPanel = GameObject.Find("SafeAreaPanel");
            var rectTransform = safeAreaPanel.GetComponent<RectTransform>();

            Assert.That(rectTransform.offsetMax, Is.EqualTo(Vector2.zero));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_DateLabelIsUnderSafeAreaPanel()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var dateLabel = GameObject.Find("DateLabel");

            Assert.That(dateLabel.transform.parent.name, Is.EqualTo("SafeAreaPanel"));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_NextMonthButtonIsUnderSafeAreaPanel()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var nextMonthButton = GameObject.Find("NextMonthButton");

            Assert.That(nextMonthButton.transform.parent.name, Is.EqualTo("SafeAreaPanel"));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_GameStatePresenterIsUnderSafeAreaPanel()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var presenter = Object.FindFirstObjectByType<GameStatePresenter>();

            Assert.That(presenter.transform.parent.name, Is.EqualTo("SafeAreaPanel"));
        }
    }
}
