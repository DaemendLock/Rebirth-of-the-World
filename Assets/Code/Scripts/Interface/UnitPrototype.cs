using Combat.SpellOld;
using Combat.Status;
using System;
using System.Collections.Generic;

namespace UnitOperations {
    public interface IHealable {
        void Heal(HealEventInstance e);
        float GetHealRecive();
        void OnHealRecived(HealEventInstance e);
    }

    public interface IDamagable {
        void Damage(AttackEventInstance e);
        float GetDamageReceive(AttackEventInstance e);
        void OnDamage(AttackEventInstance e);
    }

    public interface IKillable {
        void Kill();
        void Respawn();
        void OnDeath();
    }

    public interface IHealthOwner : IDamagable, IHealable, IKillable {
        void SetHealth(float health, Unit inflictor = null, OldAbility ability = null);
    }

    public interface IManaOwner {
        void SetResource(float ammount, int resourceType);
        void SpendMana(float ammount, int resource);
    }

    public interface ICaster {

        bool Casting { get; }

        //bool Channeling { get; }

        float CastTimeRemain { get; }

        bool Channeling { get; }

        OldAbility CurrentCastAbility { get; }

        OldAbility FindAbilityByName(string abilityName);

        OldAbility GetAbilityByIndex(int index);

        void CastAbility(OldAbility ability);

        void Interrupt(bool succes = false);
    }

    public interface IStatusOwner {
        Status AddNewStatus(Unit caster, OldAbility ability, String name, Dictionary<String, float> data);

        List<Status> GetAllStatuses();

        void ForceRemove(Status s);

        void RemoveStatusByName(string name);

        void RemoveStatusByNameAndCaster(string name, Unit caster);

        void RemoveAllStatusesOfName(string name);
    }
}

namespace Events {
    public struct HealthChangeEvent {
        public Unit unit { get; }
        public float delta { get; }
        public HealthChangeEvent(Unit unit, float delta) {
            this.unit = unit;
            this.delta = delta;
        }
    }

    public struct StartCastEvent {
        public Unit unit { get; }
        public OldAbility ability { get; }
        public StartCastEvent(Unit unit, OldAbility ability) {
            this.unit = unit;
            this.ability = ability;
        }
    }

    public struct UpdateCastEvent {
        public Unit unit { get; }
        public UpdateCastEvent(Unit unit) {
            this.unit = unit;
        }
    }

    public struct EndCastEvent {
        public Unit unit { get; }
        public bool succes { get; }
        public EndCastEvent(Unit unit, bool succes) {
            this.unit = unit;
            this.succes = succes;
        }
    }
}
