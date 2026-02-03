namespace TitleScreen
{
    public class SpySceneTransitioner : ISceneTransitioner
    {
        public string LastTransitionedSceneName { get; private set; }
        public int TransitionCallCount { get; private set; }

        public void TransitionTo(string sceneName)
        {
            LastTransitionedSceneName = sceneName;
            TransitionCallCount++;
        }
    }
}
