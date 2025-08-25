using Server.Combat.Domain.Attributes;
using Server.Combat.Domain.DTO;
using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Units.ValueObjects;

namespace Server.Combat.Domain.Services
{
    public interface IStatusUpdateService
    {

    }

    public interface IAttributeEvaluationService
    {
        float GetAttributeValue(EntityId id, Attribute attribute);

        void ClearCache();
    }

    public interface IHealDamageApplicationService
    {
        void ApplyDamage(Unit target, DamageData data);

        void ApplyHealing(Unit target, HealingData data);
    }
}
