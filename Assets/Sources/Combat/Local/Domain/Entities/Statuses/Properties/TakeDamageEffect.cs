using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities.Statuses
{
    public interface ITakeDamageEffectStrategy
    {
        void HandleDamage(DamageResult @event);
    }
}
