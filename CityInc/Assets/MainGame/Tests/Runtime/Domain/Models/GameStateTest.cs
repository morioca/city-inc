using Domain.Models;
using NUnit.Framework;

namespace Domain.Models
{
    [TestFixture]
    public class GameStateTest
    {
        [Test]
        public void Constructor_WhenGivenGameDate_StoresItAsCurrentDate()
        {
            var date = new GameDate(2024, 4);

            var sut = new GameState(date);

            Assert.That(sut.CurrentDate, Is.EqualTo(date));
        }

        [Test]
        public void CurrentDate_WhenCreatedWithSameDate_AlwaysReturnsSameValue()
        {
            var date = new GameDate(2024, 4);
            var sut = new GameState(date);

            var first = sut.CurrentDate;
            var second = sut.CurrentDate;

            Assert.That(first, Is.EqualTo(second));
        }
    }
}
