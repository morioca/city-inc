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

        [Test]
        public void GameState_CreateInitial_WhenCalled_SetsEqualDistributionBudgetAllocation()
        {
            var sut = GameState.CreateInitial();

            Assert.That(sut.CurrentAllocation.WelfareHealthcare, Is.EqualTo(16666666L));
            Assert.That(sut.CurrentAllocation.EducationChildcare, Is.EqualTo(16666666L));
            Assert.That(sut.CurrentAllocation.IndustryDevelopment, Is.EqualTo(16666666L));
            Assert.That(sut.CurrentAllocation.Infrastructure, Is.EqualTo(16666666L));
            Assert.That(sut.CurrentAllocation.DisasterPrevention, Is.EqualTo(16666666L));
            Assert.That(sut.CurrentAllocation.TourismCulture, Is.EqualTo(16666666L));
        }
    }
}
