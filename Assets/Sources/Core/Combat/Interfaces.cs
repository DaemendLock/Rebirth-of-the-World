using Core.Combat.Abilities;
using Core.Combat.Auras;
using Core.Combat.Auras.AuraEffects;
using Core.Combat.Stats;
using Core.Combat.Units;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
using Utils.DataTypes;

namespace Core.Combat.Interfaces
{
    public interface UnitAssignable
    {
        public void AssignTo(Unit unit);
    }

    public interface AbilityOwner
    {
        /// <summary>
        /// Create new <see cref="Ability"/> based on given <paramref name="spell"/>.
        /// </summary>
        /// <returns>True when added new ability;<br/>False when spell exists.</returns>
        bool GiveAbility(Spell spell);

        bool HasAbility(Spell spell);

        bool RemoveAbility(Spell spell);

        Ability FindAbility(Spell spell);

        Ability GetAbility(SpellSlot slot);

        CommandResult CastAbility(CastEventData data);

        CommandResult CastSpell(CastEventData data);

        void Interrupt(InterruptData data);

        void OverrideAbility(Spell repalce, Spell with);
    }

    public interface AuraOwner
    {
        void ApplyAura(CastEventData data, AuraEffect effect);

        Status FindStatus(Spell spell);

        bool HasStatus(Spell spell);

        void Dispell(DispellType dispellType);

        void Purge(DispellType dispellType);
    }

    public interface DynamicStatOwner
    {
        public PercentModifiedValue EvaluateStat(UnitStat stat);
    }

    public interface CastResourceOwner
    {
        public void SpendResource(AbilityCost value);

        public void GiveResource(ResourceType type, float value);

        public bool CanPay(AbilityCost value);

        public bool HasResource(ResourceType type);

        public float GetResourceValue(ResourceType type);
    }

    public interface TeamOwner
    {
        public Team.Team Team { get; }

        public bool CanHelp(TeamOwner teamOwner);

        public bool CanHurt(TeamOwner teamOwner);
    }

    public interface Damageable
    {
        public void TakeDamage(DamageEvent @event);
    }

    public interface Damager
    {
        public void AmplifyDamage(DamageEvent@event);
    }
}
