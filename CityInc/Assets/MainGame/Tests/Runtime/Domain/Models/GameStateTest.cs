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

        [Test]
        public void GameState_Constructor_WhenGivenPopulation_StoresItAsPopulation()
        {
            var population = 50000;

            var sut = new GameState(new GameDate(1, 1), population, 100000000L, 60);

            Assert.That(sut.Population, Is.EqualTo(50000));
        }

        [Test]
        public void GameState_Constructor_WhenGivenBudget_StoresItAsBudget()
        {
            var budget = 100000000L;

            var sut = new GameState(new GameDate(1, 1), 50000, budget, 60);

            Assert.That(sut.Budget, Is.EqualTo(100000000L));
        }

        [Test]
        public void GameState_Constructor_WhenGivenApprovalRating_StoresItAsApprovalRating()
        {
            var approvalRating = 60;

            var sut = new GameState(new GameDate(1, 1), 50000, 100000000L, approvalRating);

            Assert.That(sut.ApprovalRating, Is.EqualTo(60));
        }

        [Test]
        public void GameState_CreateInitial_WhenCalled_ReturnsYear1Month1()
        {
            var sut = GameState.CreateInitial();

            Assert.That(sut.CurrentDate, Is.EqualTo(new GameDate(1, 1)));
        }

        [Test]
        public void GameState_CreateInitial_WhenCalled_ReturnsPopulation50000()
        {
            var sut = GameState.CreateInitial();

            Assert.That(sut.Population, Is.EqualTo(50000));
        }

        [Test]
        public void GameState_CreateInitial_WhenCalled_ReturnsBudget100000000()
        {
            var sut = GameState.CreateInitial();

            Assert.That(sut.Budget, Is.EqualTo(100000000L));
        }

        [Test]
        public void GameState_CreateInitial_WhenCalled_ReturnsApprovalRating60()
        {
            var sut = GameState.CreateInitial();

            Assert.That(sut.ApprovalRating, Is.EqualTo(60));
        }

        [Test]
        public void GameState_Population_WhenAccessedTwice_ReturnsSameValue()
        {
            var sut = new GameState(new GameDate(1, 1), 50000, 100000000L, 60);

            var first = sut.Population;
            var second = sut.Population;

            Assert.That(first, Is.EqualTo(second));
        }

        [Test]
        public void GameState_Budget_WhenAccessedTwice_ReturnsSameValue()
        {
            var sut = new GameState(new GameDate(1, 1), 50000, 100000000L, 60);

            var first = sut.Budget;
            var second = sut.Budget;

            Assert.That(first, Is.EqualTo(second));
        }

        [Test]
        public void GameState_ApprovalRating_WhenAccessedTwice_ReturnsSameValue()
        {
            var sut = new GameState(new GameDate(1, 1), 50000, 100000000L, 60);

            var first = sut.ApprovalRating;
            var second = sut.ApprovalRating;

            Assert.That(first, Is.EqualTo(second));
        }
    }
}
