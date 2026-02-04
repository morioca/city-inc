using NUnit.Framework;

namespace TitleScreen
{
    [TestFixture]
    public class TitleSceneTransitionTest
    {
        // MGS-001: When TransitionToMainGame is called, transitions to main game scene
        [Test]
        public void TransitionToMainGame_WhenCalled_TransitionsToMainGameScene()
        {
            var spySceneTransitioner = new SpySceneTransitioner();
            var sut = new TitleSceneTransition(spySceneTransitioner);

            sut.TransitionToMainGame();

            Assert.That(spySceneTransitioner.LastTransitionedSceneName,
                Is.EqualTo(TitleSceneTransition.MainGameSceneName));
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
