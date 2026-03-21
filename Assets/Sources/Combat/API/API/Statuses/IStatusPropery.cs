using Combat.API.DTO;
using Combat.API.ValueObjects;
using Combat.Common.Flags;

namespace Combat.API.Statuses
{
    public interface IStatusPropery { }

    public interface IOutgoingDamageModifier : IStatusPropery
    {
        float GetDamageDealthModification_Value(DamageInstanceApi instanceApi) => 0f;
        float GetDamageDealthModification_Percent(DamageInstanceApi instanceApi) => 0f;
        float GetDamageDealthModification_Bonus(DamageInstanceApi instanceApi) => 0f;
        DamageFlags GetDamageFlagMask(DamageInstanceApi instanceApi) => DamageFlags.None;
    }

    public interface IOutgoingHealingModifier : IStatusPropery
    {
        float GetBonusHealingDealth(HealingInstanceApi instanceApi) => 0f;
        float GetBonusHealingDealthPercent(HealingInstanceApi instanceApi) => 0f;
        float GetBonusHealingDealthValue(HealingInstanceApi instanceApi) => 0f;
        HealingFlags GetHealingFlagMask(HealingInstanceApi instanceApi) => HealingFlags.None;
    }

    public interface IIncomingHealDamageModifier : IStatusPropery
    {
        float GetBonusDamageRecived(DamageInstanceApi instanceApi) => 0f;
        float GetBonusDamageRecivedPercent(DamageInstanceApi instanceApi) => 0f;
        float GetBonusDamageRecivedValue(DamageInstanceApi instanceApi) => 0f;
        DamageFlags GetDamageFlagMask(DamageInstanceApi instanceApi) => DamageFlags.None;
    }

    public interface ITimeScaleModifier : IStatusPropery
    {
        float GetModification() => 0f;
    }

    public interface IAttributesModifier : IStatusPropery
    {
        void GetAttributesBonuses(AttributesData data);
    }

    public interface IResourceGainSpendHandler
    {
        void OnGainResource(ResourceChangeRecord @event);
        void OnSpendResource(ResourceChangeRecord @event);
    }
}
