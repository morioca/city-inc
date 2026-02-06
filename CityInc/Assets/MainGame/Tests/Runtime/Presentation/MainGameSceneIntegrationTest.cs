using System.Collections;
using NUnit.Framework;
using TMPro;
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
    }
}
