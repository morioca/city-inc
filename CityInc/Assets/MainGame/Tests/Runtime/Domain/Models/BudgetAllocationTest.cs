using Domain.Models;
using NUnit.Framework;

namespace Domain.Models
{
    [TestFixture]
    public class BudgetAllocationTest
    {
        [Test]
        public void BudgetAllocation_TotalAllocated_WhenCategoriesHaveValues_ReturnsCorrectSum()
        {
            var sut = new BudgetAllocation(10000000L, 20000000L, 15000000L, 25000000L, 5000000L, 25000000L);

            var actual = sut.TotalAllocated;

            Assert.That(actual, Is.EqualTo(100000000L));
        }

        [Test]
        public void BudgetAllocation_EqualDistribution_WhenTotalBudgetIs100Million_ReturnsEqualAllocation()
        {
            var sut = BudgetAllocation.EqualDistribution(100000000L);

            Assert.That(sut.WelfareHealthcare, Is.EqualTo(16666666L));
            Assert.That(sut.EducationChildcare, Is.EqualTo(16666666L));
            Assert.That(sut.IndustryDevelopment, Is.EqualTo(16666666L));
            Assert.That(sut.Infrastructure, Is.EqualTo(16666666L));
            Assert.That(sut.DisasterPrevention, Is.EqualTo(16666666L));
            Assert.That(sut.TourismCulture, Is.EqualTo(16666666L));
        }

        [Test]
        public void BudgetAllocation_EqualDistribution_WhenTotalBudgetIs1000_ReturnsEqualDistribution()
        {
            var sut = BudgetAllocation.EqualDistribution(1000L);

            Assert.That(sut.WelfareHealthcare, Is.EqualTo(166L));
            Assert.That(sut.EducationChildcare, Is.EqualTo(166L));
            Assert.That(sut.IndustryDevelopment, Is.EqualTo(166L));
            Assert.That(sut.Infrastructure, Is.EqualTo(166L));
            Assert.That(sut.DisasterPrevention, Is.EqualTo(166L));
            Assert.That(sut.TourismCulture, Is.EqualTo(166L));
        }
    }
}
