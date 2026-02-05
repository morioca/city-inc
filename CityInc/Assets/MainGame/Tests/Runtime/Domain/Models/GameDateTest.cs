using Domain.Models;
using NUnit.Framework;

namespace Domain.Models
{
    [TestFixture]
    public class GameDateTest
    {
        [TestCase(2024, 1, 2024, 2)]
        [TestCase(2024, 6, 2024, 7)]
        [TestCase(2024, 11, 2024, 12)]
        public void AddMonths_WhenNormalMonth_ReturnsNextMonth(
            int year, int month, int expectedYear, int expectedMonth)
        {
            var sut = new GameDate(year, month);

            var actual = sut.AddMonths(1);

            Assert.That(actual, Is.EqualTo(new GameDate(expectedYear, expectedMonth)));
        }

        [Test]
        public void AddMonths_WhenDecember_ReturnsNextYearJanuary()
        {
            var sut = new GameDate(2024, 12);

            var actual = sut.AddMonths(1);

            Assert.That(actual, Is.EqualTo(new GameDate(2025, 1)));
        }

        [Test]
        public void AddMonths_WhenTwelveMonths_ReturnsSameMonthNextYear()
        {
            var sut = new GameDate(2024, 4);

            var actual = sut.AddMonths(12);

            Assert.That(actual, Is.EqualTo(new GameDate(2025, 4)));
        }

        [Test]
        public void AddMonths_WhenZero_ReturnsSameDate()
        {
            var sut = new GameDate(2024, 7);

            var actual = sut.AddMonths(0);

            Assert.That(actual, Is.EqualTo(new GameDate(2024, 7)));
        }

        [Test]
        public void AddMonths_WhenNegativeOneFromNormalMonth_ReturnsPreviousMonth()
        {
            var sut = new GameDate(2024, 2);

            var actual = sut.AddMonths(-1);

            Assert.That(actual, Is.EqualTo(new GameDate(2024, 1)));
        }

        [Test]
        public void AddMonths_WhenNegativeOneFromJanuary_ReturnsPreviousYearDecember()
        {
            var sut = new GameDate(2024, 1);

            var actual = sut.AddMonths(-1);

            Assert.That(actual, Is.EqualTo(new GameDate(2023, 12)));
        }

        [TestCase(2024, 1, "2024年01月")]
        [TestCase(2024, 4, "2024年04月")]
        [TestCase(2024, 9, "2024年09月")]
        public void ToDisplayString_WhenSingleDigitMonth_ReturnsZeroPaddedFormat(
            int year, int month, string expected)
        {
            var sut = new GameDate(year, month);

            var actual = sut.ToDisplayString();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(2024, 10, "2024年10月")]
        [TestCase(2024, 11, "2024年11月")]
        [TestCase(2024, 12, "2024年12月")]
        public void ToDisplayString_WhenDoubleDigitMonth_ReturnsFormattedString(
            int year, int month, string expected)
        {
            var sut = new GameDate(year, month);

            var actual = sut.ToDisplayString();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
