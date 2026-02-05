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
        public Button NextMonthButton { get; set; }

        private TurnManager _turnManager;
        private GameState _currentState;

        /// <summary>
        /// Set the initial game state and update the UI.
        /// </summary>
        /// <param name="initialState">The initial game state</param>
        public void Initialize(GameState initialState)
        {
        }

        /// <summary>
        /// Handle the next month button click.
        /// </summary>
        public void OnNextMonthButtonClicked()
        {
        }
    }
}
