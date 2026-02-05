using System.Collections;
using Domain.Models;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Presentation
{
    [TestFixture]
    public class GameStatePresenterTest
    {
        private GameObject _sutObject;

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_sutObject);
        }

        private GameObject CreateSystemUnderTestObject()
        {
            var gameObject = new GameObject("GameStatePresenter");
            var presenter = gameObject.AddComponent<GameStatePresenter>();

            var dateLabelObj = new GameObject("DateLabel");
            dateLabelObj.transform.SetParent(gameObject.transform);
            var dateLabel = dateLabelObj.AddComponent<TextMeshProUGUI>();
            SetPrivateSerializedField(presenter, "<DateLabel>k__BackingField", dateLabel);

            var nextMonthButtonObj = new GameObject("NextMonthButton");
            nextMonthButtonObj.transform.SetParent(gameObject.transform);
            nextMonthButtonObj.AddComponent<Image>();
            var nextMonthButton = nextMonthButtonObj.AddComponent<Button>();
            SetPrivateSerializedField(presenter, "<NextMonthButton>k__BackingField", nextMonthButton);

            return gameObject;
        }

        private void SetPrivateSerializedField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(obj, value);
        }

        [UnityTest]
        public IEnumerator Initialize_WhenCalled_DisplaysCorrectDate()
        {
            _sutObject = CreateSystemUnderTestObject();
            var sut = _sutObject.GetComponent<GameStatePresenter>();
            yield return null;

            sut.Initialize(new GameState(new GameDate(2024, 4)));

            Assert.That(sut.DateLabel.text, Is.EqualTo("2024年04月"));
        }

        [UnityTest]
        public IEnumerator OnNextMonthButtonClicked_WhenNormalMonth_DisplaysNextMonth()
        {
            _sutObject = CreateSystemUnderTestObject();
            var sut = _sutObject.GetComponent<GameStatePresenter>();
            yield return null;

            sut.Initialize(new GameState(new GameDate(2024, 4)));
            sut.OnNextMonthButtonClicked();

            Assert.That(sut.DateLabel.text, Is.EqualTo("2024年05月"));
        }

        [UnityTest]
        public IEnumerator OnNextMonthButtonClicked_WhenDecember_DisplaysNextYearJanuary()
        {
            _sutObject = CreateSystemUnderTestObject();
            var sut = _sutObject.GetComponent<GameStatePresenter>();
            yield return null;

            sut.Initialize(new GameState(new GameDate(2024, 12)));
            sut.OnNextMonthButtonClicked();

            Assert.That(sut.DateLabel.text, Is.EqualTo("2025年01月"));
        }
    }
}
