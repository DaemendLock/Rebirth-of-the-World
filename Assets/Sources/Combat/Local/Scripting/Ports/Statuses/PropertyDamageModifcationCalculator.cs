using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Capabilities.Statuses;
using Combat.Local.Scripting.Runtime;

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
                if (_statusRegistry.TryGet(id, out StatusRuntime runtime) == false)
                {
                    continue;
                }

                IStatusModifyOutgoingDamageCapability effect = runtime.Container.GetCapability<IStatusModifyOutgoingDamageCapability>();

                if (effect == null)
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
                if (_statusRegistry.TryGet(id, out StatusRuntime runtime) == false)
                {
                    continue;
                }

                IStatusModifyIncomingDamageCapability effect = runtime.Container.GetCapability<IStatusModifyIncomingDamageCapability>();

                if (effect == null)
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
                if (_statusRegistry.TryGet(id, out StatusRuntime runtime) == false)
                {
                    continue;
                }

                IStatusModifyOutgoingHealingCapability effect = runtime.Container.GetCapability<IStatusModifyOutgoingHealingCapability>();

                if (effect == null)
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
