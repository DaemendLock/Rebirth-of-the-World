using Combat.Common.ValueObjects;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Capabilities.Statuses;
using Combat.Local.Scripting.Runtime;

using System;

namespace Combat.Local.Scripting.Ports.Statuses
{
    public sealed class StatusAttributeModifierCalculator : IStatusAttributeCalculator
    {
        private readonly IStatusRuntimeRegistry _statusRuntimeRegistry;

        public StatusAttributeModifierCalculator(IStatusRuntimeRegistry statusRuntimeRegistry)
        {
            _statusRuntimeRegistry = statusRuntimeRegistry;
        }

        public AttributesModification Evaluate(ReadOnlySpan<StatusId> values)
        {
            AttributesModification result = new()
            {
                TimeScale = 100f
            };

            foreach (StatusId statusId in values)
            {
                if (_statusRuntimeRegistry.TryGet(statusId, out var properties) == false)
                {
                    continue;
                }

                if (properties.TryGetProperty(out ModifyAttributesCapability attributeEffect))
                {
                    AttributesModification modification = attributeEffect.GetModification();
                    result.Attack += modification.Attack;
                    result.Spellpower += modification.Spellpower;
                    result.Speed += modification.Speed;
                }

                if (properties.TryGetProperty(out ModifyTimeScaleCapability timeScaleEffect))
                {
                    result.TimeScale += timeScaleEffect.GetModification();
                }
            }

            return result;
        }
    }
}
