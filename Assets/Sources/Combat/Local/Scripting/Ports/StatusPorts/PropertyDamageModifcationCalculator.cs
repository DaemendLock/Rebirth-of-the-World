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

namespace Combat.Local.Scripting.Ports.StatusPorts
{
    public sealed class PropertyDamageModifcationCalculator : IDamageModifierCalculator
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

                if (properties.TryGetProperty(out IIncomingHealDamageModifier effect) == false)
                {
                    continue;
                }

                result += GetModification(effect, damageInstanceApi);
            }

            return result;
        }

        private DamageModification GetModification(IOutgoingDamageModifier modifier, DamageInstanceApi instanceApi) =>
            new DamageModification(modifier.GetDamageDealthModification_Value(instanceApi),
                modifier.GetDamageDealthModification_Percent(instanceApi),
                modifier.GetDamageDealthModification_Bonus(instanceApi),
                modifier.GetDamageFlagMask(instanceApi));

        private DamageModification GetModification(IIncomingHealDamageModifier modifier, DamageInstanceApi instanceApi) =>
            new DamageModification(modifier.GetBonusDamageRecivedValue(instanceApi),
                modifier.GetBonusDamageRecivedPercent(instanceApi),
                modifier.GetBonusDamageRecived(instanceApi),
                modifier.GetDamageFlagMask(instanceApi));
    }
}
