using Core.Combat.Abilities;
using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine;
using Core.Combat.Interfaces;
using Core.Combat.Statuses;
using Core.Combat.Statuses.Auras;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
using System;
using System.Runtime.CompilerServices;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Combat.Units.Components
{
    public class UnitState : TeamOwner, StatsOwner
    {
        private const float CritEffiency = 0.05f;
        private const float VersalityEffiency = 0.2f;

        private readonly AuraOwner _auras;
        private readonly CastResources _resource;
        private readonly StatsTable _stats;

        private Resource _health;
        private bool _alive;
        private Team.Team _team;

        private UnitState()
        {
            throw new InvalidOperationException();
        }

        public UnitState(StatsTable baseStats, CastResources resources, Team.Team team, int id)
        {
            _auras = new AuraOwner();
            _alive = true;

            _stats = baseStats;
            _resource = resources;
            _team = team;
            _health = new Resource((int) baseStats[UnitStat.MAX_HEALTH].CalculatedValue);
            _health.Value = _health.MaxValue;

            EntityId = id;
        }

        public int EntityId { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(IActionRecordContainer container)
        {
            _auras.Update(container);
            _resource.Update(GetResourceIncrease(_resource.LeftType, ModelUpdate.UpdateTime), GetResourceIncrease(_resource.RightType, ModelUpdate.UpdateTime));
        }

        //TODO: move to passive ability???
        private float GetResourceIncrease(ResourceType type, long time) => type switch
        {
            ResourceType.MANA => time * Engine.Combat.GetManaRestoreRate() / 1000,
            ResourceType.ENERGY => time * EvaluateHasteTimeDivider() * Engine.Combat.GetEnergyRechargeRate() / 1000,
            ResourceType.CONCENTRATION => time * Engine.Combat.GetConcentrationRestoreRate() / 1000,
            _ => 0,
        };

        #region _health
        public float CurrentHealth => _health.Value;

        public bool Alive => _alive;

        public void ApplyHealingEvent(HealthChangeEventData data)
        {
            float healing = data.Value * (1 + EvaluateStat(UnitStat.HEALING_TAKEN).CalculatedValue);

            _health += healing;

            //TODO Logger.Log($"{this} healed by {healing}({data.Value}).");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TakeDamage(DamageEvent @event)
        {
            if (_alive == false)
            {
                return;
            }

            //@event.Evade(EntityId);
            //@event.Parry(EntityId);

            if ((@event.Result == HealthChangeFailType.EVADE) || (@event.Result == HealthChangeFailType.PARRY))
            {
                return;
            }

            //@event.Block(EntityId);

            @event.ApplyVerasilityDivider(EvaluateVersalityMultiplier(), EntityId);

            @event.ApplyIncomeReduction(EvaluateStat(UnitStat.DAMAGE_TAKEN), EntityId);

            //_auras.TakeDamage(@event, target);
            _auras.AbsorpDamage(@event);

            _health -= @event.EvaluateDamage();
            //TODO Logger.Log($"{data.Target} damaged by {damage}({data.Value}).");

            if (_health == _health.MinValue)
            {
                Kill(null);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AmplifyDamage(DamageEvent @event)
        {
            @event.ApplyVerasilityMultiplier(EvaluateVersalityMultiplier(), EntityId);
            @event.ApplyOutcomeAmplification(EvaluateStat(UnitStat.DAMAGE_DONE), EntityId);
        }

        public void Kill(KillEventData data)
        {
            _alive = false;
        }

        public void Resurect(ResurrectionData data)
        {
            _health.Value = data.Health;
            _resource.GiveResource(ResourceType.MANA, data.Mana);
            _alive = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AbsorbDamage(CastInputData data, float absorption, SchoolType school) => _auras.AbsorbDamage(data, absorption, school);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetAbsorption() => _auras.GetAbsorption();
        #endregion

        #region _resource
        public bool CanPay(AbilityCost cost) => _resource.CanPay(cost);
        public bool HasResource(ResourceType type) => _resource.HasResource(type);
        public float GetResourceValue(ResourceType type) => _resource.GetResourceValue(type);
        public float GiveResource(ResourceType type, float value) => _resource.GiveResource(type, value);
        public void SpendResource(AbilityCost cost) => _resource.SpendResource(cost);
        #endregion

        #region _stats
        public float EvaluateHasteTimeDivider() => StatBonusEvaluator.EvaluateHasteTimeDivider(EvaluateStat(UnitStat.HASTE).CalculatedValue);

        public float EvaluateCritChance() => EvaluateStat(UnitStat.CRIT).CalculatedValue * CritEffiency;

        public float EvaluateVersalityMultiplier() => 1 + Math.Max(0, EvaluateStat(UnitStat.VERSALITY).CalculatedValue * VersalityEffiency * 0.01f);

        public int EvaluateStatValue(UnitStat stat) => (int) EvaluateStat(stat).CalculatedValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PercentModifiedValue EvaluateStat(UnitStat stat) => _auras.EvaluateStat(stat) + _stats[stat];

        public PercentModifiedValue this[UnitStat stat]
        {
            get => _stats[stat];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _stats[stat] = value;

                if (stat == UnitStat.MAX_HEALTH)
                {
                    _health.MaxValue = value.CalculatedValue;
                }
            }
        }
        #endregion

        #region _team
        public Team.Team Team => _team;

        public bool CanHelp(TeamOwner teamOwner)
        {
            return teamOwner != null && _team == teamOwner.Team;
        }

        public bool CanHurt(TeamOwner teamOwner)
        {
            return teamOwner != null && _team != teamOwner.Team;
        }
        #endregion

        #region _aura
        public void ApplyAura(CastInputData data, Aura aura) => _auras.ApplyAura(data, aura);

        public void RemoveStatus(Aura aura) => _auras.RemoveStatus(aura);

        public void RemoveStatus(SpellId spell) => _auras.RemoveStatus(spell);

        public Status FindStatus(Aura aura) => _auras.FindStatus(aura);

        public bool HasStatus(Aura aura) => _auras.HasStatus(aura);

        public bool HasStatus(SpellId spell) => _auras.HasStatus(spell);

        public void Dispell(DispellType dispellType) => _auras.Dispell(dispellType);

        public void Purge(DispellType dispellType) => _auras.Dispell(dispellType);

        public bool HasImmunity(Mechanic mechanic) => _auras.ImmunityValue(mechanic) > 0;
        #endregion

        public void InformAction(UnitAction action, CastInputData data)
        {
            _auras.InformAction(action, data);
        }

        public override string ToString()
        {
            return EntityId.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal (Resource, Resource) GetResources() => (_resource.Left, _resource.Right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal (ResourceType left, ResourceType right) GetResourceTypes() => (_resource.LeftType, _resource.RightType);
    }
}
