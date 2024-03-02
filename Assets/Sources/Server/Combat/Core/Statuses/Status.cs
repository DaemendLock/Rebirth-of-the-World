using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine;
using Core.Combat.Statuses.AuraEffects;
using Core.Combat.Statuses.Auras;
using Core.Combat.Units;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
using Server.Combat.Core.Utils;
using System;
using System.Collections.Generic;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Combat.Statuses
{
    public class Status : Updatable
    {
        private readonly Aura _aura;

        private readonly Unit _parent;
        private readonly List<Unit> _casters;
        private readonly List<ModStat> _stats;

        private Duration _duration;

        private DamageAbsorption _absorption;

        internal Status(Unit parent, Aura aura, long duration)
        {
            _parent = parent;
            _aura = aura;

            _casters = new();
            _stats = new List<ModStat>();
            _duration = new Duration(duration);
            _absorption = default;

            aura.Apply(parent);
        }

        public int StackCount { get; private set; } = 1;

        public bool Expired => _duration.Expired;

        public float Absorption => _absorption.Capacity;

        public bool TryApplyCaster(Unit caster)
        {
            if (_casters.Contains(caster))
            {
                return false;
            }

            _casters.Add(caster);
            return true;
        }

        public RemoveAuraActionRecord Remove()
        {
            _duration = new Duration(0);
            return ClearEffects();
        }

        public float AbsorbDamage(HealthChangeEvent @event, float damageLeft)
        {
            if (_absorption.Capacity == 0)
            {
                return 0;
            }

            float result;

            @event.Absorb(_absorption.Capacity, _casters[0].Id);

            if (damageLeft > _absorption.Capacity)
            {
                if (_aura.Flags.HasFlag(AuraFlags.DontDestroyOnBreak) == false)
                {
                    Remove();
                }

                result = _absorption.Capacity;
                _absorption.Capacity = 0;
                return result;
            }

            _absorption.Capacity -= damageLeft;
            return damageLeft;
        }

        internal void AmplifyOutcomeDamage(HealthChangeEvent @event)
        {

        }

        public void RegisterStatModification(ModStat effect)
        {
            if (_stats.Contains(effect) == false)
            {
                _stats.Add(effect);
            }
        }

        public void RemoveStatModification(ModStat effect) => _stats.Remove(effect);

        public void GiveAbsorption(DamageAbsorption effect)
        {
            _absorption = effect;
        }

        public RemoveAuraActionRecord ClearEffects()
        {
            throw new NotImplementedException();
            //foreach (AuraEffect effect in _effects)
            //{
            //    effect.RemoveEffect(this);
            //}

            _stats.Clear();
            _absorption = new();

            return new RemoveAuraActionRecord();
        }

        public PercentModifiedValue EvaluateStat(UnitStat stat)
        {
            PercentModifiedValue result = new PercentModifiedValue();

            foreach (ModStat effect in _stats)
            {
                if (effect.Stat != stat)
                {
                    continue;
                }

                if (effect.IsPercent)
                {
                    result.Percent += effect.Evaluate(this);
                }
                else
                {
                    result.BaseValue += effect.Evaluate(this);
                }
            }

            return result;
        }

        public float GetEffectiveStat(UnitStat stat)
        {
            float result = 0;

            foreach (Unit caster in _casters)
            {

                float currentValue = StatsEvaluator.EvaluateUnitStatValue(stat, caster);

                if (currentValue > result)
                {
                    result = currentValue;
                }
            }

            return result;
        }

        public void Update(IActionRecordContainer container)
        {
            _aura.Update(container);
        }
    }
}
