using UnityEngine;

namespace UI
{
    /// <summary>
    /// Default implementation of IScreenProvider that wraps Unity's Screen class.
    /// </summary>
    public class UnityScreenProvider : IScreenProvider
    {
        /// <inheritdoc/>
        public Rect SafeArea => Screen.safeArea;

        /// <inheritdoc/>
        public int Width => Screen.width;

        /// <inheritdoc/>
        public int Height => Screen.height;
    }
}
