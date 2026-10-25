using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Local.Application.Queries.Statuses;
using Combat.Local.Application.ValueObjects;
using Combat.Local.Scripting.Capabilities.Statuses;
using Combat.Local.Scripting.Runtime;

using System.Collections.Generic;

namespace Combat.Local.Scripting.Ports.Statuses
{
    public sealed class PropertyDamageModifcationCalculator : IHealingDamageModificationCalculator
    {
        private readonly IStatusRuntimeRegistry _statusRegistry;

        public PropertyDamageModifcationCalculator(IStatusRuntimeRegistry statusRegistry)
        {
            _statusRegistry = statusRegistry;
        }

        public DamageModification GetOutgoingDamageModification(IEnumerable<StatusId> values, in DamageInstance instance)
        {
            DamageModification result = new(0, 0, 0, DamageFlags.None);

            foreach (StatusId id in values)
            {
                if (_statusRegistry.TryGet(id, out StatusRuntime runtime) == false)
                {
                    continue;
                }

                if (runtime.Container.TryGetCapability<IStatusModifyOutgoingDamageCapability>(out var effect))
                {
                    result += effect.GetModification(instance);
                }
            }

            return result;
        }

        public DamageModification GetIncomingDamageModification(IEnumerable<StatusId> values, in DamageInstance instance)
        {
            DamageModification result = new(0, 0, 0, DamageFlags.None);

            foreach (StatusId id in values)
            {
                if (_statusRegistry.TryGet(id, out StatusRuntime runtime) == false)
                {
                    continue;
                }

                if (runtime.Container.TryGetCapability(out IStatusModifyIncomingDamageCapability effect))
                {
                    result += effect.GetModification(instance);
                }
            }

            return result;
        }

        public HealingModification GetOutgoingHealingModification(IEnumerable<StatusId> values, in HealingInstance instance)
        {
            HealingModification result = new(0, 0, 0, HealingFlags.None);

            foreach (StatusId id in values)
            {
                if (_statusRegistry.TryGet(id, out StatusRuntime runtime) == false)
                {
                    continue;
                }

                if (runtime.Container.TryGetCapability(out IStatusModifyOutgoingHealingCapability effect))
                {
                    HealingModification modification = effect.GetModification(instance);
                    result = new(result.BaseValue + modification.BaseValue,
                        result.PercentModication + modification.PercentModication,
                        result.BonusValue + modification.BonusValue,
                        result.FlagsModification | modification.FlagsModification);
                }
            }

            return result;
        }
    }
}
