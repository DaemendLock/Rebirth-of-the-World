using Combat.Common.Primitives;
using Combat.Local.Application.Queries.Statuses;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Capabilities.Statuses;
using Combat.Local.Scripting.Runtime;

using System;

namespace Combat.Local.Scripting.Ports.Statuses
{
    public sealed class PropertyAttributesModificationCalculator : IAttributesModificationCalculator
    {
        private readonly IStatusRuntimeRegistry _statusRuntimeRegistry;

        public PropertyAttributesModificationCalculator(IStatusRuntimeRegistry statusRuntimeRegistry)
        {
            _statusRuntimeRegistry = statusRuntimeRegistry;
        }

        public AttributesModification Evaluate(ReadOnlySpan<StatusId> values)
        {
            AttributesModification result = new();

            foreach (StatusId statusId in values)
            {
                if (_statusRuntimeRegistry.TryGet(statusId, out StatusRuntime runtime) == false)
                {
                    continue;
                }

                IStatusModifyAttributesCapability attributeEffect = runtime.Container.GetCapability<IStatusModifyAttributesCapability>();

                if (attributeEffect != null)
                {
                    AttributesModification modification = attributeEffect.GetModification();
                    result.Attack += modification.Attack;
                    result.Spellpower += modification.Spellpower;
                    result.Speed += modification.Speed;
                }

                IStatusModifyTimeScaleCapability timeScaleEffect = runtime.Container.GetCapability<IStatusModifyTimeScaleCapability>();

                if (timeScaleEffect != null)
                {
                    result.TimeScale += timeScaleEffect.GetModification();
                }
            }

            return result;
        }
    }
}
