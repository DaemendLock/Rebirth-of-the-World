using Combat.Common.Primitives;

namespace Combat.API
{
    public interface IActor
    {
        UnitId Id { get; }

        ScaleEffectId StartScaleOverTime(float rate);
        void StopScaleOverTime(ScaleEffectId id);
        //TODO: Prob tags
    }

    public interface IActionOwner
    {
        float CurrentHealth { get; }
    }

    public interface ITargetable
    {
        float CurrentHealth { get; }

        void ApplyDamage(DTO.ApplyDamageInfo damageInfo);
        void ApplyHealing(DTO.ApplyHealingInfo healingInfo);
    }
}
