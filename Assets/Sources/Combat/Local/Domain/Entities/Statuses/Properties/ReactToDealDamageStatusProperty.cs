using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities.Statuses
{
    public interface IDealDamageEffectStrategy
    {
        void HandleDamage(DamageResult @event);
    }
}
