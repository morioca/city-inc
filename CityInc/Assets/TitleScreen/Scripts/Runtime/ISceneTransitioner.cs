namespace TitleScreen
{
    /// <summary>
    /// Interface for scene transition operations.
    /// </summary>
    public interface ISceneTransitioner
    {
        /// <summary>
        /// Transitions to the specified scene.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        void TransitionTo(string sceneName);
    }
}
