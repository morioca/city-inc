using Domain.Models;
using Domain.Systems;
using NUnit.Framework;

namespace Domain.Systems
{
    [TestFixture]
    public class TurnManagerTest
    {
        [Test]
        public void ProgressToNextMonth_WhenNormalMonth_ReturnsStateWithNextMonth()
        {
            var sut = new TurnManager();
            var currentState = new GameState(new GameDate(2024, 4));

            var actual = sut.ProgressToNextMonth(currentState);

            Assert.That(actual.CurrentDate, Is.EqualTo(new GameDate(2024, 5)));
        }

        [Test]
        public void ProgressToNextMonth_WhenDecember_ReturnsStateWithNextYearJanuary()
        {
            var sut = new TurnManager();
            var currentState = new GameState(new GameDate(2024, 12));

            var actual = sut.ProgressToNextMonth(currentState);

            Assert.That(actual.CurrentDate, Is.EqualTo(new GameDate(2025, 1)));
        }

        [Test]
        public void ProgressToNextMonth_WhenCalled_DoesNotModifyOriginalState()
        {
            var sut = new TurnManager();
            var currentState = new GameState(new GameDate(2024, 4));
            var expected = new GameDate(2024, 4);

            sut.ProgressToNextMonth(currentState);

            Assert.That(currentState.CurrentDate, Is.EqualTo(expected));
        }
    }
}
