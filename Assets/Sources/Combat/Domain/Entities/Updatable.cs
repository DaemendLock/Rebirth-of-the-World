using Combat.Common.Primitives;

namespace Combat.Domain.Entities.Units
{
    public readonly struct Updatable
    {
        public Updatable(UnitId id, float timeScale)
        {
            Id = id;
            TimeScale = timeScale;

            if (TimeScale < 0)
            {
                TimeScale = 0;
            }
        }

        public UnitId Id { get; }
        public float TimeScale { get; }
    }
}
