namespace Data.Characters.Lobby
{
    public readonly struct ProgressValue
    {
        public readonly byte Level;
        public readonly byte MaxLevel;
        public readonly uint CurrentProgress;
        public readonly uint MaxProgression;

        public ProgressValue(byte level, byte maxLevel, uint currentProgress, uint maxProgression)
        {
            Level = level;
            MaxLevel = maxLevel;
            CurrentProgress = currentProgress;
            MaxProgression = maxProgression;
        }
    }
}
