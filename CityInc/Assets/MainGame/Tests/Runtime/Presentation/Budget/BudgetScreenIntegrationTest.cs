using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Presentation.Budget
{
    [TestFixture]
    public class BudgetScreenIntegrationTest
    {
        [UnityTest]
        public IEnumerator BudgetButton_WhenClicked_OpensBudgetModal()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var budgetButton = GameObject.Find("BudgetButton")?.GetComponent<Button>();
            budgetButton.onClick.Invoke();
            yield return null;

            var budgetScreen = GameObject.Find("BudgetScreen");
            Assert.That(budgetScreen.activeSelf, Is.True);
        }

        [UnityTest]
        public IEnumerator CloseButton_WhenClicked_ClosesModalAndReturnsToMainScreen()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var budgetButton = GameObject.Find("BudgetButton")?.GetComponent<Button>();
            budgetButton.onClick.Invoke();
            yield return null;

            var presenter = Object.FindFirstObjectByType<BudgetAllocationPresenter>();
            presenter.OnCloseButtonClicked();
            yield return null;

            var budgetScreen = GameObject.Find("BudgetScreen");
            Assert.That(budgetScreen == null || !budgetScreen.activeSelf, Is.True);
        }

        [UnityTest]
        public IEnumerator ConfirmButton_WhenClicked_SavesAllocationAndClosesModal()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var budgetButton = GameObject.Find("BudgetButton")?.GetComponent<Button>();
            budgetButton.onClick.Invoke();
            yield return null;

            var presenter = Object.FindFirstObjectByType<BudgetAllocationPresenter>();
            presenter.OnConfirmButtonClicked();
            yield return null;

            var budgetScreen = GameObject.Find("BudgetScreen");
            Assert.That(budgetScreen == null || !budgetScreen.activeSelf, Is.True);
        }

        [UnityTest]
        public IEnumerator ReopenAfterConfirm_WhenOpened_DisplaysPreviousAllocation()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var budgetButton = GameObject.Find("BudgetButton")?.GetComponent<Button>();
            budgetButton.onClick.Invoke();
            yield return null;

            var presenter = Object.FindFirstObjectByType<BudgetAllocationPresenter>();
            presenter.WelfareSlider.value = 40000000;
            presenter.EducationSlider.value = 60000000;
            presenter.IndustrySlider.value = 0;
            presenter.InfrastructureSlider.value = 0;
            presenter.DisasterSlider.value = 0;
            presenter.TourismSlider.value = 0;
            yield return null;

            presenter.OnConfirmButtonClicked();
            yield return null;

            budgetButton.onClick.Invoke();
            yield return null;

            var reopenedPresenter = Object.FindFirstObjectByType<BudgetAllocationPresenter>();
            Assert.That(reopenedPresenter.WelfareSlider.value, Is.EqualTo(40000000));
        }

        [UnityTest]
        public IEnumerator ReopenAfterClose_WhenOpenedWithoutConfirm_DisplaysUnchangedAllocation()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var budgetButton = GameObject.Find("BudgetButton")?.GetComponent<Button>();
            budgetButton.onClick.Invoke();
            yield return null;

            var presenter = Object.FindFirstObjectByType<BudgetAllocationPresenter>();
            var originalValue = presenter.WelfareSlider.value;
            presenter.WelfareSlider.value = 50000000;
            yield return null;

            presenter.OnCloseButtonClicked();
            yield return null;

            budgetButton.onClick.Invoke();
            yield return null;

            var reopenedPresenter = Object.FindFirstObjectByType<BudgetAllocationPresenter>();
            Assert.That(reopenedPresenter.WelfareSlider.value, Is.EqualTo(originalValue));
        }

        [UnityTest]
        public IEnumerator MainGameScene_BudgetButton_WhenClicked_MaintainsGameState()
        {
            yield return SceneManager.LoadSceneAsync("MainGameScene");
            yield return null;

            var populationLabel = GameObject.Find("PopulationLabel")?.GetComponent<TMP_Text>();
            var budgetLabel = GameObject.Find("BudgetLabel")?.GetComponent<TMP_Text>();
            var approvalRatingLabel = GameObject.Find("ApprovalRatingLabel")?.GetComponent<TMP_Text>();
            var budgetButton = GameObject.Find("BudgetButton")?.GetComponent<Button>();

            budgetButton.onClick.Invoke();
            yield return null;

            var presenter = Object.FindFirstObjectByType<BudgetAllocationPresenter>();
            presenter.OnConfirmButtonClicked();
            yield return null;

            var actual = new[] { populationLabel.text, budgetLabel.text, approvalRatingLabel.text };
            var expected = new[] { "人口 50,000人", "財政 100,000,000円", "支持率 60%" };
            Assert.That(actual, Is.EqualTo(expected));
        }

    }
}
