using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine;
using Core.Combat.Units.Interfaces;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
using System;
using System.Runtime.CompilerServices;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Combat.Units.Components
{
    public sealed class UnitState : TeamOwner
    {
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
            _health.MaxValue = StatsEvaluator.EvaluateUnitStatValue(UnitStat.MAX_HEALTH, Engine.Units.GetUnitById(EntityId));
            _resource.Update(GetResourceIncrease(_resource.LeftType, ModelUpdate.UpdateTime), GetResourceIncrease(_resource.RightType, ModelUpdate.UpdateTime));
        }

        //TODO: move to passive ability???
        private float GetResourceIncrease(ResourceType type, long time) => type switch
        {
            ResourceType.MANA => time * Engine.Combat.GetManaRestoreRate() / 1000,
            //ResourceType.ENERGY => time * EvaluateHasteTimeDivider() * Engine.Combat.GetEnergyRechargeRate() / 1000,
            ResourceType.CONCENTRATION => time * Engine.Combat.GetConcentrationRestoreRate() / 1000,
            _ => 0,
        };

        #region _health
        public float CurrentHealth => _health.Value;

        public bool Alive => _alive;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HealthModificationRecord Heal(HealthChangeEvent @event)
        {
            float healing = @event.Evaluate();
            _health += healing;
            return new(new(@event.Caster, @event.Target, @event.Source), CurrentHealth, healing);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HealthModificationRecord TakeDamage(HealthChangeEvent @event)
        {
            float damage = @event.Evaluate();
            _health -= damage;
            return new(new(@event.Caster, @event.Target, @event.Source), CurrentHealth, -damage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyOutcomeHealingModification()
        {

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
        #endregion

        #region _resource
        public bool CanPay(AbilityCost cost) => _resource.CanPay(cost);
        public bool HasResource(ResourceType type) => _resource.HasResource(type);
        public float GetResourceValue(ResourceType type) => _resource.GetResourceValue(type);
        public float GiveResource(ResourceType type, float value) => _resource.GiveResource(type, value);
        public void SpendResource(AbilityCost cost) => _resource.SpendResource(cost);
        #endregion

        #region _stats
        public PercentModifiedValue GetStat(UnitStat stat) => _stats[stat];

        public PercentModifiedValue this[UnitStat stat]
        {
            get => _stats[stat];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal set
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
