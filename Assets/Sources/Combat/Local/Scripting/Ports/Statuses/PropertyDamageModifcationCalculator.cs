using Combat.API.API.IDK;
using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Idk.Capabilities.Statuses;

using System;

namespace Combat.Local.Scripting.Ports.Statuses
{
    public sealed class PropertyDamageModifcationCalculator : IHealingDamageModifierCalculator
    {
        private readonly IStatusRuntimeRegistry _statusRegistry;

        public PropertyDamageModifcationCalculator(IStatusRuntimeRegistry statusRegistry)
        {
            _statusRegistry = statusRegistry;
        }

        public DamageModification GetAttackerDamageModification(ReadOnlySpan<StatusId> values, in DamageInstance instance)
        {
            DamageModification result = new(0, 0, 0, DamageFlags.None);

            foreach (StatusId id in values)
            {
                if (_statusRegistry.TryGet(id, out var properties) == false)
                {
                    continue;
                }

                if (properties.TryGetProperty(out ModifyOutgoingDamageCapability effect) == false)
                {
                    continue;
                }

                result += effect.GetModification(instance);
            }

            return result;
        }

        public DamageModification GetDefenderDamageModification(ReadOnlySpan<StatusId> values, in DamageInstance instance)
        {
            DamageModification result = new(0, 0, 0, DamageFlags.None);

            foreach (StatusId id in values)
            {
                if (_statusRegistry.TryGet(id, out var properties) == false)
                {
                    continue;
                }

                if (properties.TryGetProperty(out ModifyIncomingDamageCapability effect) == false)
                {
                    continue;
                }

                result += effect.GetModification(instance);
            }

            return result;
        }

        public HealingModification GetHealingModification(ReadOnlySpan<StatusId> values, in HealingInstance instance)
        {
            HealingModification result = new(0, 0, 0, HealingFlags.None);

            foreach (StatusId id in values)
            {
                if (_statusRegistry.TryGet(id, out var properties) == false)
                {
                    continue;
                }

                if (properties.TryGetProperty(out ModifyOutgoingHealingCapability effect) == false)
                {
                    continue;
                }

                HealingModification modification = effect.GetModification(instance);
                result = new(result.BaseValue + modification.BaseValue,
                    result.PercentModication + modification.PercentModication,
                    result.BonusValue + modification.BonusValue,
                    result.FlagsModification | modification.FlagsModification);
            }

            return result;
        }
    }
}
