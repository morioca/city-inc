using System;
using UnityEngine.SceneManagement;

namespace TitleScreen
{
    /// <summary>
    /// Unity's SceneManager wrapper for scene transitions.
    /// </summary>
    public class UnitySceneTransitioner : ISceneTransitioner
    {
        /// <inheritdoc/>
        public void TransitionTo(string sceneName)
        {
            if (sceneName == null)
            {
                throw new ArgumentNullException(nameof(sceneName));
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                throw new ArgumentException("Scene name cannot be empty.", nameof(sceneName));
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}
