namespace Combat.Common
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

        public readonly float Left => FullDuration - ActiveTime;

        public void Progress(float deltaTime)
        {
            ActiveTime += deltaTime;
        }
    }
}
