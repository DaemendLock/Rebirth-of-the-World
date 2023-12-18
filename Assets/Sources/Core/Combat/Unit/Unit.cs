using Core.Combat.Abilities;
using Core.Combat.Auras;
using Core.Combat.Auras.AuraEffects;
using Core.Combat.Engine;
using Core.Combat.Interfaces;
using Core.Combat.Units.Components;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
using System;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Combat.Units
{
    public sealed class Unit : AbilityOwner, Interfaces.AuraOwner, CastResourceOwner, TeamOwner, DynamicStatOwner, Updatable, Damageable, Damager
    {
        public event UnitState.HealthChanged OnHealthChanged;
        public event UnitState.Damaged OnDamaged;
        public event UnitState.StatusApplied OnStatusApplied;

        public event Spellcasting.Casted Casted;
        public event Spellcasting.Attacked Attacked;
        public event Spellcasting.StartedPrecast StartedPrecast;
        public event Spellcasting.StoppedPrecast StoppedCast;

        internal Unit Target;

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

        public int Id => _unitState.EntityId;

        #region Status
        public Status FindStatus(Spell spell) => _unitState.FindStatus(spell);

        public bool HasStatus(Spell spell) => _unitState.HasStatus(spell);

        public bool HasStatus(SpellId spell) => _unitState.HasStatus(spell);

        public bool HasImmunity(Mechanic mechanic) => _unitState.HasImmunity(mechanic);
        #endregion

        #region Abilities
        public float GCD => _casting.GCD;
        public float CastTime => _casting.CastTime;
        public float FullCastTime => _casting.FullCastTime;

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
        public CommandResult CastAbility(CastEventData data)
        {
            return _casting.CastAbility(data);
            /*Casted?.Invoke(data.Spell, result);
            return result;*/
        }
        #endregion

        #region UnitState
        public bool Alive => _unitState.Alive;

        public float CurrentHealth => _unitState.CurrentHealth;

        public float MaxHealth => _unitState[UnitStat.MAX_HEALTH].CalculatedValue;

        public Vector3 Position
        {
            get => _unitState.Position;
            internal set => _unitState.Position = value;
        }

        public Vector3 MoveDirection
        {
            get => _unitState.MoveDirection;
            internal set
            {
                _unitState.MoveDirection = value;

                if (value != Vector3.zero && _casting.CastingSpell != null && !_casting.CastingSpell.Flags.HasFlag(SpellFlags.CAN_CAST_WHILE_MOVING))
                {
                    Interrupt(new InterruptData(true, new SpellId(), 0));
                }
            }
        }

        public float Rotation
        {
            get => _unitState.Rotation;
            internal set => _unitState.Rotation = value;
        }

        public bool IsMoving => _unitState.IsMoving;
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

        public (Resource, Resource) GetResources() => _unitState.GetResources();

        public (ResourceType left, ResourceType right) GetResourceTypes() => _unitState.GetResourceTypes();

        public Team.Team Team => _unitState.Team;

        public bool CanHurt(TeamOwner teamOwner) => _unitState.CanHurt(teamOwner);

        public bool CanHelp(TeamOwner teamOwner) => _unitState.CanHelp(teamOwner);

        public float GetStat(UnitStat stat) => _unitState[stat].CalculatedValue;

        public float GetAbsorption() => _unitState.GetAbsorption();

        public void ModifyStat(UnitStat stat, PercentModifiedValue value) => _unitState[stat] += value;

        public PercentModifiedValue EvaluateStat(UnitStat stat) => _unitState.EvaluateStat(stat);

        public float this[UnitStat stat] => _unitState.EvaluateStat(stat).CalculatedValue;

        public float EvaluateHasteTimeDivider() => _unitState.EvaluateHasteTimeDivider();
        public float EvaluateVersalityMultiplyer() => _unitState.EvaluateVersalityMultiplier();
        public float EvaluateCritChance() => _unitState.EvaluateCritChance();

        public void TakeDamage(DamageEvent @event) => _unitState.TakeDamage(@event);

        public void AmplifyDamage(DamageEvent @event) => _unitState.AmplifyDamage(@event);
        #endregion

        #region internal
        internal void ApplyAura(CastEventData data, AuraEffect effect) => _unitState.ApplyAura(data, effect);

        internal void RemoveStatus(Spell spell) => _unitState.RemoveStatus(spell);

        internal void RemoveStatus(SpellId spell) => _unitState.RemoveStatus(spell);

        internal void Dispell(DispellType dispellType) => _unitState.Dispell(dispellType);

        internal void Purge(DispellType dispellType) => _unitState.Purge(dispellType);

        /// <summary>
        /// Create new <see cref="Ability"/> based on given <paramref name="spell"/> or overrides one already exists.
        /// </summary>
        internal bool GiveAbility(Spell spell) => _casting.GiveAbility(spell);

        /// <summary>
        /// Remove <see cref="Ability"/> driven by given <paramref name="spell"/>.
        /// </summary>
        internal bool RemoveAbility(Spell spell) => _casting.RemoveAbility(spell);

        /// <summary>
        /// Find first <see cref="Ability"/> driven by given spell.
        /// </summary>
        /// 
        /// <returns>
        /// OR <see cref="Ability"/> driven by spell<br/>
        /// OR it's replacement<br/>
        /// OR null if spell not found.
        /// </returns>
        internal Ability FindAbility(Spell spell) => _casting.FindAbility(spell); internal CommandResult CastSpell(CastEventData data) => _casting.CastSpell(data);

        internal void Attack(Spell attack)
        {
            CommandResult result = CastSpell(new CastEventData(this, Target, attack));

            if (result != CommandResult.SUCCES)
            {
                return;
            }

            _unitState.InformAction(UnitAction.AUTOATTACK, new CastEventData(this, Target, attack));
            Attacked?.Invoke(attack, Target);
        }

        /// <summary>
        /// Execute attempt to interrupt current cast or channaling.
        /// </summary>
        /// 
        /// <param name="data">
        /// Interrupt source data.
        /// </param>
        internal void Interrupt(InterruptData data)
        {
            _casting.Interrupt(data);

            if (_casting.FullCastTime == float.PositiveInfinity)
            { StoppedCast?.Invoke(); }
        }

        internal void OverrideAbility(Spell repalce, Spell with) => _casting.OverrideAbility(repalce, with);

        internal void ModifySpellEffect(SpellId spellId, int effect, float modifyValue) => _casting.ModifySpellEffect(spellId, effect, modifyValue);

        internal void GiveResource(ResourceType type, float value) => _unitState.GiveResource(type, value);

        internal void SpendResource(AbilityCost cost) => _unitState.SpendResource(cost);

        internal void Heal(HealthChangeEventData data) => _unitState.ApplyHealingEvent(data);

        internal void AbsorbDamage(CastEventData data, float absorption, SchoolType school) => _unitState.AbsorbDamage(data, absorption, school);

        internal void InformCastingBehavior(Spell spell, float duration)
        {
            if (duration == float.PositiveInfinity || duration == 0)
            {
                StoppedCast?.Invoke();
                return;
            }

            StartedPrecast?.Invoke(spell, duration);
        }

        internal void InformCastingStopped()
        {
            StoppedCast?.Invoke();
        }
        #endregion
        public void Update()
        {
            _casting.Update();
            _unitState.Update();
        }

        public override string ToString()
        {
            return _unitState.ToString();
        }
    }
}
