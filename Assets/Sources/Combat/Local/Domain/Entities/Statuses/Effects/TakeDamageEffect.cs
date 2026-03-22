using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities.Statuses
{
    public interface ITakeDamageEffectStrategy
    {
        void HandleDamage(DamageResult @event);
    }

    public readonly ref struct TakeDamageEffect
    {
        public readonly ITakeDamageEffectStrategy _strategy;

        public TakeDamageEffect(StatusId status, ITakeDamageEffectStrategy strategy)
        {
            Status = status;
            _strategy = strategy;
        }

        public StatusId Status { get; }

        public void Handle(DamageResult @event) => _strategy.HandleDamage(@event);
    }
}
