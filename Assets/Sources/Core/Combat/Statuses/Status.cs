using Core.Combat.Abilities;
using Core.Combat.Statuses.AuraEffects;
using Core.Combat.Interfaces;
using Core.Combat.Units;
using Core.Combat.Units.Components;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
using System.Collections.Generic;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Combat.Statuses
{
    public class Status : DynamicStatOwner, Damageable
    {
        public readonly Unit Parent;
        public readonly Spell Spell;

        private readonly List<Unit> _casters;
        private readonly List<AuraEffect> _effects;
        private readonly List<ModStat> _stats;
        private readonly List<DynamicEffect>[] _dynamicEffects;

        private float _periodicEffectDelay;
        private Spell _periodicSpell;
        private Duration _duration;
        private Duration _periodicEffectCountdown;

        private DamageAbsorption _absorption;

        public Status(Unit parent, Spell spell)
        {
            Parent = parent;
            Spell = spell;

            _casters = new();
            _effects = new();
            _dynamicEffects = new List<DynamicEffect>[5];

            _stats = new List<ModStat>();

            if (spell == null)
            {
                _duration = new Duration(float.PositiveInfinity);
            }
            else
            {
                _duration = new Duration(spell.Duration);
            }

            _periodicEffectCountdown = new Duration(float.PositiveInfinity);
            _absorption = default;

            //TODO: Logger.Log($"{Parent} <- Status({Spell.Id}), {_duration.FullTime} sec.");
        }

        public int StackCount { get; private set; } = 1;

        public bool Expired => _duration.Expired;

        public float Absorption => _absorption.Capacity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effect">NOT an actual status passed to ApplyEffect, just copy</param>
        public void AddEffect(AuraEffect effect)
        {
            effect.ApplyEffect(this);
        }

        public bool TryApplyCaster(Unit caster)
        {
            if (_casters.Contains(caster))
            {
                return false;
            }

            _casters.Add(caster);
            return true;
        }

        public void Refresh(CastEventData data)
        {
            _duration += data.Spell.Duration - _duration.Left;
        }

        public void SetDuration(float duration)
        {
            _duration += duration - _duration.Left;
        }

        public void Remove()
        {
            ClearEffects();
            _duration = new Duration(0);
        }

        public void TakeDamage(DamageEvent @event)
        {
        }

        public float AbsorbDamage(DamageEvent @event, float damageLeft)
        {
            if (_absorption.Capacity == 0)
            {
                return 0;
            }

            float result;

            @event.Absorb(_absorption.Capacity, _casters[0].Id);

            if (damageLeft > _absorption.Capacity)
            {
                if (Spell.Flags.HasFlag(SpellFlags.DONT_DESTROY_ON_BREAK) == false)
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

        public void RegisterDynamicEffect(DynamicEffect effect, UnitAction action)
        {
            if (_dynamicEffects[(int) action] == null)
            {
                _dynamicEffects[(int) action] = new List<DynamicEffect>();
            }

            _dynamicEffects[(int) action].Add(effect);
        }

        public void RegisterStatModification(ModStat effect)
        {
            if (_stats.Contains(effect) == false)
            {
                _stats.Add(effect);
            }
        }

        public void RegisterPeriodicEffect(PeriodicallyTriggerSpell effect)
        {
            _periodicEffectDelay = effect.Period;
            _periodicEffectCountdown = new Duration(_periodicEffectDelay * UnitState.EvaluateHasteTimeDivider(GetEffectiveStat(UnitStat.HASTE)));
            _periodicSpell = Spell.Get(effect.Spell);
        }

        public void RemoveStatModification(ModStat effect) => _stats.Remove(effect);

        public void GiveAbsorption(DamageAbsorption effect)
        {
            _absorption = effect;
        }

        public void ClearEffects()
        {
            foreach (AuraEffect effect in _effects)
            {
                effect.RemoveEffect(this);
            }

            foreach (List<DynamicEffect> list in _dynamicEffects)
            {
                list?.Clear();
            }

            _stats.Clear();
            _absorption = new();
            _periodicEffectCountdown = new Duration(float.PositiveInfinity);

            //TODO Logger.Log($"Status({Spell.Id}) effects cleard: {Parent}");
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

        public void CallAction(UnitAction action, CastEventData data)
        {
            if (_dynamicEffects[(int) action] == null)
            {
                return;
            }

            foreach (DynamicEffect effect in _dynamicEffects[(int) action])
            {
                effect.Update(this, data);
            }
        }

        public float GetEffectiveStat(UnitStat stat)
        {
            float result = 0;

            foreach (Unit caster in _casters)
            {
                if (caster == Parent)
                {
                    continue;
                }

                float currentValue = caster.GetStat(stat);

                if (currentValue > result)
                {
                    result = currentValue;
                }
            }

            return result;
        }

        public void Update()
        {
            if (_periodicSpell == null)
            {
                return;
            }

            if (_periodicEffectCountdown.Expired)
            {
                _casters[0].CastSpell(new(_casters[0], Parent, _periodicSpell));

                _periodicEffectCountdown = new Duration(_periodicEffectDelay * UnitState.EvaluateHasteTimeDivider(GetEffectiveStat(UnitStat.HASTE)));
            }
        }
    }

    public enum UnitAction
    {
        TAKE_DAMAGE,
        DEAL_DAMAGE,
        RESOTORE_HEALTH,
        UPDATE_RESOURCES_VALUE,
        AUTOATTACK,
    }
}
