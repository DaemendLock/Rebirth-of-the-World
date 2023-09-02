using Core.Combat.Abilities;
using Core.Combat.Auras;
using Core.Combat.Auras.AuraEffects;
using Core.Combat.Interfaces;
using Core.Combat.Items.Gear;
using Core.Combat.Stats;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
using System;
using Utils.DataTypes;

namespace Core.Combat.Units.Components
{
    public class UnitState : CastResourceOwner, TeamOwner, Core.Combat.Interfaces.AuraOwner, DynamicStatOwner
    {
        public delegate void HealthChanged(float currentHealth, float maxHealth);

        private const float HasteEffiency = 0.075f;
        private const float CritEffiency = 0.05f;
        private const float VersalityEffiency = 0.2f;

        private Resource _health;
        private bool _alive;

        private readonly CastResources _resource;
        private readonly StatsTable _stats;
        private Core.Team.Team _team;
        private readonly Equipment _equipment;
        private readonly PositionComponent _position;

        private readonly AuraOwner _auras;

        private UnitState()
        {
            throw new InvalidOperationException();
        }

        public UnitState(StatsTable baseStats, CastResources resources, Core.Team.Team team, int id = 0)
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

        public void Update()
        {
            //_resource.Update();
            //_position.Update();

            _auras.Update();
        }

        #region _health
        public float CurrentHealth => _health.Value;

        public void ApplyHealingEvent(HealthChangeEventData data)
        {
            float healing = data.Value * (1 + EvaluateStat(UnitStat.HEALING_TAKEN).CalculatedValue);

            _health += healing;

            //TODO Logger.Log($"{this} healed by {healing}({data.Value}).");
        }

        public void ApplyDamageEvent(HealthChangeEventData data)
        {
            if (_alive == false)
            {
                return;
            }

            float damage = data.Value * (1 + EvaluateStat(UnitStat.DAMAGE_TAKEN).CalculatedValue) / EvaluateVersalityMultiplier();
            _health -= damage;

            //TODO Logger.Log($"{data.Target} damaged by {damage}({data.Value}).");

            if (_health == _health.MinValue)
            {
                Kill(null);
            }
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

        private ProccesedHealthChangeEvent EvaluateDamageModification(HealthChangeEventData data)
        {
            //Get attack fail modification
            //Get dynamic versality mod
            //Get total reduction
            //Get absorbtion

            return default;
        }

        private ProccesedHealthChangeEvent EvaluateHealingModification(HealthChangeEventData data)
        {
            //Get total modification
            //Get absorbtion

            return default;
        }
        #endregion

        #region _resource
        public bool CanPay(AbilityCost cost) => _resource.CanPay(cost);
        public bool HasResource(ResourceType type) => _resource.HasResource(type);
        public float GetResourceValue(ResourceType type) => _resource.GetResourceValue(type);
        public void GiveResource(ResourceType type, float value) => _resource.GiveResource(type, value);
        public void SpendResource(AbilityCost cost) => _resource.SpendResource(cost);
        #endregion

        #region _stats

        public float EvaluateHasteTimeDivider() => (float) Math.Clamp(1 + EvaluateStat(UnitStat.HASTE).CalculatedValue * HasteEffiency * 0.01f, 0.5, 2);

        public float EvaluateCritChance() => EvaluateStat(UnitStat.CRIT).CalculatedValue * CritEffiency;

        public float EvaluateVersalityMultiplier() => 1 + Math.Max(0, EvaluateStat(UnitStat.VERSALITY).CalculatedValue * VersalityEffiency * 0.01f);

        public PercentModifiedValue EvaluateStat(UnitStat stat) => _auras.EvaluateStat(stat) + _stats[stat];

        public PercentModifiedValue this[UnitStat stat]
        {
            get => _stats[stat];
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
        public Core.Team.Team Team => _team;

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

        public void ApplyAura(EventData data, AuraEffect effect) => _auras.ApplyAura(data, effect);

        public void RemoveStatus(Spell spell) => _auras.RemoveStatus(spell);

        public Status FindStatus(Spell spell) => _auras.FindStatus(spell);

        public bool HasStatus(Spell spell) => _auras.HasStatus(spell);

        public void Dispell(DispellType dispellType) => _auras.Dispell(dispellType);

        public void Purge(DispellType dispellType) => _auras.Dispell(dispellType);

        public bool HasImmunity(Mechanic mechanic) => _auras.ImmunityValue(mechanic) > 0;
        #endregion

        public bool TryEquip(CombatGear item)
        {
            if (_equipment.CanEquip(item) == false)
            {
                return false;
            }

            _equipment.Equip(item);
            _stats.Add(item.Stats);

            return true;
        }

        public CombatGear Unequip(GearSlot slot)
        {
            CombatGear item = _equipment.Unequip(slot);

            if (item == null)
            {
                return null;
            }

            _stats.Subtract(item.Stats);
            return item;
        }

        public void InformAction(UnitAction action, EventData data)
        {
            _auras.InformAction(action, data);
        }

        public override string ToString()
        {
            return EntityId.ToString();
        }
    }
}
