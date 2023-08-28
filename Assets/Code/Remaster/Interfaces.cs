﻿using System.IO;

namespace Model.Interfaces
{
    public interface UnitAssignable
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

        CommandResult CastSpell(EventData data);

        void Interrupt(InterruptData data);

        void OverrideAbility(Spell repalce, Spell with);
    }

    public interface AuraOwner
    {
        void ApplyAura(EventData data, AuraEffect effect);

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
        public Team Team { get; }

        public bool CanHelp(TeamOwner teamOwner);

        public bool CanHurt(TeamOwner teamOwner);
    }

    public interface SerializableInterface
    {
        public void Serialize(BinaryWriter buffer);
    }
}
