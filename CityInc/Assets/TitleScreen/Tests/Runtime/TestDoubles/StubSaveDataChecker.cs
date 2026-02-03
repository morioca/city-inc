namespace TitleScreen
{
    public class StubSaveDataChecker : ISaveDataChecker
    {
        public bool HasSaveData { get; set; }

        public StubSaveDataChecker(bool hasSaveData)
        {
            HasSaveData = hasSaveData;
        }
    }
}
