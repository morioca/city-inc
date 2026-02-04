using UnityEngine;

namespace TitleScreen
{
    /// <summary>
    /// Initializes the title scene and wires up components.
    /// </summary>
    public class TitleSceneInitializer : MonoBehaviour
    {
        [field: SerializeField]
        public TitleMenuController MenuController { get; private set; }

        private TitleSceneTransition _sceneTransition;

        private void Awake()
        {
            if (MenuController == null)
            {
                return;
            }

            var saveDataChecker = new SaveDataChecker();
            var sceneTransitioner = new UnitySceneTransitioner();
            _sceneTransition = new TitleSceneTransition(sceneTransitioner);

            MenuController.Initialize(saveDataChecker);

            MenuController.OnNewGameSelected += _sceneTransition.TransitionToMainGame;
            MenuController.OnContinueSelected += _sceneTransition.TransitionToGameWithLatestSave;
        }

        private void OnDestroy()
        {
            if (MenuController != null && _sceneTransition != null)
            {
                MenuController.OnNewGameSelected -= _sceneTransition.TransitionToMainGame;
                MenuController.OnContinueSelected -= _sceneTransition.TransitionToGameWithLatestSave;
            }
        }
    }
}
