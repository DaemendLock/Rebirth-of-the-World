using System.Collections.Generic;

using Server.Combat.Domain.Attributes;
using Server.Combat.Domain.DTO;
using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Events;
using Server.Combat.Domain.Implementations.Attributes;
using Server.Combat.Domain.Repositories;
using Server.Combat.Domain.Services;
using Server.Combat.Domain.Units.ValueObjects;
using Server.Combat.Infrastructure.Repositories;

namespace Server.Combat.Domain.Units.Services.Implementations
{
    public class AttributesEvaluationService : IAttributeEvaluationService
    {
        private readonly IStatusRepository _effectRepository;
        private readonly IAttributesRepository _attributesRepository;

        private readonly Dictionary<EntityId, CachedAttributes> _cachedValues;

        private byte _version;

        public AttributesEvaluationService(IStatusRepository effectRepository, IAttributesRepository attributesRepository)
        {
            _effectRepository = effectRepository;
            _attributesRepository = attributesRepository;

            _cachedValues = new();
            _version = 0;
        }

        public float GetAttributeValue(EntityId id, Attribute attribute)
        {
            if (_cachedValues.TryGetValue(id, out CachedAttributes cachedValue) == false)
            {
                StatsTable memory = new(_attributesRepository.Get(id));
                cachedValue = new(memory, _version);
            }

            if (cachedValue.Version != _version)
            {
                cachedValue.Values.Clear();
                cachedValue.Values.Add(_attributesRepository.Get(id));
                _cachedValues.Add(id, new(cachedValue.Values, _version));

                IEnumerable<StatusEffect> effects = _effectRepository.FindStatusEffects(id);

                foreach (StatusEffect effect in effects)
                {
                    effect.ModifyAttributes(cachedValue.Values);
                }
            }

            return cachedValue.Values[attribute].CalculatedValue;
        }

        public void ClearCache()
        {
            _version++;
        }

        private readonly struct CachedAttributes
        {
            public CachedAttributes(StatsTable values, int version)
            {
                Values = values;
                Version = version;
            }

            public StatsTable Values { get; }
            public int Version { get; }
        }
    }

    public class HealDamageApplicationService : IHealDamageApplicationService
    {
        private readonly IStatusRepository _statusRepository;

        public void ApplyDamage(Unit target, DamageData data)
        {
            DamageEvent @event = new(data.Attacker, target, data.Source, data.Damage, data.Flags);

            IEnumerable<StatusEffect> attackerEffects = _statusRepository.FindStatusEffects(@event.Attacker.Id);
            IEnumerable<StatusEffect> defenderEffects = _statusRepository.FindStatusEffects(@event.Victim.Id);

            foreach (StatusEffect effect in attackerEffects)
            {
                effect.ModifyDamageDealth(@event);
            }

            foreach (StatusEffect effect in defenderEffects)
            {
                effect.ModifyDamageRecive(@event);
            }

            if (@event.InProgress == false)
            {
                return;
            }

            float health = target.CurrentHealth;
            float damage = @event.GetCurrentDamage();

            if (damage > health && @event.Flags.HasFlag(DamageFlags.NonLethal))
            {
                health = 1;
            }
            else
            {
                health -= damage;
            }

            DamageInstance instance = new(@event.Attacker, @event.Victim, damage, @event.Flags);

            if (data.Flags.HasFlag(DamageFlags.NonReactable) == false)
            {
                foreach (StatusEffect effect in defenderEffects)
                {
                    effect.OnTakeDamage(instance);
                }

                foreach (StatusEffect effect in attackerEffects)
                {
                    effect.OnTakeDamage(instance);
                }
            }

            if (health < 0 && !target.Alive)
            {
                target.Kill(new(data.Attacker, data.Source, KillFlags.None));
            }
        }

        public void ApplyHealing(Unit target, HealingData data)
        {
            HealingEvent @event = new(data.Healer, target, data.Source, data.Healing, data.Flags);

            IEnumerable<StatusEffect> healerEffects = _statusRepository.FindStatusEffects(@event.Healer.Id);
            IEnumerable<StatusEffect> healeeEffects = _statusRepository.FindStatusEffects(@event.Healee.Id);

        }
    }

    public class KillReviveService : IKillReviveService
    {
        public void Kill(Unit target, KillData data)
        {
            target.Alive = false;
        }

        public void Revive(Unit target, ReviveData data)
        {
            if (target.CurrentHealth < 0)
            {
                target.CurrentHealth = 1;
            }

            target.Alive = true;
        }
    }
}
