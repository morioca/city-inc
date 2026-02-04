using NUnit.Framework;

namespace TitleScreen
{
    [TestFixture]
    public class TitleSceneTransitionTest
    {
        // MGS-001: When TransitionToScenarioSelect is called, transitions to main game scene
        [Test]
        public void TransitionToScenarioSelect_WhenCalled_TransitionsToMainGameScene()
        {
            var spySceneTransitioner = new SpySceneTransitioner();
            var sut = new TitleSceneTransition(spySceneTransitioner);

            sut.TransitionToScenarioSelect();

            Assert.That(spySceneTransitioner.LastTransitionedSceneName, Is.EqualTo("MainGameScene"));
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
