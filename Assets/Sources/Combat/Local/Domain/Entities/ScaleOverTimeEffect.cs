using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Units
{
    public readonly struct ScaleOverTimeEffect
    {
        public readonly ScaleEffectId Id;
        public readonly float Rate;

        public ScaleOverTimeEffect(ScaleEffectId id, float rate)
        {
            Id = id;
            Rate = rate;
        }
    }
}
