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
            Assert.That(dateLabel.text, Is.EqualTo("1年01月"));

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

            Assert.That(dateLabel.text, Is.EqualTo("1年01月"));

            nextMonthButton.onClick.Invoke();
            yield return null;

            Assert.That(dateLabel.text, Is.EqualTo("1年02月"));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenButtonClickedInDecember_ShowsNextYearJanuary()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var dateLabel = GameObject.Find("DateLabel")?.GetComponent<TMP_Text>();
            var nextMonthButton = GameObject.Find("NextMonthButton")?.GetComponent<Button>();

            for (var i = 0; i < 11; i++)
            {
                nextMonthButton.onClick.Invoke();
                yield return null;
            }

            Assert.That(dateLabel.text, Is.EqualTo("1年12月"));

            nextMonthButton.onClick.Invoke();
            yield return null;

            Assert.That(dateLabel.text, Is.EqualTo("2年01月"));
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

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_CreatesStatusBarPanel()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var statusBarPanel = GameObject.Find("StatusBarPanel");

            Assert.That(statusBarPanel, Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_StatusBarPanelIsUnderSafeAreaPanel()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var statusBarPanel = GameObject.Find("StatusBarPanel");

            Assert.That(statusBarPanel.transform.parent.name, Is.EqualTo("SafeAreaPanel"));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_PopulationLabelExists()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var populationLabel = GameObject.Find("PopulationLabel")?.GetComponent<TMP_Text>();

            Assert.That(populationLabel, Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_BudgetLabelExists()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var budgetLabel = GameObject.Find("BudgetLabel")?.GetComponent<TMP_Text>();

            Assert.That(budgetLabel, Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_ApprovalRatingLabelExists()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var approvalRatingLabel = GameObject.Find("ApprovalRatingLabel")?.GetComponent<TMP_Text>();

            Assert.That(approvalRatingLabel, Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_PopulationLabelShowsInitialValue()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var populationLabel = GameObject.Find("PopulationLabel")?.GetComponent<TMP_Text>();

            Assert.That(populationLabel.text, Is.EqualTo("人口 50,000人"));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_BudgetLabelShowsInitialValue()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var budgetLabel = GameObject.Find("BudgetLabel")?.GetComponent<TMP_Text>();

            Assert.That(budgetLabel.text, Is.EqualTo("財政 100,000,000円"));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_ApprovalRatingLabelShowsInitialValue()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var approvalRatingLabel = GameObject.Find("ApprovalRatingLabel")?.GetComponent<TMP_Text>();

            Assert.That(approvalRatingLabel.text, Is.EqualTo("支持率 60%"));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_PopulationLabelHasLayoutElement()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var populationLabel = GameObject.Find("PopulationLabel");
            var layoutElement = populationLabel?.GetComponent<LayoutElement>();

            Assert.That(layoutElement, Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_PopulationLabelHasCorrectPreferredWidth()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var populationLabel = GameObject.Find("PopulationLabel");
            var layoutElement = populationLabel?.GetComponent<LayoutElement>();

            Assert.That(layoutElement.preferredWidth, Is.EqualTo(200));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_BudgetLabelHasLayoutElement()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var budgetLabel = GameObject.Find("BudgetLabel");
            var layoutElement = budgetLabel?.GetComponent<LayoutElement>();

            Assert.That(layoutElement, Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_BudgetLabelHasCorrectPreferredWidth()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var budgetLabel = GameObject.Find("BudgetLabel");
            var layoutElement = budgetLabel?.GetComponent<LayoutElement>();

            Assert.That(layoutElement.preferredWidth, Is.EqualTo(250));
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_ApprovalRatingLabelHasLayoutElement()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var approvalRatingLabel = GameObject.Find("ApprovalRatingLabel");
            var layoutElement = approvalRatingLabel?.GetComponent<LayoutElement>();

            Assert.That(layoutElement, Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator MainGameScene_WhenLoaded_ApprovalRatingLabelHasCorrectPreferredWidth()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var approvalRatingLabel = GameObject.Find("ApprovalRatingLabel");
            var layoutElement = approvalRatingLabel?.GetComponent<LayoutElement>();

            Assert.That(layoutElement.preferredWidth, Is.EqualTo(150));
        }
    }
}
