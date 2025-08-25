using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.API.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.API.Statuses
{
    public interface IIncomingHealDamageModifier : IStatusPropery
    {
        float GetBonusHealingRecived(HealingInstance instance) => 0f;

        float GetBonusHealingRecivedPercent(HealingInstance instance) => 0f;

        HealingFlags GetHealingFlagMask(HealingInstance instance) => HealingFlags.None;

        float GetBonusDamageRecived(DamageInstance instance) => 0f;

        float GetBonusDamageRecivedPercent(DamageInstance instance) => 0f;

        DamageFlags GetDamageFlagMask(DamageInstance instance) => DamageFlags.None;
    }
}
