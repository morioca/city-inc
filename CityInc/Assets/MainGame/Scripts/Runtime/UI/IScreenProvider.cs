using UnityEngine;

namespace UI
{
    /// <summary>
    /// Provides screen information for safe area layout calculations.
    /// This interface allows for testability by abstracting Unity's Screen class.
    /// </summary>
    public interface IScreenProvider
    {
        /// <summary>
        /// Gets the safe area of the screen in pixels.
        /// </summary>
        Rect SafeArea { get; }

        /// <summary>
        /// Gets the current width of the screen in pixels.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the current height of the screen in pixels.
        /// </summary>
        int Height { get; }
    }
}
