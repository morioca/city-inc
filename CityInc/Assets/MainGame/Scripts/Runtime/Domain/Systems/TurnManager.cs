using Domain.Models;

namespace Domain.Systems
{
    /// <summary>
    /// Handles the turn progression logic.
    /// </summary>
    public class TurnManager
    {
        /// <summary>
        /// Advance the game state by one month and return the new state.
        /// </summary>
        /// <param name="currentState">The current game state</param>
        /// <returns>A new GameState with the date advanced by one month</returns>
        public GameState ProgressToNextMonth(GameState currentState)
        {
            var nextDate = currentState.CurrentDate.AddMonths(1);
            return new GameState(
                nextDate,
                currentState.Population,
                currentState.Budget,
                currentState.ApprovalRating,
                currentState.CurrentAllocation
            );
        }
    }
}
