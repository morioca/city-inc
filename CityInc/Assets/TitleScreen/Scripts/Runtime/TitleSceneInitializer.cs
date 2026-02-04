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
        }

        private void OnDestroy()
        {
        }
    }
}
