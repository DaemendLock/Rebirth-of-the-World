using Combat.API;
using Combat.API.Adapters;
using Combat.API.API.IDK;
using Combat.API.Statuses;
using Combat.API.ValueObjects;
using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.ValueObjects;

using System;

using UnityEngine.SocialPlatforms;

namespace Combat.Local.Scripting.Ports.Statuses
{
    public sealed class PropertyDamageModifcationCalculator : IHealingDamageModifierCalculator
    {
        private readonly IStatusRuntimeRegistry _statusRegisrty;
        private readonly ICharacterApiAdapter _unitApiAdapter;
        private readonly IAbilityApiAdapter _skillApiProvider;

        public PropertyDamageModifcationCalculator(IStatusRuntimeRegistry statusRepository, ICharacterApiAdapter unitApiAdapter, IAbilityApiAdapter skillApiProvider)
        {
            _statusRegisrty = statusRepository;
            _unitApiAdapter = unitApiAdapter;
            _skillApiProvider = skillApiProvider;
        }

        public DamageModification GetAttackerDamageModification(ReadOnlySpan<StatusId> values, in DamageInstance instance)
        {
            Unit target = _unitApiAdapter.Adaptee(instance.Target);
            Unit attacker = instance.Attacker.HasValue ? _unitApiAdapter.Adaptee(instance.Attacker.Value) : null;
            AbilityApi source;

            if (instance.Source.HasValue)
            {
                source = _skillApiProvider.Adaptee(instance.Source.Value);
            }
            else
            {
                source = null;
            }

            DamageInstanceApi damageInstanceApi = new(target, attacker, source, instance.OriginalDamage, instance.Flags);
            DamageModification result = new(0, 0, 0, DamageFlags.None);

            foreach (StatusId id in values)
            {
                if (_statusRegisrty.TryGet(id, out var properties) == false)
                {
                    continue;
                }

                if (properties.TryGetProperty(out IOutgoingDamageModifier effect) == false)
                {
                    continue;
                }

                result += GetModification(effect, damageInstanceApi);
            }

            return result;
        }

        public DamageModification GetDefenderDamageModification(ReadOnlySpan<StatusId> values, in DamageInstance instance)
        {
            DamageInstanceApi damageInstanceApi = AdaptDamageInstance(instance);
            DamageModification result = new(0, 0, 0, DamageFlags.None);

            foreach (StatusId id in values)
            {
                if (_statusRegisrty.TryGet(id, out var properties) == false)
                {
                    continue;
                }

                if (properties.TryGetProperty(out IIncomingHealDamageModifier effect) == false)
                {
                    continue;
                }

                result += GetModification(effect, damageInstanceApi);
            }

            return result;
        }

        public HealingModification GetHealingModification(ReadOnlySpan<StatusId> values, in HealingInstance instance)
        {
            HealingInstanceApi healingInstanceApi = AdaptHealingInstance(instance);
            HealingModification finalModification = new(0, 0, 0, HealingFlags.None);

            foreach (StatusId id in values)
            {
                if (_statusRegisrty.TryGet(id, out var properties) == false)
                {
                    continue;
                }

                if (properties.TryGetProperty(out IOutgoingHealingModifier effect) == false)
                {
                    continue;
                }

                HealingModification modification = GetModification(effect, healingInstanceApi);
                finalModification = new(finalModification.BaseValue + modification.BaseValue,
                        finalModification.PercentModication + modification.PercentModication,
                        finalModification.BonusValue + modification.BonusValue,
                        finalModification.FlagsModification | modification.FlagsModification);
            }

            return finalModification;
        }

        private DamageInstanceApi AdaptDamageInstance(DamageInstance instance)
        {
            Unit target = _unitApiAdapter.Adaptee(instance.Target);
            Unit attacker = instance.Attacker.HasValue ? _unitApiAdapter.Adaptee(instance.Attacker.Value) : null;
            AbilityApi source;

            if (instance.Source.HasValue)
            {
                source = _skillApiProvider.Adaptee(instance.Source.Value);
            }
            else
            {
                source = null;
            }

            return new DamageInstanceApi(target, attacker, source, instance.OriginalDamage, instance.Flags);
        }

        private HealingInstanceApi AdaptHealingInstance(HealingInstance instance)
        {
            Unit target = _unitApiAdapter.Adaptee(instance.Target);
            Unit attacker = instance.Healer.HasValue ? _unitApiAdapter.Adaptee(instance.Healer.Value) : null;
            AbilityApi source;

            if (instance.Source.HasValue)
            {
                source = _skillApiProvider.Adaptee(instance.Source.Value);
            }
            else
            {
                source = null;
            }

            return new HealingInstanceApi(target, attacker, source, instance.OriginalHealing, instance.Flags);
        }

        private DamageModification GetModification(IOutgoingDamageModifier modifier, DamageInstanceApi instanceApi) =>
            new(modifier.GetDamageDealthModification_Value(instanceApi),
                modifier.GetDamageDealthModification_Percent(instanceApi),
                modifier.GetDamageDealthModification_Bonus(instanceApi),
                modifier.GetDamageFlagMask(instanceApi));

        private DamageModification GetModification(IIncomingHealDamageModifier modifier, DamageInstanceApi instanceApi) =>
            new(modifier.GetBonusDamageRecivedValue(instanceApi),
                modifier.GetBonusDamageRecivedPercent(instanceApi),
                modifier.GetBonusDamageRecived(instanceApi),
                modifier.GetDamageFlagMask(instanceApi));

        private HealingModification GetModification(IOutgoingHealingModifier modifier, HealingInstanceApi instanceApi) =>
            new(modifier.GetBonusHealingDealthValue(instanceApi),
                modifier.GetBonusHealingDealthPercent(instanceApi),
                modifier.GetBonusHealingDealth(instanceApi),
                modifier.GetHealingFlagMask(instanceApi));
    }
}
