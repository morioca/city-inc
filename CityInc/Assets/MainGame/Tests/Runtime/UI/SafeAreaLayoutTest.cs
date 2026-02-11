using System.Collections;
using NUnit.Framework;
using TestDoubles;
using UI;
using UnityEngine;
using UnityEngine.TestTools;

namespace UI
{
    [TestFixture]
    public class SafeAreaLayoutTest
    {
        private GameObject _testObject;

        [TearDown]
        public void TearDown()
        {
            if (_testObject != null)
            {
                Object.DestroyImmediate(_testObject);
            }
        }

        private (SafeAreaLayout sut, RectTransform rectTransform) CreateSystemUnderTest(
            StubScreenProvider stubScreenProvider)
        {
            _testObject = new GameObject("SafeAreaPanel");
            var rectTransform = _testObject.AddComponent<RectTransform>();
            var sut = _testObject.AddComponent<SafeAreaLayout>();
            sut.SetScreenProvider(stubScreenProvider);
            return (sut, rectTransform);
        }

        // TC-01: Safe Areaが画面全体の場合にanchorが正しく設定される
        [UnityTest]
        public IEnumerator Awake_WhenSafeAreaIsFullScreen_SetsAnchorsCorrectly()
        {
            var stubScreenProvider = new StubScreenProvider(
                new Rect(0, 0, 1920, 1080), 1920, 1080);

            var (sut, rectTransform) = CreateSystemUnderTest(stubScreenProvider);
            yield return null;

            Assert.That(rectTransform.anchorMin, Is.EqualTo(new Vector2(0, 0)));
            Assert.That(rectTransform.anchorMax, Is.EqualTo(new Vector2(1, 1)));
        }

        // TC-03: Safe Areaが画面上部にオフセットがある場合（ノッチ対応）
        [UnityTest]
        public IEnumerator Awake_WhenSafeAreaHasTopOffset_SetsAnchorsCorrectly()
        {
            var stubScreenProvider = new StubScreenProvider(
                new Rect(0, 100, 1920, 980), 1920, 1080);

            var (sut, rectTransform) = CreateSystemUnderTest(stubScreenProvider);
            yield return null;

            var expectedMinY = 100f / 1080f;
            Assert.That(rectTransform.anchorMin, Is.EqualTo(new Vector2(0, expectedMinY)).Within(0.001f));
            Assert.That(rectTransform.anchorMax, Is.EqualTo(new Vector2(1, 1)));
        }

        // TC-05: Safe Areaが左右にオフセットがある場合
        [UnityTest]
        public IEnumerator Awake_WhenSafeAreaHasHorizontalOffset_SetsAnchorsCorrectly()
        {
            var stubScreenProvider = new StubScreenProvider(
                new Rect(50, 0, 1820, 1080), 1920, 1080);

            var (sut, rectTransform) = CreateSystemUnderTest(stubScreenProvider);
            yield return null;

            var expectedMin = new Vector2(50f / 1920f, 0);
            var expectedMax = new Vector2((50f + 1820f) / 1920f, 1);
            Assert.That(rectTransform.anchorMin, Is.EqualTo(expectedMin).Within(0.001f));
            Assert.That(rectTransform.anchorMax, Is.EqualTo(expectedMax).Within(0.001f));
        }

        // TC-07: Safe Areaが四辺すべてにオフセットがある場合
        [UnityTest]
        public IEnumerator Awake_WhenSafeAreaHasAllSidesOffset_SetsAnchorsCorrectly()
        {
            var stubScreenProvider = new StubScreenProvider(
                new Rect(50, 100, 1820, 880), 1920, 1080);

            var (sut, rectTransform) = CreateSystemUnderTest(stubScreenProvider);
            yield return null;

            var expectedMin = new Vector2(50f / 1920f, 100f / 1080f);
            var expectedMax = new Vector2((50f + 1820f) / 1920f, (100f + 880f) / 1080f);
            Assert.That(rectTransform.anchorMin, Is.EqualTo(expectedMin).Within(0.001f));
            Assert.That(rectTransform.anchorMax, Is.EqualTo(expectedMax).Within(0.001f));
        }

        // TC-10: 画面サイズが変更されたときにanchorが再計算される
        [UnityTest]
        public IEnumerator Update_WhenScreenSizeChanges_RecalculatesAnchors()
        {
            var stubScreenProvider = new StubScreenProvider(
                new Rect(0, 100, 1920, 980), 1920, 1080);

            var (sut, rectTransform) = CreateSystemUnderTest(stubScreenProvider);
            yield return null;

            stubScreenProvider.SafeArea = new Rect(0, 59, 2436, 1066);
            stubScreenProvider.Width = 2436;
            stubScreenProvider.Height = 1125;
            yield return null;

            var expectedMinY = 59f / 1125f;
            Assert.That(rectTransform.anchorMin.y, Is.EqualTo(expectedMinY).Within(0.001f));
        }

        // TC-12: Safe Areaのみが変更されたときにanchorが再計算される
        [UnityTest]
        public IEnumerator Update_WhenOnlySafeAreaChanges_RecalculatesAnchors()
        {
            var stubScreenProvider = new StubScreenProvider(
                new Rect(0, 100, 1920, 980), 1920, 1080);

            var (sut, rectTransform) = CreateSystemUnderTest(stubScreenProvider);
            yield return null;

            stubScreenProvider.SafeArea = new Rect(0, 0, 1920, 1080);
            yield return null;

            Assert.That(rectTransform.anchorMin, Is.EqualTo(new Vector2(0, 0)));
        }


        // TC-20: 極小画面サイズでも正しく動作する
        [UnityTest]
        public IEnumerator Awake_WhenVerySmallScreen_SetsAnchorsCorrectly()
        {
            var stubScreenProvider = new StubScreenProvider(
                new Rect(10, 10, 80, 80), 100, 100);

            var (sut, rectTransform) = CreateSystemUnderTest(stubScreenProvider);
            yield return null;

            Assert.That(rectTransform.anchorMin, Is.EqualTo(new Vector2(0.1f, 0.1f)));
            Assert.That(rectTransform.anchorMax, Is.EqualTo(new Vector2(0.9f, 0.9f)));
        }

        // TC-22: Safe Areaのwidthが0の場合でも例外が発生しない
        [UnityTest]
        public IEnumerator Awake_WhenSafeAreaWidthIsZero_DoesNotThrow()
        {
            var stubScreenProvider = new StubScreenProvider(
                new Rect(0, 0, 0, 1080), 1920, 1080);

            var (sut, rectTransform) = CreateSystemUnderTest(stubScreenProvider);
            yield return null;

            Assert.That(rectTransform.anchorMin.x, Is.EqualTo(rectTransform.anchorMax.x));
        }

        // TC-24: Safe Areaのheightが0の場合でも例外が発生しない
        [UnityTest]
        public IEnumerator Awake_WhenSafeAreaHeightIsZero_DoesNotThrow()
        {
            var stubScreenProvider = new StubScreenProvider(
                new Rect(0, 0, 1920, 0), 1920, 1080);

            var (sut, rectTransform) = CreateSystemUnderTest(stubScreenProvider);
            yield return null;

            Assert.That(rectTransform.anchorMin.y, Is.EqualTo(rectTransform.anchorMax.y));
        }
    }
}
