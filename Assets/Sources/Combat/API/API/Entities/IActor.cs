using Combat.Common.ValueObjects;

namespace Combat.API
{
    // OnScene/External elements
    public interface IActor
    {
        UnitId Id { get; }
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
