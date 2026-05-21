namespace Combat.Local.Domain.ValueObjects
{
    public struct Duration
    {
        public Duration(float activeTime, float fullDuration)
        {
            ActiveTime = activeTime;
            FullDuration = fullDuration;
        }

        public float ActiveTime { get; set; }
        public float FullDuration { get; set; }

        public float Left => FullDuration - ActiveTime;

        public void Progress(float deltaTime)
        {
            ActiveTime += deltaTime;
        }
    }
}
