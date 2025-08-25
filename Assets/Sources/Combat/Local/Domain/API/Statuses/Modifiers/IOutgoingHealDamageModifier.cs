using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.API.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.API.Statuses
{
    public interface IOutgoingHealDamageModifier : IStatusPropery
    {
        float GetBonusHealingDealth(HealingInstance instance) => 0f;

        float GetBonusHealingDealthPercent(HealingInstance instance) => 0f;

        HealingFlags GetHealingFlagMask(HealingInstance instance) => HealingFlags.None;

        float GetBonusDamageDealth(DamageInstance instance) => 0f;

        float GetBonusDamageDealthPercent(DamageInstance instance) => 0f;

        DamageFlags GetDamageFlagMask(DamageInstance instance) => DamageFlags.None;
    }
}
