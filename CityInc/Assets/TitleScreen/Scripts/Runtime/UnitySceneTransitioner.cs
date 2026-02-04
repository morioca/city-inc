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
            if (string.IsNullOrEmpty(sceneName))
            {
                throw new ArgumentException("Scene name cannot be null or empty.", nameof(sceneName));
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}
