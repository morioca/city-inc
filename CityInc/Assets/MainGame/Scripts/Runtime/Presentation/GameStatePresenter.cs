using Domain.Models;
using Domain.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation
{
    /// <summary>
    /// Reflects the game state onto UI elements.
    /// </summary>
    public class GameStatePresenter : MonoBehaviour
    {
        [field: SerializeField]
        public TMP_Text DateLabel { get; set; }

        [field: SerializeField]
        public TMP_Text PopulationLabel { get; set; }

        [field: SerializeField]
        public TMP_Text BudgetLabel { get; set; }

        [field: SerializeField]
        public TMP_Text ApprovalRatingLabel { get; set; }

        [field: SerializeField]
        public Button NextMonthButton { get; set; }

        private readonly TurnManager _turnManager = new TurnManager();
        private GameState _currentState;

        /// <summary>
        /// Set the initial game state and update the UI.
        /// </summary>
        /// <param name="initialState">The initial game state</param>
        public void Initialize(GameState initialState)
        {
            _currentState = initialState;
            UpdateDateLabel();
            UpdateMetricLabels();
        }

        /// <summary>
        /// Handle the next month button click.
        /// </summary>
        public void OnNextMonthButtonClicked()
        {
            _currentState = _turnManager.ProgressToNextMonth(_currentState);
            UpdateDateLabel();
        }

        private void UpdateDateLabel()
        {
            DateLabel.text = _currentState.CurrentDate.ToDisplayString();
        }

        private void UpdateMetricLabels()
        {
            PopulationLabel.text = string.Format("人口 {0:N0}人", _currentState.Population);
            BudgetLabel.text = string.Format("財政 {0:N0}円", _currentState.Budget);
            ApprovalRatingLabel.text = string.Format("支持率 {0}%", _currentState.ApprovalRating);
        }
    }
}
