using System.Collections;
using Domain.Models;
using Domain.Systems;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Presentation.Budget
{
    [TestFixture]
    public class BudgetAllocationPresenterTest
    {
        private GameObject _sutObject;

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_sutObject);
        }

        private GameObject CreateSystemUnderTestObject()
        {
            var gameObject = new GameObject("BudgetAllocationPresenter");
            var presenter = gameObject.AddComponent<BudgetAllocationPresenter>();

            var totalBudgetLabelObj = new GameObject("TotalBudgetLabel");
            totalBudgetLabelObj.transform.SetParent(gameObject.transform);
            var totalBudgetLabel = totalBudgetLabelObj.AddComponent<TextMeshProUGUI>();
            SetPrivateSerializedField(presenter, "<TotalBudgetLabel>k__BackingField", totalBudgetLabel);

            var remainingBudgetLabelObj = new GameObject("RemainingBudgetLabel");
            remainingBudgetLabelObj.transform.SetParent(gameObject.transform);
            var remainingBudgetLabel = remainingBudgetLabelObj.AddComponent<TextMeshProUGUI>();
            SetPrivateSerializedField(presenter, "<RemainingBudgetLabel>k__BackingField", remainingBudgetLabel);

            CreateSliderWithLabels(gameObject, presenter, "Welfare");
            CreateSliderWithLabels(gameObject, presenter, "Education");
            CreateSliderWithLabels(gameObject, presenter, "Industry");
            CreateSliderWithLabels(gameObject, presenter, "Infrastructure");
            CreateSliderWithLabels(gameObject, presenter, "Disaster");
            CreateSliderWithLabels(gameObject, presenter, "Tourism");

            var confirmButtonObj = new GameObject("ConfirmButton");
            confirmButtonObj.transform.SetParent(gameObject.transform);
            confirmButtonObj.AddComponent<Image>();
            var confirmButton = confirmButtonObj.AddComponent<Button>();
            SetPrivateSerializedField(presenter, "<ConfirmButton>k__BackingField", confirmButton);

            var closeButtonObj = new GameObject("CloseButton");
            closeButtonObj.transform.SetParent(gameObject.transform);
            closeButtonObj.AddComponent<Image>();
            var closeButton = closeButtonObj.AddComponent<Button>();
            SetPrivateSerializedField(presenter, "<CloseButton>k__BackingField", closeButton);

            return gameObject;
        }

        private void CreateSliderWithLabels(GameObject parent, BudgetAllocationPresenter presenter, string category)
        {
            var sliderObj = new GameObject(category + "Slider");
            sliderObj.transform.SetParent(parent.transform);
            var slider = sliderObj.AddComponent<Slider>();
            slider.minValue = 0;
            slider.maxValue = 100000000;
            slider.wholeNumbers = true;
            SetPrivateSerializedField(presenter, "<" + category + "Slider>k__BackingField", slider);

            var amountLabelObj = new GameObject(category + "AmountLabel");
            amountLabelObj.transform.SetParent(parent.transform);
            var amountLabel = amountLabelObj.AddComponent<TextMeshProUGUI>();
            SetPrivateSerializedField(presenter, "<" + category + "AmountLabel>k__BackingField", amountLabel);

            var percentLabelObj = new GameObject(category + "PercentLabel");
            percentLabelObj.transform.SetParent(parent.transform);
            var percentLabel = percentLabelObj.AddComponent<TextMeshProUGUI>();
            SetPrivateSerializedField(presenter, "<" + category + "PercentLabel>k__BackingField", percentLabel);
        }

        private void SetPrivateSerializedField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(obj, value);
        }

        private GameState CreateGameStateWithBudget(long budget, BudgetAllocation allocation)
        {
            return new GameState(new GameDate(1, 1), 50000, budget, 60, allocation);
        }

        [UnityTest]
        public IEnumerator Initialize_WhenCalledWithValidAllocation_ShowsZeroRemaining()
        {
            _sutObject = CreateSystemUnderTestObject();
            var sut = _sutObject.GetComponent<BudgetAllocationPresenter>();
            var allocation = new BudgetAllocation(20000000, 20000000, 20000000, 20000000, 10000000, 10000000);
            var gameState = CreateGameStateWithBudget(100000000L, allocation);
            var validator = new BudgetValidator();
            yield return null;

            sut.Initialize(gameState, validator);

            Assert.That(sut.RemainingBudgetLabel.text, Contains.Substring("0"));
        }

        [UnityTest]
        public IEnumerator Initialize_WhenCalledWithInvalidAllocation_ShowsNonZeroRemaining()
        {
            _sutObject = CreateSystemUnderTestObject();
            var sut = _sutObject.GetComponent<BudgetAllocationPresenter>();
            var allocation = new BudgetAllocation(15000000, 15000000, 15000000, 15000000, 15000000, 15000000);
            var gameState = CreateGameStateWithBudget(100000000L, allocation);
            var validator = new BudgetValidator();
            yield return null;

            sut.Initialize(gameState, validator);

            Assert.That(sut.RemainingBudgetLabel.text, Contains.Substring("10,000,000"));
        }

        [UnityTest]
        public IEnumerator OnSliderValueChanged_WhenSliderChanges_UpdatesRemainingBudget()
        {
            _sutObject = CreateSystemUnderTestObject();
            var sut = _sutObject.GetComponent<BudgetAllocationPresenter>();
            var gameState = GameState.CreateInitial();
            var validator = new BudgetValidator();
            yield return null;
            sut.Initialize(gameState, validator);

            sut.WelfareSlider.value = 30000000;
            yield return null;

            Assert.That(sut.RemainingBudgetLabel.text, Contains.Substring("13,333,330"));
        }

        [UnityTest]
        public IEnumerator OnSliderValueChanged_WhenTotalExceedsBudget_DisablesConfirmButton()
        {
            _sutObject = CreateSystemUnderTestObject();
            var sut = _sutObject.GetComponent<BudgetAllocationPresenter>();
            var allocation = new BudgetAllocation(20000000, 20000000, 20000000, 20000000, 10000000, 10000000);
            var gameState = CreateGameStateWithBudget(100000000L, allocation);
            var validator = new BudgetValidator();
            yield return null;
            sut.Initialize(gameState, validator);

            sut.WelfareSlider.value = 50000000;
            yield return null;

            Assert.That(sut.ConfirmButton.interactable, Is.False);
        }

        [UnityTest]
        public IEnumerator OnSliderValueChanged_WhenTotalMatchesBudget_EnablesConfirmButton()
        {
            _sutObject = CreateSystemUnderTestObject();
            var sut = _sutObject.GetComponent<BudgetAllocationPresenter>();
            var allocation = new BudgetAllocation(15000000, 15000000, 15000000, 15000000, 15000000, 15000000);
            var gameState = CreateGameStateWithBudget(100000000L, allocation);
            var validator = new BudgetValidator();
            yield return null;
            sut.Initialize(gameState, validator);

            sut.WelfareSlider.value = 25000000;
            yield return null;

            Assert.That(sut.ConfirmButton.interactable, Is.True);
        }

        [UnityTest]
        public IEnumerator OnSliderValueChanged_WhenMultipleSlidersAdjusted_CalculatesCorrectTotal()
        {
            _sutObject = CreateSystemUnderTestObject();
            var sut = _sutObject.GetComponent<BudgetAllocationPresenter>();
            var gameState = GameState.CreateInitial();
            var validator = new BudgetValidator();
            yield return null;
            sut.Initialize(gameState, validator);

            sut.WelfareSlider.value = 25000000;
            sut.EducationSlider.value = 30000000;
            sut.IndustrySlider.value = 15000000;
            sut.InfrastructureSlider.value = 20000000;
            sut.DisasterSlider.value = 5000000;
            sut.TourismSlider.value = 5000000;
            yield return null;

            Assert.That(sut.ConfirmButton.interactable, Is.True);
        }

        [UnityTest]
        public IEnumerator OnConfirmButtonClicked_WhenAllocationIsValid_EmitsAllocationEvent()
        {
            _sutObject = CreateSystemUnderTestObject();
            var sut = _sutObject.GetComponent<BudgetAllocationPresenter>();
            var allocation = new BudgetAllocation(20000000, 20000000, 20000000, 20000000, 10000000, 10000000);
            var gameState = CreateGameStateWithBudget(100000000L, allocation);
            var validator = new BudgetValidator();
            yield return null;
            sut.Initialize(gameState, validator);
            BudgetAllocation actual = null;
            sut.AllocationConfirmed += a => actual = a;

            sut.OnConfirmButtonClicked();

            Assert.That(actual, Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator OnConfirmButtonClicked_WhenAllocationIsValid_ClosesModal()
        {
            _sutObject = CreateSystemUnderTestObject();
            var sut = _sutObject.GetComponent<BudgetAllocationPresenter>();
            var allocation = new BudgetAllocation(20000000, 20000000, 20000000, 20000000, 10000000, 10000000);
            var gameState = CreateGameStateWithBudget(100000000L, allocation);
            var validator = new BudgetValidator();
            yield return null;
            sut.Initialize(gameState, validator);

            sut.OnConfirmButtonClicked();

            Assert.That(_sutObject.activeSelf, Is.False);
        }

        [UnityTest]
        public IEnumerator Initialize_WhenCalledMultipleTimes_DisplaysMostRecentAllocation()
        {
            _sutObject = CreateSystemUnderTestObject();
            var sut = _sutObject.GetComponent<BudgetAllocationPresenter>();
            var validator = new BudgetValidator();
            var firstAllocation = BudgetAllocation.EqualDistribution(100000000L);
            var firstGameState = CreateGameStateWithBudget(100000000L, firstAllocation);
            var secondAllocation = new BudgetAllocation(30000000, 20000000, 15000000, 10000000, 15000000, 10000000);
            var secondGameState = CreateGameStateWithBudget(100000000L, secondAllocation);
            yield return null;

            sut.Initialize(firstGameState, validator);
            sut.Initialize(secondGameState, validator);

            Assert.That(sut.WelfareSlider.value, Is.EqualTo(30000000));
        }

    }
}
