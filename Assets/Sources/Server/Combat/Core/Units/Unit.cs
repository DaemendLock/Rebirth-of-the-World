using Core.Combat.Abilities;
using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine;
using Core.Combat.Statuses;
using Core.Combat.Statuses.Auras;
using Core.Combat.Units.Components;
using Core.Combat.Units.Interfaces;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
using Server.Combat.Core.Utils;
using System;
using System.Runtime.CompilerServices;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Combat.Units
{
    public sealed class Unit : TeamOwner, StatsOwner, Updatable, Entity
    {
        private readonly Spellcasting _casting;
        private readonly UnitState _unitState;
        private readonly PositionComponent _position;
        private readonly AuraOwner _auras;

        internal Unit(Spellcasting spellcasting, UnitState defaultState)
        {
            if (spellcasting.TryAssignTo(this) == false)
            {
                throw new InvalidOperationException();
            }

            _casting = spellcasting;
            _unitState = defaultState;
            _position = new PositionComponent(this);
            _auras = new AuraOwner();
        }

        internal Unit Target { get; set; }

        public int Id => _unitState.EntityId;

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

        internal PrecastActionRecord StartCast(Ability ability, Unit target, float time) => _casting.StartCast(ability, target, time);

        internal SpellValueProvider GetSpellValues(SpellId spell) => _casting.GetSpellValues(spell);
        #endregion

        #region UnitState
        public bool Alive => _unitState.Alive;

        public float CurrentHealth => _unitState.CurrentHealth;

        public float MaxHealth => StatsEvaluator.EvaluateUnitStat(UnitStat.MAX_HEALTH, this).CalculatedValue;

        public Vector3 Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Engine.Units.GetPosition(Id).Location;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal set
            {
                Position position = Engine.Units.GetPosition(Id);
                position = new(value, position.Rotation);
                Engine.Units.SetPosition(Id, position);
            }
        }

        public float Rotation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Engine.Units.GetPosition(Id).Rotation;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal set
            {
                Position position = Engine.Units.GetPosition(Id);
                position = new(position.Location, value);
                Engine.Units.SetPosition(Id, position);
            }
        }

        public Vector3 MoveDirection
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _position.MoveDirection;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal set
            {
                _position.MoveDirection = value;
            }
        }

        public bool IsMoving => _position.IsMoving;
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

        public (Resource left, Resource right) GetResources() => _unitState.GetResources();

        public (ResourceType left, ResourceType right) GetResourceTypes() => _unitState.GetResourceTypes();

        public Team.Team Team => _unitState.Team;

        public bool CanHurt(TeamOwner teamOwner) => _unitState.CanHurt(teamOwner);

        public bool CanHelp(TeamOwner teamOwner) => _unitState.CanHelp(teamOwner);

        public float GetStat(UnitStat stat) => _unitState[stat].CalculatedValue;

        public float GetAbsorption() => 0;

        internal void ModifyStat(UnitStat stat, PercentModifiedValue value) => _unitState[stat] += value;

        public int EvaluateStatValue(UnitStat stat) => (int) (_unitState[stat] + _auras.EvaluateStat(stat)).CalculatedValue;

        public PercentModifiedValue EvaluateStat(UnitStat stat) => _unitState[stat] + _auras.EvaluateStat(stat);

        internal HealthModificationRecord TakeDamage(HealthChangeEvent @event)
        {
            HealthModificationRecord result = _unitState.TakeDamage(@event);
            _auras.OnTakeDamage(result);
            @event.Caster._auras.OnDealDamage(result);
            return result;
        }

        internal void ApplyIncomeDamageModification(HealthChangeEvent @event)
        {
            @event.ApplyVerasilityModification(1 / StatsEvaluator.EvaluateVersalityMultiplier(this), Id);
            @event.ApplyIncomeModification(StatsEvaluator.EvaluateUnitStat(UnitStat.DAMAGE_TAKEN, this), Id);
            _auras.ApplyIncomeDamageModification(@event);
        }

        internal void ApplyIncomeHealingModification(HealthChangeEvent @event)
        {
            @event.ApplyIncomeModification(StatsEvaluator.EvaluateUnitStat(UnitStat.HEALING_TAKEN, this), Id);
        }

        internal void ApplyOutcomeDamageModification(HealthChangeEvent @event)
        {
            @event.ApplyVerasilityModification(StatsEvaluator.EvaluateVersalityMultiplier(this), Id);
            @event.ApplyOutcomeModifiction(StatsEvaluator.EvaluateUnitStat(UnitStat.DAMAGE_DONE, this), Id);
            _auras.AmplifyOutcomeDamage(@event);
        }

        internal void ApplyOutcomeHealingModification(HealthChangeEvent @event)
        {
            @event.ApplyVerasilityModification(StatsEvaluator.EvaluateVersalityMultiplier(this), Id);
            @event.ApplyOutcomeModifiction(StatsEvaluator.EvaluateUnitStat(UnitStat.HEALING_DONE, this), Id);
            _auras.AmplifyOutcomeHealing(@event);
        }
        #endregion

        #region Status
        internal void ApplyAura(CastInputData data, Aura aura, long duration) => _auras.ApplyAura(data, aura, duration);

        public void RemoveStatus(Aura aura) => _auras.RemoveStatus(aura);

        public void RemoveStatus(SpellId spell) => _auras.RemoveStatus(spell);

        public Status FindStatus(Aura aura) => _auras.FindStatus(aura);

        public bool HasStatus(Aura aura) => _auras.HasStatus(aura);

        public bool HasStatus(SpellId spell) => _auras.HasStatus(spell);
        #endregion

        #region internal
        /// <summary>
        /// Create new <see cref="Ability"/> based on given <paramref name="spellId"/> or overrides one already exists.
        /// </summary>
        internal bool GiveAbility(SpellId spellId) => _casting.GiveAbility(Spell.Get(spellId));

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
        internal Ability FindAbility(Spell spell) => _casting.FindAbility(spell);

        //internal CommandResult CastSpell(CastEventData data) => _casting.CastSpell(data);

        internal void Attack(Spell attack, Unit target)
        {
            //CommandResult result = CastSpell(new CastEventData(this, target, attack));

            //if (result != CommandResult.SUCCES)
            //{
            //    return;
            //}
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
            { }
        }

        internal void OverrideAbility(Spell repalce, Spell with) => _casting.OverrideAbility(repalce, with);

        internal void ModifySpellEffect(SpellId spellId, int effect, float modifyValue) => _casting.ModifySpellEffect(spellId, effect, modifyValue);

        internal void ModifyCooldown(SpellId id, PercentModifiedValue value) => _casting.ModifyCooldown(id, value);

        internal float GiveResource(ResourceType type, float value) => _unitState.GiveResource(type, value);

        internal void SpendResource(AbilityCost cost) => _unitState.SpendResource(cost);

        internal HealthModificationRecord Heal(HealthChangeEvent @event) => _unitState.Heal(@event);

        //internal void AbsorbDamage(CastEventData data, float absorption, SchoolType school) => _unitState.AbsorbDamage(data, absorption, school);
        #endregion

        public void Update(IActionRecordContainer container)
        {
            _casting.Update(container);
            _unitState.Update(container);
            _auras.Update(container);
        }

        public override string ToString()
        {
            return _unitState.ToString();
        }
    }
}
