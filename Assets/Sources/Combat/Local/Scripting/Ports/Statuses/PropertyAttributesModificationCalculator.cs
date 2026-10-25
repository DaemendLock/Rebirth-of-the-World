using Combat.Common.Primitives;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Queries.Statuses;
using Combat.Local.Application.ValueObjects;
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

        public StatusBonuses Evaluate(IEnumerable<StatusId> values)
        {
            AttributesBonuses attributes = new();
            float timeScale = 1.0f;
            AttributeModifcation movespeed = new();

            foreach (StatusId statusId in values)
            {
                if (_statusRuntimeRegistry.TryGet(statusId, out StatusRuntime runtime) == false)
                {
                    continue;
                }

                if (runtime.Container.TryGetCapability(out IStatusModifyAttributesCapability attributeEffect))
                {
                    AttributesBonuses modification = attributeEffect.GetModification();
                    attributes.Attack += modification.Attack;
                    attributes.Spellpower += modification.Spellpower;
                    attributes.Swiftness += modification.Swiftness;
                }

                if (runtime.Container.TryGetCapability(out IStatusModifyTimeScaleCapability timeScaleEffect))
                {
                    timeScale += timeScaleEffect.GetModification();
                }

                if (runtime.Container.TryGetCapability(out IStatusModifyMovespeedCapability movespeedEffect))
                {
                    movespeed += movespeedEffect.GetModification();
                }
            }

            return new(attributes, timeScale, movespeed);
        }
    }
}
