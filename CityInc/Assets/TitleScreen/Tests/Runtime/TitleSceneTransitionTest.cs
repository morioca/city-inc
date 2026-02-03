using NUnit.Framework;

namespace TitleScreen
{
    [TestFixture]
    public class TitleSceneTransitionTest
    {
        // TST-001: When TransitionToScenarioSelect is called, transitions to scenario select scene
        [Test]
        public void TransitionToScenarioSelect_WhenCalled_TransitionsToScenarioSelectScene()
        {
            var spySceneTransitioner = new SpySceneTransitioner();
            var sut = new TitleSceneTransition(spySceneTransitioner);

            sut.TransitionToScenarioSelect();

            Assert.That(spySceneTransitioner.LastTransitionedSceneName,
                Is.EqualTo(TitleSceneTransition.ScenarioSelectSceneName));
        }

        // TST-002: When TransitionToGameWithLatestSave is called, transitions to game scene with latest save
        [Test]
        public void TransitionToGameWithLatestSave_WhenCalled_TransitionsToGameScene()
        {
            var spySceneTransitioner = new SpySceneTransitioner();
            var sut = new TitleSceneTransition(spySceneTransitioner);

            sut.TransitionToGameWithLatestSave();

            Assert.That(spySceneTransitioner.LastTransitionedSceneName,
                Is.EqualTo(TitleSceneTransition.GameSceneName));
        }
    }
}
