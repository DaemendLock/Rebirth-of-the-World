using Combat.API.ValueObjects;
using Combat.Common.Flags;

namespace Combat.API.Statuses
{
    public interface IIncomingHealDamageModifier : IStatusPropery
    {
        float GetBonusHealingRecived(HealingInstanceApi instance) => 0f;

        float GetBonusHealingRecivedPercent(HealingInstanceApi instance) => 0f;

        HealingFlags GetHealingFlagMask(HealingInstanceApi instance) => HealingFlags.None;

        float GetBonusDamageRecived(DamageInstanceApi instance) => 0f;

        float GetBonusDamageRecivedPercent(DamageInstanceApi instance) => 0f;

        DamageFlags GetDamageFlagMask(DamageInstanceApi instance) => DamageFlags.None;
    }
}
