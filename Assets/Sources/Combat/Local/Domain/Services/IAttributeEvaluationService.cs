using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Services
{
    public interface IAttributeEvaluationService
    {
        float GetAttributeValue(EntityId entityId, Attribute attribute);
        float GetMaxHealthBonus(EntityId id);
        float GetVersalityModifier(EntityId id);
        float GetHasteModifier(EntityId id);
    }
}
