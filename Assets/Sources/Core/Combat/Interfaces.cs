using Core.Combat.Abilities;
using Core.Combat.Auras;
using Core.Combat.Units;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
using Utils.DataStructure;
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
        bool HasAbility(Spell spell);

        Ability GetAbility(SpellSlot slot);

        public CommandResult CastAbility(CastEventData data);
    }

    public interface AuraOwner
    {
        Status FindStatus(Spell spell);

        bool HasStatus(Spell spell);
    }

    public interface DynamicStatOwner
    {
        public PercentModifiedValue EvaluateStat(UnitStat stat);
    }

    public interface CastResourceOwner
    {
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
        void TakeDamage(DamageEvent @event);
    }

    public interface Damager
    {
        public void AmplifyDamage(DamageEvent @event);
    }
}
