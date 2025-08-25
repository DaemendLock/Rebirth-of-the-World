using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.Services
{
    public sealed class HealthService
    {
        private readonly IHealthRepository _healthRepository;

        public HealthService(IHealthRepository healthRepository)
        {
            _healthRepository = healthRepository;
        }

        public Health GetHealth(EntityId id) => _healthRepository.Get(id);

        public void SetCurrentHealth(EntityId id, float value)
        {
            Health health = _healthRepository.Get(id);
            health.CurrentHealth = value;
            _healthRepository.Update(health);
        }

        public Health ApplyDamage(EntityId target, float damage, bool nonLethat)
        {
            Health health = _healthRepository.Get(target);

            health.CurrentHealth -= damage;

            if (health.CurrentHealth < 0 && nonLethat)
            {
                health.CurrentHealth = 1;
            }

            _healthRepository.Update(health);
            return health;
        }

        public Health ApplyHealing(EntityId target, float healing)
        {
            Health health = _healthRepository.Get(target);

            float maxHealth = health.MaxHealth;

            health.CurrentHealth += healing;

            if (health.CurrentHealth > maxHealth)
            {
                health.CurrentHealth = maxHealth;
            }

            _healthRepository.Update(health);
            return health;
        }

        //private class DamageInstanceFactory
        //{
        //    private readonly UnitApiRepository _unitApiRepository;
        //    private readonly IStatusLookupService _statusLookupService;

        //    public DamageInstance Create(EntityId targetId, float damage, EntityId? attackerId, ScriptedSkill source, DamageFlags flags)
        //    {
        //        Unit target = _unitApiRepository.Get(targetId);
        //        Unit attacker = attackerId.HasValue ? _unitApiRepository.Get(attackerId.Value) : null;

        //        DamageInstance instance = new(target, attacker, source, damage, flags);

        //        if (instance.Attacker != null)
        //        {
        //            IEnumerable<StatusApi> attackerEffects = _statusLookupService.FindStatusesOnUnit(attacker.Id);
        //            IOutgoingHealDamageModifier outgoingHealDamageModifier;

        //            foreach (StatusApi effect in attackerEffects)
        //            {
        //                if (effect.TryGetProperty(out outgoingHealDamageModifier) == false)
        //                {
        //                    continue;
        //                }

        //                instance.BaseDamage += outgoingHealDamageModifier.GetBonusDamageDealth(instance);
        //                instance.DamagePercent += outgoingHealDamageModifier.GetBonusDamageDealthPercent(instance);
        //                instance.Flags |= outgoingHealDamageModifier.GetDamageFlagMask(instance);
        //            }
        //        }

        //        IEnumerable<StatusApi> defenderEffects = _statusLookupService.FindStatusesOnUnit(target.Id);
        //        IIncomingHealDamageModifier incomingHalDamageModifier;

        //        foreach (StatusApi effect in defenderEffects)
        //        {
        //            if (effect.TryGetProperty(out incomingHalDamageModifier) == false)
        //            {
        //                continue;
        //            }

        //            instance.BaseDamage += incomingHalDamageModifier.GetBonusDamageRecived(instance);
        //            instance.DamagePercent += incomingHalDamageModifier.GetBonusDamageRecivedPercent(instance);
        //            instance.Flags |= incomingHalDamageModifier.GetDamageFlagMask(instance);
        //        }

        //        return instance;
        //    }
        //}
    }
}
