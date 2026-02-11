using Domain.Models;
using NUnit.Framework;

namespace Domain.Models
{
    [TestFixture]
    public class BudgetAllocationTest
    {
        [Test]
        public void BudgetAllocation_Constructor_WhenCalledWithValidValues_SetsAllProperties()
        {
            var sut = new BudgetAllocation(
                welfareHealthcare: 10000000L,
                educationChildcare: 20000000L,
                industryDevelopment: 15000000L,
                infrastructure: 25000000L,
                disasterPrevention: 5000000L,
                tourismCulture: 25000000L);

            Assert.That(sut.WelfareHealthcare, Is.EqualTo(10000000L));
            Assert.That(sut.EducationChildcare, Is.EqualTo(20000000L));
            Assert.That(sut.IndustryDevelopment, Is.EqualTo(15000000L));
            Assert.That(sut.Infrastructure, Is.EqualTo(25000000L));
            Assert.That(sut.DisasterPrevention, Is.EqualTo(5000000L));
            Assert.That(sut.TourismCulture, Is.EqualTo(25000000L));
        }

        [Test]
        public void BudgetAllocation_TotalAllocated_WhenCategoriesAreZero_ReturnsZero()
        {
            var sut = new BudgetAllocation(0, 0, 0, 0, 0, 0);

            var actual = sut.TotalAllocated;

            Assert.That(actual, Is.EqualTo(0L));
        }

        [Test]
        public void BudgetAllocation_TotalAllocated_WhenCategoriesHaveValues_ReturnsCorrectSum()
        {
            var sut = new BudgetAllocation(10000000L, 20000000L, 15000000L, 25000000L, 5000000L, 25000000L);

            var actual = sut.TotalAllocated;

            Assert.That(actual, Is.EqualTo(100000000L));
        }

        [Test]
        public void BudgetAllocation_TotalAllocated_WhenSingleCategoryHasAllBudget_ReturnsTotalBudget()
        {
            var sut = new BudgetAllocation(100000000L, 0, 0, 0, 0, 0);

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
        public void BudgetAllocation_EqualDistribution_WhenTotalBudgetIs0_ReturnsZeroAllocation()
        {
            var sut = BudgetAllocation.EqualDistribution(0L);

            Assert.That(sut.WelfareHealthcare, Is.EqualTo(0L));
            Assert.That(sut.EducationChildcare, Is.EqualTo(0L));
            Assert.That(sut.IndustryDevelopment, Is.EqualTo(0L));
            Assert.That(sut.Infrastructure, Is.EqualTo(0L));
            Assert.That(sut.DisasterPrevention, Is.EqualTo(0L));
            Assert.That(sut.TourismCulture, Is.EqualTo(0L));
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

        [Test]
        public void BudgetAllocation_Constructor_WhenCalledWithNegativeValues_AcceptsNegativeValues()
        {
            var sut = new BudgetAllocation(-1000L, -2000L, -3000L, -4000L, -5000L, -6000L);

            Assert.That(sut.WelfareHealthcare, Is.EqualTo(-1000L));
            Assert.That(sut.EducationChildcare, Is.EqualTo(-2000L));
            Assert.That(sut.IndustryDevelopment, Is.EqualTo(-3000L));
            Assert.That(sut.Infrastructure, Is.EqualTo(-4000L));
            Assert.That(sut.DisasterPrevention, Is.EqualTo(-5000L));
            Assert.That(sut.TourismCulture, Is.EqualTo(-6000L));
        }
    }
}
