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

        /// <summary>
        /// Initialize the presenter with the current game state and validator.
        /// </summary>
        /// <param name="gameState">The current game state</param>
        /// <param name="validator">The budget validator</param>
        public void Initialize(GameState gameState, BudgetValidator validator)
        {
        }

        /// <summary>
        /// Handle the confirm button click.
        /// </summary>
        public void OnConfirmButtonClicked()
        {
        }

        /// <summary>
        /// Handle the close button click.
        /// </summary>
        public void OnCloseButtonClicked()
        {
        }
    }
}
