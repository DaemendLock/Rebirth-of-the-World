using Combat.Common.Primitives;

namespace Combat.Domain.Entities.Units
{
    public readonly struct Updatable
    {
        public Updatable(UnitContext unit, float timeScale)
        {
            Unit = unit;
            TimeScale = timeScale;

            if (TimeScale < 0)
            {
                TimeScale = 0;
            }
        }

        public UnitContext Unit { get; }
        public float TimeScale { get; }
    }
}
