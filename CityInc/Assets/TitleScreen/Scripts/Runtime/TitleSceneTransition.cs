using System;

namespace TitleScreen
{
    /// <summary>
    /// Handles scene transitions from the title screen.
    /// </summary>
    public class TitleSceneTransition
    {
        private readonly ISceneTransitioner _sceneTransitioner;

        /// <summary>
        /// The name of the scenario select scene.
        /// </summary>
        public const string ScenarioSelectSceneName = "ScenarioSelectScene";

        /// <summary>
        /// The name of the game scene.
        /// </summary>
        public const string GameSceneName = "GameScene";

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleSceneTransition"/> class.
        /// </summary>
        /// <param name="sceneTransitioner">The scene transitioner to use.</param>
        public TitleSceneTransition(ISceneTransitioner sceneTransitioner)
        {
            _sceneTransitioner = sceneTransitioner ?? throw new ArgumentNullException(nameof(sceneTransitioner));
        }

        /// <summary>
        /// Transitions to the scenario select screen.
        /// </summary>
        public void TransitionToScenarioSelect()
        {
            _sceneTransitioner.TransitionTo(ScenarioSelectSceneName);
        }

        /// <summary>
        /// Transitions to the game with the latest save data.
        /// </summary>
        public void TransitionToGameWithLatestSave()
        {
            _sceneTransitioner.TransitionTo(GameSceneName);
        }
    }
}
