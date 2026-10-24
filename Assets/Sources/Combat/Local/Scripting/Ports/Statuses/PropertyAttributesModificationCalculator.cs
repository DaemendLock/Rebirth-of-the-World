using Combat.Common.Primitives;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Queries.Statuses;
using Combat.Local.Scripting.Capabilities.Statuses;
using Combat.Local.Scripting.Runtime;

using System.Collections.Generic;

namespace Combat.Local.Scripting.Ports.Statuses
{
    public sealed class PropertyAttributesModificationCalculator : IAttributesModificationCalculator
    {
        private readonly IStatusRuntimeRegistry _statusRuntimeRegistry;

        public PropertyAttributesModificationCalculator(IStatusRuntimeRegistry statusRuntimeRegistry)
        {
            _statusRuntimeRegistry = statusRuntimeRegistry;
        }

        public AttributesBonuses Evaluate(IEnumerable<StatusId> values)
        {
            AttributesBonuses result = new();

            foreach (StatusId statusId in values)
            {
                if (_statusRuntimeRegistry.TryGet(statusId, out StatusRuntime runtime) == false)
                {
                    continue;
                }

                IStatusModifyAttributesCapability attributeEffect = runtime.Container.GetCapability<IStatusModifyAttributesCapability>();

                if (attributeEffect != null)
                {
                    AttributesBonuses modification = attributeEffect.GetModification();
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
