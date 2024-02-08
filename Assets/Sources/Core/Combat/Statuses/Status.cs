using Core.Combat.Abilities;
using Core.Combat.Statuses.AuraEffects;
using Core.Combat.Statuses.Auras;
using Core.Combat.Units;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
using System.Collections.Generic;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Combat.Statuses
{
    public class Status
    {
        public readonly Unit Parent;
        public readonly Aura Aura;

        private readonly List<Unit> _casters;
        private readonly List<AuraEffect> _effects;
        private readonly List<ModStat> _stats;
        private readonly List<DynamicEffect>[] _dynamicEffects;

        private float _periodicEffectDelay;
        private Spell _periodicSpell;
        private Duration _duration;
        private Duration _periodicEffectCountdown;

        private DamageAbsorption _absorption;

        internal Status(Unit parent, Aura aura)
        {
            Parent = parent;
            Aura = aura;

            _casters = new();
            _effects = new();
            _dynamicEffects = new List<DynamicEffect>[5];

            _stats = new List<ModStat>();

            _duration = new Duration(aura.Duration);

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

        public void Refresh(CastInputData data)
        {
            //_duration += data.Spell.Duration - _duration.Left;
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
                if (Aura.Flags.HasFlag(AuraFlags.DontDestroyOnBreak) == false)
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
            //TODO _periodicEffectCountdown = new Duration(_periodicEffectDelay * UnitState.EvaluateHasteTimeDivider(GetEffectiveStat(UnitStat.HASTE)));
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

        public void CallAction(UnitAction action, CastInputData data)
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
                //TODO: _casters[0].CastSpell(new(_casters[0], Parent, _periodicSpell));

                //TODO _periodicEffectCountdown = new Duration(_periodicEffectDelay * UnitState.EvaluateHasteTimeDivider(GetEffectiveStat(UnitStat.HASTE)));
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
