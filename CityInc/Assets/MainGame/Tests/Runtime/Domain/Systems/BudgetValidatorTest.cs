using Domain.Models;
using Domain.Systems;
using NUnit.Framework;

namespace Domain.Systems
{
    [TestFixture]
    public class BudgetValidatorTest
    {
        [Test]
        public void BudgetValidator_IsValid_WhenTotalAllocatedEqualsTotalBudget_ReturnsTrue()
        {
            var sut = new BudgetValidator();
            var allocation = new BudgetAllocation(10000000L, 20000000L, 15000000L, 25000000L, 5000000L, 25000000L);

            var actual = sut.IsValid(allocation, 100000000L);

            Assert.That(actual, Is.True);
        }

        [Test]
        public void BudgetValidator_IsValid_WhenTotalAllocatedExceedsTotalBudget_ReturnsFalse()
        {
            var sut = new BudgetValidator();
            var allocation = new BudgetAllocation(20000000L, 20000000L, 20000000L, 20000000L, 20000000L, 10000000L);

            var actual = sut.IsValid(allocation, 100000000L);

            Assert.That(actual, Is.False);
        }

        [Test]
        public void BudgetValidator_IsValid_WhenTotalAllocatedLessThanTotalBudget_ReturnsFalse()
        {
            var sut = new BudgetValidator();
            var allocation = new BudgetAllocation(10000000L, 10000000L, 10000000L, 10000000L, 10000000L, 40000000L);

            var actual = sut.IsValid(allocation, 100000000L);

            Assert.That(actual, Is.False);
        }

        [Test]
        public void BudgetValidator_IsValid_WhenTotalAllocatedIsZeroAndBudgetIsZero_ReturnsTrue()
        {
            var sut = new BudgetValidator();
            var allocation = new BudgetAllocation(0, 0, 0, 0, 0, 0);

            var actual = sut.IsValid(allocation, 0L);

            Assert.That(actual, Is.True);
        }

        [Test]
        public void BudgetValidator_GetRemaining_WhenTotalAllocatedLessThanBudget_ReturnsPositiveValue()
        {
            var sut = new BudgetValidator();
            var allocation = new BudgetAllocation(10000000L, 10000000L, 10000000L, 10000000L, 10000000L, 30000000L);

            var actual = sut.GetRemaining(allocation, 100000000L);

            Assert.That(actual, Is.EqualTo(20000000L));
        }

        [Test]
        public void BudgetValidator_GetRemaining_WhenTotalAllocatedExceedsBudget_ReturnsNegativeValue()
        {
            var sut = new BudgetValidator();
            var allocation = new BudgetAllocation(20000000L, 20000000L, 20000000L, 20000000L, 20000000L, 20000000L);

            var actual = sut.GetRemaining(allocation, 100000000L);

            Assert.That(actual, Is.EqualTo(-20000000L));
        }
    }
}
