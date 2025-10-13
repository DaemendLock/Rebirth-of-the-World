using Combat.API.ValueObjects;
using Combat.Common.Flags;

namespace Combat.API.Statuses
{
    public interface IOutgoingHealDamageModifier : IStatusPropery
    {
        float GetBonusHealingDealth(HealingInstanceApi instance) => 0f;

        float GetBonusHealingDealthPercent(HealingInstanceApi instance) => 0f;

        HealingFlags GetHealingFlagMask(HealingInstanceApi instance) => HealingFlags.None;

        float GetBonusDamageDealth(DamageInstanceApi instance) => 0f;

        float GetBonusDamageDealthPercent(DamageInstanceApi instance) => 0f;

        DamageFlags GetDamageFlagMask(DamageInstanceApi instance) => DamageFlags.None;
    }
}
