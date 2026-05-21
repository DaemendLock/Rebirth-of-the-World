using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Units
{
    public readonly struct Updatable
    {
        public Updatable(UnitId id, float timeScale)
        {
            Id = id;
            TimeScale = timeScale;

            if (timeScale < 0)
            {
                timeScale = 0;
            }
        }

        public UnitId Id { get; }
        public float TimeScale { get; }
    }
}
