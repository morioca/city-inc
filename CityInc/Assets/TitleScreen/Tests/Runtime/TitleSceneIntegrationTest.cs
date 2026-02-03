using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace TitleScreen
{
    [TestFixture]
    public class TitleSceneIntegrationTest
    {
        private const string TitleScenePath = "Assets/Scenes/TitleScene.unity";

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync(TitleScenePath);
        }

        // TSI-010: TitleScene をロードすると、TitleMenuController コンポーネントが存在する
        [UnityTest]
        public IEnumerator LoadScene_WhenLoaded_TitleMenuControllerExists()
        {
            yield return null;

            var sut = Object.FindFirstObjectByType<TitleMenuController>();

            Assert.That(sut, Is.Not.Null);
        }

        // TSI-020: TitleScene をロードすると、NewGameButton が TitleMenuController に設定されている
        [UnityTest]
        public IEnumerator LoadScene_WhenLoaded_NewGameButtonIsAssigned()
        {
            yield return null;

            var sut = Object.FindFirstObjectByType<TitleMenuController>();

            Assert.That(sut.NewGameButton, Is.Not.Null);
        }

        // TSI-030: TitleScene をロードすると、ContinueButton が TitleMenuController に設定されている
        [UnityTest]
        public IEnumerator LoadScene_WhenLoaded_ContinueButtonIsAssigned()
        {
            yield return null;

            var sut = Object.FindFirstObjectByType<TitleMenuController>();

            Assert.That(sut.ContinueButton, Is.Not.Null);
        }

        // TSI-040: TitleScene をロードすると、SettingsButton が TitleMenuController に設定されている
        [UnityTest]
        public IEnumerator LoadScene_WhenLoaded_SettingsButtonIsAssigned()
        {
            yield return null;

            var sut = Object.FindFirstObjectByType<TitleMenuController>();

            Assert.That(sut.SettingsButton, Is.Not.Null);
        }
    }
}
