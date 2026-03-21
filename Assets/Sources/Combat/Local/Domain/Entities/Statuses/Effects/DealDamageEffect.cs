using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Local.Domain.Entities.Statuses.Effects
{
    public interface IDealDamageEffectStrategy
    {
        void HandleDamage(DamageResult @event);
    }

    public readonly ref struct DealDamageEffect
    {
        public readonly IDealDamageEffectStrategy _strategy;

        public DealDamageEffect(StatusId status, IDealDamageEffectStrategy strategy)
        {
            Status = status;
            _strategy = strategy;
        }

        public StatusId Status { get; }

        public void Handle(DamageResult @event) => _strategy.HandleDamage(@event);
    }
}
