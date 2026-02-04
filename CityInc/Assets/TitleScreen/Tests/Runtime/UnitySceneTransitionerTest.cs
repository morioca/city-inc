using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace TitleScreen
{
    [TestFixture]
    public class UnitySceneTransitionerTest
    {
        [UnityTest]
        public IEnumerator TransitionTo_WhenCalledWithSceneName_LoadsScene()
        {
            var sut = new UnitySceneTransitioner();
            var testSceneName = "MainGameScene";

            sut.TransitionTo(testSceneName);
            yield return null;

            var activeScene = SceneManager.GetActiveScene();
            Assert.That(activeScene.name, Is.EqualTo(testSceneName));
        }

        [Test]
        public void TransitionTo_WhenCalledWithNullSceneName_ThrowsArgumentException()
        {
            var sut = new UnitySceneTransitioner();

            Assert.That(() => sut.TransitionTo(null), Throws.ArgumentException);
        }

        [Test]
        public void TransitionTo_WhenCalledWithEmptySceneName_ThrowsArgumentException()
        {
            var sut = new UnitySceneTransitioner();

            Assert.That(() => sut.TransitionTo(""), Throws.ArgumentException);
        }
    }
}
