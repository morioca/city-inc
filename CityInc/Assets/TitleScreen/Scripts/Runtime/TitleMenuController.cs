using System;
using UnityEngine;
using UnityEngine.UI;

namespace TitleScreen
{
    /// <summary>
    /// Controls the title menu button states and handles user interactions.
    /// </summary>
    public class TitleMenuController : MonoBehaviour
    {
        [field: SerializeField]
        public Button NewGameButton { get; private set; }

        [field: SerializeField]
        public Button ContinueButton { get; private set; }

        [field: SerializeField]
        public Button SettingsButton { get; private set; }

        /// <summary>
        /// Occurs when the new game button is selected.
        /// </summary>
        public event Action OnNewGameSelected;

        /// <summary>
        /// Occurs when the continue button is selected.
        /// </summary>
        public event Action OnContinueSelected;

        /// <summary>
        /// Occurs when the settings button is selected.
        /// </summary>
        public event Action OnSettingsSelected;

        /// <summary>
        /// Initializes the menu with the specified save data checker.
        /// </summary>
        /// <param name="saveDataChecker">The save data checker to determine continue button state.</param>
        public void Initialize(ISaveDataChecker saveDataChecker)
        {
            throw new NotImplementedException();
        }
    }
}
