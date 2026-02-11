using Domain.Models;
using NUnit.Framework;

namespace Domain.Models
{
    [TestFixture]
    public class GameStateTest
    {
        [Test]
        public void GameState_CreateInitial_WhenCalled_ReturnsCorrectInitialState()
        {
            var sut = GameState.CreateInitial();

            Assert.That(sut.CurrentDate, Is.EqualTo(new GameDate(1, 1)));
            Assert.That(sut.Population, Is.EqualTo(50000));
            Assert.That(sut.Budget, Is.EqualTo(100000000L));
            Assert.That(sut.ApprovalRating, Is.EqualTo(60));
        }
    }
}
