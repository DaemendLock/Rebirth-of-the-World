using Combat.Common.Flags;
using Combat.Common.Primitives;
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

        public DamageModification GetAttackerDamageModification(ReadOnlySpan<StatusInstance> values, in DamageInstance instance)
        {
            DamageModification result = new(0, 0, 0, DamageFlags.None);

            foreach (var info in values)
            {
                if (_statusRegistry.TryGet(info.StatusId, out StatusRuntime runtime) == false)
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

        public DamageModification GetDefenderDamageModification(ReadOnlySpan<StatusInstance> values, in DamageInstance instance)
        {
            DamageModification result = new(0, 0, 0, DamageFlags.None);

            foreach (var info in values)
            {
                if (_statusRegistry.TryGet(info.StatusId, out StatusRuntime runtime) == false)
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

        public HealingModification GetHealingModification(ReadOnlySpan<StatusInstance> values, in HealingInstance instance)
        {
            HealingModification result = new(0, 0, 0, HealingFlags.None);

            foreach (StatusInstance info in values)
            {
                if (_statusRegistry.TryGet(info.StatusId, out StatusRuntime runtime) == false)
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
