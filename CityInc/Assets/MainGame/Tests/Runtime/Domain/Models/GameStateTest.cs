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
        public void GameState_Constructor_WhenCalledWithBudgetAllocation_SetsBudgetAllocationProperty()
        {
            var allocation = new BudgetAllocation(10000000L, 20000000L, 15000000L, 25000000L, 5000000L, 25000000L);
            var sut = new GameState(new GameDate(1, 1), 50000, 100000000L, 60, allocation);

            Assert.That(sut.CurrentAllocation, Is.EqualTo(allocation));
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

        [Test]
        public void GameState_CreateInitial_WhenCalled_BudgetAllocationMatchesTotalBudget()
        {
            var sut = GameState.CreateInitial();
            var expected = BudgetAllocation.EqualDistribution(100000000L);

            Assert.That(sut.Budget, Is.EqualTo(100000000L));
            Assert.That(sut.CurrentAllocation.TotalAllocated, Is.EqualTo(expected.TotalAllocated));
        }
    }
}
