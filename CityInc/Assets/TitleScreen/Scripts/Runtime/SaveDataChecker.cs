using System.IO;
using UnityEngine;

namespace TitleScreen
{
    /// <summary>
    /// Checks whether save data exists.
    /// </summary>
    public class SaveDataChecker : ISaveDataChecker
    {
        private const string SaveFileName = "save.dat";

        /// <inheritdoc/>
        public bool HasSaveData => File.Exists(Path.Combine(Application.persistentDataPath, SaveFileName));
    }
}
