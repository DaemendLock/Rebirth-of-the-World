using Combat.Common.Flags;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases.Character
{
    public readonly struct ProcessDamageInstancesUseCase
    {
        private readonly IDamageInstanceQueue _queue;
        private readonly IDamageEventsQueue _events;

        private readonly IHealthRepository _healthRepository;
        private readonly IHealthOutput _healthOutput;

        public void Execute()
        {
            lock (_queue)
            {
                while (_queue.Count > 0)
                {
                    HandleNext();
                }
            }
        }

        private void HandleNext()
        {
            DamageInstance instance = _queue.Dequeue();

            Health health = _healthRepository.Get(instance.Target);
            float finalDamage = instance.Damage;

            if (finalDamage >= health.CurrentHealth && instance.Flags.HasFlag(DamageFlags.NonLethal))
            {
                finalDamage = health.CurrentHealth - 1;
            }

            health.TakeDamage(finalDamage);

            _healthRepository.Update(health);
            _healthOutput.Present(health);

            if (instance.Flags.HasFlag(DamageFlags.NonReactable) == false)
            {
                DamageResult result = new(instance.Target, instance.OriginalDamage, finalDamage, instance.Flags, instance.Attacker, instance.Source.Skill, instance.Source.Unit);
                _events.Enqueue(result);
            }
        }
    }
}
