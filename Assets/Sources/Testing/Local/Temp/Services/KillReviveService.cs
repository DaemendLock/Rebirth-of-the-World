using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services;

namespace Testing.Local.Temp.Services
{

    public class KillReviveService : IKillReviveService
    {
        private readonly IKillableRepository _killableRepository;
        private readonly IHealthRepository _healthRepository;

        public KillReviveService(IKillableRepository killableRepository, IHealthRepository healthRepository)
        {
            _killableRepository = killableRepository;
            _healthRepository = healthRepository;
        }

        public bool IsAlive(EntityId target) => _killableRepository.Get(target).Alive; 

        public void Kill(EntityId target)
        {
            Killable killable = _killableRepository.Get(target);

            if (killable.Alive == false)
            {
                return;
            }

            killable.Alive = false;
            _killableRepository.Update(killable);
        }

        public void Revive(EntityId target)
        {
            Killable unit = _killableRepository.Get(target);

            if (unit.Alive)
            {
                return;
            }

            Health health = _healthRepository.Get(target);

            if (health.CurrentHealth < 0)
            {
                health.CurrentHealth = 1;
            }

            _healthRepository.Update(health);

            unit.Alive = true;
            _killableRepository.Update(unit);
        }
    }
}
