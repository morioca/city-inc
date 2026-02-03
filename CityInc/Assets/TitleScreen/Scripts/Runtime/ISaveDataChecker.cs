namespace TitleScreen
{
    /// <summary>
    /// Interface for checking save data existence.
    /// </summary>
    public interface ISaveDataChecker
    {
        /// <summary>
        /// Gets a value indicating whether save data exists.
        /// </summary>
        bool HasSaveData { get; }
    }
}
