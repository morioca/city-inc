using System;

namespace Domain.Models
{
    /// <summary>
    /// Represents a year and month in the game world as an immutable value object.
    /// </summary>
    public readonly struct GameDate : IEquatable<GameDate>
    {
        /// <summary>
        /// The year component of the game date.
        /// </summary>
        public int Year { get; }

        /// <summary>
        /// The month component of the game date (1-12).
        /// </summary>
        public int Month { get; }

        /// <summary>
        /// Create a new GameDate with the specified year and month.
        /// </summary>
        /// <param name="year">The year</param>
        /// <param name="month">The month (1-12)</param>
        public GameDate(int year, int month)
        {
            Year = year;
            Month = month;
        }

        /// <summary>
        /// Return a new GameDate with the specified number of months added.
        /// </summary>
        /// <param name="months">The number of months to add (can be negative)</param>
        /// <returns>A new GameDate</returns>
        public GameDate AddMonths(int months)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a display string in the format "YYYY年MM月".
        /// </summary>
        /// <returns>The formatted display string</returns>
        public string ToDisplayString()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Equals(GameDate other)
        {
            return Year == other.Year && Month == other.Month;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is GameDate other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Year, Month);
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        public static bool operator ==(GameDate left, GameDate right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        public static bool operator !=(GameDate left, GameDate right)
        {
            return !left.Equals(right);
        }
    }
}
