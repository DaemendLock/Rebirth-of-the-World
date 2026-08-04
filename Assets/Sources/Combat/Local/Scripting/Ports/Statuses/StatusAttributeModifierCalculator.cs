using Combat.API.API.IDK;
using Combat.API.DTO;
using Combat.API.Statuses;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Scripting.Ports.Statuses
{
    public sealed class StatusAttributeModifierCalculator : IStatusAttributeCalculator
    {
        private readonly IStatusRuntimeRegistry _runtimeStatusRepository;

        public StatusAttributeModifierCalculator(IStatusRuntimeRegistry statusRuntimeRegistry)
        {
            _runtimeStatusRepository = statusRuntimeRegistry;
        }

        public AttributesModification Evaluate(ReadOnlySpan<StatusId> values)
        {
            AttributesModification result = new()
            {
                TimeScale = 100f
            };

            foreach (StatusId statusId in values)
            {
                if (_runtimeStatusRepository.TryGet(statusId, out var properties) == false)
                {
                    continue;
                }

                if (properties.TryGetProperty(out IAttributesModifier attributeEffect))
                {
                    AttributesModification modification = GetModification(attributeEffect);
                    result += modification;
                }

                if (properties.TryGetProperty(out ITimeScaleModifier timeScaleEffect))
                {
                    result.TimeScale += timeScaleEffect.GetTimeModification();
                }
            }

            return result;
        }

        private AttributesModification GetModification(IAttributesModifier modifier)
        {
            AttributesModification result = new();

            System.Span<AttributeValue> baseValues = stackalloc AttributeValue[AttributesOwner.AttributeCount];
            System.Span<AttributeValue> bonusValues = stackalloc AttributeValue[baseValues.Length];

            baseValues.Clear();
            bonusValues.Clear();

            AttributesData attributesData = new(baseValues, bonusValues);
            modifier.GetAttributesBonuses(attributesData);

            result.Attack = new(bonusValues[(int)Common.ValueObjects.Attribute.Atk]);
            result.Spellpower = new(bonusValues[(int)Common.ValueObjects.Attribute.Spellpower]);
            result.Speed = new(bonusValues[(int)Common.ValueObjects.Attribute.Speed]);

            return result;
        }
    }
}
