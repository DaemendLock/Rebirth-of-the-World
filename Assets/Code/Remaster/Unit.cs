using Remaster.AuraEffects;
using Remaster.Combat;
using Remaster.Events;
using Remaster.Interfaces;
using Remaster.Items;
using Remaster.Stats;
using Remaster.UnitComponents;
using Remaster.Utils;
using System;

namespace Remaster
{
    public enum ResourceType : ushort
    {
        NONE,
        MANA,
        CONCENTRATION,
        ENERGY,
        RAGE,
        LIGHT_POWER,
        OTHER
    }

    public sealed class Unit : AbilityOwner, Interfaces.AuraOwner, CastResourceOwner, TeamOwner, DynamicStatOwner, Updatable
    {
        private readonly Spellcasting _casting;
        private readonly UnitState _unitState;

        public Unit(Spellcasting spellcasting, UnitState defaultState)
        {
            if (spellcasting.TryAssignTo(this) == false)
            {
                throw new InvalidOperationException();
            }
            
            _casting = spellcasting;
            _unitState = defaultState;
        }

        #region Status
        public void AddStatus(EventData data) => _unitState.AddStatus(data);

        public void ApplyAura(EventData data, AuraEffect effect) => _unitState.ApplyAura(data, effect);

        public void RemoveStatus(Spell spell, Unit caster) => _unitState.RemoveStatus(spell, caster);

        public Status FindStatus(Spell spell, Unit caster) => _unitState.FindStatus(spell, caster);

        public bool HasStatus(Spell spell, Unit caster) => _unitState.HasStatus(spell, caster);

        public void Dispell(DispellType dispellType) => _unitState.Dispell(dispellType);

        public void Purge(DispellType dispellType) => _unitState.Dispell(dispellType);

        public bool HasImmunity(Mechanic mechanic) => _unitState.HasImmunity(mechanic);
        #endregion

        #region Abilities
        /// <summary>
        /// Create new <see cref="Ability"/> based on given <paramref name="spell"/> or overrides one already exists.
        /// </summary>
        public bool GiveAbility(Spell spell) => _casting.GiveAbility(spell);

        /// <summary>
        /// Remove <see cref="Ability"/> driven by given <paramref name="spell"/>.
        /// </summary>
        public bool RemoveAbility(Spell spell) => _casting.RemoveAbility(spell);

        /// <summary>
        /// Find first <see cref="Ability"/> driven by given spell.
        /// </summary>
        /// 
        /// <returns>
        /// OR <see cref="Ability"/> driven by spell<br/>
        /// OR it's replacement<br/>
        /// OR null if spell not found.
        /// </returns>
        public Ability FindAbility(Spell spell) => _casting.FindAbility(spell);

        /// <summary>
        /// Get ability in specified <paramref name="slot"/>.
        /// </summary>
        /// <returns>OR Ability placed in <paramref name="slot"/><br/>
        /// OR null if </returns>
        public Ability GetAbility(SpellSlot slot) => _casting.GetAbility(slot);

        public bool HasAbility(Spell spell) => _casting.HasAbility(spell);

        /// <summary>
        /// Start casting ability placed in given slot.
        /// </summary>
        public CommandResult CastAbility(EventData data) => _casting.CastAbility(data);
        

        /// <summary>
        /// Execute attempt to interrupt current cast or channaling.
        /// </summary>
        /// 
        /// <param name="data">
        /// Interrupt source data.
        /// </param>
        public void Interrupt(InterruptData data) => _casting.Interrupt(data);

        public void OverrideAbility(Spell repalce, Spell with) => _casting.OverrideAbility(repalce, with);

        public void ModifySpellEffect(int spellId, int effect, float modifyValue) => _casting.ModifySpellEffect(spellId, effect, modifyValue);
        #endregion

        #region UnitState

        public float CurrentHealth => _unitState.CurrentHealth;

        public void Heal(HealthChangeEventData data) => _unitState.ApplyHealingEvent(data);

        public void Damage(HealthChangeEventData data) => _unitState.ApplyDamageEvent(data);

        /*
        public void Kill(DeathData data);

        public void Respawn(ResurectionData data);
        */
        /// <summary>
        /// Determine wether <paramref name="cost"/> can be paid.
        /// </summary>
        public bool CanPay(AbilityCost cost) => _unitState.CanPay(cost);

        public bool HasResource(ResourceType type) => _unitState.HasResource(type);

        public float GetResourceValue(ResourceType type) => _unitState.GetResourceValue(type);

        public void GiveResource(ResourceType type, float value) => _unitState.GiveResource(type, value);

        public void SpendResource(AbilityCost cost) => _unitState.SpendResource(cost);

        public void Teleport(PositionComponent position)
        {
            throw new NotImplementedException();
        }

        public void StartMovement(PositionComponent position)
        {
            throw new NotImplementedException();
        }

        public void StopMovement()
        {
            throw new NotImplementedException();
        }

        public void Equip(CombatGear item)
        {
            if (_unitState.TryEquip(item))
            {
                GiveAbility(SpellLib.SpellLib.GetSpell(item.SpellId));
            }
        }

        public void Unequip(GearSlot slot)
        {
            CombatGear item = _unitState.Unequip(slot);

            if (item != null)
            {
                RemoveAbility(SpellLib.SpellLib.GetSpell(item.SpellId));
            }
        }

        public Team Team => _unitState.Team;

        public bool CanHurt(TeamOwner teamOwner) => _unitState.CanHurt(teamOwner);

        public bool CanHelp(TeamOwner teamOwner) => _unitState.CanHelp(teamOwner);

        public float GetStat(UnitStat stat) => _unitState[stat].CalculatedValue;

        public void ModifyStat(UnitStat stat, PercentModifiedValue value) => _unitState[stat] += value;

        public PercentModifiedValue EvaluateDynamicStat(UnitStat stat) => _unitState.EvaluateDynamicStat(stat);

        public float EvaluateHasteTimeDivider() => _unitState.EvaluateHasteTimeDivider();
        public float EvaluateVersalityMultiplyer() => _unitState.EvaluateVersalityMultiplier();
        public float EvaluateCritChance() => _unitState.EvaluateCritChance();
        #endregion

        #region events
        public void InformCast(EventData data, CommandResult result)
        {
            if (result != CommandResult.SUCCES)
            {
                return;
            }

            if (data.Spell.Flags.HasFlag(SpellFlags.AUTOATTACK))
            {
                _unitState.InformAction(UnitAction.AUTOATTACK, data);
                return;
            }
        }
        #endregion
    
        public void Update()
        {
            _casting.Update();
            _unitState.Update();
        }
    }
}
