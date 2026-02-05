namespace Domain.Models
{
    /// <summary>
    /// Represents the entire game state as an immutable value object.
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// The current date of the game.
        /// </summary>
        public GameDate CurrentDate { get; }

        /// <summary>
        /// Create a new GameState with the specified date.
        /// </summary>
        /// <param name="currentDate">The current game date</param>
        public GameState(GameDate currentDate)
        {
            CurrentDate = currentDate;
        }
    }
}
