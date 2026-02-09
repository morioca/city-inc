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
        /// The city's population.
        /// </summary>
        public int Population { get; }

        /// <summary>
        /// The city's budget in yen.
        /// </summary>
        public long Budget { get; }

        /// <summary>
        /// The mayor's approval rating (0-100).
        /// </summary>
        public int ApprovalRating { get; }

        /// <summary>
        /// Create a new GameState with the specified date.
        /// </summary>
        /// <param name="currentDate">The current game date</param>
        public GameState(GameDate currentDate)
        {
            CurrentDate = currentDate;
        }

        /// <summary>
        /// Create a new GameState with all parameters.
        /// </summary>
        /// <param name="currentDate">The current game date</param>
        /// <param name="population">The city's population</param>
        /// <param name="budget">The city's budget in yen</param>
        /// <param name="approvalRating">The mayor's approval rating (0-100)</param>
        public GameState(GameDate currentDate, int population, long budget, int approvalRating)
        {
            CurrentDate = currentDate;
            Population = population;
            Budget = budget;
            ApprovalRating = approvalRating;
        }

        /// <summary>
        /// Create an initial game state with default values.
        /// </summary>
        /// <returns>A new GameState with initial values</returns>
        public static GameState CreateInitial()
        {
            throw new System.NotImplementedException();
        }
    }
}
