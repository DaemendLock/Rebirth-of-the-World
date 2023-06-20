using Remaster.AuraEffects;
using Remaster.Events;
using Remaster.Stats;
using Remaster.Utils;
using System.IO;

namespace Remaster.Interfaces
{
    public interface IUnitAssignable
    {
        public bool TryAssignTo(Unit unit);
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

        CommandResult CastAbility(EventData data);

        void Interrupt(InterruptData data);

        void OverrideAbility(Spell repalce, Spell with);
    }

    public interface AuraOwner
    {
        void AddStatus(EventData data);

        void ApplyAura(EventData data, AuraEffect effect);

        Status FindStatus(Spell spell, Unit caster);

        bool HasStatus(Spell spell, Unit caster);

        void Dispell(DispellType dispellType);

        void Purge(DispellType dispellType);
    }

    public interface DynamicStatOwner
    {
        public PercentModifiedValue EvaluateDynamicStat(UnitStat stat);
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
        public Team Team { get; }

        public bool CanHelp(TeamOwner teamOwner);

        public bool CanHurt(TeamOwner teamOwner);
    }

    public interface SerializableInterface
    {
        public void Serialize(BinaryWriter buffer);
    }
}
