namespace Combat.Common
{

    public readonly struct Duration
    {
        public Duration(float activeTime, float fullDuration)
        {
            ActiveTime = activeTime;
            FullDuration = fullDuration;
        }

        public float ActiveTime { get; }
        public float FullDuration { get; }

        public readonly float Left => FullDuration - ActiveTime;

        public Duration Progress(float deltaTime) => new(ActiveTime + deltaTime, FullDuration);
    }
}
