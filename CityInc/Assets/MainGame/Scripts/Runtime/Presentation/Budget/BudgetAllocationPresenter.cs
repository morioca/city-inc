using System;
using Domain.Models;
using Domain.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Budget
{
    /// <summary>
    /// Manages the budget allocation UI, allowing the player to view and modify budget allocations.
    /// </summary>
    public class BudgetAllocationPresenter : MonoBehaviour
    {
        [field: SerializeField]
        public TMP_Text TotalBudgetLabel { get; set; }

        [field: SerializeField]
        public TMP_Text RemainingBudgetLabel { get; set; }

        [field: SerializeField]
        public Slider WelfareSlider { get; set; }

        [field: SerializeField]
        public TMP_Text WelfareAmountLabel { get; set; }

        [field: SerializeField]
        public TMP_Text WelfarePercentLabel { get; set; }

        [field: SerializeField]
        public Slider EducationSlider { get; set; }

        [field: SerializeField]
        public TMP_Text EducationAmountLabel { get; set; }

        [field: SerializeField]
        public TMP_Text EducationPercentLabel { get; set; }

        [field: SerializeField]
        public Slider IndustrySlider { get; set; }

        [field: SerializeField]
        public TMP_Text IndustryAmountLabel { get; set; }

        [field: SerializeField]
        public TMP_Text IndustryPercentLabel { get; set; }

        [field: SerializeField]
        public Slider InfrastructureSlider { get; set; }

        [field: SerializeField]
        public TMP_Text InfrastructureAmountLabel { get; set; }

        [field: SerializeField]
        public TMP_Text InfrastructurePercentLabel { get; set; }

        [field: SerializeField]
        public Slider DisasterSlider { get; set; }

        [field: SerializeField]
        public TMP_Text DisasterAmountLabel { get; set; }

        [field: SerializeField]
        public TMP_Text DisasterPercentLabel { get; set; }

        [field: SerializeField]
        public Slider TourismSlider { get; set; }

        [field: SerializeField]
        public TMP_Text TourismAmountLabel { get; set; }

        [field: SerializeField]
        public TMP_Text TourismPercentLabel { get; set; }

        [field: SerializeField]
        public Button ConfirmButton { get; set; }

        [field: SerializeField]
        public Button CloseButton { get; set; }

        /// <summary>
        /// Event raised when the player confirms the budget allocation.
        /// </summary>
        public event Action<BudgetAllocation> AllocationConfirmed;

        /// <summary>
        /// Event raised when the player closes the modal without confirming.
        /// </summary>
        public event Action ModalClosed;

        private long _totalBudget;
        private BudgetValidator _validator;
        private Color _normalColor;

        /// <summary>
        /// Initialize the presenter with the current game state and validator.
        /// </summary>
        /// <param name="gameState">The current game state</param>
        /// <param name="validator">The budget validator</param>
        public void Initialize(GameState gameState, BudgetValidator validator)
        {
            _totalBudget = gameState.Budget;
            _validator = validator;
            _normalColor = RemainingBudgetLabel.color;

            RemoveSliderListeners();

            TotalBudgetLabel.text = string.Format("Total Budget: \u00a5{0:N0}", _totalBudget);

            var allocation = gameState.CurrentAllocation;
            SetSliderRange(WelfareSlider);
            SetSliderRange(EducationSlider);
            SetSliderRange(IndustrySlider);
            SetSliderRange(InfrastructureSlider);
            SetSliderRange(DisasterSlider);
            SetSliderRange(TourismSlider);

            WelfareSlider.value = allocation.WelfareHealthcare;
            EducationSlider.value = allocation.EducationChildcare;
            IndustrySlider.value = allocation.IndustryDevelopment;
            InfrastructureSlider.value = allocation.Infrastructure;
            DisasterSlider.value = allocation.DisasterPrevention;
            TourismSlider.value = allocation.TourismCulture;

            UpdateCategoryLabels();
            UpdateRemainingAndConfirmButton();
            SetupSliderListeners();
        }

        /// <summary>
        /// Handle the confirm button click.
        /// </summary>
        public void OnConfirmButtonClicked()
        {
            var allocation = BuildAllocationFromSliders();
            AllocationConfirmed?.Invoke(allocation);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Handle the close button click.
        /// </summary>
        public void OnCloseButtonClicked()
        {
            ModalClosed?.Invoke();
            gameObject.SetActive(false);
        }

        private void SetSliderRange(Slider slider)
        {
            slider.minValue = 0;
            slider.maxValue = _totalBudget;
            slider.wholeNumbers = true;
        }

        private void SetupSliderListeners()
        {
            WelfareSlider.onValueChanged.AddListener(_ => OnSliderValueChanged());
            EducationSlider.onValueChanged.AddListener(_ => OnSliderValueChanged());
            IndustrySlider.onValueChanged.AddListener(_ => OnSliderValueChanged());
            InfrastructureSlider.onValueChanged.AddListener(_ => OnSliderValueChanged());
            DisasterSlider.onValueChanged.AddListener(_ => OnSliderValueChanged());
            TourismSlider.onValueChanged.AddListener(_ => OnSliderValueChanged());
        }

        private void RemoveSliderListeners()
        {
            WelfareSlider.onValueChanged.RemoveAllListeners();
            EducationSlider.onValueChanged.RemoveAllListeners();
            IndustrySlider.onValueChanged.RemoveAllListeners();
            InfrastructureSlider.onValueChanged.RemoveAllListeners();
            DisasterSlider.onValueChanged.RemoveAllListeners();
            TourismSlider.onValueChanged.RemoveAllListeners();
        }

        private void OnSliderValueChanged()
        {
            UpdateCategoryLabels();
            UpdateRemainingAndConfirmButton();
        }

        private void UpdateCategoryLabels()
        {
            UpdateCategoryLabel(WelfareSlider, WelfareAmountLabel, WelfarePercentLabel);
            UpdateCategoryLabel(EducationSlider, EducationAmountLabel, EducationPercentLabel);
            UpdateCategoryLabel(IndustrySlider, IndustryAmountLabel, IndustryPercentLabel);
            UpdateCategoryLabel(InfrastructureSlider, InfrastructureAmountLabel, InfrastructurePercentLabel);
            UpdateCategoryLabel(DisasterSlider, DisasterAmountLabel, DisasterPercentLabel);
            UpdateCategoryLabel(TourismSlider, TourismAmountLabel, TourismPercentLabel);
        }

        private void UpdateCategoryLabel(Slider slider, TMP_Text amountLabel, TMP_Text percentLabel)
        {
            long amount = (long)slider.value;
            amountLabel.text = string.Format("\u00a5{0:N0}", amount);

            if (_totalBudget == 0)
            {
                percentLabel.text = "0%";
                return;
            }

            double percent = (double)amount / _totalBudget * 100.0;
            percentLabel.text = string.Format("{0:0.#}%", percent);
        }

        private void UpdateRemainingAndConfirmButton()
        {
            var allocation = BuildAllocationFromSliders();
            long remaining = _validator.GetRemaining(allocation, _totalBudget);
            bool isValid = _validator.IsValid(allocation, _totalBudget);

            RemainingBudgetLabel.text = string.Format("Remaining: \u00a5{0:N0}", remaining);
            ConfirmButton.interactable = isValid;

            RemainingBudgetLabel.color = remaining < 0 ? Color.red : _normalColor;
        }

        private BudgetAllocation BuildAllocationFromSliders()
        {
            return new BudgetAllocation(
                (long)WelfareSlider.value,
                (long)EducationSlider.value,
                (long)IndustrySlider.value,
                (long)InfrastructureSlider.value,
                (long)DisasterSlider.value,
                (long)TourismSlider.value
            );
        }
    }
}
