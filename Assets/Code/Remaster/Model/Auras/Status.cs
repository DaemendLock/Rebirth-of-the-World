using Remaster.AuraEffects;
using Remaster.Events;
using Remaster.Interfaces;
using Remaster.Stats;
using Remaster.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Remaster
{
    public class Status : DynamicStatOwner
    {
        public readonly Unit Parent;
        public readonly Spell Spell;

        private Duration _duration;

        private List<Unit> _casters;
        private List<AuraEffect> _effects;
        private List<ModStat> _stats;

        private int _periodicEffectId;
        private Duration _periodicEffectCountdown;

        private Dictionary<UnitAction, List<DynamicEffect>> _dynamicEffects;

        public Status(Unit parent, Spell spell)
        {
            _casters = new List<Unit>() { };
            _effects = new List<AuraEffect>();
            _dynamicEffects = new Dictionary<UnitAction, List<DynamicEffect>>();
            _stats = new List<ModStat>();

            Parent = parent;
            Spell = spell;
            _duration = new Duration(spell.Duration);

            Logger.Log($"{Parent} <- Status({Spell.Id}), {_duration.FullTime} sec.");
        }

        public int StackCount { get; private set; }

        public bool Expired => _duration.Expired;

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

        public void Refresh(EventData data)
        {
            _duration += data.Spell.Duration - _duration.Left;
        }

        public void SetDuration(float duration)
        {
            _duration += duration - _duration.Left;
        }

        public void Remove()
        {
            Parent.RemoveStatus(Spell);
        }

        public void RegisterDynamicEffect(DynamicEffect effect, UnitAction action)
        {
            if (_dynamicEffects.ContainsKey(action) == false)
            {
                _dynamicEffects[action] = new List<DynamicEffect>();
            }

            _dynamicEffects[action].Add(effect);
        }

        public void RegisterStatModification(ModStat effect)
        {
            if (_stats.Contains(effect) == false)
            {
                _stats.Add(effect);
            }
        }

        public void ClearEffects()
        {
            foreach (AuraEffect effect in _effects)
            {
                effect.RemoveEffect(this);
            }

            _dynamicEffects = null;
            _stats = null;

            Logger.Log($"Status({Spell.Id}) effects cleard: {Parent}");
        }

        public PercentModifiedValue EvaluateStat(UnitStat stat)
        {
            PercentModifiedValue result = new PercentModifiedValue();

            foreach (ModStat effect in _stats.Where((effect) => effect.Stat == stat))
            {
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

        public void CallAction(UnitAction action, EventData data)
        {
            if (_dynamicEffects.ContainsKey(action) == false)
            {
                return;
            }

            foreach (DynamicEffect effect in _dynamicEffects[action])
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
